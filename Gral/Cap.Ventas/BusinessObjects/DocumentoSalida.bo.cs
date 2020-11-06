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

using System;
using System.ComponentModel;
using System.Drawing;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using ExtensionMethods;
using Cap.Generales.BusinessObjects.Object;
using Cap.Generales.BusinessObjects.Empresa;
using Cap.Fe.BusinessObjects;
using Cap.Inventarios.BusinessObjects;
using Cap.Clientes.BusinessObjects.Clientes;
using Cap.Clientes.BusinessObjects.Generales;

namespace Cap.Ventas.BusinessObjects
{
    [NavigationItem("Ventas")]
    [Appearance("DocumentoSalida.CartaPorte", TargetItems = "Dstn, Transprt, Oprdr, Plcs, Otros," +
        " Autopis, Manio, EntregaD, Recolecc, Seguro, Flete, ConduceA, ConduceDe, ReembCn, Reemb," +
        " ValorD, CuotaC, RecogerEn", 
        AppearanceItemType = "LayoutItem", Visibility = ViewItemVisibility.Hide, Context = "DetailView", Method = "CartaPorte")]
    [Appearance("DocumentoSalida.Lectura", TargetItems = "Concilia, SelloSatCan, Uuid, RetenIVA," +
        " RetenISR, Secuencial", 
        Context = "DetailView", Enabled = false, FontStyle = FontStyle.Italic)]
    [Appearance("Cancel", TargetItems = "*", Context = "ListView", Criteria = "[Status] = 4", FontColor = "Red")]
    [Appearance("Alta", TargetItems = "*", Context = "ListView", Criteria = "[Status] = 1", FontColor = "Green")]
    [DefaultProperty("DisplayLook")]
    [XafDisplayName("Factura")]
    [ImageName("BO_Invoice")]
    public partial class DocumentoSalida : IDisposable
    {
        private Vendedor FVendedor;
        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        public Vendedor Vendedor
        {
            get { return FVendedor; }
            set
            {
                if (!IsLoading && value != null)
                    Comision = Convert.ToDecimal(value.Comision);
                SetPropertyValue("Vendedor", ref FVendedor, value);
            }
        }

        private DateTime FFechaEntrega;
        [NonCloneable]
        [ModelDefault("DisplayFormat", "{0:dd MMM yyyy}")]
        [VisibleInLookupListView(false)]
        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        public DateTime FechaEntrega
        {
            get { return FFechaEntrega; }
            set { SetPropertyValue("FechaEntrega", ref FFechaEntrega, value); }
        }

        //#region + Fecha de vencimiento
        /* Uso FechaVigencia
        public DateTime FFechaVencimiento;
        // [DbType("smalldatetime")]
        public DateTime FechaVencimiento
        {
            get { return FFechaEntrega; }
            set { SetPropertyValue("FechaVencimiento", ref FFechaVencimiento, value); }
        }*/

        private decimal FComision;
        [ModelDefault("DisplayFormat", "{0:n2}%")]
        [NonCloneable]
        [VisibleInLookupListView(false)]
        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        public decimal Comision
        {
            get { return FComision; }
            set { SetPropertyValue("Comision", ref FComision, value); }
        }

        //#region + Enviar a o entregar en !
        private string FEnviarA;
        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        [Size(120/*SizeAttribute.Unlimited*/)] // Estaba en 100 Oct 2018
        public string EnviarA
        {
            get { return FEnviarA; }
            set { SetPropertyValue("EnviarA", ref FEnviarA, value != null ? value.Trim(new char[] { ' ', '\0' }) : string.Empty); }
        }

        private Cliente FCliente;
        [VisibleInLookupListView(true)]
        [DataSourceCriteria("Status != 'Suspendido'"+
            " AND (ClntPrspct is null OR ClntPrspct.EnVnts)")]
        // [DataSourceCriteria("Status != 'Suspendido' & ClntPrspct is null")]
        //[DataSourceProperty("Clientes")]
        [Index(1)]
        [RuleRequiredField("RuleRequiredField for DocumentoSalida.Cliente", DefaultContexts.Save, "Debe capturar el Cliente", SkipNullOrEmptyValues = false)]
        public Cliente Cliente
        {
            get { return FCliente; }
            set
            {
                SetPropertyValue("Cliente", ref FCliente, value);
                if (!IsLoading && IsNewObject() && value != null)
                {
                    /*TIT Ver 3.3 Mrz 2018
                    AsignaFormaPago(value);*/
                    Vendedor = value.Vendedor;
                }
            }
        }

        /*
        private XPCollection<Cliente> clientes;
        [Browsable(false)] // Prohibits showing the collection separately 
        public XPCollection<Cliente> Clientes
        {
            get
            {
                if (clientes == null)
                {
                    clientes = new XPCollection<Cliente>(Session, false);
                    GroupOperator gp = new GroupOperator();

                    gp.Operands.Add(new BinaryOperator("Status", EStatusPrvdClnt.Activo));
                    // gp.Operands.Add(new BinaryOperator("ClntPrspct", null));
                }


                return clientes;
            }
        }*/


        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [NonPersistent]
        public string CliNombre
        {
            get
            {
                return Cliente == null || Cliente.Compania == null ? string.Empty : Cliente.Compania.Nombre;
            }
        }

        /* En Documento
        [VisibleInLookupListView(false)]
        [VisibleInListView(false)]
        [NonPersistent]
        public string Domicilio
        {
            get { return Cliente == null ? string.Empty : Cliente.Compania.Direccion.Domicilio; }
        }*/

