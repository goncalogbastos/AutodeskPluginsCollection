using Autodesk.AutoCAD.Runtime;

namespace Civil3D_Plugins
{
    public class Civil3D_Plugins
    {
        // Count Civil alignments
        [CommandMethod("ca")]
        public void count_alignments()
        {
            var creator = new Count_Alignments();
            creator.Create();
        }
    }
}