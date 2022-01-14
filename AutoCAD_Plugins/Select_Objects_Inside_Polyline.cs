using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;

namespace AutoCAD_Plugins
{
    public class Select_Objects_Inside_Polyline
    {
        public void Create()
        {
            Document doc = null;
            Database db = null;
            Editor ed = null;

            double plOffset = 10;

            try
            {
                doc = Application.DocumentManager.MdiActiveDocument;
                if (doc == null)
                    throw new System.Exception("No MdiActiveDocument");
                db = doc.Database;
                ed = doc.Editor;


                TypedValue[] plTv = new TypedValue[] { new TypedValue(0, "LWPOLYLINE") };
                SelectionFilter plFlt = new SelectionFilter(plTv);
                PromptSelectionResult plSr = ed.GetSelection(plFlt);
                if (plSr.Status != PromptStatus.OK)
                    return;

                Point3dCollection polyPoints = null;
                using (Transaction tr = db.TransactionManager.StartTransaction())
                {
                    var curSpace = (BlockTableRecord)tr.GetObject(db.CurrentSpaceId, OpenMode.ForWrite);
                    foreach (SelectedObject obj in plSr.Value)
                    {
                        var pl = tr.GetObject(obj.ObjectId, OpenMode.ForRead) as Polyline;


                        var offsetObjs = pl.GetOffsetCurves(plOffset);
                        var offPl = offsetObjs[0] as Polyline;
                        if (offPl.Area < pl.Area)
                        {
                            offPl.Erase();
                            offsetObjs = pl.GetOffsetCurves(-plOffset);
                        }

                        curSpace.AppendEntity(offPl);
                        tr.AddNewlyCreatedDBObject(offPl, true);

                        polyPoints = new Point3dCollection();
                        for (int i = 0; i < offPl.NumberOfVertices; i++)
                            polyPoints.Add(offPl.GetPoint3dAt(i));

                    }
                    tr.Commit();
                }
                if (polyPoints == null)
                    throw new System.Exception("Failed to calculate Polyline Points");

                PromptSelectionOptions ss = new PromptSelectionOptions();
                TypedValue[] tv = { new TypedValue(0, "CIRCLE"), new TypedValue(0, "LINE") };
                SelectionFilter ftr = new SelectionFilter(tv);
                PromptSelectionResult res = ed.SelectCrossingPolygon(polyPoints); //, ftr);
                if (res.Status != PromptStatus.OK)
                    return;
                using (Transaction tr = db.TransactionManager.StartTransaction())
                {
                    var curSpace = (BlockTableRecord)tr.GetObject(db.CurrentSpaceId, OpenMode.ForRead);

                    foreach (var item in res.Value.GetObjectIds())
                    {
                        Entity c = (Entity)tr.GetObject(item, OpenMode.ForRead);

                        c.Highlight();
                    }
                    tr.Commit();
                }

            }
            catch (System.Exception ex)
            {
                if (ed != null)
                    ed.WriteMessage("\n Error in SelectInsidePolyline: {0}", ex.Message);
            }
        }
    }
}