        public void Dispose()
        {
            /* Ziur, no procede
            if (availableDirecciones != null)
            {
                availableDirecciones.Dispose();
                availableDirecciones = null;
            }*/
        }

        [Obsolete("Ya no usar para la versión 3.3")]
        private void AsignaFormaPago(Cliente value)
        {
            // Creo debe ser igual para notas credito cargo
            // if (Tipo == DocumentoTipo.Factura)
            // {
                bool find = false;

                if (value != null && value.FormasP != null)
                {
                    foreach (ItemFormaP item in value.FormasP)
                    {
                        if (item != null && item.Moneda != null && Moneda.Clave == item.Moneda.Clave)
                        {
                            NumCtaPago = item.NumCtaPago;
                            /*
                            FrmPago = item.FrmPago;
                            BanNom = item.BanNom;*/

                            FrmPg = item.FrmPg;
                            Banco = item.Banco;

                            find = true;
                            break;
                        }
                    }

                    if (!find)
                    {
                        NumCtaPago = "No Identificado";
                        /*
                        FrmPago = FormaPago.Indefinido;
                        BanNom = string.Empty;*/
                        FrmPg = Session.FindObject<Pago>(new BinaryOperator("Descrip", "Efectivo"/*FormaPago.Efectivo*//*.Indefinido No valido Abr 2014*//*.GetStringValue()*/));
                        Banco = null;
                    }
                }
            // }
        }

        public override ProveedorCliente Causante
        {
            get { return Cliente; }
        }

        // Otro cliente que tiene la dirección de entrega... 
        private Cliente FConsignatario;
        [VisibleInLookupListView(false)]
        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        public Cliente Consignatario
        {
            get { return FConsignatario; }
            set { SetPropertyValue("Consignatario", ref FConsignatario, value); }
        }

        private string FMostrador;
        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        [Size(SizeAttribute.Unlimited)]
        // [Delayed]
        public string Mostrador
        {
            get { return FMostrador; }
            set { SetPropertyValue("Mostrador", ref FMostrador, value != null ? value.Trim(new char[] { ' ', '\0' }) : string.Empty); }
        }

        //#region + Partidas
        /*>
        // Apply the Association attribute to mark the Orders property 
        // as the one end of the Customer-Orders association.
        [ExpandObjectMembers(ExpandObjectMembers.Always)]
        [Association("DocumentoSalida-Partidas", typeof(PartidaSalida)), DevExpress.Xpo.Aggregated]
        public XPCollection Partidas
        {
            get { return GetCollection("Partidas"); }
        }*/
        //#endregion

        // También será para el caso de una nota de crédito la fecha de la factura
        private DateTime mFechaPed;
        [VisibleInLookupListView(false)]
        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        public DateTime FechaPed
        {
            get { return mFechaPed; }
            set { SetPropertyValue("FechaPed", ref mFechaPed, value); }
        }

        /*TI Según yo se usaba en el CFD
        //#region + Folio
        // Para usarlo en el xml dirFolios
        // [NonPersistent]
        [Obsolete("Se usaba en el CFD ")]
        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        public Folio Folio
        {
            get { return GetPropertyValue<Folio>("Folio"); }
            set
            {
                SetPropertyValue("Folio", value);
                if (!IsLoading && value != null)
                {
                    Moneda = value.Moneda;
                    AsignaSecuencial(value);
                    / *
                    decimal ult;

                    ult = value.UltFolio + 1;
                    Serie = value.Serie;
                    Secuencial = ult.ToString("n0");* /
                }
            }
        }*/

            /*TI For cfd, now only cfdi
        //#region + Asigna Secuencial
        public void AsignaSecuencial(Folio value)
        {
            decimal ult;

            if (value != null)
            {
                ult = value.UltFolio + 1;
                Serie = value.Serie;
                Secuencial = ult.ToString("#");
            }
        }*/

        // Algunos serán redundantes con lo que se tiene?
        private string mSerie;
        /// <summary>
        /// De 1 a 25 caracteres, SAT
        /// </summary>
        [VisibleInLookupListView(false)]
        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        [Size(10)]
        public string Serie
        {
            get { return mSerie; }
            set { SetPropertyValue("Serie", ref mSerie, ValorString("Serie", value)); }
        }

        //#region + Serie certificado
        private string mSerieCert;
        [VisibleInLookupListView(false)]
        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        [Size(25)]
        public string SerieCert
        {
            get { return mSerieCert; }
            set { SetPropertyValue("SerieCert", ref mSerieCert, ValorString("SerieCert", value)); }
        }

        private string mSecuencial;
        /// <summary>
        /// Puede ser de 1 hasta 40 caracteres 
        /// </summary>
        [VisibleInLookupListView(true)]
        // [Appearance("Secuencial", Context = "DetailView", Enabled = false, FontStyle = FontStyle.Italic)]
        [VisibleInListView(false)]
        [RuleRequiredField("RuleRequiredField for DocumentoSalida.Secuencial", DefaultContexts.Save, "Debe capturar la Clave", SkipNullOrEmptyValues = false)]
        [Size(20)]
        public string Secuencial
        {
            get { return mSecuencial; }
            set { SetPropertyValue("Secuencial", ref mSecuencial, ValorString("Secuencial", value)); }
        }

        //#region + Aprobacion, número
        private int mAprobacion;
        [VisibleInLookupListView(false)]
        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        public int Aprobacion
        {
            get { return mAprobacion; }
            set { SetPropertyValue("Aprobacion", ref mAprobacion, value); }
        }

