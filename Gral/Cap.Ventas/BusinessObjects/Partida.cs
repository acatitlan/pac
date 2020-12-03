#region Copyright (c) 2017-2019 cjlc
/*
{+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++)
{                                                                   }
{     cjlc Cap control administrativo personal                      }
{                                                                   }
{     Copyrigth (c) 2017-2019 cjlc                                  }
{     Todos los derechos reservados                                 }
{                                                                   }
{*******************************************************************}
 */
#endregion Copyright (c) 2017-2019 cjlc

 using System;
using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Validation;
using DevExpress.ExpressApp.ConditionalAppearance;
using System.Drawing;
using Cap.Generales.BusinessObjects.Object;
using Cap.Generales.BusinessObjects.Unidades;
using DevExpress.ExpressApp.DC;
using DevExpress.Data.Filtering;
using Cap.Inventarios.BusinessObjects;

// With XPO, the data model is declared by classes (so-called Persistent Objects) that will define the database structure, and consequently, the user interface (http://documentation.devexpress.com/#Xaf/CustomDocument2600).
namespace Cap.Ventas.BusinessObjects
{
    // Specify various UI options for your persistent class and its properties using a declarative approach via built-in attributes (http://documentation.devexpress.com/#Xaf/CustomDocument3146).
    //[ImageName("BO_Contact")]
    //[DefaultProperty("PersistentProperty")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewAndDetailView, true, NewItemRowPosition.Top)]
    // [DefaultClassOptions]
    [NonPersistent]
    public class Partida : PObject
    { // You can use a different base persistent class based on your requirements (http://documentation.devexpress.com/#Xaf/CustomDocument3146).
        public Partida(Session session)
            : base(session)
        {
            // This constructor is used when an object is loaded from a persistent storage.
            // Do not place any code here or place it only when the IsLoading property is false.
        }

        /*>
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place here your initialization code (check out http://documentation.devexpress.com/#Xaf/CustomDocument2834 for more details).
        }*/

        //private string _PersistentProperty;
        //[XafDisplayName("My Persistent Property")]
        //[ToolTip("Specify a hint message for a property in the UI.")]
        //[ModelDefault("EditMask", "(000)-00")]
        //[Index(0), VisibleInListView(true), VisibleInDetailView(false), VisibleInLookupListView(true)]
        //[RuleRequiredField(DefaultContexts.Save)]
        //public string PersistentProperty {
        //    get { return _PersistentProperty; }
        //    set { SetPropertyValue("PersistentProperty", ref _PersistentProperty, value); }
        //}

        //[Action(Caption = "My Action Method", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = True)]
        //public void ActionMethod() {
        //    // Define an Action in the UI that can execute your custom code or invoke a dialog for processing information gathered from end-users (http://documentation.devexpress.com/#Xaf/CustomDocument2619).
        //}

        [Indexed("Documento; Item", Unique = true)]

        #region + Tipo folio ?, Por el momento no lo uso
        /*
		        // Pero para qué lo queremos? No es suficiente con lo que trae la cabeza?
		        // Bueno podria tener el A001 y el B001 y si no pongo el tipo fallará
		        // Bueno pero como objetos A001 es diferente al B001 tons no es necesario
		        private FolioDocumento FTipoD;
		        public FolioDocumento TipoFolio
		        {
		            get { return FTipoD; }
		            set { SetPropertyValue("TipoFolio", ref FTipoD, value); }
		        }*/
        #endregion

        //#region + Documento, esto no lo uso
        /*
		private Documento FDocumento;
		// Apply the Association attribute to mark the Customer property 
		// as the many end of the Customer-Orders association.
		[Association("Documento-Partidas")]
		public Documento Documento
		{
		    get { return FDocumento; }
		    set { SetPropertyValue("Documento", ref FDocumento, value); }
		}*/

        /*
        private Partida FAnterior;
        public Partida Anterior
        {
            get { return FAnterior; }
            set { SetPropertyValue("Anterior", ref FAnterior, value); }
        }

        private Partida FSiguiente;
        private Partida Siguiente
        {
            get { return FSiguiente; }
            set { SetPropertyValue("Siguiente", ref FSiguiente, value); }
        }*/
        //#endregion

        //#region + Item
        private short FItem;
        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        public short Item
        {
            get { return FItem; }
            set { SetPropertyValue("Item", ref FItem, value); }
        }
        //#endregion Item

        //#region + Status
        private PartidaStatus FStatus;
        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        public PartidaStatus Status
        {
            get { return FStatus; }
            set { SetPropertyValue("Status", ref FStatus, value); }
        }

