#region Copyright (c) 2017-2020 TIT
/*
{+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++)
{                                                                   }
{     Cap control administrativo personal                           }
{                                                                   }
{     Copyrigth (c) 2017-2020                                       }
{     tlacaelel.icpac@gmail.com                                     }
{                                                                   }
{*******************************************************************}
 */
#endregion

using System;
using Cap.Fe.BusinessObjects;
using Cap.Generales.BusinessObjects.Empresa;
using Cap.Generales.BusinessObjects.General;
using Cap.Ventas.BusinessObjects;
using Cap.Ventas.Utilerias;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.BaseImpl;

namespace Cap.Ventas.Controllers
{
    // TIT Sep 2018
    // Tengo una Accion de CreaPdf, pero no sé si sea necesaria ahora
    // Tenemos una acción que sólo sella, pero como ya está la de guardar y sellar
    // La accion de reporte resumen, pero como sale de la sección de reportes
    //
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class VCFactura : ViewController
    {
        public VCFactura()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.

            TargetObjectType = typeof(DocumentoSalida);

            simpleActionMail.TargetObjectType = typeof(DocumentoSalida);
            simpleActionMail.TargetObjectsCriteria = string.Format("Status == {0}", DocumentoStatus.Sellada.GetHashCode());

            simpleActionVerXML.TargetViewType = ViewType.ListView;
            simpleActionVerXML.TargetObjectType = typeof(DocumentoSalida);
            simpleActionVerXML.ImageName = "Document-Properties";
            simpleActionVerXML.TargetObjectsCriteria = "Status == 'Sellada'";
            simpleActionVerXML.SelectionDependencyType = SelectionDependencyType.RequireSingleObject;

            simpleActionViewPdf.TargetViewType = ViewType.ListView;
            simpleActionViewPdf.TargetObjectType = typeof(DocumentoSalida);
            simpleActionViewPdf.ImageName = "Doc-Acrobat";
            simpleActionViewPdf.TargetObjectsCriteria = "Status == 'Sellada'";
            simpleActionViewPdf.SelectionDependencyType = SelectionDependencyType.RequireSingleObject;

            simpleActionSavSellr.TargetViewType = ViewType.DetailView;
            simpleActionSavSellr.Id = "Cap.Ventas.SvSllr";
            simpleActionSavSellr.Caption = "Guardar y Sellar";
            simpleActionSavSellr.Category = "Save";
            simpleActionSavSellr.ConfirmationMessage = "Está seguro de Guardar y Sellar ?";
            simpleActionSavSellr.ActionMeaning = ActionMeaning.Accept;
            simpleActionSavSellr.Execute += SimpleActionSavSellr_Execute;
            simpleActionSavSellr.ImageName = "Save_and_Close";
            simpleActionSavSellr.TargetObjectType = typeof(DocumentoSalida);
            simpleActionSavSellr.TargetObjectsCriteria = string.Format("Status == {0}", DocumentoStatus.Alta.GetHashCode());

            simpleActionCanclr.TargetViewType = ViewType.ListView;
            simpleActionCanclr.TargetObjectsCriteria = string.Format("Status != {0}", DocumentoStatus.Cancelado.GetHashCode());
            simpleActionCanclr.TargetObjectType = typeof(DocumentoSalida);


            simpleActionImprmAcs.TargetObjectType = typeof(DocumentoSalida);
            simpleActionImprmAcs.TargetObjectsCriteria = string.Format("Status == {0}", DocumentoStatus.Cancelado.GetHashCode());

            simpleActionImprmr.TargetObjectType = typeof(DocumentoSalida);
            simpleActionImprmr.SelectionDependencyType = SelectionDependencyType.RequireSingleObject;

            simpleActionCreaPdf.TargetObjectType = typeof(DocumentoSalida);
            simpleActionCreaPdf.SelectionDependencyType = SelectionDependencyType.RequireSingleObject;

            simpleActionCrCxc.TargetObjectType = typeof(DocumentoSalida);
            simpleActionCrCxc.TargetObjectsCriteria = string.Format("Status == 'Sellada'");

            simpleActionCnstrSttsSAT.TargetObjectType = typeof(DocumentoSalida);
            simpleActionCnstrSttsSAT.TargetViewType = ViewType.ListView;
        }

        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.

