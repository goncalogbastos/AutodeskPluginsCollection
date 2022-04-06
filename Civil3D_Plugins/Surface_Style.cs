using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.Civil.ApplicationServices;
using Autodesk.Civil.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;

namespace Civil3D_Plugins
{
    public class Surface_Style
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
                    // Ask the user to select a surface
                    PromptEntityOptions peo = new PromptEntityOptions("\nSelect a surface:");
                    peo.SetRejectMessage("\nObject must be a surface.");
                    peo.AddAllowedClass(typeof(Autodesk.Civil.DatabaseServices.TinSurface), true);
                    PromptEntityResult result = ed.GetEntity(peo);
                    if (result.Status != PromptStatus.OK) return;
                    ObjectId surfaceid = result.ObjectId;
                    
                    TinSurface surface = tr.GetObject(surfaceid, OpenMode.ForRead) as TinSurface;

                    
                    //change the style and rebuild
                    ObjectId styleId_triang;
                    ObjectId styleId_lim2D;
                    var style_ = surface.StyleId;

                    styleId_triang = civil_doc.Styles.SurfaceStyles["TRIÂNGULOS E PONTOS"];
                    styleId_lim2D = civil_doc.Styles.SurfaceStyles["Limite da Triangulação (2D)"];
                    
                    if (style_ == styleId_triang)
                    {
                        surface.StyleId = styleId_lim2D;
                    }
                    else
                    {
                        surface.StyleId = styleId_triang;
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