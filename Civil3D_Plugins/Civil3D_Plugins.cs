using Autodesk.AutoCAD.Runtime;

namespace Civil3D_Plugins
{
    public class Civil3D_Plugins
    {
        // Create Alignment from polyline
        [CommandMethod("ca")]
        public void alignment_from_pline()
        {
            var creator = new Alignment_from_Pline();
            creator.Create();
        }
        
        // Create Surface Profile
        [CommandMethod("sp")]
        public void surface_profile()
        {
            var creator = new Surface_Profile();
            creator.Create();
        }
        
    }
}