using DevExpress.Xpo;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using Cap.Generales.BusinessObjects.Object;
using Cap.Generales.BusinessObjects.General;

namespace Cap.Inventarios.BusinessObjects
{
    //[DefaultClassOptions]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class InvFisImporta : PObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public InvFisImporta(Session session)
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

        [RuleRequiredField("RuleRequiredField for InvFisImporta.InputFile", DefaultContexts.Save, "Debe capturar el Archivo", SkipNullOrEmptyValues = false)]
        [XafDisplayName("Archivo a Cargar")]
        [FileTypeFilter("Excel", 1, "*.xlsx")]
        public MyFileData InputFile { get; set; }

        [RuleRequiredField("RuleRequiredField for InvFisImporta.PscnClv", DefaultContexts.Save, "Debe capturar la posición de la Clave", SkipNullOrEmptyValues = false)]
        [XafDisplayName("Clave")]
        public string PscnClv { get; set; }
        
        [XafDisplayName("Descripción")]
        public string PscnDscrpcn { get; set; }

        [XafDisplayName("Precio P.")]
        public string PscnPrcP { get; set; }

        [RuleRequiredField("RuleRequiredField for InvFisImporta.PscnCntdd", DefaultContexts.Save, "Debe capturar la posición de la Cantidad", SkipNullOrEmptyValues = false)]
        [XafDisplayName("Cantidad")]
        public string PscnCntdd { get; set; }

        [XafDisplayName("Lote")]
        public string PscnLt { get; set; }

        [XafDisplayName("Fecha Caducidad")]
        public string PscnFchCdcdd { get; set; }
    }
}