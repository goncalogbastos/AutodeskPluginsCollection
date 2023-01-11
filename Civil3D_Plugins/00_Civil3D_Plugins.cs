using Autodesk.AutoCAD.Runtime;

namespace Civil3D_Plugins
{
    public class Civil3D_Plugins
    {
        // 01_AlignmentFromPolyline
        [CommandMethod("ca")]
        public void AlignmentFromPolyline()
        {
            var creator = new AlignmentFromPolyline();
            creator.Create();
        }

        // 02_SurfaceProfile
        [CommandMethod("sp")]
        public void SurfaceProfile()
        {
            var creator = new SurfaceProfile();
            creator.Create();
        }

        // 03_ProfileView
        [CommandMethod("pv")]
        public void Profile_View()
        {
            var creator = new Profile_View();
            creator.Create();
        }

        // 04_CorridorFrequency
        [CommandMethod("fr")]
        public void CorridorFrequency()
        {
            var creator = new CorridorFrequency();
            creator.Create();
        }

        // 05_CorridorTargets
        [CommandMethod("st")]
        public void CorridorTargets()
        {
            var creator = new CorridorTargets();
            creator.Create();
        }

        // 06_EventViewer
        [CommandMethod("ev")]
        public void EventViewer()
        {
            var creator = new EventViewer();
            creator.Create();
        }

        // 07_SurfaceStyle
        [CommandMethod("ss")]
        public void SurfaceStyle()
        {
            var creator = new SurfaceStyle();
            creator.Create();
        }

        // 08_OffsetAlignment
        [CommandMethod("oa")]
        public void OffsetAlignment()
        {
            var creator = new OffsetAlignment();
            creator.Create();
        }

        // 09_MleaderObjectName
        [CommandMethod("ann")]
        public void MleaderObjectName()
        {
            var creator = new MleaderObjectName();
            creator.Create();
        }


    }
}