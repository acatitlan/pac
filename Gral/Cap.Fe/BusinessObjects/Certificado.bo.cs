/*
 * tlacaelel.icpac@gmail.com
 *
 * User: Tlacaelel Icpac
 * Date: 20/ Abr /2016
 * Time: 04:13 p. m.
 */

using System;
using System.ComponentModel;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using Cap.Generales.BusinessObjects.General;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.Persistent.Validation;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Cap.Fe.BusinessObjects
{
    [ImageName("Certificate")]
    [NavigationItem(("Configuración")/*, GroupName = "Configuracion"*/)]
    public partial class Certificado : ISingleton // TIT Ago Ya no será, pero hay que hacer el trámite !
    {
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place here your initialization code.

            PasswCertif = string.Empty;
            AvisarCer = ETiempoAntes.Mes;
            AvisarFol = 10;
            SerieCertif = string.Empty;
        }

        /*TI Sólo tenemos un objeto no es necesaria la Clave
        // Creo que no es necesario que tenga Clave 27 enero 2009
        // Provocamos que se caiga pues si cambia el nombre ya no lo encuentra 
        private string FClave;
        [VisibleInDetailView(false)]
        [Indexed(Unique = true), Size(10)]
        public string Clave
        {
            get { return FClave.Trim(); }
            set { SetPropertyValue("Clave", ref FClave, value); }
        }*/

        private MyFileData mFileCertif;
        // TI Sep 2017 Ahora ya no es indispensable, sólo los archivos PEM
        // Pero como hay que crear los archivos PEM, para la gueb no funciona esto.
        // Por eso son necesarios el certificado y la llave
        //
        [ImmediatePostData]
        [RuleRequiredField("RuleRequiredField for Certificado.FileCertif", DefaultContexts.Save, "Debe asignar el Archivo Certificado", SkipNullOrEmptyValues = false)]
        [XafDisplayName("Archivo Certificado")]
        [FileTypeFilter("Certificados", 1, "*.cer")]
        public MyFileData FileCertif
        {
            get { return mFileCertif; }
            set
            {
                SetPropertyValue("FileCertif", ref mFileCertif, value);
                /* TIT, value cuando se asigna no trae el nombre del archivo 
                 * No funciona el ImmediatePostData
                if (!IsLoading)
                {
                    ValoresCertificado();
                }*/
            }
        }

        private MyFileData mFilePrivKy;
        [RuleRequiredField("RuleRequiredField for Certificado.FilePrivKy", DefaultContexts.Save, "Debe asignar el Archivo Llave", SkipNullOrEmptyValues = false)]
        [XafDisplayName("Archivo Llave")]
        [FileTypeFilter("Llaves", 1, "*.key")]
        public MyFileData FilePrivKy
        {
            get { return mFilePrivKy; }
            set { SetPropertyValue("FilePrivKy", ref mFilePrivKy, value); }
        }

        private string mPasswCertif;
        [XafDisplayName("Contraseña Certificado")]
        [PasswordPropertyText(true)]
        [Size(25)]
        public string PasswCertif
        {
            get { return mPasswCertif; }
            set { SetPropertyValue("PasswCertif", ref mPasswCertif, value); }
        }

        [DevExpress.Xpo.DisplayName("Contraseña Certificado")]
        //[Appearance("Contra", AppearanceItemType = "LayoutItem", Context = "DetailView", Method = "NoIsAdmin" /*Criteria = "!IsCurrentUserInRole('Administrator')"*/,
        //    Visibility = ViewItemVisibility.Hide)]
        // Administrator / Administrador
        [Appearance("Contra", AppearanceItemType = "LayoutItem", Context = "DetailView", Criteria = "!IsCurrentUserInRole('Administrador')", Visibility = ViewItemVisibility.Hide)]
        [Size(25)]
        [NonPersistent]
        public string Contra
        {
            get { return PasswCertif; }
            set { PasswCertif = value; }
        }


        private string mSerieCertif;
        [XafDisplayName("Serie Certificado")]
        // [Appearance("SerieCertif", Context = "DetailView", Enabled = false, FontStyle = System.Drawing.FontStyle.Italic)]
        [Size(25)]
        public string SerieCertif
        {
            get { return mSerieCertif; }
            set { SetPropertyValue("SerieCertif", ref mSerieCertif, value); }
        }

        private string mEmprCertif;
        [DevExpress.Xpo.DisplayName("Empresa Certificado")]
        [NonPersistent]
        public string EmprCertif
        {
            get { return mEmprCertif; }
            // set { SetPropertyValue("EmprCertif", ref mEmprCertif, value); }
            set { mEmprCertif = value; }
        }

        // Asi no jala !
        /*
        [NonPersistent]
        public string EmisorCertif { get; set; }*/
        private string mEmisorCertif;
        [XafDisplayName("Emisor Certificado")]
        [NonPersistent]
        public string EmisorCertif
        {
            get { return mEmisorCertif; }
            // set { SetPropertyValue("EmisorCertif", ref mEmisorCertif, value); }
            set { mEmisorCertif = value; }
        }

        private DateTime mFechaIni;
        [XafDisplayName("Fecha Inicial")]
        // [Appearance("FechaIni", Context = "DetailView", Enabled = false, FontStyle = System.Drawing.FontStyle.Italic)]
        [ModelDefault("DisplayFormat", "{0:dd MMM yyyy}")]
        public DateTime FechaIni
        {
            get { return mFechaIni; }
            set { SetPropertyValue("FechaIni", ref mFechaIni, value); }
        }

        private DateTime mFechaFin;
        // ? si es un dato que trae el certificado Mrz 2021
        //[ImmediatePostData]
        [XafDisplayName("Fecha Final")]
        // [Appearance("FechaFin", Context = "DetailView", Enabled = false, FontStyle = System.Drawing.FontStyle.Italic)]
        [ModelDefault("DisplayFormat", "{0:dd MMM yyyy}")]
        public DateTime FechaFin
        {
            get { return mFechaFin; }
            set { SetPropertyValue("FechaFin", ref mFechaFin, value); }
        }

        private string mCertificadoCad;
        [Obsolete("Parece que ya no es necesario !")]
        [DevExpress.Xpo.DisplayName("Certificado Cadena")]
        // Jul 2019 Para qué lo oculto? Por eso lo comenté
        //[Appearance("CertificadoCad", AppearanceItemType = "LayoutItem", Context = "DetailView", 
        //    Criteria = "!IsCurrentUserInRole('Administrador') || !IsCurrentUserInRole('Administrator')", Visibility = ViewItemVisibility.Hide)]
        
        //[Appearance("CertificadoCad", AppearanceItemType = "LayoutItem", Context = "DetailView", Method = "NoIsAdmin"/* Criteria = "!IsCurrentUserInRole('Administrator')"*/,
        //    Visibility = ViewItemVisibility.Hide)]
        [Size(SizeAttribute.Unlimited)]
        public string CertificadoCad
        {
            get { return mCertificadoCad; }
            set { SetPropertyValue("CertificadoCad", ref mCertificadoCad, value); }
        }

        private ETiempoAntes FAvisar;
        [DevExpress.Xpo.DisplayName("Avisar Caducidad Certificado")]
        public ETiempoAntes AvisarCer
        {
            get { return FAvisar; }
            set { SetPropertyValue("Avisar", ref FAvisar, value); }
        }

        //#region + Avisar Folio
        private float FAvisarFol;
        // Cómo hacer esto es decir como nos informa el PAC ?
        [VisibleInDetailView(false)]
        public float AvisarFol
        {
            get { return FAvisarFol; }
            set { SetPropertyValue("AvisarFol", ref FAvisarFol, value); }
        }


        private MyFileData mFlCrtfcdPm;
        [Obsolete("Parece que ya no se usa !")]
        // [RuleRequiredField("RuleRequiredField for Certificado.FlCrtfcdPm", DefaultContexts.Save, "Debe asignar el Archivo Certificado PEM", SkipNullOrEmptyValues = false)]
        [XafDisplayName("Archivo Certificado PEM")]
        //[VisibleInDetailView(false)]
        public MyFileData FlCrtfcdPm
        {
            get { return mFlCrtfcdPm; }
            set { SetPropertyValue("FlCrtfcdPm", ref mFlCrtfcdPm, value); }
        }

        private MyFileData mFlKyPm;
        [Obsolete("Parece que ya no se usa !")]
        // [RuleRequiredField("RuleRequiredField for Certificado.FlKyPm", DefaultContexts.Save, "Debe asignar el Archivo Llave PEM", SkipNullOrEmptyValues = false)]
        [XafDisplayName("Archivo Llave PEM")]
        //[VisibleInDetailView(false)]
        public MyFileData FlKyPm
        {
            get { return mFlKyPm; }
            set { SetPropertyValue("FlKyPm", ref mFlKyPm, value); }
        }



        [RuleFromBoolProperty("Certificado.FlKyPm", DefaultContexts.Save, "No se creó bien el Archivo PEM, la contraseña es incorrecta")]
        protected bool FlKyPmOk
        {
            get { return true;  /*return FlKyPm != null && FlKyPm.Size > 0;*/ }

        }

        [RuleFromBoolProperty("Certificado.PasswCertif", DefaultContexts.Save, "Debe capturar la contraseña de la Llave")]
        protected bool PasswCertifOk
        {
            get { return !string.IsNullOrEmpty(PasswCertif.Trim()); }
        }

        /*
        void ValoresCertificado()
        {
            X509Certificate2 x509 = new X509Certificate2();
            x509.Import(FileCertif.Content);

            byte[] nSerie = x509.GetSerialNumber();
            SerieCertif = Encoding.ASCII.GetString(nSerie);

            EmprCertif = x509.SubjectName.Name;
            EmisorCertif = x509.IssuerName.Name;

            FechaIni = Convert.ToDateTime(x509.GetEffectiveDateString());
            FechaFin = Convert.ToDateTime(x509.GetExpirationDateString());
        }*/
    }


    public enum ETiempoAntes
    {
        [XafDisplayName("Dia antes")]
        Dia = 1,
        [XafDisplayName("Semana antes")]
        Semana = 2,
        [XafDisplayName("Quincena antes")]
        Quincena = 3,
        [XafDisplayName("Mes antes")]
        Mes = 4,
        Indefinido = 0
    }
}
