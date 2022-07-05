using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;

namespace AutoCAD_Plugins
{
    public class Current_View_Front
    {
        public void Create()
        {
            var doc = Application.DocumentManager.MdiActiveDocument;
            var db = doc.Database;
            var ed = Application.DocumentManager.MdiActiveDocument.Editor;
            dynamic acadApp = Application.AcadApplication;

            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                try
                {
                    //var vDir = "Top";

                    Vector3d viewDir = new Vector3d();
                    viewDir = Vector3d.YAxis;

                    using (ViewTableRecord view = ed.GetCurrentView())
                    {
                        view.ViewDirection = viewDir;
                        ed.SetCurrentView(view);
                        acadApp.ZoomExtents();
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
