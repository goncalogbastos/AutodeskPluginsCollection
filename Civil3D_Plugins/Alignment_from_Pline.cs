using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.Civil.ApplicationServices;
using Autodesk.Civil.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;

// Credits: http://blog.civil3dreminders.com/2013/04/creating-alignments-with-net-api.html

namespace Civil3D_Plugins
{
    public class Alignment_from_Pline
    {
        public void Create()
        {
            var civil_doc = CivilApplication.ActiveDocument;
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

            // Prompt the user for the alignment name
            PromptStringOptions pso = new PromptStringOptions("\nEnter alignment name: ");
            pso.AllowSpaces = true;
            PromptResult pr = ed.GetString(pso);
            string alignmentName = pr.ToString();

            // Define your own layers, styles and labels
            string LayerId = "C-ROAD";
            string align_style = "COBA_EIXO";
            string label_style = "COBA_Eixo_PExecução";

            // Create the alignment from object
            ObjectId alignObjId = Alignment.Create(civil_doc,plops,alignmentName, null, LayerId, align_style, label_style);
        }
    }
}