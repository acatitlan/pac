using System;
using DevExpress.ExpressApp;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.Updating;
using System.Collections.Generic;
using Cap.Generales.BusinessObjects.Unidades;
using Cap.Generales.BusinessObjects.General;

namespace Cap.Generales.DatabaseUpdate
{
    // For more typical usage scenarios, be sure to check out http://documentation.devexpress.com/#Xaf/clsDevExpressExpressAppUpdatingModuleUpdatertopic
    public class Updater : ModuleUpdater
    {
        public Updater(IObjectSpace objectSpace, Version currentDBVersion) :
            base(objectSpace, currentDBVersion)
        {
        }
        public override void UpdateDatabaseAfterUpdateSchema()
        {
            base.UpdateDatabaseAfterUpdateSchema();
            //string name = "MyName";
            //DomainObject1 theObject = ObjectSpace.FindObject<DomainObject1>(CriteriaOperator.Parse("Name=?", name));
            //if(theObject == null) {
            //    theObject = ObjectSpace.CreateObject<DomainObject1>();
            //    theObject.Name = name;
            //}
            Datos();
        }
        public override void UpdateDatabaseBeforeUpdateSchema()
        {
            base.UpdateDatabaseBeforeUpdateSchema();
            //if(CurrentDBVersion < new Version("1.1.0.0") && CurrentDBVersion > new Version("0.0.0.0")) {
            //    RenameColumn("DomainObject1Table", "OldColumnName", "NewColumnName");
            //}

            if (CurrentDBVersion <= new Version("1.0.6542.36364")
                        && CurrentDBVersion > new Version("0.0.0.0"))
            {
                if (((DevExpress.ExpressApp.Xpo.XPObjectSpace)this.ObjectSpace).Connection.ConnectionString.Contains("postgres"))
                {
                    ExecuteNonQueryCommand(
                        "alter table \"MyFileData\" alter column \"FullName\" Type varchar(200)", true);
                }
                else
                {
                    ExecuteNonQueryCommand(
                        "alter table MyFileData alter column FullName nvarchar(200)", false);
                }
            }

            if (CurrentDBVersion <= new Version("1.0.7304.16110")
                && CurrentDBVersion > new Version("0.0.0.0"))
            {
                if (((DevExpress.ExpressApp.Xpo.XPObjectSpace)this.ObjectSpace).Connection.ConnectionString.Contains("postgres"))
                {
                    ExecuteNonQueryCommand(
                        "alter table \"Compania\" alter column \"Nombre\" Type varchar(180)", true);
                }
                else
                {
                    ExecuteNonQueryCommand(
                        "alter table Compania alter column Nombre nvarchar(180)", false);
                }
            }

            if (CurrentDBVersion <= new Version("1.0.7523.18794")
                && CurrentDBVersion > new Version("0.0.0.0"))
            {
                if (((DevExpress.ExpressApp.Xpo.XPObjectSpace)this.ObjectSpace).Connection.ConnectionString.Contains("postgres"))
                {
                    ExecuteNonQueryCommand(
                        "alter table \"Regimen\" alter column \"Dscrpcn\" Type varchar(100)", true);
                }
                else
                {
                    ExecuteNonQueryCommand(
                        "alter table Regimen alter column Dscrpcn nvarchar(100)", false);
                }
            }

            if (CurrentDBVersion <= new Version("1.0.7527.21705")
                && CurrentDBVersion > new Version("0.0.0.0"))
            {
                if (((DevExpress.ExpressApp.Xpo.XPObjectSpace)this.ObjectSpace).Connection.ConnectionString.Contains("postgres"))
                {
                    ExecuteNonQueryCommand(
                        "alter table \"Direccion\" alter column \"Numero\" Type varchar(45)", true);
                }
                else
                {
                    ExecuteNonQueryCommand(
                        "alter table Direccion alter column Numero nvarchar(45)", false);
                }
            }
        }
    


