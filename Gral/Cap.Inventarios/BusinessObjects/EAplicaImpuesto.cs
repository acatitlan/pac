using System;
using System.Collections.Generic;
using DevExpress.ExpressApp.DC;

namespace Cap.Inventarios.BusinessObjects
{
    //#region + Aplicar impuestos enums
    public enum EAplicaImpuesto
    {
        Precio = 1,
        [XafDisplayName("Acumulado 1")]
        Acumulado1 = 2,
        [XafDisplayName("Acumulado 2")]
        Acumulado2 = 3,
        [XafDisplayName("Acumulado 3")]
        Acumulado3 = 4,
        [XafDisplayName("No definido")]
        Ninguno = 5
    }
}
