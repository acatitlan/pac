using System;
using DevExpress.ExpressApp;
using DevExpress.Data.Filtering;
using LCap.Module.Win.Editors;
using DevExpress.XtraEditors;
using DevExpress.ExpressApp.Win.Editors;
using Cap.Ventas.BusinessObjects;

namespace LCap.Module.Win.Controllers
{
    // For more typical usage scenarios, be sure to check out http://documentation.devexpress.com/#Xaf/clsDevExpressExpressAppViewControllertopic.
    public partial class VCFactura : ViewController
    {
        public VCFactura()
        {
            InitializeComponent();
            RegisterActions(components);
            // Target required Views (via the TargetXXX properties) and create their Actions.
            TargetObjectType = typeof(DocumentoSalida);
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.
            
            string aux = string.Empty;
            if (((DevExpress.ExpressApp.Win.WinWindow)(Application.MainWindow)).Form != null)
            {
                aux = ((DevExpress.ExpressApp.Win.WinWindow)(Application.MainWindow)).Form.Text;

                DocumentoSalida doc = View.CurrentObject as DocumentoSalida;
                if (doc != null && doc.Tipo == DocumentoTipo.Ninguno)
                {
                    if (aux.Contains("Facturas"))
                        doc.Tipo = DocumentoTipo.Factura;
                    else if (aux.Contains("Devoluciones"))
                        doc.Tipo = DocumentoTipo.DevolucionVenta;
                    else if (aux.Contains("Cargo"))
                        doc.Tipo = DocumentoTipo.NotaCargo;
                    else if (aux.Contains("Pedido"))
                        doc.Tipo = DocumentoTipo.Pedido;
                    else if (aux.Contains("Remisiones"))
                        doc.Tipo = DocumentoTipo.Remision;
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

        private void VCFactura_ViewControlsCreated(object sender, EventArgs e)
        {
            FormasP();

            DetailView dt = View as DetailView;

            if (dt != null)
            {
                StringPropertyEditor de = dt.FindItem("DocEnlace") as StringPropertyEditor;
                if (de != null && de.Control != null)
                    de.Control.Validated += Control_ValidatedDocEnl;
            }
        }

        private void FormasP()
        {
            DetailView dv = View as DetailView;

            if (dv != null)
            {
                CustomStringEditor csF = dv.FindItem("FrmPago2") as CustomStringEditor;

                if (csF != null && csF.Control != null)
                {
                    (csF.Control as ComboBoxEdit).Properties.Items.Clear();
                    /*
                    (csF.Control as ComboBoxEdit).Properties.Items.Add(FormaPago.Cheque.GetStringValue());
                    (csF.Control as ComboBoxEdit).Properties.Items.Add(FormaPago.Credito.GetStringValue());
                    (csF.Control as ComboBoxEdit).Properties.Items.Add(FormaPago.Debito.GetStringValue());
                    (csF.Control as ComboBoxEdit).Properties.Items.Add(FormaPago.Deposito.GetStringValue());
                    (csF.Control as ComboBoxEdit).Properties.Items.Add(FormaPago.Efectivo.GetStringValue());
                    (csF.Control as ComboBoxEdit).Properties.Items.Add(FormaPago.Indefinido.GetStringValue());
                    (csF.Control as ComboBoxEdit).Properties.Items.Add(FormaPago.Monedero.GetStringValue());
                    (csF.Control as ComboBoxEdit).Properties.Items.Add(FormaPago.Transferencia.GetStringValue());*/
                }
            }
        }

        void Control_ValidatedDocEnl(object sender, EventArgs e)
        {
            if (View != null)
            {
                DocumentoSalida fac = View.CurrentObject as DocumentoSalida;
                PartidaSalida pit;

                if (!string.IsNullOrEmpty(fac.DocEnlace))
                {
                    string aux = fac.DocEnlace;
                    string[] arrdocs = aux.Split(':');
                    // DocumentoSalida docOrig = null;
                    DocumenSal docOrig = null;

                    foreach (string doc in arrdocs)
                    {
                        string cve = DocumentoSalida.ClaveFto(doc.Substring(1));
                        string tp = doc.Substring(0, 1).ToUpper();
                        DocumentoTipo dt = tp == "C" ? DocumentoTipo.Cotizacion
                            : tp == "P" ? DocumentoTipo.Pedido : DocumentoTipo.Remision;
                        GroupOperator gp = new GroupOperator();
                        gp.Operands.Add(new BinaryOperator("Clave", cve));
                        gp.Operands.Add(new BinaryOperator("Tipo", dt));
                        // docOrig = fac.Session.FindObject<DocumentoSalida>(gp);
                        if (tp == "C")
                            docOrig = fac.Session.FindObject<Cotizacion>(gp);
                        else
                            docOrig = fac.Session.FindObject<DocumenSal>(gp);

                        if (fac != null && docOrig != null)
                        {
                            fac.Cliente = docOrig.Cliente;
                            // foreach (PartidaSalida cit in docOrig.LasPartidas())
                            foreach (PartSal cit in docOrig.LasPartidas())
                            {
                                if (cit.CantidadRemanente > 0)
                                {
                                    pit = ObjectSpace.CreateObject<PartidaSalida>();
                                    pit.Item = cit.Item;
                                    pit.Cantidad = cit.Cantidad;
                                    pit.Producto = cit.Producto;
                                    pit.MontoUnitario = cit.MontoUnitario;
                                    pit.MontoUnitario = cit.Precio;
                                    /* Nov 2020, Cómo resuelvo esto ?
                                    pit.Anterior = cit;*/

                                    fac.VentaItems.Add(pit);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
