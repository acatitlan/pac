namespace TCap.Module.Controllers.Proyectos
{
    partial class VCProyecto
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
            this.simpleActionCxc = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.simpleActionAcptd = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.simpleActionCncld = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.simpleActionTrmnd = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.simpleActionEnPrcs = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.popupWindowShowActionNwTsk = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            this.popupWindowShowActionPryctCncld = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            this.simpleActionImprmrCtzcn = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.simpleActionEdtrCtzcn = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.popupWindowShowActionEnvrCrr = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            // 
            // simpleActionCxc
            // 
            this.simpleActionCxc.Caption = "Crea la Cxc";
            this.simpleActionCxc.ConfirmationMessage = "Está seguro de Crear la Cxc ?";
            this.simpleActionCxc.Id = "f2fd6aae-bc6c-44fc-982f-1dc8bdf92dd8";
            this.simpleActionCxc.ToolTip = null;
            this.simpleActionCxc.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.simpleActionCxc_Execute);
            // 
            // simpleActionAcptd
            // 
            this.simpleActionAcptd.Caption = "Proyecto Aceptado";
            this.simpleActionAcptd.Category = "RecordEdit";
            this.simpleActionAcptd.ConfirmationMessage = "Está seguro de Activar el Proyecto ?";
            this.simpleActionAcptd.Id = "eeae70fa-34f2-43aa-a307-27bd02475115";
            this.simpleActionAcptd.ToolTip = null;
            this.simpleActionAcptd.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.simpleActionAcptd_Execute);
            // 
            // simpleActionCncld
            // 
            this.simpleActionCncld.Caption = "Proyecto Cancelado";
            this.simpleActionCncld.ConfirmationMessage = "Está seguro de Cancelar el Proyecto ?";
            this.simpleActionCncld.Id = "8dcba1ab-38af-4f64-8b7b-6d0d7b938eac";
            this.simpleActionCncld.ToolTip = null;
            this.simpleActionCncld.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.simpleActionCncld_Execute);
            // 
            // simpleActionTrmnd
            // 
            this.simpleActionTrmnd.Caption = "Proyecto Terminado";
            this.simpleActionTrmnd.ConfirmationMessage = "Está seguro de Terminar el Proyecto?";
            this.simpleActionTrmnd.Id = "66f69e8c-e788-4e6d-bedf-c8fa85ffffbe";
            this.simpleActionTrmnd.ToolTip = null;
            this.simpleActionTrmnd.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.simpleActionTrmnd_Execute);
            // 
            // simpleActionEnPrcs
            // 
            this.simpleActionEnPrcs.Caption = "En Proceso";
            this.simpleActionEnPrcs.ConfirmationMessage = "Está seguro de Poner en Marcha el Proyecto?";
            this.simpleActionEnPrcs.Id = "063eda01-b38d-4377-b429-1dd7579a410f";
            this.simpleActionEnPrcs.ToolTip = null;
            this.simpleActionEnPrcs.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.simpleActionEnPrcs_Execute);
            // 
            // popupWindowShowActionNwTsk
            // 
            this.popupWindowShowActionNwTsk.AcceptButtonCaption = "Agregar";
            this.popupWindowShowActionNwTsk.CancelButtonCaption = null;
            this.popupWindowShowActionNwTsk.Caption = "Agregar Tarea";
            this.popupWindowShowActionNwTsk.Category = "RecordEdit";
            this.popupWindowShowActionNwTsk.ConfirmationMessage = null;
            this.popupWindowShowActionNwTsk.Id = "77fcb4c5-200a-4df2-a7da-5ec4962bf472";
            this.popupWindowShowActionNwTsk.ToolTip = null;
            this.popupWindowShowActionNwTsk.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.popupWindowShowActionNwTsk_CustomizePopupWindowParams);
            this.popupWindowShowActionNwTsk.Execute += new DevExpress.ExpressApp.Actions.PopupWindowShowActionExecuteEventHandler(this.popupWindowShowActionNwTsk_Execute);
            // 
            // popupWindowShowActionPryctCncld
            // 
            this.popupWindowShowActionPryctCncld.AcceptButtonCaption = "Cancelar Proyecto";
            this.popupWindowShowActionPryctCncld.CancelButtonCaption = null;
            this.popupWindowShowActionPryctCncld.Caption = "Cancelar Proyecto";
            this.popupWindowShowActionPryctCncld.Category = "RecordEdit";
            this.popupWindowShowActionPryctCncld.ConfirmationMessage = null;
            this.popupWindowShowActionPryctCncld.Id = "ce364e00-38b0-4f8e-b4c1-d51df5e06529";
            this.popupWindowShowActionPryctCncld.ToolTip = null;
            this.popupWindowShowActionPryctCncld.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.popupWindowShowActionPryctCncld_CustomizePopupWindowParams);
            this.popupWindowShowActionPryctCncld.Execute += new DevExpress.ExpressApp.Actions.PopupWindowShowActionExecuteEventHandler(this.popupWindowShowActionPryctCncld_Execute);
            // 
            // simpleActionImprmrCtzcn
            // 
            this.simpleActionImprmrCtzcn.Caption = "Imprimir Cotización";
            this.simpleActionImprmrCtzcn.Category = "Print";
            this.simpleActionImprmrCtzcn.ConfirmationMessage = "Está seguro de Imprimir la Cotización?";
            this.simpleActionImprmrCtzcn.Id = "ca284f30-7513-4cf7-8215-01d924030a4b";
            this.simpleActionImprmrCtzcn.ToolTip = null;
            this.simpleActionImprmrCtzcn.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.simpleActionImprmrCtzcn_Execute);
            // 
            // simpleActionEdtrCtzcn
            // 
            this.simpleActionEdtrCtzcn.Caption = "Editar Cotización";
            this.simpleActionEdtrCtzcn.Category = "Export";
            this.simpleActionEdtrCtzcn.ConfirmationMessage = "Está seguro de Editar la Cotización ?";
            this.simpleActionEdtrCtzcn.Id = "d278c7af-fc6a-4202-a8de-4ac4ddb8dc72";
            this.simpleActionEdtrCtzcn.ToolTip = null;
            this.simpleActionEdtrCtzcn.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.simpleActionEdtrCtzcn_Execute);
            // 
            // popupWindowShowActionEnvrCrr
            // 
            this.popupWindowShowActionEnvrCrr.AcceptButtonCaption = "Enviar";
            this.popupWindowShowActionEnvrCrr.CancelButtonCaption = "Cancelar";
            this.popupWindowShowActionEnvrCrr.Caption = "Enviar Correo";
            this.popupWindowShowActionEnvrCrr.Category = "PopupActions";
            this.popupWindowShowActionEnvrCrr.ConfirmationMessage = null;
            this.popupWindowShowActionEnvrCrr.Id = "990643c5-47f4-4d33-9afd-36b6fd4b8c3d";
            this.popupWindowShowActionEnvrCrr.ToolTip = null;
            this.popupWindowShowActionEnvrCrr.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.popupWindowShowActionEnvrCrr_CustomizePopupWindowParams);
            this.popupWindowShowActionEnvrCrr.Execute += new DevExpress.ExpressApp.Actions.PopupWindowShowActionExecuteEventHandler(this.popupWindowShowActionEnvrCrr_Execute);
            // 
            // VCProyecto
            // 
            this.Actions.Add(this.simpleActionCxc);
            this.Actions.Add(this.simpleActionAcptd);
            this.Actions.Add(this.simpleActionCncld);
            this.Actions.Add(this.simpleActionTrmnd);
            this.Actions.Add(this.simpleActionEnPrcs);
            this.Actions.Add(this.popupWindowShowActionNwTsk);
            this.Actions.Add(this.popupWindowShowActionPryctCncld);
            this.Actions.Add(this.simpleActionImprmrCtzcn);
            this.Actions.Add(this.simpleActionEdtrCtzcn);
            this.Actions.Add(this.popupWindowShowActionEnvrCrr);
            this.ViewControlsCreated += new System.EventHandler(this.VCProyecto_ViewControlsCreated);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction simpleActionCxc;
        private DevExpress.ExpressApp.Actions.SimpleAction simpleActionAcptd;
        private DevExpress.ExpressApp.Actions.SimpleAction simpleActionCncld;
        private DevExpress.ExpressApp.Actions.SimpleAction simpleActionTrmnd;
        private DevExpress.ExpressApp.Actions.SimpleAction simpleActionEnPrcs;
        private DevExpress.ExpressApp.Actions.PopupWindowShowAction popupWindowShowActionNwTsk;
        private DevExpress.ExpressApp.Actions.PopupWindowShowAction popupWindowShowActionPryctCncld;
        private DevExpress.ExpressApp.Actions.SimpleAction simpleActionImprmrCtzcn;
        private DevExpress.ExpressApp.Actions.SimpleAction simpleActionEdtrCtzcn;
        private DevExpress.ExpressApp.Actions.PopupWindowShowAction popupWindowShowActionEnvrCrr;
    }
}
