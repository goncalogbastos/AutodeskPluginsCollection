using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.Civil.ApplicationServices;
using Autodesk.Civil.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;


// Credits: https://civilizeddevelopment.typepad.com/civilized-development/corridors/

namespace Civil3D_Plugins
{
    public class CorridorTargets
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
                    // Ask the user to select a corridor
                    PromptEntityOptions opt = new PromptEntityOptions("\nSelect a corridor:");
                    opt.SetRejectMessage("\nObject must be a corridor.");
                    opt.AddAllowedClass(typeof(Autodesk.Civil.DatabaseServices.Corridor), true);
                    PromptEntityResult res = ed.GetEntity(opt);
                    if (res.Status != PromptStatus.OK) return;
                    ObjectId corridorId = res.ObjectId;

                    // Ask the user to select a surface
                    PromptEntityOptions peo = new PromptEntityOptions("\nSelect a surface:");
                    peo.SetRejectMessage("\nObject must be a surface.");
                    peo.AddAllowedClass(typeof(Autodesk.Civil.DatabaseServices.TinSurface), true);
                    PromptEntityResult result = ed.GetEntity(peo);
                    if (result.Status != PromptStatus.OK) return;
                    ObjectId surfaceid = result.ObjectId;

                    // Initiate surface and corridor instance and access baseline region collection
                    TinSurface surface = tr.GetObject(surfaceid, OpenMode.ForRead) as TinSurface;
                    Corridor corridor = tr.GetObject(corridorId, OpenMode.ForWrite) as Corridor;
                    BaselineRegionCollection baselineRegionColl = corridor.Baselines[0].BaselineRegions; // Only works for the first baseline
                    var enumerator = baselineRegionColl.GetEnumerator();

                    while (enumerator.MoveNext())
                    {
                        BaselineRegion baselineregion = enumerator.Current;

                        SubassemblyTargetInfoCollection targets = baselineregion.GetTargets();

                        foreach (SubassemblyTargetInfo target in targets)
                        {
                            target.TargetIds.Add(surfaceid);
                            //target.TargetIds = surfaceid;
                        }

                        baselineregion.SetTargets(targets);

                    }

                    // Rebuild the corridor
                    corridor.Rebuild();

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