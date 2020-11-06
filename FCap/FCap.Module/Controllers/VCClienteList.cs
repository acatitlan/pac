using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Model;
using DevExpress.Data;
using DevExpress.Xpo.DB;
using Cap.Clientes.BusinessObjects.Clientes;
using DevExpress.ExpressApp.Actions;
using FCap.Module.Utilerias;

namespace FCap.Module.Controllers
{
    // For more information on Controllers and their life cycle, check out the http://documentation.devexpress.com/#Xaf/CustomDocument2621 and http://documentation.devexpress.com/#Xaf/CustomDocument3118 help articles.
    public partial class VCClienteList : ViewController
    {
        // Use this to do something when a Controller is instantiated (do not execute heavy operations here!).
        public VCClienteList()
        {
            InitializeComponent();
            RegisterActions(components);
            // For instance, you can specify activation conditions of a Controller or create its Actions (http://documentation.devexpress.com/#Xaf/CustomDocument2622).
            //TargetObjectType = typeof(DomainObject1);
            //TargetViewType = ViewType.DetailView;
            //TargetViewId = ModelNodeIdHelper.GetDetailViewId(typeof(DomainObject1));
            //TargetViewNesting = Nesting.Root;
            //SimpleAction myAction = new SimpleAction(this, "MyActionId", DevExpress.Persistent.Base.PredefinedCategory.RecordEdit);

            TargetObjectType = typeof(Cliente);
            TargetViewType = ViewType.ListView;

            popupWindowShowActionImprtrClnts.TargetObjectType = typeof(Cliente);
            popupWindowShowActionImprtrClnts.TargetViewType = ViewType.ListView;
        }
        // Override to do something before Controllers are activated within the current Frame (their View property is not yet assigned).
        protected override void OnFrameAssigned()
        {
            base.OnFrameAssigned();
            //For instance, you can access another Controller via the Frame.GetController<AnotherControllerType>() method to customize it or subscribe to its events.
        }
        // Override to do something when a Controller is activated and its View is assigned.
        protected override void OnActivated()
        {
            base.OnActivated();
            //For instance, you can customize the current View and its editors (http://documentation.devexpress.com/#Xaf/CustomDocument2729) or manage the Controller's Actions visibility and availability (http://documentation.devexpress.com/#Xaf/CustomDocument2728).

            // Se puede poner en tiempo de ejecución !
            // Ordenar();
        }
        // Override to access the controls of a View for which the current Controller is intended.
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // For instance, refer to the http://documentation.devexpress.com/Xaf/CustomDocument3165.aspx help article to see how to access grid control properties.
        }
        // Override to do something when a Controller is deactivated.
        protected override void OnDeactivated()
        {
            // For instance, you can unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }

        private void Ordenar()
        {
            string propertyName = "Clave";
            bool demoFlag = true;
            ListView lv = View as ListView;

            //Dennis: This code applies a client side sorting.
            if (demoFlag)
            {
                IModelColumn columnInfo = ((IModelList<IModelColumn>)lv.Model.Columns)[propertyName];
                if (columnInfo != null)
                {
                    columnInfo.SortIndex = 0;
                    columnInfo.SortOrder = ColumnSortOrder.Descending;
                }
            }
            else
            {
                //Dennis: This code is used for the server side sorting.
                if (((IModelList<IModelSortProperty>)lv.Model.Sorting)[propertyName] == null)
                {
                    IModelSortProperty sortProperty = lv.Model.Sorting.AddNode<IModelSortProperty>(propertyName);
                    sortProperty.Direction = SortingDirection.Descending;
                    sortProperty.PropertyName = propertyName;
                }
            }
        }


        private void popupWindowShowActionImprtrClnts_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            IObjectSpace objectSpace = Application.CreateObjectSpace();
            ClienteImportar fil = objectSpace.FindObject<ClienteImportar>(null);

            if (fil == null)
                fil = objectSpace.CreateObject<ClienteImportar>();

            e.View = Application.CreateDetailView(objectSpace, "ClienteImportar_DetailView", true, fil);
        }

        private void popupWindowShowActionImprtrClnts_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            ClienteImportar parameter = (ClienteImportar)e.PopupWindowViewCurrentObject;

            NegocioAdmin.ImportaClientes(parameter, Application.CreateObjectSpace());
        }

    }
}