            if (View is DetailView)
                View.ObjectSpace.Committing += ObjectSpace_Committing;


            bool puede = Empresa != null ? Empresa.ConCfdi : false;
            simpleActionSavSellr.Active.SetItemValue("Visible", puede && Licencia() /* Empresa.ConCfdi*/);
            simpleActionImprmAcs.Active.SetItemValue("Visible", puede /* Empresa.ConCfdi*/);
            simpleActionMail.Active.SetItemValue("Visible", puede /* Empresa.ConCfdi*/);

            // Se agregó al Portafolio
            //simpleActionVerXML.Active.SetItemValue("Visible", false);
            simpleActionVerXML.ToolTip = "Tal Vez está en los Archivos de la Factura";
            //simpleActionViewPdf.Active.SetItemValue("Visible", false);
            simpleActionViewPdf.ToolTip = "Tal Vez está en los Archivos de la Factura";


            /* No se dejó
            if ((View is ListView))
            {
                GroupOperator grp = new GroupOperator();

                for (int i = 1; i < 2; i++)
                {
                    grp.Operands.Add(GroupOperator.And(new BinaryOperator("FechaDoc", apl.Log.Fecha.FechaInicial(i), BinaryOperatorType.GreaterOrEqual),
                        new BinaryOperator("FechaDoc", apl.Log.Fecha.FechaFinal(i), BinaryOperatorType.LessOrEqual)));
                    
                    string fullMonthName = new DateTime(2015, i, 1).ToString("MMMM", CultureInfo.CreateSpecificCulture("es"));
                    ((ListView)View).CollectionSource.Criteria[fullMonthName] = grp;
                    
                    grp = new GroupOperator();
                }
            }*/
            // Vemos a ver si queda aquí o lo movemos de lugar
            //
        }

        private bool Licencia()
        {
            return NegocioVentas.Licencia(View.ObjectSpace);
        }

        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.

            //*
            if (View != null && View is DetailView)
            {
                DocumentoSalida fac = View.CurrentObject as DocumentoSalida;

                if (View.ObjectSpace != null && View.ObjectSpace.IsNewObject(fac))
                    NegocioVentas.IniciaDocumento(fac);

                if (fac != null)
                {
                    fac.VentaItems.ListChanged += FacturaItems_ListChanged;
                }
            }//*/
        }

        protected override void OnDeactivated()
        {
            //*
            if (View is DetailView)
                View.ObjectSpace.Committing -= ObjectSpace_Committing;//*/

            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }

        void FacturaItems_ListChanged(object sender, System.ComponentModel.ListChangedEventArgs e)
        {
            if (View != null && View.ObjectSpace != null && !View.ObjectSpace.IsCommitting)
            {
                DocumentoSalida fac = View.CurrentObject as DocumentoSalida;

                NegocioVentas.CalculaDocumento(fac);
            }
        }

        void ObjectSpace_Committing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;

            if (View != null)
            {
                DocumentoSalida fac = View.CurrentObject as DocumentoSalida;

                if (View.ObjectSpace != null)
                {
                    if (View.ObjectSpace.IsNewObject(fac))
                    {
                        NegocioVentas.GrabaDocumento(fac);
                        e.Cancel = false;
                    }
                    else
                    {
                        e.Cancel = (fac.Status == DocumentoStatus.Cancelado ||
                            (Empresa.ConCfdi && fac.Status == DocumentoStatus.Sellada));
                    }
                }
            }
        }


        private void SimpleActionSavSellr_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            if (View != null && View.ObjectSpace != null)
            {
                DocumentoSalida doc = View.CurrentObject as DocumentoSalida;
                if (string.IsNullOrEmpty(doc.Sello))
                {
                    View.ObjectSpace.CommitChanges();

                    CfgPagos();
                    CfgCertificado();

                    Sella();
                    View.ObjectSpace.Committing -= ObjectSpace_Committing;
                    View.ObjectSpace.CommitChanges();
                    View.ObjectSpace.Committing += ObjectSpace_Committing;

                    /* Puede mandar una excepcion si no encuentra o
                     puede crear la ruta. Tal vez si no la encuentra 
                     debería guardarlo en memoria */
                    CreaPdfImprime(true, true);
                    View.ObjectSpace.Committing -= ObjectSpace_Committing;
                    View.ObjectSpace.CommitChanges();
                    View.ObjectSpace.Committing += ObjectSpace_Committing;

                    NegocioVentas.CreaCxc(View.CurrentObject as DocumentoSalida, Prms);
                    View.ObjectSpace.Committing -= ObjectSpace_Committing;
                    View.ObjectSpace.CommitChanges();
                    View.ObjectSpace.Committing += ObjectSpace_Committing;

                    // DocumentoSalida doc = View.CurrentObject as DocumentoSalida;
                    if (Prms.SendM && doc.EnvioC != EnvioCorreo.Enviado)
                    {
                        NegocioVentas.EnviaMailDocTim(doc, View.ObjectSpace, Prms);
                        View.ObjectSpace.Committing -= ObjectSpace_Committing;
                        View.ObjectSpace.CommitChanges();
                        View.ObjectSpace.Committing += ObjectSpace_Committing;
                    }

                    /*Feb 2021 se enoja, pero tal vez no es necesario
                    View.Close();*/
                }
            }
        }


        private void Sella()
        {
            if (View != null)
            {
                DocumentoSalida doc = View.CurrentObject as DocumentoSalida;

                if (doc != null)
                {
                    if (string.IsNullOrEmpty(doc.SelloSat))
                    {
                        NegocioVentas.Sella(doc, View.ObjectSpace);
                        doc.Status = DocumentoStatus.Sellada;
                    }
                }
            }
        }


        /// <summary>
        /// Obtiene el documento actual de la vista y, lo imprime o crea el pdf
        /// </summary>
        /// <param name="imprime"></param>
        /// <param name="pdf"></param>
        private void CreaPdfImprime(bool imprime, bool pdf)
        {
            if (View != null)
            {
                DocumentoSalida doc = View.CurrentObject as DocumentoSalida;

                if (doc != null)
                {
                    string aux = string.Format("FacturaCFDI{0}", doc.Cliente != null
                        ? doc.Cliente.Clave.Trim()
                        : string.Empty);

                    if (doc.Session.FindObject<ReportDataV2>(
                        new BinaryOperator("DisplayName", aux)) == null)
                        aux = "FacturaCFDI";

                    NegocioVentas.ImprFto(aux, imprime, pdf,
                        View.ObjectSpace, doc, Prms, Application.Modules);
                }
            }
        }

        private Empresa mEmpresa;
        private Empresa Empresa
        {
            get
            {
                if (mEmpresa == null)
                {
                    if (View != null && View.ObjectSpace != null)
                        mEmpresa = View.ObjectSpace.FindObject<Empresa>(null);
                }
                return mEmpresa;
            }
        }

        private void simpleActionVerXML_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            DocumentoSalida doc = View.CurrentObject as DocumentoSalida;
            string path = NameXml(doc as Documento);

            NegocioVentas.VerArchivo(DocumentType.Xml, path, doc);
        }

        private string NameXml(Documento doc)
        {
            return NegocioVentas.NamePdf(doc, doc.Status, "xml", Prms);
        }

        private VentaCfdi mPrms;
        private VentaCfdi Prms
        {
            get
            {
                if (mPrms == null)
                {
                    if (View != null && View.ObjectSpace != null)
                        mPrms = View.ObjectSpace.FindObject<VentaCfdi>(null);
                }
                return mPrms;
            }
        }

        private void simpleActionViewPdf_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            DocumentoSalida doc = View.CurrentObject as DocumentoSalida;
            string path = NegocioVentas.NamePdf(doc, DocumentoStatus.Sellada, "pdf", Prms);

            NegocioVentas.VerArchivo(DocumentType.Pdf, path, doc);
        }


        private void CfgPagos()
        {
            DocumentoSalida doc = View.CurrentObject as DocumentoSalida;

            if (doc.LyndPg != null && doc.LyndPg.Clv == "PPD")
            {
                if (Prms.CncptCbr == null)
                {
                    throw new Exception($"Falta configurar el Concepto de Cobro." +
                        $"{Environment.NewLine}(En Parámetros)");
                }
            }
        }

        private void CfgCertificado()
        {
            Certificado crt = View.ObjectSpace.FindObject<Certificado>(null);

            if ((crt.FlCrtfcdPm == null || string.IsNullOrEmpty(crt.FlCrtfcdPm.FileName))
                || (crt.FlKyPm == null || string.IsNullOrEmpty(crt.FlKyPm.FileName)
                || crt.FlKyPm.Size <= 0))
            {
                throw new Exception($"Falta configurar el Certificado del Sello. " +
                    $"{Environment.NewLine}(En Parámetros)");
            }
        }

        private void simpleActionMail_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            if (View != null)
            {
                foreach (DocumentoSalida doc in View.SelectedObjects)
                {
                    NegocioVentas.EnviaMailDocTim(doc, View.ObjectSpace, Prms);
                }
                View.ObjectSpace.Committing -= ObjectSpace_Committing;
                View.ObjectSpace.CommitChanges();
                View.ObjectSpace.Committing += ObjectSpace_Committing;
            }
        }

        string ftoCan = "AcuseCancelacion";
        private void simpleActionImprmAcs_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            if (View != null)
            {
                NegocioVentas.ImprFto(ftoCan, true, true, View.ObjectSpace, 
                    View.CurrentObject as DocumentoSalida, Prms, Application.Modules);
            }
        }

        private void simpleActionImprmr_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            CreaPdfImprime(true, false);
        }

        private void simpleActionCanclr_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            if (View != null && View.ObjectSpace != null)
            {
                DocumentoSalida fac = View.CurrentObject as DocumentoSalida;

                if (fac != null)
                {
                    NegocioVentas.CancelaDocumento(fac);

                    View.ObjectSpace.Committing -= ObjectSpace_Committing;
                    View.ObjectSpace.CommitChanges();
                    View.ObjectSpace.Committing += ObjectSpace_Committing;

                    // Mejor lo guardamos y después enviamos la notificación por correo Jun 2014
                    if (fac.EnvioC == EnvioCorreo.Enviado)
                        NegocioVentas.EnviaNotifCan(View.ObjectSpace, fac);

                    // Imprime acuse
                    if (Empresa != null && Empresa.ConCfdi && !string.IsNullOrEmpty(fac.SelloSatCan))
                        NegocioVentas.ImprFto(ftoCan, true, true, View.ObjectSpace, 
                            fac, Prms, Application.Modules);
                }
            }
        }

        private void simpleActionCreaPdf_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            CreaPdfImprime(false, true);
        }

        private void simpleActionCrCxc_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            NegocioVentas.CreaCxc(View.CurrentObject as DocumentoSalida, Prms);
            View.ObjectSpace.Committing -= ObjectSpace_Committing;
            View.ObjectSpace.CommitChanges();
            View.ObjectSpace.Committing += ObjectSpace_Committing;
        }

        private void simpleActionCnstrSttsSAT_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            DocumentoSalida doc = View.CurrentObject as DocumentoSalida;

            NegocioVentas.EsttusSAT(doc);

            View.ObjectSpace.Committing -= ObjectSpace_Committing;
            View.ObjectSpace.CommitChanges();
            View.ObjectSpace.Committing += ObjectSpace_Committing;
        }
    }
}
