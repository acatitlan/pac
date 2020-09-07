using System;

using DevExpress.ExpressApp;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using DevExpress.ExpressApp.Reports;
using FCap.Module.BusinessObjects.Ventas;

namespace FCap.Module.Controllers
{
    using System.IO;
    using System.Net.Mime;

    using DevExpress.XtraReports.UI;
    using Utilerias;
    using Cap.Generales.BusinessObjects.Empresa;
    using System.Diagnostics;
    using Cap.Generales.BusinessObjects.Object;
    using Cap.Generales.BusinessObjects.General;
    using Cap.Clientes.BusinessObjects.Generales;
    using Cap.Ventas.BusinessObjects;
    using DevExpress.ExpressApp.ReportsV2;
    using Cap.Fe.BusinessObjects;
    using System.Collections.Generic;

    // For more information on Controllers and their life cycle, check out the http://documentation.devexpress.com/#Xaf/CustomDocument2621 and http://documentation.devexpress.com/#Xaf/CustomDocument3118 help articles.
    public partial class VCFactura : ViewController
    {
        // Use this to do something when a Controller is instantiated (do not execute heavy operations here!).
        public VCFactura()
        {
            InitializeComponent();
            RegisterActions(components);
            // For instance, you can specify activation conditions of a Controller or create its Actions (http://documentation.devexpress.com/#Xaf/CustomDocument2622).
            //TargetObjectType = typeof(DomainObject1);
            //TargetViewType = ViewType.DetailView;
            //TargetViewId = "DomainObject1_DetailView";
            //TargetViewNesting = Nesting.Root;
            //SimpleAction myAction = new SimpleAction(this, "MyActionId", DevExpress.Persistent.Base.PredefinedCategory.RecordEdit);

            TargetObjectType = typeof(DocumentoSalida);

            simpleActionCanclr.TargetObjectsCriteria = string.Format("Status != {0}", DocumentoStatus.Cancelado.GetHashCode());
            simpleActionCanclr.TargetViewType = ViewType.ListView;

            simpleActionSellr.TargetObjectsCriteria = string.Format("Status == {0}", DocumentoStatus.Alta.GetHashCode());

            simpleActionRprtRsmn.TargetViewType = ViewType.ListView;
            simpleActionRprtRsmn.SelectionDependencyType = SelectionDependencyType.RequireSingleObject;

            simpleActionImprmAcs.TargetObjectType = typeof(DocumentoSalida);
            simpleActionImprmAcs.TargetObjectsCriteria = string.Format("Status == {0}", DocumentoStatus.Cancelado.GetHashCode());

            simpleActionSavSellr.TargetObjectType = typeof(DocumentoSalida);
            simpleActionSavSellr.TargetViewType = ViewType.DetailView;
            simpleActionSavSellr.TargetObjectsCriteria = string.Format("Status == {0}", DocumentoStatus.Alta.GetHashCode());

            simpleActionCreaPdf.TargetObjectType = typeof(DocumentoSalida);
            // simpleActionCreaPdf.TargetObjectsCriteria = string.Format("Status == {0}", DocumentoStatus.Sellada.GetHashCode());
            simpleActionCreaPdf.SelectionDependencyType = SelectionDependencyType.RequireSingleObject;

            simpleActionImprmr.SelectionDependencyType = SelectionDependencyType.RequireSingleObject;

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
        }

        // Override to do something before Controllers are activated within the current Frame (their View property is not yet assigned).
        protected override void OnFrameAssigned()
        {
            base.OnFrameAssigned();
            //For instance, you can access another Controller via the Frame.GetController<AnotherControllerType>() method to customize it or subscribe to its events.
        }

