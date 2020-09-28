using System;
using DevExpress.ExpressApp;
using Cap.Bancos.BusinessObjects;
using Cap.Bancos.Utilerias;

namespace Cap.Bancos.Controllers
{
    // For more typical usage scenarios, be sure to check out http://documentation.devexpress.com/#Xaf/clsDevExpressExpressAppViewControllertopic.
    public partial class VCTransferenciaSave : ViewController
    {
        public VCTransferenciaSave()
        {
            InitializeComponent();
            RegisterActions(components);
            // Target required Views (via the TargetXXX properties) and create their Actions.

            TargetObjectType = typeof(Transferencia);
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
            if (View != null)
            {
                Transferencia obj = View.CurrentObject as Transferencia;

                if (obj != null)
                    e.Cancel = !NegocioBancos.GrabaTransfer(obj);
            }
        }

        private void VCTransferenciaSave_ViewControlsCreated(object sender, EventArgs e)
        {
            if (View != null && View.CurrentObject != null)
            {
                Transferencia obj = View.CurrentObject as Transferencia;

                obj.FecApli = DateTime.Today;
            }
        }
    }
}
