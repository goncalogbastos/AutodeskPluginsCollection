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

        // Create Profile View
        [CommandMethod("pv")]
        public void profile_view()
        {
            var creator = new Profile_View();
            creator.Create();
        }

        // Change Corridor frequency_
        [CommandMethod("fr")]
        public void corridor_frequency()
        {
            var creator = new Corridor_Frequency();
            creator.Create();
        }

        // Set all targets on corridor
        [CommandMethod("st")]
        public void corridor_targets()
        {
            var creator = new Corridor_Targets();
            creator.Create();
        }

        // Turn off event viewer
        [CommandMethod("ev")]
        public void event_viewer()
        {
            var creator = new Event_Viewer();
            creator.Create();
        }

        // Change Surface Style
        [CommandMethod("ss")]
        public void surface_style()
        {
            var creator = new Surface_Style();
            creator.Create();
        }

        // Create Offset Alignment
        [CommandMethod("oa")]
        public void offset_alignment()
        {
            var creator = new Offset_Alignment();
            creator.Create();
        }

        // Annotate mleader with field object name
        [CommandMethod("ann")]
        public void mleader_name()
        {
            var creator = new Mleader_Name();
            creator.Create();
        }


    }
}