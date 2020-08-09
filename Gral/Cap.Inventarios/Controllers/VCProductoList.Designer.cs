namespace Cap.Inventarios.Controllers
{
    partial class VCProductoList
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
            this.popupWindowShowActionActPrecs = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            // 
            // popupWindowShowActionActPrecs
            // 
            this.popupWindowShowActionActPrecs.AcceptButtonCaption = "Actualizar";
            this.popupWindowShowActionActPrecs.CancelButtonCaption = "Cancelar";
            this.popupWindowShowActionActPrecs.Caption = "Actualizar Precios";
            this.popupWindowShowActionActPrecs.ConfirmationMessage = null;
            this.popupWindowShowActionActPrecs.Id = "4a601862-4627-497d-89c2-c5e3d478e4b8";
            this.popupWindowShowActionActPrecs.ToolTip = null;
            this.popupWindowShowActionActPrecs.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.popupWindowShowActionActPrecs_CustomizePopupWindowParams);
            this.popupWindowShowActionActPrecs.Execute += new DevExpress.ExpressApp.Actions.PopupWindowShowActionExecuteEventHandler(this.popupWindowShowActionActPrecs_Execute);
            // 
            // VCProductoList
            // 
            this.Actions.Add(this.popupWindowShowActionActPrecs);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.PopupWindowShowAction popupWindowShowActionActPrecs;
    }
}
