using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;

namespace IFEOManage
{

    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private IFEOInstance IFEO = IFEOInstance.Instance;
        public MainWindow()
        {
            InitializeComponent();
            IFEO.Load();
            listView.ItemsSource = IFEO.Items;


            Version ThisVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            VersionBlock.Text = string.Format("{0}.{1}.{2}.{3}", ThisVersion.Major, ThisVersion.Minor, ThisVersion.Build, ThisVersion.Revision);
        }

        private void mnuAdd_Click(object sender, RoutedEventArgs e)
        {
            Window WindowDetail = new Detail();
            WindowDetail.Show();
        }

        private void mnuRefresh_Click(object sender, RoutedEventArgs e)
        {
            IFEO.Load();
        }

        private void mnuModify_Click(object sender, RoutedEventArgs e)
        {
            if (listView.SelectedIndex == -1) return;
            List<IFEOItem> Items = listView.SelectedItems.Cast<IFEOItem>().ToList();
            Items.ForEach(delegate (IFEOItem Item)
            {
                Detail WindowDetail = new Detail();
                WindowDetail.Data.ID = IFEO.Items.IndexOf(Item);
                WindowDetail.Show();
            });
        }

        private void mnuDelete_Click(object sender, RoutedEventArgs e)
        {
            if (listView.SelectedIndex == -1) return;
            List<int> SelectedItems = new List<int>();
            List<IFEOItem> Items = listView.SelectedItems.Cast<IFEOItem>().ToList();

            MessageBoxResult Result = MessageBox.Show((string)FindResource("msgConfirm"), this.Title, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);

            if (Result == MessageBoxResult.Yes)
            {
                IFEO.Delete(Items);
            }

        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void AuthorHyperLink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start((string)FindResource("IFEOAuthorUri"));
        }

        private void GitHubHyperLink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(e.Uri.ToString());
        }

        private void TranslationHyperLink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start((string)FindResource("TranslationAuthorUri"));
        }

        private void mnuExport_Click(object sender, RoutedEventArgs e)
        {
            if (listView.SelectedIndex == -1) return;
            Microsoft.Win32.SaveFileDialog ofd = new Microsoft.Win32.SaveFileDialog();
            ofd.DefaultExt = ".reg";
            ofd.Filter = "Importing Registration Entries|*.reg";
            if (ofd.ShowDialog() != true) return;
            if (ofd.FileName == "") return;
            List<IFEOItem> Items = listView.SelectedItems.Cast<IFEOItem>().ToList();
            IFEO.Export(Items, ofd.FileName);
        }

        private void listView_KeyDown(object sender, KeyEventArgs e)
        {
            if ((Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)) && Keyboard.IsKeyDown(Key.A))
            {
                listView.SelectAll();
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            listView.Focus();
            listView_KeyDown(sender, e);
        }
    }
}
