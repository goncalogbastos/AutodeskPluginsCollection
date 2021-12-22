using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Colors;

// Credits: https://www.keanw.com/2010/01/creating-an-autocad-block-using-net.html

namespace AutoCAD_Plugins
{
    public class New_Block

    {
        public void Create()
        {
            var doc = Application.DocumentManager.MdiActiveDocument;
            var db = doc.Database;
            var ed = doc.Editor;
            var tr = db.TransactionManager.StartTransaction();

            using (tr)
            {
                var bt = (BlockTable)tr.GetObject(db.BlockTableId, OpenMode.ForRead);

                // Prompt the user for the block name
                //PromptStringOptions pso = new PromptStringOptions("\nEnter new block name: ");
                //pso.AllowSpaces = true;

                // A variable for the block's name
                var blkName = "coordinates";

                /*
                do
                {
                    PromptResult pr = ed.GetString(pso);

                    // Return if the user cancelled (will abort the transaction as we drop out of the using statement's scope)
                    if (pr.Status != PromptStatus.OK)
                        return;

                    try
                    {
                        // Validate the provided symbol table name
                        SymbolUtilityServices.ValidateSymbolName(pr.StringResult, false);

                        // Only set the block name if it isn't in use
                        if (bt.Has(pr.StringResult))
                            ed.WriteMessage("\nA block with this name already exists.");
                        else
                            blkName = pr.StringResult;
                    }

                    catch
                    {
                        // An exception has been thrown, indicating the name is invalid
                        ed.WriteMessage("\nInvalid block name.");
                    }
                }
                */

                //while (blkName == "") ;

                // Create our new block table record and its properties
                BlockTableRecord btr = new BlockTableRecord();
                btr.Name = blkName;

                bt.UpgradeOpen();
                ObjectId btrId = bt.Add(btr);
                tr.AddNewlyCreatedDBObject(btr, true);

                // Function to create a block
                DBObjectCollection ents = SquareOfLines(5);

                foreach (Entity ent in ents)
                {
                    btr.AppendEntity(ent);
                    tr.AddNewlyCreatedDBObject(ent, true);
                }





                // Insertation point
                PromptPointOptions pPtOpts = new PromptPointOptions("");
                pPtOpts.Message = "\nEnter the insertation point: ";

                // Convert insertation point to 3D point
                PromptPointResult pPtRes = doc.Editor.GetPoint(pPtOpts);
                Point3d insPt = pPtRes.Value;


                // Add a block reference to the model space
                BlockTableRecord ms = (BlockTableRecord)tr.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite);

                BlockReference br = new BlockReference(insPt, btrId);

                ms.AppendEntity(br);

                tr.AddNewlyCreatedDBObject(br, true);

                // Commit the transaction
                tr.Commit();

                // Report what we've done
                //ed.WriteMessage($"\nCreated block named \"{blkName}\" containing {ents.Count} entities.");

            }

        }


        private DBObjectCollection SquareOfLines(double size)
        {

            // A function to generate a set of entities for our block
            DBObjectCollection ents = new DBObjectCollection();




            Point3d[] pts =

                { new Point3d(-size, -size, 0),

                new Point3d(size, -size, 0),

                new Point3d(size, size, 0),

                new Point3d(-size, size, 0)

              };

            int max = pts.GetUpperBound(0);



            for (int i = 0; i <= max; i++)

            {

                int j = (i == max ? 0 : i + 1);

                Line ln = new Line(pts[i], pts[j]);

                ents.Add(ln);

            }

            return ents;

        }
    }

}































/*
// Prompt the user for the inseration point
PromptPointOptions pPtOpts = new PromptPointOptions("");
pPtOpts.Message = "\nEnter the insertation point: ";

// Convert inseration point to 3D point
PromptPointResult pPtRes = doc.Editor.GetPoint(pPtOpts);
Point3d insPt = pPtRes.Value;

using (var br = new BlockReference(insPt, bt[blockName]))
{
    var space = (BlockTableRecord)tr.GetObject(db.CurrentSpaceId, OpenMode.ForWrite);
    space.AppendEntity(br);
    tr.AddNewlyCreatedDBObject(br, true);
}
tr.Commit();
}

{
// check if the block table already has the 'blockName'" block
var bt = (BlockTable)tr.GetObject(db.BlockTableId, OpenMode.ForRead);
if (!bt.Has(blockName))
{
    try
    {
        // search for a dwg file named 'blockName' in AutoCAD search paths
        var filename = HostApplicationServices.Current.FindFile(blockName + ".dwg", db, FindFileHint.Default);
        // add the dwg model space as 'blockName' block definition in the current database block table
        using (var sourceDb = new Database(false, true))
        {
            sourceDb.ReadDwgFile(filename, FileOpenMode.OpenForReadAndAllShare, true, "");
            db.Insert(blockName, sourceDb, true);
        }
    }
    catch
    {
        ed.WriteMessage($"\nBlock '{blockName}' not found.");
        return;
    }
}
*/

// create a new block reference