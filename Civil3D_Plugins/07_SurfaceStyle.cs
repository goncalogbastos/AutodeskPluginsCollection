using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.Civil.ApplicationServices;
using Autodesk.Civil.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using System;
using System.Linq;

namespace Civil3D_Plugins
{
    public class SurfaceStyle
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
                    PromptKeywordOptions pKeyOpts = new PromptKeywordOptions("\nSelect a surface: ");
                    pKeyOpts.AllowNone = false;

                    ObjectIdCollection SurfaceIds = civil_doc.GetSurfaceIds();
                    foreach (ObjectId surfaceId in SurfaceIds)
                    {
                        TinSurface oSurface = surfaceId.GetObject(OpenMode.ForRead) as TinSurface;
                        string surf = oSurface.Name;
                        string surf_ = String.Concat(surf.Where(c => !Char.IsWhiteSpace(c)));
                        string surf__ = surf_.Replace("_", string.Empty).Replace("-",string.Empty);
                        pKeyOpts.Keywords.Add(surf__);

                    }

                    PromptResult result = ed.GetKeywords(pKeyOpts);
                    if (result.Status != PromptStatus.OK) return;

                    foreach (ObjectId surfaceId in SurfaceIds)
                    {
                        TinSurface oSurface = surfaceId.GetObject(OpenMode.ForRead) as TinSurface;
                        string surf = oSurface.Name;
                        string surf_ = String.Concat(surf.Where(c => !Char.IsWhiteSpace(c)));
                        string surf__ = surf_.Replace("_", string.Empty).Replace("-", string.Empty);
                        if (result.StringResult == surf__)
                        {
                            TinSurface surface = tr.GetObject(surfaceId, OpenMode.ForRead) as TinSurface;
                            //change the style and rebuild
                            ObjectId styleId_triang;
                            ObjectId styleId_lim2D;
                            ObjectId styleId_inv;

                            var style_ = surface.StyleId;

                            styleId_triang = civil_doc.Styles.SurfaceStyles["TRIÂNGULOS E PONTOS"];
                            styleId_lim2D = civil_doc.Styles.SurfaceStyles["Limite da Triangulação (2D)"];
                            styleId_inv = civil_doc.Styles.SurfaceStyles["_DESLIGADO"];

                            if (style_ == styleId_triang)
                            {
                                surface.StyleId = styleId_lim2D;
                            }
                            else if(style_ == styleId_lim2D)
                            {
                                surface.StyleId = styleId_inv;
                            }
                            else if (style_ == styleId_inv)
                            {
                                surface.StyleId = styleId_triang;
                            }
                            else
                            {
                                surface.StyleId = styleId_triang;
                            }
                        }
                        else
                        {
                            continue;
                        }
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