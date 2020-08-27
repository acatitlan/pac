using System;
using DevExpress.Xpo;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Model;
using Cap.Generales.BusinessObjects.Object;
using Cap.Generales.BusinessObjects.General;

namespace TCap.Module.BusinessObjects.Proyectos
{
    //[NonPersistent]
    //[DefaultClassOptions]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class ActividadCompletada : PObject, ISingleton
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public ActividadCompletada(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).

            //FchTrmncn = DateTime.Today;
            //Stts = TASKSTATUS.Completada;
            Hrs = 1;
        }
        //private string _PersistentProperty;
        //[XafDisplayName("My display name"), ToolTip("My hint message")]
        //[ModelDefault("EditMask", "(000)-00"), Index(0), VisibleInListView(false)]
        //[Persistent("DatabaseColumnName"), RuleRequiredField(DefaultContexts.Save)]
        //public string PersistentProperty {
        //    get { return _PersistentProperty; }
        //    set { SetPropertyValue("PersistentProperty", ref _PersistentProperty, value); }
        //}

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}



        [ModelDefault("DisplayFormat", "{0:dd MMM yyyy}")]
        [XafDisplayName("Fecha de Terminacion")]
        public DateTime FchTrmncn { get; set; }

        [XafDisplayName("Status")]
        public TASKSTATUS Stts { get { return TASKSTATUS.Completada; } }

        [XafDisplayName("Tiempo (Hrs)")]
        public float Hrs;

        [EditorAlias("RTF")]
        [Size(SizeAttribute.Unlimited)]
        [XafDisplayName("Descripción")]
        public string Dscrpcn { get; set; }
    }
}