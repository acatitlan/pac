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
using System.Windows.Forms;

namespace apl.Log
{
    /// <summary>
    /// 
    /// </summary>
    public class CWaitCursor : CCursor
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public CWaitCursor() : base(Cursors.WaitCursor) { }
    }
}
