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

using apl.Log;
using System;

namespace Cap.Bancos.BusinessObjects
{
    public partial class Agenda
    {
        //#region Properties
        //#region + Cuenta sugerida
        private Bancaria FCuenta;
        public Bancaria Cuenta
        {
            get { return FCuenta; }
            set { SetPropertyValue("Cuenta", ref FCuenta, value); }
        }

        //#region + Concepto sugerido
        private ConceptoB FConcepto;
        public ConceptoB Concepto
        {
            get { return FConcepto; }
            set { SetPropertyValue("Concepto", ref FConcepto, value); }
        }

        //#region + Fecha inicial
        private DateTime FFecha;
        public DateTime Fecha
        {
            get { return FFecha; }
            set { SetPropertyValue("Fecha", ref FFecha, value); }
        }

        //#region + Monto original
        private decimal FMontoOriginal;
        public decimal Monto
        {
            get { return FMontoOriginal; }
            set { SetPropertyValue("Monto", ref FMontoOriginal, value); }
        }

        //#region + Numero de incidencias
        private short FIncidencias;
        public short Incidencias
        {
            get { return FIncidencias; }
            set { SetPropertyValue("Incidencias", ref FIncidencias, value); }
        }

        //#region + Incidencias aplicadas
        private short FIncidenciasAplicadas;
        public short IncidenciasAplicadas
        {
            get { return FIncidenciasAplicadas; }
            set { SetPropertyValue("IncidenciasAplicadas", ref FIncidenciasAplicadas, value); }
        }

        //#region + Periodicidad
        private EPeriodicidad FPeriodicidad;
        public EPeriodicidad Periodo
        {
            get { return FPeriodicidad; }
            set { SetPropertyValue("Periodo", ref FPeriodicidad, value); }
        }

        //#region no se usa
        // private float FPorcentajeRendimiento;
        // private decimal FMontoParcialidad;
        // private decimal FTotal;

        //#region + Clave
        /* Parece que no es necesaria la clave
        private string FClave;
        [Size(20), Indexed(Unique = true)]
        public string Clave
        {
            get { return FClave; }
            set 
            {
                if (value != null)
                {
                    FClave = value.Trim();
                    if (FClave.Length > 20)
                        FClave = FClave.Substring(0, 20);
                }
            }
        }*/
        //#endregion

        //#region + Descripcion
        /* Parece que tampoco sería necesaria la descripción
        private string FDescripcion;
        [Size(40)]
        public string Descripcion
        {
            get { return FDescripcion; }
            set 
            {
                if (value != null)
                {
                    FDescripcion = value.Trim();
                    if (FDescripcion.Length > 40)
                        FDescripcion = FDescripcion.Substring(1, 40);
                }
            }
        }*/
        //#endregion

        //#region + Tipo
        /* Pa que era?
        private short FTipo;
        public short Tipo
        {
            get { return FTipo; }
            set { FTipo = value; }
        }*/
        //#endregion

        //#region + Saldo
        /*Tal vez ya no es necesario
        private decimal FSaldo;
        public decimal Saldo
        {
            get { return FSaldo; }
            set { FSaldo = value; }
        }*/
        //#endregion

        //#region + Monto parcial
        /* Tal vez ya no es necesario
        private decimal FMontoParcial;
        public decimal MAplicar
        {
            get { return FMontoParcial; }
            set { FMontoParcial = value; }
        }*/
        //#endregion

        //#region + Dia del mes en que se aplica
        /*
        private short FDiaAplicar;
        public short DiaAplicado
        {
            get { return FDiaAplicar; }
            set { SetPropertyValue("DiaAplicado", ref FDiaAplicar, value); }
        }*/
        //#endregion
        //#endregion
        //#endregion

        public override void AfterConstruction()
        {
            base.AfterConstruction();

            Concepto = null;
            Cuenta = null;
            Fecha = DateTime.Today;
            Incidencias = 0;
            IncidenciasAplicadas = 0;
            Monto = 0.0m;
            Periodo = EPeriodicidad.Diaria;
        }
    }
}
