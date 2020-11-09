using System;
using DevExpress.Xpo;
using System.ComponentModel;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Validation;
using DevExpress.ExpressApp.ConditionalAppearance;
using Cap.Generales.BusinessObjects.Object;
using Cap.Personas.BusinessObjects;
using DevExpress.ExpressApp.DC;

namespace Cap.Clientes.BusinessObjects.Clientes
{
    [NavigationItem("Clientes")]
    [Appearance("Suspendido", TargetItems = "*", Context = "ListView", Criteria = "[Status] == 'Suspendido'", FontColor = "Green"/*, FontStyle = FontStyle.Strikeout*/)]
    [ImageName("Manager")]
    [DefaultProperty("DisplayLook")]
    [DefaultClassOptions]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class Vendedor : PObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public Vendedor(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).


            Clave = string.Empty;
            // Otra forma de asignar la clave, lo malo de recorrer todo la tabla es:
            // recorrerla, y si quiero que empiece o se salte a la 100 digamos ya no podré.
            // Clave = UltimaClave(typeof(Vendedor), "Clave", null);
            /*
            AsignaClave();*/

            // Clave = XpoSiteId.GetNextSitedValue(Session.DataLayer, "Clave").ToString(); 
            Comision = 0.0f;
            /*
            Empleado = null;*/
            Nombre = string.Empty;
            Persona = null;
            Zona = null;
            Status = StatusVnd.Activo;
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



        const ushort LONCLAVE = 6;  // 5
        const ushort LONNOM = 35; // 40

        //#region Properties
        // Hay que hacer que cuando el parametro sea consecutivo tome el valor del OID
        // No porque el Oid no es consecutivo
        // [NonPersistent]
        private string FClave;
        [RuleRequiredField("RuleRequiredField for Vendedor.Clave", DefaultContexts.Save, "Debe capturar la Clave", SkipNullOrEmptyValues = false)]
        [Size(LONCLAVE), Indexed(Unique = true)]
        public string Clave
        {
            get { return FClave; }
            set
            {
                if (value != null)
                {
                    if (IsLoading)
                        SetPropertyValue("Clave", ref FClave, value);
                    else
                    {
                        SetPropertyValue("Clave", ref FClave, ValorString("Clave", value.ToUpper()));

                        if (apl.Log.Cadena.IsNumber(FClave))
                            SetPropertyValue("Clave", ref FClave, FClave.PadLeft(LONCLAVE, ' '));
                    }
                }
            }
        }

        public static string ClaveFto(string val)
        {
            string cve = val.Trim(new char[] { ' ', '\0' }).Length > LONCLAVE
                ? val.Trim(new char[] { ' ', '\0' }).Substring(0, LONCLAVE).ToUpper() : val.Trim(new char[] { ' ', '\0' }).ToUpper();

            if (apl.Log.Cadena.IsNumber(cve))
                cve = cve.PadLeft(LONCLAVE, ' ');

            return cve;
        }

        private string FNombre;
        [RuleRequiredField("RuleRequiredField for Vendedor.Nombre", DefaultContexts.Save, "Debe capturar el Nombre", SkipNullOrEmptyValues = false)]
        [Size(LONNOM)]
        public string Nombre
        {
            get { return FNombre; }
            set
            {
                if (value != null)
                    SetPropertyValue("Nombre", ref FNombre, value.Trim(new char[] { ' ', '\0' }).Length > LONNOM
                        ? value.Trim().Substring(0, LONNOM) : value.Trim(new char[] { ' ', '\0' }));
            }
        }

        //#region + Porc de comisión
        [RuleFromBoolProperty("ComisionOk", DefaultContexts.Save, "El Porcentaje de Comision no puede ser negativo o  Mayor o igual a 100")]
        protected bool ComisionOk
        {
            get { return Comision >= 0 && Comision < 100; }
        }

