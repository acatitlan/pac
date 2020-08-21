using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using Escul.Module.BusinessObjects;
using Escul.Module.Utilerias;

namespace Escul.Module.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class VCHorario : ViewController
    {
        public VCHorario()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.

            TargetObjectType = typeof(Horario);
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.
            if (View is DetailView)
                View.ObjectSpace.Committing += ObjectSpace_Committing;
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
        }
        protected override void OnDeactivated()
        {
            if (View is DetailView)
                View.ObjectSpace.Committing -= ObjectSpace_Committing;

            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }

        void ObjectSpace_Committing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (View != null)
            {
                Horario obj = View.CurrentObject as Horario;

                Negocio.HorasSemana(obj.MtrGrp);
                Negocio.HorasCalendario(obj.MtrGrp);
                Negocio.FechasAplicacionTemas(obj.MtrGrp);
            }
        }

    }
}
