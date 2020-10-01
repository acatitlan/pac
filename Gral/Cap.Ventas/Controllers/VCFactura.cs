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
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Mime;
using Cap.Clientes.BusinessObjects.Generales;
using Cap.Fe.BusinessObjects;
using Cap.Generales.BusinessObjects.Empresa;
using Cap.Generales.BusinessObjects.General;
using Cap.Generales.BusinessObjects.Object;
using Cap.Generales.Utilerias;
using Cap.Ventas.BusinessObjects;
using Cap.Ventas.Utilerias;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.ReportsV2;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using DevExpress.XtraReports.UI;

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

            simpleActionSavSellr.Id = "Cap.Ventas.SvSllr";
            simpleActionSavSellr.Caption = "Guardar y Sellar";
            simpleActionSavSellr.Category = "Save";
            simpleActionSavSellr.ConfirmationMessage = "Está seguro de Guardar y Sellar ?";
            simpleActionSavSellr.ActionMeaning = ActionMeaning.Accept;

            simpleActionSavSellr.Execute += SimpleActionSavSellr_Execute;
            simpleActionSavSellr.ImageName = "Save_and_Close";

            simpleActionSavSellr.TargetObjectType = typeof(DocumentoSalida);
            simpleActionSavSellr.TargetViewType = ViewType.DetailView;
            simpleActionSavSellr.TargetObjectsCriteria = string.Format("Status == {0}", DocumentoStatus.Alta.GetHashCode());


            simpleActionViewPdf.TargetObjectType = typeof(DocumentoSalida);
            simpleActionViewPdf.TargetViewType = ViewType.ListView;
            simpleActionViewPdf.ImageName = "Doc-Acrobat";
            simpleActionViewPdf.TargetObjectsCriteria = "Status == 'Sellada'";
            simpleActionViewPdf.SelectionDependencyType = SelectionDependencyType.RequireSingleObject;

            simpleActionVerXML.TargetObjectType = typeof(DocumentoSalida);
            simpleActionVerXML.TargetViewType = ViewType.ListView;
            simpleActionVerXML.ImageName = "Document-Properties";
            simpleActionVerXML.TargetObjectsCriteria = "Status == 'Sellada'";
            simpleActionVerXML.SelectionDependencyType = SelectionDependencyType.RequireSingleObject;

            simpleActionMail.TargetObjectType = typeof(DocumentoSalida);
            simpleActionMail.TargetObjectsCriteria = string.Format("Status == {0}", DocumentoStatus.Sellada.GetHashCode());

            simpleActionImprmAcs.TargetObjectType = typeof(DocumentoSalida);
            simpleActionImprmAcs.TargetObjectsCriteria = string.Format("Status == {0}", DocumentoStatus.Cancelado.GetHashCode());

            simpleActionImprmr.TargetObjectType = typeof(DocumentoSalida);
            simpleActionImprmr.SelectionDependencyType = SelectionDependencyType.RequireSingleObject;

            simpleActionCanclr.TargetObjectsCriteria = string.Format("Status != {0}", DocumentoStatus.Cancelado.GetHashCode());
            simpleActionCanclr.TargetObjectType = typeof(DocumentoSalida);
            simpleActionCanclr.TargetViewType = ViewType.ListView;

            simpleActionCreaPdf.TargetObjectType = typeof(DocumentoSalida);
            simpleActionCreaPdf.SelectionDependencyType = SelectionDependencyType.RequireSingleObject;

            simpleActionMail.TargetObjectsCriteria = string.Format("Status == {0}", DocumentoStatus.Sellada.GetHashCode());

            simpleActionCrCxc.TargetObjectType = typeof(DocumentoSalida);
            simpleActionCrCxc.TargetObjectsCriteria = string.Format("Status == 'Sellada'");
        }

        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.

            //*
            if (View is DetailView)
                View.ObjectSpace.Committing += ObjectSpace_Committing;

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
            bool puede = Empresa != null ? Empresa.ConCfdi : false;
            simpleActionSavSellr.Active.SetItemValue("Visible", puede && Licencia() /* Empresa.ConCfdi*/);
            simpleActionImprmAcs.Active.SetItemValue("Visible", puede /* Empresa.ConCfdi*/);
            simpleActionMail.Active.SetItemValue("Visible", puede /* Empresa.ConCfdi*/);

            // Se agregó al Portafolio
            //simpleActionVerXML.Active.SetItemValue("Visible", false);
            simpleActionVerXML.ToolTip = "Tal Vez está en los Archivos de la Factura";
            //simpleActionViewPdf.Active.SetItemValue("Visible", false);
            simpleActionViewPdf.ToolTip = "Tal Vez está en los Archivos de la Factura";
            /*
            popupWindowShowActionDscrgMsv.Active.SetItemValue("Visible", Licencia());*/


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

        bool Save = false;
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
                        e.Cancel = !Save && (fac.Status == DocumentoStatus.Cancelado ||
                            (Empresa.ConCfdi && fac.Status == DocumentoStatus.Sellada));
                        Save = false;
                    }
                }
            }
        }





        private void SimpleActionSavSellr_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            if (View != null && View.ObjectSpace != null)
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

                CreaCxc();
                View.ObjectSpace.Committing -= ObjectSpace_Committing;
                View.ObjectSpace.CommitChanges();
                View.ObjectSpace.Committing += ObjectSpace_Committing;

                DocumentoSalida doc = View.CurrentObject as DocumentoSalida;
                if (Prms.SendM && doc.EnvioC != EnvioCorreo.Enviado)
                {
                    Correo(doc);
                    View.ObjectSpace.Committing -= ObjectSpace_Committing;
                    View.ObjectSpace.CommitChanges();
                    View.ObjectSpace.Committing += ObjectSpace_Committing;
                }

                View.Close();
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

        private void CreaCxc()
        {
            if (View != null)
            {
                DocumentoSalida doc = View.CurrentObject as DocumentoSalida;

                if (doc != null)
                {
                    if (doc.Status == DocumentoStatus.Sellada)
                    {
                        NegocioVentas.FacGrabaCxc(doc, Prms);
                        // Y cómo sabemos que ya hicimos su cxc?
                        //doc.Status = DocumentoStatus.Sellada;
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

                    ImprFto(aux, imprime, pdf);
                }
            }
        }

        private void ImprFto(string format, bool imprime, bool pdf)
        {
            if (View != null && View.ObjectSpace != null)
            {
                DocumentoSalida fac = View.CurrentObject as DocumentoSalida;
                if (fac != null)
                {
                    IObjectSpace objectSpace =
                        ReportDataProvider.ReportObjectSpaceProvider.CreateObjectSpace(typeof(ReportDataV2));
                    Session uow = fac.Session;
                    DocumentoSalida doc = fac;

                    IReportDataV2 reportData2 = objectSpace.FindObject<ReportDataV2>(
                        new BinaryOperator("DisplayName", format));

                    doc.Empresa = uow.FindObject<Empresa>(null);
                    if ((!doc.Empresa.ConCfdi || doc.Status == DocumentoStatus.Alta)
                        && doc.Cliente != null)
                    {
                        doc.Cliente.Reload();
                        doc.Cliente.Compania.Reload();
                        doc.Cliente.Compania.Direccion.Reload();
                    }

                    QRCode2(doc);

                    if (reportData2 != null)
                    {
                        string reportContainerHandler =
                            ReportDataProvider.ReportsStorage.GetReportContainerHandle(reportData2);

                        XtraReport report2 = ReportDataProvider.ReportsStorage.LoadReport(reportData2);

                        List<DocumentoSalida> list = new List<DocumentoSalida>();
                        list.Add(doc);

                        report2.DataSource = list;

                        if (pdf)
                        {
                            string nameF;
                            string ruta;
                            try
                            {
                                // Puede ser que no pueda crear la ruta
                                // Entonces lo alojamos en memoria
                                nameF = NegocioVentas.NamePdf(doc, Prms.RutaPdfVnts);
                                ruta = Prms.RutaPdfVnts;
                            }
                            catch(Exception)
                            {
                                nameF = NegocioVentas.NamePdf(doc, string.Empty);
                                ruta = string.Empty;
                            }

                            if (string.IsNullOrEmpty(ruta))
                            {
                                PortfolioFileData fd = new PortfolioFileData(doc.Session);
                                MemoryStream ms = new MemoryStream();

                                report2.ExportToPdf(ms);
                                ms.Seek(0, SeekOrigin.Begin);

                                fd.File = new FileData(doc.Session);
                                fd.File.LoadFromStream(Path.GetFileName(nameF), ms);
                                fd.DocumentType = DocumentType.Pdf;

                                doc.Portfolio.Add(fd);
                            }
                            else
                            {
                                report2.ExportToPdf(nameF);
                            }
                        }
                        if (imprime)
                        {
                            try
                            {
                                ReportsModuleV2.FindReportsModule(Application.Modules).ReportsDataSourceHelper.SetupBeforePrint(report2);

                                report2.ShowPreview();
                            }
                            catch (Exception)
                            {
                            }
                        }
                    }
                }
            }
        }

        private void QRCode2(DocumentoSalida fac)
        {
            String data;

            if (fac != null && fac.Empresa != null && fac.Empresa.Compania != null
                && fac.Cliente != null && fac.Cliente.Compania != null)
            {
                data = string.Format("?re={0}&rr={1}&tt={2}&id={3}",
                    fac.Empresa.Compania.Rfc, fac.Cliente.Compania.Rfc,
                    fac.Total.ToString("F6"), fac.Uuid);
                data = string.Format("https://verificacfdi.facturaelectronica.sat.gob.mx/default.aspx?id={0}&re={1}&rr={2}&tt={3}&fe={4}",
                    fac.Uuid, fac.Empresa.Compania.Rfc, fac.Cliente.Compania.Rfc,
                    fac.Total.ToString("F6"),
                    string.IsNullOrEmpty(fac.Sello) ? string.Empty : fac.Sello.Substring(fac.Sello.Length - 8));
            }
            else
                data = string.Empty;

            fac.CadenaCBB = data;
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

            VerArchivo(DocumentType.Xml, path);
            /*
            if (View != null)
            {
                DocumentoSalida doc = View.CurrentObject as DocumentoSalida;

                if (doc != null)
                {
                    if (doc.Portfolio != null && doc.Portfolio.Count > 0)
                    {
                        foreach (PortfolioFileData fil in doc.Portfolio)
                        {
                            if (fil.DocumentType == DocumentType.Xml)
                            {
                                string tempDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("B"));
                                try
                                {
                                    Directory.CreateDirectory(tempDirectory);
                                }
                                catch
                                {
                                    Tracing.Tracer.LogValue("tempDirectory", tempDirectory);
                                    throw;
                                }
                                string tempFileName = Path.Combine(tempDirectory, fil.File.FileName);
                                try
                                {
                                    using (FileStream stream = new FileStream(tempFileName, FileMode.CreateNew))
                                    {
                                        fil.File.SaveToStream(stream);
                                    }
                                    Process.Start(tempFileName);
                                }
                                catch
                                {
                                    Tracing.Tracer.LogValue("tempFileName", tempFileName);
                                    throw;
                                }

                            }
                        }
                    }
                    else
                    {
                        Process process = new Process();
                        string path = NameXml(doc as Documento);
                        if (File.Exists(path))
                        {
                            process.StartInfo.FileName = path;
                            process.Start();
                            process.WaitForInputIdle();
                        }
                    }
                }
            }*/
        }


        private string NameXml(Documento doc)
        {
            return NamePdf(doc, doc.Status, "xml");
        }

        private string NamePdf(Documento doc, DocumentoStatus stat, string ext)
        {
            string aux = string.Empty;

            if (doc != null)
            {
                // TIT Falta la ruta
                string p = Path.Combine(Prms.RutaPdfVnts, doc.Tipo.ToString());
                if (string.IsNullOrEmpty(((DocumentoSalida)doc).Mostrador)
                    && !string.IsNullOrEmpty(Prms.RutaPdfVnts))
                    p = NegocioBase.CreaDirs(p, doc.FechaDoc);

                aux = Path.Combine(p, string.Format("{2}{0}-{1}.{3}", doc.Tipo,
                ((DocumentoSalida)doc).Clave.Trim(),
                (stat == DocumentoStatus.Cancelado && ext != "xml")
                ? "AC" : string.Empty, ext));
            }
            return aux;
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
            string path = NamePdf(doc, DocumentoStatus.Sellada, "pdf");

            VerArchivo(DocumentType.Pdf, path);

            /*
            if (View != null)
            {
                DocumentoSalida doc = View.CurrentObject as DocumentoSalida;
                if (doc != null)
                {
                    Process process = new Process();
                    string path = NamePdf(doc, DocumentoStatus.Sellada, "pdf");
                    if (File.Exists(path))
                    {
                        process.StartInfo.FileName = path;
                        process.Start();
                        process.WaitForInputIdle();
                    }
                }
            }*/
        }

        private void VerArchivo(DocumentType tA, string fN)
        {
            if (View != null)
            {
                DocumentoSalida doc = View.CurrentObject as DocumentoSalida;

                if (doc != null)
                {
                    if (doc.Portfolio != null && doc.Portfolio.Count > 0)
                    {
                        foreach (PortfolioFileData fil in doc.Portfolio)
                        {
                            if (fil.DocumentType == tA /*DocumentType.Xml*/)
                            {
                                string tempDirectory = Path.Combine(Path.GetTempPath(),
                                    Guid.NewGuid().ToString("B"));
                                try
                                {
                                    Directory.CreateDirectory(tempDirectory);
                                }
                                catch
                                {
                                    Tracing.Tracer.LogValue("tempDirectory", tempDirectory);
                                    throw;
                                }
                                string tempFileName = Path.Combine(tempDirectory,
                                    fil.File.FileName);
                                try
                                {
                                    using (FileStream stream = new FileStream(tempFileName,
                                        FileMode.CreateNew))
                                    {
                                        fil.File.SaveToStream(stream);
                                    }
                                    Process.Start(tempFileName);
                                }
                                catch
                                {
                                    Tracing.Tracer.LogValue("tempFileName", tempFileName);
                                    throw;
                                }

                            }
                        }
                    }
                    else
                    {
                        Process process = new Process();
                        // string path = NameXml(doc as Documento);
                        if (File.Exists(fN /*path*/))
                        {
                            process.StartInfo.FileName = fN; // path;
                            process.Start();
                            process.WaitForInputIdle();
                        }
                    }
                }
            }
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
                    Correo(doc);
                }
                View.ObjectSpace.Committing -= ObjectSpace_Committing;
                View.ObjectSpace.CommitChanges();
                View.ObjectSpace.Committing += ObjectSpace_Committing;
            }
        }

        private void Correo(DocumentoSalida fac)
        {
            if (View != null)
            {
                if (fac != null)
                {
                    Correo objCorreo = View.ObjectSpace.FindObject<Correo>(null);

                    if (objCorreo != null)
                    {
                        string aux = NamePdf(fac, fac.Status, "pdf");
                        string asn = String.Format("{0}({1})", objCorreo.Asunto, Path.GetFileName(aux));

                        CorreoSend.AddFile(aux, MediaTypeNames.Application.Pdf);
                        if (fac.Empresa.ConCfdi)
                        {
                            aux = Path.ChangeExtension(aux, ".xml");
                            CorreoSend.AddFile(aux, MediaTypeNames.Application.Soap);
                        }

                        MemoryStream ms = null;
                        if (fac.Empresa.Logo != null)
                        {
                            JpegStorageConverter jpeg = new JpegStorageConverter();

                            Byte[] arr = jpeg.ConvertToStorageType(fac.Empresa.Logo) as Byte[];
                            ms = new MemoryStream(arr);
                        }

                        string mail = string.Empty;

                        fac.Cliente.Compania.Direccion.Reload();
                        if (!string.IsNullOrEmpty(Prms.SendCopy))
                            mail = string.Format("{0}, {1}",
                                fac.Cliente.Compania.Direccion.Email, Prms.SendCopy);
                        else
                            mail = fac.Cliente.Compania.Direccion.Email;
                        if (CorreoSend.MandaCorreo(objCorreo, mail, asn, null, ms))
                        {
                            fac.EnvioC = EnvioCorreo.Enviado;
                            Save = true;
                        }
                        if (ms != null)
                            ms.Dispose();
                    }
                }
            }
        }

        string ftoCan = "AcuseCancelacion";
        private void simpleActionImprmAcs_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            if (View != null)
            {
                ImprFto(ftoCan, true, true);
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
                        EnviaNotifCan(fac);

                    // Imprime acuse
                    if (Empresa != null && Empresa.ConCfdi && !string.IsNullOrEmpty(fac.SelloSatCan))
                        ImprFto(ftoCan, true, true);
                }
            }
        }

        private void EnviaNotifCan(DocumentoSalida sal)
        {
            if (View != null && View.ObjectSpace != null && sal != null)
            {
                CorreoBase objCorreo = View.ObjectSpace.CreateObject<CorreoBase>();

                Correo objSend = View.ObjectSpace.FindObject<Correo>(null);
                if (objSend != null)
                {
                    objCorreo.Cuenta = objSend.Cuenta;
                    objCorreo.Identificdr = objSend.Identificdr;
                    objCorreo.Passw = objSend.Passw;
                    objCorreo.Puerto = objSend.Puerto;
                    objCorreo.SegurdSSL = objSend.SegurdSSL;
                    objCorreo.ServidorSMTP = objSend.ServidorSMTP;
                    objCorreo.Usuario = objSend.Usuario;
                }



                objCorreo.Asunto = string.Format("El documento {0}, se ha cancelado", sal.Secuencial);
                objCorreo.Mensaje1 = "Notificación de cancelación";

                ProveedorCliente pc = sal.Causante;
                Empresa emp = sal.Session.FindObject<Empresa>(null);

                if (pc != null && pc.Compania != null && pc.Compania.Direccion != null)
                    pc.Compania.Direccion.Reload();


                if (emp != null && emp.ConCfdi)
                {
                    objCorreo.Mensaje1 = "Estimado, le notificamos por este medio que el CFDI (Comprobante Fiscal Digital por Internet) con las siguientes características ha sido cancelado:<br>";
                }
                else
                {
                    objCorreo.Mensaje1 = "Estimado, le notificamos por este medio que el Comprobante con las siguientes características ha sido cancelado:<br>";
                }

                //objCorreo.Mensaje2 = string.Format("{0}\n{1}\n{2}", "<table>", "<tbody>", "<tr>");
                //objCorreo.Mensaje2 = string.Format("<tr><td style=width:60px>RFC Emisor:</td><td style=width:60px>{0}</td></tr>", emp.Compania.Rfc);
                objCorreo.Mensaje2 = string.Format("RFC Emisor: {0}", emp.Compania.Rfc);
                objCorreo.Mensaje2 = string.Format("{0}<br>Razón Social Emisor:    <b>{1}</b>", objCorreo.Mensaje2, emp.Compania.Nombre);
                objCorreo.Mensaje2 = string.Format("{0}<br>RFC Receptor:             <b>{1}</b>", objCorreo.Mensaje2, sal.Cliente.Compania.Rfc);
                objCorreo.Mensaje2 = string.Format("{0}<br>Razón Social Receptor: <b>{1}</b>", objCorreo.Mensaje2, sal.Cliente.Compania.Nombre);
                objCorreo.Mensaje2 = string.Format("{0}<br>Serie y Folio:              <b>{1}</b>", objCorreo.Mensaje2, sal.Secuencial);
                objCorreo.Mensaje2 = string.Format("{0}<br>Fecha de Emisión:        {1}", objCorreo.Mensaje2, String.Format("<b>{0:yyyy-MM-ddTHH:mm:ss}</b>", sal.FechaDoc));
                objCorreo.Mensaje2 = string.Format("{0}<br>Tipo de Comprobante:   <b>{1}({2})</b>", objCorreo.Mensaje2, sal.Tipo != DocumentoTipo.DevolucionVenta ? "ingreso" : "egreso", sal.Tipo);
                objCorreo.Mensaje2 = string.Format("{0}<br>Monto Total:                <b>{1}, {2}</b>", objCorreo.Mensaje2, sal.Total.ToString("c2"), sal.Moneda.Clave);
                objCorreo.Mensaje2 = htmlCan(emp.Compania.Rfc, emp.Compania.Nombre, sal.Cliente.Compania.Rfc,
                    sal.Cliente.Compania.Nombre, sal.Secuencial, String.Format("{0:yyyy-MM-ddTHH:mm:ss}", sal.FechaDoc),
                    string.Format("{0}({1})", sal.Tipo != DocumentoTipo.DevolucionVenta ? "ingreso" : "egreso", sal.Tipo),
                    string.Format("{0}, {1}", sal.Total.ToString("c2"), sal.Moneda.Clave));

                MemoryStream ms = null;
                if (emp.Logo != null)
                {
                    JpegStorageConverter jpeg = new JpegStorageConverter();

                    Byte[] arr = jpeg.ConvertToStorageType(emp.Logo) as Byte[];
                    ms = new MemoryStream(arr);
                }

                CorreoSend.MandaCorreo(objCorreo, pc.Compania.Direccion.Email, objCorreo.Asunto, objCorreo.Identificdr, ms);
                objCorreo.Mensaje1 =
                objCorreo.Mensaje2 = string.Empty;

                if (ms != null)
                    ms.Dispose();
            }
        }

        private string htmlCan(string rfcemi, string emi, string rfcrec, string rec, string sf, string fch, string tp, string tt)
        {
            return string.Format("<table>" +
            /*
        "<tbody>"+
            "<tr>"+
                "<td colspan='2'>"+
                    "<span style='font-size:medium; font-family:Arial'>"+
"Estimado, le notificamos por este medio que el CFDI (Comprobante Fiscal Digital por Internet) con las siguientes características ha sido cancelado:"+
                          "</span>"+
            "</td>"+
            "</tr>"+
        "</tbody>"+*/
            "<tr>" +
                    "<td width='40%'><span style='font-size: small; font-family: Arial'>RFC Emisor:</span></td>" +
                    "<td width='60%'><span style='font-weight: bold; font-size: small; font-family: Arial'>{0}</span></td>" +
"</tr>" +
            "<tr>" +
                "<td><span style='font-size: small; font-family: Arial'>Razón Social Emisor:</span></td>" +
                "<td><span style='font-weight: bold; font-size: small; font-family: Arial'>{1}</span> </td>" +
            "</tr>" +
            "<tr>" +
                "<td><span style='font-size: small; font-family: Arial'>RFC Receptor: </span> </td>" +
                "<td><span style='font-weight: bold; font-size: small; font-family: Arial'>{2}</span> </td>" +
            "</tr>" +
            "<tr>" +
                "<td><span style='font-size: small; font-family: Arial'>Razón Social Receptor: </span> </td>" +
                "<td><span style='font-weight: bold; font-size: small; font-family: Arial'>{3}</span> </td>" +
            "</tr>" +
            "<tr>" +
                "<td><span style='font-size: small; font-family: Arial'>Serie y Folio: </span> </td>" +
                "<td><span style='font-weight: bold; font-size: small; font-family: Arial'>{4}</span> </td>" +
            "</tr>" +
            "<tr>" +
                "<td><span style='font-size: small; font-family: Arial'>Fecha de Emisión: </span> </td>" +
                "<td><span style='font-weight: bold; font-size: small; font-family: Arial'>{5}</span> </td>" +
            "</tr>" +
            "<tr>" +
                "<td><span style='font-size: small; font-family: Arial'>Tipo de Comprobante: </span> </td>" +
                "<td><span style='font-weight: bold; font-size: small; font-family: Arial'>{6}</span> </td>" +
            "</tr>" +
            "<tr>" +
                "<td><span style='font-size: small; font-family: Arial'>Monto Total: </span></td>" +
                "<td><span style='font-weight: bold; font-size: small; font-family: Arial'>{7}</span> </td>" +
            "</tr>" +
        "</table>", rfcemi, emi, rfcrec, rec, sf, fch, tp, tt);

        }

        private void simpleActionCreaPdf_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            CreaPdfImprime(false, true);
        }

        private void simpleActionCrCxc_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            CreaCxc();
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

        private string key = "d12573bh";
        private void popupWindowShowActionDscrgMsv_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            /*
            IObjectSpace objectSpace = Application.CreateObjectSpace();
            DescargaMasiva newObj;

            newObj = objectSpace.FindObject<DescargaMasiva>(null);
            if (newObj == null)
                newObj = objectSpace.CreateObject<DescargaMasiva>();

            newObj.RfcEmsr = Empresa.Compania.Rfc;
            newObj.RfcRcptr = Empresa.Compania.Rfc;
            newObj.RtDscrg = Prms.RutaPdfVnts;

            string rfcaux = new SymmCrypto(SymmCrypto.SymmProvEnum.DES).
                Decrypting(string.IsNullOrEmpty(Empresa.Contra) ? "LICENCIA" : Empresa.Contra, key);
            string[] toks = rfcaux.Split('|');

            if (toks[0] != Empresa.Compania.Rfc)
            {
                newObj.RfcEmsr = 
                newObj.RfcRcptr = "AAA010101AAA";
            }
            e.View = Application.CreateDetailView(objectSpace, "DescargaMasiva_DetailView", true, newObj);*/
        }

        private void popupWindowShowActionDscrgMsv_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            /*
            DescargaMasiva obj = e.PopupWindowViewCurrentObject as DescargaMasiva;

            if (obj != null)
            {
                int tntos = 0, mxTntos = 10;
                string startPath = Path.Combine(obj.RtDscrg, "Paquetes");
                if (!Directory.Exists(startPath))
                    Directory.CreateDirectory(startPath);

                string fi = obj.FchIncl.ToString("yyyy-MM-dd");
                string ff = obj.FcnFnl.ToString("yyyy-MM-dd");
                do
                {
                    if (DescargaMasivaSAT.Prueba(obj.ArchvPfx.FullName, obj.Cntrs,
                        obj.RfcEmsr, obj.RfcRcptr, fi, ff, obj.Slctd, obj.Emtds, obj.Rcbds, startPath, obj.Mtdt))
                    {
                        obj.Slctd = string.Empty;
                    https://docs.microsoft.com/en-us/dotnet/api/system.io.compression.zipfile.extracttodirectory?view=netframework-4.8

                        string zipPath = Path.Combine(startPath, DescargaMasivaSAT.idPaquete + ".gzip");
                        string extractPath = Path.Combine(obj.RtDscrg, "Extract");

                        ZipFile.ExtractToDirectory(zipPath, extractPath);
                    }
                    else
                    {
                        if ((EEstadoSolicitud)DescargaMasivaSAT.estadoSolicitud == EEstadoSolicitud.EnProceso)
                            obj.Slctd = DescargaMasivaSAT.idSolicitud;
                    }
                    obj.EstdSlctd = (EEstadoSolicitud)DescargaMasivaSAT.estadoSolicitud;
                
                    if (obj.EstdSlctd == EEstadoSolicitud.Aceptada
                        || obj.EstdSlctd == EEstadoSolicitud.EnProceso)
                        System.Threading.Thread.Sleep(5000);

                } while ((obj.EstdSlctd == EEstadoSolicitud.Aceptada 
                || obj.EstdSlctd == EEstadoSolicitud.EnProceso) && tntos++ < mxTntos) ;
                
                View.ObjectSpace.CommitChanges();
            }*/
        }
    }
}
