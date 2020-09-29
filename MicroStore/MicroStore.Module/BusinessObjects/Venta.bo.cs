using Cap.Generales.BusinessObjects.Empresa;
using Cap.Inventarios.BusinessObjects;
using Cap.Personas.BusinessObjects;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.ComponentModel;
using System.Drawing;

namespace MicroStore.Module.BusinessObjects
{
    [Appearance("Venta.Cancelada", TargetItems = "Stts", Context = "ListView", Criteria = "Stts = 'Cancelada'", FontColor = "Red")]
    [Appearance("Venta.Edit", AppearanceItemType = "ViewItem", TargetItems = "*", Context = "DetailView", Enabled = false, Criteria = "!IsNewObject(This)")]
    [NavigationItem("Ventas")]
    public partial class Venta
    {
        /* TI Primero lo primero, uso de ratón luego vemos si es necesario cambiar !
        private string mPrdctCptr;
        [ImmediatePostData]
        [VisibleInListView(false)]
        [XafDisplayName("Producto")]
        [NonPersistent]
        public string PrdctCptr
        {
            get { return !CnLt ? string.Empty : mPrdctCptr; }
            set
            {
                SetPropertyValue("PrdctCptr", ref mPrdctCptr, value);
                if (!string.IsNullOrEmpty(value))
                {
                    Producto prd = Session.FindObject<Producto>(new BinaryOperator("Clv", value));
                    
                    if (prd != null)
                    {
                        CnLt = prd.Lt;
                        if (!prd.Lt)
                        {
                            VentaItem itm = new VentaItem(Session);

                            itm.Cntdd = Cntdd;
                            itm.Prdct = prd;
                            itm.Prc = itm.Prdct.Prc;

                            ItemsVenta.Add(itm);
                            UpdateOrdersTotal(true);
                        }
                    }
                }
            }
        }*/

        /*
        [Appearance("Venta.LtCptr", AppearanceItemType = "LayoutItem", Context = "DetailView", Criteria = "!Prdct.Lt", Visibility = ViewItemVisibility.Hide)]
        [NonPersistent]
        public string LtCptr;*/

        private Lote mLt;
        [XafDisplayName("Lote")]
        [VisibleInListView(false)]
        // [DataSourceProperty("Lotes")]
        [DataSourceCriteriaProperty("Criterion")]
        [Appearance("Venta.Lt", AppearanceItemType = "LayoutItem", Context = "DetailView", Criteria = "!Prdct.Lotes", Visibility = ViewItemVisibility.Hide)]
        [NonPersistent]
        public Lote Lt
        {
            get { return mLt; }
            set { SetPropertyValue("Lt", ref mLt, value); }
        }

        private CriteriaOperator _Criterion;
        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        public CriteriaOperator Criterion
        {
            get
            {
                return _Criterion;
            }
            set
            {
                SetPropertyValue("Criterion", ref _Criterion, value);
                // Refresh the Tasks property data source
                // RefreshAvailableTasks();
            }
        }

        [VisibleInLookupListView(false)]
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [NonPersistent]
        public bool CnLt;

        [Persistent("Ttl")]
        private decimal? mTtl = null;
        [DevExpress.Xpo.DisplayName("Total")]
        [PersistentAlias("mTtl")]
        public decimal? Ttl
        {
            get
            {
                /* Sep 2020
                if (!IsLoading && !IsSaving && mTtl == null)
                    UpdateVentaTotal(false);*/
                return mTtl;
            }
            set 
            {
                SetPropertyValue("Ttl", ref mTtl, value);
            }
        }

        [VisibleInListView(false)]
        [ImmediatePostData]
        [NonPersistent]
        [XafDisplayName("Pago")]
        public decimal Pg { get; set; }

        [VisibleInListView(false)]
        [NonPersistent]
        [XafDisplayName("Cambio")]
        public decimal Cmb
        {
            get { return Pg == 0 ? 0 : Pg - Convert.ToDecimal(Ttl); }
        }