        private float FComision;
        [XafDisplayName("Comisión")]
        [VisibleInLookupListView(false)]
        [ModelDefault("DisplayFormat", "{0:n2}%")]
        public float Comision
        {
            get { return FComision; }
            set { SetPropertyValue("Comision", ref FComision, value); }
        }

        // Pondremos un catálogo de zonas?
        private Zona FZona;
        public Zona Zona
        {
            get { return FZona; }
            set { SetPropertyValue("Zona", ref FZona, value); }
        }

        [VisibleInLookupListView(false)]
        public StatusVnd /*Ene 2015 Tipo*/ Status
        {
            get { return GetPropertyValue<StatusVnd>("Status"); }
            set { SetPropertyValue("Status", value); }
        }

        //#region + Empleado
        /*
        private Empleado FEmpleado;
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        [VisibleInListView(false)]
        public Empleado Empleado
        {
            get { return FEmpleado; }
            set { SetPropertyValue("Empleado", ref FEmpleado, value); }
        }*/
        //#endregion

        // Si no es empleado puedo saber mas del vendedor, y a través de aqui su dirección por ejemplo.
        private Persona FPersona;
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        [VisibleInListView(false)]
        public Persona Persona
        {
            get { return FPersona; }
            set { SetPropertyValue("Persona", ref FPersona, value); }
        }

        [VisibleInLookupListView(false)]
        [VisibleInListView(false)]
        [Association("SupervisorVendedor")]
        public Vendedor Supervisor
        {
            get { return GetPropertyValue("Supervisor") as Vendedor; }
            set { SetPropertyValue("Supervisor", value); }
        }
        
        //#region + Vendedores
        [VisibleInDetailView(false)]
        [Association("SupervisorVendedor", typeof(Vendedor)), DevExpress.Xpo.Aggregated]
        public XPCollection Vendedores
        {
            get { return GetCollection("Vendedores"); }
        }


        [MemberDesignTimeVisibility(false)]
        [VisibleInLookupListView(false)]
        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        [NonPersistent]
        public string DisplayLook
        {
            get { return string.Format("{0} - {1}", Clave, Nombre); }
        }

            /*
        #region + After construction
        public override void AfterConstruction()
        {
            base.AfterConstruction();

            Clave = string.Empty;
            // Otra forma de asignar la clave, lo malo de recorrer todo la tabla es:
            // recorrerla, y si quiero que empiece o se salte a la 100 digamos ya no podré.
            // Clave = UltimaClave(typeof(Vendedor), "Clave", null);
            / *
            AsignaClave();* /

            // Clave = XpoSiteId.GetNextSitedValue(Session.DataLayer, "Clave").ToString(); 
            Comision = 0.0f;
            / *
            Empleado = null;* /
            Nombre = string.Empty;
            / *
            Persona = null;
            Zona = null;* /
            Status = StatusTipo.Activo;
        }
        #endregion*/

        // #region # override On Saving
        /*
        protected override void OnSaving()
        {
            base.OnSaving();

            if (cap.Log.Cadena.IsNumber(Clave.Trim()))
            {
                Administracion.Administracion admin = Session.FindObject(typeof(Administracion.Administracion), new BinaryOperator("Clave", "ROOT")) as Administracion.Administracion;

                admin.UltVendedor = Convert.ToUInt32(Clave);
                admin.Save();
                admin.Dispose();
                admin = null;
            }
        }*/
        // #endregion

        [NonPersistent]
        static public bool ConZona { get; set; }

        [Action(ToolTip = "Baja, no se muestra en las ayudas", TargetObjectsCriteria = "Status = 3", Caption = "Dar de Baja")]
        public void DarBaja()
        {
            Status = StatusVnd.Baja;
        }

        [Action(ToolTip = "Activa, se muestra en las ayudas", TargetObjectsCriteria = "Status = 2")]
        public void Activar()
        {
            Status = StatusVnd.Activo;
        }
    }

    public enum StatusVnd
    {
        Baja = 2,
        Activo = 3
    }
}