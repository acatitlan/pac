#region Copyright (c) 2014-2020 cjlc
/*
{+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++)
{                                                                   }
{     cjlc Cap control administrativo personal                      }
{                                                                   }
{     Copyrigth (c) 2014-2020 cjlc                                  }
{     Todos los derechos reservados                                 }
{                                                                   }
{*******************************************************************}
 */
#endregion

using DevExpress.ExpressApp.DC;

namespace Cap.Bancos.BusinessObjects
{
    //#region Pago tipo
    public enum EPagoTipo
    {
        Efectivo = 1,
        [XafDisplayName("Débito")]
        Debito = 2,
        [XafDisplayName("Crédito")]
        Credito = 3,
        Cheque = 4
    }
    //#endregion
}
