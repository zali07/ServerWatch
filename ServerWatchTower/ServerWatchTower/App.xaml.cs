using Cosys.SilverLib.Core;
using Cosys.SilverLib.Model;
using System;
using System.Collections.Generic;

namespace ServerWatchTower
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="App"/> class.
        /// </summary>
        public App()
        {
            this.ApplicationRegistryKey = @"Software\Cosys\ServerWatchTower";
            this.ApplicationTitle = "ServerWatchTower";
        }

        protected override void SetUpApplicationTheme()
        {
            Telerik.Windows.Controls.RadRibbonWindow.IsWindowsThemeEnabled = false;
            Telerik.Windows.Controls.StyleManager.ApplicationTheme = new Telerik.Windows.Controls.Office_BlackTheme();
        }

        protected override List<Type> GetCoreCompositionTypes(IConnectionProvider connectionProvider)
        {
            return new List<Type>()
            {
                typeof(Cosys.SilverLib.View.TelerikReports.TelerikReportHandler)
            };
        }

        //protected override IEnumerable<IModule> CreateModuleInstances()
        //{
        //    return new SingleItemEnumerator<IModule>(new PersonalFPModule());
        //}

        protected override SessionInfo AuthenticateUser()
        {
            return new WindowsSessionInfo();
        }

        //protected override void InitializeMainWindow(MainWindow window)
        //{
        //    var iconUri = new Uri("pack://application:,,,/PersonalFP.Silver;component/money.ico");
        //    window.Icon = BitmapFrame.Create(iconUri);
        //}
    }
}