        private Producto mPrdct;
        /*
        [VisibleInDetailView(false)]*/
        [ImmediatePostData]
        [VisibleInListView(false)]
        [LookupEditorMode(LookupEditorMode.AllItemsWithSearch)]
        [DevExpress.Xpo.DisplayName("Producto")]
        [NonPersistent]
        public Producto Prdct
        {
            get { return mPrdct; }
            set
            {
                SetPropertyValue("Prdct", ref mPrdct, value);

                GroupOperator criterion = new GroupOperator();

                criterion.Operands.Add(new BinaryOperator("Prdct", value));
                criterion.Operands.Add(new BinaryOperator("Rmnnt", 0, BinaryOperatorType.Greater));

                SetPropertyValue("Criterion", ref _Criterion, criterion);
                OnChanged("Lt");

                /*TI Primero lo primero
                if (value != null)
                {
                    VentaItem itm = new VentaItem(Session);

                    itm.Cntdd = Cntdd;
                    itm.Prdct = Prdct;
                    itm.Prc = value.Prc;

                    ItemsVenta.Add(itm);
                    UpdateOrdersTotal(true);
                    Prdct = null;
                }*/
            }
        }

        private float mCntdd;
        [ImmediatePostData]
        [VisibleInListView(false)]
        [DevExpress.Xpo.DisplayName("Cantidad")]
        [NonPersistent]
        public float Cntdd 
        { 
            get { return mCntdd; }
            set { SetPropertyValue("Cntdd", ref mCntdd, value); }
        }

        private string mFl;
        [Appearance("Venta.Fl", Context = "DetailView", Enabled = false, FontStyle = FontStyle.Italic)]
        [DevExpress.Xpo.DisplayName("Folio")]
        [Size(10)]
        public string Fl
        {
            get { return mFl; }
            set { SetPropertyValue("Fl", ref mFl, value); }
        }

        private DateTime mFchVnt;
        [Appearance("Venta.FchVnt", Context = "DetailView", Enabled = false, FontStyle = FontStyle.Italic)]
        [ModelDefault("DisplayFormat", "{0:dd MMM yyyy | HH:mm}")]
        [DevExpress.Xpo.DisplayName("Fecha Venta")]
        public DateTime FchVnt
        {
            get { return mFchVnt; }
            set { SetPropertyValue("FchVnt", ref mFchVnt, value); }
        }

        private EEstadoVenta mStts;
        [Appearance("Venta.Stts", Context = "DetailView", Enabled = false, FontStyle = FontStyle.Italic)]
        [XafDisplayName("Status")]
        public EEstadoVenta Stts
        {
            get { return mStts; }
            set { SetPropertyValue("Stts", ref mStts, value); }
        }

        [Association("Venta-ItemsVenta", typeof(VentaItem)), DevExpress.Xpo.Aggregated]
        public XPCollection ItemsVenta
        {
            get { return GetCollection("ItemsVenta"); }
        }

        // Descuento

        private XPCollection<Lote> mLotes;
        [Browsable(false)] // Prohibits showing the collection separately 
        public XPCollection<Lote> Lotes
        {
            get
            {
                if (mLotes == null)
                {
                    // Creat collection
                    mLotes = new XPCollection<Lote>(Session);
                }
                // Filter the retrieved collection according to the current conditions 
                RefreshAvailableLotes();
                // Return the collection with parents
                return mLotes;
            }
        }

        private decimal mImpuesto04;
        [Appearance("Documento.Impuesto04", AppearanceItemType = "LayoutItem", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "Impuesto04 = 0")]
        [Appearance("Impuesto04", Context = "DetailView", Enabled = false, FontStyle = FontStyle.Italic)]
        public decimal Impuesto04
        {
            get { return mImpuesto04; }
            set { SetPropertyValue("Impuesto04", ref mImpuesto04, value); }
        }

        /*
        private Oferta mOfrt;
        public Oferta Ofrt
        {
            get { return mOfrt; }
            set
            {
                SetPropertyValue("Ofrt", mOfrt, value);
            }
        }*/


        private void RefreshAvailableLotes()
        {
            if (mLotes == null)
                return;

            if (/*!string.IsNullOrEmpty(PrdctCptr)*/ Prdct != null)
            {
                //Filter the collection 
                mLotes.Criteria = new BinaryOperator("Prdct.Clv", /*PrdctCptr*/Prdct.Clave);
            }
            else
            {
                //Remove the applied filter 
                //Lo que se busca es que no se muestre nada, si no hay lote capturado
                // mLotes.Criteria = null;
                mLotes.Criteria = new BinaryOperator("Oid", string.Empty);
            }
        }


