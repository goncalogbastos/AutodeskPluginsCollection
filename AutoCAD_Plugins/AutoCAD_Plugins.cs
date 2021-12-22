using Autodesk.AutoCAD.Runtime;

namespace AutoCAD_Plugins
{
    public class AutoCAD_Plugins
    {
        // Creates new auxiliar layer named "@AUX"
        [CommandMethod("aux")]
        public void aux_layer()
        {
            var creator = new Aux_Layer();
            creator.Create();
        }

        // Creates a new layer
        [CommandMethod("nl")]
        public void new_layer()
        {
            var creator = new New_Layer();
            creator.Create();
        }

        // Creates a new block *************************NOT FINISHED***********************
        [CommandMethod("nb")]
        public void new_block()
        {
            var creator = new New_Block();
            creator.Create();
        }

        // Inserts a block *************************NOT FINISHED***********************
        [CommandMethod("ib")]
        public void insert_block()
        {
            var creator = new Insert_Block();
            creator.Create();
        }

        // Inserts a polyline with a certain slope        
        [CommandMethod("sl")]
        public void slope()
        {
            var creator = new Slope();
            creator.Create();
        }

    }
}
