using DevExpress.Xpo;
using DevExpress.ExpressApp.DC;
using Cap.Generales.BusinessObjects.Object;
using Cap.Clientes.BusinessObjects.Generales;

namespace Cap.Clientes.BusinessObjects.Clientes
{
    // [DefaultClassOptions]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class AnuncioFiltro : PObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public AnuncioFiltro(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }
        //private string _PersistentProperty;
        //[XafDisplayName("My display name"), ToolTip("My hint message")]
        //[ModelDefault("EditMask", "(000)-00"), Index(0), VisibleInListView(false)]
        //[Persistent("DatabaseColumnName"), RuleRequiredField(DefaultContexts.Save)]
        //public string PersistentProperty {
        //    get { return _PersistentProperty; }
        //    set { SetPropertyValue(nameof(PersistentProperty), ref _PersistentProperty, value); }
        //}

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}

        [XafDisplayName("Identificador")]
        [Size(40)]
        public string Idntfcdr { get; set; }

        [XafDisplayName("Asunto")]
        [Size(80)]
        public string Asnt { get; set; }

        [XafDisplayName("Clave")]
        [Size(15)]
        public string Clv { get; set; }

        [XafDisplayName("ClienteProspecto")]
        public CatalogoCliente ClntPrspct { get; set; }

        [XafDisplayName("Clasificacion")]
        public Clasificacion Clsfccn { get; set; }

        [XafDisplayName("Estado")]
        public EStatusPrvdClnt? Stts { get; set; }
    }
}