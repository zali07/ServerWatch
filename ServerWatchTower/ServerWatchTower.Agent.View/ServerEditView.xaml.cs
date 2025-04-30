namespace ServerWatchTower.Agent.View
{
    using Cosys.SilverLib.Core;
    using Cosys.SilverLib.View;
    using System.ComponentModel.Composition;
    using System.Windows.Input;
    using Telerik.Windows.Controls;

    /// <summary>
    /// The view of the Partner catalog.
    /// </summary>
    [Export("ServerWatchTower.Agent.ServerEditView"), PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class ServerEditView : SilverViewBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ServerEditView"/> class.
        /// </summary>
        public ServerEditView()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Method called when the control bound to a specific property should be focused on the user interface.
        /// </summary>
        /// <param name="sender">The sender of the event, which is the ViewModel instance.</param>
        /// <param name="e">The arguments of the event, with information about the property.</param>
        protected override void OnPropertySelected(object sender, PropertySelectedEventArgs e)
        {
            switch (e.PropertyName)
            {
                //case "PartnerTypeBox":
                //    this.PartnerTypeBox.Focus();
                //    break;
                //case "PartnerCodeBox":
                //    this.PartnerCodeBox.Focus();
                //    break;
                //case "CodeGestBox":
                //    this.CodeGestBox.Focus();
                //    break;
                //case "CodeCentBox":
                //    this.CodeCentBox.Focus();
                //    break;
                //case "ChkAutoReception":
                //    this.ChkAutoReception.Focus();
                //    break;
                //case "ChkAutoDeliveryTakeover":
                //    this.ChkAutoDeliveryTakeover.Focus();
                //    break;
                //case "ChkAutoProductTakeover":
                //    this.ChkAutoProductTakeover.Focus();
                //    break;
                //case "SectorComboBox":
                //    this.SectorComboBox.Focus();
                //    break;
                //case "IncludeMappedProductCheckBox":
                //    this.IncludeMappedProductCheckBox.Focus();
                //    break;
            }
        }

        /// <summary>
        /// Opens the drop-down of the <see cref="RadComboBox"/> when the user starts to edit its content.
        /// </summary>
        /// <param name="sender">The sender of the event, which is supposed to be a <see cref="RadComboBox"/>.</param>
        /// <param name="e">The arguments of the event.</param>
        private void ComboBoxPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (sender is RadComboBox comboBox && !comboBox.IsDropDownOpen)
            {
                comboBox.IsDropDownOpen = true;
            }
        }
    }
}
