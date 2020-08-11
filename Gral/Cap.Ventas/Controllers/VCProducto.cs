using Cap.Inventarios.BusinessObjects;
using Cap.Ventas.Utilerias;
using DevExpress.Data;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Model;
using DevExpress.Xpo.DB;

namespace Cap.Ventas.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class VCProducto : ViewController
    {
        public VCProducto()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
            TargetObjectType = typeof(Producto);
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.
            // Ordena(); Ago 2019 Que lo ordene el usuario a través de las columnas

            if (View != null && View is DetailView && View.ObjectSpace != null)
                View.ObjectSpace.Committing += ObjectSpace_Committing;
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
            if (View != null && View is DetailView)
            {
                Producto obj = View.CurrentObject as Producto;

                if (obj != null && View.ObjectSpace.IsNewObject(obj))
                    NegocioVentas.IniciaProducto(obj);
            }
        }
        protected override void OnDeactivated()
        {
            if (View is DetailView)
                View.ObjectSpace.Committing -= ObjectSpace_Committing;
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }

        private void ObjectSpace_Committing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (View != null)
            {
                Producto obj = View.CurrentObject as Producto;

                if (obj != null && View.ObjectSpace.IsNewObject(obj))
                {
                    NegocioVentas.GrabaProducto(obj);
                    e.Cancel = false;
                }
            }
        }

        private void Ordena()
        {
            ListView lv = View as ListView;
            string propertyName = "Clave";
            bool demoFlag = true;
            //Dennis: This code applies a client side sorting.
            if (demoFlag)
            {
                IModelColumn columnInfo = ((IModelList<IModelColumn>)lv.Model.Columns)[propertyName];
                if (columnInfo != null)
                {
                    columnInfo.SortIndex = 0;
                    columnInfo.SortOrder = ColumnSortOrder.Ascending;
                }
            }
            else
            {
                //Dennis: This code is used for the server side sorting.

                if (((IModelList<IModelSortProperty>)lv.Model.Sorting)[propertyName] == null)
                {
                    IModelSortProperty sortProperty = lv.Model.Sorting.AddNode<IModelSortProperty>(propertyName);
                    sortProperty.Direction = SortingDirection.Ascending;
                    sortProperty.PropertyName = propertyName;
                }
            }
        }
    }
}
