namespace FCap.Module.Win.Controllers
{
    partial class VCEmpresaABC
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
            this.simpleActionPAC = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // simpleActionPAC
            // 
            this.simpleActionPAC.Caption = "Probar PAC";
            this.simpleActionPAC.ConfirmationMessage = null;
            this.simpleActionPAC.Id = "72ee6d43-93ba-4013-ae54-79a66fe028a7";
            this.simpleActionPAC.IsExecuting = false;
            this.simpleActionPAC.ToolTip = null;
            this.simpleActionPAC.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.simpleActionPAC_Execute);
            // 
            // VCEmpresaABC
            // 
            this.ViewControlsCreated += new System.EventHandler(this.VCEmpresaABC_ViewControlsCreated);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction simpleActionPAC;
    }
}
