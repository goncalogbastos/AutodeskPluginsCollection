using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using System;

namespace AutoCAD_Plugins
{
    public class PickMPZPoint
    {
        public void Create()
        {

            var doc = Application.DocumentManager.MdiActiveDocument;
            var db = doc.Database;
            var ed = doc.Editor;

            try
            {
                // Prompt the user to pick the object point
                PromptPointOptions pPtOpts = new PromptPointOptions("");
                pPtOpts.Message = "\nEnter the object point: ";
                PromptPointResult res = ed.GetPoint(pPtOpts);

                if (res.Status != PromptStatus.OK)
                {
                    return;
                }

                // Convert insertation point to 3D point
                var insPt = res.Value;
                var PtX = Math.Round(insPt.X,4);
                var PtY = Math.Round(insPt.Y,4);
                var PtZ = Math.Round(insPt.Z,3);

                Vector3d vec = new Vector3d(5, 5, 0);
                Point3d insPt_ = insPt.Add(vec);

                // Prompt the user for the object name
                PromptStringOptions name = new PromptStringOptions("\nEnter the object name: ");
                name.AllowSpaces = true;
                PromptResult objName = ed.GetString(name);
                string OBJNAME = objName.StringResult;

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
                    label.Location = insPt_;
                    label.Attachment = AttachmentPoint.MiddleCenter;
                    label.TextHeight = 1;
                    label.Contents = OBJNAME + "\n" + PtX.ToString() + "\n" + PtY.ToString() + "\n" + PtZ.ToString();
                    block_table_record.AppendEntity(label);
                    tr.AddNewlyCreatedDBObject(label, true);

                    tr.Commit();
                    ed.Regen();
                }
            }
            catch (Autodesk.AutoCAD.Runtime.Exception ex)
            {
                ed.WriteMessage("\nException message :" + ex.Message);
            }
        }
    }
}