        // Override to do something when a Controller is activated and its View is assigned.
        protected override void OnActivated()
        {
            base.OnActivated();
            //For instance, you can customize the current View and its editors (http://documentation.devexpress.com/#Xaf/CustomDocument2729) or manage the Controller's Actions visibility and availability (http://documentation.devexpress.com/#Xaf/CustomDocument2728).

            /*
            if (View is DetailView)
                View.ObjectSpace.Committing += ObjectSpace_Committing;*/

            // Hacemos invisible la Action Sellar y enviar por correo
            if (Empresa != null && !Empresa.ConCfdi)
            {
                simpleActionSellr.Category = "NonVisible";
                simpleActionSellr.TargetObjectsCriteria = string.Format("Status = {0}", DocumentoStatus.Ninguno.GetHashCode());

                simpleActionImprmAcs.TargetObjectsCriteria = string.Format("Status = {0}", DocumentoStatus.Ninguno.GetHashCode());

                simpleActionMail.TargetObjectsCriteria = string.Format("Status == {0}", DocumentoStatus.Alta.GetHashCode());
            }
            else
                simpleActionMail.TargetObjectsCriteria = string.Format("Status == {0}", DocumentoStatus.Sellada.GetHashCode());

            // Se puede hacer desde los Reportes
            simpleActionRprtRsmn.Active.SetItemValue("Visible", false);
            // Se agregó al Portafolio
            //simpleActionVerXML.Active.SetItemValue("Visible", false);
            simpleActionVerXML.ToolTip = "Tal Vez está en los Archivos de la Factura";
            //simpleActionViewPdf.Active.SetItemValue("Visible", false);
            simpleActionViewPdf.ToolTip = "Tal Vez está en los Archivos de la Factura";

            simpleActionVerXML.Active.SetItemValue("Visible", false);
            simpleActionViewPdf.Active.SetItemValue("Visible", false);
            simpleActionCreaPdf.Active.SetItemValue("Visible", false);
            simpleActionSavSellr.Active.SetItemValue("Visible", false);
            simpleActionSellr.Active.SetItemValue("Visible", false);
            simpleActionMail.Active.SetItemValue("Visible", false);
            simpleActionImprmr.Active.SetItemValue("Visible", false);
            simpleActionCanclr.Active.SetItemValue("Visible", false);
            simpleActionImprmAcs.Active.SetItemValue("Visible", false);
        }

        // Override to access the controls of a View for which the current Controller is intended.
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // For instance, refer to the http://documentation.devexpress.com/Xaf/CustomDocument3165.aspx help article to see how to access grid control properties.

            /*TIT Ago 2018, Ya no uso ConRet
            if (View != null)
            {
                if (View.ObjectSpace != null)
                {
                    Empresa emp = View.ObjectSpace.FindObject<Empresa>(null);

                    if (emp != null && !emp.ConCfdi && emp.ConRet)
                        View.Caption = View.Caption.Replace("Factura", "Recibo");
                }
            }*/
        }

        // Override to do something when a Controller is deactivated.
        protected override void OnDeactivated()
        {
            // For instance, you can unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }


        //bool Save = false;
        /*
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
                        NegocioAdmin.GrabaDocumento(fac);
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
        }*/

        /*
        private void QRCode2(DocumentoSalida fac)
        {
            / *TI Jun 2015 problemas con la Ñ
            QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();

            qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
            qrCodeEncoder.QRCodeScale = 4;
            qrCodeEncoder.QRCodeVersion = 8;
            qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.Q;* /

            string aux = fac.Total.ToString("F6");
            String data;
            if (fac != null && fac.Empresa != null && fac.Empresa.Compania != null && fac.Cliente != null && fac.Cliente.Compania != null)
                data = string.Format("?re={0}&rr={1}&tt={2}&id={3}", fac.Empresa.Compania.Rfc, fac.Cliente.Compania.Rfc, aux, fac.Uuid);
            else
                data = string.Empty;

            fac.CadenaCBB = data;
        }*/

        private void VCFactura_ViewControlsCreated(object sender, EventArgs e)
        {
            /*
            if (View != null)
            {
                DocumentoSalida fac = View.CurrentObject as DocumentoSalida;

                if (View.ObjectSpace != null && View.ObjectSpace.IsNewObject(fac))
                    NegocioAdmin.IniciaDocumento(fac);

                / *
                if (fac != null)
                {
                    fac.VentaItems.ListChanged += FacturaItems_ListChanged;
                }* /
            }*/
        }

        private void simpleActionCanclr_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            /*
            if (View != null && View.ObjectSpace != null)
            {
                DocumentoSalida fac = View.CurrentObject as DocumentoSalida;

                if (fac != null)
                {
                    NegocioAdmin.CancelaDocumento(fac);

                    Save = true;
                    View.ObjectSpace.CommitChanges();

                    // Mejor lo guardamos y después enviamos la notificación por correo Jun 2014
                    if (fac.EnvioC == EnvioCorreo.Enviado)
                        EnviaNotifCan(fac);

                    // Imprime acuse
                    if (Empresa != null && Empresa.ConCfdi 
                        && !string.IsNullOrEmpty(fac.SelloSatCan))
                        ImprFto(ftoCan, true, true);
                }
            }*/
        }

        /*
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
        }*/

