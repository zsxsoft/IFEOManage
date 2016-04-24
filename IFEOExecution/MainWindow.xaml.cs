using IFEOGlobal;
using System;
using System.Linq;
using System.Windows;
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
        private int RemainTime = Config.Data.Timeout;

        public MainWindow()
        {
            InitializeComponent();
            // Check is administrator
            if (Global.IsAdministrator())
            {
                Title += $"({(string)FindResource("Administrator")})";
            }
            ArgumentString = string.Join(" ", App.Arguments);
            TxtArgumentString.Text = ArgumentString; // Don't need data binging here.
            Timer = new DispatcherTimer
            {
                Interval = new TimeSpan(0, 0, 1)
            };
            Timer.Tick += (o, e) =>
            {
                RemainTime--;
                if (RemainTime == 0)
                {
                    Environment.Exit(0);
                }
                TxtTimerUpdate.Text = (string)FindResource("txtClosingTime") + " " + RemainTime;
            };
            Timer.Start();
        }

        private void btnOpen_Click(object sender, RoutedEventArgs e)
        {
            string ProcessName = App.Arguments[0];
            string ProcessArgument = string.Join(" ", App.Arguments.Skip(1));
            Launcher.CreateProcess(ProcessName, ProcessArgument);
            Environment.Exit(0);
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