        [RuleFromBoolProperty("Partida.Cantidad", DefaultContexts.Save, "La Cantidad debe ser mayor a 0")]
        protected bool PartidaCantidadOk
        {
            get { return Cantidad > 0; }
        }

        private double FCantidad;
        [VisibleInListView(true)]
        [Index(0)]
        public double Cantidad
        {
            get { return FCantidad; }
            set
            {
                SetPropertyValue("Cantidad", ref FCantidad, value);
                if (!IsLoading)
                    CalculaImpsts();
            }
        }

        protected virtual void CalculaImpsts()
        {
        }

        //#region + Cantidad autorizada
        private double FCantAuto;
        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        public double CantAuto
        {
            get { return FCantAuto; }
            set { SetPropertyValue("CantAuto", ref FCantAuto, value); }
        }

        //#region + Descripcion, por el momento no lo usamos, producto
        [RuleFromBoolProperty("DescripcionOk", DefaultContexts.Save, "Debe capturar una Descripción !")]
        protected bool DescripcionOk
        {
            get { return Descripcion.Trim().Length > 0; }
        }

        // Qué pasa cuando no están ligadas las facturas con los productos?
        // o usamos las notas?
        //[Size(60)]
        private string FDescrip;
        [Appearance("Partida.Desc", Context = "DetailView", Method = "CapEnFac", Enabled = false, FontStyle = FontStyle.Italic)]
        [DisplayName("Descripción")]
        [Size(SizeAttribute.Unlimited)]
        public string Descripcion
        {
            get
            {
                if (Producto != null && !Producto.CapEnFact)
                    return Producto.Descripcion;
                return FDescrip;
            }
            set { SetPropertyValue("Descripcion", ref FDescrip, value); }
        }

        //#region + Producto
        protected Producto FProducto;
        [ImmediatePostData]
        [DataSourceCriteria("Status != 4 && PrVnta")]
        [VisibleInListView(true)]
        [Index(1)]
        [RuleRequiredField("RuleRequiredField for Partida.Producto", DefaultContexts.Save, "Debe capturar el Producto", SkipNullOrEmptyValues = false)]
        virtual public Producto Producto
        {
            get { return FProducto; }
            set
            {
                SetPropertyValue("Producto", ref FProducto, value);
                SetPropertyValue("Criterion", ref _Criterion, new BinaryOperator("Prdct", value));
                // TODO: Falta ver cuando sea compra !!
                if (!IsLoading && IsNewObject() && value != null)
                {
                    AsignaProducto(value);
                }
            }
        }

        //#region + Descripcion Amplia
        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        [NonPersistent]
        public string DescripAmplia
        {
            get
            {
                if (Producto != null)
                    return string.Format("{0} {1}",
                        string.IsNullOrEmpty(Descripcion) ? Producto.Descripcion : Descripcion, Notas);
                else
                    return Notas;
            }
        }

        //#region + Almacen producto
        private AlmacenProducto FInventario;
        [VisibleInDetailView(false)]
        public AlmacenProducto Inventario
        {
            get { return FInventario; }
            set { SetPropertyValue("Inventario", ref FInventario, value); }
        }

        #region Descuentos, 1, 2, 3 porcentajes y un monto
        [RuleFromBoolProperty("PartidaDesc1", DefaultContexts.Save, "El Descuento 1 debe ser mayor o igual a 0 y menor a 100")]
        protected bool PartidaDesc1
        {
            get { return Descuento01 >= 0 && Descuento01 < 100; }
        }

        // Porcentaje  Descuento 1, con esto calculamos el descuento1 pero dejamos editable el Descuento1
        private float mDescuento01;
        [ModelDefault("DisplayFormat", "{0:n2}%")]
        [VisibleInListView(false)]
        public float Descuento01
        {
            get { return mDescuento01; }
            set
            {
                SetPropertyValue("Descuento01", ref mDescuento01, value);
                if (!IsLoading)
                    CalculaImpsts();
            }
        }
        
        [RuleFromBoolProperty("PartidaDesc2", DefaultContexts.Save, "El Descuento 2 debe ser mayor o igual a 0 y menor a 100")]
        protected bool PartidaDesc2
        {
            get { return Descuento02 >= 0 && Descuento02 < 100; }
        }

