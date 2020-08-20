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

    #region + Importa Observaciones Cxc
    public class ImportaObsCxc : ImportaObs
    {
        public ImportaObsCxc(string path, string numEmp)
            : base(path, numEmp)
        {
            NombreFile = "OCXC.D";
        }
    }
    #endregion
}
