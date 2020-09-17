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
    public enum MovimientoStatus
    {
        Firme = 1,
        [XafDisplayName("Tránsito")]
        Transito = 2,
        Cancelado = 3,
        Conciliado = 4
    }
}
