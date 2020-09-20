using Cap.Generales.BusinessObjects.General;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using System;
using System.ComponentModel;
using System.Drawing;

namespace Cap.Compras.BusinessObjects
{
    [DefaultProperty("Clave")]
    [NavigationItem("Compras")]
    [Appearance("Recepcion.Cancel", TargetItems = "Status, Total", Context = "ListView", Criteria = "[Status] = 4", FontColor = "Red")]
    [ImageName("Receipt")]
    public partial class Recepcion
    {
        private MyFileData mArchXml;
        [Appearance("Recepcion.ArchXml", AppearanceItemType = "ViewItem", Context = "DetailView", Enabled = false, Criteria = "!IsNewObject(This)")]
        [VisibleInListView(false)]
        [DevExpress.Xpo.DisplayName("Archivo XML")]
        [FileTypeFilter("Xml", 1, "*.xml")]
        public MyFileData ArchXml
        {
            get { return mArchXml; }
            set { SetPropertyValue("ArchXml", ref mArchXml, value); }
        }

        // Apply the Association attribute to mark the Orders property 
        // as the one end of the Customer-Orders association.
        [Appearance("Recepcion.RecepcionItems", AppearanceItemType = "ViewItem", Context = "DetailView", Enabled = false, Criteria = "!IsNewObject(This)")]
        [ExpandObjectMembers(ExpandObjectMembers.Always)]
        [Association("Recepcion-RecepcionItems", typeof(RecepcionItem)), DevExpress.Xpo.Aggregated]
        public XPCollection RecepcionItems
        {
            get { return GetCollection("RecepcionItems"); }
        }


        private string mRefer;
        /// <summary>
        /// Es la clave del documento según el proveedor
        /// </summary>
        [XafDisplayName("Referencia")]
        [Appearance("Recepcion.Refer", AppearanceItemType = "ViewItem", Context = "DetailView", Enabled = false, Criteria = "!IsNewObject(This)")]
        [Size(10)]
        public string Refer
        {
            get { return mRefer; }
            set { SetPropertyValue("Refer", ref mRefer, ValorString("Refer", value)); }
        }

        private MyFileData mArchPdf;
        // Ene 2018 TIT ahora si se me olvida ponerlo o luego lo consigo quiero que me deje ponerlo.
        //[Appearance("Recepcion.ArchPdf", AppearanceItemType = "ViewItem", Context = "DetailView", Enabled = false, Criteria = "!IsNewObject(This)")]
        [VisibleInListView(false)]
        [DevExpress.Xpo.DisplayName("Archivo PDF")]
        [FileTypeFilter("Pdf", 1, "*.pdf")]
        public MyFileData ArchPdf
        {
            get { return mArchPdf; }
            set { SetPropertyValue("ArchPdf", ref mArchPdf, value); }
        }

        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        [NonPersistent]
        public string NumCertSAT;

        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        [NonPersistent]
        public string NumCertEmsr;

        private string mUuid;
        [Appearance("Recepcion.Uuid", Context = "DetailView", Enabled = false,
            FontStyle = FontStyle.Italic)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        [Size(60)]
        public string Uuid 
        {
            get { return mUuid; }
            set { SetPropertyValue("Uuid", ref mUuid, ValorString("Uuid", value)); }
        }

        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        [NonPersistent]
        public string FrmPg;

        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        [NonPersistent]
        public string MtdPg;

        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        [NonPersistent]
        public string NmrCnt;

        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        [NonPersistent]
        public string Bnc;

        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        [NonPersistent]
        public string CadenaCBB;

        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        [NonPersistent]
        public string SelloCfd;

        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        [NonPersistent]
        public string SelloSat;

        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        [NonPersistent]
        public string CdnOrgnl;

        private ConceptoC mCncpt;
        [XafDisplayName("Concepto")]
        public ConceptoC Cncpt
        {
            get { return mCncpt; }
            set { SetPropertyValue("Cncpt", ref mCncpt, value); }
        }

        // Está en la base como Imagen, pero comentada Ene 2016 TI
        private FileData mArchvImgn;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [DevExpress.Xpo.DisplayName("Archivo Imagen")]
        [FileTypeFilter("Imagenes", 1, "*.bmp", "*.png", "*.gif", "*.jpg")]
        [FileTypeFilter("Todos", 2, "*.*")]
        public FileData ArchvImgn
        {
            get { return mArchvImgn; }
            set { SetPropertyValue("ArchvImgn", ref mArchvImgn, value); }
        }


        public override void AfterConstruction()
        {
            base.AfterConstruction();

            Tipo = Ventas.BusinessObjects.DocumentoTipo.Recepcion;
        }


        public override XPCollection LasPartidas()
        {
            return RecepcionItems;
        }

        private bool MSinCfdi()
        {
            return true; // Empresa == null || !Empresa.ConCfdi;
        }

        private bool MRetenISR1()
        {
            return true;            
        }


        /*
        [Action(Caption = "Cancelar", ConfirmationMessage = "Está seguro de Cancelar?", 
        ImageName = "Note_Book_Delete", AutoCommit = true, 
        SelectionDependencyType = MethodActionSelectionDependencyType.RequireSingleObject, 
        TargetObjectsCriteria ="Status != 'Cancelado'")]
        public void ActionMethod()
        {
            // Trigger a custom business logic for the current record in the UI 
            (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
            this.Status = Cap.Ventas.BusinessObjects.DocumentoStatus.Cancelado;
        }*/
    }
}
