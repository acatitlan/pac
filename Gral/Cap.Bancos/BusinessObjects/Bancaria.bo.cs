#region Copyright (c) 2017-2020 cjlc
/*
{+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++)
{                                                                   }
{     cjlc Cap control administrativo personal                      }
{                                                                   }
{     Copyrigth (c) 2017-2020 cjlc                                  }
{     Todos los derechos reservados                                 }
{                                                                   }
{*******************************************************************}
 */
#endregion

using Cap.Generales.BusinessObjects.General;
using Cap.Generales.BusinessObjects.Unidades;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.Drawing;

namespace Cap.Bancos.BusinessObjects
{
    [NavigationItem("Bancos")]
    [Appearance("Valida", TargetItems ="*", Context ="ListView", Method ="Vence", FontColor ="Orange")]
    [Appearance("Baja", TargetItems = "*", Context = "ListView", Criteria = "[Status] == 2", FontColor = "Green", FontStyle = FontStyle.Strikeout)]
    [Appearance("Afore", TargetItems = "SaldoFinal, SldDspnbl", Context = "ListView", Criteria = "[Tipo] == 5", FontColor = "Gray")]
    [Appearance("Credito", TargetItems = "SaldoFinal, Clave", Context = "ListView", Criteria = "[Tipo] == 0 && [SaldoFinal] != 0", FontColor = "Red")]
    [ImageName("Cash_Count")]
    public partial class Bancaria
    {
        //#region + Beneficiario de la cuenta
        /* Falta ver qué hacemos con esto.
        public XPersona Beneficiario;*/
        /* Jun 2020
        private string FBeneficiario;
        [Obsolete("En su lugar usar Beneficiarios")]
        [VisibleInLookupListView(false)]
        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        [Size(40)]
        public string Beneficiario
        {
            get { return FBeneficiario; }
            set { SetPropertyValue("Beneficiario", ref FBeneficiario, ValorString("Beneficiario", value)); }
        }*/

        private StatusTipoA status;
        [VisibleInLookupListView(false)]
        public StatusTipoA Status
        {
            get { return status; }
            set { SetPropertyValue("Status", ref status, value); }
        }

        private Moneda FMoneda;
        [RuleRequiredField("RuleRequiredField for Cuenta.Moneda", DefaultContexts.Save, "Debe asignar una Moneda", SkipNullOrEmptyValues = false)]
        [VisibleInLookupListView(false)]
        public Moneda Moneda
        {
            get { return FMoneda; }
            set { SetPropertyValue("Moneda", ref FMoneda, value); }
        }

        //#region + Fecha de alta
        private DateTime fAlta;
        [Appearance("FechaAlta", Context = "DetailView", Enabled = false, FontStyle = FontStyle.Italic)]
        [VisibleInLookupListView(false)]
        [VisibleInListView(false)]
        [ModelDefault("DisplayFormat", "{0:dd MMM yyyy}")]
        public DateTime FechaAlta
        {
            get { return fAlta; }
            set { SetPropertyValue("FechaAlta", ref fAlta, value); }
        }

        //#region + Fecha de ultimo movimiento
        private DateTime fUltMov;
        [DisplayName("Fecha Últ. Mov.")]
        [Appearance("FechaUltimoMov", Context = "DetailView", Enabled = false, FontStyle = FontStyle.Italic)]
        [VisibleInLookupListView(false)]
        [ModelDefault("DisplayFormat", "{0:dd MMM yyyy}")]
        public DateTime FechaUltimoMov
        {
            get { return fUltMov; }
            set { SetPropertyValue("FechaUltimoMov", ref fUltMov, value); }
        }

        //#region + Verifica saldo
        private bool verifSldo;
        [VisibleInLookupListView(false)]
        [VisibleInListView(false)]
        public bool VerificaSaldo
        {
            get { return verifSldo; }
            set { SetPropertyValue("VerificaSaldo", ref verifSldo, value); }
        }

        private string banco;
        [VisibleInLookupListView(true)]
        [Size(20)]
        public string Banco
        {
            get { return banco; }
            set { SetPropertyValue("Banco", ref banco, ValorString("Banco", value)); }
        }

        private string plaza;
        [VisibleInLookupListView(false)]
        [VisibleInListView(false)]
        [Size(20)]
        public string Sucursal
        {
            get { return plaza; }
            set { SetPropertyValue("Sucursal", ref plaza, ValorString("Sucursal", value)); }
        }

