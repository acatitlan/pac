using Cap.Clientes.BusinessObjects.Clientes;
using Cap.Clientes.BusinessObjects.Generales;
using Cap.Generales.BusinessObjects.Unidades;
using Cap.Proveedores.BusinessObjects.Proveedores;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.Drawing;

namespace Cap.Inventarios.BusinessObjects
{
    [Appearance("MovimientoI.Costo", AppearanceItemType = "ViewItem", TargetItems = "Cst", Context = "ListView", Enabled = false, Criteria = "[Cst] > [CstPrdct]", FontColor = "Green")]
    [Appearance("MovimientoI.Edit", AppearanceItemType = "ViewItem", TargetItems = "*", Context = "DetailView", Enabled = false, Criteria = "!IsNewObject(This)", FontStyle = FontStyle.Italic)]
    [NavigationItem("Inventarios")]
    public partial class MovimientoI
    {
        private Producto mPrdct;
        [RuleRequiredField("RuleRequiredField for MovimientoI.Prdct", DefaultContexts.Save, "Debe capturar el Producto", SkipNullOrEmptyValues = false)]
        [DisplayName("Producto")]
        public Producto Prdct
        {
            get { return mPrdct; }
            set
            {
                SetPropertyValue("Prdct", ref mPrdct, value);
                // Yo digo que en Negocio hay que hacer esto
                if (IsNewObject() && value != null)
                {
                    Precio = value.PrecioPublico;
                    if (Cncpt /*Concepto*/ != null)
                    {
                        /* May 2013 Revisar esto
                        if (Concepto.EsSalida())
                            Costo = value.CostoPromedio;
                        else*/
                        Cst = value.CostoUltimo;
                        CstPrdct = value.CostoUltimo;
                    }
                    Costeo = value.Costeo;
                }
            }
        }

        private DateTime mFch;
        [ModelDefault("DisplayFormat", "{0:dd MMM yyyy}")]
        [RuleRequiredField("RuleRequiredField for MovimientoI.Fch", DefaultContexts.Save, "Debe capturar la Fecha", SkipNullOrEmptyValues = false)]
        [DisplayName("Fecha")]
        public DateTime Fch
        {
            get { return mFch; }
            set { SetPropertyValue("Fch", ref mFch, value); }
        }

        // Por qué lo dejé flotante si debería ser doble !
        // Lo cambio a ver qué pasa ! Ene 20
        private /*float*/double mCntdd;
        // No jalo no sé por qué 
        [ModelDefault("DisplayFormat", "{0:N2}")]
        [DisplayName("Cantidad")]
        public /*float*/double Cntdd
        {
            get { return mCntdd; }
            set { SetPropertyValue("Cntdd", ref mCntdd, value); }
        }

        private ConceptoMI mCncpt;
        [ImmediatePostData]
        [RuleRequiredField("RuleRequiredField for MovimientoI.Cncpt", DefaultContexts.Save, "Debe capturar el Concepto", SkipNullOrEmptyValues = false)]
        [DisplayName("Concepto")]
        public ConceptoMI Cncpt
        {
            get { return mCncpt; }
            set
            {
                SetPropertyValue("Cncpt", ref mCncpt, value);
                if (value != null && IsNewObject())
                {
                    // May 2013 Revisar esto
                    if (/*value.UsaCliente() &&*/ Cliente == null && Causante is Cliente)
                        Cliente = Causante as Cliente;
                    else if (/*value.UsaProveedor() &&*/ Prvdr /*Proveedor*/ == null && Causante is Proveedor)
                        Prvdr /*Proveedor*/ = Causante as Proveedor;
                }
            }
        }

        private Proveedor mPrvdr;
        [VisibleInListView(false)]
        [Appearance("MovimientoI.Prvdr", AppearanceItemType = "LayoutItem", Context = "DetailView", Criteria = "Cncpt.Csnt != 'Proveedor'", Visibility = ViewItemVisibility.Hide)]
        [DisplayName("Proveedor")]
        public Proveedor Prvdr
        {
            get { return mPrvdr; }
            set { SetPropertyValue("Prvdr", ref mPrvdr, value); }
        }

