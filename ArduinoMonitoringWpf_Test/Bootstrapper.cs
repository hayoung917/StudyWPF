using ArduinoMonitoringWpf_Test.ViewModels;
using Caliburn.Micro;
using System.Windows;

namespace ArduinoMonitoringWpf_Test
{
    public class Bootstrapper : BootstrapperBase
    {
        public Bootstrapper()
        {
            Initialize();
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<MainViewModel>();
        }
    }
}
