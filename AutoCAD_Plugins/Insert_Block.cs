using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Colors;


// Credits: https://adndevblog.typepad.com/autocad/2012/05/insert-block-from-a-different-dwg-using-net-.html

namespace AutoCAD_Plugins
{
    public class Insert_Block
    {
        public void Create()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;

            using (Database OpenDb = new Database(false, true))
            {

                OpenDb.ReadDwgFile("D:\\COBA\\CODING\\C#\\Coordinates.dwg", System.IO.FileShare.ReadWrite, true, "");

                ObjectIdCollection ids = new ObjectIdCollection();

                using (Transaction tr =

                        OpenDb.TransactionManager.StartTransaction())

                {

                    //For example, Get the block by name "TEST"

                    BlockTable bt;

                    bt = (BlockTable)tr.GetObject(OpenDb.BlockTableId

                                                   , OpenMode.ForRead);



                    if (bt.Has("coordinates"))

                    {

                        ids.Add(bt["coordinates"]);

                    }

                    tr.Commit();

                }



                //if found, add the block

                if (ids.Count != 0)

                {

                    //get the current drawing database

                    Database destdb = doc.Database;



                    IdMapping iMap = new IdMapping();

                    destdb.WblockCloneObjects(ids, destdb.BlockTableId

                           , iMap, DuplicateRecordCloning.Ignore, false);

                }

            }

        }
    }
}