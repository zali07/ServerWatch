namespace ServerWatchTower.Agent.View
{
    using Cosys.SilverLib.Core;
    using Cosys.SilverLib.View;
    using System.ComponentModel.Composition;
    using System.Windows.Input;
    using Telerik.Windows.Controls;

    /// <summary>
    /// The view of the Server catalog.
    /// </summary>
    [Export("ServerWatchTower.Agent.ServerCatalogView"), PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class ServerCatalogView : SilverViewBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ServerCatalogView"/> class.
        /// </summary>
        public ServerCatalogView()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Method called when an item has been selected in one of the collections, and it should be brought into view
        /// on the user interface.
        /// </summary>
        /// <param name="sender">The sender of the event, which is the ViewModel instance.</param>
        /// <param name="e">The arguments of the event, with information about the collection and the
        /// selected item from it.</param>
        protected override void OnCollectionItemSelected(object sender, CollectionItemSelectedEventArgs e)
        {
            this.gridView.ScrollIntoView(e.DataItem);
        }

        /// <summary>
        /// Activates the Quick filter box when the user starts typing in the GridView.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The arguments of the event.</param>
        private void GridView_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var focusedElement = Keyboard.FocusedElement;

            if (focusedElement is Telerik.Windows.Controls.GridView.GridViewCell || focusedElement is RadGridView)
            {
                this.quickFilterBox.SelectAll();
                this.quickFilterBox.Focus();
            }
        }

        /// <summary>
        /// Activates the GridView when the user uses the Up and Down arrow keys in the Quick filter box.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The arguments of the event.</param>
        private void QuickFilterBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up || e.Key == Key.Down)
            {
                this.gridView.Focus();
            }
        }
    }
}
