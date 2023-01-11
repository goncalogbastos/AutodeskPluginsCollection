using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.Civil.ApplicationServices;
using Autodesk.Civil.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;

namespace Civil3D_Plugins
{
    public class CorridorFrequency
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

                    // Prompt the user for the frequency
                    PromptDoubleOptions pdo = new PromptDoubleOptions("\nEnter frequency: ");
                    PromptDoubleResult freq = ed.GetDouble(pdo);

                    // Initiate corridor instance and access baseline region collection
                    Corridor corridor = tr.GetObject(corridorId, OpenMode.ForWrite) as Corridor;
                    BaselineRegionCollection baselineRegionColl = corridor.Baselines[0].BaselineRegions; // Only works for the first baseline
                    var enumerator = baselineRegionColl.GetEnumerator();

                    while (enumerator.MoveNext())
                    {
                        BaselineRegion baselineregion = enumerator.Current;

                        // Apply frequency defined by the user
                        baselineregion.AppliedAssemblySetting.FrequencyAlongCurves = freq.Value;
                        baselineregion.AppliedAssemblySetting.FrequencyAlongProfileCurves = freq.Value;
                        baselineregion.AppliedAssemblySetting.FrequencyAlongSpirals = freq.Value;
                        baselineregion.AppliedAssemblySetting.FrequencyAlongTangents = freq.Value;
                        baselineregion.AppliedAssemblySetting.FrequencyAlongTargetCurves = freq.Value;

                        // Other baseline region options:
                        baselineregion.AppliedAssemblySetting.CorridorAlongCurvesOption = 0; // 0: CurveAtIncrement; 1: CurveByCurvature; 2: CurveBoth
                        baselineregion.AppliedAssemblySetting.AppliedAtHorizontalGeometryPoints = false;
                        baselineregion.AppliedAssemblySetting.AppliedAtSuperelevationCriticalPoints = false;
                        baselineregion.AppliedAssemblySetting.AppliedAdjacentToOffsetTargetStartEnd = false;
                        baselineregion.AppliedAssemblySetting.AppliedAtOffsetTargetGeometryPoints = false;
                        baselineregion.AppliedAssemblySetting.AppliedAtProfileGeometryPoints = false;
                        baselineregion.AppliedAssemblySetting.AppliedAtProfileHighLowPoints = false;
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