using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace IFEOExecution
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private string ArgumentString {
            get; set;
        }
        private DispatcherTimer Timer = null;
        private int RemainTime = 5;

        public MainWindow()
        {
            InitializeComponent();
            // Check is administrator
            if (IFEOGlobal.Function.IsAdministrator())
            {
                this.Title += "(" + (string)FindResource("Administrator") + ")";
            }
            ArgumentString = string.Join(" ", App.Arguments);
            TxtArgumentString.Text = ArgumentString; // Don't need data binging here.
            Timer = new System.Windows.Threading.DispatcherTimer();
            Timer.Tick += new EventHandler(delegate
            {
                RemainTime--;
                if (RemainTime == 0)
                {
                    Environment.Exit(0);
                }
                TxtTimerUpdate.Text = (string)FindResource("txtClosingTime") + " " + RemainTime;
            });
            Timer.Interval = new TimeSpan(0, 0, 1);
            Timer.Start();
        }

        private void btnOpen_Click(object sender, RoutedEventArgs e)
        {
            string ProcessName = App.Arguments[0];
            string ProcessArgument = string.Join(" ", App.Arguments.Skip(1));
            IFEOGlobal.Launcher.CreateProcess(ProcessName, ProcessArgument);
            Environment.Exit(0);
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