        //#region + Web
        private string url;
        [EditorAlias("HyperLinkPropertyEditor")]
        [VisibleInLookupListView(false)]
        [VisibleInListView(false)]
        [Size(50)]  // 40 Ene 2020
        public string Url
        {
            get { return url; }
            set { SetPropertyValue("Url", ref url, ValorString("Url", value)); }
        }

        private string FWebUsuario;
        [VisibleInLookupListView(false)]
        [VisibleInListView(false)]
        [Size(20)]
        public string WebUsuario
        {
            get { return FWebUsuario; }
            set { SetPropertyValue("WebUsuario", ref FWebUsuario, ValorString("WebUsuario", value)); }
        }

        private string FWebNip;
        [VisibleInLookupListView(false)]
        [VisibleInListView(false)]
        [Size(20)]
        public string WebNip
        {
            get { return FWebNip; }
            set { SetPropertyValue("WebNip", ref FWebNip, ValorString("WebNip", value)); }
        }

        private string FWebClave;
        [VisibleInLookupListView(false)]
        [VisibleInListView(false)]
        [Size(20)]
        public string WebClave
        {
            get { return FWebClave; }
            set { SetPropertyValue("WebClave", ref FWebClave, ValorString("WebClave", value)); }
        }
        //#endregion

        // Tal vez esto no tiene sentido
        private string FProveedor;
        [VisibleInLookupListView(false)]
        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        [Size(5)]
        [NonPersistent]
        public string Proveedor
        {
            get { return FProveedor; }
            set { SetPropertyValue("Proveedor", ref FProveedor, ValorString("Proveedor", value)); }
        }

        #region Atencion del banco
        //#region + Nombre de quien atiende
        private string FAtencion;
        [VisibleInLookupListView(false)]
        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        [Size(30)]
        [NonPersistent]
        public string Atencion
        {
            get { return FAtencion; }
            set { SetPropertyValue("Atencion", ref FAtencion, ValorString("Atencion", value)); }
        }
        //#endregion

        private string FTelefono;
        [ModelDefault("EditMask", "[0-9\x20-\x20]+"), ModelDefault("EditMaskType", "RegEx")]
        [VisibleInLookupListView(false)]
        [VisibleInListView(false)]
        [Size(15)]
        public string Telefono
        {
            get { return FTelefono; }
            set { SetPropertyValue("Telefono", ref FTelefono, ValorString("Telefono", value)); }
        }

        //#region + Email
        private string FEmail;
        [VisibleInLookupListView(false)]
        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        [Size(30)]
        [NonPersistent]
        public string Email
        {
            get { return FEmail; }
            set { SetPropertyValue("Email", ref FEmail, ValorString("Email", value)); }
        }
        //#endregion

        // Falta ver qué hacemos con esto.
        // public Direccion Direccion;
        // TODO: public Empleado, falta hacer esta parte
        #endregion

        //#region + Saldo conciliado
        private decimal saldoC;
        [VisibleInLookupListView(false)]
        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        public decimal SaldoConciliado
        {
            get { return saldoC; }
            set { SetPropertyValue("SaldoConciliado", ref saldoC, value); }
        }

        private decimal saldoP;
        [Appearance("SaldoPromedio", Context = "DetailView", Enabled = false, FontStyle = FontStyle.Italic)]
        [VisibleInLookupListView(false)]
        [VisibleInListView(false)]
        public decimal SaldoPromedio
        {
            get { return saldoP; }
            set { SetPropertyValue("SaldoPromedio", ref saldoP, value); }
        }

        //#region + Saldo minimo
        [RuleFromBoolProperty("SaldominimoOk", DefaultContexts.Save, "El saldo mínimo no puede ser negativo")]
        protected bool SaldominimoOk
        {
            get { return Saldominimo >= 0; }
        }

        private decimal saldom;
        [DisplayName("Saldo mínimo")]
        [Appearance("Saldominimo", AppearanceItemType = "LayoutItem", Context = "DetailView", Criteria = "Tipo = 0", Visibility = ViewItemVisibility.Hide)]
        [VisibleInLookupListView(false)]
        [VisibleInListView(false)]
        public decimal Saldominimo
        {
            get { return saldom; }
            set { SetPropertyValue("Saldominimo", ref saldom, value); }
        }