        private decimal mCst;
        [Appearance("MovimientoI.Cst", AppearanceItemType = "LayoutItem", Context = "DetailView", Criteria = "IsNewObject(This)", Visibility = ViewItemVisibility.Hide)]
        [DisplayName("Costo")]
        public decimal Cst
        {
            get { return mCst; }
            set { SetPropertyValue("Cst", ref mCst, value); }
        }

        private Lote mLt;
        [Appearance("MovimientoI.Lt", AppearanceItemType = "LayoutItem", Context = "DetailView", Criteria = "!Prdct.Lotes", Visibility = ViewItemVisibility.Hide)]
        [DisplayName("Lote")]
        public Lote Lt
        {
            get { return mLt; }
            set { SetPropertyValue("Lt", ref mLt, value); }
        }

        [ModelDefault("DisplayFormat", "{0:dd MMM yyyy}")]
        [Appearance("MovimientoI.FchCdcdd", AppearanceItemType = "LayoutItem", Context = "DetailView", Criteria = "!Prdct.Lotes", Visibility = ViewItemVisibility.Hide)]
        [XafDisplayName("Fecha Caducidad")]
        public DateTime? FchCdcdd
        {
            get { return Lt == null ? null : Lt.FchCdcdd; }
        }

        /* Oct 2018
        #region + Lotes
        [VisibleInDetailView(false)]
        [Association("MovimientoI-Lotes", typeof(Lote)), DevExpress.Xpo.Aggregated]
        public XPCollection Lotes
        {
            get { return GetCollection("Lotes"); }
        }
        #endregion*/


        private XPDelayedProperty mNts = new XPDelayedProperty();
        [Delayed("mNts", true)]
        [Size(SizeAttribute.Unlimited)]
        [DisplayName("Notas")]
        public string Nts
        {
            get { return (string)mNts.Value; }
            set { mNts.Value = value; }
        }


        const ushort LONDOC = 10; // 16;

        private string FDocumento;
        [RuleRequiredField("RuleRequiredField for Movimiento.Documento", 
            DefaultContexts.Save, "Debe capturar el Documento", SkipNullOrEmptyValues = false)]
        [Size(LONDOC)]
        public string Documento
        {
            get { return FDocumento; }
            set { SetPropertyValue("Documento", ref FDocumento, ValorString("Documento", value)); }
        }


        //#region + Secuencia
        // Se refiere al número común de varios movimientos de partida en el documento que genera el movimiento
        private short FItem;
        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        public short Item
        {
            get { return FItem; }
            set { SetPropertyValue("Item", ref FItem, value); }
        }

        //#region + Fecha de alta
        /* PObject Ya tiene un campo CreadoEl: FechaAlta*/
        /*
        private DateTime FFAlta;
        [Obsolete("En su lugar usar 'CreadoEl'")]
        [Appearance("FechaAlta", Context = "DetailView", Enabled = false, FontStyle = FontStyle.Italic)]
        [ModelDefault("DisplayFormat", "{0:dd MMM yy}")]
        public DateTime FechaAlta
        {
            get { return FFAlta; }
            set { SetPropertyValue("FechaAlta", ref FFAlta, value); }
        }*/

        private Cliente FCliente;
        [Appearance("Cliente", AppearanceItemType = "LayoutItem", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "Cncpt.Csnt != 'Cliente'")]
        [VisibleInListView(false)]
        public Cliente Cliente
        {
            get { return FCliente; }
            set { SetPropertyValue("Cliente", ref FCliente, value); }
        }

        // Creo que esto no se puede hacer, porque ProveedorCliente es 'abstracta'
        private ProveedorCliente FCausante;
        [VisibleInDetailView(false)]
        // [Appearance("CausanteV", AppearanceItemType = "LayoutItem", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "IsNewObject(This)")]
        // [Appearance("Causante", Context = "DetailView", Enabled = false, FontStyle = FontStyle.Italic)]
        [NonPersistent]
        public ProveedorCliente Causante
        {
            get
            {
                if (Cliente != null)
                    return Cliente;
                if (Prvdr != null)
                    return Prvdr;

                return null;
            }
            set
            {
                FCausante = value;
                if (Cncpt /*Concepto*/ != null && IsNewObject())
                {
                    // May 2013 Revisar esta parte !
                    if (/*Concepto.UsaCliente() && */Cliente == null && value is Cliente)
                        Cliente = value as Cliente;
                    else if (/*Concepto.UsaProveedor() && */Prvdr /*Proveedor*/ == null && value is Proveedor)
                        Prvdr /*Proveedor*/ = value as Proveedor;
                }
            }
        }

