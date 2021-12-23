using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.Civil.ApplicationServices;
using Autodesk.Civil.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;

// Credits: https://adndevblog.typepad.com/infrastructure/2012/05/creating-a-profile-object-using-civil-3d-net-api.html

namespace Civil3D_Plugins
{
    public class Surface_Profile
    {
        public void Create()
        {
            var civil_doc = CivilApplication.ActiveDocument;
            var ed = Application.DocumentManager.MdiActiveDocument.Editor;


            // Ask the user to select an alignment
            PromptEntityOptions opt = new PromptEntityOptions("\nSelect an alignment:");
            opt.SetRejectMessage("\nObject must be an alignment.");
            opt.AddAllowedClass(typeof(Alignment), false);
            PromptEntityResult res = ed.GetEntity(opt);
            string alignmentName = res.StringResult;

            // Ask the user to select a surface
            PromptEntityOptions peo = new PromptEntityOptions("\nSelect a surface:");
            peo.SetRejectMessage("\nObject must be a surface.");
            peo.AddAllowedClass(typeof(Autodesk.Civil.DatabaseServices.Surface), false);
            PromptEntityResult result = ed.GetEntity(opt);
            string existing_ground = result.StringResult;

            //string existing_ground = "Terreno_T3.2_PE_Uniao_Cart_PTs";
            string profile_name = alignmentName + "_";
            string lay_name = "0";
            string style_name = "COBA_RASANTE";
            string label_name = "COBA_ANOTAÇÃO_RASANTE";

            ObjectId surfObjId = Profile.CreateFromSurface(profile_name, civil_doc, alignmentName, existing_ground, lay_name, style_name, label_name);
        }
    }
}