        private Empresa mEmpresa;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        [NonPersistent]
        // Para imprimirlo en los ftos
        public Empresa Empresa
        {
            get
            {
                if (mEmpresa == null)
                    mEmpresa = Session.FindObject<Empresa>(null);
                return mEmpresa;
            }
            set { SetPropertyValue("Empresa", ref mEmpresa, value); }
        }

        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        [NonPersistent]
        [XafDisplayName("Fecha Group")]
        public int FchVl
        {
            get { return mFchVnt.Year + (mFchVnt.Month * 10) + mFchVnt.Day; }
        }


        private Persona mMdc;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [XafDisplayName("Médico")]
        public Persona Mdc
        {
            get { return mMdc; }
            set { SetPropertyValue("Mdc", ref mMdc, value); }
        }

        [RuleFromBoolProperty("Venta.Items", DefaultContexts.Save, "Debe capturar al menos una Partida")]
        protected bool VentaItemsOk
        {
            get { return ItemsVenta != null && ItemsVenta.Count > 0; }
        }


        public override void AfterConstruction()
        {
            base.AfterConstruction();

            Cntdd = 1;
            FchVnt = DateTime.Now;
            // Criterion = new BinaryOperator("Prdct", null);
            Criterion = null;
            Stts = EEstadoVenta.Alta;
        }


        public void UpdateVentaTotal(bool forceChangeEvents)
        {
            decimal? oldOrdersTotal = mTtl;
            decimal tempTotal = 0m;
            //decimal tempDscnt = 0m;
            decimal tempImpst04 = 0m;

            foreach (VentaItem it in ItemsVenta)
            {
                tempTotal += it.Prc * Convert.ToDecimal(it.Cntdd); // *(decimal)(1.0f - it.Dscnt01 / 100);
                // tempDscnt += it.Prc * (decimal)(it.Dscnt01 / 100);
                tempImpst04 += it.TotalImpuesto04;
            }
            mTtl = tempTotal + tempImpst04;
            //mDscntTtl = tempDscnt;


            if (forceChangeEvents)
                OnChanged("Ttl", oldOrdersTotal, mTtl);
        }


        public void AddItem()
        {
            if (Prdct != null)
            {
                Producto prd = Prdct;

                CnLt = prd.Lotes;
                VentaItem itm = new VentaItem(Session);
                if (prd.Lotes)
                {
                    if (Lt != null)
                    {
                        itm.Cntdd = Cntdd;
                        itm.Prdct = prd;
                        itm.Prc = itm.Prdct.PrecioPublico;
                        itm.Lt = Lt.Lt;

                        /*
                        ItemsVenta.Add(itm);
                        UpdateVentaTotal(true);*/

                        /*
                        mPrdctCptr = string.Empty;*/
                        Prdct = null;
                        Lt = null;
                    }
                }
                else
                {
                    itm.Cntdd = Cntdd;
                    itm.Prdct = prd;
                    itm.Prc = itm.Prdct.PrecioPublico;

                    /*
                    ItemsVenta.Add(itm);
                    UpdateVentaTotal(true);*/

                    Prdct = null;
                    Lt = null;
                }

                /*
                float auxCnt = itm.Cntdd;
                foreach (VentaItem vi in this.ItemsVenta)
                {
                    if (vi.Prdct.Clave == prd.Clave)
                        auxCnt += vi.Cntdd;
                }

                decimal auxPrc = itm.Prdct.PrecioPublico;
                foreach (ProductoPrecios prcds in prd.Precios)
                {
                    if (prcds.Cntdd != null && auxCnt >= prcds.Cntdd)
                        auxPrc = prcds.Prc;
                }
                foreach (VentaItem vi in this.ItemsVenta)
                {
                    if (vi.Prdct.Clave == prd.Clave)
                        vi.Prc = auxPrc;
                }
                itm.Prc = auxPrc;*/

                ItemsVenta.Add(itm);
                /*Sep 2020
                UpdateVentaTotal(true);*/
                Cntdd = 1;
            }
        }
    }

    public enum EEstadoVenta
    {
        Alta,
        Cancelada
    }
}
