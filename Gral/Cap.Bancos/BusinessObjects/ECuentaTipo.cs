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

using DevExpress.ExpressApp.DC;

namespace Cap.Bancos.BusinessObjects
{
    public enum ECuentaTipo
    {
        [XafDisplayName("Crédito")]
        Credito = 0,
        [XafDisplayName("Débito")]
        Debito = 1,
        Maestra = 2,
        [XafDisplayName("Inversión")]
        Inversion = 3,
        Caja = 4,
        Afore = 5,
        Ninguna = 6,
        [XafDisplayName("Inválida")]
        Invalida = -1,
        Todos = 7
    }
}
