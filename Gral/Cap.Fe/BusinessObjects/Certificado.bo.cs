/*
 * javier1604@gmail.com
 * Carlos Javier Lopez Cruz
 *
 * User: Tlacaelel Icpac
 * Date: 20/ Abr /2016
 * Time: 04:13 p. m.
 * 
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
        [ImmediatePostData]
        // TI Sep 2017 Ahora ya no es indispensable, sólo los archivos PEM
        //[RuleRequiredField("RuleRequiredField for Certificado.FileCertif", DefaultContexts.Save, "Debe asignar el Archivo Certificado", SkipNullOrEmptyValues = false)]
        [DevExpress.Xpo.DisplayName("Archivo Certificado")]
        [FileTypeFilter("Certificados", 1, "*.cer")]
        public MyFileData FileCertif
        {
            get { return mFileCertif; }
            set
            {
                SetPropertyValue("FileCertif", ref mFileCertif, value);
                /* TIT, value cuando se asigna no trae el nombre del archivo
                if (!IsLoading)
                {
                    ValoresCertificado();
                }*/
            }
        }

        private MyFileData mFilePrivKy;
        [DevExpress.Xpo.DisplayName("Archivo Llave")]
        [FileTypeFilter("Llaves", 1, "*.key")]
        public MyFileData FilePrivKy
        {
            get { return mFilePrivKy; }
            set { SetPropertyValue("FilePrivKy", ref mFilePrivKy, value); }
        }

        private string mPasswCertif;
        [DevExpress.Xpo.DisplayName("Contraseña Certificado")]
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
        [DevExpress.Xpo.DisplayName("Serie Certificado")]
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
        [DevExpress.Xpo.DisplayName("Emisor Certificado")]
        [NonPersistent]
        public string EmisorCertif
        {
            get { return mEmisorCertif; }
            // set { SetPropertyValue("EmisorCertif", ref mEmisorCertif, value); }
            set { mEmisorCertif = value; }
        }

        private DateTime mFechaIni;
        [DevExpress.Xpo.DisplayName("Fecha Inicial")]
        // [Appearance("FechaIni", Context = "DetailView", Enabled = false, FontStyle = System.Drawing.FontStyle.Italic)]
        [ModelDefault("DisplayFormat", "{0:dd MMM yyyy}")]
        public DateTime FechaIni
        {
            get { return mFechaIni; }
            set { SetPropertyValue("FechaIni", ref mFechaIni, value); }
        }

        private DateTime mFechaFin;
        [ImmediatePostData]
        [DevExpress.Xpo.DisplayName("Fecha Final")]
        // [Appearance("FechaFin", Context = "DetailView", Enabled = false, FontStyle = System.Drawing.FontStyle.Italic)]
        [ModelDefault("DisplayFormat", "{0:dd MMM yyyy}")]
        public DateTime FechaFin
        {
            get { return mFechaFin; }
            set { SetPropertyValue("FechaFin", ref mFechaFin, value); }
        }

        private string mCertificadoCad;
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
        [RuleRequiredField("RuleRequiredField for Certificado.FlCrtfcdPm", DefaultContexts.Save, "Debe asignar el Archivo Certificado PEM", SkipNullOrEmptyValues = false)]
        [XafDisplayName("Archivo Certificado PEM")]
        //[VisibleInDetailView(false)]
        public MyFileData FlCrtfcdPm
        {
            get { return mFlCrtfcdPm; }
            set { SetPropertyValue("FlCrtfcdPm", ref mFlCrtfcdPm, value); }
        }

        private MyFileData mFlKyPm;
        [RuleRequiredField("RuleRequiredField for Certificado.FlKyPm", DefaultContexts.Save, "Debe asignar el Archivo Llave PEM", SkipNullOrEmptyValues = false)]
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
            get { return FlKyPm != null && FlKyPm.Size > 0; }
        }


        /* qué hongo con esto? Oct 2018
        private bool NoIsAdmin()
        {
            bool isAdm = false;

            SecuritySystemUser currentUser = SecuritySystem.CurrentUser as SecuritySystemUser;
            if (currentUser != null)
            {
                foreach (SecuritySystemRole role in currentUser.Roles)
                {
                    if (role.Name == "Administrator")
                    {
                        isAdm = true;
                        break;
                    }
                }
            }
            return !isAdm;
        }*/

        /*
        private void ValoresCertificado()
        {
            if (mFileCertif != null &&
                !string.IsNullOrEmpty(mFileCertif.FullName))
            {
                X509Certificate2 cert509 = null;

                try
                {
                    cert509 = new X509Certificate2(mFileCertif.FullName);
                }
                catch (Exception)
                {
                }

                if (cert509 != null)
                {
                    / * Hasta que se genera el archivo pem !
                    StringPropertyEditor crt = dv.FindItem("SerieCertif") as StringPropertyEditor;

                    if (crt != null)
                    {
                        string ArchCerPem = string.Empty;

                        ArchCerPem = cert.FileCertif.FullName + ".pem";

                        TextReader trCer = new StreamReader(ArchCerPem);
                        PemReader rdCer = new PemReader(trCer);

                        Org.BouncyCastle.X509.X509Certificate Cert = (Org.BouncyCastle.X509.X509Certificate)rdCer.ReadObject();

                        byte[] nSerie = Cert.SerialNumber.ToByteArray();
                        string nCertificado = Encoding.ASCII.GetString(nSerie);

                        crt.PropertyValue = nCertificado; //Encoding.Default.GetString(nCertificado);
                    }* /

                    EmprCertif = cert509.SubjectName.Name;
                    / *
                    StringPropertyEditor emp = dv.FindItem("EmprCertif") as StringPropertyEditor;
                    if (emp != null)
                        emp.PropertyValue = cert509.SubjectName.Name;* /

                    EmisorCertif = cert509.IssuerName.Name;
                    / *
                    StringPropertyEditor emsr = dv.FindItem("EmisorCertif") as StringPropertyEditor;
                    if (emsr != null)
                        emsr.PropertyValue = cert509.IssuerName.Name;* /

                    FechaIni = Convert.ToDateTime(cert509.GetEffectiveDateString());
                    / *
                    DatePropertyEditor fi = dv.FindItem("FechaIni") as DatePropertyEditor;
                    if (fi != null)
                        fi.PropertyValue = Convert.ToDateTime(cert509.GetEffectiveDateString());* /

                    FechaFin = Convert.ToDateTime(cert509.GetExpirationDateString());
                    / *
                    DatePropertyEditor ff = dv.FindItem("FechaFin") as DatePropertyEditor;
                    if (ff != null)
                        ff.PropertyValue = Convert.ToDateTime(cert509.GetExpirationDateString());* /
                }
            }
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

    /* Qué hongo con esto? Oct 2018
    public enum EVerFacElec
    {
        Ninguno = 0,
        [XafDisplayName("Versión 2.0")]
        CFDV20 = 1,
        [XafDisplayName("Versión 2.2")]
        CFDV22 = 2,
        [XafDisplayName("Versión 3.0")]
        CFDI30 = 3,
        [XafDisplayName("Versión 3.2")]
        CFDI32 = 4
    }*/
}
