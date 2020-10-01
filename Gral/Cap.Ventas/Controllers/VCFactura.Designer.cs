namespace Cap.Ventas.Controllers
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
            this.simpleActionSavSellr = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.simpleActionVerXML = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.simpleActionViewPdf = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.simpleActionMail = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.simpleActionImprmAcs = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.simpleActionImprmr = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.simpleActionCanclr = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.simpleActionCreaPdf = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.simpleActionCrCxc = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.simpleActionCnstrSttsSAT = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // simpleActionSavSellr
            // 
            this.simpleActionSavSellr.Caption = "312fa769-4eb3-4de3-a856-960ca2a458e6";
            this.simpleActionSavSellr.ConfirmationMessage = null;
            this.simpleActionSavSellr.Id = "312fa769-4eb3-4de3-a856-960ca2a458e6";
            this.simpleActionSavSellr.ToolTip = null;
            this.simpleActionSavSellr.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.SimpleActionSavSellr_Execute);
            // 
            // simpleActionVerXML
            // 
            this.simpleActionVerXML.Caption = "Ver XML";
            this.simpleActionVerXML.Category = "View";
            this.simpleActionVerXML.ConfirmationMessage = null;
            this.simpleActionVerXML.Id = "42d5f17e-eb07-4fde-9bf8-97aa54c5b8a0";
            this.simpleActionVerXML.ToolTip = "Muestra el XML";
            this.simpleActionVerXML.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.simpleActionVerXML_Execute);
            // 
            // simpleActionViewPdf
            // 
            this.simpleActionViewPdf.Caption = "Ver Pdf";
            this.simpleActionViewPdf.Category = "View";
            this.simpleActionViewPdf.ConfirmationMessage = null;
            this.simpleActionViewPdf.Id = "823484f4-fce5-412c-853b-5da881366f1d";
            this.simpleActionViewPdf.ToolTip = null;
            this.simpleActionViewPdf.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.simpleActionViewPdf_Execute);
            // 
            // simpleActionMail
            // 
            this.simpleActionMail.Caption = "Enviar por Correo";
            this.simpleActionMail.ConfirmationMessage = null;
            this.simpleActionMail.Id = "fba7fb96-b70b-4f29-928f-2e232635bc3e";
            this.simpleActionMail.ImageName = "Mail_New_2";
            this.simpleActionMail.ToolTip = null;
            this.simpleActionMail.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.simpleActionMail_Execute);
            // 
            // simpleActionImprmAcs
            // 
            this.simpleActionImprmAcs.Caption = "Imprimir Acuse";
            this.simpleActionImprmAcs.ConfirmationMessage = null;
            this.simpleActionImprmAcs.Id = "d9b4d44a-7f2f-452a-80ee-771a86c24c19";
            this.simpleActionImprmAcs.ToolTip = null;
            this.simpleActionImprmAcs.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.simpleActionImprmAcs_Execute);
            // 
            // simpleActionImprmr
            // 
            this.simpleActionImprmr.Caption = "Imprimir Documento";
            this.simpleActionImprmr.ConfirmationMessage = null;
            this.simpleActionImprmr.Id = "6d47f2b2-fb60-4dbe-82a8-66835c09f5ec";
            this.simpleActionImprmr.ImageName = "Print_Area";
            this.simpleActionImprmr.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.CaptionAndImage;
            this.simpleActionImprmr.ToolTip = null;
            this.simpleActionImprmr.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.simpleActionImprmr_Execute);
            // 
            // simpleActionCanclr
            // 
            this.simpleActionCanclr.Caption = "Cancelar Documento";
            this.simpleActionCanclr.ConfirmationMessage = "Está seguro de Cancelar ?";
            this.simpleActionCanclr.Id = "8ca4dbef-9245-418c-b543-6a8a68cae92e";
            this.simpleActionCanclr.ImageName = "Baloon";
            this.simpleActionCanclr.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
            this.simpleActionCanclr.ToolTip = null;
            this.simpleActionCanclr.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.simpleActionCanclr_Execute);
            // 
            // simpleActionCreaPdf
            // 
            this.simpleActionCreaPdf.Caption = "Genera Pdf";
            this.simpleActionCreaPdf.Category = "Options";
            this.simpleActionCreaPdf.ConfirmationMessage = "Está seguro de generar el Pdf ?";
            this.simpleActionCreaPdf.Id = "7dab7c74-2ea1-42cf-b625-b2c5a29391b2";
            this.simpleActionCreaPdf.ToolTip = null;
            this.simpleActionCreaPdf.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.simpleActionCreaPdf_Execute);
            // 
            // simpleActionCrCxc
            // 
            this.simpleActionCrCxc.Caption = "Generar la CXC";
            this.simpleActionCrCxc.ConfirmationMessage = "Está seguro de Generar la CXC?";
            this.simpleActionCrCxc.Id = "d3f9f112-8f5b-45d8-a993-bd603ea09d15";
            this.simpleActionCrCxc.ToolTip = null;
            this.simpleActionCrCxc.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.simpleActionCrCxc_Execute);
            // 
            // simpleActionCnstrSttsSAT
            // 
            this.simpleActionCnstrSttsSAT.Caption = "Consultar Status SAT";
            this.simpleActionCnstrSttsSAT.ConfirmationMessage = "Está seguro de Consultar el Status ?";
            this.simpleActionCnstrSttsSAT.Id = "b307beae-b8bf-4c5d-bd93-293baaa77185";
            this.simpleActionCnstrSttsSAT.ToolTip = null;
            this.simpleActionCnstrSttsSAT.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.simpleActionCnstrSttsSAT_Execute);
            // 
            // VCFactura
            // 
            this.Actions.Add(this.simpleActionSavSellr);
            this.Actions.Add(this.simpleActionVerXML);
            this.Actions.Add(this.simpleActionViewPdf);
            this.Actions.Add(this.simpleActionMail);
            this.Actions.Add(this.simpleActionImprmAcs);
            this.Actions.Add(this.simpleActionImprmr);
            this.Actions.Add(this.simpleActionCanclr);
            this.Actions.Add(this.simpleActionCreaPdf);
            this.Actions.Add(this.simpleActionCrCxc);
            this.Actions.Add(this.simpleActionCnstrSttsSAT);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction simpleActionSavSellr;
        private DevExpress.ExpressApp.Actions.SimpleAction simpleActionVerXML;
        private DevExpress.ExpressApp.Actions.SimpleAction simpleActionViewPdf;
        private DevExpress.ExpressApp.Actions.SimpleAction simpleActionMail;
        private DevExpress.ExpressApp.Actions.SimpleAction simpleActionImprmAcs;
        private DevExpress.ExpressApp.Actions.SimpleAction simpleActionImprmr;
        private DevExpress.ExpressApp.Actions.SimpleAction simpleActionCanclr;
        private DevExpress.ExpressApp.Actions.SimpleAction simpleActionCreaPdf;
        private DevExpress.ExpressApp.Actions.SimpleAction simpleActionCrCxc;
        private DevExpress.ExpressApp.Actions.SimpleAction simpleActionCnstrSttsSAT;
    }
}
