﻿using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;

namespace AutoCAD_Plugins
{
    public class PolylineLength
    {
        public void Create()
        {
            var running = true;
            while (running)
            {
                var doc = Application.DocumentManager.MdiActiveDocument;
                var db = doc.Database;
                var ed = doc.Editor;

                try
                {
                    // Ask the user to select a polyline 
                    PromptEntityOptions opt = new PromptEntityOptions("\nSelect a polyline: ");
                    opt.SetRejectMessage("\nObject must be a polyline.");
                    opt.AddAllowedClass(typeof(Polyline), true);
                    opt.AddAllowedClass(typeof(Polyline), true);
                    PromptEntityResult res = ed.GetEntity(opt);

                    if (res.Status != PromptStatus.OK)
                    {
                        running = false;
                        ed.Regen();
                        return;
                    }

                    // Prompt the user for the alignment name
                    PromptStringOptions pso = new PromptStringOptions("\nEnter name: ");
                    pso.AllowSpaces = true;
                    PromptResult pr = ed.GetString(pso);
                    string alignmentName = pr.StringResult;

                    // Create a field with area of the polyline -> <NAME> + <FIELD> + <m>
                    string strObjId = res.ObjectId.ToString();
                    strObjId = strObjId.Replace("(", "");
                    strObjId = strObjId.Replace(")", "");
                    string field = alignmentName + "\n"
                        + @"%<\AcObjProp Object(%<\_ObjId "
                        + strObjId + @">%).Length \f "
                        + "\"%lu6\""
                        + @">%"
                        + " m";
                        //+ @"\U+00B2";

                    // Prompt the user for the insertation point
                    PromptPointOptions pPtOpts = new PromptPointOptions("");
                    pPtOpts.Message = "\nEnter the insertation point: ";

                    // Convert insertation point to 3D point
                    PromptPointResult pPtRes = ed.GetPoint(pPtOpts);
                    var insPt = pPtRes.Value;

                    using (Transaction tr = db.TransactionManager.StartTransaction())
                    {
                        // Open the Block table for read
                        BlockTable block_table;
                        block_table = tr.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;

                        // Open the Block table record Model space for write
                        BlockTableRecord block_table_record;
                        block_table_record = tr.GetObject(block_table[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;

                        // Create a multiline text object
                        MText label = new MText();
                        label.SetDatabaseDefaults();
                        label.Location = insPt;
                        label.Attachment = AttachmentPoint.MiddleCenter;
                        label.TextHeight = 0.2;
                        label.Contents = field.ToString();
                        block_table_record.AppendEntity(label);
                        tr.AddNewlyCreatedDBObject(label, true);

                        tr.Commit();
                    }
                }
                catch (Autodesk.AutoCAD.Runtime.Exception ex)
                {
                    ed.WriteMessage("\nException message :" + ex.Message);
                }
            }
        }
    }
}