        /*
        private string htmlCan(string rfcemi, string emi, string rfcrec, string rec, string sf, string fch, string tp, string tt)
        {
            return string.Format("<table>" +
            / *
        "<tbody>"+
            "<tr>"+
                "<td colspan='2'>"+
                    "<span style='font-size:medium; font-family:Arial'>"+
"Estimado, le notificamos por este medio que el CFDI (Comprobante Fiscal Digital por Internet) con las siguientes características ha sido cancelado:"+
                          "</span>"+
            "</td>"+
            "</tr>"+
        "</tbody>"+* /
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
        }*/

        /*
        void FacturaItems_ListChanged(object sender, System.ComponentModel.ListChangedEventArgs e)
        {
            / *
            if (View != null && View.ObjectSpace != null && !View.ObjectSpace.IsCommitting)
            {
                DocumentoSalida fac = View.CurrentObject as DocumentoSalida;

                NegocioAdmin.CalculaDocumento(fac);
            }* /
        }*/

        //bool InCommited = false;
        /*
        private void Sella()
        {
            if (!InCommited && View != null)
            {
                DocumentoSalida doc = View.CurrentObject as DocumentoSalida;

                if (doc != null)
                {
                    if (Empresa.ConCfdi)
                    {
                        if (string.IsNullOrEmpty(doc.SelloSat))
                        {
                            NegocioAdmin.Sella(doc, View.ObjectSpace);
                            doc.Status = DocumentoStatus.Sellada;

                            Save = true;
                            InCommited = true;
                            View.ObjectSpace.CommitChanges();
                            InCommited = false;
                        }
                    }
                    CreaPdfImprime(true, true);

                    if (Ventas.SendM && doc.EnvioC != EnvioCorreo.Enviado)
                    {
                        simpleActionMail_Execute(this, null);
                        InCommited = true;
                        View.ObjectSpace.CommitChanges();
                        InCommited = false;
                    }
                }
            }
        }*/

        /*
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
                    doc.Empresa = View.ObjectSpace.FindObject<Empresa>(null);

                    if (doc.Empresa != null && !doc.Empresa.ConCfdi)
                    {
                        / *TIT Ago 2018, sólo cfdi 3.3
                        if (doc.Empresa.ConRet)
                            ImprFto("FacturaHonor", imprime, pdf);
                        else* /
                            ImprFto("FacturaCBB", imprime, pdf);
                    }
                    else
                    {
                        string aux = string.Format("FacturaCFDI{0}", doc.Cliente != null ? doc.Cliente.Clave.Trim() : string.Empty);

                        if (doc.Session.FindObject<ReportDataV2>(new BinaryOperator("DisplayName", aux)) == null)
                            aux = "FacturaCFDI";
                        ImprFto(aux, imprime, pdf);
                    }
                }
            }
        }*/

        /*
        private string NamePdf(Documento doc)
        {
            return NamePdf(doc, doc.Status, "pdf");
        }*/

        /*
        private string NameXml(Documento doc)
        {
            return NamePdf(doc, doc.Status, "xml");
        }*/

        private string NamePdf(Documento doc, DocumentoStatus stat, string ext)
        {
            string aux = string.Empty;

            if (doc != null)
            {
                string p = Path.Combine(Ventas.VntCfdi.RutaPdfVnts, doc.Tipo.ToString());
                if (string.IsNullOrEmpty(((DocumentoSalida)doc).Mostrador) 
                    && !string.IsNullOrEmpty(Ventas.VntCfdi.RutaPdfVnts))
                    p = NegocioAdmin.CreaDirs(p, doc.FechaDoc);

                aux = Path.Combine(p, string.Format("{2}{0}-{1}.{3}", doc.Tipo,
                ((DocumentoSalida)doc).Clave.Trim(),
                (stat == DocumentoStatus.Cancelado && ext != "xml") ? "AC" : string.Empty, ext));
            }
            return aux;
        }

