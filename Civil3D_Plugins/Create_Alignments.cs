using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.Civil.ApplicationServices;
using Autodesk.Civil.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;


// Credits: http://blog.civil3dreminders.com/2013/04/creating-alignments-with-net-api.html

namespace Civil3D_Plugins
{
    public class Create_Alignments
    {
        public void Create()
        {
            var doc = CivilApplication.ActiveDocument;
            var ed = Application.DocumentManager.MdiActiveDocument.Editor;

            // Ask the user to select a polyline to convert to an alignment
            PromptEntityOptions opt = new PromptEntityOptions("\nSelect a polyline to convert to an Alignment");
            opt.SetRejectMessage("\nObject must be a polyline.");
            opt.AddAllowedClass(typeof(Polyline), false);
            PromptEntityResult res = ed.GetEntity(opt);

            // create some polyline options for creating the new alignment
            PolylineOptions plops = new PolylineOptions();
            plops.AddCurvesBetweenTangents = false;
            plops.EraseExistingEntities = true;
            plops.PlineId = res.ObjectId;

            // uses an existing Alignment Style and Label Set Style
            ObjectId testAlignmentID = Alignment.FromAcadObject(res.ObjectId);

        }
    }
}