        //#region + Año, aprobacion
        private short mYearAprobacion;
        [Obsolete("Oct 2018, Era para el CFD?")]
        [VisibleInLookupListView(false)]
        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        public short YearAprobacion
        {
            get { return mYearAprobacion; }
            set { SetPropertyValue("YearAprobacion", ref mYearAprobacion, value); }
        }

        private decimal mRetenISR;
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        // Estaba habilitado, por qué lo dejé? Mar 2014
        // [Appearance("RetenISRE", Context = "DetailView", Enabled = false, FontStyle = FontStyle.Italic)]
        [Appearance("RetenISR", AppearanceItemType = "LayoutItem", Visibility = ViewItemVisibility.Hide, Context = "DetailView", Method = "MRetenISR")]
        [VisibleInListView(false)]
        public decimal RetenISR
        {
            get { return mRetenISR; }
            set { SetPropertyValue("RetenISR", ref mRetenISR, value); }
        }

        public bool MRetenISR()
        {
            if (Empresa == null /* TIT Ago 2018, se usa el esquema || !Empresa.ConRet*/)
            {
                return true;
            }
            else
            {
                /*
                if (Cliente != null && Cliente.Compania != null && !string.IsNullOrEmpty(Cliente.Compania.Rfc))
                {
                    if (Cliente.Compania.Rfc.Length >= 4)
                        return char.IsLetter(Cliente.Compania.Rfc, 3);
                }*/
                bool hide = true;

                foreach (PartidaSalida item in VentaItems)
                {
                    if (item.Producto.Tipo == EProductoTipo.Servicio && item.Producto.PRetIsr > 0)
                    {
                        hide = false;
                        break;
                    }
                }

                return hide;
            }
        }

        public bool MRetenIVA()
        {
            /*TIT Ago 2018, ahora se usa el esquema
            if (Empresa == null || !Empresa.ConRet)
            {
                return true;
            }
            else
            {*/
                bool hide = true;

                foreach (PartidaSalida item in VentaItems)
                {
                    if (item.Producto.Tipo == EProductoTipo.Servicio && item.Producto.PRetIva > 0)
                    {
                        hide = false;
                        break;
                    }
                }

                return hide;
            //}
        }

        private bool MRetenISR1()
        {
            if (Empresa == null /*TIT Ago 2018, ahora se usa esquema || Empresa.ConRet*/)
            {
                /*
                if (Cliente != null && Cliente.Compania != null && !string.IsNullOrEmpty(Cliente.Compania.Rfc))
                {
                    if (Cliente.Compania.Rfc.Length >= 4)
                        return !char.IsLetter(Cliente.Compania.Rfc, 3);
                }

                return true;*/
                bool hide = false;

                foreach (PartidaSalida item in VentaItems)
                {
                    if (item.Producto.Tipo == EProductoTipo.Servicio && item.Producto.PRetIsr > 0)
                    {
                        hide = true;
                        break;
                    }
                }

                return hide;
            }
            else
                return false;
        }

        private bool MSinCfdi()
        {
            return IsNewObject() || Empresa == null || !Empresa.ConCfdi;
        }

        public bool MDsctT()
        {
            return true;
        }

        private decimal mRetenIVA;
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        // Estaba habilitado, por qué lo dejé habilitado ? Mar 2014
        // [Appearance("RetenIVAE", Context = "DetailView", Enabled = false, FontStyle = FontStyle.Italic)]
        [Appearance("RetenIVA", AppearanceItemType = "LayoutItem", Visibility = ViewItemVisibility.Hide, Context = "DetailView", Method = "MRetenIVA")]
        [VisibleInListView(false)]
        public decimal RetenIVA
        {
            get { return mRetenIVA; }
            set { SetPropertyValue("RetenIVA", ref mRetenIVA, value); }
        }

        [RuleFromBoolProperty("DocSPorcD", DefaultContexts.Save, "El Porcentaje de Descuento debe estar entre 0 y 100")]
        protected bool DocSPorcD
        {
            get { return PorcDesc >= 0 && PorcDesc < 100; }
        }

        //#region + Porcentaje Descuento
        private float mPorcDesc;
        [XafDisplayName("% Descuento")]
        [VisibleInLookupListView(false)]
        [VisibleInListView(false)]
        [ModelDefault("DisplayFormat", "{0:n2}%")]
        public float PorcDesc
        {
            get { return mPorcDesc; }
            set { SetPropertyValue("PorcDesc", ref mPorcDesc, value); }
        }

        [RuleFromBoolProperty("DocSPorcDF", DefaultContexts.Save, "El Porcentaje de Descuento Financiero debe estar entre 0 y 100")]
        protected bool DocSPorcDF
        {
            get { return PorcDescFinan >= 0 && PorcDescFinan < 100; }
        }

        //#region + Porcentaje Descuento Financiero
        private float mPorcDescFinan;
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        [ModelDefault("DisplayFormat", "{0:n2}%")]
        [VisibleInListView(false)]
        public float PorcDescFinan
        {
            get { return mPorcDescFinan; }
            set { SetPropertyValue("PorcDescFinan", ref mPorcDescFinan, value); }
        }
        
        /* TI Usar Pago
        private FormaPago mFrmPago;
        [Obsolete("En su lugar usar FrmPg")]
        [VisibleInLookupListView(false)]
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        public FormaPago FrmPago
        {
            get { return mFrmPago; }
            set { SetPropertyValue("FrmPago", ref mFrmPago, value); }
        }*/

