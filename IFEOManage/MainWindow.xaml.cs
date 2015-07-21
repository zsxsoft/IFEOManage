using System;
using System.Collections.Generic;
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
            this.listView.ItemsSource = IFEO.Items;//为ListView绑定数据源  
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
            List<IFEOItem> Items = listView.SelectedItems.Cast<IFEOItem>().ToList();
            Items.ForEach(delegate(IFEOItem Item)
            {
                Detail WindowDetail = new Detail();
                WindowDetail.Data.ID = IFEO.Items.IndexOf(Item);
                WindowDetail.Show();
            });
        }

        private void mnuDelete_Click(object sender, RoutedEventArgs e)
        {
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
            System.Environment.Exit(0);
        }
    }
}
