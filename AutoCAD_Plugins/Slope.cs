using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;


namespace AutoCAD_Plugins
{
    public class Slope
    {
        public void Create()
        {
            var doc = Application.DocumentManager.MdiActiveDocument;
            var db = doc.Database;
            var ed = doc.Editor;

            using (var tr = db.TransactionManager.StartTransaction())
            {
                try
                {
                    BlockTable bt;
                    BlockTableRecord btr;
                    bt = tr.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                    btr = tr.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;

                    // Prompt the user for command keyword
                    PromptKeywordOptions pKeyOpts = new PromptKeywordOptions("\nEnter an option");                     
                    pKeyOpts.Keywords.Add("N"); // Normal h:v
                    pKeyOpts.Keywords.Add("P"); // Normal %
                    pKeyOpts.Keywords.Add("SE"); // Superelevation %
                    pKeyOpts.Keywords.Default = "N";
                    //pKeyOpts.AllowNone = true;                    
                    PromptResult pKeyRes = ed.GetKeywords(pKeyOpts);                    

                    // Prompt the user for the slope
                    PromptDoubleOptions pdo = new PromptDoubleOptions("\nEnter slope: ");
                    PromptDoubleResult sl_ = ed.GetDouble(pdo);
                    
                    // Exit if cancelled
                    if (sl_.Status != PromptStatus.OK)
                        return;                    

                    /* Prompt the user for the orientation. Valid options:                  
                    |   2   |   1   |
                    |   3   |   4   |                    
                    */
                    int orient = -1;
                    while (orient <= 0 || orient >= 5)
                    {                        
                        PromptIntegerOptions or = new PromptIntegerOptions("\nEnter orientation: ");
                        PromptIntegerResult or_ = ed.GetInteger(or);
                        // Exit if cancelled
                        if (or_.Status != PromptStatus.OK)
                            return;
                        orient = or_.Value;
                        if(orient <= 0 || orient >= 5)
                        {
                            ed.WriteMessage("\nOrientation not valid. Valid options: [1], [2], [3], [4]");   
                        }
                    }
                    
                    // Prompt the user for the insertation point
                    PromptPointOptions pPtOpts = new PromptPointOptions("");
                    pPtOpts.Message = "\nEnter the insertation point: ";

                    // Convert insertation point to 3D point
                    PromptPointResult pPtRes = doc.Editor.GetPoint(pPtOpts);
                    var insPt = pPtRes.Value;


                    var kw = pKeyRes.StringResult;
                    if (kw == "N")
                    {
                        if (orient == 1)
                        {
                            Polyline pl = new Polyline();
                            pl.AddVertexAt(0, new Point2d(insPt.X, insPt.Y), 0, 0, 0);
                            pl.AddVertexAt(1, new Point2d(insPt.X + sl_.Value * 100, insPt.Y + 100), 0, 0, 0);
                            // Update database and commit transaction                        
                            pl.SetDatabaseDefaults();
                            btr.AppendEntity(pl);
                            tr.AddNewlyCreatedDBObject(pl, true);
                            tr.Commit();
                        }
                        else if (orient == 2)
                        {
                            Polyline pl = new Polyline();
                            pl.AddVertexAt(0, new Point2d(insPt.X, insPt.Y), 0, 0, 0);
                            pl.AddVertexAt(1, new Point2d(insPt.X - sl_.Value * 100, insPt.Y + 100), 0, 0, 0);
                            // Update database and commit transaction
                            pl.SetDatabaseDefaults();
                            btr.AppendEntity(pl);
                            tr.AddNewlyCreatedDBObject(pl, true);
                            tr.Commit();
                        }
                        else if (orient == 3)
                        {
                            Polyline pl = new Polyline();
                            pl.AddVertexAt(0, new Point2d(insPt.X, insPt.Y), 0, 0, 0);
                            pl.AddVertexAt(1, new Point2d(insPt.X - sl_.Value * 100, insPt.Y - 100), 0, 0, 0);
                            // Update database and commit transaction
                            pl.SetDatabaseDefaults();
                            btr.AppendEntity(pl);
                            tr.AddNewlyCreatedDBObject(pl, true);
                            tr.Commit();
                        }
                        else
                        {
                            Polyline pl = new Polyline();
                            pl.AddVertexAt(0, new Point2d(insPt.X, insPt.Y), 0, 0, 0);
                            pl.AddVertexAt(1, new Point2d(insPt.X + sl_.Value * 100, insPt.Y - 100), 0, 0, 0);
                            // Update database and commit transaction
                            pl.SetDatabaseDefaults();
                            btr.AppendEntity(pl);
                            tr.AddNewlyCreatedDBObject(pl, true);
                            tr.Commit();
                        }
                    }
                    else if(kw == "P")
                    {
                        if (orient == 1)
                        {
                            Polyline pl = new Polyline();
                            pl.AddVertexAt(0, new Point2d(insPt.X, insPt.Y), 0, 0, 0);
                            pl.AddVertexAt(1, new Point2d(insPt.X + 100, insPt.Y + sl_.Value), 0, 0, 0);
                            // Update database and commit transaction
                            pl.SetDatabaseDefaults();
                            btr.AppendEntity(pl);
                            tr.AddNewlyCreatedDBObject(pl, true);
                            tr.Commit();
                        }
                        else if (orient == 2)
                        {
                            Polyline pl = new Polyline();
                            pl.AddVertexAt(0, new Point2d(insPt.X, insPt.Y), 0, 0, 0);
                            pl.AddVertexAt(1, new Point2d(insPt.X - 100, insPt.Y + sl_.Value), 0, 0, 0);
                            // Update database and commit transaction
                            pl.SetDatabaseDefaults();
                            btr.AppendEntity(pl);
                            tr.AddNewlyCreatedDBObject(pl, true);
                            tr.Commit();
                        }
                        else if (orient == 3)
                        {
                            Polyline pl = new Polyline();
                            pl.AddVertexAt(0, new Point2d(insPt.X, insPt.Y), 0, 0, 0);
                            pl.AddVertexAt(1, new Point2d(insPt.X - 100, insPt.Y - sl_.Value), 0, 0, 0);
                            // Update database and commit transaction
                            pl.SetDatabaseDefaults();
                            btr.AppendEntity(pl);
                            tr.AddNewlyCreatedDBObject(pl, true);
                            tr.Commit();
                        }
                        else
                        {
                            Polyline pl = new Polyline();
                            pl.AddVertexAt(0, new Point2d(insPt.X, insPt.Y), 0, 0, 0);
                            pl.AddVertexAt(1, new Point2d(insPt.X + 100, insPt.Y - sl_.Value), 0, 0, 0);
                            // Update database and commit transaction
                            pl.SetDatabaseDefaults();
                            btr.AppendEntity(pl);
                            tr.AddNewlyCreatedDBObject(pl, true);
                            tr.Commit();
                        }
                    }
                    else if(kw == "SE")
                    {
                        if (orient == 1)
                        {
                            Polyline pl = new Polyline();
                            pl.AddVertexAt(0, new Point2d(insPt.X, insPt.Y), 0, 0, 0);
                            pl.AddVertexAt(1, new Point2d(insPt.X + 100, insPt.Y + sl_.Value * 10), 0, 0, 0);
                            // Update database and commit transaction
                            pl.SetDatabaseDefaults();
                            btr.AppendEntity(pl);
                            tr.AddNewlyCreatedDBObject(pl, true);
                            tr.Commit();
                        }
                        else if (orient == 2)
                        {
                            Polyline pl = new Polyline();
                            pl.AddVertexAt(0, new Point2d(insPt.X, insPt.Y), 0, 0, 0);
                            pl.AddVertexAt(1, new Point2d(insPt.X - 100, insPt.Y + sl_.Value * 10), 0, 0, 0);
                            // Update database and commit transaction
                            pl.SetDatabaseDefaults();
                            btr.AppendEntity(pl);
                            tr.AddNewlyCreatedDBObject(pl, true);
                            tr.Commit();
                        }
                        else if (orient == 3)
                        {
                            Polyline pl = new Polyline();
                            pl.AddVertexAt(0, new Point2d(insPt.X, insPt.Y), 0, 0, 0);
                            pl.AddVertexAt(1, new Point2d(insPt.X - 100, insPt.Y - sl_.Value * 10), 0, 0, 0);
                            // Update database and commit transaction
                            pl.SetDatabaseDefaults();
                            btr.AppendEntity(pl);
                            tr.AddNewlyCreatedDBObject(pl, true);
                            tr.Commit();
                        }
                        else
                        {
                            Polyline pl = new Polyline();
                            pl.AddVertexAt(0, new Point2d(insPt.X, insPt.Y), 0, 0, 0);
                            pl.AddVertexAt(1, new Point2d(insPt.X + 100, insPt.Y - sl_.Value * 10), 0, 0, 0);
                            // Update database and commit transaction
                            pl.SetDatabaseDefaults();
                            btr.AppendEntity(pl);
                            tr.AddNewlyCreatedDBObject(pl, true);
                            tr.Commit();
                        }
                    }
                    else
                    {
                        return;
                    }
                }
                catch (System.Exception ex)
                {
                    ed.WriteMessage($"Error found:{ex.Message}");
                }
            }
        }
    }
}