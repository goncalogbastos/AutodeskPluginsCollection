using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;

namespace Civil3D_Plugins
{
    public class MleaderObjectName
    {
        public void Create()
        {
            // Global Variables
            var doc = Application.DocumentManager.MdiActiveDocument;
            var db = doc.Database;
            var ed = doc.Editor;
            var Tx = db.TransactionManager.StartTransaction();

            using (Tx)
            {
                // Block table and block table record
                BlockTable table = Tx.GetObject(db.BlockTableId,OpenMode.ForRead) as BlockTable;
                BlockTableRecord model = Tx.GetObject(table[BlockTableRecord.ModelSpace],OpenMode.ForWrite) as BlockTableRecord;

                // Ask the user to select an object
                PromptEntityOptions opt = new PromptEntityOptions("\nSelect an object: ");
                PromptEntityResult res = ed.GetEntity(opt);

                if (res.Status != PromptStatus.OK)
                {
                    return;
                }

                // Create a field with name of the object
                string strObjId = res.ObjectId.ToString();
                strObjId = strObjId.Replace("(", "");
                strObjId = strObjId.Replace(")", "");
                string field = @"%<\AcObjProp Object(%<\_ObjId " + strObjId + @">%).Name>%";

                // Prompt the user for the insertation point and convert it to 3D point
                PromptPointOptions pPtOpts = new PromptPointOptions("");
                pPtOpts.Message = "\nEnter the arrowhead insertation point: ";
                PromptPointResult pPtRes = ed.GetPoint(pPtOpts);
                var insPt = pPtRes.Value;

                // Prompt the user for the insertation point and convert it to 3D point
                PromptPointOptions pPtOpts_ = new PromptPointOptions("");
                pPtOpts_.Message = "\nEnter the landing point: ";
                PromptPointResult pPtRes_ = ed.GetPoint(pPtOpts_);
                var landingPt = pPtRes_.Value;

                // Create new mleader
                MLeader leader = new MLeader();
                leader.SetDatabaseDefaults();
                leader.ContentType = ContentType.MTextContent;
                leader.ArrowSize = 15;                

                // Create new Mtext
                MText mText = new MText();
                mText.SetDatabaseDefaults();
                mText.Width = 10;
                mText.Height = 5;
                mText.TextHeight = 10;
                mText.SetContentsRtf(field);
                mText.Location = landingPt;

                // Insert mtext in leader
                leader.MText = mText;
                leader.AddLeaderLine(insPt);
                model.AppendEntity(leader);
                Tx.AddNewlyCreatedDBObject(leader, true);

                // Commit the transaction
                Tx.Commit();
            }

        }

    }

}
