using System;
using DevExpress.Xpo;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Validation;
using Cap.Generales.BusinessObjects.Object;

namespace TCap.Module.BusinessObjects.Proyectos
{
    [NavigationItem("Proyectos")]
    [DefaultClassOptions]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class Informacion : PObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public Informacion(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).

            FchAlt = DateTime.Today;
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



        private Proyecto mPryct;
        [Association("Proyecto-Informaciones")]
        public Proyecto Pryct
        {
            get { return mPryct; }
            set
            {
                /*
                Proyecto oldPryct = mPryct;*/
                SetPropertyValue("Pryct", ref mPryct, value);
                /*
                if (!IsLoading && !IsSaving && oldPryct != mPryct)
                {
                    oldPryct = oldPryct ?? mPryct;
                    oldPryct.UpdateHrsEstmdsTtl(true);
                }*/
            }
        }

        private string mAsnt;
        //Como lo asigno al agregar una nueva tarea, puede ser que no lo quiera dar de alta
        // y por lo tanto no lleva asunto !  Nov 2020
        //[RuleRequiredField("RuleRequiredField for Informacion.Asnt", DefaultContexts.Save, "Debe capturar el Asunto", SkipNullOrEmptyValues = false)]
        [Size(150)]
        [XafDisplayName("Asunto")]
        public string Asnt
        {
            get { return mAsnt; }
            set { SetPropertyValue("Asnt", ref mAsnt, value); }
        }

        /*
        private TASKSTATUS mStts;
        [XafDisplayName("Status")]
        public TASKSTATUS Stts
        {
            get { return mStts; }
            set { SetPropertyValue("Stts", ref mStts, value); }
        }*/

        /*
        private Empleado mAsgnd;
        [XafDisplayName("Asignado A")]
        public Empleado Asgnd
        {
            get { return mAsgnd; }
            set { SetPropertyValue("Asgnd", ref mAsgnd, value); }
        }*/

        private DateTime mFchAlt;
        // Necesito capturar en qué fecha es la información    
        //[Appearance("Informacion.FchAlt", Context = "DetailView", Enabled = false, FontStyle = System.Drawing.FontStyle.Italic)]
        [ModelDefault("DisplayFormat", "{0:dd MMM yyyy}")]
        [XafDisplayName("Fecha Alta")]
        public DateTime FchAlt
        {
            get { return mFchAlt; }
            set { SetPropertyValue("FchAlt", ref mFchAlt, value); }
        }

        /*
        private DateTime mFchInc;
        [ModelDefault("DisplayFormat", "{0:dd MMM yyyy}")]
        [XafDisplayName("Fecha de Inicio")]
        public DateTime FchInc
        {
            get { return mFchInc; }
            set { SetPropertyValue("FchInc", ref mFchInc, value); }
        }*/

        /*
        private DateTime? mFchFn = null;
        [ModelDefault("DisplayFormat", "{0:dd MMM yyyy}")]
        [XafDisplayName("Fecha de Término")]
        public DateTime? FchFn
        {
            get { return mFchFn; }
            set { SetPropertyValue("FchFn", ref mFchFn, value); }
        }*/

        /*
        private float mHrsEstmds;
        [XafDisplayName("Horas Estimadas")]
        public float HrsEstmds
        {
            get { return mHrsEstmds; }
            set
            {
                SetPropertyValue("HrsEstmds", ref mHrsEstmds, value);
                if (!IsLoading && !IsSaving && Pryct != null)
                {
                    Pryct.UpdateHrsEstmdsTtl(true);
                }
            }
        }*/

        /*
        [Persistent("HrsRls")]
        private float? mHrsRls = null;
        [XafDisplayName("Horas Reales")]
        [PersistentAlias("mHrsRls")]
        public float? HrsRls
        {
            get
            {
                if (!IsLoading && !IsSaving && mHrsRls == null)
                    UpdateHrsTtl(false);
                return mHrsRls;
            }
        }*/

        /*
        private EPRIORITY mPrrdd;
        [XafDisplayName("Prioridad")]
        public EPRIORITY Prrdd
        {
            get { return mPrrdd; }
            set { SetPropertyValue("Prrdd", ref mPrrdd, value); }
        }*/

        /*
        private Catalogo mTp;
        [Obsolete("En su lugar usar TpP")]
        [DataSourceCriteria("Tipo == 'Actividad'")]
        [XafDisplayName("Tipo")]
        public Catalogo Tp
        {
            get { return mTp; }
            set { SetPropertyValue("Tp", ref mTp, value); }
        }*/

        private CatalogoProyecto mTpP;
        [DataSourceCriteria("Tp == 'Actividad'")]
        [XafDisplayName("Tipo")]
        public CatalogoProyecto TpP
        {
            get { return mTpP; }
            set { SetPropertyValue("TpP", ref mTpP, value); }
        }

        private string _Text; // mNts;
        [EditorAlias("RTF")]
        [Size(SizeAttribute.Unlimited)]
        [XafDisplayName("Notas")]
        public string Nts
        {
            get
            {
                /*
                RichEditDocumentServer server = new RichEditDocumentServer();
                server.Text = value;
                return server.RtfText*/
                if (_Text == null) return _Text;
                if (_Text.StartsWith(@"{\rtf"))
                    return _Text;
                else return TextHelper.GetRTFText(_Text);
                // return mNts;
            }
            set
            {
                if (value != null && value.StartsWith(@"{\rtf"))
                    SetPropertyValue("Text", ref _Text, value);
                else
                    SetPropertyValue("Text", ref _Text, TextHelper.GetRTFText(value));
                //SetPropertyValue("Nts", ref mNts, value);
            }
        }

        /*
        [Association("Actividad-Incidencias", typeof(Incidencia)), DevExpress.Xpo.Aggregated]
        public XPCollection Incidencias
        {
            get { return GetCollection("Incidencias"); }
        }*/

        [Association("Informacion-ExpedienteArchivos", typeof(ExpedienteArchivo)), DevExpress.Xpo.Aggregated]
        public XPCollection Archivos
        {
            get { return GetCollection("Archivos"); }
        }

        /*
        public void UpdateHrsTtl(bool forceChangeEvents)
        {
            float? oldHrsTtl = mHrsRls;
            float tempTotal = 0;

            foreach (Incidencia ac in Incidencias)
            {
                tempTotal += ac.Hrs;
            }
            mHrsRls = tempTotal;

            if (forceChangeEvents)
                OnChanged("HrsRls", oldHrsTtl, mHrsRls);

            if (Pryct != null)
                Pryct.UpdateHrsTtl(forceChangeEvents);
        }*/



        /*
        [RuleFromBoolProperty("Actividad.FchIniFin", DefaultContexts.Save, "La fecha Final debe ser mayor o igual a la de Inicio")]
        protected bool FchsOk
        {
            get { return mFchFn == null || (mFchFn >= mFchInc); }
        }*/

        /*
        [RuleFromBoolProperty("Actividad.HrsEstmds", DefaultContexts.Save, "El tiempo estimado debe ser mayor o igual a CERO")]
        protected bool HrsEstmdsOk
        {
            get { return mHrsEstmds >= 0; }
        }*/
    }
}