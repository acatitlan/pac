using System;
using System.Collections;
using Cap.Inventarios.BusinessObjects;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;

namespace Cap.Inventarios.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class VCProductoList : ViewController
    {
        public VCProductoList()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
            TargetObjectType = typeof(Producto);
            TargetViewType = ViewType.ListView;

            popupWindowShowActionActPrecs.TargetObjectType = typeof(Producto);
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
        }
        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }

        private void popupWindowShowActionActPrecs_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            IObjectSpace objectSpace = Application.CreateObjectSpace();
            ProductoFiltroPrecio fil = objectSpace.CreateObject<ProductoFiltroPrecio>();

            e.View = Application.CreateDetailView(objectSpace, "ProductoFiltroPrecio_DetailView", true, fil);
        }

        private void popupWindowShowActionActPrecs_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            ProductoFiltroPrecio dlgFil = e.PopupWindowViewCurrentObject as ProductoFiltroPrecio;


            if (dlgFil != null)
            {
                IObjectSpace objectSpace = Application.CreateObjectSpace();
                GroupOperator fil = new GroupOperator();

                fil.Operands.Clear();

                IList prds = objectSpace.CreateCollection(typeof(Producto), fil);
                foreach (Producto prd in prds)
                {
                    if (prd != null)
                    {
                        if (prd.CostoUltimo > 0 && prd.IncrementoP > 0)
                            prd.PrecioPublico = prd.CostoUltimo * Convert.ToDecimal(prd.IncrementoP/100 + 1);
                    }
                    /*
                    if (prd != null && prd.Precios != null && prd.Precios.Count > 0)
                    {
                        foreach (ProductoPrecios prc in prd.Precios)
                        {
                            prc.Prc = prd.CostoUltimo * Convert.ToDecimal(prc.Incrmnt + 1.0);
                        }
                    }*/
                }
                objectSpace.CommitChanges();
            }
        }
    }
}
