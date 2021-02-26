using Cap.Bancos.BusinessObjects;
using Cap.Fe.BusinessObjects;
using Cap.Generales.BusinessObjects.Empresa;
using Cap.Generales.BusinessObjects.General;
using Cap.Inventarios.BusinessObjects;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;

namespace Hospital.Module.Controllers
{
    public static class SingletonFactory
    {
        public static object GetSingletonInstance(IObjectSpace objectSpace, ITypeInfo typeInfo)
        {
            object obj = null;


            if (typeInfo.Name == typeof(Empresa).Name)
            {
                obj = objectSpace.FindObject<Empresa>(null); 
                if (obj == null)
                {
                    obj = objectSpace.CreateObject(typeof(Empresa));
                }
            }
            else if (typeInfo.Name == typeof(HCap.Module.BusinessObjects.Hospital.Hospital).Name)
            {
                obj = objectSpace.FindObject<HCap.Module.BusinessObjects.Hospital.Hospital>(null);
                if (obj == null)
                {
                    obj = objectSpace.CreateObject(typeof(HCap.Module.BusinessObjects.Hospital.Hospital));
                }
            }
            else if (typeInfo.Name == typeof(Correo).Name)
            {
                obj = objectSpace.FindObject<Correo>(null);
                if (obj == null)
                    obj = objectSpace.CreateObject(typeof(Correo));
            }
            else if (typeInfo.Name == typeof(Certificado).Name)
            {
                obj = objectSpace.FindObject<Certificado>(null);
                if (obj == null)
                    obj = objectSpace.CreateObject(typeof(Certificado));
            }


            else if ((typeInfo.Name == typeof(Transferencia).Name))
            {
                obj = objectSpace.FindObject<Transferencia>(null);
                if (obj == null)
                    obj = objectSpace.CreateObject<Transferencia>();
            }
            else if ((typeInfo.Name == typeof(Presupuesto).Name))
            {
                obj = objectSpace.FindObject<Presupuesto>(null);
                if (obj == null)
                    obj = objectSpace.CreateObject<Presupuesto>();
            }
            else if (typeInfo.Name == typeof(MovimientosI).Name)
            {
                obj = objectSpace.FindObject<MovimientosI>(null);
                if (obj == null)
                    obj = objectSpace.CreateObject(typeof(MovimientosI));
            }

            return obj;
        }
    }
}
