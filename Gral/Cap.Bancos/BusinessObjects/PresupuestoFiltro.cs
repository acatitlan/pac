#region Copyright (c) 2019-2020 cjlc
/*
{+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++)
{                                                                   }
{     cjlc Cap control administrativo personal                      }
{                                                                   }
{     Copyrigth (c) 2019-2020 cjlc                                  }
{     Todos los derechos reservados                                 }
{                                                                   }
{*******************************************************************}
 */
#endregion

using System;
using DevExpress.Xpo;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Model;
using Cap.Generales.BusinessObjects.Object;
using Cap.Generales.BusinessObjects.General;

namespace Cap.Bancos.BusinessObjects
{
    //[DefaultClassOptions]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class PresupuestoFiltro : PObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public PresupuestoFiltro(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).

            /*
            FechasInicial(ref mFechaIni, ref mFechaFin, ref mPeriodo);*/
            FchIncl = apl.Log.Fecha.FechaInicial(DateTime.Today.Month);
            FchFnl = apl.Log.Fecha.FechaFinal(DateTime.Today.Month);
            Periodo = EFechas.EsteAño;
            /*
            switch (DateTime.Today.Month)
            {
                case 1:
                    FchIncl = apl.Log.Fecha.FechaInicial(1, DateTime.Today.Year - 1);
                    FchFnl = apl.Log.Fecha.FechaFinal(1, DateTime.Today.Year - 1);
                    Periodo = EFechas.Diciembre;
                    break;
                case 2:
                    Periodo = EFechas.Enero;
                    break;
                case 3:
                    Periodo = EFechas.Febrero;
                    break;
                case 4:
                    Periodo = EFechas.Marzo;
                    break;
                case 5:
                    Periodo = EFechas.Abril;
                    break;
                case 6:
                    Periodo = EFechas.Mayo;
                    break;
                case 7:
                    Periodo = EFechas.Junio;
                    break;
                case 8:
                    Periodo = EFechas.Julio;
                    break;
                case 9:
                    Periodo = EFechas.Agosto;
                    break;
                case 10:
                    Periodo = EFechas.Septiembre;
                    break;
                case 11:
                    Periodo = EFechas.Octubre;
                    break;
                case 12:
                    Periodo = EFechas.Noviembre;
                    break;
            }*/
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

        /* Será necesario ?
        [XafDisplayName("Cuenta")]
        public Bancaria Cnt { get; set; }

        [XafDisplayName("Concepto")]
        public ConceptoB Cncpt { get; set; }*/

        private DateTime mFechaIni;
        [ModelDefault("DisplayFormat", "{0:dd MMM yy}")]
        [XafDisplayName("Fecha Inicial")]
        public DateTime FchIncl
        {
            get { return apl.Log.Fecha.FechaInicial(mFechaIni); }
            set { mFechaIni = value; }
        }

        private DateTime mFechaFin;
        [ModelDefault("DisplayFormat", "{0:dd MMM yy}")]
        [XafDisplayName("Fecha Final")]
        public DateTime FchFnl
        {
            get { return apl.Log.Fecha.FechaFinal(mFechaFin); }
            set { mFechaFin = value; }
        }

        private EFechas mPeriodo;
        [ImmediatePostData]
        public EFechas Periodo
        {
            get { return mPeriodo; }
            set
            {
                mPeriodo = value;
                Fechas.CalculaFechas(value.GetHashCode() + 1, ref mFechaIni, ref mFechaFin);
            }
        }

        /*
        private void FechasInicial(ref DateTime fi, ref DateTime ff, ref EFechas prd )
        {
            fi = apl.Log.Fecha.FechaInicial(DateTime.Today.Month);
            ff = apl.Log.Fecha.FechaFinal(DateTime.Today.Month);
            switch (DateTime.Today.Month)
            {
                case 1:
                    fi = apl.Log.Fecha.FechaInicial(1, DateTime.Today.Year - 1);
                    ff = apl.Log.Fecha.FechaFinal(1, DateTime.Today.Year - 1);
                    prd = EFechas.Diciembre;
                    break;
                case 2:
                    prd = EFechas.Enero;
                    break;
                case 3:
                    prd = EFechas.Febrero;
                    break;
                case 4:
                    prd = EFechas.Marzo;
                    break;
                case 5:
                    prd = EFechas.Abril;
                    break;
                case 6:
                    prd = EFechas.Mayo;
                    break;
                case 7:
                    prd = EFechas.Junio;
                    break;
                case 8:
                    prd = EFechas.Julio;
                    break;
                case 9:
                    prd = EFechas.Agosto;
                    break;
                case 10:
                    prd = EFechas.Septiembre;
                    break;
                case 11:
                    prd = EFechas.Octubre;
                    break;
                case 12:
                    prd = EFechas.Noviembre;
                    break;
            }
            if ()
        }*/
    }
}