        /*
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
                    / *TIT
                    XPCollection dosyas = new XPCollection(fac.Session, typeof(DocumentoSalida), false);
                    XafReport report = null;* /
                    DocumentoSalida doc = fac;

                    IReportDataV2 reportData2 = objectSpace.FindObject<ReportDataV2>(
                        new BinaryOperator("DisplayName", format));

                    / *
                    if (doc != null)
                    {* /
                    doc.Empresa = uow.FindObject<Empresa>(null);
                    if ((!doc.Empresa.ConCfdi || doc.Status == DocumentoStatus.Alta) && doc.Cliente != null)
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

                        / *TIT
                        dosyas.Add(doc);
                        report2.DataSource = dosyas;
                        report2.FillDataSource();* /
                        report2.DataSource = list;

                        if (pdf)
                        {
                            string nameF = NamePdf(doc);

                            / *TIT
                            ReportsModuleV2 reportsModule = ReportsModuleV2.FindReportsModule(Application.Modules);
                            if (reportsModule != null && reportsModule.ReportsDataSourceHelper != null)
                            {
                                reportsModule.ReportsDataSourceHelper.SetupBeforePrint(report2);
                            }* /
                            if (string.IsNullOrEmpty(Ventas.RutaPdfVnts))
                            {
                                PortfolioFileData fd = new PortfolioFileData(doc.Session);
                                MemoryStream ms = new MemoryStream();
                                report2.ExportToPdf(ms);
                                ms.Seek(0, SeekOrigin.Begin);

                                fd.File = new FileData(doc.Session);
                                fd.File.LoadFromStream(Path.GetFileName(nameF), ms);
                                fd.DocumentType = DocumentType.Pdf;

                                doc.Portfolio.Add(fd);

                                Save = true;
                                InCommited = true;
                                View.ObjectSpace.CommitChanges();
                                InCommited = false;

                            }
                            else
                            {
                                / *tit
                                if (string.IsNullOrEmpty(doc.Mostrador))
                                {* /
                                    report2.ExportToPdf(nameF);
                                / *tit
                                }* /
                            }
                        }
                        if (imprime)
                        {
                            try
                            {
                                ReportsModuleV2.FindReportsModule(Application.Modules).ReportsDataSourceHelper.SetupBeforePrint(report2);
                                report2.ShowPreview();
                                / *TIT
                                BinaryOperator fil = new BinaryOperator("Oid", fac.Oid);

                                ReportServiceController controller = Frame.GetController<ReportServiceController>();
                                if (controller != null)
                                {
                                    controller.ShowPreview(reportContainerHandler, fil);
                                }* /
                            }
                            catch (Exception)
                            {
                            }
                        }
                    }
                    //dosyas.Dispose();
                    //}
                }
            }
        }*/

