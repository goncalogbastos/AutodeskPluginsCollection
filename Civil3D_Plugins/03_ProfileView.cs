using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.Civil.ApplicationServices;
using Autodesk.Civil.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using System;

namespace Civil3D_Plugins
{
    public class Profile_View
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
                    // Prompt the user for command keyword
                    PromptKeywordOptions pKeyOpts = new PromptKeywordOptions("\nEnter an option");
                    pKeyOpts.Keywords.Add("Normal"); // Normal h:v
                    pKeyOpts.Keywords.Add("SE"); // Superelevation
                    pKeyOpts.Keywords.Default = "Normal";
                    pKeyOpts.AllowNone = false;
                    PromptResult pKeyRes = ed.GetKeywords(pKeyOpts);

                    // Ask the user to select an alignment
                    PromptEntityOptions opt = new PromptEntityOptions("\nSelect an alignment:");
                    opt.SetRejectMessage("\nObject must be an alignment.");
                    opt.AddAllowedClass(typeof(Autodesk.Civil.DatabaseServices.Alignment), true);
                    PromptEntityResult res = ed.GetEntity(opt);
                    if (res.Status != PromptStatus.OK) return;
                    ObjectId alignmentId = res.ObjectId;

                    // Prompt the user for the insertation point
                    PromptPointOptions pPtOpts = new PromptPointOptions("");
                    pPtOpts.Message = "\nEnter the insertation point: ";

                    // Convert insertation point to 3D point
                    PromptPointResult pPtRes = ed.GetPoint(pPtOpts);
                    var insPt = pPtRes.Value;

                    var kw = pKeyRes.StringResult;
                    if (kw == "Normal")
                    {
                        // Get alignment and profile view properties
                        Alignment alignment = tr.GetObject(alignmentId, OpenMode.ForRead) as Alignment;
                        ObjectId profileviewstyleId = civil_doc.Styles.ProfileViewStyles["COBA_Grelha  [1_1000]Km. 25m - 100m_nSobrelevado"];
                        ObjectId bandSetId = civil_doc.Styles.ProfileViewBandSetStyles["COBA_PENTE_km100_cotas25_v02"];

                        System.Random random = new System.Random();
                        double val = Math.Round((random.NextDouble()), 2);
                        string profileview_name = $"{alignment.Name.ToString()}_{val}";

                        ObjectId profileviewId = ProfileView.Create(alignmentId, insPt, profileview_name, bandSetId, profileviewstyleId);
                        tr.Commit();
                    }
                    else if(kw == "SE")
                    {
                        // Get alignment and profile view properties
                        Alignment alignment = tr.GetObject(alignmentId, OpenMode.ForRead) as Alignment;
                        ObjectId profileviewstyleId = civil_doc.Styles.ProfileViewStyles["COBA_Grelha  [1_1000]Km. 25m - 100m"];
                        ObjectId bandSetId = civil_doc.Styles.ProfileViewBandSetStyles["COBA_PENTE_km100_cotas25_v02"];

                        System.Random random = new System.Random();
                        double val = Math.Round((random.NextDouble()), 2);
                        string profileview_name = $"{alignment.Name.ToString()}_{val}";

                        ObjectId profileviewId = ProfileView.Create(alignmentId, insPt, profileview_name, bandSetId, profileviewstyleId);
                        tr.Commit();
                    }
                    else
                    {
                        return;
                    }
                }
                catch (Autodesk.AutoCAD.Runtime.Exception ex)
                {
                    ed.WriteMessage("/n Exception message :" + ex.Message);
                }
            }
        }
    }
}