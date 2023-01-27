using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.Civil.ApplicationServices;

namespace Civil3D_Plugins
{
    public class EventViewer
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
                    var event_viewer = civil_doc.Settings.DrawingSettings.AmbientSettings.General.ShowEventViewer;
                    if(event_viewer.Value == true)
                    {
                        event_viewer.Value = false;
                        ed.WriteMessage("Event viewer was turned off.");
                    }
                    else
                    {
                        event_viewer.Value = true;
                        ed.WriteMessage("Event viewer was turned on.");
                    }

                    
                    tr.Commit();
                }

                catch (Autodesk.AutoCAD.Runtime.Exception ex)
                {
                    ed.WriteMessage("/n Exception message :" + ex.Message);
                }
            }
        }
    }
}