using Cap.Compras.BusinessObjects;
using Cap.Fe.BusinessObjects;
using Cap.Generales.Utilerias;
using Cap.Inventarios.BusinessObjects;
using Cap.Personas.BusinessObjects;
using Cap.Proveedores.BusinessObjects.Proveedores;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.Xpo;
using FCE;
using MicroStore.Module.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MicroStore.Module.Utilerias
{
    public class Negocio : NegocioBase
    {
        public static void IniciaVenta(Venta obj, Parametros prm)
        {
            if (obj != null)
            {
                if (prm == null)
                {
                    prm = obj.Session.FindObject<Parametros>(null);
                    if (prm != null)
                        prm.Reload();
                }

                if (prm != null)
                {
                    if (prm.UltmVnt > 0)
                        obj.Fl = prm.UltmVnt.ToString();
                }
            }
        }

        public static void CancelaVenta(Venta obj, Parametros prm)
        {
            if (prm == null)
            {
                prm = obj.Session.FindObject<Parametros>(null);
                prm.Reload();
            }
            if (prm != null)
            {
                MovimientoI movs = null;
                ConceptoMI cncpt = obj.Session.FindObject<ConceptoMI>(new BinaryOperator("Oid", prm.CncptCnclrVnt.Oid));

                foreach (VentaItem it in obj.ItemsVenta)
                {
                    movs = new MovimientoI(obj.Session);

                    movs.Documento = $"VC{obj.Fl}";
                    movs.Cntdd = it.Cntdd;
                    movs.Cncpt = cncpt;
                    movs.Cst = it.Prdct.CostoUltimo;
                    movs.Fch = DateTime.Now;
                    movs.Lt = null;
                    movs.Prdct = it.Prdct;

                    if (it.Prdct != null && it.Prdct.Lotes)
                    {
                        movs.Lt = obj.Session.FindObject<Lote>(new BinaryOperator("Lt", it.Lt));
                    }

                    GrabaMovimiento(movs);
                }
                obj.Stts = EEstadoVenta.Cancelada;
            }
        }

        public static void GrabaVenta(Venta obj, Parametros prm)
        {
            if (prm == null)
            {
                prm = obj.Session.FindObject<Parametros>(null);
                prm.Reload();
            }
            if (prm != null)
            {
                if (prm.UltmVnt > 0 && apl.Log.Cadena.IsNumber(obj.Fl))
                    prm.UltmVnt = Convert.ToUInt16(Convert.ToUInt16(obj.Fl) + 1);
            }

            ConceptoMI sal = obj.Session.FindObject<ConceptoMI>(new BinaryOperator("Oid", prm.CncptVnt.Oid));
            
            CreaMovimiento(obj, sal, prm);
        }

        /* Jul, lo hicimos en el objeto Venta, no como en Factura, qué será mejor?
        public static void CalculaTotal(Venta vnt)
        {
            decimal mSubTotal = 0;
            decimal mDescTtl = 0;
            decimal mIvaTtl = 0;
            decimal mTtl = 0;

            foreach (VentaItem item in vnt.ItemsVenta)
            {
                mSubTotal += Convert.ToDecimal(item.Cntdd) * item.Prc; // .Importe;
                mDescTtl += 0; // item.TotalDescuento01;
                // item.MImpuesto04 = item.TotalImpuesto04;
                mIvaTtl += item.TotalImpuesto04;
            }
            mTtl += (mSubTotal - mDescTtl + mIvaTtl + doc.GEnvio);


            doc.SubTotal = mSubTotal;
            doc.DescuentoTotal = mDescTtl;
            doc.Impuesto04 = mIvaTtl;

            doc.Total = mTtl;
        }*/

        /// <summary>
        /// Crea y graba los movimientos
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="prm"></param>
        private static void CreaMovimiento(Venta obj, ConceptoMI cncptSld, Parametros prm)
        {
            if (cncptSld != null)
            {
                MovimientoI movs = null;

                foreach (VentaItem it in obj.ItemsVenta)
                {
                    movs = new MovimientoI(obj.Session);

                    movs.Cntdd = it.Cntdd;
                    movs.Cncpt = cncptSld;
                    movs.Cst = it.Prdct.CostoUltimo;
                    movs.Fch = DateTime.Now;
                    movs.Lt = null;
                    movs.Prdct = it.Prdct;
                    movs.Documento = obj.Fl;

                    if (!prm.VntSnExstnc)
                    {
                        if (it.Cntdd > it.Prdct.Existencia)
                            throw new Exception("No hay existencias suficientes");
                    }

                    if (it.Prdct != null && it.Prdct.Lotes)
                    {
                        Lote lt = obj.Session.FindObject<Lote>(new BinaryOperator("Lt", it.Lt));

                        if (lt != null)
                        {
                            movs.Lt = lt;
                        }
                    }

                    GrabaMovimiento(movs);
                }
            }
        }

        /*
         * Producto ultimo costo = mov.costo Debido a las entradas
         * Cuando haya movimientos de entrada
         * Producto costo promedio = 
         * 10 de a 2 pesos entonces costo promedio = 2 pesos
         * 12 de a 3 
         * promedio = 10 * 2 + 12 * 3  = 20 + 36 = 56 / 22 = 2.54
         * 5 * 2 + 36 = 46 / 17 = 2.70... Este es el que me gusta más.
         * 3 + 2 = 2.5
         * 
         * 15 * 2 + 5 * 3 = 30 + 15 = 45 / 20 = 2.25
         * 
         * 20 * 3 + 10 * 4 = 60 + 40 = 100 / 30 = 3.33
         * 20 * 2.25 + 10 * 4 = 85 / 30 = 2.83
         * 2 * 2.83 + 20 * 4.5 = 5.66 + 90 = 4.34 
         */
        public static void GrabaMovimiento(MovimientoI mov)
        {
            if (mov != null)
            {
                if (mov.Cncpt.Tipo == EConceptoTipoMI.Salida)
                {
                    mov.Prdct.Existencia -= mov.Cntdd;
                    mov.Prdct.FUltimaVenta = mov.Fch;

                    if (mov.Prdct.Lotes && mov.Lt != null)
                        mov.Lt.Rmnnt -= mov.Cntdd;
                }
                else
                {
                    // Costo promedio
                    double exist = mov.Prdct.Existencia;
                    decimal cstPrm = mov.Prdct.CostoPromedio;
                    decimal prom;

                    /*
                    if (Math.Abs(cstPrm) < 0.00001m)
                        cstPrm = mov.Prdct.CostoUltimo;*/

                    prom = Convert.ToDecimal(exist) * cstPrm
                        + Convert.ToDecimal(mov.Cntdd) * mov.Cst;

                    mov.Prdct.Existencia += mov.Cntdd;
                    mov.Prdct.FUltimaCompra = mov.Fch;
                    mov.Prdct.CostoPromedio = 
                        prom / Convert.ToDecimal(mov.Prdct.Existencia);

                    if (mov.Prdct.Lotes && mov.Lt != null && !mov.Lt.IsNewObject())
                        mov.Lt.Rmnnt += mov.Cntdd;

                }
                mov.Save();
            }
        }

        public static void IniciaCompra(Recepcion obj, Parametros prm)
        {
            if (obj != null)
            {
                if (prm == null)
                {
                    prm = obj.Session.FindObject<Parametros>(null);
                    prm.Reload();
                }

                if (prm != null)
                {
                    if (prm.UltmCmpr > 0)
                        obj.Clave /*.Fl*/ = prm.UltmCmpr.ToString();
                }
            }
        }

        public static void CancelaCompra(Recepcion obj, Parametros prm)
        {
            if (prm == null)
            {
                prm = obj.Session.FindObject<Parametros>(null);
                prm.Reload();
            }
            if (prm != null)
            {
                obj.FechaCan = DateTime.Today;

                MovimientoI movs;
                ConceptoMI cncpt = obj.Session.FindObject<ConceptoMI>
                    (new BinaryOperator("Oid", prm.CncptCnclrCmpr.Oid));

                foreach (RecepcionItem it in obj.RecepcionItems)
                {
                    movs = new MovimientoI(obj.Session);

                    movs.Documento = $"RC{obj.Clave}";
                    movs.Cntdd = Convert.ToSingle(it.Cantidad);
                    movs.Cncpt = cncpt;
                    movs.Cst = it.Costo;
                    movs.Fch = DateTime.Now;
                    movs.Lt = null;
                    movs.Prdct = it.Producto;
                    movs.Prvdr = obj.Proveedor;
                    movs.Unidad = it.Producto.UEntrada;

                    if (it.Producto != null && it.Producto.Lotes)
                    {
                        movs.Lt = obj.Session.FindObject<Lote>(new BinaryOperator("Lt", it.Lt));
                    }

                    GroupOperator fil = new GroupOperator();
                    fil.Operands.Add(new BinaryOperator("Prdct", it.Producto));
                    fil.Operands.Add(new BinaryOperator("Cncpt", prm.CncptCmpr));
                    fil.Operands.Add(new BinaryOperator("Documento", $"R{obj.Clave}"));
                    MovimientoI entrd = obj.Session.FindObject<MovimientoI>(fil);

                    if (entrd != null)
                    {
                        movs.Cst = entrd.CstPrdct;
                        movs.Prdct.CostoUltimo = entrd.CstPrdct;
                    }

                    GrabaMovimiento(movs);
                }
                obj.Status = Cap.Ventas.BusinessObjects.DocumentoStatus.Cancelado;                 
            }
        }

        public static void GrabaCompra(Recepcion obj, Parametros prm)
        {
            if (prm == null)
            {
                prm = obj.Session.FindObject<Parametros>(null);
            }

            if (prm != null)
            {
                prm.Reload();
                if (prm.UltmCmpr > 0 && apl.Log.Cadena.IsNumber(obj.Clave.Trim()))
                    prm.UltmCmpr = Convert.ToUInt16(Convert.ToUInt16(obj.Clave.Trim()) + 1);
            }

            CreaMovCmpr(obj, prm);
        }

        private static void CreaMovCmpr(Recepcion obj, Parametros prm)
        {
            /*
            if (prm == null)
            {
                prm = obj.Session.FindObject<Parametros>(null);
            }*/
            if (prm == null || prm.CncptCmpr == null)
                throw new Exception("Falta definir la Configuración Conceptos MI");

            if (prm != null)
            {
                /*
                prm.Reload();*/

                MovimientoI movs = null;
                ConceptoMI sal = obj.Session.FindObject<ConceptoMI>(new BinaryOperator("Oid", prm.CncptCmpr.Oid));

                if (obj.Proveedor.FUltMovimiento /*.FchUltmCmpr*/ < obj.FechaDoc /*.Fch*/)
                    obj.Proveedor.FUltMovimiento /*.FchUltmCmpr*/ = obj.FechaDoc /*.Fch*/;

                foreach (/*CompraItem*/ RecepcionItem it in obj.RecepcionItems /*.ItemsCompra*/)
                {
                    movs = new MovimientoI(obj.Session);

                    movs.Documento = $"R{obj.Clave}"; 
                    movs.Cntdd = Convert.ToSingle(it.Cantidad); //.Cntdd;
                    movs.Cncpt = sal;
                    movs.Fch = DateTime.Now;
                    movs.Lt = null;
                    movs.Prdct = it.Producto; //.Prdct;
                    movs.Prvdr = obj.Proveedor; // .Prvdr;
                    movs.Prdct.CostoUltimo = it.Costo; //.Cst;
                    movs.Unidad = it.Producto.USalida;
                    // Le asigna costo cuando se asigna Producto ! TIT Dic 19
                    movs.Cst = it.Costo;

                    if (it.Producto /*.Prdct*/ != null && it.Producto /*.Prdct*/.Lotes)
                    {
                        Lote lt = new Lote(obj.Session);

                        lt.Cntdd = Convert.ToSingle(it.Cantidad); // .Cntdd;
                        lt.FchCdcdd = it.FchCdcdd;
                        lt.Lt = it.Lt;
                        lt.Prdct = it.Producto; //.Prdct;
                        lt.Rmnnt = Convert.ToSingle(it.Cantidad); //.Cntdd;

                        lt.Save();
                        movs.Lt = lt;
                    }

                    GrabaMovimiento(movs);
                }
            }
        }

        public static void Inventario(InventarioFisico inve, Parametros prm)
        {
            if (prm == null)
            {
                prm = inve.Session.FindObject<Parametros>(null);
                prm.Reload();
            }
            if (prm != null)
            {
                /*float*/double dif = Math.Abs(inve.CntddSstm - inve.CntddFsc);

                if (!inve.Aplcd && dif > 0)
                {
                    MovimientoI movs = new MovimientoI(inve.Session);

                    movs.Documento = string.Format("IF{0}{1}{2}", DateTime.Today.Year,
                        DateTime.Today.Month, DateTime.Today.Day);
                    movs.Cntdd = dif;
                    if (inve.CntddSstm > inve.CntddFsc)
                        movs.Cncpt = inve.Session.FindObject<ConceptoMI>(new BinaryOperator("Oid", prm.CncptAjstInvtrS.Oid));
                    else
                        movs.Cncpt = inve.Session.FindObject<ConceptoMI>(new BinaryOperator("Oid", prm.CncptAjstInvtrE.Oid));

                    movs.Cst = inve.Prdct.CostoUltimo;
                    movs.Fch = DateTime.Now;
                    movs.Lt = inve.Session.FindObject<Lote>(new BinaryOperator("Lt", inve.Lt));
                    movs.Prdct = inve.Prdct;
                    movs.Prvdr = null;

                    if (movs.Prdct.Lotes && movs.Lt == null && !string.IsNullOrEmpty(inve.Lt))
                    {
                        Lote lt = new Lote(movs.Session);

                        lt.Cntdd = movs.Cntdd;
                        lt.FchCdcdd = inve.FchCdcdd;
                        lt.Lt = inve.Lt;
                        lt.Prdct = movs.Prdct;
                        lt.Rmnnt = movs.Cntdd;

                        lt.Save();
                        movs.Lt = lt;
                    }
                    GrabaMovimiento(movs);

                    inve.Aplcd = true;
                }
            }
        }


        public static void IniciaProveedor(Proveedor pc, Parametros prm)
        {
            if (pc != null)
            {
                if (prm == null)
                    prm = pc.Session.FindObject<Parametros>(null);

                if (prm != null && prm.UltPrvdr > 0)
                    pc.Clave = prm.UltPrvdr.ToString();
            }
        }

        public static void GrabaProveedor(Proveedor obj)
        {
            if (obj != null)
            {
                Parametros vta = obj.Session.FindObject<Parametros>(null);

                if (vta != null && vta.UltPrvdr > 0)
                {
                    string nextclv = string.Empty;

                    vta.Reload();
                    nextclv = vta.UltPrvdr.ToString();
                    vta.UltPrvdr++;

                    obj.Clave = nextclv;
                }
            }
        }

        public static void CalculaTotal(DocumenEnt doc)
        {
            decimal mSubTotal = 0;
            decimal mDescTtl = 0;
            decimal mIvaTtl = 0;
            decimal mTtl = 0;

            foreach (PartEnt item in doc.LasPartidas())
            {
                mSubTotal += item.Importe;
                mDescTtl += item.TotalDescuento01;
                // item.MImpuesto04 = item.TotalImpuesto04;
                mIvaTtl += item.TotalImpuesto04;
            }
            mTtl += (mSubTotal - mDescTtl + mIvaTtl + doc.GEnvio);


            doc.SubTotal = mSubTotal;
            doc.DescuentoTotal = mDescTtl;
            doc.Impuesto04 = mIvaTtl;

            doc.Total = mTtl;
        }

        public static void VaciaStructCert(FacturaE obj, IObjectSpace oS)
        {
            Cap.Fe.BusinessObjects.Certificado crt = oS.FindObject<Cap.Fe.BusinessObjects.Certificado>(null);

            obj.Cert.SerieCertif = crt.SerieCertif;
            if (crt.FileCertif != null)
                obj.Cert.FileCertif_FullName = crt.FileCertif.FullName;
            if (crt.FilePrivKy != null)
                obj.Cert.FilePrivKy_FullName = crt.FilePrivKy.FullName;
            obj.Cert.CertificadoCad = crt.CertificadoCad;
            obj.Cert.PasswCertif = crt.PasswCertif;

            crt.FlCrtfcdPm.SaveToStream(obj.Cert.CrtPm);
            crt.FlKyPm.SaveToStream(obj.Cert.KyPm);

            crt.FileCertif.SaveToStream(obj.Cert.Crtfcd);
            crt.FilePrivKy.SaveToStream(obj.Cert.Ky);
        }

        public static void IniciaPersona(Persona obj)
        {
            obj.Curp = "CRP";
            obj.Rfc = "AAA010101AAA";
        }


        public static void IniciaMovimientos(MovimientosI movs)
        {
            movs.Cliente = null;
            movs.Concepto = null;
            movs.Documento = string.Empty;
            movs.FchDcmnt = DateTime.Now;
            movs.Notas = string.Empty;
            movs.Proveedor = null;
            while (movs.MIItems.Count > 0)
            {
                movs.MIItems.Remove(movs.MIItems[0]);
            }
        }

        public static void GuardaMovimientos(MovimientosI movs)
        {
            if (movs != null)
            {
                foreach (MIItems itm in movs.MIItems)
                {
                    MovimientoI mov = new MovimientoI(movs.Session);

                    mov.Documento = movs.Documento;
                    mov.Cntdd = itm.Cantidad;
                    mov.Cncpt = movs.Concepto;
                    mov.Cst = itm.Costo;
                    mov.Fch = DateTime.Now;
                    mov.Lt = itm.Session.FindObject<Lote>(new BinaryOperator("Lt", itm.Lt));
                    mov.Prdct = itm.Producto;
                    // movs.Prvdr = null;

                    /*
                    if (movs.Prdct.Lotes && movs.Lt == null && !string.IsNullOrEmpty(inve.Lt))
                    {
                        Lote lt = new Lote(movs.Session);

                        lt.Cntdd = movs.Cntdd;
                        lt.FchCdcdd = inve.FchCdcdd;
                        lt.Lt = inve.Lt;
                        lt.Prdct = movs.Prdct;
                        lt.Rmnnt = movs.Cntdd;

                        lt.Save();
                        movs.Lt = lt;
                    }*/

                    GrabaMovimiento(mov);
                }
            }
        }


        public static string Acabarse(IObjectSpace objs)
        {
            BinaryOperator[] operands = new BinaryOperator[2];
            Certificado certificado;
            string msg = string.Empty;

            certificado = objs.FindObject<Certificado>(null);
            if (certificado != null)
            {
                TimeSpan resul;
                

                resul = certificado.FechaFin - DateTime.Today;
                if (resul.Days < 0)
                    msg = "El Certificado está Vencido !";
                else if (certificado.AvisarCer == ETiempoAntes.Dia && resul.Days <= 1)
                    msg = "El Certificado está por Vencerse!";
                else if (certificado.AvisarCer == ETiempoAntes.Semana && resul.Days <= 7)
                    msg = "El Certificado está por Vencerse!";
                else if (certificado.AvisarCer == ETiempoAntes.Quincena && resul.Days <= 14)
                    msg = "El Certificado está por Vencerse!";
                else if (certificado.AvisarCer == ETiempoAntes.Mes && resul.Days <= 30)
                    msg = "El Certificado está por Vencerse!";
            }
            return msg;
        }
    }
}