        /* TIT Sep 2018
        /// <summary>
        /// Motivo Descuento
        /// </summary>
        private EMotivoDesc mMotivDesc;
        [Obsolete("Ahora usamos MtvDscn")]
        [VisibleInLookupListView(false)]
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        public EMotivoDesc MotivDesc
        {
            get { return mMotivDesc; }
            set { SetPropertyValue("MotivDesc", ref mMotivDesc, value); }
        }*/

        //#region + Leyenda de Pago
        private ELeyendaPago mLeyendaPago;
        [VisibleInLookupListView(false)]
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        public ELeyendaPago LeyendaPago
        {
            get { return mLeyendaPago; }
            set { SetPropertyValue("LeyendaPago", ref mLeyendaPago, value); }
        }

        private string mSello;
        [NonCloneable]
        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        [Size(SizeAttribute.Unlimited)]
        public string Sello
        {
            get { return mSello; }
            set { SetPropertyValue("Sello", ref mSello, value); }
        }

        private string mCadenaOriginal;
        [NonCloneable]
        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        // [NonPersistent]
        [Size(SizeAttribute.Unlimited)]
        public string CadenaOriginal 
        { 
            get
            {
                /*TI Yo digo que fue un obsolete !
                if (!string.IsNullOrEmpty(mCadenaOriginal) && mCadenaOriginal.Substring(0, 5) == "||3.2")
                    mCadenaOriginal = FCap.Module.Utilerias.FacturaE.CadenaOriginalT(string.Empty, this, null);*/

                return mCadenaOriginal; 
            }
            set { SetPropertyValue("CadenaOriginal", ref mCadenaOriginal, value); } 
        }

        /*
        private string mReferenciaTipo;
        [Size(1)]
        public string ReferTipo
        {
            get { return mReferenciaTipo; }
            set { SetPropertyValue("ReferTipo", ref mReferenciaTipo, value); }
        }*/

        private string mReferencia;
        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        [Size(10)]
        public string Referencia
        {
            get { return mReferencia; }
            set { SetPropertyValue("Referencia", ref mReferencia, ValorString("Referencia", value)); }
        }

        //#region + Autoriza, creo que se refiere a quien está autorizando el Documento
        private string mAutoriza;
        [VisibleInLookupListView(false)]
        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        [Size(10)]
        public string Autoriza
        {
            get { return mAutoriza; }
            set { SetPropertyValue("Autoriza", ref mAutoriza, ValorString("Autoriza", value)); }
        }

        // #region + Anticipo, para LG, no recuerdo que lo estemos usando !
        // También se usa para las constructoras ! AXA
        //
        private decimal mAnticipo;
        /// <summary>
        /// Amortización del Anticipo, para las constructoras, usualmente del 30 % ?
        /// </summary>
        [VisibleInLookupListView(false)]
        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        public decimal Anticipo
        {
            get { return mAnticipo; }
            set { SetPropertyValue("Anticipo", ref mAnticipo, value); }
        }

        private string mNumCtaPago;
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        [XafDisplayName("Núm. Cta. Pago")]
        [VisibleInListView(false)]
        [Size(30)]
        public string NumCtaPago
        {
            get { return mNumCtaPago; }
            set { SetPropertyValue("NumCtaPago", ref mNumCtaPago, ValorString("NumCtaPago", value)); }
        }

        private string mBanNom;
        [VisibleInLookupListView(false)]
        [ModelDefault("PropertyEditorType", "FCap.Module.Win.Editors.CustomStringEditor")]
        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        // [NonPersistent]
        [Size(20)]
        public string BanNom
        {
            get { return mBanNom; }
            set { SetPropertyValue("BanNom", ref mBanNom, value); }
        }

        private string mLeyAntcp;
        [VisibleInLookupListView(false)]
        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        [Size(30)]
        public string LeyAntcp
        {
            get { return mLeyAntcp; }
            set { SetPropertyValue("LeyAntcp", ref mLeyAntcp, ValorString("LeyAntcp", value)); }
        }

        // May 2013 Para LG pero creo que si se ocupará.
        [VisibleInLookupListView(false)]
        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        [NonPersistent]
        public string DocumentoAnterior { get; set; }

        // May 2013 Para el cfdi
        [NonCloneable]
        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        // [NonPersistent]
        [Size(SizeAttribute.Unlimited)]
        public string SelloSat { get; set; }

        private string mUuid;
        [NonCloneable]
        [VisibleInLookupListView(false)]
        //[Appearance("Uuid", Context = "DetailView", Enabled = false, 
        //    FontStyle = FontStyle.Italic)]
        [Appearance("UuidV", AppearanceItemType = "LayoutItem",
            // Context = "DetailView", Visibility = ViewItemVisibility.Hide, Method = "MSinCfdi")]
            Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "IsNullOrEmpty(Uuid)")]
        [VisibleInListView(false)]
        [VisibleInDetailView(true)]
        [Size(60)]
        public string Uuid
        {
            get { return mUuid; }
            set { SetPropertyValue("Uuid", ref mUuid, ValorString("Uuid", value)); }
        }

        [NonCloneable]
        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        // [NonPersistent]
        [Size(SizeAttribute.Unlimited)]
        public string CertificadoSat { get; set; }


