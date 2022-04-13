using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.Civil.ApplicationServices;
using Autodesk.Civil.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;

namespace Civil3D_Plugins
{
    public class Offset_Alignment
    {
        public void Create()
        {
            var civil_doc = CivilApplication.ActiveDocument;
            var ed = Application.DocumentManager.MdiActiveDocument.Editor;
            var db = Application.DocumentManager.MdiActiveDocument.Database;

            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                try
                {
                    // Ask the user to select an alignment
                    PromptEntityOptions opt = new PromptEntityOptions("\nSelect an alignment:");
                    opt.SetRejectMessage("\nObject must be an alignment.");
                    opt.AddAllowedClass(typeof(Autodesk.Civil.DatabaseServices.Alignment), true);
                    PromptEntityResult res = ed.GetEntity(opt);
                    if (res.Status != PromptStatus.OK) return;
                    ObjectId alignmentId = res.ObjectId;


                    Alignment myAlignment = tr.GetObject(alignmentId, OpenMode.ForWrite) as Alignment;

                    //var start_ = myAlignment.StartingStation;
                    //var final_ = myAlignment.EndingStation;

                    PromptDoubleOptions pdo__ = new PromptDoubleOptions("\nLeft [0] or Right [1]: ");
                    PromptDoubleResult lr = ed.GetDouble(pdo__);

                    PromptDoubleOptions pdo = new PromptDoubleOptions("\nEnter offset start station: ");
                    PromptDoubleResult os_ = ed.GetDouble(pdo);

                    PromptDoubleOptions pdo_ = new PromptDoubleOptions("\nEnter offset final station: ");
                    PromptDoubleResult of_ = ed.GetDouble(pdo_);
                    

                    if (lr.Value == 0)
                    {
                        ObjectId alignObjId = Alignment.CreateOffsetAlignment("<[Parent Alignment Name(CP)]>-<[Side]>-<[Offset Distance]>", alignmentId, -10, myAlignment.StyleId, os_.Value, of_.Value);
                    }
                    else
                    {
                        ObjectId alignObjId = Alignment.CreateOffsetAlignment("<[Parent Alignment Name(CP)]>-<[Side]>-<[Offset Distance]>", alignmentId, 10, myAlignment.StyleId, os_.Value, of_.Value);
                    }
                    

                    tr.Commit();
                    ed.Regen();

                }

                catch (Autodesk.AutoCAD.Runtime.Exception ex)
                {
                    ed.WriteMessage("/n Exception message :" + ex.Message);
                }
            }
        }
    }
}