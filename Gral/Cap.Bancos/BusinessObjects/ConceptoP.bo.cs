#region Copyright (c) 2015-2020 cjlc
/*
{+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++)
{                                                                   }
{     cjlc Cap control administrativo personal                      }
{                                                                   }
{     Copyrigth (c) 2015-2020 cjlc                                  }
{     Todos los derechos reservados                                 }
{                                                                   }
{*******************************************************************}
 */
#endregion

using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;

namespace Cap.Bancos.BusinessObjects
{
    [Appearance("ConceptoP.Nuevo", TargetItems = "*", Context = "ListView", Criteria = "[Monto] == 0", FontColor = "Green")]
    [Appearance("ConceptoP.Monto", TargetItems = "Cargo", Context = "ListView", Criteria = "[Concepto.Tipo] == 'Cargo' && [Monto] != 0", FontColor = "Red")]
    // Ya la tiene la propiedad 
    // [Appearance("Cargo", TargetItems = "Cargo", Context = "ListView", Criteria = "[Concepto.Tipo] == 'Cargo'", FontColor = "Red")]
    public partial class ConceptoP
    {
        //#region Properties
        private Presupuesto FPresupuesto;
        [Association("Presupuesto-Partidas")]
        public Presupuesto Presupuesto
        {
            get { return FPresupuesto; }
            set { SetPropertyValue("Presupuesto", ref FPresupuesto, value); }
        }

        private ConceptoB FConcepto;
        [RuleRequiredField("RuleRequiredField for ConceptoP.Concepto", DefaultContexts.Save, "Debe capturar el Concepto", SkipNullOrEmptyValues = false)]
        public ConceptoB Concepto
        {
            get { return FConcepto; }
            set { SetPropertyValue("Concepto", ref FConcepto, value); }
        }

        [RuleFromBoolProperty("MontoOk", DefaultContexts.Save, "El Monto no puede ser negativo")]
        protected bool MontoOk
        {
            get { return Monto >= 0; }
        }

        private decimal FMonto;
        // No sirve nican, por eso lo muevo a la clase
        //[Appearance("ConceptoP.Monto", Context = "ListView", Criteria = "[Concepto.Tipo] == 'Cargo'", FontColor = "Red")]
        public decimal Monto
        {
            get { return FMonto; }
            set { SetPropertyValue("Monto", ref FMonto, value); }
        }

        private string FNotas;
        [VisibleInListView(true)]
        [Size(SizeAttribute.Unlimited)]
        public string Notas
        {
            get { return FNotas; }
            set { SetPropertyValue("Notas", ref FNotas, value); }
        }

        [VisibleInDetailView(false)]
        [NonPersistent]
        public decimal Abono
        {
            get { return Concepto == null || Concepto.Tipo == EConceptoTipo.Cargo ? 0 : Monto; }
        }

        [VisibleInDetailView(false)]
        [NonPersistent]
        public decimal Cargo
        {
            get { return Concepto == null || Concepto.Tipo == EConceptoTipo.Abono ? 0 : Monto; }
        }

        private decimal mMntRl;
        [VisibleInDetailView(false)]
        [XafDisplayName("Monto Real Anual")]
        public decimal MntRl
        {
            get { return mMntRl; }
            set { SetPropertyValue("MntRl", ref mMntRl, value); }
        }

        [VisibleInDetailView(false)]
        [XafDisplayName("Monto Real Mensual")]
        public decimal MntRlMns
        {
            get { return MntRl / 12; }
        }

        [VisibleInDetailView(false)]
        [ModelDefault("DisplayFormat", "{0:n2}%")]
        [XafDisplayName("Porcentaje")]
        public float Prcntj
        {
            get { return Monto != 0 ? Convert.ToSingle(MntRlMns / Monto) * 100 : 0; }
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();

            Concepto = null;
            Monto = 0;
            MntRl = 0;
            Presupuesto = null;
            Notas = string.Empty;
        }
    }
}