        [VisibleInLookupListView(false)]
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [NonPersistent]
        public string DisplayLook
        {
            get { return string.Format("{0} - {1}...", Clave, Cliente != null && Cliente.Compania != null ? Cliente.Compania.Nombre.Length > 10 ? Cliente.Compania.Nombre.Substring(0, 5) : Cliente.Compania.Nombre : string.Empty); }
        }

        private string mSelloSatCan;
        [NonCloneable]
        // [Appearance("SelloSatCan", Context = "DetailView", Enabled = false, FontStyle = FontStyle.Italic)]
        // [Appearance("SelloSatCanV", AppearanceItemType = "LayoutItem", Context = "DetailView", Criteria = "Status != 4", Visibility = ViewItemVisibility.Hide)]
        [Appearance("SelloSatCanV", AppearanceItemType = "LayoutItem", Context = "DetailView", Criteria = "IsNullOrEmpty(SelloSatCan)", Visibility = ViewItemVisibility.Hide)]
        [VisibleInListView(false)]
        [VisibleInDetailView(true)]
        [Size(SizeAttribute.Unlimited)]
        public string SelloSatCan
        {
            get { return mSelloSatCan; }
            set { SetPropertyValue("SelloSatCan", ref mSelloSatCan, value); }
        }

        private readonly XPDelayedProperty bytesImagen = new XPDelayedProperty();
        [VisibleInDetailView(false)]
        [Delayed("bytesImagen")]
        [ValueConverter(typeof(JpegStorageConverter))]
        [NonPersistent]
        public Image CBB
        {
            get { return (Image)bytesImagen.Value; }
            set
            {
                bytesImagen.Value = value;
                if (!IsLoading)
                    OnChanged("CBB");
            }
        }

        [VisibleInLookupListView(false)]
        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        [NonPersistent]
        public string CadenaCBB { get; set; }

        // Activar o no la action enviar correo
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        [NonPersistent]
        public bool SendMail
        {
            get { return Cliente != null && Cliente.Compania != null && Cliente.Compania.Direccion != null && !string.IsNullOrEmpty(Cliente.Compania.Direccion.Email); }
            set { }
        }

        /*
        private Direccion mDireccion;
        [Obsolete("Ziur, no procede")]
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        [VisibleInListView(false)]
        [ModelDefault("DataSourceProperty", "AvailableDirecciones")]
        [Appearance("Direccion", AppearanceItemType = "LayoutItem", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Method = "UnaDir")]
        /// <summary>
        /// Direccion del cliente, que puede tener varias.
        /// </summary>
        public Direccion Direccion
        {
            get { return mDireccion; }
            set { SetPropertyValue("Direccion", ref mDireccion, value); }
        }

        private XPCollection<Direccion> availableDirecciones;
        [Obsolete("Ziur, no procede")]
        [VisibleInDetailView(false)]
        public XPCollection<Direccion> AvailableDirecciones
        {
            get
            {
                if (availableDirecciones == null)
                    availableDirecciones = new XPCollection<Direccion>(Session);

                if (Cliente != null && Cliente.Compania != null)
                {
                    availableDirecciones.Criteria = new BinaryOperator("Compania", Cliente.Compania);
                }
                else
                {                    
                    availableDirecciones.Criteria = null;
                }
                return availableDirecciones;

            }
        }*/

        private Banco mBanco;
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        [VisibleInListView(false)]
        public Banco Banco
        {
            get { return mBanco; }
            set { SetPropertyValue("Banco", ref mBanco, value); }
        }

        /*
        private string FClave;
        [VisibleInListView(true)]
        [VisibleInDetailView(false)]
        [Index(1)]
        [Indexed("Tipo", Unique = true)]
        // [Indexed(Unique = true)]
        [Size(LONCVE)]
        new public string Clave
        {
            get { return FClave; }
            set
            {
                if (value != null)
                {
                    if (IsLoading)
                    {
                        SetPropertyValue("Clave", ref FClave, value);
                    }
                    else
                    {
                        SetPropertyValue("Clave", ref FClave, ValorString("Clave", value));

                        if (Cadena.IsNumber(FClave))
                            SetPropertyValue("Clave", ref FClave, FClave.PadLeft(LONCVE, ' '));
                    }
                }
            }
        }*/

        private Pago mFrmPg;
        [ImmediatePostData]
        [XafDisplayName("Forma de Pago")]
        [RuleRequiredField("RuleRequiredField for DocumentoSalida.FrmPg", DefaultContexts.Save, "Debe capturar la Forma de Pago", SkipNullOrEmptyValues = false)]
        [VisibleInLookupListView(false)]
        [VisibleInListView(false)]
        [DataSourceCriteria("Tipo == 0")]
        public Pago FrmPg
        {
            get { return mFrmPg; }
            set { SetPropertyValue("FrmPg", ref mFrmPg, value); }
        }

        private Pago mLyndPg;
        [XafDisplayName("Método de Pago")]
        [RuleRequiredField("RuleRequiredField for DocumentoSalida.LyndPg", DefaultContexts.Save, "Debe capturar el Método de Pago", SkipNullOrEmptyValues = false)]
        [VisibleInLookupListView(false)]
        [DataSourceCriteria("Tipo == 'Metodo'")]
        [VisibleInListView(false)]
        public Pago LyndPg
        {
            get { return mLyndPg; }
            set { SetPropertyValue("LyndPg", ref mLyndPg, value); }
        }

