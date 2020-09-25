namespace Cap.Bancos.Controllers
{
    partial class VCMovimientoB
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
            this.simpleActionCanclr = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.simpleActionRprtMovsmnts = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.simpleActionAplTrnst = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // simpleActionCanclr
            // 
            this.simpleActionCanclr.Caption = "Cancelar Movimiento";
            this.simpleActionCanclr.Category = "Edit";
            this.simpleActionCanclr.ConfirmationMessage = "Está seguro de Cancelar ?";
            this.simpleActionCanclr.Id = "0078b469-c3a1-4dac-a696-15de36a1543d";
            this.simpleActionCanclr.ImageName = "Baloom";
            this.simpleActionCanclr.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
            this.simpleActionCanclr.ToolTip = null;
            this.simpleActionCanclr.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.simpleActionCanclr_Execute);
            // 
            // simpleActionRprtMovsmnts
            // 
            this.simpleActionRprtMovsmnts.Caption = "Reporte Movimientos";
            this.simpleActionRprtMovsmnts.Category = "Print";
            this.simpleActionRprtMovsmnts.ConfirmationMessage = null;
            this.simpleActionRprtMovsmnts.Id = "6fc1dea5-ee66-4fa1-a328-d627590383ef";
            this.simpleActionRprtMovsmnts.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            this.simpleActionRprtMovsmnts.ToolTip = null;
            this.simpleActionRprtMovsmnts.TypeOfView = typeof(DevExpress.ExpressApp.ListView);
            this.simpleActionRprtMovsmnts.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.simpleActionRprtMovsmnts_Execute);
            // 
            // simpleActionAplTrnst
            // 
            this.simpleActionAplTrnst.Caption = "Aplicar Tránsito";
            this.simpleActionAplTrnst.Category = "Edit";
            this.simpleActionAplTrnst.ConfirmationMessage = null;
            this.simpleActionAplTrnst.Id = "112e8c78-d5db-4ea4-bef4-237c80db74ed";
            this.simpleActionAplTrnst.ToolTip = null;
            this.simpleActionAplTrnst.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.simpleActionAplTrnst_Execute);
            // 
            // VCMovimientoB
            // 
            this.Actions.Add(this.simpleActionCanclr);
            this.Actions.Add(this.simpleActionRprtMovsmnts);
            this.Actions.Add(this.simpleActionAplTrnst);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction simpleActionCanclr;
        private DevExpress.ExpressApp.Actions.SimpleAction simpleActionRprtMovsmnts;
        private DevExpress.ExpressApp.Actions.SimpleAction simpleActionAplTrnst;
    }
}
