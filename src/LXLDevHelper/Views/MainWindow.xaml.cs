using System.Windows;
using System.Windows.Controls;
using ModernWpf;
using Prism.Ioc;
using Prism.Regions;

namespace LXLDevHelper.Views
{
    public partial class MainWindow
    {
        internal static IContainerExtension _container;
        internal static IRegionManager _regionManager;
        public MainWindow(IContainerExtension container, IRegionManager regionManager)
        {
            InitializeComponent();
            _container = container;
            _regionManager = regionManager;
            regionManager.RegisterViewWithRegion("ContentRegion", typeof(MainContent));
        }

        private delegate void MethodInvoker();
        private void DarkLight_Toggled(object sender, RoutedEventArgs e)//切换主题
        {
            Application.Current.Dispatcher.BeginInvoke((MethodInvoker)delegate
            {
                var tm = ThemeManager.Current;
                if (((ModernWpf.Controls.ToggleSwitch)sender).IsOn)
                { tm.ApplicationTheme = ApplicationTheme.Dark; }
                else { tm.ApplicationTheme = ApplicationTheme.Light; }
                ClearValue(ThemeManager.RequestedThemeProperty);
            });
            //ModernWpf.MessageBox.Show("test");
        }

        private void TopToggleSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            Topmost = ((ModernWpf.Controls.ToggleSwitch)sender).IsOn;
        }
    }
}