        private Pago mMtvDscnt;
        [VisibleInDetailView(false)]
        [XafDisplayName("Motivo de Descuento")]
        [VisibleInLookupListView(false)]
        [DataSourceCriteria("Tipo == 2")]
        [VisibleInListView(false)]
        public Pago MtvDscnt
        {
            get { return mMtvDscnt ; }
            set { SetPropertyValue("MtvDscnt", ref mMtvDscnt, value); }
        }

        /* Lo tiene en la clase base
        private string mDocEnlace;
        // Lo dejo asi o lo cambio?
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [NonPersistent]
        public string DocEnlace
        {
            get { return mDocEnlace; }
            set { SetPropertyValue("DocEnlace", ref mDocEnlace, value); }
        }*/

        /* Ago 2015 no jaló esta prueba porque DocumenSal es Nonpersistent
        private DocumenSal mDcEnlace;
        public DocumenSal DcEnlace
        {
            get { return mDcEnlace; }
            set { SetPropertyValue("DcEnlace", ref mDcEnlace, value); }
        }*/



        // >>> Carta Porte
        private string mRecogerEn; // Recoger en
        [Obsolete("En su lugar usar CartaPorte")]
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        // [Appearance("DocumentoSalida.RecogerEn", AppearanceItemType = "LayoutItem", Visibility = ViewItemVisibility.Hide, Context = "DetailView", Method = "CartaPorte")]
        [Size(40)]
        public string RecogerEn
        {
            get { return mRecogerEn; }
            set { SetPropertyValue("RecogerEn", ref mRecogerEn, value != null ? value.Trim(new char[] { ' ', '\0' }) : string.Empty); }
        }

        // Cuota Convenida
        private string mCuotaC;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        // [Appearance("DocumentoSalida.CuotaC", AppearanceItemType = "LayoutItem", Visibility = ViewItemVisibility.Hide, Context = "DetailView", Method = "CartaPorte")]
        [Size(15)]
        public string CuotaC
        {
            get { return mCuotaC; }
            set { SetPropertyValue("CuotaC", ref mCuotaC, value); }
        }

        // Valor Declarado, no sé si es sólo un SI o un NO
        private string mValorD;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        // [Appearance("DocumentoSalida.ValorD", AppearanceItemType = "LayoutItem", Visibility = ViewItemVisibility.Hide, Context = "DetailView", Method = "CartaPorte")]
        [Size(10)]
        public string ValorD
        {
            get { return mValorD; }
            set { SetPropertyValue("ValorD", ref mValorD, value); }
        }

        // Reembarco
        private string mReemb;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        // [Appearance("DocumentoSalida.Reemb", AppearanceItemType = "LayoutItem", Visibility = ViewItemVisibility.Hide, Context = "DetailView", Method = "CartaPorte")]
        [Size(40)]
        public string Reemb
        {
            get { return mReemb; }
            set { SetPropertyValue("Reemb", ref mReemb, value); }
        }

        // Reembarcarse con
        private string mReembCn;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        // [Appearance("DocumentoSalida.ReembCn", AppearanceItemType = "LayoutItem", Visibility = ViewItemVisibility.Hide, Context = "DetailView", Method = "CartaPorte")]
        [Size(40)]
        public string ReembCn
        {
            get { return mReembCn; }
            set { SetPropertyValue("ReembCn", ref mReembCn, value); }
        }

        private string mConduceDe;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        // [Appearance("DocumentoSalida.ConduceDe", AppearanceItemType = "LayoutItem", Visibility = ViewItemVisibility.Hide, Context = "DetailView", Method = "CartaPorte")]
        [Size(30)]
        public string ConduceDe
        {
            get { return mConduceDe; }
            set { SetPropertyValue("ConduceDe", ref mConduceDe, value); }
        }

        private string mConduceA;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        // [Appearance("DocumentoSalida.CuotaA", AppearanceItemType = "LayoutItem", Visibility = ViewItemVisibility.Hide, Context = "DetailView", Method = "CartaPorte")]
        [Size(30)]
        public string ConduceA
        {
            get { return mConduceA; }
            set { SetPropertyValue("ConduceA", ref mConduceA, value); }
        }

        private decimal mFlete;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        // [Appearance("DocumentoSalida.Flete", AppearanceItemType = "LayoutItem", Visibility = ViewItemVisibility.Hide, Context = "DetailView", Method = "CartaPorte")]
        public decimal Flete
        {
            get { return mFlete; }
            set { SetPropertyValue("Flete", ref mFlete, value); }
        }

        private decimal mSeguro;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        // [Appearance("DocumentoSalida.Seguro", AppearanceItemType = "LayoutItem", Visibility = ViewItemVisibility.Hide, Context = "DetailView", Method = "CartaPorte")]
        public decimal Seguro
        {
            get { return mSeguro; }
            set { SetPropertyValue("Seguro", ref mSeguro, value); }
        }

        // Recolección
        private decimal mRecolecc;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        // [Appearance("DocumentoSalida.Recolecc", AppearanceItemType = "LayoutItem", Visibility = ViewItemVisibility.Hide, Context = "DetailView", Method = "CartaPorte")]
        public decimal Recolecc
        {
            get { return mRecolecc; }
            set { SetPropertyValue("Recolecc", ref mRecolecc, value); }
        }

        // Entrega a domicilio
        private decimal mEntregaD;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        // [Appearance("DocumentoSalida.EntregaD", AppearanceItemType = "LayoutItem", Visibility = ViewItemVisibility.Hide, Context = "DetailView", Method = "CartaPorte")]
        public decimal EntregaD
        {
            get { return mEntregaD; }
            set { SetPropertyValue("EntregaD", ref mEntregaD, value); }
        }

