using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.SystemModule;
using Cap.Ventas.BusinessObjects;
using FCap.Module.BusinessObjects.Ventas;
using FCap.Module.BusinessObjects;

namespace FCap.Module.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppWindowControllertopic.aspx.
    public partial class WCItemNew : WindowController
    {
        public WCItemNew()
        {
            InitializeComponent();
            // Target required Windows (via the TargetXXX properties) and create their Actions.
            TargetWindowType = WindowType.Main;
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target Window.
            ShowNavigationItemController showNavigationItemController = Frame.GetController<ShowNavigationItemController>();
            showNavigationItemController.CustomShowNavigationItem += showNavigationItemController_CustomShowNavigationItem;
        }
        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }

        void showNavigationItemController_CustomShowNavigationItem(object sender, CustomShowNavigationItemEventArgs e)
        {
            /*
            if (e.ActionArguments.SelectedChoiceActionItem.Id == "NvCbr")
            {
                IObjectSpace objectSpace = Application.CreateObjectSpace();
                DocumentoSalida newDoc = objectSpace.CreateObject<DocumentoSalida>();
                DetailView dv = Application.CreateDetailView(objectSpace, "DocumentoSalida_DetailView", true, newDoc);
                dv.ViewEditMode = ViewEditMode.Edit;
                e.ActionArguments.ShowViewParameters.CreatedView = dv;
                e.Handled = true;
            }
            else*/ if (e.ActionArguments.SelectedChoiceActionItem.Id == "NwCbr")
            {
                IObjectSpace objectSpace = Application.CreateObjectSpace();
                Ventas prm = objectSpace.FindObject<Ventas>(null);

                DetailView dv = null;
                // Tal vez esto hay que moverlo a Negocio
                if (prm.VntCfdi.CncptPg != null)
                {
                    Cxc newObj = objectSpace.CreateObject<Cxc>();
                    newObj.Cepto = objectSpace.FindObject<ConceptoCxcxp>
                        (new BinaryOperator("Oid", prm.VntCfdi.CncptPg.Oid));
                    newObj.Folio = prm.VntCfdi.UltmPg.ToString();
                    newObj.Serie = prm.VntCfdi.SrPg;

                    dv = Application.CreateDetailView(objectSpace, "Cxc_DetailView", true, newObj);
                }
                else
                {
                    dv = Application.CreateDetailView(objectSpace, "Ventas_DetailView", true, prm);
                }
                // DetailView dv = Application.CreateDetailView(objectSpace, "Cxc_DetailView", true, newObj);
                dv.ViewEditMode = ViewEditMode.Edit;
                e.ActionArguments.ShowViewParameters.CreatedView = dv;
                e.Handled = true;
            }
            else if (e.ActionArguments.SelectedChoiceActionItem.Id == "NwAntcp")
            {
                IObjectSpace objectSpace = Application.CreateObjectSpace();
                Ventas prm = objectSpace.FindObject<Ventas>(null);

                DetailView dv = null;
                // Tal vez esto hay que moverlo a Negocio
                if (prm.VntCfdi.CncptPg != null)
                {
                    Cxc newObj = objectSpace.CreateObject<Cxc>();
                    newObj.Cepto = objectSpace.FindObject<ConceptoCxcxp>(new BinaryOperator
                        ("Oid", prm.VntCfdi.CncptPg.Oid));
                    newObj.Folio = prm.VntCfdi.UltmPg.ToString();
                    newObj.Serie = prm.VntCfdi.SrPg;
                    newObj.CncptSrvc = "Anticipo del bien o servicio";

                    dv = Application.CreateDetailView(objectSpace, "Cxc_DetailView", true, newObj);
                }
                else
                {
                    dv = Application.CreateDetailView(objectSpace, "Ventas_DetailView", true, prm);
                }
                dv.ViewEditMode = ViewEditMode.Edit;
                e.ActionArguments.ShowViewParameters.CreatedView = dv;
                e.Handled = true;
            }
            else if (e.ActionArguments.SelectedChoiceActionItem.Id == "VentasPeriodo")
            {
                IObjectSpace objectSpace = Application.CreateObjectSpace();
                // e.ActionArguments.SelectedChoiceActionItem.
                //e.ActionArguments.ShowViewParameters.
                MyAnalysis myA = objectSpace.FindObject<MyAnalysis>(new BinaryOperator("Name", "Ventas Periodos"));
                DetailView detailView = Application.CreateDetailView(objectSpace, myA);
                detailView.ViewEditMode = ViewEditMode.Edit;
                e.ActionArguments.ShowViewParameters.CreatedView = detailView;
                e.ActionArguments.ShowViewParameters.TargetWindow = TargetWindow.Current;
                e.Handled = true;
            }
        }
    }
}
