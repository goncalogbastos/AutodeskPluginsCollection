using Autodesk.AutoCAD.Runtime;

namespace AutoCAD_Plugins
{
    public class AutoCAD_Plugins
    {
        // 01_AuxiliarLayer
        [CommandMethod("aux")]
        public void AuxiliarLayer()
        {
            var creator = new AuxiliarLayer();
            creator.Create();
        }

        // 02_NewLayer
        [CommandMethod("nl")]
        public void NewLayer()
        {
            var creator = new NewLayer();
            creator.Create();
        }

        // 03_NewBlock *************************NOT FINISHED***********************
        [CommandMethod("nb")]
        public void NewBlock()
        {
            var creator = new NewBlock();
            creator.Create();
        }

        // 04_InsertBlock *************************NOT FINISHED***********************
        [CommandMethod("ib")]
        public void InsertBlock()
        {
            var creator = new InsertBlock();
            creator.Create();
        }

        // 05_Slope     
        [CommandMethod("sl")]
        public void Slope()
        {
            var creator = new Slope();
            creator.Create();
        }

        // 06_GetSlope      
        [CommandMethod("gsl")]
        public void GetSlope()
        {
            var creator = new GetSlope();
            creator.Create();
        }

        // 07_PolylineAreaKm2   
        [CommandMethod("paf")]
        public void PolylineAreaKm2()
        {
            var creator = new PolylineAreaKm2();
            creator.Create();
        }

        // 08_PolylineAreaM2
        [CommandMethod("pam")]
        public void PolylineAreaM2()
        {
            var creator = new PolylineAreaM2();
            creator.Create();
        }

        // 09_PolylineLength
        [CommandMethod("comp")]
        public void PolylineLength()
        {
            var creator = new PolylineLength();
            creator.Create();
        }


        // 10_DoubleOffset     
        [CommandMethod("do")]
        public void DoubleOffset()
        {
            var creator = new DoubleOffset();
            creator.Create();
        }

        // 11_SelectObjectsInsidePolyline
        [CommandMethod("oip")]
        public void SelectObjectsInsidePolyline()
        {
            var creator = new SelectObjectsInsidePolyline();
            creator.Create();
        }

        // 12_CurrentViewTop
        [CommandMethod("vt")]
        public void CurrentViewTop()
        {
            var creator = new CurrentViewTop();
            creator.Create();
        }

        // 13_CurrentViewFront
        [CommandMethod("vf")]
        public void CurrentViewFront()
        {
            var creator = new CurrentViewFront();
            creator.Create();
        }

        // 14_MultipleChamfer *************************NOT FINISHED***********************
        [CommandMethod("mc")]
        public void MultipleChamfer()
        {
            var creator = new MultipleChamfer();
            creator.Create();
        }

        // 15_PolylineLenghtAndData
        [CommandMethod("pwf")]
        public void PolylineLenghtAndData()
        {
            var creator = new PolylineLenghtAndData();
            creator.Create();
        }

        // 16_CrossSuperelevationByPlanData
        [CommandMethod("cs")]
        public void CrossSuperelevationByPlanData()
        {
            var creator = new CrossSuperelevationByPlanData();
            creator.Create();
        }

        // 17_EndPointElevationByCrossSuperelevation
        [CommandMethod("ce")]
        public void EndPointElevationByCrossSuperelevation()
        {
            var creator = new EndPointElevationByCrossSuperelevation();
            creator.Create();
        }

        // 18_PickMPZPoint
        [CommandMethod("pmp")]
        public void PickMPZPoint()
        {
            var creator = new PickMPZPoint();
            creator.Create();
        }
    }
}
