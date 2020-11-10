using DevExpress.ExpressApp;
using Cap.Bancos.BusinessObjects;
using Cap.Bancos.Utilerias;

namespace Cap.Bancos.Controllers
{
    // For more typical usage scenarios, be sure to check out http://documentation.devexpress.com/#Xaf/clsDevExpressExpressAppViewControllertopic.
    public partial class VCConceptoSave : ViewController
    {
        public VCConceptoSave()
        {
            InitializeComponent();
            RegisterActions(components);
            // Target required Views (via the TargetXXX properties) and create their Actions.
            TargetObjectType = typeof(ConceptoB);
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
                ConceptoB obj = View.CurrentObject as ConceptoB;

                if (obj != null)
                    e.Cancel = !NegocioBancos.GrabaConcepto(obj);
            }
        }
    }
}
