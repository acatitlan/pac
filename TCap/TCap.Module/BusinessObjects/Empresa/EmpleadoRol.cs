using System;
using System.Linq;
using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using System.Collections.Generic;
using DevExpress.ExpressApp.Security.Strategy;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;

namespace TCap.Module.BusinessObjects.Empresa
{
    [Obsolete("Regresé al PermissionPolicyRole, para acceder con Mobile")]
    [XafDisplayName("Roles")]
    [NavigationItem("Empresa")]
    [ImageName("BO_Role")]
    [DefaultClassOptions]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    // public class EmpleadoRol : SecuritySystemRoleBase
    public class EmpleadoRol : PermissionPolicyRoleBase, IPermissionPolicyRoleWithUsers, ICanInitializeRole
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public EmpleadoRol(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        //private string _PersistentProperty;
        //[XafDisplayName("My display name"), ToolTip("My hint message")]
        //[ModelDefault("EditMask", "(000)-00"), Index(0), VisibleInListView(false)]
        //[Persistent("DatabaseColumnName"), RuleRequiredField(DefaultContexts.Save)]
        //public string PersistentProperty {
        //    get { return _PersistentProperty; }
        //    set { SetPropertyValue("PersistentProperty", ref _PersistentProperty, value); }
        //}

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}

        
        // TIT Mar 2021
        [Obsolete("Ya no se hará así")]
        // [Association("EmpleadoProyecto-EmpleadoRoles")]
        public XPCollection<EmpleadoProyecto> Empleados
        {
            get
            {
                return GetCollection<EmpleadoProyecto>("Empleados");
            }
        }

        IEnumerable<IPermissionPolicyUser> IPermissionPolicyRoleWithUsers.Users
        {
            get { return Empleados.OfType<IPermissionPolicyUser>(); }
        }

        public bool AddUser(object user)
        {
            bool result = false;
            EmpleadoProyecto permissionPolicyUser = user as EmpleadoProyecto;
            if (permissionPolicyUser != null)
            {
                Empleados.Add(permissionPolicyUser);
                result = true;
            }
            return result;
        }
    }
}