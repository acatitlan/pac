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
    #region + Importa Observaciones Productos
    public class ImportaObsInv : ImportaObs
    {
        #region + Constructor
        public ImportaObsInv(string path, string numEmp)
            : base(path, numEmp)
        {
            NombreFile = "OINV.D";
        }
        #endregion
    }
    #endregion
}
