namespace MicroStore.Module.Controllers
{
    partial class VCVenta
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
            this.simpleActionAddItm = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.simpleActionCnclr = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // simpleActionAddItm
            // 
            this.simpleActionAddItm.Caption = "Agregar Item";
            this.simpleActionAddItm.Category = "Items";
            this.simpleActionAddItm.ConfirmationMessage = null;
            this.simpleActionAddItm.Id = "33d0ae1a-aadd-455d-9bb7-d962ea1c47fd";
            this.simpleActionAddItm.ToolTip = null;
            this.simpleActionAddItm.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.simpleActionAddItm_Execute);
            // 
            // simpleActionCnclr
            // 
            this.simpleActionCnclr.Caption = "Cancelar";
            this.simpleActionCnclr.ConfirmationMessage = "Está seguro de Cancelar ?";
            this.simpleActionCnclr.Id = "65cda530-e769-4059-8437-01fa8db60b6f";
            this.simpleActionCnclr.ToolTip = null;
            this.simpleActionCnclr.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.simpleActionCnclr_Execute);
            // 
            // VCVenta
            // 
            this.Actions.Add(this.simpleActionAddItm);
            this.Actions.Add(this.simpleActionCnclr);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction simpleActionAddItm;
        private DevExpress.ExpressApp.Actions.SimpleAction simpleActionCnclr;
    }
}