        // Descuento 2
        private float mDescuento02;
        [ModelDefault("DisplayFormat", "{0:n2}%")]
        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        public float Descuento02
        {
            get { return mDescuento02; }
            set
            {
                SetPropertyValue("Descuento02", ref mDescuento02, value);
                if (!IsLoading)
                    CalculaImpsts();
            }
        }

        [RuleFromBoolProperty("PartidaDesc3", DefaultContexts.Save, "El Descuento 3 debe ser mayor o igual a 0 y menor a 100")]
        protected bool PartidaDesc3
        {
            get { return Descuento03 >= 0 && Descuento03 < 100; }
        }

        private float mDescuento03;
        [ModelDefault("DisplayFormat", "{0:n2}%")]
        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        public float Descuento03
        {
            get { return mDescuento03; }
            set
            {
                SetPropertyValue("Descuento03", ref mDescuento03, value);
                if (!IsLoading)
                    CalculaImpsts();
            }
        }

        private decimal mDescuento;
        [Obsolete("En su lugar usar TotalDescuento01")]
        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        public decimal Descuento
        {
            get { return mDescuento; }
            set { SetPropertyValue("Descuento", ref mDescuento, value); }
        }

        [RuleFromBoolProperty("PartidaPorcDescF", DefaultContexts.Save, "El Descuento Financiero debe ser mayor o igual a 0 y menor a 100")]
        protected bool PartidaPorcDescF
        {
            get { return PorcDescFinan >= 0 && PorcDescFinan < 100; }
        }

        // Porc Descuento Financiero
        private float mPorcDescFinan;
        [ModelDefault("DisplayFormat", "{0:n2}%")]
        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        public float PorcDescFinan
        {
            get { return mPorcDescFinan; }
            set { SetPropertyValue("PorcDescFinan", ref mPorcDescFinan, value); }
        }

        [RuleFromBoolProperty("PartidaDescF", DefaultContexts.Save, "El Descuento Financiero debe ser mayor o igual a 0")]
        protected bool PartidaDescF
        {
            get { return DescFinan >= 0; }
        }

        // Descuento Financiero
        private decimal mDescFinan;
        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        public decimal DescFinan
        {
            get { return mDescFinan; }
            set
            {
                SetPropertyValue("DescFinan", ref mDescFinan, value);
                if (!IsLoading)
                    CalculaImpsts();
            }
        }
        #endregion

        [RuleFromBoolProperty("Partida.MontoUnitario", DefaultContexts.Save, "El Precio debe ser mayor a 0")]
        protected bool PartidaMontoUnitarioOk
        {
            get { return MontoUnitario > 0; }
        }

        private decimal FMontoU;
        /// <summary>
        /// Precio unitario
        /// </summary>
        [VisibleInListView(true)]
        [Index(2)]
        public decimal MontoUnitario
        {
            get { return FMontoU; }
            set
            {
                SetPropertyValue("MontoUnitario", ref FMontoU, value);
                if (!IsLoading)
                    CalculaImpsts();
            }
        }

        // #region + Costo
        private decimal FCosto;
        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        public decimal Costo
        {
            get { return FCosto; }
            set { SetPropertyValue("Costo", ref FCosto, value); }
        }

        // Cantidad remanente, de asociar
        private double FCanRem;
        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        public double CantidadRemanente
        {
            get { return FCanRem; }
            set { SetPropertyValue("CantidadRemanente", ref FCanRem, value); }
        }

        [Appearance("Importe", Context = "DetailView", Enabled = false, FontStyle = FontStyle.Italic)]
        [Index(3)]
        virtual public decimal Importe
        {
            get { return Convert.ToDecimal(Cantidad) * MontoUnitario; }
        }

        // + Subtotal por partida, aunque este se puede calcular, sera?
        [VisibleInListView(false)]
        public virtual decimal SubTotal
        {
            get
            {
                decimal sub = Importe; 
                decimal sub1 = sub * Convert.ToDecimal(1.0 - Descuento01 / 100.0);
                decimal sub2 = sub1 * Convert.ToDecimal(1.0 - Descuento02 / 100.0);
                decimal sub3 = sub2 * Convert.ToDecimal(1.0 - Descuento03 / 100.0);
                sub3 -= DescFinan;
                return sub3 * Convert.ToDecimal(1 + Impuesto04 / 100);
            }
            set 
            {
                if (!IsLoading && MontoUnitario == 0)
                    MontoUnitario = value / Convert.ToDecimal((1 + Impuesto04 / 100));
            }
        }

