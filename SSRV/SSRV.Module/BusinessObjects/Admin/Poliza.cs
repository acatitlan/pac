using System;
using DevExpress.Xpo;
using System.ComponentModel;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Validation;
using Cap.Generales.BusinessObjects.Object;
using Cap.Clientes.BusinessObjects.Clientes;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.SystemModule;

namespace SSRV.Module.BusinessObjects.Admin
{
    /* Esto hace que en el loockup no se vean las pólizas TIT Mar 2021
    [ListViewFilter("Poliza.Activas", "Stts = 'Activa'", "Activas", true, Index = 0)]
    [ListViewFilter("Poliza.Suspendidas", "Stts = 'Suspendida'", "Suspendidas", true, Index = 1)]
    [ListViewFilter("Poliza.Terminadas", "HrsRstnts <= 0", "Terminadas", true, Index = 2)]*/
    [Appearance("Poliza.Fin", TargetItems = "*", Context = "ListView", Criteria = "HrsRstnts <= 0 || Stts = 'Suspendida'", FontColor = "tomato",
        FontStyle = System.Drawing.FontStyle.Strikeout)]
    [NavigationItem("Servicios")]
    [DefaultClassOptions]
    [DefaultProperty("DisplayLook")]
    //[ImageName("BO_Contact")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class Poliza : PObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public Poliza(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).

            FchAlt = DateTime.Today;
            mHrs = 1;
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

        private string mClv;
        [RuleRequiredField("RuleRequiredField for Poliza.Clv", DefaultContexts.Save, "Debe capturar la Clave", SkipNullOrEmptyValues = false)]
        [XafDisplayName("Clave")]
        [Size(12)]
        public string Clv
        {
            get { return mClv; }
            set
            {
                if (IsLoading)
                    SetPropertyValue("Clv", ref mClv, value);
                else
                    SetPropertyValue("Clave", ref mClv, ClaveFto(value, 12));
            }
        }

        private Cliente mClnt;
        [RuleRequiredField("RuleRequiredField for Poliza.Clnt", DefaultContexts.Save, "Debe capturar el Cliente", SkipNullOrEmptyValues = false)]
        [XafDisplayName("Cliente")]
        public Cliente Clnt
        {
            get { return mClnt; }
            set { SetPropertyValue("Clnt", ref mClnt, value); }
        }

        private DateTime mFchAlt;
        [ModelDefault("DisplayFormat", "{0:dd MMM yyyy}")]
        [XafDisplayName("Fecha Alta")]
        public DateTime FchAlt
        {
            get { return mFchAlt; }
            set { SetPropertyValue("FchAlt", ref mFchAlt, value); }
        }

        private ESTTSPLZ mStts;
        [XafDisplayName("Status")]
        public ESTTSPLZ Stts
        {
            get { return mStts; }
            set { SetPropertyValue("Stts", ref mStts, value); }
        }

        private string mRfrnc;
        [Obsolete("Para qué es esto ?")]
        [XafDisplayName("Referencia")]
        [Size(12)]
        public string Rfrnc
        {
            get { return mRfrnc; }
            set { SetPropertyValue("Rfrnc", ref mRfrnc, value); }
        }

        private DateTime mFchRfrnc;
        [Obsolete("Para qué es esto ?")]
        [ModelDefault("DisplayFormat", "{0:dd MMM yyyy}")]
        [XafDisplayName("Fecha Referencia")]
        public DateTime FchRfrnc
        {
            get { return mFchRfrnc; }
            set { SetPropertyValue("FchRfrnc", ref mFchRfrnc, value); }
        }

        private decimal mImprtRfrnc;
        [Obsolete("Para qué es esto ?")]
        [XafDisplayName("Importe Referencia")]
        public decimal ImprtRfrnc
        {
            get { return mImprtRfrnc; }
            set { SetPropertyValue("ImprtRfrnc", ref mImprtRfrnc, value); }
        }

        private decimal mPgdRfrnc;
        [XafDisplayName("Importe Pagado")]
        public decimal PgdRfrnc
        {
            get { return mPgdRfrnc; }
            set { SetPropertyValue("PgdRfrnc", ref mPgdRfrnc, value); }
        }

        private float mHrs;
        [ImmediatePostData]
        [XafDisplayName("Horas Asignadas")]
        public float Hrs
        {
            get { return mHrs; }
            set { SetPropertyValue("Hrs", ref mHrs, value); }
        }

        private float mHrsUsds;
        [ImmediatePostData]
        [XafDisplayName("Horas Usadas")]
        public float HrsUsds
        {
            get { return mHrsUsds; }
            set { SetPropertyValue("HrsUsds", ref mHrsUsds, value); }
        }

        //private float mHrsRstnts;
        [VisibleInLookupListView(true)]
        [XafDisplayName("Horas Restantes")]
        public float HrsRstnts
        {
            get { return mHrs - mHrsUsds; /*mHrsRstnts;*/ }
            // set { SetPropertyValue("HrsRstnts", ref mHrsRstnts, value); }
        }

        private string mNts;
        [Size(SizeAttribute.Unlimited)]
        [XafDisplayName("Notas")]
        public string Nts
        {
            get { return mNts; }
            set { SetPropertyValue("Nts", ref mNts, value); }
        }

        [Obsolete("Todavía no sé si tiene sentido")]
        [ExpandObjectMembers(ExpandObjectMembers.Always)]
        [Association("Poliza-PolizaItems", typeof(PolizaItem)), DevExpress.Xpo.Aggregated]
        public XPCollection PolizaItems
        {
            get { return GetCollection("PolizaItems"); }
        }



        [MemberDesignTimeVisibility(false)]
        [VisibleInLookupListView(false)]
        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        [NonPersistent]
        public string DisplayLook
        {
            get { return string.Format("{0} - {1}", Clv, string.IsNullOrEmpty(Nts) ?
                string.Empty : Nts.Length > 50 ? Nts.Substring(0, 5) : Nts); }
        }




        [RuleFromBoolProperty("Poliza_HrsUsdsOk", DefaultContexts.Save, "El número de horas Usadas debe ser  MENOR  O  IGUAL  al de horas asignadas")]
        protected bool Poliza_HrsUsdsOk
        {
            get { return mHrsUsds <= mHrs; }
        }


        [Action(ToolTip = "Suspende, no se muestra en las ayudas", TargetObjectsCriteria = "Stts = 'Activa'")]
        public void Suspende()
        {
            Stts = ESTTSPLZ.Suspendida;
        }

        [Action(ToolTip = "Activa, se muestra en las ayudas", TargetObjectsCriteria = "Stts = 'Suspendida'")]
        public void Activa()
        {
            Stts = ESTTSPLZ.Activa;
        }

    }

    public enum ESTTSPLZ
    {
        Activa, 
        Suspendida
    }
}