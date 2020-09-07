using Cap.Ventas.BusinessObjects;

namespace FCap.Module.Controllers
{
    partial class VCFactura
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.simpleActionImprmAcs = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.simpleActionSavSellr = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.simpleActionCanclr = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.simpleActionImprmr = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.simpleActionMail = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.simpleActionRprtRsmn = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.simpleActionSellr = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.simpleActionCreaPdf = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.simpleActionViewPdf = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.simpleActionVerXML = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // simpleActionImprmAcs
            // 
            this.simpleActionImprmAcs.Caption = "Imprimir Acuse";
            this.simpleActionImprmAcs.ConfirmationMessage = null;
            this.simpleActionImprmAcs.Id = "dae45127-c0ab-4bdb-974c-9c83fe3a551e";
            this.simpleActionImprmAcs.ToolTip = null;
            this.simpleActionImprmAcs.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.simpleActionImprmAcs_Execute);
            // 
            // simpleActionSavSellr
            // 
            this.simpleActionSavSellr.ActionMeaning = DevExpress.ExpressApp.Actions.ActionMeaning.Accept;
            this.simpleActionSavSellr.Caption = "Guardar y Sellar";
            this.simpleActionSavSellr.Category = "Save";
            this.simpleActionSavSellr.ConfirmationMessage = "Está seguro de Guardar y Sellar ?";
            this.simpleActionSavSellr.Id = "65b34ba5-bedc-4a37-80b0-4f8dce230094";
            this.simpleActionSavSellr.ToolTip = null;
            this.simpleActionSavSellr.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.simpleActionSavSellr_Execute);
            // 
            // simpleActionCanclr
            // 
            this.simpleActionCanclr.Caption = "Cancelar Documento";
            this.simpleActionCanclr.ConfirmationMessage = "Está seguro de Cancelar ?";
            this.simpleActionCanclr.Id = "b55feb43-b06f-4d5d-98d9-0eaba96cee1c";
            this.simpleActionCanclr.ImageName = "Baloon";
            this.simpleActionCanclr.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
            this.simpleActionCanclr.TargetObjectType = typeof(DocumentoSalida);
            this.simpleActionCanclr.ToolTip = null;
            this.simpleActionCanclr.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.simpleActionCanclr_Execute);
            // 
            // simpleActionImprmr
            // 
            this.simpleActionImprmr.Caption = "Imprimir Documento";
            this.simpleActionImprmr.ConfirmationMessage = null;
            this.simpleActionImprmr.Id = "6207169c-830e-4582-bc8b-0a7674835de7";
            this.simpleActionImprmr.ImageName = "Print_Area";
            this.simpleActionImprmr.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.CaptionAndImage;
            this.simpleActionImprmr.TargetObjectType = typeof(DocumentoSalida);
            this.simpleActionImprmr.ToolTip = null;
            this.simpleActionImprmr.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.simpleActionImprmr_Execute);
            // 
            // simpleActionMail
            // 
            this.simpleActionMail.Caption = "Enviar por Correo";
            this.simpleActionMail.ConfirmationMessage = null;
            this.simpleActionMail.Id = "e34b6559-b5e7-4b8a-b924-6a5a46afb915";
            this.simpleActionMail.ImageName = "Mail_New_2";
            this.simpleActionMail.TargetObjectType = typeof(DocumentoSalida);
            this.simpleActionMail.ToolTip = null;
            this.simpleActionMail.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.simpleActionMail_Execute);
            // 
            // simpleActionRprtRsmn
            // 
            this.simpleActionRprtRsmn.Caption = "Reporte Resumen";
            this.simpleActionRprtRsmn.Category = "Reports";
            this.simpleActionRprtRsmn.ConfirmationMessage = null;
            this.simpleActionRprtRsmn.Id = "b0e72446-000a-4d5f-9d7c-9126fc58d002";
            this.simpleActionRprtRsmn.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
            this.simpleActionRprtRsmn.TargetObjectType = typeof(DocumentoSalida);
            this.simpleActionRprtRsmn.ToolTip = null;
            this.simpleActionRprtRsmn.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.simpleActionRprtRsmn_Execute);
            // 
            // simpleActionSellr
            // 
            this.simpleActionSellr.Caption = "Sellar Documento";
            this.simpleActionSellr.Category = "Tools";
            this.simpleActionSellr.ConfirmationMessage = "Está seguro de Sellar el Documento?";
            this.simpleActionSellr.Id = "c5b17c65-6b7c-47f7-87ee-7397c09e957e";
            this.simpleActionSellr.TargetObjectType = typeof(DocumentoSalida);
            this.simpleActionSellr.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            this.simpleActionSellr.ToolTip = null;
            this.simpleActionSellr.TypeOfView = typeof(DevExpress.ExpressApp.ListView);
            this.simpleActionSellr.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.simpleActionSellr_Execute);
            // 
            // simpleActionCreaPdf
            // 
            this.simpleActionCreaPdf.Caption = "Genera Pdf";
            this.simpleActionCreaPdf.Category = "Options";
            this.simpleActionCreaPdf.ConfirmationMessage = "Está seguro de generar el Pdf ?";
            this.simpleActionCreaPdf.Id = "e56ecb41-0665-404c-8ddb-ae6be7304f5a";
            this.simpleActionCreaPdf.ToolTip = null;
            this.simpleActionCreaPdf.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.simpleActionCreaPdf_Execute);
            // 
            // simpleActionViewPdf
            // 
            this.simpleActionViewPdf.Caption = "Ver Pdf";
            this.simpleActionViewPdf.Category = "View";
            this.simpleActionViewPdf.ConfirmationMessage = null;
            this.simpleActionViewPdf.Id = "cb7b8b9e-f7da-4a72-8faa-0e1389f9378f";
            this.simpleActionViewPdf.ToolTip = null;
            this.simpleActionViewPdf.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.simpleActionViewPdf_Execute);
            // 
            // simpleActionVerXML
            // 
            this.simpleActionVerXML.Caption = "Ver XML";
            this.simpleActionVerXML.Category = "View";
            this.simpleActionVerXML.ConfirmationMessage = null;
            this.simpleActionVerXML.Id = "VwXML";
            this.simpleActionVerXML.ToolTip = "Muestra el XML";
            this.simpleActionVerXML.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.simpleActionVerXML_Execute);
            // 
            // VCFactura
            // 
            this.ViewControlsCreated += new System.EventHandler(this.VCFactura_ViewControlsCreated);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction simpleActionCanclr;
        private DevExpress.ExpressApp.Actions.SimpleAction simpleActionImprmr;
        private DevExpress.ExpressApp.Actions.SimpleAction simpleActionMail;
        private DevExpress.ExpressApp.Actions.SimpleAction simpleActionRprtRsmn;
        private DevExpress.ExpressApp.Actions.SimpleAction simpleActionSellr;
        private DevExpress.ExpressApp.Actions.SimpleAction simpleActionImprmAcs;
        private DevExpress.ExpressApp.Actions.SimpleAction simpleActionSavSellr;
        private DevExpress.ExpressApp.Actions.SimpleAction simpleActionCreaPdf;
        private DevExpress.ExpressApp.Actions.SimpleAction simpleActionViewPdf;
        private DevExpress.ExpressApp.Actions.SimpleAction simpleActionVerXML;
    }
}
