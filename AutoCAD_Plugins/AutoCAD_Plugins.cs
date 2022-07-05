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

        // Gets the slope of a polyline        
        [CommandMethod("gsl")]
        public void get_slop()
        {
            var creator = new Get_Slope();
            creator.Create();
        }

        //Area of closed polyline km2     
        [CommandMethod("paf")]
        public void area_pline_field()
        {
            var creator = new Area_Pline_Field();
            creator.Create();
        }

        //Area of closed polyline m2    
        [CommandMethod("aaa")]
        public void area_pline()
        {
            var creator = new Area_Pline();
            creator.Create();
        }

        //Lenght of polyline m
        [CommandMethod("comp")]
        public void len_pl()
        {
            var creator = new Len_PL();
            creator.Create();
        }


        //Double offset of polyline       
        [CommandMethod("do")]
        public void double_offset()
        {
            var creator = new Double_Offset();
            creator.Create();
        }

        // Select all objects inside an object
        [CommandMethod("oip")]
        public void select_objects_inside_polyline()
        {
            var creator = new Select_Objects_Inside_Polyline();
            creator.Create();
        }

        // Change current view to top view
        [CommandMethod("vt")]
        public void current_view_top()
        {
            var creator = new Current_View_Top();
            creator.Create();
        }

        // Change current view to front view
        [CommandMethod("vf")]
        public void current_view_front()
        {
            var creator = new Current_View_Front();
            creator.Create();
        }

        // Multiple chamfer with layer control *************************NOT FINISHED***********************
        [CommandMethod("mc")]
        public void multiple_chamfer()
        {
            var creator = new Multiple_Chamfer();
            creator.Create();
        }

    }
}
