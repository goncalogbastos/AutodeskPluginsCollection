using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.Civil.ApplicationServices;
using Autodesk.Civil.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using System;


namespace Civil3D_Plugins
{
    public class Surfaces
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


                    Alignment alignment = tr.GetObject(alignmentId, OpenMode.ForRead) as Alignment;

                    var SURFid = alignment.GetProfileIds();

                    for (int i = 0; i < SURFid.Count; i++)
                    {
                        ObjectId profileID = SURFid[i];
                        Profile profile = tr.GetObject(profileID, OpenMode.ForRead) as Profile;
                        ed.WriteMessage(profile.Name);
                    }
                       

                        




                    /*
                    // Ask the user to select a surface
                    PromptEntityOptions peo = new PromptEntityOptions("\nSelect a surface:");
                    peo.SetRejectMessage("\nObject must be a surface.");
                    peo.AddAllowedClass(typeof(Autodesk.Civil.DatabaseServices.TinSurface), true);
                    PromptEntityResult result = ed.GetEntity(peo);
                    if (result.Status != PromptStatus.OK) return;
                    ObjectId surfaceid = result.ObjectId;

                    // Get alignment properties
                    Alignment alignment = tr.GetObject(alignmentId, OpenMode.ForRead) as Alignment;
                    TinSurface surface = tr.GetObject(surfaceid, OpenMode.ForRead) as TinSurface;
                    ObjectId layerId = alignment.LayerId;
                    ObjectId styleId = civil_doc.Styles.ProfileStyles["COBA_TERRENO NATURAL"];
                    ObjectId labelId = civil_doc.Styles.LabelSetStyles.ProfileLabelSetStyles["_Aucun affichage"];

                    // Create Surface Profile
                    System.Random random = new System.Random();
                    double val = Math.Round((random.NextDouble()), 2);
                    ObjectId surfObjId = Profile.CreateFromSurface($"{surface.Name.ToString()}_{val}", alignmentId, surfaceid, layerId, styleId, labelId);
                    */
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