        //#region + Comision por saldo menor al minimo
        [RuleFromBoolProperty("ComisionSmOk", DefaultContexts.Save, "La Comisión por saldo menor al Mínimo no puede ser negativo")]
        protected bool ComisionSmOk
        {
            get { return ComisionSm >= 0; }
        }

        private decimal FComisionSM;
        [DisplayName("Comisión Saldo mínimo")]
        [Appearance("ComisionSm", AppearanceItemType = "LayoutItem", Context = "DetailView", Criteria = "Tipo == 0", Visibility = ViewItemVisibility.Hide)]
        [VisibleInLookupListView(false)]
        [VisibleInListView(false)]
        public decimal ComisionSm
        {
            get { return FComisionSM; }
            set { SetPropertyValue("ComisionSm", ref FComisionSM, value); }
        }

        //#region Comisiones de cheques
        // A lo mejor esto se puede modelar con la ayuda de los conceptos
        // Cuantos conceptos libres por mes, y comisión por concepto extra
        private double FChequeGirado;
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        [VisibleInListView(false)]
        [NonPersistent]
        public double ChequeGirado
        {
            get { return FChequeGirado; }
            set { SetPropertyValue("ChequeGirado", ref FChequeGirado, value); }

        }

        private UInt16 FChequesLibres;
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        [VisibleInListView(false)]
        [NonPersistent]
        public UInt16 ChequesLibres
        {
            get { return FChequesLibres; }
            set { SetPropertyValue("ChequesLibres", ref FChequesLibres, value); }
        }

        private double FChequeDevuelto;
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        [VisibleInListView(false)]
        [NonPersistent]
        public double ChequeDevuelto
        {
            get { return FChequeDevuelto; }
            set { SetPropertyValue("ChequeDevuelto", ref FChequeDevuelto, value); }
        }

        private double FRetiroCajero;
        [VisibleInDetailView(false)]
        [NonPersistent]
        [VisibleInLookupListView(false)]
        [VisibleInListView(false)]
        public double RetiroCajero
        {
            get { return FRetiroCajero; }
            set { SetPropertyValue("RetiroCajero", ref FRetiroCajero, value); }
        }

        private UInt16 FRetirosLibres;
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        [VisibleInListView(false)]
        [NonPersistent]
        public UInt16 RetirosLibres
        {
            get { return FRetirosLibres; }
            set { SetPropertyValue("RetirosLibres", ref FRetirosLibres, value); }
        }

        private double FRetiroSucursal;
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        [VisibleInListView(false)]
        [NonPersistent]
        public double RetiroSucursal
        {
            get { return FRetiroSucursal; }
            set { SetPropertyValue("RetiroSucursal", ref FRetiroSucursal, value); }
        }

        private double FSaldoInsuficienteRetiro;
        // Tal vez después lo active aunque sea como informativo
        [VisibleInDetailView(false)]
        [DisplayName("Cms. por Saldo Insuficiente")]
        [VisibleInLookupListView(false)]
        [VisibleInListView(false)]
        [NonPersistent]
        public double SaldoInsuficienteRetiro
        {
            get { return FSaldoInsuficienteRetiro; }
            set { SetPropertyValue("SaldoInsuficienteRetiro", ref FSaldoInsuficienteRetiro, value); }
        }

        //#region + Cuota anual
        [RuleFromBoolProperty("CuotaAnualOk", DefaultContexts.Save, "La Cuota Anual debe ser mayor o igual a 0")]
        protected bool CuotaAnualOk
        {
            get { return CuotaAnual >= 0; }
        }

        private decimal cuotaA;
        [VisibleInLookupListView(false)]
        [VisibleInListView(false)]
        public decimal CuotaAnual
        {
            get { return cuotaA; }
            set { SetPropertyValue("CuotaAnual", ref cuotaA, value); }
        }

        private DateTime fCuotaA;
        /// <summary>
        /// Dia y mes del año en que cobran la cuota anual, por uso de su tarjeta
        /// </summary>
        [ModelDefault("EditMask", "m"), ModelDefault("EditMaskType", "Default")]
        [Appearance("FechaCuota", Context = "DetailView", Criteria = "CuotaAnual = 0", Enabled = false, FontStyle = FontStyle.Italic)]
        [ModelDefault("DisplayFormat", "{0:dd MMM}")]
        [VisibleInLookupListView(false)]
        [VisibleInListView(false)]
        public DateTime FechaCuota
        {
            get { return fCuotaA; }
            set { SetPropertyValue("FechaCuota", ref fCuotaA, value); }
        }

