using DevExpress.Xpo;
using System.ComponentModel;
using DevExpress.ExpressApp.DC;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using Cap.Generales.BusinessObjects.Object;

namespace Cap.Ventas.BusinessObjects
{
    //[DefaultClassOptions]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class RelacionadoUuid : PObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public RelacionadoUuid(Session session)
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
        //    set { SetPropertyValue("PersistentProperty", ref _PersistentProperty, value); }
        //}

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}


        private DocumentoSalida mDcmnt;
        [XafDisplayName("Documento")]
        [VisibleInDetailView(false)]
        [Association("DocumentoSalida-RelacionadosUuid")]
        public DocumentoSalida Dcmnt
        {
            get { return mDcmnt; }
            set { SetPropertyValue("Dcmnt", ref mDcmnt, value); }
        }

        private Cxc mRlcnd;
        [DataSourceProperty("Relacionados")]
        [XafDisplayName("Relacionado")]
        public Cxc Rlcnd
        {
            get { return mRlcnd; }
            set { SetPropertyValue("Rlcnd", ref mRlcnd, value); }
        }


        private XPCollection<Cxc> posiblesRelacionados;
        [Browsable(false)] // Prohibits showing the collection separately 
        public XPCollection<Cxc> Relacionados
        {
            get
            {
                if (posiblesRelacionados == null)
                    posiblesRelacionados = new XPCollection<Cxc>(Session);

                RefrescarDocumentos();

                return posiblesRelacionados;
            }
        }

        private void RefrescarDocumentos()
        {
            if (posiblesRelacionados == null)
                return;

            GroupOperator fil = new GroupOperator();

            fil.Operands.Add(new BinaryOperator("Cepto.Tipo", EConceptoTipo.Abono));
            fil.Operands.Add(new BinaryOperator("Stts", EEstadoDcmnt.Cancelado, BinaryOperatorType.NotEqual));

            if (Dcmnt != null && Dcmnt.Cliente != null)
                fil.Operands.Add(new BinaryOperator("Cliente", Dcmnt.Cliente));
            posiblesRelacionados.Criteria = fil;
        }
    }
}