#region Copyright (c) 2000-2012 cjlc
/*
{+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++)
{                                                                   }
{     cjlc Cap control administrativo personal                      }
{                                                                   }
{     Copyrigth (c) 2000-2012 cjlc                                  }
{     Todos los derechos reservados                                 }
{                                                                   }
{*******************************************************************}
 */
#endregion

using System;

namespace apl.Importar
{
    #region + Importa Observaciones Proveedores
    public class ImportaObsPrv : ImportaObs
    {
        #region + Constructor
        public ImportaObsPrv(string path, string numEmp)
            : base(path, numEmp)
        {
            NombreFile = "OPRV.D";
        }
        #endregion
    }
    #endregion
}