        // private string FObserva;
        [VisibleInDetailView(false)]
        [Size(SizeAttribute.Unlimited)]
        [Delayed]
        public string Notas
        {
            get { return GetDelayedPropertyValue<string>("Notas"); }
            set { SetDelayedPropertyValue("Notas", value.Trim(new char[] { ' ', '\0' })); }
        }

        // Tal vez esta partida usa moneda extranjera y 
        // por tanto el tipo de cambio del documento.
        // Supongo debe tomar la moneda del producto
        // No sé si sea necesario conocer la moneda o sólo saber que es otra moneda
        private Moneda FMoneda;
        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        public Moneda Moneda
        {
            get { return FMoneda; }
            set { SetPropertyValue("Moneda", ref FMoneda, value); }
        }

        private float FImpuesto01;
        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        public float Impuesto01
        {
            get { return FImpuesto01; }
            set { SetPropertyValue("Impuesto01", ref FImpuesto01, value); }
        }

        private float FImpuesto02;
        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        public float Impuesto02
        {
            get { return FImpuesto02; }
            set { SetPropertyValue("Impuesto02", ref FImpuesto02, value); }
        }

        private float FImpuesto03;
        /// <summary>
        /// Porc IEPS
        /// </summary>
        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        public float Impuesto03
        {
            get { return FImpuesto03; }
            set { SetPropertyValue("Impuesto03", ref FImpuesto03, value); }
        }

        [RuleFromBoolProperty("PartidaImp4", DefaultContexts.Save, "El Impuesto debe ser mayor o igual a 0")]
        protected bool PartidaImp4
        {
            get { return Impuesto04 >= 0; }
        }

        /// <summary>
        /// Porc Iva
        /// </summary>
        private float mImpuesto04;
        [Appearance("Impuesto04", Context = "DetailView", Enabled = false, FontStyle = FontStyle.Italic)]
        [VisibleInListView(false)]
        [DevExpress.Xpo.DisplayName("IVA")]
        [ModelDefault("DisplayFormat", "{0:n2}%")]
        public float Impuesto04
        {
            get { return mImpuesto04; }
            set { SetPropertyValue("Impuesto04", ref mImpuesto04, value); }
        }

        private decimal mMImpuesto04;
        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        public decimal MImpuesto04
        {
            get { return mMImpuesto04; }
            set { SetPropertyValue("MImpuesto04", ref mMImpuesto04, value); }
        }

        #region + Porc Retncion ISR
        private float mPorcRetISR;
        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        public float PorcRetISR
        {
            get { return mPorcRetISR; }
            set { SetPropertyValue("PorcRetISR", ref mPorcRetISR, value); }
        }
        #endregion

        #region + Porc Retención IVA
        private float mPorcRetIVA;
        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        public float PorcRetIVA
        {
            get { return mPorcRetIVA; }
            set { SetPropertyValue("PorcRetIVA", ref mPorcRetIVA, value); }
        }
        #endregion

        private decimal mIEPS;
        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        public decimal IEPS
        {
            get { return mIEPS; }
            set { SetPropertyValue("IEPS", ref mIEPS, value); }
        }

        #region + Retención ISR
        private decimal mRetISR;
        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        public decimal RetISR
        {
            get { return mRetISR; }
            set { SetPropertyValue("RetISR", ref mRetISR, value); }
        }
        #endregion

        #region + Retención IVA
        private decimal mRetIVA;
        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        public decimal RetIva
        {
            get { return mRetIVA; }
            set { SetPropertyValue("RetIva", ref mRetIVA, value); }
        }
        #endregion

        //#region + Fecha de alta
        // No lo hacemos small, porque debe llevar la hora también.
        // No basta con el documento ? 
        // Talvez tiene sentido si el documento se puede modificar !
        /*
        private DateTime FFechaAlta;
        [Obsolete("No será suficiente con el documento ?")]
        [NonPersistent]
        public DateTime FechaAlta
        {
            get { return FFechaAlta; }
            set { SetPropertyValue("FechaAlta", ref FFechaAlta, value); }
        }*/
        //#endregion

        //#region + Unidad, de venta
        private Unidad FUnidad;
        // TIT, Sep 2018, le damos oportunidad de elegir otra unidad
        // [Appearance("Unidad", Context = "DetailView", Enabled = false, FontStyle = FontStyle.Italic)]
        public Unidad Unidad
        {
            get { return FUnidad; }
            set { SetPropertyValue("Unidad", ref FUnidad, value); }
        }

        private float FComision;
        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        public float Comision
        {
            get { return FComision; }
            set { SetPropertyValue("Comision", ref FComision, value); }
        }

