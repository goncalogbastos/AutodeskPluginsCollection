﻿using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.Civil.ApplicationServices;
using Autodesk.Civil.DatabaseServices;
using System.Collections.Generic;
using System;

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
                // ############## INPUTS ############## 
                PromptDoubleOptions pdoOffset = new PromptDoubleOptions("\nEnter offset: ");
                pdoOffset.DefaultValue = 10;
                PromptDoubleResult offset = ed.GetDouble(pdoOffset);
                if (offset.Status != PromptStatus.OK) return;

                PromptDoubleOptions pdoFrequency = new PromptDoubleOptions("\nEnter frequency: ");
                pdoFrequency.DefaultValue = 25;
                PromptDoubleResult frequency = ed.GetDouble(pdoFrequency);
                
                if(frequency.Status != PromptStatus.OK) return;

                PromptEntityOptions opt = new PromptEntityOptions("\nSelect an alignment: ");
                opt.SetRejectMessage("\nObject must be an alignment.");
                opt.AddAllowedClass(typeof(Alignment), true);
                PromptEntityResult res = ed.GetEntity(opt);
                if (res.Status != PromptStatus.OK) return;


                // ############## SAMPLE LINE GROUP & SAMPLE LINES ############## 

                using (Transaction tr = db.TransactionManager.StartTransaction())
                {
                    ObjectId alignmentId = res.ObjectId;
                    Alignment myAlignment = tr.GetObject(alignmentId, OpenMode.ForWrite) as Alignment;

                    ObjectId slgId = SampleLineGroup.Create($"PT{myAlignment.GetSampleLineGroupIds().Count + 1}_{frequency.Value}-{frequency.Value}m_{myAlignment.Name}", alignmentId);
                    SampleLineGroup sampleLineGroup = tr.GetObject(slgId, OpenMode.ForWrite) as SampleLineGroup;
                    
                    double startStation = myAlignment.StartingStation;
                    double finalStation = myAlignment.EndingStation;

                    List<double> stations = new List<double>();
                    for (double station = startStation; station <= finalStation; station += frequency.Value)
                    {
                        stations.Add(station);
                    }
                    stations.Add(finalStation);

                    int count = 0;
                    foreach (double station in stations)
                    {
                        SampleLines SL = new SampleLines();
                        Point2d[] points = SL.SampleLinePoints(myAlignment, station, offset.Value); // Call SampleLinePoints function

                        ObjectId slatStationId = SampleLine.Create($"SL_{sampleLineGroup.Name}_{count}", slgId, new Point2dCollection()
                        {
                            points[0],
                            points[1]
                        });

                        using (Transaction tran = db.TransactionManager.StartTransaction())
                        {
                            SampleLine sampleLine = tran.GetObject(slatStationId, OpenMode.ForWrite) as SampleLine;
                            sampleLine.StyleId = civil_doc.Styles.SampleLineStyles["PLANTA"];
                                                        
                            SectionSourceCollection sources = sampleLineGroup.GetSectionSources();
                                                                                   
                            foreach (SectionSource source in sources)
                            {
                                source.IsSampled = true;
                                Autodesk.Civil.DatabaseServices.Entity ent = source.SourceId.GetObject(OpenMode.ForRead) as Autodesk.Civil.DatabaseServices.Entity;
                                if (ent.Name.Contains("Terreno") ^ ent.Name.Contains("Classico") ^ ent.Name.Contains("Cartografia"))
                                {
                                    source.StyleId = civil_doc.Styles.SectionStyles["COBA_Terreno Natural"];
                                }
                                else
                                {
                                    source.StyleId = civil_doc.Styles.SectionStyles["COBA_PAVIMENTO"];
                                }
                            }
                            tran.Commit();
                        }
                        count ++;
                    }

                    // ############## SECTION VIEWS ############## 

                    SectionViewGroupCollection sectionViewGroupColl = sampleLineGroup.SectionViewGroups;
                    PromptPointResult ppr = ed.GetPoint("\nSelect a Location for the SectionViews: ");
                    if (ppr.Status != PromptStatus.OK)
                    {
                        tr.Commit();
                        ed.Regen();
                        return;
                    }                       
                    sectionViewGroupColl.Add(ppr.Value);
                    
                    foreach (SectionViewGroup svg in sectionViewGroupColl)
                    {
                        svg.PlotStyleId = civil_doc.Styles.GroupPlotStyles["COBA_Seções_PTransversais"];
                        foreach (ObjectId svID in svg.GetSectionViewIds())
                        {
                            SectionView sv = tr.GetObject(svID, OpenMode.ForWrite) as SectionView;
                            sv.StyleId = civil_doc.Styles.SectionViewStyles["COBA_Secções Transversais"];

                            ObjectId bandId = civil_doc.Styles.SectionViewBandSetStyles["COBA_Pente_Cotas Terreno_Sobrl_Cotas proj_Dist proj"]; 
                            sv.Bands.ImportBandSetStyle(bandId);
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

        private Point2d[] SampleLinePoints(Alignment alignment, Double station, Double offset)
        {
            // ############## GET SAMPLE LINES START AND END POINTS  ############## 

            Point3d pt1 = alignment.GetPointAtDist(station);
            Point3d pt2;
            try
            {
                pt2 = alignment.GetPointAtDist(station + 0.01);
            }
            catch
            {
                pt2 = alignment.GetPointAtDist(station - 0.01);
            }

            Polyline tangent = new Polyline();
            tangent.AddVertexAt(0, new Point2d(pt1.X, pt1.Y), 0, 0, 0);
            tangent.AddVertexAt(1, new Point2d(pt2.X, pt2.Y), 0, 0, 0);

            Vector3d vecTangent = tangent.GetFirstDerivative(tangent.GetParameterAtPoint(pt1));
            vecTangent = vecTangent.GetNormal() * offset; //Define offset
            vecTangent = vecTangent.TransformBy(Matrix3d.Rotation(Math.PI / 2, tangent.Normal, Point3d.Origin));
            Line perpendicular = new Line(pt1 - vecTangent, pt1 + vecTangent);

            Point2d pt3 = new Point2d(perpendicular.StartPoint.X, perpendicular.StartPoint.Y);
            Point2d pt4 = new Point2d(perpendicular.EndPoint.X, perpendicular.EndPoint.Y);

            Point2d[] points = { pt3, pt4 };

            return points;
        }
    }
}