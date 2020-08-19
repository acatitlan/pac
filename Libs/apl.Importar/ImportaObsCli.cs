#region 
/*
{+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++)
{                                                                   }
{     tlacaelel.icpac                                               }
{     Cap control administrativo personal                           }
{                                                                   }
{     2000-2012                                                     }
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