        #region + Secuencial
        private string mSecuencial;
        /// <summary>
        /// </summary>
        [NonPersistent]
        [Obsolete("Se usa? No es la del documento?")]  // Será para ordenar las partidas?
        [Size(20)]
        public string Secuencial
        {
            get { return mSecuencial; }
            set { SetPropertyValue("Secuencial", ref mSecuencial, value); }
        }
        #endregion

        #region + Serie
        /*
        private string mSerie;
        [NonPersistent]
        [Obsolete("Se usa? No es la del documento?")]
        [Size(10)]
        public string Serie
        {
            get { return mSerie; }
            set { SetPropertyValue("Serie", ref mSerie, value); }
        }*/
        #endregion


        #region + MovimientoI
        // Como una asociación o sólo como una indicación? 
        // Si viene null es que para esta partida falta el movimiento de entrada o salida.
        private MovimientoI mMovimiento;
        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        public MovimientoI Movimiento
        {
            get { return mMovimiento; }
            set { SetPropertyValue("Movimiento", ref mMovimiento, value); }
        }
        #endregion MovimientoI



        #region + Total descuento
        [NonPersistent]
        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        public decimal TotalDescuento
        {
            get { return TotalDescuento01 + TotalDescuento02 + TotalDescuento03; }
        }

        [NonPersistent]
        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        public decimal TotalDescuentos
        {
            get { return TotalDescuento01 + TotalDescuento02 + TotalDescuento03 + DescFinan; }
        }

        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        [NonPersistent]
        public decimal TotalDescuento01
        {
            get
            {
                decimal aux = 
                (decimal)Cantidad * (MontoUnitario == 0 ? Costo : MontoUnitario)
                    * (decimal)Descuento01 / 100;
                aux = Math.Round(aux, 2, MidpointRounding.AwayFromZero);

                return aux;
            }
        }

        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        [NonPersistent]
        public decimal TotalDescuento02
        {
            get
            {
                return ((decimal)Cantidad * (MontoUnitario == 0 ? Costo : MontoUnitario)
                * (decimal)(1.0 - Descuento01 / 100)) * (decimal)Descuento02 / 100;
            }
        }

        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        [NonPersistent]
        public decimal TotalDescuento03
        {
            get
            {
                return ((decimal)Cantidad * (MontoUnitario == 0 ? Costo : MontoUnitario)
                * (decimal)(1.0 - Descuento01 / 100) * (decimal)(1.0 - Descuento02 / 100)) * (decimal)Descuento03 / 100;
            }
        }
        #endregion Total descuento

        /// <summary>
        /// Total de impuesto IVA
        /// </summary>
        [Appearance("TotalImpuesto04", Context = "DetailView", Enabled = false, FontStyle = FontStyle.Italic)]
        [DevExpress.Xpo.DisplayName("Total IVA")]
        [VisibleInListView(false)]
        [NonPersistent]
        public decimal TotalImpuesto04
        {
            get
            {
                /*
                return (decimal)Cantidad * MontoU()
                    * (decimal)(1.0 - Descuento01 / 100.0)
                    * (decimal)(1.0 - Descuento02 / 100.0)
                    * (decimal)(1.0 - PorcDescFinan / 100.0)
                    * (decimal)Impuesto04 / 100;*/
                // return (Importe - TotalDescuentos) * (decimal)Impuesto04 / 100;
                // TI Jul 2016, Parece que esto hace una diferencia en los decimales con el Total Garu Cxc
                return Math.Round((Importe - TotalDescuentos) * (decimal)Impuesto04 / 100, 
                    2, MidpointRounding.AwayFromZero);
            }
        }


        private decimal mTotalImpuesto01;
        /// <summary>
        /// Total de impuesto 1
        /// </summary>
        //[Appearance("TotalImpuesto01", Context = "DetailView", Enabled = false, FontStyle = FontStyle.Italic)]
        // [DevExpress.Xpo.DisplayName("Total IVA")]
        [VisibleInListView(false)]
        //[NonPersistent]
        public decimal TotalImpuesto01
        {
            get
            {
                if (mTotalImpuesto01 == 0)
                {
                    decimal val = 0;
                    if (Producto != null && Producto.Esquema != null)
                    {
                        if (Producto.Esquema.AplImpuesto1 == EAplicaImpuesto.Precio)
                            val = Impuesto01 < 0 ? Importe
                                * -(decimal)Impuesto01 / 100 : Importe
                                * (decimal)Impuesto01 / 100;
                    }
                    return Math.Round(val, 2, MidpointRounding.AwayFromZero);
                }
                else
                    return mTotalImpuesto01;
            }
            set
            {
                SetPropertyValue("TotalImpuesto01", ref mTotalImpuesto01, value);
            }
        }

