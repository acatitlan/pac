/*
 * javier1604@gmail.com
 * Carlos Javier Lopez Cruz
 *
 * User: Tlacaelel Icpac
 * Date: 21/ Mar /2016
 * Time: 09:59 a. m.
 * 
 */

using System.Drawing;
using Cap.Generales.BusinessObjects.General;
using Cap.Generales.BusinessObjects.Object;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Security.Strategy;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.DC;
using System;

namespace Cap.Generales.BusinessObjects.Empresa
{
    [XafDefaultProperty("Compania.Nombre")]
    [NavigationItem("Configuración")]
    // [NavigationItem("General")]
    [ImageName("BO_Organization")]
    public partial class Empresa : ISingleton
    {
        // Creo que no es necesario que tenga Clave 27 enero 2009
        // Provocamos que se caiga pues si cambia el nombre ya no lo encuentra 
        private string FClave;
        [Obsolete("En su lugar no hay nada que usar pues es Singleton")]
        [VisibleInDetailView(false)]
        [Indexed(Unique = true), Size(10)]
        public string Clave
        {
            get { return FClave/*.Trim()*/; }
            set { SetPropertyValue("Clave", ref FClave, value); }
        }

        private Compania FCompania;
        [VisibleInDetailView(false)]
        [ExpandObjectMembers(ExpandObjectMembers.Always)]
        public Compania Compania
        {
            get { return FCompania; }
            set { SetPropertyValue("Compania", ref FCompania, value); }
        }

        //#region + Usuario, pac
        private string mUsuario;
        /*TIT Oct 2017 Bueno, puede pagarme por el programa y usarlo con otro distribuidor sin pagarme timbres ! 
        [Appearance("Usuario", AppearanceItemType = "LayoutItem", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Method = "Sin_Cfdi")]*/
        [Size(15)]
        public string Usuario
        {
            get { return mUsuario; }
            set { SetPropertyValue("Usuario", ref mUsuario, value); }
        }
        //#endregion

        private string mPassw;
        [DevExpress.Xpo.DisplayName("Contraseña")]
        // [PasswordPropertyText(true)]
        /*TIT Oct 2017 Bueno, puede pagarme por el programa y usarlo con otro distribuidor sin pagarme timbres ! 
        [Appearance("Passw", AppearanceItemType = "LayoutItem", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Method = "Sin_Cfdi")]*/
        [Size(15)]
        public string Passw
        {
            get { return mPassw; }
            set { SetPropertyValue("Passw", ref mPassw, value); }
        }

        private string mContra;
        [DevExpress.Xpo.DisplayName("Licencia")]
        /*TIT Oct 2017 Bueno, puede pagarme por el programa y usarlo con otro distribuidor sin pagarme timbres ! 
        [Appearance("Contra", AppearanceItemType = "LayoutItem", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Method = "Sin_Cfdi")]*/
        [Size(50)]
        public string Contra
        {
            get { return mContra; }
            set { SetPropertyValue("Contra", ref mContra, value); }
        }
        /*
        [Appearance("Contra", AppearanceItemType = "LayoutItem", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Method = "Sin_Cfdi")]
        [Size(15)]
        [NonPersistent]
        public string Contra
        {
            get { return Passw; }
            set { Passw = value; }
        }*/
        //#endregion

        #region + Status
        /*
        private StatusTipo mStatus;
        public StatusTipo Status
        {
            get { return mStatus; }
            set { SetPropertyValue("Status", ref mStatus, value); }
        }*/
        #endregion

        private string mRegimen;
        /// <summary>
        /// Regimen del SAT 
        /// </summary>
        [Obsolete("En su lugar usar Rgmn para Cfdi 3.3")]
        [ModelDefault("PropertyEditorType", "FCap.Module.Win.Editors.CustomStringEditor")]
        [ModelDefault("PropertyEditorType", "LCap.Module.Win.Editors.CustomStringEditor")]
        [DisplayName("Régimen")]
        [Size(100)]
        public string Regimen
        {
            get { return mRegimen; }
            set { SetPropertyValue("Regimen", ref mRegimen, value); }
        }

