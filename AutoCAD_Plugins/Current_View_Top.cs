using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;

namespace AutoCAD_Plugins
{
    public class Current_View_Top
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
                    //var vDir = "Top";

                    Vector3d viewDir = new Vector3d();
                    viewDir = Vector3d.ZAxis;

                    using (ViewTableRecord view = ed.GetCurrentView())
                    {
                        view.ViewDirection = viewDir;
                        ed.SetCurrentView(view);
                    }
                    tr.Commit();
                }
                catch (Autodesk.AutoCAD.Runtime.Exception ex)
                {
                    ed.WriteMessage("\nException message :" + ex.Message);
                }
            }
        }
    }
}