        private void simpleActionImprmr_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            /*
            CreaPdfImprime(true, false);*/
        }

        private void simpleActionMail_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            /*
            if (View != null)
            {
                foreach (DocumentoSalida doc in View.SelectedObjects)
                {
                    Correo(doc);
                }
            }*/
        }

        /*
        private void Correo(DocumentoSalida fac)
        {
            if (View != null)
            {
                if (fac != null)
                {
                    Correo objCorreo = View.ObjectSpace.FindObject<Correo>(null);

                    if (objCorreo != null)
                    {
                        string aux = NamePdf(fac);
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
                        if (!string.IsNullOrEmpty(Ventas.SendCopy))
                            mail = string.Format("{0}, {1}", fac.Cliente.Compania.Direccion.Email, Ventas.SendCopy);
                        else
                            mail = fac.Cliente.Compania.Direccion.Email;
                        if (CorreoSend.MandaCorreo(objCorreo, mail, asn, null, ms))
                        {
                            fac.EnvioC = EnvioCorreo.Enviado;
                            Save = true;

                            InCommited = true;
                            View.ObjectSpace.CommitChanges();
                            InCommited = false;
                        }
                        if (ms != null)
                            ms.Dispose();
                    }
                }
            }
        }*/

        private Empresa mEmpresa;
        private Empresa Empresa
        {
            get
            {
                if (mEmpresa == null)
                {
                    if (View != null && View.ObjectSpace != null)
                        mEmpresa = View.ObjectSpace.FindObject<Empresa>(null); // (CriteriaOperator.Parse("Clave = 'icpac'"));
                }
                return mEmpresa;
            }
        }

        private Ventas mVentas;
        private Ventas Ventas
        {
            get
            {
                if (mVentas == null)
                {
                    if (View != null && View.ObjectSpace != null)
                        mVentas = View.ObjectSpace.FindObject<Ventas>(null);
                }
                return mVentas;
            }
        }

        private void simpleActionRprtRsmn_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            /*
            string nameR = string.Empty;*/

            // Ago 2014
            /* Ahora se hace por parámetro en el filtro del reporte
            if (Empresa == null || (!Empresa.ConCfdi && Empresa.ConRet))
                nameR = "ResumenFacturasHonor";
            else*/
            /*TI Sep 2017 Lo estoy cambiando a la V2
            nameR = "ResumenFacturas";

            ReportData donneesEtat = (from reportData in new XPQuery<ReportData>(((XPObjectSpace)View.ObjectSpace).Session)
                                      where reportData.ReportName == nameR
                                      select reportData).FirstOrDefault();

            Frame.GetController<ReportServiceController>().ShowPreview(donneesEtat, null);*/
        }

        private void simpleActionSellr_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            /*
            // Verificamos que esté bien la configuración, en el sentido de que al menos está hecha
            // Aunque estó hace más lento el proceso, lo importante es que no me estén molestando !
            if (CfgCertificado() && CfgPagos())
                Sella();*/
        }

        /*
        private bool CfgCertificado()
        {
            bool cl = true;
            IObjectSpace os = Application.CreateObjectSpace();

            Certificado crt = os.FindObject<Certificado>(null);

            if ((crt.FlCrtfcdPm == null || string.IsNullOrEmpty(crt.FlCrtfcdPm.FileName)) || (crt.FlKyPm == null || string.IsNullOrEmpty(crt.FlKyPm.FileName)))
            {
                // IObjectSpace os = Application.CreateObjectSpace();
                //Find an existing object.
                //Contact obj = os.FindObject<Contact>(CriteriaOperator.Parse("FirstName=?", "My Contact"));
                //Or create a new object.
                // Contact obj = os.CreateObject<Contact>();
                // obj.FirstName = "My Contact";
                //Save the changes if necessary.
                //os.CommitChanges();
                //Configure how our View will be displayed (all parameters except for the CreatedView are optional).

                ShowViewParameters svp = new ShowViewParameters();
                DetailView dv = Application.CreateDetailView(os, crt);
                dv.ViewEditMode = DevExpress.ExpressApp.Editors.ViewEditMode.Edit;
                svp.CreatedView = dv;
                //svp.TargetWindow = TargetWindow.NewModalWindow;
                //svp.Context = TemplateContext.PopupWindow;
                //svp.CreateAllControllers = true;
                //You can pass custom Controllers for intercommunication or to provide a standard functionality (e.g., functionality of a dialog window).
                //DialogController dc = Application.CreateController<DialogController>();
                //svp.Controllers.Add(dc);
                // Show our View once the ShowViewParameters object is initialized.
                Application.ShowViewStrategy.ShowView(svp, new ShowViewSource(null, null));
                / *
                IObjectSpace objectSpace = Application.CreateObjectSpace(typeof(Certificado));
                e.ShowViewParameters.CreatedView = Application.CreateDetailView(objectSpace, typeof(Certificado), true); * /
                cl = false;
            }

            return cl;
        }*/

        /*
        private bool CfgPagos()
        {
            bool cl = true;

            DocumentoSalida doc = View.CurrentObject as DocumentoSalida;

            if (doc.LyndPg != null && doc.LyndPg.Clv == "PPD")
            {
                IObjectSpace os = Application.CreateObjectSpace();
                Ventas prm = os.FindObject<Ventas>(null);

                if (prm.CncptCbr == null)
                {
                    ShowViewParameters svp = new ShowViewParameters();
                    DetailView dv = Application.CreateDetailView(os, prm);
                    dv.ViewEditMode = DevExpress.ExpressApp.Editors.ViewEditMode.Edit;
                    svp.CreatedView = dv;
                    //svp.TargetWindow = TargetWindow.NewModalWindow;
                    //svp.Context = TemplateContext.PopupWindow;
                    //svp.CreateAllControllers = true;
                    //You can pass custom Controllers for intercommunication or to provide a standard functionality (e.g., functionality of a dialog window).
                    //DialogController dc = Application.CreateController<DialogController>();
                    //svp.Controllers.Add(dc);
                    // Show our View once the ShowViewParameters object is initialized.
                    Application.ShowViewStrategy.ShowView(svp, new ShowViewSource(null, null));
                    / *
                    IObjectSpace objectSpace = Application.CreateObjectSpace(typeof(Certificado));
                    e.ShowViewParameters.CreatedView = Application.CreateDetailView(objectSpace, typeof(Certificado), true);* /
                    cl = false;
                }
            }

            return cl;
        }*/

        /*
        string ftoCan = "AcuseCancelacion";*/

        private void simpleActionImprmAcs_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            /*
            if (View != null)
            {
                ImprFto(ftoCan, true, true);
            }*/
        }

        private void simpleActionSavSellr_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            /*
            if (View != null && View.ObjectSpace != null)
            {
                if (CfgPagos())
                {
                    View.ObjectSpace.CommitChanges();
                    if (CfgCertificado())
                    {
                        Sella();
                        View.Close();
                    }
                }
            }*/
        }

        private void simpleActionCreaPdf_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            /*
            CreaPdfImprime(false, true);*/
        }

        private void simpleActionViewPdf_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
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

        private void simpleActionVerXML_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
        }
    }
}
