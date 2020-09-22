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
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;

namespace Cap.Bancos.BusinessObjects
{
    [NavigationItem("Bancos")]
    [ImageName("Rain_Check")]
    public partial class Transferencia : ISingleton
    {
        [RuleFromBoolProperty("Transferencia.CuentasOk", DefaultContexts.Save, 
            "La Cuenta Origen debe ser diferente a la Cuenta Destino")]
        protected bool CuentasOk
        {
            get { return CtaOrigen != CtaDestino; }
        }

        private Bancaria mCtaOrigen;
        [ImmediatePostData]
        [DisplayName("Cuenta Origen")]
        [DataSourceCriteria("Status != 'Baja'")]
        [RuleRequiredField("RuleRequiredField for Transferencia.CtaOrigen", DefaultContexts.Save, 
            "Debe asignar una Cuenta Origen", SkipNullOrEmptyValues = false)]
        public Bancaria CtaOrigen
        {
            get { return mCtaOrigen; }
            set { SetPropertyValue("CtaOrigen", ref mCtaOrigen, value); }
        }

        [XafDisplayName("Descripción Origen")]
        public string CntOrgnDscrpcn
        {
            get { return mCtaOrigen != null ? mCtaOrigen.Descripcion : string.Empty; }
        }

        [XafDisplayName("Saldo Disponible")]
        public decimal CntOrgnSldDspnbl
        {
            get { return mCtaOrigen != null ? mCtaOrigen.SldDspnbl : 0; }
        }

        private Bancaria mCtaDestino;
        [ImmediatePostData]
        [DisplayName("Cuenta Destino")]
        [DataSourceCriteria("Status != 'Baja'")]
        [RuleRequiredField("RuleRequiredField for Transferencia.CtaDestino", DefaultContexts.Save, 
            "Debe asignar una Cuenta Destino", SkipNullOrEmptyValues = false)]
        public Bancaria CtaDestino
        {
            get { return mCtaDestino; }
            set { SetPropertyValue("CtaDestino", ref mCtaDestino, value); }
        }

        [XafDisplayName("Descripción Destino")]
        public string CntDstnDscrpcn
        {
            get { return mCtaDestino != null ? mCtaDestino.Descripcion : string.Empty; }
        }

        [XafDisplayName("Saldo Final")]
        public decimal CntOrgnSldFnl
        {
            get { return mCtaDestino != null ? mCtaDestino.SaldoFinal : 0; }
        }

        [RuleFromBoolProperty("Transferencia.MontoOk", DefaultContexts.Save, 
            "El Monto debe ser mayor que 0")]
        protected bool MontoOk
        {
            get { return Monto > 0; }
        }

        private decimal mMonto;
        public decimal Monto
        {
            get { return mMonto; }
            set { SetPropertyValue("Monto", ref mMonto, value); }
        }

        private DateTime mFecApli;
        [DisplayName("Fecha de Aplicación")]
        [ModelDefault("DisplayFormat", "{0:dd MMM yyyy}")]
        public DateTime FecApli
        {
            get { return mFecApli; }
            set { SetPropertyValue("FecApli", ref mFecApli, value); }
        }

        private string mNotas;
        [Size(SizeAttribute.Unlimited)]
        public string Notas
        {
            get { return mNotas; }
            set { SetPropertyValue("Notas", ref mNotas, value); }
        }

        private string FClave;
        [VisibleInDetailView(false)]
        [Indexed(Unique = true), Size(10)]
        public string Clave
        {
            get { return string.IsNullOrEmpty(FClave) ? string.Empty : FClave.Trim(); }
            set { SetPropertyValue("Clave", ref FClave, ValorString("Clave", value)); }
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();

            Clave = "icpac";
            FecApli = DateTime.Today;
        }
    }
}
