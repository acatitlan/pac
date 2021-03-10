using System;
using System.ComponentModel;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using TCap.Module.BusinessObjects.Proyectos;
using TCap.Module.Utilerias;
using TCap.Module.BusinessObjects.Empresa;
using DevExpress.Data.Filtering;
using DevExpress.XtraRichEdit;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace TCap.Module.Controllers.Proyectos
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class VCProyecto : ViewController
    {
        public VCProyecto()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.

            TargetObjectType = typeof(Proyecto);
            simpleActionCxc.TargetViewType = ViewType.DetailView;

            simpleActionAcptd.TargetObjectType = typeof(Proyecto);
            simpleActionAcptd.TargetViewType = ViewType.DetailView;

            simpleActionCncld.TargetObjectType = typeof(Proyecto);
            simpleActionCncld.TargetViewType = ViewType.DetailView;

            simpleActionTrmnd.TargetObjectType = typeof(Proyecto);
            simpleActionTrmnd.TargetViewType = ViewType.DetailView;

            simpleActionEnPrcs.TargetObjectType = typeof(Proyecto);
            simpleActionEnPrcs.TargetViewType = ViewType.DetailView;

            popupWindowShowActionNwTsk.TargetObjectType = typeof(Proyecto);
            popupWindowShowActionPryctCncld.TargetObjectType = typeof(Proyecto);

            simpleActionImprmrCtzcn.TargetObjectType = typeof(Proyecto);
            simpleActionImprmrCtzcn.SelectionDependencyType = SelectionDependencyType.RequireSingleObject;
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.
            View.ObjectSpace.Committing += ObjectSpace_Committing;

            simpleActionCxc.Active.SetItemValue("Visible", false);
            simpleActionCncld.Active.SetItemValue("Visible", false);

            simpleActionAcptd.TargetObjectsCriteria = "[Stts] == 'Prospecto' && !IsNewObject(This)";
            simpleActionEnPrcs.TargetObjectsCriteria = "[Stts] == 'Aceptado' && [VvMrt] != 'Cancelado'";
            simpleActionCxc.TargetObjectsCriteria = "[Stts] == 'Activo'";
            simpleActionCncld.TargetObjectsCriteria = "!IsNewObject(This) && ([VvMrt] != 'Cancelado' && [Stts] != 'Terminado')";
            simpleActionTrmnd.TargetObjectsCriteria = "[Stts] == 'EnProceso' && [VvMrt] != 'Cancelado'";

            popupWindowShowActionNwTsk.TargetObjectsCriteria = "[VvMrt] != 'Cancelado' && [Stts] != 'Prospecto'";
            popupWindowShowActionPryctCncld.TargetObjectsCriteria = "!IsNewObject(This) && ([VvMrt] != 'Cancelado' && [Stts] != 'Terminado')";
        }

        private void ObjectSpace_Committing(object sender, CancelEventArgs e)
        {
            if (View != null)
            {
                Proyecto doc = View.CurrentObject as Proyecto;

                if (doc != null && View.ObjectSpace != null)
                {
                    doc.SttsAntrr = doc.Stts;
                    if (View.ObjectSpace.IsNewObject(doc))
                        Negocio.GrabaProyecto(doc);
                }
            }
        }

        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
        }
        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }

        private void simpleActionCxc_Execute(object sender, SimpleActionExecuteEventArgs e)
        {

        }

        private void VCProyecto_ViewControlsCreated(object sender, EventArgs e)
        {
            if (View != null)
            {
                Proyecto doc = View.CurrentObject as Proyecto;

                if (View.ObjectSpace != null && View.ObjectSpace.IsNewObject(doc))
                    Negocio.IniciaProyecto(doc as Proyecto);

                /*
                if (doc != null)
                {
                    doc.RemisionItems.ListChanged += RemisionItems_ListChanged;
                }*/
            }
        }

        private void simpleActionAcptd_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            if (View != null)
            {
                Proyecto doc = View.CurrentObject as Proyecto;

                // doc.Stts = EProyectoStatus.Aceptado;
                Negocio.ProyectoAceptado(doc);
                View.ObjectSpace.CommitChanges();
            }
        }

        private void simpleActionCncld_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            if (View != null)
            {
                Proyecto doc = View.CurrentObject as Proyecto;

                doc.SttsAntrr = doc.Stts;
                doc.VvMrt = EEstadoDcmntPryct.Cancelado;
            }
        }

        private void simpleActionTrmnd_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            if (View != null)
            {
                Proyecto doc = View.CurrentObject as Proyecto;

                doc.Stts = EProyectoStatus.Terminado;
            }
        }

        private void simpleActionEnPrcs_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            if (View != null)
            {
                Proyecto doc = View.CurrentObject as Proyecto;

                doc.Stts = EProyectoStatus.EnProceso;
            }
        }

        private void popupWindowShowActionNwTsk_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            IObjectSpace objectSpace = Application.CreateObjectSpace();
            AgregarActividad newObj;

            newObj = objectSpace.FindObject<AgregarActividad>(null);
            if (newObj != null)
                objectSpace.Delete(newObj);

            newObj = objectSpace.CreateObject<AgregarActividad>();
            e.View = Application.CreateDetailView(objectSpace, "AgregarActividad_DetailView", true, newObj);
        }

        private void popupWindowShowActionNwTsk_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            AgregarActividad ha = e.PopupWindowViewCurrentObject as AgregarActividad;

            if (View != null)
            {
                IObjectSpace os = View.ObjectSpace;
                Proyecto obj = View.CurrentObject as Proyecto;

                Actividad act = os.CreateObject<Actividad>();

                act.Asnt = ha.Asnt;
                if (ha.Asgnd != null)
                    act.Asgnd = os.FindObject<EmpleadoProyecto>(new BinaryOperator("Oid", ha.Asgnd.Oid));
                act.HrsEstmds = ha.HrsEstmds;
                act.Prrdd = ha.Prrdd;
                act.TpP = ha.TpP;
                act.FchVncmnt = ha.FchVncmnt;
                if (ha.FchVncmnt != null)
                    act.RemindIn = TimeSpan.FromMinutes(1);
                act.Nts = ha.Nts;


                obj.Tareas.Add(act);
                os.CommitChanges();
            }
        }

        private void popupWindowShowActionPryctCncld_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            IObjectSpace objectSpace = Application.CreateObjectSpace();
            ProyectoCancelado newObj = objectSpace.FindObject<ProyectoCancelado>(null);

            // TIT 2021 Mrz no me deja reusar un mismo objeto
            if (newObj != null)
                objectSpace.Delete(newObj);


            newObj = objectSpace.CreateObject<ProyectoCancelado>();
            e.View = Application.CreateDetailView(objectSpace, "ProyectoCancelado_DetailView", true, newObj);
        }

        // TIT Mrz 2021 No recuerdo por qué tengo dos acciones 
        // que 'aparentemente' hacen lo mismo, dejaré sólo esta
        private void popupWindowShowActionPryctCncld_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            ProyectoCancelado ha = e.PopupWindowViewCurrentObject as ProyectoCancelado;

            if (View != null)
            {
                IObjectSpace os = e.PopupWindowView.ObjectSpace;
                Proyecto obj = os.FindObject<Proyecto>(new BinaryOperator("Oid",
                    (View.CurrentObject as Proyecto).Oid));

                obj.SttsAntrr = obj.Stts;
                obj.VvMrt = EEstadoDcmntPryct.Cancelado;
                obj.FchCnclcn = ha.FchCnclcn;
                obj.MtvCnclcnP = ha.MtvCnclcnP;
                obj.Cnclcn = ha.Cnclcn;

                os.CommitChanges();
            }
        }

        private void simpleActionImprmrCtzcn_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            Proyecto obj = View.CurrentObject as Proyecto;
            RichEditDocumentServer server = new RichEditDocumentServer();
            List<Proyecto> datos = new List<Proyecto>();
            datos.Add(obj);

            server.LoadDocument("Cotizacion.docx", DocumentFormat.Doc);

            DevExpress.XtraPrinting.PdfExportOptions pdf = new DevExpress.XtraPrinting.PdfExportOptions();

            FieldOptions fieldOptions = server.Options.Fields;
            fieldOptions.HighlightMode = FieldsHighlightMode.Auto;
            fieldOptions.HighlightColor = System.Drawing.Color.LightSalmon;
            fieldOptions.ThrowExceptionOnInvalidFormatSwitch = true;

            // pdf.PageRange = "1";
            server.Options.MailMerge.ViewMergedData = true;
            server.Options.MailMerge.DataSource = datos;
            server.Document.Fields.Update();

            string fn = string.Format("{0}.pdf", obj.Clv);
            server.ExportToPdf(fn);
            ViewFile(fn);
        }

        private void ViewFile(string fn)
        {
            Process process = new Process();

            if (File.Exists(fn))
            {
                process.StartInfo.FileName = fn;
                process.Start();
                process.WaitForInputIdle();
            }
        }

        private void simpleActionEdtrCtzcn_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            ViewFile("Cotizacion.docx");
        }

        private void popupWindowShowActionEnvrCrr_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            IObjectSpace objectSpace = Application.CreateObjectSpace();
            ProyectoCancelado newObj = objectSpace.CreateObject<ProyectoCancelado>();
            e.View = Application.CreateDetailView(objectSpace, "ProyectoCancelado_DetailView", true, newObj);
        }

        private void popupWindowShowActionEnvrCrr_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {

        }
    }
}
