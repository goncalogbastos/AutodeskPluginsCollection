using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;

namespace AutoCAD_Plugins
{
    public class Area_Pline
    {
        public void Create()
        {
            var doc = Application.DocumentManager.MdiActiveDocument;
            var db = doc.Database;
            var ed = doc.Editor;
            var running = true;

            // Prompt the user for the text height
            PromptDoubleOptions pdo = new PromptDoubleOptions("\nEnter text height: ");
            pdo.DefaultValue = 25;
            PromptDoubleResult th_ = ed.GetDouble(pdo);

            while (running)
            {
                using (Transaction tr = db.TransactionManager.StartTransaction())
                {
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
                            return;
                        }

                        // Create a field with area of the polyline -> <NAME> + <FIELD> + <m2>
                        string strObjId = res.ObjectId.ToString();
                        strObjId = strObjId.Replace("(", "");
                        strObjId = strObjId.Replace(")", "");
                        string field = @"%<\AcObjProp.16.2 Object(%<\_ObjId "
                            + strObjId + @">%).Area \f "
                            + "\"%lu6%qf1\""
                            + @">%"
                            + " m"
                            + @"\U+00B2";

                        // Prompt the user for the insertation point
                        PromptPointOptions pPtOpts = new PromptPointOptions("");
                        pPtOpts.Message = "\nEnter the insertation point: ";

                        // Convert insertation point to 3D point
                        PromptPointResult pPtRes = ed.GetPoint(pPtOpts);
                        var insPt = pPtRes.Value;

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
                        label.TextHeight = th_.Value;
                        label.Contents = field.ToString();
                        block_table_record.AppendEntity(label);
                        tr.AddNewlyCreatedDBObject(label, true);

                        tr.Commit();
                        ed.Regen();
                    }
                    catch (Autodesk.AutoCAD.Runtime.Exception ex)
                    {
                        ed.WriteMessage("\nException message :" + ex.Message);
                    }
                }
            }
        }
    }
}