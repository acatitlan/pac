namespace FCap.Module.Controllers
{
    partial class VCClienteList
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
            this.popupWindowShowActionImprtrClnts = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            // 
            // popupWindowShowActionImprtrClnts
            // 
            this.popupWindowShowActionImprtrClnts.AcceptButtonCaption = "Importar";
            this.popupWindowShowActionImprtrClnts.ActionMeaning = DevExpress.ExpressApp.Actions.ActionMeaning.Accept;
            this.popupWindowShowActionImprtrClnts.CancelButtonCaption = "Cancelar";
            this.popupWindowShowActionImprtrClnts.Caption = "Importar Clientes";
            this.popupWindowShowActionImprtrClnts.ConfirmationMessage = null;
            this.popupWindowShowActionImprtrClnts.Id = "31605a42-724c-4f9f-97ce-71e86e111361";
            this.popupWindowShowActionImprtrClnts.ToolTip = null;
            this.popupWindowShowActionImprtrClnts.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.popupWindowShowActionImprtrClnts_CustomizePopupWindowParams);
            this.popupWindowShowActionImprtrClnts.Execute += new DevExpress.ExpressApp.Actions.PopupWindowShowActionExecuteEventHandler(this.popupWindowShowActionImprtrClnts_Execute);
            // 
            // VCClienteList
            // 
            this.Actions.Add(this.popupWindowShowActionImprtrClnts);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.PopupWindowShowAction popupWindowShowActionImprtrClnts;
    }
}