        // Maniobras
        private decimal mManio;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        // [Appearance("DocumentoSalida.Manio", AppearanceItemType = "LayoutItem", Visibility = ViewItemVisibility.Hide, Context = "DetailView", Method = "CartaPorte")]
        // [Size(10)]
        public decimal Manio
        {
            get { return mManio; }
            set { SetPropertyValue("Manio", ref mManio, value); }
        }

        private decimal mAutopis;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        // [Appearance("DocumentoSalida.Autopis", AppearanceItemType = "LayoutItem", Visibility = ViewItemVisibility.Hide, Context = "DetailView", Method = "CartaPorte")]
        // [Size(10)]
        public decimal Autopis
        {
            get { return mAutopis; }
            set { SetPropertyValue("Autopis", ref mAutopis, value); }
        }

        private decimal mOtros;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        // [Appearance("DocumentoSalida.Otros", AppearanceItemType = "LayoutItem", Visibility = ViewItemVisibility.Hide, Context = "DetailView", Method = "CartaPorte")]
        // [Size(10)]
        public decimal Otros
        {
            get { return mOtros; }
            set { SetPropertyValue("Otros", ref mOtros, value); }
        }

        private string mPlcs;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        // [Appearance("DocumentoSalida.Plcs", AppearanceItemType = "LayoutItem", Visibility = ViewItemVisibility.Hide, Context = "DetailView", Method = "CartaPorte")]
        [Size(10)]
        public string Plcs
        {
            get { return mPlcs; }
            set { SetPropertyValue("Plcs", ref mPlcs, value); }
        }

        private string mOprdr;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        // [Appearance("DocumentoSalida.Oprdr", AppearanceItemType = "LayoutItem", Visibility = ViewItemVisibility.Hide, Context = "DetailView", Method = "CartaPorte")]
        [Size(20)]
        public string Oprdr
        {
            get { return mOprdr; }
            set { SetPropertyValue("Oprdr", ref mOprdr, value); }
        }

        private string mTransprt;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        // [Appearance("DocumentoSalida.Transprt", AppearanceItemType = "LayoutItem", Visibility = ViewItemVisibility.Hide, Context = "DetailView", Method = "CartaPorte")]
        [Size(20)]
        public string Transprt
        {
            get { return mTransprt; }
            set { SetPropertyValue("Transprt", ref mTransprt, value); }
        }

        private Compania mDstn;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        // [Appearance("DocumentoSalida.Dstn", AppearanceItemType = "LayoutItem", Visibility = ViewItemVisibility.Hide, Context = "DetailView", Method = "CartaPorte")]
        public Compania Dstn
        {
            get { return mDstn; }
            set { SetPropertyValue("Dstn", ref mDstn, value); }
        }
        // <<< Carta Porte



        private bool mConcilia;
        [VisibleInListView(false)]
        [NonCloneable]
        // [Appearance("Concilia", Context = "DetailView", Enabled = false, FontStyle = FontStyle.Italic)]
        [VisibleInDetailView(false)]
        public bool Concilia
        {
            get { return mConcilia; }
            set { SetPropertyValue("Concilia", ref mConcilia, value); }
        }

        private Pago mUsCfdi;
        [XafDisplayName("Uso de CFDI")]
        [RuleRequiredField("RuleRequiredField for DocumentoSalida.UsoCfdi", DefaultContexts.Save, "Debe capturar el Uso del Cfdi", SkipNullOrEmptyValues = false)]
        [VisibleInLookupListView(false)]
        [DataSourceCriteria("Tipo == 'UsoCFDI'")]
        [VisibleInListView(false)]
        public Pago UsCfdi
        {
            get { return mUsCfdi; }
            set { SetPropertyValue("UsCfdi", ref mUsCfdi, value); }
        }

        // Apply the Association attribute to mark the Orders property 
        // as the one end of the Customer-Orders association.
        [ExpandObjectMembers(ExpandObjectMembers.Always)]
        [Association("DocumentoSalida-VentaItems", typeof(PartidaSalida)), DevExpress.Xpo.Aggregated]
        public XPCollection VentaItems
        {
            get { return GetCollection("VentaItems"); }
            /*
            get 
            { 
                XPCollection itms = GetCollection("Partidas");
                if (itms.Sorting.Count <= 0 / * == null* /)
                    itms.Sorting = new SortingCollection(new SortProperty("Item", SortingDirection.Ascending));
                return itms;
            }
             */
        }

        [Appearance("DocumentoSalida.Portfolio", AppearanceItemType = "LayoutItem", Visibility = ViewItemVisibility.Hide, Context = "DetailView", Criteria = "Portfolio.Count = 0")]
        [NonCloneable]
        [XafDisplayName("Archivos")]
        [DevExpress.Xpo.Aggregated, Association("Venta-PortfolioFileData")]
        public XPCollection<PortfolioFileData> Portfolio
        {
            get { return GetCollection<PortfolioFileData>("Portfolio"); }
        }

        private Regimen mRgmn;
        [VisibleInListView(false)]
        [RuleRequiredField("RuleRequiredField for DocumentoSalida.Rgmn", DefaultContexts.Save, "Debe capturar el Régimen", SkipNullOrEmptyValues = false)]
        [XafDisplayName("Régimen")]
        [DataSourceProperty("Regimenes")]
        public Regimen Rgmn
        {
            get { return mRgmn; }
            set { SetPropertyValue("Rgmn", ref mRgmn, value); }
        }

