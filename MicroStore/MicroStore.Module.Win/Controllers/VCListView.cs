using System;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Win.Editors;
using DevExpress.XtraGrid.Views.Grid;

namespace MicroStore.Module.Win.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class VCListView : ViewController
    {
        public VCListView()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
            TargetViewType = ViewType.ListView;
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
            if (GridView != null)
            {
                /*TI
                GridView.DataSourceChanged += new EventHandler(GridView_DataSourceChanged);*/
                GridView.OptionsView.ColumnAutoWidth = false;
            }
        }
        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }

        private void VCListView_ViewControlsCreated(object sender, EventArgs e)
        {
            /*
            ListView listView = View as ListView;
            if (listView != null && listView.Editor is GridListEditor)
            {
                GridListEditor gridListEditor = listView.Editor as GridListEditor;
                GridView gridView = gridListEditor.GridView;
                // (gridListEditor.Control as GridControl).Load += new EventHandler(VControler1_Load);
                gridView.OptionsView.ColumnAutoWidth = false; // <- los anchos de las columnas deben ser configurables
                gridView.ColumnPanelRowHeight = (gridView.Appearance.HeaderPanel.FontHeight + 4) * 2/ *renglonesEncabezadoColumnas* /;
            }*/

            if (GridView != null)
            {
                // GridView.OptionsView.ColumnAutoWidth = false; // <- los anchos de las columnas deben ser configurables
                //GridView.ColumnPanelRowHeight = (GridView.Appearance.HeaderPanel.FontHeight + 4) * 2/*renglonesEncabezadoColumnas*/;
                GridView.Appearance.HeaderPanel.FontSizeDelta = 4;
            }
        }

        protected GridView GridView
        {
            get
            {
                if ((View is ListView) && ((View as ListView).Editor is GridListEditor))
                    return ((View as ListView).Editor as GridListEditor).GridView;
                return null;
            }
        }
    }
}