        private Regimen mRgmn;
        [Obsolete("En su lugar usar Regimenes")]
        [XafDisplayName("Nuevo Régimen")]
        public Regimen Rgmn
        {
            get { return mRgmn; }
            set { SetPropertyValue("Rgmn", ref mRgmn, value); }
        }

        [ValueConverter(typeof(JpegStorageConverter)), Delayed]
        public Image Logo
        {
            get { return GetDelayedPropertyValue<Image>("Logo"); }
            set { SetDelayedPropertyValue<Image>("Logo", value); }
        }

        #region + CBB
        /*
        [Obsolete("En la impresión de la factura se calcula al imprimir ")]
        / *
        private readonly XPDelayedProperty bytesImagen = new XPDelayedProperty();* /
        [Appearance("CBB", AppearanceItemType = "LayoutItem", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Method = "Con_Cfdi"/ *Criteria = "IsCurrentUserInRole('CFDI')", * /)]
        / *
        [Delayed("bytesImagen")]* /
        [ValueConverter(typeof(JpegStorageConverter)), Delayed]
        public Image CBB
        {
            get { return GetDelayedPropertyValue<Image>("CBB"); }
            set { SetDelayedPropertyValue<Image>("CBB", value); }
            / *
            get { return (Image)bytesImagen.Value; }
            set
            {
                bytesImagen.Value = value;
                if (!IsLoading)
                    OnChanged("CBB");
            }* /
        }*/
        #endregion

        private bool isAdmin()
        {
            bool isAdm = false;
            // SecuritySystemUser currentUser = SecuritySystem.CurrentUser as SecuritySystemUser;
            // Jul 2017, es mejor en este caso usar la interfaz.
            DevExpress.ExpressApp.Security.ISecurityUser currentUserr = SecuritySystem.CurrentUser as DevExpress.ExpressApp.Security.ISecurityUser;
            // isAdm = currentUser != null && (currentUser.UserName == "root" || currentUser.UserName == "instala");
            isAdm = currentUserr != null && (currentUserr.UserName == "root" || currentUserr.UserName == "instala");
            return isAdm;
        }

        private bool Con_Cfdi()
        {
            return !isAdmin() && ConCfdi;
        }

        private bool Sin_Cfdi()
        {
            return !isAdmin();
        }

        private bool NoEsAdmin()
        {
            return !isAdmin();
        }

        [Obsolete("Parece que ya no se usa")]
        [VisibleInDetailView(false)]
        [DisplayName("Cédula")]
        [ValueConverter(typeof(JpegStorageConverter)), Delayed]
        public Image Cedula
        {
            get { return GetDelayedPropertyValue<Image>("Cedula"); }
            set { SetDelayedPropertyValue<Image>("Cedula", value); }
        }

        private bool mConCfdi;
        /*TIT Oct 2017 Bueno, puede pagarme por el programa y usarlo con otro distribuidor sin pagarme timbres ! 
        [Appearance("ConCfdi", AppearanceItemType = "LayoutItem", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Method = "NoEsAdmin" / *Criteria = "!IsCurrentUserInRole('Administrator')"* /)]*/
        public bool ConCfdi
        {
            get { return mConCfdi; }
            set { SetPropertyValue("ConCfdi", ref mConCfdi, value); }
        }

        /*
        private bool mConRet;
        [Obsolete("En su lugar usar el EsquemaImpuesto")]
        [XafDisplayName("Con Retenciones")]
        [Appearance("ConRet", AppearanceItemType = "LayoutItem", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Method = "NoEsAdmin"/ * Criteria = "!IsCurrentUserInRole('Administrator')"* /)]
        public bool ConRet
        {
            get { return mConRet; }
            set { SetPropertyValue("ConRet", ref mConRet, value); }
        }*/

        private string mSlogan;
        [DisplayName("Eslogan")]
        [Size(50)]
        public string Slogan
        {
            get { return mSlogan; }
            set { SetPropertyValue("Slogan", ref mSlogan, value); }
        }

