using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Colors;
using Autodesk.AutoCAD.DatabaseServices;

namespace AutoCAD_Plugins
{
    public class Aux_Layer
    {
        public void Create()
        {
            var layerName = "@AUX";
            var doc = Application.DocumentManager.MdiActiveDocument;
            var database = doc.Database;

            // Turn lineweight on
            database.LineWeightDisplay = true;

            // Start transactionansaction -> Create Layer
            using (var tr = database.TransactionManager.StartTransaction())
            {
                var layerTable = (LayerTable)tr.GetObject(database.LayerTableId, Autodesk.AutoCAD.DatabaseServices.OpenMode.ForRead);
                LayerTableRecord layer;
                if (layerTable.Has(layerName) == false) // Checking if layer already exists

                // if layer not exists, create one
                {
                    layer = new LayerTableRecord
                    {
                        Name = layerName,
                        Color = Color.FromColorIndex(ColorMethod.ByAci, 3),
                        LineWeight = LineWeight.LineWeight035
                    };

                    layerTable.UpgradeOpen();
                    layerTable.Add(layer);
                    tr.AddNewlyCreatedDBObject(layer, true);
                }

                database.Clayer = layerTable[layerName];
                tr.Commit();
            };
        }
    }
}