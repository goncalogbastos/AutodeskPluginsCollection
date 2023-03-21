using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
//using Autodesk.AutoCAD.DynamoNodes;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.Civil.ApplicationServices;
using Autodesk.Civil.DatabaseServices;
using System.Collections.Generic;
using System;
//using Autodesk.DesignScript.Geometry;


namespace Civil3D_Plugins
{
    public class SampleLines
    {
        public void Create()
        {
            var civil_doc = CivilApplication.ActiveDocument;
            var ed = Application.DocumentManager.MdiActiveDocument.Editor;
            var db = Application.DocumentManager.MdiActiveDocument.Database;            
            
            try
            {
                PromptEntityOptions opt = new PromptEntityOptions("\nSelect an alignment:");
                opt.SetRejectMessage("\nObject must be an alignment.");
                opt.AddAllowedClass(typeof(Alignment), true);
                PromptEntityResult res = ed.GetEntity(opt);
                if (res.Status != PromptStatus.OK) return;                

                using (Transaction tr = db.TransactionManager.StartTransaction())
                {
                    ObjectId alignmentId = res.ObjectId;
                    Alignment myAlignment = tr.GetObject(alignmentId, OpenMode.ForWrite) as Alignment;
                    ObjectId slgId = SampleLineGroup.Create($"PT_{myAlignment.Name.ToString()}", alignmentId);
                    SampleLineGroup sampleLineGroup = tr.GetObject(slgId, OpenMode.ForWrite) as SampleLineGroup;          

                    var start_ = myAlignment.StartingStation;
                    var final_ = myAlignment.EndingStation;

                    List<double> stations = new List<double>();
                    for (double i = start_; i <= final_; i = i + 25)
                    {
                        stations.Add(i);
                    }
                    stations.Add(final_);

                    List<ObjectId> SLines = new List<ObjectId>();
                    foreach (double station in stations)
                    {
                        Point3d pt1 = myAlignment.GetPointAtDist(station);
                        Point3d pt2 = new Point3d();
                        try
                        {
                            pt2 = myAlignment.GetPointAtDist(station + 0.01);
                        }
                        catch
                        {
                            pt2 = myAlignment.GetPointAtDist(station - 0.01);
                        }                        
                        
                        Polyline pl = new Polyline();
                        pl.AddVertexAt(0, new Point2d(pt1.X, pt1.Y), 0, 0, 0);
                        pl.AddVertexAt(1, new Point2d(pt2.X, pt2.Y), 0, 0, 0);

                        Vector3d ang = pl.GetFirstDerivative(pl.GetParameterAtPoint(pt1));
                        ang = ang.GetNormal() * 10;
                        ang = ang.TransformBy(Matrix3d.Rotation(Math.PI / 2, pl.Normal, Point3d.Origin));
                        Line line = new Line(pt1 - ang, pt1 + ang);

                        double pt3X = line.StartPoint.X;
                        double pt3Y = line.StartPoint.Y;
                        double pt4X = line.EndPoint.X;
                        double pt4Y = line.EndPoint.Y;

                        Point2d pt3_ = new Point2d(pt3X, pt3Y);
                        Point2d pt4_ = new Point2d(pt4X, pt4Y);

                        ObjectId slatStationId = SampleLine.Create($"SL_{myAlignment.Name.ToString()}_{station.ToString()}", slgId, new Point2dCollection()
                        {
                            pt3_,
                            pt4_
                        });

                        using (Transaction tran = db.TransactionManager.StartTransaction())
                        {
                            SampleLine sampleLine = tran.GetObject(slatStationId, OpenMode.ForWrite) as SampleLine;
                            sampleLine.StyleId = civil_doc.Styles.SampleLineStyles["PLANTA"];

                            var sources = sampleLineGroup.GetSectionSources();
                            foreach (var source in sources)
                            {
                                source.IsSampled = true;
                            }
                            tran.Commit();
                        }
                    }
                    tr.Commit();

                }
                ed.Regen();
            }

            catch (Autodesk.AutoCAD.Runtime.Exception ex)
            {
                ed.WriteMessage("\nException message: " + ex.Message);
            }
            
        }
    }
}