        private Pago mTpRlcn;
        [DataSourceCriteria("Tipo == 'TipoRelacion'")]
        [VisibleInListView(false)]
        [XafDisplayName("Tipo Relación")]
        public Pago TpRlcn
        {
            get { return mTpRlcn; }
            set { SetPropertyValue("TpRlcn", ref mTpRlcn, value); }
        }

        [Appearance("DocumentoSalida.RelacionadosUuid", AppearanceItemType = "LayoutItem", Visibility = ViewItemVisibility.Hide, Context = "DetailView", Criteria = "RelacionadosUuid.Count = 0")]
        [Association("DocumentoSalida-RelacionadosUuid", typeof(RelacionadoUuid)), DevExpress.Xpo.Aggregated]
        public XPCollection RelacionadosUuid
        {
            get { return GetCollection("RelacionadosUuid"); }
        }

        private ECancelable? mCnclbl;
        [NonPersistent]
        [XafDisplayName("Cancelable")]
        public ECancelable? Cnclbl
        {
            get { return mCnclbl; }
            set { SetPropertyValue("Cnclbl", ref mCnclbl, value); }
        }



        public override void AfterConstruction()
        {
            base.AfterConstruction();

            Autoriza = string.Empty;
            Condicion = string.Empty;
            Cliente = null;
            Consignatario = null;
            FechaEntrega = DateTime.Today;
            /*TI
            FrmPago = FormaPago.Efectivo; // Abr 2014 Se supone que no está permitido !.Indefinido;*/
            LeyendaPago = ELeyendaPago.EnUnaSola;
            Sello = string.Empty;
            CadenaOriginal = string.Empty;
            Referencia = string.Empty;
            // FechaVencimiento = DateTime.Today;
            Vendedor = null;
            SuPedido = string.Empty;
            Anticipo = 0;
            NumCtaPago = "No identificado";
            BanNom = string.Empty;
            LeyAntcp = string.Empty;

            DocumentoAnterior = string.Empty;
            SelloSat = string.Empty;
            Uuid = string.Empty;
            CertificadoSat = string.Empty;
            Tipo = DocumentoTipo.Factura;

            /*TI
            MotivDesc = EMotivoDesc.Ninguno;*/
            FrmPg = Session.FindObject<Pago>(new BinaryOperator("Descrip", "EFECTIVO"/*TI FrmPago.GetStringValue()*/));
            LyndPg = Session.FindObject<Pago>(new BinaryOperator("Clv", "PUE" /*LeyendaPago.GetStringValue()*/));
            MtvDscnt = Session.FindObject<Pago>(new BinaryOperator("Descrip", EMotivoDesc.Ninguno.GetStringValue()));
            
            /*TI Se movió al negocio
            Ventas vta = Session.FindObject<Ventas>(null);
            if (vta != null && !string.IsNullOrEmpty(vta.Serie))
                Serie = vta.Serie;
            else 
                Serie = "A";*/

            Concilia = false;

            /* Ya lo tiene la clase base
            if (mEmpresa == null)
                mEmpresa = Session.FindObject<Empresa>(null);

            if (mEmpresa != null && mEmpresa.Regimenes != null && mEmpresa.Regimenes.Count > 0)
                Rgmn = (mEmpresa.Regimenes[0] as RegimenEmpresa).Rgmn;*/

            if (Empresa != null && Empresa.Regimenes != null && Empresa.Regimenes.Count > 0)
                Rgmn = (Empresa.Regimenes[0] as RegimenEmpresa).Rgmn;
        }


        /* Ziur, no procede
        private bool UnaDir()
        {
            return true;
            // return Cliente == null || Cliente.Compania == null || Cliente.Compania.Direcciones == null || Cliente.Compania.Direcciones.Count == 0;
        }*/



        public override XPCollection LasPartidas()
        {
            return VentaItems;
        }

        private bool CartaPorte()
        {
            return Tipo != DocumentoTipo.CartaPorte;
        }

        // private Empresa mEmpresa; Ya lo tiene la clase base 
        private XPCollection<Regimen> regimenes;
        [Browsable(false)] // Prohibits showing the collection separately 
        public XPCollection<Regimen> Regimenes
        {
            get
            {
                if (Empresa == null)
                {
                    Empresa = Session.FindObject<Empresa>(null);
                }
                if (regimenes == null)
                    regimenes = new XPCollection<Regimen>(Session, false);
                /*
                // Filter the retrieved collection according to the current conditions 
                RefreshAvailablePersonas();
                // Return the collection with parents
                return padres;*/

                if (Empresa != null)
                {
                    foreach (RegimenEmpresa rg in Empresa.Regimenes)
                        regimenes.Add(rg.Rgmn);
                }

                return regimenes;
            }
        }
    }

    public enum ECancelable
    {
        NoCancelable,
        SinAceptacion,
        ConAceptacion
    }


    /*TI Parece que está obsoleto Jul 2017*/
    [Obsolete("En su lugar usar Pago")]
    public enum EMotivoDesc
    {
        [XafDisplayName("--NINGUNO--"), StringValue("--NINGUNO--")]
        Ninguno = 1,
        [XafDisplayName("PRONTO PAGO"), StringValue("PRONTO PAGO")]
        ProntoPago = 2,
        Indefinido = 0
    }

    public enum DocumentType
    {
        Xml = 1, Pdf = 2, /*Documentation = 3,
        Diagrams = 4, ScreenShots = 5,*/ Unknown = 6
    }
}