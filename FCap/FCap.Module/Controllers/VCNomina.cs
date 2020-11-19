using System;
using DevExpress.ExpressApp;
using Nomina.Utilerias;

namespace FCap.Module.Controllers.NominaF
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class VCNomina : ViewController
    {
        public VCNomina()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
            TargetObjectType = typeof(Cap.Nomina.BusinessObjects.Nomina);
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.
            if (View != null && View is DetailView && View.ObjectSpace != null)
                View.ObjectSpace.Committing += ObjectSpace_Committing;
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
        }
        protected override void OnDeactivated()
        {
            if (View != null && View is DetailView && View.ObjectSpace != null)
                View.ObjectSpace.Committing -= ObjectSpace_Committing;

            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }

        private void VCNomina_ViewControlsCreated(object sender, EventArgs e)
        {
            if (View != null)
            {
                Cap.Nomina.BusinessObjects.Nomina doc = View.CurrentObject as Cap.Nomina.BusinessObjects.Nomina;

                if (View.ObjectSpace != null && View.ObjectSpace.IsNewObject(doc))
                    NegocioNom.IniciaNom(doc);
            }
        }


        private void ObjectSpace_Committing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (View != null)
            {
                Cap.Nomina.BusinessObjects.Nomina obj = View.CurrentObject 
                    as Cap.Nomina.BusinessObjects.Nomina;

                if (obj != null && View.ObjectSpace.IsNewObject(obj))
                {
                    NegocioNom.GrabaNom(obj);
                    e.Cancel = false;
                }
            }            
        }
    }
}