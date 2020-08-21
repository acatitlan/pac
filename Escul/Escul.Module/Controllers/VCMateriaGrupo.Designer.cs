namespace Escul.Module.Controllers
{
    partial class VCMateriaGrupo
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

        #region InitializeComponent
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.simpleActionExprtrLst = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.simpleActionClclHrs = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.simpleActionClclHrsSmn = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.simpleActionClclFchAplccn = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.simpleActionImprtLstAlmns = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.simpleActionClfccnFnl = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.popupWindowShowActionFlter = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            this.popupWindowShowActionFOSEP09 = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            this.popupWindowShowActionClclClfccn = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            this.simpleActionObtnTms = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // simpleActionExprtrLst
            // 
            this.simpleActionExprtrLst.Caption = "Exportar Lista Alumnos";
            this.simpleActionExprtrLst.ConfirmationMessage = "Está seguro de exportar la Lista?";
            this.simpleActionExprtrLst.Id = "d038e14f-015c-4257-b16f-c67b4f7dae8d";
            this.simpleActionExprtrLst.ToolTip = null;
            this.simpleActionExprtrLst.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.simpleActionExprtrLst_Execute);
            // 
            // simpleActionClclHrs
            // 
            this.simpleActionClclHrs.Caption = "Calcula Horas Totales";
            this.simpleActionClclHrs.ConfirmationMessage = "Está seguro de Calcular las Horas?";
            this.simpleActionClclHrs.Id = "b82421bc-45aa-4f7c-b6c0-5dd645f0ad9e";
            this.simpleActionClclHrs.ToolTip = "Calcula cuántas horas totales para esta materia";
            this.simpleActionClclHrs.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.simpleActionClclHrs_Execute);
            // 
            // simpleActionClclHrsSmn
            // 
            this.simpleActionClclHrsSmn.Caption = "Calcula Horas Semana";
            this.simpleActionClclHrsSmn.ConfirmationMessage = "Está seguro de calcular las Horas Semana?";
            this.simpleActionClclHrsSmn.Id = "d733c3b5-1d9f-4947-bc2a-98e923ace219";
            this.simpleActionClclHrsSmn.ToolTip = "Calcula las horas a la Semana de acuerdo al Horario";
            this.simpleActionClclHrsSmn.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.simpleActionClclHrsSmn_Execute);
            // 
            // simpleActionClclFchAplccn
            // 
            this.simpleActionClclFchAplccn.Caption = "Calcula Fechas de Aplicación";
            this.simpleActionClclFchAplccn.ConfirmationMessage = "Está seguro de Calcular las Fechas?";
            this.simpleActionClclFchAplccn.Id = "7b465eb3-5e52-4519-a224-2c5ae444c1a4";
            this.simpleActionClclFchAplccn.ToolTip = "Según horas y Calendario calcula la Fecha para los Temas";
            this.simpleActionClclFchAplccn.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.simpleActionClclFchAplccn_Execute);
            // 
            // simpleActionImprtLstAlmns
            // 
            this.simpleActionImprtLstAlmns.Caption = "Importa Alumnos";
            this.simpleActionImprtLstAlmns.ConfirmationMessage = "Está seguro de Importar Alumnos";
            this.simpleActionImprtLstAlmns.Id = "337a55c8-bdd7-44ee-b953-8c20e74334b5";
            this.simpleActionImprtLstAlmns.ToolTip = "Se captura en el sistemas los ALUMNOS.TXT";
            this.simpleActionImprtLstAlmns.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.simpleActionImprtLstAlmns_Execute);
            // 
            // simpleActionClfccnFnl
            // 
            this.simpleActionClfccnFnl.Caption = "Calcula Calificación Final";
            this.simpleActionClfccnFnl.ConfirmationMessage = "Está seguro que desa Calcular C.F.?";
            this.simpleActionClfccnFnl.Id = "d1a99cb2-5a41-421d-9198-aae58cd64125";
            this.simpleActionClfccnFnl.ToolTip = "Calcula la calificación Final";
            this.simpleActionClfccnFnl.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.simpleActionClfccnFnl_Execute);
            // 
            // popupWindowShowActionFlter
            // 
            this.popupWindowShowActionFlter.AcceptButtonCaption = "Generar";
            this.popupWindowShowActionFlter.CancelButtonCaption = null;
            this.popupWindowShowActionFlter.Caption = "Generar Reporte EC FOSEP 10";
            this.popupWindowShowActionFlter.ConfirmationMessage = "Está seguro de generar el Reporte?";
            this.popupWindowShowActionFlter.Id = "91ca4825-1b8c-4301-b64a-ccacfeec2e14";
            this.popupWindowShowActionFlter.ToolTip = "Formato de Evaluación Continua";
            this.popupWindowShowActionFlter.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.popupWindowShowActionFlter_CustomizePopupWindowParams);
            this.popupWindowShowActionFlter.Execute += new DevExpress.ExpressApp.Actions.PopupWindowShowActionExecuteEventHandler(this.popupWindowShowActionFlter_Execute);
            // 
            // popupWindowShowActionFOSEP09
            // 
            this.popupWindowShowActionFOSEP09.AcceptButtonCaption = "Genera";
            this.popupWindowShowActionFOSEP09.CancelButtonCaption = null;
            this.popupWindowShowActionFOSEP09.Caption = "Formatos";
            this.popupWindowShowActionFOSEP09.ConfirmationMessage = null;
            this.popupWindowShowActionFOSEP09.Id = "7348ea3e-00e6-4140-9707-f8137feb4bd5";
            this.popupWindowShowActionFOSEP09.ToolTip = null;
            this.popupWindowShowActionFOSEP09.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.popupWindowShowActionFOSEP09_CustomizePopupWindowParams);
            this.popupWindowShowActionFOSEP09.Execute += new DevExpress.ExpressApp.Actions.PopupWindowShowActionExecuteEventHandler(this.popupWindowShowActionFOSEP09_Execute);
            // 
            // popupWindowShowActionClclClfccn
            // 
            this.popupWindowShowActionClclClfccn.AcceptButtonCaption = "Calcula";
            this.popupWindowShowActionClclClfccn.CancelButtonCaption = null;
            this.popupWindowShowActionClclClfccn.Caption = "Calcula calificación";
            this.popupWindowShowActionClclClfccn.ConfirmationMessage = "Está seguro de Calcular?";
            this.popupWindowShowActionClclClfccn.Id = "d6fdfe75-bedb-4ad3-b7fc-ff8e3f2e2e26";
            this.popupWindowShowActionClclClfccn.ToolTip = "Calcula de cierto periodo y cierto tipo la Calificación de un periodo";
            this.popupWindowShowActionClclClfccn.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.popupWindowShowActionClclClfccn_CustomizePopupWindowParams);
            this.popupWindowShowActionClclClfccn.Execute += new DevExpress.ExpressApp.Actions.PopupWindowShowActionExecuteEventHandler(this.popupWindowShowActionClclClfccn_Execute);
            // 
            // simpleActionObtnTms
            // 
            this.simpleActionObtnTms.Caption = "Obten Temas";
            this.simpleActionObtnTms.ConfirmationMessage = "Está seguro de Obtener los Temas ?";
            this.simpleActionObtnTms.Id = "e114e8dc-a317-4540-b2f7-6ab570394d2a";
            this.simpleActionObtnTms.ToolTip = null;
            this.simpleActionObtnTms.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.simpleActionObtnTms_Execute);
            // 
            // VCMateriaGrupo
            // 
            this.Actions.Add(this.simpleActionExprtrLst);
            this.Actions.Add(this.simpleActionClclHrs);
            this.Actions.Add(this.simpleActionClclHrsSmn);
            this.Actions.Add(this.simpleActionClclFchAplccn);
            this.Actions.Add(this.simpleActionImprtLstAlmns);
            this.Actions.Add(this.simpleActionClfccnFnl);
            this.Actions.Add(this.popupWindowShowActionFlter);
            this.Actions.Add(this.popupWindowShowActionFOSEP09);
            this.Actions.Add(this.popupWindowShowActionClclClfccn);
            this.Actions.Add(this.simpleActionObtnTms);

        }
        #endregion

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction simpleActionExprtrLst;
        private DevExpress.ExpressApp.Actions.SimpleAction simpleActionClclHrs;
        private DevExpress.ExpressApp.Actions.SimpleAction simpleActionClclHrsSmn;
        private DevExpress.ExpressApp.Actions.SimpleAction simpleActionClclFchAplccn;
        private DevExpress.ExpressApp.Actions.SimpleAction simpleActionImprtLstAlmns;
        private DevExpress.ExpressApp.Actions.SimpleAction simpleActionClfccnFnl;
        private DevExpress.ExpressApp.Actions.PopupWindowShowAction popupWindowShowActionFlter;
        private DevExpress.ExpressApp.Actions.PopupWindowShowAction popupWindowShowActionFOSEP09;
        private DevExpress.ExpressApp.Actions.PopupWindowShowAction popupWindowShowActionClclClfccn;
        private DevExpress.ExpressApp.Actions.SimpleAction simpleActionObtnTms;
    }
}
