using System;
using System.Collections.Generic;
using DevExpress.ExpressApp.DC;

namespace Cap.Inventarios.BusinessObjects
{
    //#region + Costeo tipo enums
    public enum EProductoCosteo
    {
        [XafDisplayName("U.E.P.S.")]
        Ueps = 1,
        [XafDisplayName("P.E.P.S.")]
        Peps = 2,
        Promedio = 3,
        // [XafDisplayName("Est�ndar")]
        // Estandar = 4,
        [XafDisplayName("�ltimo")]
        Ultimo = 5,
        Indefinido = 0
    }
    //#endregion
}
