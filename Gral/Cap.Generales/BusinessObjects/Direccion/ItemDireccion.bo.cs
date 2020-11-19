using System;
using System.Collections.Generic;
using System.Text;
using Cap.Generales.BusinessObjects.Empresa;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;

namespace Cap.Generales.BusinessObjects.Direccion
{
    public partial class ItemDireccion
    {
        #region + Cliente
        private Compania mCompania;
        // [Association("Compania-Direccion")]
        public Compania Compania
        {
            get { return mCompania; }
            set { SetPropertyValue("Compania", ref mCompania, value); }
        }
        #endregion

        #region + Direccion
        private Direccion mDireccion;
        [ExpandObjectMembers(ExpandObjectMembers.Always)]
        public Direccion Direccion
        {
            get { return mDireccion; }
            set { SetPropertyValue("Direccion", ref mDireccion, value); }
        }
        #endregion

        public override void AfterConstruction()
        {
            base.AfterConstruction();

            Direccion = new Direccion(Session);
        }
    }
}