        private bool mTrans;
        [Obsolete("Por el momento no lo Soportamos Nov 18")]
        [DisplayName("Transportista")]
        [Appearance("TransVisible", AppearanceItemType = "LayoutItem", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Method = "NoEsAdmin")]
        [Appearance("Trans", Context = "DetailView", Enabled = false, FontStyle = System.Drawing.FontStyle.Italic/*TIT Ago 2018 se eliminó , Criteria = "ConRet != true"*/)]
        // [Appearance("Trans", AppearanceItemType = "LayoutItem", Context = "DetailView", Criteria = "Tipo != 2", Visibility = ViewItemVisibility.Hide)]
        // [Appearance("ConRet", AppearanceItemType = "LayoutItem", Context = "DetailView", /*Criteria = "!IsCurrentUserInRole('Administrator')",*/ Visibility = ViewItemVisibility.Hide, Method = "Sin_Cfdi")]
        public bool Trans
        {
            get { return mTrans; }
            set { SetPropertyValue("Trans", ref mTrans, value); }
        }

        private string mNumRegPatSS;
        [Size(20)]
        public string NumRegPatSS
        {
            get { return mNumRegPatSS; }
            set { SetPropertyValue("NumRegPatSS", ref mNumRegPatSS, value); }
        }

        private string mPermiso;
        [Obsolete("Parece que no se usa. No recuerdo para qué es")]
        [Size(20)]
        public string Permiso
        {
            get { return mPermiso; }
            set { SetPropertyValue("Permiso", ref mPermiso, value); }
        }

        // [Association("Compania-Direccion", typeof(ItemDireccion)), DevExpress.Xpo.Aggregated]
        [Association("Regimenes", typeof(RegimenEmpresa))]
        public XPCollection Regimenes
        {
            get { return GetCollection("Regimenes"); }
        }

        [VisibleInListView(false)]
        // (((http|https|ftp)\://)?[a-zA-Z0-9\-\.]+\.[a-zA-Z]{2,3}(:[a-zA-Z0-9]*)?/?([a-zA-Z0-9\-\._\?\,\'/\\\+&amp;amp;%\$#\=~])*)|([a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,6})
        [EditorAlias("HyperLinkPropertyEditor")]
        public string PortalCfdi
        {
            get
            {
                return "https://portalcfdi.facturaelectronica.sat.gob.mx/";
            }
        }

        [VisibleInListView(false)]
        [EditorAlias("HyperLinkPropertyEditor")]
        public string Terceros
        {
            get
            {
                return "https://tramitesdigitales.sat.gob.mx/InformativaDeTerceros.Internet";
            }
        }

        // Lo puse en Persona, pero Empresa no tiene acceso a Persona |:(
        private string mCdlPrfsnl;
        [Size(10)]
        [XafDisplayName("Cédula Profesional")]
        public string CdlPrfsnl
        {
            get { return mCdlPrfsnl; }
            set { SetPropertyValue("CdlPrfsnl", ref mCdlPrfsnl, value); }
        }

        [VisibleInListView(false)]
        [EditorAlias("HyperLinkPropertyEditor")]
        public string ServiciosUnidades
        {
            get
            {
                return "http://pys.sat.gob.mx/PyS/catPyS.aspx";
            }
        }


        // https://www.acceso.sat.gob.mx/_mem_bin/FormsLogin.asp?/Acceso/CertiSAT.asp
        // 
        // https://www.facturacfdi.mx/WSTimbrado/FirmaContrato

        public override void AfterConstruction()
        {
            base.AfterConstruction();

            Compania = new Compania(Session);
            /*TIT Ago 2018, ahora se usa el esquema
            ConRet = false;*/
            ConCfdi = false;
            Trans = false;
            Slogan = string.Empty;
            Contra = "mdBc591/Q7UJwKzkmTEQ8w==";
            CdlPrfsnl = string.Empty;
        }

        /*POEmpresa
        // Not null
        private short mNumero;
        [Indexed(Unique = true)]
        public short Numero
        {
            get { return mNumero; }
            set { SetPropertyValue("Numero", ref mNumero, value); }
        }

        #region + Sucursales
        [Association("Empresa-Sucursales", typeof(POSucursal)), Aggregated]
        public XPCollection Sucursales
        {
            get { return GetCollection("Sucursales"); }
        }
        #endregion
         */
    }
}
