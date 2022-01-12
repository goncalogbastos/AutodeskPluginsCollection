using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using System;
using Autodesk.AutoCAD.Runtime;

namespace AutoCAD_Plugins
{
    public class Get_Slope
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
                    // Prompt the user for normal or superelevated slope
                    PromptKeywordOptions pKeyOpts = new PromptKeywordOptions("\nEnter an option: ");
                    pKeyOpts.Keywords.Add("N"); // Normal %
                    pKeyOpts.Keywords.Add("SE"); // Superelevation %
                    pKeyOpts.Keywords.Default = "N";
                    pKeyOpts.AllowNone = true;
                    PromptResult pKeyRes = ed.GetKeywords(pKeyOpts);

                    // Ask the user to select a polyline 
                    PromptEntityOptions opt = new PromptEntityOptions("\nSelect a polyline: ");
                    opt.SetRejectMessage("\nObject must be a polyline.");
                    opt.AddAllowedClass(typeof(Polyline), true);
                    PromptEntityResult res = ed.GetEntity(opt);

                    // Get the start and end point of polyline selected
                    Polyline pl = tr.GetObject(res.ObjectId, OpenMode.ForRead) as Polyline;
                    Point3d start_point = pl.StartPoint;
                    Point3d end_point = pl.EndPoint;

                    var kw = pKeyRes.StringResult;
                    if (kw == "N")
                    {
                        // Calculate slope
                        var delta_x = Math.Abs(start_point.X - end_point.X);
                        var delta_y = Math.Abs(start_point.Y - end_point.Y);
                        var slope = 100 * (delta_y / delta_x);

                        // Open the Block table for read
                        BlockTable block_table;
                        block_table = tr.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;

                        // Open the Block table record Model space for write
                        BlockTableRecord block_table_record;
                        block_table_record = tr.GetObject(block_table[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;

                        // Create a multiline text object
                        MText inc_label = new MText();
                        inc_label.SetDatabaseDefaults();
                        inc_label.Location = end_point;
                        inc_label.Height = 5;
                        inc_label.Contents = $"{Math.Round(slope, 4)}%";
                        block_table_record.AppendEntity(inc_label);
                        tr.AddNewlyCreatedDBObject(inc_label, true);

                        // Write slope in editor
                        ed.WriteMessage($"\nSlope: {Math.Round(slope, 2)}%");
                        tr.Commit();
                    }
                    else if (kw == "SE")
                    {
                        // Calculate slope
                        var delta_x = Math.Abs(start_point.X - end_point.X);
                        var delta_y = Math.Abs(start_point.Y - end_point.Y)/10;
                        var slope = 100 * (delta_y / delta_x);

                        // Open the Block table for read
                        BlockTable block_table;
                        block_table = tr.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;

                        // Open the Block table record Model space for write
                        BlockTableRecord block_table_record;
                        block_table_record = tr.GetObject(block_table[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;

                        // Create a multiline text object
                        MText inc_label = new MText();
                        inc_label.SetDatabaseDefaults();
                        inc_label.Location = end_point;
                        inc_label.Height = 5;
                        inc_label.Contents = $"{Math.Round(slope, 4)}%";
                        block_table_record.AppendEntity(inc_label);
                        tr.AddNewlyCreatedDBObject(inc_label, true);

                        // Write slope in editor
                        ed.WriteMessage($"\nSlope{Math.Round(slope, 4)}%");
                        tr.Commit();
                    }
                    else
                    {
                        return;
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