        private Proveedor FProveedor;
        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        public Proveedor Proveedor
        {
            get { return FProveedor; }
            set { SetPropertyValue("Proveedor", ref FProveedor, value); }
        }

        // Será que esto es necesario?, no es suficiente con el inventario?
        private Almacen FAlmacen;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        public Almacen Almacen
        {
            get { return FAlmacen; }
            set { SetPropertyValue("Almacen", ref FAlmacen, value); }
        }

        private AlmacenProducto FInventario;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        public AlmacenProducto Inventario
        {
            get { return FInventario; }
            set { SetPropertyValue("Inventario", ref FInventario, value); }
        }

        //#region + Por costear, cantidad
        // Del sae, pero creo que no es buena idea
        private double FPorCostear;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        public double PorCostear
        {
            get { return FPorCostear; }
            set { SetPropertyValue("PorCostear", ref FPorCostear, value); }
        }

        private Unidad FUnidad;
        public Unidad Unidad
        {
            get { return FUnidad; }
            set { SetPropertyValue("Unidad", ref FUnidad, value); }
        }

        private decimal FPrecio;
        [Appearance("MovimientoI.Precio", AppearanceItemType = "LayoutItem", Context = "DetailView", Visibility = ViewItemVisibility.Hide)]
        [VisibleInListView(false)]
        public decimal Precio
        {
            get { return FPrecio; }
            set { SetPropertyValue("Precio", ref FPrecio, value); }
        }

        //#region + Monto, no persistente, sólo para la captura
        // Usaba esto para la captura, según el concepto salida entrada
        // usaba el precio o el costo
        // pero parece que siempre hay que poner el costo !
        //
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [NonPersistent]
        public decimal Monto
        {
            get
            {
                try
                {
                    // Revisar esto May 2013
                    return /*(Concepto.EsSalida()) ? Precio :*/ Cst; // Costo;
                }
                catch (Exception)
                {
                    return Cst; // Costo;
                }
            }
            set
            {
                try
                {
                    /* May 2013 Revisar esto
                    if (Concepto.EsSalida())
                        Precio = value;
                    else*/
                    Cst /*Costo*/ = value;
                }
                catch (Exception)
                {
                    Cst /*Costo*/ = value;
                }
            }
        }

        [VisibleInListView(false)]
        [NonPersistent]
        public decimal SubTotal
        {
            get { return (decimal)Cntdd /*Cantidad*/ * Monto; }
        }

        // Creo que es para saber si viene de factura, compra o directamente movimiento.  Abr 2013 no será mejor DocumentoTipo ?
        private EMovimientoOrigen FOrigen;
        [VisibleInListView(false)]
        public EMovimientoOrigen Origen
        {
            get { return FOrigen; }
            set { SetPropertyValue("Origen", ref FOrigen, value); }
        }

        //#region + Vendedor
        private Vendedor FVendedor;
        [VisibleInListView(false)]
        public Vendedor Vendedor
        {
            get { return FVendedor; }
            set { SetPropertyValue("Vendedor", ref FVendedor, value); }

        }

        //#region + Documento Venta
        /* Será necesario Oct 2018
        private DocumentoSalida FDocSal;
        [VisibleInListView(false)]
        public DocumentoSalida DocSalida
        {
            get { return FDocSal; }
            set { SetPropertyValue("DocSalida", ref FDocSal, value); }
        }*/
        //#endregion

        //#region + Documento Compra
        /* Será necesario Oct 2018
        private DocumentoEntrada FDocEnt;
        [VisibleInListView(false)]
        public DocumentoEntrada DocEntrada
        {
            get { return FDocEnt; }
            set { SetPropertyValue("DocEntrada", ref FDocEnt, value); }
        }*/
        //#endregion

        //#region + Costeo
        // Todos los almacenes usarán el mismo costeo
        private EProductoCosteo FCosteo;
        [VisibleInListView(false)]
        public EProductoCosteo Costeo
        {
            get { return FCosteo; }
            set { SetPropertyValue("Costeo", ref FCosteo, value); }
        }

