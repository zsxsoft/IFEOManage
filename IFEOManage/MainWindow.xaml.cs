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

        }

        private void mnuDelete_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