        /// <summary>
        /// Total de impuesto 3
        /// </summary>
        [Appearance("TotalImpuesto03", Context = "DetailView", Enabled = false, FontStyle = FontStyle.Italic)]
        [VisibleInListView(false)]
        [NonPersistent]
        public decimal TotalImpuesto03
        {
            get
            {
                // TI Sep 2017 lo dejé aqui pero según nuestra idea debería ir en Negocio.
                decimal val = 0;
                if (Producto != null && Producto.Esquema != null)
                {
                    if (Producto.Esquema.AplImpuesto3 == EAplicaImpuesto.Precio)
                        val = Impuesto03 < 0 ? Importe
                            * -(decimal)Impuesto03 / 100 : Importe
                            * (decimal)Impuesto03 / 100;
                    else if (Producto.Esquema.AplImpuesto3 == EAplicaImpuesto.Acumulado3)
                        val = Impuesto03 < 0 
                            ? TotalImpuesto04 * -(decimal)Impuesto03 / 100 
                            : TotalImpuesto04 * (decimal)Impuesto03 / 100;
                }
                return Math.Round(val, 2, MidpointRounding.AwayFromZero); 
                // return val;
            }
        }

        /// <summary>
        /// Total de impuesto 2
        /// </summary>
        [Appearance("TotalImpuesto02", Context = "DetailView", Enabled = false, FontStyle = FontStyle.Italic)]
        [VisibleInListView(false)]
        [NonPersistent]
        public decimal TotalImpuesto02
        {
            get
            {
                decimal val = 0;
                if (Producto != null && Producto.Esquema != null)
                {
                    if (Producto.Esquema.AplImpuesto2 == EAplicaImpuesto.Precio)
                        val = Impuesto02 < 0 ? Importe
                            * -(decimal)Impuesto02 / 100 : Importe
                            * (decimal)Impuesto02 / 100;
                }
                return Math.Round(val, 2, MidpointRounding.AwayFromZero); 
                // return val;
            }
        }

        private ProductoPrecios mPrdctPrcs;
        [VisibleInDetailView(false)] // TI Mientras veo como ponerlo
        [ImmediatePostData]
        [VisibleInListView(false)]
        [XafDisplayName("Otros Precios")]
        [DataSourceCriteriaProperty("Criterion")]
        public ProductoPrecios PrdctPrcs
        {
            get { return mPrdctPrcs; }
            set
            {
                SetPropertyValue("PrdctPrcs", ref mPrdctPrcs, value);
                if (!IsLoading && !IsSaving && value != null)
                    MontoUnitario = value.Prc;
            }
        }

        private CriteriaOperator _Criterion;
        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        public CriteriaOperator Criterion
        {
            get { return _Criterion; }
            set
            {
                SetPropertyValue("Criterion", ref _Criterion, value);
                // Refresh the Tasks property data source
                // RefreshAvailableTasks();
            }
        }



        protected virtual decimal MontoU()
        {
            return 0;
        }





        public override void AfterConstruction()
        {
            base.AfterConstruction();

            Cantidad = 1;
            CantAuto = 0;
            CantidadRemanente = 0;
            Descuento01 = 0;
            Descuento02 = 0;
            Descuento03 = 0;
            Descuento = 0;
            DescFinan = 0;
            Impuesto01 = 0;
            Impuesto02 = 0;
            Impuesto03 = 0;
            Impuesto04 = 0;
            Inventario = null;
            Item = 0;
            Moneda = null;
            MontoUnitario = 0;
            Notas = string.Empty;
            Producto = null;
            Status = PartidaStatus.Autorizado;

            Criterion = new BinaryOperator("Prdct", string.Empty);
        }
        



        protected virtual void AsignaProducto(Producto value)
        {
        }


        public virtual Documento ElDocumento()
        {
            return null;
        }

        /// <summary>
        /// Para saber si hay algún impuesto aplicado
        /// </summary>
        /// <returns></returns>
        public bool HayImpuesto()
        {
            decimal val = (TotalImpuesto01 + TotalImpuesto02 + TotalImpuesto03 + TotalImpuesto04);
            return Math.Abs(Math.Round(val, 2, MidpointRounding.AwayFromZero)) > 0;
        }
    }
}