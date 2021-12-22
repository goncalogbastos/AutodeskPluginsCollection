using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.Civil.ApplicationServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;


namespace Civil3D_Plugins
{
    public class Count_Alignments
    {
        public void Create()
        {
            CivilDocument doc = CivilApplication.ActiveDocument;
            ObjectIdCollection alignments = doc.GetAlignmentIds();
            string docInfo = string.Format($"\nHello World!\nThis document has {alignments.Count} alignments.\n");
            Application.DocumentManager.MdiActiveDocument.Editor.WriteMessage(docInfo);
        }
    }
}