        [VisibleInListView(false)]
        [NonPersistent]
        public decimal CostoTotal
        {
            get { return (decimal)Cntdd /*Cantidad*/ * Cst /*Costo*/; }
        }

        private Moneda FMoneda;
        [VisibleInListView(false)]
        public Moneda Moneda
        {
            get { return FMoneda; }
            set
            {
                SetPropertyValue("Moneda", ref FMoneda, value);
                if (!IsLoading && value != null)
                    TipoCambio = value.MontoTC;
            }
        }

        private decimal FTipChange;
        [VisibleInListView(false)]
        [ModelDefault("DisplayFormat", "{0:c4}")]
        [ModelDefault("EditMask", "c4"), ModelDefault("EditMaskType", "Default")]
        public decimal TipoCambio
        {
            get { return FTipChange; }
            set { SetPropertyValue("TipoCambio", ref FTipChange, value); }
        }

        private decimal mCstPrdct;
        [XafDisplayName("Costo Producto")]
        [Appearance("MovimientoI.CstPrdct", Context = "DetailView", Enabled = false, FontStyle = FontStyle.Italic)]
        public decimal CstPrdct
        {
            get { return mCstPrdct; }
            set { SetPropertyValue("CstPrdct", ref mCstPrdct, value); }
        }

        [Appearance("DisableProp", AppearanceItemType = "ViewItem", Context = "DetailView", Enabled = false,
            TargetItems = "Documento; Concepto; Cantidad; Producto; Unidad; Costo; Precio; Moneda; TipoCambio; Monto; Origen; Vendedor; Costeo; Notas; DocSalida; DocEntrada")]
        protected bool IsNotNew()
        {
            return !Session.IsNewObject(this);
        }







        [RuleFromBoolProperty("MovimientoI.Cntdd", DefaultContexts.Save, "La Cantidad debe ser mayor a CERO")]
        protected bool CntddOk
        {
            get { return mCntdd > 0; }
        }

        [RuleFromBoolProperty("MovimientoI.Cst", DefaultContexts.Save, "El Costo debe ser mayor o igual a CERO")]
        protected bool CstOk
        {
            get { return mCst >= 0; }
        }

        [RuleFromBoolProperty("MovimientoI.Prvdr", DefaultContexts.Save, "Debe capturar el Proveedor")]
        protected bool PrvdrOk
        {
            get { return mCncpt == null || mCncpt.Csnt != EClienteProveedor.Proveedor || mPrvdr != null; }
        }

        //#region + Causante, cliente proveedor
        [RuleFromBoolProperty("ClienteOk", DefaultContexts.Save, "Debe capturar el Cliente")]
        protected bool ClienteOk
        {
            get { return Cncpt /*Concepto*/.Csnt /*.Causante*/ != EClienteProveedor.Cliente /*ECausanteTipo.Cliente*/ || Cliente != null; }
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();


            Fch = DateTime.Now;

            Almacen = null;
            Cntdd /*Cantidad*/ = 0;
            Cliente = null;
            Cncpt /*Concepto*/ = null;
            Cst /*Costo*/ = 0.0m;
            Documento = string.Empty;
            /* Oct 2018
            DocEntrada = null;
            DocSalida = null;*/
            /*
            FechaAlta = DateTime.Today;*/
            Inventario = null;
            Item = 0;
            Nts = string.Empty;
            Origen = EMovimientoOrigen.Otro;
            PorCostear = 0;
            Precio = 0.0m;
            Prdct /*Producto*/ = null;
            Prvdr /*Proveedor*/ = null;
            Unidad = null;
            Vendedor = null;

            Moneda = Session.FindObject<Moneda>(new BinaryOperator("Sistema", true));

            CstPrdct = 0;
        }
    }


    public enum EMovimientoOrigen
    {
        [XafDisplayName("Remisión")]
        Remision = 1,
        Factura = 2,
        [XafDisplayName("Devolución venta")]
        DevolucionVenta = 3,
        [XafDisplayName("Recepción")]
        Recepcion = 4,
        [XafDisplayName("Devolución compra")]
        DevolucionCompra = 5,
        Ticket = 6,
        Traspaso = 7,
        Otro = 8
    }
}
