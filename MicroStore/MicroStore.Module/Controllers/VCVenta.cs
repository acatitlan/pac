using System;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.ReportsV2;
using DevExpress.Persistent.BaseImpl;
using MicroStore.Module.BusinessObjects;
using MicroStore.Module.Utilerias;

namespace MicroStore.Module.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class VCVenta : ViewController
    {
        bool imprmTck = false;
        public VCVenta()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
            TargetObjectType = typeof(Venta);

            simpleActionAddItm.TargetObjectType = typeof(Venta);
            simpleActionCnclr.TargetObjectsCriteria = "Stts == 'Alta' && !IsNewObject(This)";
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.
            if (View is DetailView)
            {
                View.ObjectSpace.Committing += ObjectSpace_Committing;
                View.ObjectSpace.Committed += ObjectSpace_Committed;

                
                Venta vnt = View.CurrentObject as Venta;
                if (vnt != null)
                {
                    vnt.ItemsVenta.ListChanged += ItemsVenta_ListChanged;
                }


                Parametros prm = View.ObjectSpace.FindObject<Parametros>(null);

                if (prm != null)
                    imprmTck = prm.ImprmrTckt;
            }
        }

        //* Bien,  lo hicimos en el objeto, qué será mejor? Lo hicimos así en factura
        private void ItemsVenta_ListChanged(object sender, System.ComponentModel.ListChangedEventArgs e)
        {
            if (View != null && View.ObjectSpace != null 
                && !View.ObjectSpace.IsCommitting)
            {
                Venta vnt = View.CurrentObject as Venta;

                vnt.Stts = EEstadoVenta.Alta;
                vnt.ItemsVenta.ListChanged -= ItemsVenta_ListChanged;
                Negocio.CalculaTotal(vnt);
                vnt.ItemsVenta.ListChanged += ItemsVenta_ListChanged;
            }
        }

        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
            if (View != null && View.CurrentObject != null)
            {
                Venta obj = View.CurrentObject as Venta;
                if (View.ObjectSpace.IsNewObject(obj))
                    Negocio.IniciaVenta(obj, null);
            }
        }
        protected override void OnDeactivated()
        {
            View.ObjectSpace.Committing -= ObjectSpace_Committing;
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }

        void ObjectSpace_Committing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (View != null && View.CurrentObject != null)
            {
                Venta doc = View.CurrentObject as Venta;
                if (View.ObjectSpace.IsNewObject(doc))
                    Negocio.GrabaVenta(doc, null);
            }
        }

        void ObjectSpace_Committed(object sender, EventArgs e)
        {
            // View.ObjectSpace.Committed -= ObjectSpace_Committed; 
            if (imprmTck)
            {
                CriteriaOperator fil = new BinaryOperator("Oid", (View.CurrentObject as BaseObject).Oid);
                Imprime("Ticket", fil);
            }
        }

        private void simpleActionAddItm_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            if (View != null && View.CurrentObject != null)
            {
                Venta doc = View.CurrentObject as Venta;

                /*Sep 2020
                doc.AddItem();*/

                /*
                // View.ObjectSpace.Refresh();
                DetailView dv = View as DetailView;
                ((ListPropertyEditor)dv.FindItem("ItemsVenta")).ListView.CollectionSource.Reload();
                //OR  
                ((ListPropertyEditor)dv.FindItem("ItemsVenta")).ListView.ObjectSpace.Refresh();*/
            }
        }

        private void simpleActionCnclr_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            if (View != null && View.CurrentObject != null)
            {
                Venta doc = View.CurrentObject as Venta;
                /*
                if (View.ObjectSpace.IsNewObject(doc))
                {*/
                Negocio.CancelaVenta(doc, null);

                View.ObjectSpace.CommitChanges();
            }
        }

        private void Imprime(string name, CriteriaOperator fil)
        {
            IObjectSpace objectSpace = ReportDataProvider.ReportObjectSpaceProvider.CreateObjectSpace(typeof(ReportDataV2));
            IReportDataV2 reportData = objectSpace.FindObject<ReportDataV2>(
                new BinaryOperator("DisplayName", name));

            if (reportData == null)
            {
                throw new UserFriendlyException(string.Format("No encuentro el {0}", name));
            }
            else
            {
                string reportContainerHandler = ReportDataProvider.ReportsStorage.GetReportContainerHandle(reportData);
                /*No Mdi, si uso el frame y al dar guardar y cerrar también cierra la vista previa (*/
                Application.MainWindow.GetController
                /*Frame.GetController*/<ReportServiceController>().ShowPreview(reportContainerHandler, fil);
            }
        }
    }
}
