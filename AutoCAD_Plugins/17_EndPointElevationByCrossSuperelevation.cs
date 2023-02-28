using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using System;

namespace AutoCAD_Plugins
{
    public class EndPointElevationByCrossSuperelevation
    {
        public void Create()
        {
            var doc = Application.DocumentManager.MdiActiveDocument;
            var db = doc.Database;
            var ed = Application.DocumentManager.MdiActiveDocument.Editor;

            try
            {
                // Ask the user to select a polyline 
                PromptEntityOptions opt = new PromptEntityOptions("\nSelect a polyline: ");
                opt.SetRejectMessage("\nObject must be a polyline.");
                opt.AddAllowedClass(typeof(Polyline), true);
                PromptEntityResult res = ed.GetEntity(opt);


                if (res.Status != PromptStatus.OK)
                {
                    return;
                }

                using (Transaction tr = db.TransactionManager.StartTransaction())
                {
                    Polyline pl = tr.GetObject(res.ObjectId, OpenMode.ForRead) as Polyline;
                    Double pl_length = pl.Length;

                    PromptDoubleOptions pdo_start = new PromptDoubleOptions("\nInitial elevation: ");
                    PromptDoubleResult start = ed.GetDouble(pdo_start);

                    PromptDoubleOptions pdo_cs = new PromptDoubleOptions("\nCross superelevation: ");
                    PromptDoubleResult slope = ed.GetDouble(pdo_cs);

                    Double S = start.Value;
                    Double Slope = slope.Value;
                    Double E = Math.Round(S + Slope * pl_length, 3);

                    // Open the Block table for read
                    BlockTable block_table;
                    block_table = tr.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;

                    // Open the Block table record Model space for write
                    BlockTableRecord block_table_record;
                    block_table_record = tr.GetObject(block_table[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;

                    // Create a multiline text object
                    MText inc_label = new MText();
                    inc_label.SetDatabaseDefaults();
                    inc_label.Location = pl.EndPoint;
                    inc_label.TextHeight = 0.2;
                    inc_label.Height = 5;
                    inc_label.Contents = $"{Math.Round(E, 3)}";
                    block_table_record.AppendEntity(inc_label);
                    tr.AddNewlyCreatedDBObject(inc_label, true);

                    // Write slope in editor
                    ed.WriteMessage($"\nSlope: {Math.Round(E, 3)}");
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

