using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Colors;

// Credits: https://www.keanw.com/2010/01/creating-an-autocad-layer-using-net.html 

namespace AutoCAD_Plugins
{
    public class NewLayer
    {
        //Color of first layer created. If any additional layers are created, than the new color will be _colorIndex + 1
        static short _colorIndex = 1;

        public void Create()
        {

            // Global Variables
            var doc = Application.DocumentManager.MdiActiveDocument;
            var db = doc.Database;
            var ed = doc.Editor;
            var tr = db.TransactionManager.StartTransaction();


            using (tr)
            {
                // Get the layer table from the drawing
                LayerTable lt = (LayerTable)tr.GetObject(db.LayerTableId, OpenMode.ForRead);



                // Check the layer name, to see whether it's already in use
                PromptStringOptions pso = new PromptStringOptions("\nEnter new layer name: ");
                pso.AllowSpaces = true;


                // A variable for the layer name
                string layName = "";

                do
                {
                    PromptResult pr = ed.GetString(pso);

                    // Just return if the user cancelled (will abort the transaction as we drop out of the using statement's scope)
                    if (pr.Status != PromptStatus.OK)
                        return;

                    try
                    {
                        // Validate the provided symbol table name
                        SymbolUtilityServices.ValidateSymbolName(pr.StringResult, false);

                        // Only set the layer name if it isn't in use
                        if (lt.Has(pr.StringResult))
                            ed.WriteMessage("\nA layer with this name already exists.");
                        else
                            layName = pr.StringResult;
                    }

                    catch
                    {
                        // An exception has been thrown, indicating the name is invalid
                        ed.WriteMessage("\nInvalid layer name.");
                    }
                }

                while (layName == "");

                // Create a new layer table record and set its properties
                LayerTableRecord ltr = new LayerTableRecord();

                ltr.Name = layName;
                ltr.Color = Color.FromColorIndex(ColorMethod.ByAci, _colorIndex);


                // Add the new layer to the layer table
                lt.UpgradeOpen();
                ObjectId ltId = lt.Add(ltr);
                tr.AddNewlyCreatedDBObject(ltr, true);


                // Set the layer to be current for this drawing
                db.Clayer = ltId;

                // Commit the transaction
                tr.Commit();

                // Report
                ed.WriteMessage("\nCreated layer named \"{0}\" with a color index of {1}.", layName, _colorIndex++);

            }

        }

    }

}
