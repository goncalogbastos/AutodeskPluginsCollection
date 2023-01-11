using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;


namespace AutoCAD_Plugins
{
    public class MultipleChamfer
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
                    // Ask the user to select a polyline 
                    PromptEntityOptions opt_1 = new PromptEntityOptions("\nSelect first polyline: ");
                    opt_1.SetRejectMessage("\nObject must be a polyline.");
                    opt_1.AddAllowedClass(typeof(Polyline), true);
                    PromptEntityResult res_1 = ed.GetEntity(opt_1);
                    Polyline pl_1 = tr.GetObject(res_1.ObjectId, OpenMode.ForRead) as Polyline;

                    if (res_1.Status != PromptStatus.OK)
                    {
                        return;
                    }

                    // Ask the user to select a polyline 
                    PromptEntityOptions opt_2 = new PromptEntityOptions("\nSelect second polyline: ");
                    opt_2.SetRejectMessage("\nObject must be a polyline.");
                    opt_2.AddAllowedClass(typeof(Polyline), true);
                    PromptEntityResult res_2 = ed.GetEntity(opt_1);
                    Polyline pl_2 = tr.GetObject(res_2.ObjectId, OpenMode.ForRead) as Polyline;

                    if (res_2.Status != PromptStatus.OK)
                    {
                        return;
                    }

                    var lay_1 = pl_1.LayerId.ToString();
                    var lay_2 = pl_2.LayerId.ToString();

                    if (lay_1 != lay_2)
                    {
                        ed.WriteMessage("Objects are not on the same layer.");
                        return;
                    }


                    //****************
                    // Review. Try to insert plineID's in sendstringtoexecute
                    doc.SendStringToExecute($"._chamfer a 25 45 ", true, false, false);



                }
                catch (System.Exception ex)
                {
                    ed.WriteMessage($"Error found:{ex.Message}");
                }
            }
        }
    }
}