using DevExpress.Xpf.Core;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows;

namespace OCTiS.Knx.HomeAutomationConfigurator
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static SplashScreenManager SplashScreenManager { get; private set; }
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            SplashScreenManager = SplashScreenManager.CreateFluent();
            SplashScreenManager.ViewModel.Copyright = SplashScreenManager.ViewModel.Copyright.Replace("Company Name", "OCTiS GmbH");
            SplashScreenManager.ViewModel.Logo = null;
            //// Show a splashscreen.
            SplashScreenManager.ShowOnStartup();
        }
    }
}
