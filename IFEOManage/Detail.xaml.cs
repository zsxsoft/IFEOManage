using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace IFEOManage
{
    public class DetailData : INotifyPropertyChanged
    {
        private IFEOInstance IFEO = IFEOInstance.Instance;
        private int _id;
        /// <summary>
        /// Gets or sets the identifier of the IFEO.List in this window.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int ID {
            get
            {
                return _id;
            }
            set
            {
                if (_id == value) return;

                if (IFEO.Items.Count > value)
                {
                    _id = value;
                    IFEOItem Item = IFEO.Items[_id];
                    this.PEPath = Item.PEName;
                    this.DebuggerPath = Item.Debugger;
                    this.ManageByThis = Item.ManageByThis;
                    this.RunMethod = Item.RunMethod;
                    this.Remark = Item.Remark;
                }
                
                OnPropertyChanged(new PropertyChangedEventArgs("ID"));
            }
        }

        private string _PEPath;
        /// <summary>
        /// Gets or sets the PE File's Path.
        /// </summary>
        /// <value>
        /// The PE file's path.
        /// </value>
        public string PEPath
        {
            get
            {
                return _PEPath;
            }
            set
            {
                if (_PEPath == value) return;
                _PEPath = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PEPath"));
            }
        }

        private RunMethod _RunMethod;
        /// <summary>
        /// Gets or sets the running method
        /// </summary>
        /// <value>
        /// The running method
        /// </value>
        public RunMethod RunMethod
        {
            get
            {
                return _RunMethod;
            }
            set
            {
                if (_RunMethod == value) return;
                _RunMethod = value;
                OnPropertyChanged(new PropertyChangedEventArgs("RunMethod"));
            }
        }

        private string _DebuggerPath;
        /// <summary>
        /// Gets or sets the debugger path.
        /// </summary>
        /// <value>
        /// The debugger path.
        /// </value>
        public string DebuggerPath
        {
            get
            {
                return _DebuggerPath;
            }
            set
            {
                if (_DebuggerPath == value) return;
                _DebuggerPath = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DebuggerPath"));
            }
        }

        private string _Remark;
        /// <summary>
        /// Gets or sets the remark
        /// </summary>
        /// <value>
        /// The remark
        /// </value>
        public string Remark
        {
            get
            {
                return _Remark;
            }
            set
            {
                if (_Remark == value) return;
                _Remark = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Remark"));
            }
        }

        private bool _ManageByThis;
        /// <summary>
        /// Gets or sets a value indicating whether [manage by this].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [manage by this]; otherwise, <c>false</c>.
        /// </value>
        public bool ManageByThis
        {
            get
            {
                return _ManageByThis;
            }
            set
            {
                if (_ManageByThis == value) return;
                _ManageByThis = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ManageByThis"));
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChangedEventHandler h = PropertyChanged;
            if (h != null)
                h(this, e);
        }
    }

    /// <summary>
    /// Detail.xaml 的交互逻辑
    /// </summary>
    public partial class Detail : Window
    {
        private IFEOInstance IFEO = IFEOInstance.Instance;
        public DetailData Data;
        /// <summary>
        /// Initializes the data.
        /// </summary>
        public void InitializeData()
        {
            Data.ID = 0;
            Data.PEPath = "";
            Data.DebuggerPath = "";
            Data.Remark = "";
            Data.ManageByThis = true;
            Data.RunMethod = RunMethod.Close;
        }

        public Detail()
        {
            Data = new DetailData();
            InitializeComponent();
            InitializeData();
            txtDebugger.DataContext = Data;
            txtPEName.DataContext = Data;
            txtRemark.DataContext = Data;
            chkManageByThis.DataContext = Data;
            RadioStartup1.DataContext = Data;
            RadioStartup2.DataContext = Data;
        }

        private void btnOpenPESelector_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog();
            ofd.DefaultExt = ".exe";
            ofd.Filter = "Execution File|*.exe";
            if (ofd.ShowDialog() == true)
            {
                if (ofd.FileName != "")
                {
                    Data.PEPath = ofd.FileName;
                }
            }

        }

        private void btnOpenDebuggerSelector_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog();
            ofd.DefaultExt = ".exe";
            ofd.Filter = "Execution File|*.exe";
            if (ofd.ShowDialog() == true)
            {
                if (ofd.FileName != "")
                {
                    Data.DebuggerPath = ofd.FileName;
                }
            }
        }

        private void chkManageByThis_Checked(object sender, RoutedEventArgs e)
        {
            if (Data.ManageByThis)
            {
                Data.DebuggerPath = IFEO.IFEOExecution;
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            Data.PEPath = Data.PEPath.Trim();
            if (Data.PEPath == "") return;
            IFEOItem Item = new IFEOItem();

            Item.ManageByThis = Data.ManageByThis;
            if (Data.ManageByThis)
            {
                Item.Debugger = "\"" + IFEO.IFEOExecution + "\"";
            } else
            {
                Item.Debugger = Data.DebuggerPath;
            }
            
            Item.PEName = Data.PEPath;
            Item.Remark = Data.Remark;
            Item.RunMethod = Data.RunMethod;

            IFEO.Save(Item);
            InitializeData(); // Refresh it
        }
    }
}
