using Cosys.SilverLib.Core;
using Cosys.SilverLib.Model;
using Cosys.SilverLib.Shell;
using Cosys.SilverLib.Shell.Model;
using ServerWatchTower.Agent;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Media.Imaging;

namespace ServerWatchTower
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        /// <summary>
        /// Entropy data used to access the secure connection strings of the application.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly byte[] entropy = new byte[]
        {
            0x08, 0x39, 0xA4, 0x34, 0xF4, 0xD7, 0x29, 0x51,0x2E, 0x7A, 0xA8, 0xF6, 0xE3, 0x3F, 0x37, 0xF0,
            0xED, 0xCA, 0x6C, 0x2D, 0xE9, 0xF8, 0x51, 0x39,0xFA, 0x9B, 0x00, 0x20, 0x38, 0x75, 0xCA, 0x9B,
            0xA9, 0xAC, 0x53, 0xE6, 0xE8, 0x1B, 0x64, 0x0E, 0x16, 0x78, 0xFC, 0x74, 0x2F, 0x4A, 0x3D, 0x3F,
            0x97, 0xFD, 0x01, 0x9B, 0x8F, 0xE2, 0xDB, 0xE1, 0xF9, 0x6E, 0x63, 0x10, 0x67, 0x3D, 0x86, 0x3A,
            0x71, 0x8A, 0x73, 0x04, 0xE5, 0x73, 0x1C, 0xED, 0xC9, 0x9E, 0xC1, 0x44, 0xEA, 0xAC, 0x8D, 0x27,
            0x50, 0x54, 0xA7, 0x7D, 0xA0, 0x78, 0x0E, 0x77, 0x72, 0x5B, 0x34, 0x7B, 0x86, 0x65, 0x14, 0x97,
            0xEC, 0xEA, 0x61, 0xCB, 0x32, 0xA2, 0xEF, 0xA6, 0x77, 0xD1, 0x4D, 0x72, 0x34, 0x9E, 0x96, 0x4E,
            0xD0, 0x88, 0xBB, 0x2D, 0x2F, 0xEA, 0x84, 0xEA, 0xAD, 0x7C, 0x8C, 0x16, 0x7B, 0xBE, 0x35, 0x9C,
            0x90, 0x9B, 0xE1, 0x3E, 0xB4, 0xAA, 0xB6, 0x63, 0xC2, 0x9F, 0xEE, 0x2B, 0x1D, 0xF5, 0x28, 0x37,
            0xCC, 0x97, 0x1B, 0xB7, 0x1B, 0x1E, 0x21, 0xB9, 0xF0, 0xA6, 0x11, 0x4A, 0x2A, 0x6E, 0xC5, 0x8D
        };

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

        protected override IEnumerable<IModule> CreateModuleInstances()
        {
            return new SingleItemEnumerator<IModule>(new AgentModule());
        }

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

        /// <summary>
        /// Gets the entropy data to be used for security purposes.
        /// </summary>
        /// <param name="offset">The offset starting from which the entropy data should be read.</param>
        /// <param name="length">The length of the needed entropy data.</param>
        /// <returns>The appropriate entropy data.</returns>
        protected override byte[] GetEntropy(int offset, int length)
        {
            byte[] result = new byte[length];

            Buffer.BlockCopy(this.entropy, offset, result, 0, length);

            return result;
        }
    }
}
