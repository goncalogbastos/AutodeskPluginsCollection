using Autodesk.AutoCAD.Runtime;

namespace Civil3D_Plugins
{
    public class Civil3D_Plugins
    {
        // Count Civil alignments
        [CommandMethod("ca")]
        public void create_alignment()
        {
            var creator = new Create_Alignments();
            creator.Create();
        }
    }
}