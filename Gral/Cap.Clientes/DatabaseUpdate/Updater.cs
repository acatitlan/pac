using System;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Xpo;
using DevExpress.ExpressApp.Updating;

namespace Cap.Clientes.DatabaseUpdate
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
        }
        public override void UpdateDatabaseBeforeUpdateSchema()
        {
            base.UpdateDatabaseBeforeUpdateSchema();
            //if(CurrentDBVersion < new Version("1.1.0.0") && CurrentDBVersion > new Version("0.0.0.0")) {
            //    RenameColumn("DomainObject1Table", "OldColumnName", "NewColumnName");
            //}

            if (CurrentDBVersion <= new Version("1.0.6831.8210")
                        && CurrentDBVersion > new Version("0.0.0.0"))
            {
                if (((XPObjectSpace)ObjectSpace).Connection.ConnectionString.Contains("postgres"))
                {
                    ExecuteNonQueryCommand(
                        "alter table \"Clasificacion\" alter column \"Descripcion\" Type varchar(40)", true);
                }
                else
                {
                    ExecuteNonQueryCommand(
                        "alter table Clasificacion alter column Descripcion nvarchar(40)", false);
                }
            }

            if (CurrentDBVersion <= new Version("1.0.7612.25201")
                        && CurrentDBVersion > new Version("0.0.0.0"))
            {
                if (((XPObjectSpace)ObjectSpace).Connection.ConnectionString.Contains("postgres"))
                {
                    ExecuteNonQueryCommand("ALTER TABLE \"CatalogoCliente\" DROP CONSTRAINT \"FK_CatalogoCliente_Oid\"", false);
                    /*
                    ExecuteNonQueryCommand(
                        "drop table \"CatalogoCliente\"", true);*/
                }
                else
                {
                    ExecuteNonQueryCommand("ALTER TABLE CatalogoCliente DROP CONSTRAINT FK_CatalogoCliente_Oid", true);
                    // ExecuteNonQueryCommand("ALTER TABLE CatalogoCliente DROP CHECK CHK_Oid", false);
                    // "FK_CatalogoCliente_Oid", false);
                    /*ExecuteNonQueryCommand(
                        "drop table CatalogoCliente", false);*/
                }
            }
        }
    }
}
