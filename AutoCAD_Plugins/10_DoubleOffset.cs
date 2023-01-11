using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;

namespace AutoCAD_Plugins
{
    public class DoubleOffset
    {
        public void Create()
        {
            var doc = Application.DocumentManager.MdiActiveDocument;
            var db = doc.Database;
            var ed = Application.DocumentManager.MdiActiveDocument.Editor;

            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                try
                {
                    // Prompt the user for the offset
                    PromptDoubleOptions pdo = new PromptDoubleOptions("\nEnter offset: ");
                    PromptDoubleResult offset_ = ed.GetDouble(pdo);

                    var running = true;
                    while (running)
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
                        
                        // Get the polyline selected
                        Polyline pl = tr.GetObject(res.ObjectId, OpenMode.ForRead) as Polyline;
                        ed.WriteMessage(pl.ObjectId.ToString());

                        // Open the Block table for read
                        BlockTable block_table;
                        block_table = tr.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;

                        // Open the Block table record Model space for write
                        BlockTableRecord block_table_record;
                        block_table_record = tr.GetObject(block_table[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;

                        // Offset the polyline a given distance
                        DBObjectCollection pline_offset_plus = pl.GetOffsetCurves(offset_.Value);
                        DBObjectCollection pline_offset_minus = pl.GetOffsetCurves(-1 * offset_.Value);

                        // Step through the new objects created
                        foreach (Entity ent in pline_offset_plus)
                        {
                            // Add each offset object
                            block_table_record.AppendEntity(ent);
                            tr.AddNewlyCreatedDBObject(ent, true);
                        }

                        foreach (Entity ent in pline_offset_minus)
                        {
                            // Add each offset object
                            block_table_record.AppendEntity(ent);
                            tr.AddNewlyCreatedDBObject(ent, true);
                        }
                        
                        
                    }
                   
                }
                catch (Autodesk.AutoCAD.Runtime.Exception ex)
                {
                    ed.WriteMessage("\nException message :" + ex.Message);
                }

                tr.Commit();
            }
        }
    }
}
