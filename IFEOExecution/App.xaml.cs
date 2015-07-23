using IFEOGlobal;
using Microsoft.Win32;
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
            Log.WriteLine("Started IFEOExecution" + ((Global.IsAdministrator()) ? "(Administrator)" : "") + " with :" + Environment.CommandLine);
            if (Arguments.Length == 0)
            {
                try {
                    Process.Start("IFEOManage.exe");
                }
                catch
                {
                    // Eat it
                }
                finally
                {
                    Environment.Exit(0);
                }
                
            }
            
            Uri BaseUri = new Uri(System.IO.Directory.GetCurrentDirectory() + "/");
            Uri ProgramName = new Uri(BaseUri, Arguments[0]);
            string FileName = Path.GetFileName(ProgramName.LocalPath);
            string FilePath = Uri.UnescapeDataString(ProgramName.AbsolutePath);
            if (FileName.ToLower().IndexOf(".") < 0)
            {
                FileName += ".exe";
                FilePath += ".exe";
            }

            Log.WriteLine("Get absolute path: " + FilePath) ;
            Log.WriteLine("FileName: " + FileName);
            if (!File.Exists(FilePath))
            {
                Log.WriteLine("Path not found.");
                //MessageBox.Show((string)FindResource("cfmNotFound"), "IFEOExecution", MessageBoxButton.OK, MessageBoxImage.Error);
                Environment.Exit(1);
            } else
            {
                Config.Load();
                
                Log.WriteLine("Opening " + Global.IFEORegPath + FileName);
                RegistryKey IFEOKey = Registry.LocalMachine.OpenSubKey(Global.IFEORegPath + FileName);
                object KeyValue = IFEOKey.GetValue("IFEOManage_RunMethod");
                bool Popop = false;
                if (KeyValue != null)
                {
                    if ((RunMethod)Enum.Parse(typeof(RunMethod), (string)KeyValue) == RunMethod.Popup)
                    {
                        Log.WriteLine("Open popup window.");
                        new MainWindow().Show();
                        Popop = true;
                    }
                }
                if (!Popop)
                {
                    Log.WriteLine("Exit.");
                    Environment.Exit(0);
                }

            }

            
            
        }
    }
}