        //#region + Porcentaje de interes
        [RuleFromBoolProperty("PorcentajeInteresOk", DefaultContexts.Save, "El Porcentaje debe estar entre 0 y 100")]
        protected bool PorcentajeInteresOk
        {
            get { return PorcentajeInteres >= 0 && PorcentajeInteres < 100; }
        }

        private float porcInt;
        [DisplayName("Porcentaje Interés")]
        [ModelDefault("DisplayFormat", "{0:n2}%")]
        [ModelDefault("EditMask", "P2"), ModelDefault("EditMaskType", "Default")]
        [VisibleInLookupListView(false)]
        [VisibleInListView(false)]
        public float PorcentajeInteres
        {
            get { return porcInt; }
            set { SetPropertyValue("PorcentajeInteres", ref porcInt, value); }
        }

        //#region + Periodo, tal vez mensual, etc
        private short periodo;
        [VisibleInLookupListView(false)]
        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        public short Periodo
        {
            get { return periodo; }
            set { SetPropertyValue("Periodo", ref periodo, value); }
        }

        //#region + Intereses del periodo
        [VisibleInLookupListView(false)]
        [VisibleInListView(false)]
        [NonPersistent]
        public decimal Intereses
        {
            // Cambie la captura del porcentaje, entonces ya no es necesario dividirlo entre 100
            get { return SaldoPromedio * Convert.ToDecimal(PorcentajeInteres) /* /100 */; }
            // set { SetPropertyValue("Intereses", ref interes, value); }
        }

        //#region + Saldo limite
        [RuleFromBoolProperty("SaldoLimiteOk", DefaultContexts.Save, "El límite de crédito no puede ser negativo.")]
        protected bool SaldoLimiteOk
        {
            get { return SaldoLimite >= 0; }
        }

        private decimal saldoL;
        [Appearance("SaldoLimite", AppearanceItemType = "LayoutItem", Context = "DetailView", Criteria = "Tipo <> 0", Visibility = ViewItemVisibility.Hide)]
        [VisibleInLookupListView(false)]
        [VisibleInListView(false)]
        public decimal SaldoLimite
        {
            get { return saldoL; }
            set { SetPropertyValue("SaldoLimite", ref saldoL, value); }
        }

        //#region + Dia de corte
        [RuleFromBoolProperty("DiaCorteOk", DefaultContexts.Save, "El Dia de Corte debe estar entre 1 y 31")]
        protected bool DiaCorteOk
        {
            get { return DiaCorte >= 0 && DiaCorte <= 31; }
        }

        private UInt16 diaCorte;
        [Appearance("DiaCorte", AppearanceItemType = "LayoutItem", Context = "DetailView", Criteria = "Tipo <> 0", Visibility = ViewItemVisibility.Hide)]
        [VisibleInLookupListView(false)]
        [VisibleInListView(false)]
        public UInt16 DiaCorte
        {
            get { return diaCorte; }
            set { SetPropertyValue("DiaCorte", ref diaCorte, value); }
        }

        //#region + Dia de pago
        [RuleFromBoolProperty("DiaPagoOk", DefaultContexts.Save, "El Dia de Pago debe estar entre 1 y 31")]
        protected bool DiaPagoOk
        {
            get { return DiaPago >= 0 && DiaPago <= 31; }
        }

        private UInt16 diaPago;
        [Appearance("DiaPago", AppearanceItemType = "LayoutItem", Context = "DetailView", Criteria = "Tipo <> 0", Visibility = ViewItemVisibility.Hide)]
        [VisibleInLookupListView(false)]
        [VisibleInListView(false)]
        public UInt16 DiaPago
        {
            get { return diaPago; }
            set { SetPropertyValue("DiaPago", ref diaPago, value); }
        }

        //#region + NumDias ??
        private short FNumDias;
        [VisibleInLookupListView(false)]
        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        [NonPersistent]
        public short NumDias
        {
            get { return FNumDias; }
            set { SetPropertyValue("NumDias", ref FNumDias, value); }
        }

