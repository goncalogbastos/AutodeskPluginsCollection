using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;

namespace AutoCAD_Plugins
{
    public class PolylineLenghtAndData
    {
        public void Create()
        {
            var running = true;
            while (running)
            {
                var doc = Application.DocumentManager.MdiActiveDocument;
                var db = doc.Database;
                var ed = doc.Editor;

                using (Transaction tr = db.TransactionManager.StartTransaction())
                {
                    try
                    {
                        // Ask the user to select a polyline 
                        PromptEntityOptions optLA = new PromptEntityOptions("\nSelect a polyline: ");
                        optLA.SetRejectMessage("\nObject must be a polyline.");
                        optLA.AddAllowedClass(typeof(Polyline), true);
                        optLA.AddAllowedClass(typeof(Polyline), true);
                        PromptEntityResult resLA = ed.GetEntity(optLA);

                        if (resLA.Status != PromptStatus.OK)
                        {
                            running = false;
                            return;
                        }

                        // Create a field with area of the polyline -> <NAME> + <FIELD> + <m>
                        string strObjId = resLA.ObjectId.ToString();
                        strObjId = strObjId.Replace("(", "");
                        strObjId = strObjId.Replace(")", "");
                        string LALength = @"%<\AcObjProp Object(%<\_ObjId "
                            + strObjId + @">%).Length \f "
                            + @"%lu2%pr0"
                            + @">%";                           

                        // Ask the user to select a Mtext 
                        PromptEntityOptions optM = new PromptEntityOptions("\nSelect a Mtext: ");
                        optM.SetRejectMessage("\nObject must be a Mtext.");
                        optM.AddAllowedClass(typeof(MText), true);
                        optM.AddAllowedClass(typeof(MText), true);
                        PromptEntityResult resM = ed.GetEntity(optM);

                        if (resM.Status != PromptStatus.OK)
                        {
                            running = false;
                            return;
                        }

                        // Existing name of Mtext
                        DBObject obj = tr.GetObject(resM.ObjectId, OpenMode.ForRead);
                        MText mt = obj as MText;
                        string contents = mt.Text;
                        string name = contents.Split()[0];

                        // Prompt the user for the name
                        PromptStringOptions psoPMA = new PromptStringOptions("\nEnter PMA: ");
                        psoPMA.AllowSpaces = true;
                        PromptResult prPMA = ed.GetString(psoPMA);
                        string PMA = prPMA.StringResult;


                        // Prompt the user for the name
                        PromptStringOptions psoPMB = new PromptStringOptions("\nEnter PMB: ");
                        psoPMB.AllowSpaces = true;
                        PromptResult prPMB = ed.GetString(psoPMB);
                        string PMB = prPMB.StringResult;

                        // Prompt the user for the insertation point
                        PromptPointOptions pPtOpts = new PromptPointOptions("");
                        pPtOpts.Message = "\nEnter the insertation point: ";

                        // Convert insertation point to 3D point
                        PromptPointResult pPtRes = ed.GetPoint(pPtOpts);
                        var insPt = pPtRes.Value;

                        // Open the Block table for read
                        BlockTable block_table;
                        block_table = tr.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;

                        // Open the Block table record Model space for write
                        BlockTableRecord block_table_record;
                        block_table_record = tr.GetObject(block_table[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;

                        // Create a multiline text object
                        MText label = new MText();
                        label.SetDatabaseDefaults();
                        label.Location = insPt;
                        label.Attachment = AttachmentPoint.MiddleCenter;
                        label.TextHeight = 25;
                        label.Contents = name + "\n" + PMA + "\n" + PMB + "\n" + LALength;
                        block_table_record.AppendEntity(label);
                        tr.AddNewlyCreatedDBObject(label, true);
                        

                        tr.Commit();
                        ed.Regen();
                        
                    }
                    catch (Autodesk.AutoCAD.Runtime.Exception ex)
                    {
                        ed.WriteMessage("\nException message :" + ex.Message);
                    }
                }
            }
        }
    }
}