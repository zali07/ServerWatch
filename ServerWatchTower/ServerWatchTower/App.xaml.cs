using Cosys.SilverLib.Core;
using Cosys.SilverLib.Model;
using Cosys.SilverLib.Shell;
using Cosys.SilverLib.Shell.Model;
using System;
using System.Collections.Generic;
using System.Windows.Media.Imaging;

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
            this.ApplicationTitle = ServerWatchTower.Properties.Resources.TitleServerWatchTower;
        }

        protected override void SetUpApplicationTheme()
        {
            base.SetUpApplicationTheme();

            Telerik.Windows.Controls.RadRibbonWindow.IsWindowsThemeEnabled = false;
        }

        /// <summary>
        /// Gets the core types necessary for the composition of the application.
        /// </summary>
        /// <param name="connectionProvider">The currently selected database connection provider.</param>
        /// <returns>The list of types which need to be put into the composition catalog of
        /// the application.</returns>
        protected override List<Type> GetCoreCompositionTypes(IConnectionProvider connectionProvider)
        {
            var types = base.GetCoreCompositionTypes(connectionProvider);

            //types.Add(typeof(ServerWatchTower.Data.StartPageDataService));
            //types.Add(typeof(ServerWatchTower.View.StartPageView));
            //types.Add(typeof(ServerWatchTower.View.StartPageViewModel));

            types.Add(typeof(Cosys.SilverLib.View.TelerikReports.TelerikReportHandler));

            return types;
        }

        //protected override IEnumerable<IModule> CreateModuleInstances()
        //{
        //    return new SingleItemEnumerator<IModule>(new PersonalFPModule());
        //}

        /// <summary>
        /// Authenticates the user of the application and returns his new session.
        /// </summary>
        /// <returns>Thew new session started for the user, or <c>null</c> when the
        /// application can be started without a user session as well.</returns>
        /// <exception cref="ActionCanceledException">The user has canceled logging in and
        /// the application should be shut down.</exception>
        protected override SessionInfo AuthenticateUser()
        {
            var authenticationManager = new AuthenticationManager();

            var sessionInfo = authenticationManager.LogOn();

            return sessionInfo ?? throw new ActionCanceledException();
        }

        protected override void InitializeMainWindow(MainWindow window)
        {
            var iconUri = new Uri("pack://application:,,,/cosys.ico");
            window.Icon = BitmapFrame.Create(iconUri);
        }
    }
}
