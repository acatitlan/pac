using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using Cap.Bancos.BusinessObjects;
using Cap.Bancos.Utilerias;
using DevExpress.Persistent.BaseImpl;
using DevExpress.ExpressApp.ReportsV2;
using DevExpress.XtraReports.UI;
using DevExpress.Data.Filtering;

namespace Cap.Bancos.Controllers
{
    // For more typical usage scenarios, be sure to check out http://documentation.devexpress.com/#Xaf/clsDevExpressExpressAppViewControllertopic.
    public partial class VCMovimientoB : ViewController
    {
        public VCMovimientoB()
        {
            InitializeComponent();
            RegisterActions(components);
            // Target required Views (via the TargetXXX properties) and create their Actions.

            TargetObjectType = typeof(MovimientoB);

            simpleActionCanclr.TargetObjectsCriteria = "Status != 'Cancelado'";
                // string.Format("Status != {0}", MovimientoStatus.Cancelado.GetHashCode());
            simpleActionAplTrnst.TargetObjectsCriteria = "Status = 'Transito'";


            /*Dic 2019, para mostrar el dialogo filtro del reporte
            PopupWindowShowAction action = new PopupWindowShowAction(this, "ShowPopup", DevExpress.Persistent.Base.PredefinedCategory.View);
            action.CustomizePopupWindowParams += new CustomizePopupWindowParamsEventHandler(action_CustomizePopupWindowParams);
            action.Execute += new PopupWindowShowActionExecuteEventHandler(action_Execute);*/
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.
            View.ObjectSpace.Committing += ObjectSpace_Committing;
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
        }
        protected override void OnDeactivated()
        {
            View.ObjectSpace.Committing -= ObjectSpace_Committing;
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }


        void ObjectSpace_Committing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MovimientoB obj = View.CurrentObject as MovimientoB;

            if (obj != null)
                e.Cancel = !NegocioBancos.GrabaMovimiento(obj);
        }

        private void simpleActionRprtMovsmnts_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            /*Obsolete
            string nameR = "Movimientos";

            ReportData donneesEtat = (from reportData in new XPQuery<ReportData>(((XPObjectSpace)View.ObjectSpace).Session)
                                      where reportData.ReportName == nameR
                                      select reportData).FirstOrDefault();

            Frame.GetController<ReportServiceController>().ShowPreview(donneesEtat, null);*/
            string format = "MovimientosB";
            IObjectSpace objectSpace =
                ReportDataProvider.ReportObjectSpaceProvider.CreateObjectSpace(typeof(ReportDataV2));
            IReportDataV2 reportData2 = objectSpace.FindObject<ReportDataV2>(
                new BinaryOperator("DisplayName", format));
            string handle = ReportDataProvider.ReportsStorage.GetReportContainerHandle(reportData2);
            XtraReport report2 = ReportDataProvider.ReportsStorage.LoadReport(reportData2);
            ReportServiceController controller = Frame.GetController<ReportServiceController>();
            if (controller != null)
            {
                MovimientoB mov = View.CurrentObject as MovimientoB;
                GroupOperator fil = new GroupOperator();
                if (mov != null)
                    fil.Operands.Add(new BinaryOperator("Cuenta", mov.Cuenta));

                controller.ShowPreview(handle, fil);
            }
            else
            {
                ReportsModuleV2.FindReportsModule(Application.Modules).ReportsDataSourceHelper.SetupBeforePrint(report2);

                report2.ShowPreview();
            }
        }

        private void simpleActionCanclr_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            if (View != null)
            {
                MovimientoB obj = View.CurrentObject as MovimientoB;

                NegocioBancos.CancMov(obj);
                View.ObjectSpace.CommitChanges();
            }
        }

        private void simpleActionAplTrnst_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            if (View != null)
            {
                MovimientoB obj = View.CurrentObject as MovimientoB;

                if (obj != null)
                {
                    NegocioBancos.AppTransito(obj);
                    View.ObjectSpace.CommitChanges();
                }
            }
        }

        void action_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            /*
            RPOMovimiento2 obj = new RPOMovimiento2();
            e.View = Application.CreateDetailView(Application.CreateObjectSpace(), obj);*/
        }

        void action_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            /* Dic 2019 para mostrar el dialogo filtro y luego el reporte pero no pude.
            IObjectSpace objectSpace = ReportDataProvider.ReportObjectSpaceProvider.CreateObjectSpace(typeof(ReportDataV2));
            IReportDataV2 reportData = objectSpace.FindObject<ReportDataV2>(CriteriaOperator.Parse("[DisplayName] = 'MovimientosB'"));
            string handle = ReportDataProvider.ReportsStorage.GetReportContainerHandle(reportData);

            RPOMovimiento2 obj = e.CurrentObject as RPOMovimiento2;
            XtraReport report2 = ReportDataProvider.ReportsStorage.LoadReport(reportData);
            ReportsModuleV2.FindReportsModule(Application.Modules).ReportsDataSourceHelper.SetupBeforePrint(report2, obj, null, false, null, false);

            report2.ShowPreview();*/
        }
    }
}
