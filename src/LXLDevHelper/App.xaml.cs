using System;
using System.Diagnostics;
using System.Windows;

namespace LXLDevHelper
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            //try
            //{
            //    var args = Environment.GetCommandLineArgs();
            //    if (args.Length > 0)
            //    {
            //        if (args[0] == "build")
            //        {
            //            var p = Process.Start(new ProcessStartInfo() { FileName = "cmd.exe", UseShellExecute = false });
            //            p.WaitForExit();
            //            return;
            //        }
            //    }
            //}
            //catch (Exception ex) { System.Windows.MessageBox.Show(ex.ToString()); }
            base.OnStartup(e);
            var boot = new Bootstrapper();
            boot.Run();
        }
    }
}