        private void Datos()
        {
            List<string> mones = new List<string>();

            // clave/ des/ |nombre|termina|idioma|monto
            mones.Add("EUR|Euro|||1|0");
            mones.Add("USD|Dólar estadounidense/US Dollar|Dollar/Dollars|USD|1|0");
            mones.Add("MXN|Peso Mexicano|PESO/PESOS|M.N.|0|1");
            /*
            TI Ago el SAT nos da un catálogo de Monedas !  Tal vez hay que dejarlo porque el
            otro es muy grande
            mones.Add("GTQ|Quetzal|QUETZAL/QUETZALES|Q|2|1");
            mones.Add("CLP|Peso Chileno|PESO/PESOS|CLP|2|1");
            mones.Add("COP|Peso Colombiano|PESO/PESOS|COP|2|1");
            mones.Add("ARS|Peso Argentino|PESO/PESOS|ARS|2|1");
            mones.Add("PEN|Nuevo Sol|NUEVO SOL/NUEVOS SOLES|S/.|2|1");*/

            foreach (string ob in mones)
            {
                Moneda mone;
                string[] arrtoks = ob.Split('|');

                mone = ObjectSpace.FindObject<Moneda>(CriteriaOperator.Parse(string.Format("Clave == '{0}'", arrtoks[0])));
                if (mone == null)
                {
                    mone = ObjectSpace.CreateObject<Moneda>();
                    mone.Clave = arrtoks[0];
                    mone.Descripcion = arrtoks[1];
                    mone.Nmbr = arrtoks[2];
                    mone.Terminacion = arrtoks[3];
                    mone.Idioma = arrtoks[4] == "0" ? EIdioma.Mexicano :
                        arrtoks[4] == "1" ? EIdioma.Ingles : EIdioma.Espanol;
                    mone.MontoTC = Convert.ToDecimal(arrtoks[5]);
                    mone.Sistema = mone.Clave == "MXN";
                    mone.Save();
                }
            }

            /*
            Empresa emp = ObjectSpace.FindObject<Empresa>(null); 

            if (emp == null)
            {
                emp = ObjectSpace.CreateObject<Empresa>();
                emp.Compania.Nombre = "EMPRESA INVALIDA";
                emp.Compania.Rfc = "AAA010101AAA";
                emp.Compania.Curp = "CURPEMPRESA";
                emp.Regimen = "Regimen de la empresa";
                emp.Compania.Direccion.Calle = "Calle Empresa";
                emp.Compania.Direccion.Numero = "54";
                emp.Compania.Direccion.Colonia = "Tepelmeme Villa de Morelos";
                emp.Compania.Direccion.Municipio = "Tepelmeme Villa de Morelos";
                emp.Compania.Direccion.CP = "69310";
                emp.Compania.Direccion.Estado = "Uaxaca";
                emp.Clave = "icpac";
                emp.Usuario = "pruebasWS";
                emp.Passw = "pruebasWS";
            }*/

            /*TIT Lo hacemos en el Factory
             Abr 19, si no lo creo al salir del detalle me pregunta 
             si deseo salvar, pero como usuario pienso: no he hecho nada, 
             por qué me pide guardar? */ 
            Correo corr = ObjectSpace.FindObject<Correo>(null);
            if (corr == null)
            {
                corr = ObjectSpace.CreateObject<Correo>();
                corr.Clave = "icpac";
                corr.ServidorSMTP = "smtp.gmail.com";
                corr.Puerto = "587";
                corr.SegurdSSL = true;
                corr.Cuenta = "@gmail.com";
                corr.Identificdr = "Empresa Inválida";
                corr.Asunto = "Envío de Comprobante: ";
                corr.Mensaje1 = "Estimado Cliente le envíamos su Comprobante:";
                corr.Pie3 = "Saludos";
                corr.Save();
            }

            ObjectSpace.CommitChanges();
            /*
            Ventas vta = ObjectSpace.FindObject<Ventas>(CriteriaOperator.Parse("Clave = 'icpac'"));
            if (vta == null)
            {
                vta = ObjectSpace.CreateObject<Ventas>();
                vta.Save();
            }*/

            /*
            bool altaS = false;
            Certificado cer = ObjectSpace.FindObject<Certificado>(CriteriaOperator.Parse("Clave = 'icpac'"));
            if (cer == null)
            {
                cer = ObjectSpace.CreateObject<Certificado>();
                cer.Clave = "icpac";
                cer.PasswCertif = "12345678a";
                cer.Save();

                altaS = true;
            }*/

            /*
            if (altaS)
            {
                Producto srvc = ObjectSpace.FindObject<Producto>(CriteriaOperator.Parse("Clave = 'SRVC'"));
                if (srvc == null)
                {
                    srvc = ObjectSpace.CreateObject<Producto>();
                    srvc.Clave = "SRVC";
                    srvc.Descripcion = "SERVICIO CON IMPUESTO";
                    srvc.Tipo = EProductoTipo.Servicio;
                    srvc.CapEnFact = true;
                    srvc.USalida =
                    srvc.UProducto = ObjectSpace.FindObject<Unidad>(CriteriaOperator.Parse("Clave = 'NA'"));
                    srvc.Esquema = ObjectSpace.FindObject<EsquemaImpuesto>(CriteriaOperator.Parse("Clave = '1'"));

                    srvc.Save();
                }

                srvc = ObjectSpace.FindObject<Producto>(CriteriaOperator.Parse("Clave = 'SRVC2'"));
                if (srvc == null)
                {
                    srvc = ObjectSpace.CreateObject<Producto>();
                    srvc.Clave = "SRVC2";
                    srvc.Descripcion = "SERVICIO SIN IMPUESTO";
                    srvc.Tipo = EProductoTipo.Servicio;
                    srvc.CapEnFact = true;
                    srvc.USalida =
                    srvc.UProducto = ObjectSpace.FindObject<Unidad>(CriteriaOperator.Parse("Clave = 'NA'"));
                    srvc.Esquema = ObjectSpace.FindObject<EsquemaImpuesto>(CriteriaOperator.Parse("Clave = '2'"));

                    srvc.Save();
                }
            }*/
        }
    }
}