        private decimal FSaldoTransito;
        [DisplayName("Saldo Tránsito")]
        [Appearance("SaldoTransito", Context = "DetailView", Enabled = false, FontStyle = FontStyle.Italic)]
        [VisibleInLookupListView(false)]
        [VisibleInListView(false)]
        public decimal SaldoTransito
        {
            get { return FSaldoTransito; }
            set { SetPropertyValue("SaldoTransito", ref FSaldoTransito, value); }
        }

        //#region + Saldo por aclarar
        private decimal FSaldoAclarar;
        [VisibleInLookupListView(false)]
        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        public decimal SaldoAclarar
        {
            get { return FSaldoAclarar; }
            set { SetPropertyValue("SaldoAclarar", ref FSaldoAclarar, value); }
        }

        //#region + Tipo de cta
        private ECuentaTipo FTipo;
        [ImmediatePostData]
        [VisibleInLookupListView(false)]
        public ECuentaTipo Tipo
        {
            get { return FTipo; }
            set { SetPropertyValue("Tipo", ref FTipo, value); }
        }

        //#region + Cuenta contable
        /* Pa otra vida
        private Contable FCtaContable;
        public Contable CuentaContable
        {
            get { return FCtaContable; }
            set { SetPropertyValue("CuentaContable", ref FCtaContable, value); }
        }*/
        //#endregion

        //#region + Numero de cheque
        [RuleFromBoolProperty("NumeroChequeOk", DefaultContexts.Save, "El Número de Cheque debe ser mayor a 0")]
        protected bool NumeroChequeOk
        {
            get { return Tipo != ECuentaTipo.Maestra || NumeroCheque > 0; }
        }

