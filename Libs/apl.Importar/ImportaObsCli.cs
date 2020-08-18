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

    #region + Importa Observaciones Clientes
    public class ImportaObsCli : ImportaObs
    {
        #region + Constructor
        public ImportaObsCli(string path, string numEmp)
            : base(path, numEmp)
        {
            NombreFile = "OCLI.D";
        }
        #endregion
    }
    #endregion
}
