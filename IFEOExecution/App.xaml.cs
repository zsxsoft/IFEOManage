using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace IFEOExecution
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        public static string[] Arguments;

        public static object Argument { get; internal set; }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            Arguments = e.Args;
            if (Arguments.Length == 0)
            {
                Process.Start("IFEOManage.exe");
                Environment.Exit(0);
            }

            Uri BaseUri = new Uri(System.IO.Directory.GetCurrentDirectory());
            Uri ProgramName = new Uri(BaseUri, Arguments[0]);
            if (!File.Exists(ProgramName.AbsolutePath))
            {
                MessageBox.Show((string)FindResource("cfmNotFound"), "IFEOExecution", MessageBoxButton.OK, MessageBoxImage.Error);
                Environment.Exit(1);
            } else
            {
                new MainWindow().Show();
            }

            
            
        }
    }
}