        private int FNumeroCheque;
        [Appearance("NumeroCheque", AppearanceItemType = "LayoutItem", Context = "DetailView", Criteria = "Tipo <> 2", Visibility = ViewItemVisibility.Hide)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public int NumeroCheque
        {
            get { return FNumeroCheque; }
            set { SetPropertyValue("NumeroCheque", ref FNumeroCheque, value); }
        }

        //#region + Captura num cheque
        private bool FCapturaCheque;
        [Appearance("CapturaCheque", AppearanceItemType = "LayoutItem", Context = "DetailView", Criteria = "Tipo <> 2", Visibility = ViewItemVisibility.Hide)]
        [VisibleInLookupListView(false)]
        [VisibleInListView(false)]
        public bool CapturaCheque
        {
            get { return FCapturaCheque; }
            set { SetPropertyValue("CapturaCheque", ref FCapturaCheque, value); }
        }

        //#region + Movimientos, saldos, no por el momento
        /* > ? La dejaré o no ?
        [Association("CuentaMovimientos"), Aggregated]
        public XPCollection<MovimientoB> Movimientos
        {
            get
            {
                return GetCollection<MovimientoB>("Movimientos");
            }
        }*/

        /* Por el momento no usamos asociaciones, en la next ver
        [Association("Bancaria-Movimientos")]
        public XPCollection<MovimientoB> Movimientos
        {
            get
            {
                return GetCollection<MovimientoB>("Movimientos");
            }
        }

        [Association("Bancaria-Saldos")]
        public XPCollection<Saldo> Saldos
        { 
            get 
            { 
                return GetCollection<Saldo>("Saldos"); 
            } 
        }*/
        //#endregion

        //#region + Formato nombre
        // Para la impresión de cheques, que te parece el nombre del formato
        // estandar mas la clave.
        private string FFormato;
        [NonPersistent]
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        [VisibleInListView(false)]
        [Size(255)]
        public string FormatoNombre
        {
            get { return FFormato; }
            set { SetPropertyValue("FormatoNombre", ref FFormato, ValorString("FormatoNombre", value)); }
        }

        //#region + Clabe interbancaria
        private string FClabe;
        [VisibleInLookupListView(false)]
        [VisibleInListView(false)]
        [Size(20)]
        public string Clabe
        {
            get { return FClabe; }
            set
            {
                if (value != null)
                    SetPropertyValue("Clabe", ref FClabe, ValorString("Clabe", value.ToUpper()));
            }
        }

        #region Tarjeta
        //#region + Numero tarjeta
        private string FTarjeta;
        [VisibleInLookupListView(false)]
        [VisibleInListView(false)]
        [Size(19)]
        public string Tarjeta
        {
            get { return FTarjeta; }
            set { SetPropertyValue("Tarjeta", ref FTarjeta, ValorString("Tarjeta", value)); }
        }

        private string FTarjetaPass;
        /// Contraseña tarjeta atm
        [DisplayName("Tarjeta Contra.")]
        [VisibleInLookupListView(false)]
        [VisibleInListView(false)]
        [Size(4)]
        public string TarjetaPass
        {
            get { return FTarjetaPass; }
            set { SetPropertyValue("TarjetaPass", ref FTarjetaPass, ValorString("TarjetaPass", value)); }
        }

        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        public decimal SldDspnbl
        {
            get 
            {
                if (Tipo == ECuentaTipo.Credito)
                    return SaldoLimite - SaldoFinal;
                else
                    return SaldoFinal - Saldominimo;
            }
        }

        private DateTime mFchAprtr;
        [DisplayName("Fecha Apertura")]
        [VisibleInLookupListView(false)]
        [VisibleInListView(false)]
        [ModelDefault("DisplayFormat", "{0:dd MMM yyyy}")]
        /// Fecha de Apertura de la cuenta
        public DateTime FApertura
        {
            get { return mFchAprtr; }
            set { SetPropertyValue("FApertura", ref mFchAprtr, value); }
        }
        #endregion

        private DateTime mFchVld;
        [DisplayName("Válida Hasta")]
        [VisibleInLookupListView(false)]
        [VisibleInListView(false)]
        [ModelDefault("DisplayFormat", "{0:MMM yyyy}")]
        [ModelDefault("EditMask", "y"), ModelDefault("EditMaskType", "Default")]
        public DateTime FchVald
        {
            get { return mFchVld; }
            set { SetPropertyValue("FchVald", ref mFchVld, value); }
        }

        private string mNmrClnt;
        [DisplayName("Número de Cliente")]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        [Size(15)]
        public string NmrClnt
        {
            get { return mNmrClnt; }
            set { SetPropertyValue("NmrClnt", ref mNmrClnt, value); }
        }

        // Card Verification Code, CVC
        private string mCvc;
        [VisibleInLookupListView(false)]
        [VisibleInListView(false)]
        [Size(5)]
        public string Cvc
        {
            get { return mCvc; }
            set { SetPropertyValue("Cvc", ref mCvc, value); }
        }


        [Association("Bancaria-Beneficiarios", typeof(Beneficiario)), Aggregated]
        public XPCollection Beneficiarios
        {
            get { return GetCollection("Beneficiarios"); }
        }

        [Association("Bancaria-Aclaraciones", typeof(BAclaracion)), Aggregated]
        public XPCollection Aclaraciones
        {
            get { return GetCollection("Aclaraciones"); }
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();

            Atencion = string.Empty;
            Banco = string.Empty;
            /*
            Beneficiario = string.Empty;*/
            /* Pa otra vida
            CuentaContable = null;*/
            CuotaAnual = 0.0M;
            DiaCorte = 0;
            DiaPago = 0;
            Email = string.Empty;
            FechaCuota = DateTime.Today;
            FechaAlta = DateTime.Today;
            FechaUltimoMov = DateTime.Today;
            FormatoNombre = string.Empty;
            // Intereses = 0.0m;
            Moneda = null;
            NumeroCheque = 1;
            Periodo = 0;
            PorcentajeInteres = 0;
            Proveedor = string.Empty;
            SaldoAclarar = 0.0m;
            SaldoConciliado = 0.0M;
            SaldoLimite = 0.0M;
            Saldominimo = 0.0M;
            SaldoPromedio = 0.0M;
            SaldoTransito = 0.0m;
            Status = StatusTipoA.Activa;
            Sucursal = string.Empty;
            Telefono = string.Empty;
            Tipo = ECuentaTipo.Debito;
            VerificaSaldo = false;
            Url = string.Empty;

            Moneda = Session.FindObject<Moneda>(new BinaryOperator("Sistema", true));
            NmrClnt = string.Empty;
            Cvc = string.Empty;
        }


        private bool Vence()
        {
            DateTime newDate = DateTime.Now;

            // Difference in days, hours, and minutes.
            TimeSpan ts = newDate - FchVald;

            // Difference in days.
            int differenceInDays = ts.Days;

            return FchVald.Year > 1 && differenceInDays > -30;
        }
    }
}
