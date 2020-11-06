using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using Cap.Generales.BusinessObjects.Object;
using DevExpress.Persistent.Validation;
using DevExpress.ExpressApp.DC;
using System.ComponentModel;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.Editors;

namespace Cap.Clientes.BusinessObjects.Generales
{
    /* Nov 2020   Como Catalogo ya tiene un Tipo, y como necesito otro tipos para este 
     clasificador entonces tendré que derivarlo de PObject ! */
    [NavigationItem("Clientes")]
    [DefaultClassOptions]
    //[ImageName("BO_Contact")]
    [DefaultProperty("Dscrpcn")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class CatalogoCliente : PObject // Catalogo
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public CatalogoCliente(Session session)
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

        private string mDscrpcn;
        [RuleRequiredField("RuleRequiredField for CatalogoCliente.Dscrpcn", 
            DefaultContexts.Save, "Debe capturar la Descripción", 
            SkipNullOrEmptyValues = false)]
        [XafDisplayName("Descripción")]
        [Size(80)]   
        public string Dscrpcn
        {
            get { return mDscrpcn; }
            set { SetPropertyValue("Dscrpcn", ref mDscrpcn, value); }
        }

        private ETipoCatalogoCliente mTipo;
        virtual public ETipoCatalogoCliente Tipo
        {
            get { return mTipo; }
            set { SetPropertyValue("Tipo", ref mTipo, value); }
        }

        private bool mEnVnts;
        [Appearance("CatalogoCliente.EnVnts", AppearanceItemType = "LayoutItem", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "Tipo != 'ClienteProspecto'")]
        [XafDisplayName("EnVentas")]
        public bool EnVnts
        {
            get { return mEnVnts; }
            set { SetPropertyValue("EnVnts", ref mEnVnts, value); }
        }
    }

    public enum ETipoCatalogoCliente
    {
        ClienteProspecto
    }
}