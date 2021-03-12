using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.ReportsV2;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.Persistent.BaseImpl;
using SSRV.Module.BusinessObjects.Servicio;

namespace SATM.Module.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppWindowControllertopic.aspx.
    public partial class WCNavItem : WindowController
    {
        public WCNavItem()
        {
            InitializeComponent();
            // Target required Windows (via the TargetXXX properties) and create their Actions.
            TargetWindowType = WindowType.Main;
        }

        ShowNavigationItemController showNavigationItemController;
        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target Window.
            showNavigationItemController = Frame.GetController<ShowNavigationItemController>();
            showNavigationItemController.CustomShowNavigationItem += showNavigationItemController_CustomShowNavigationItem;
        }
        protected override void OnDeactivated()
        {
            showNavigationItemController.CustomShowNavigationItem -= showNavigationItemController_CustomShowNavigationItem;
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }


        void showNavigationItemController_CustomShowNavigationItem(object sender, CustomShowNavigationItemEventArgs e)
        {
            if (e.ActionArguments.SelectedChoiceActionItem.Id == "NewIncidencia")
            {
                IObjectSpace objectSpace = Application.CreateObjectSpace();
                Incidencia newDoc = objectSpace.CreateObject<Incidencia>();
                DetailView dv = Application.CreateDetailView(objectSpace, "Incidencia_DetailView", true, newDoc);
                dv.ViewEditMode = ViewEditMode.Edit;
                e.ActionArguments.ShowViewParameters.CreatedView = dv;
                e.Handled = true;
            }
            else if (e.ActionArguments.SelectedChoiceActionItem.Id == "RepIncidencia")
            {
                IObjectSpace objectSpace = ReportDataProvider.ReportObjectSpaceProvider.CreateObjectSpace(typeof(ReportDataV2));
                var reportData = objectSpace.FindObject<ReportDataV2>(new BinaryOperator("DisplayName", "Incidencia"));
                Frame.GetController<ReportServiceController>().ShowPreview(ReportDataProvider.ReportsStorage.GetReportContainerHandle(reportData));
                e.Handled = true;
            }
        }
    }
}
