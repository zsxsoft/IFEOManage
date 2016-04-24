using System.ComponentModel;
using System.Windows;
using IFEOGlobal;
using System.IO;

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
        public int ID
        {
            get
            {
                return _id;
            }
            set
            {
                if (_id == value) return;
                _id = value;
                if (IFEO.Items.Count > value && value != -1)
                {

                    IFEOItem Item = IFEO.Items[_id];
                    PEPath = Item.PEName;
                    DebuggerPath = Item.Debugger;
                    ManageByThis = Item.ManageByThis;
                    RunMethod = Item.RunMethod;
                    Remark = Item.Remark;
                }

                OnPropertyChanged(new PropertyChangedEventArgs("ID"));
                OnPropertyChanged(new PropertyChangedEventArgs("IsEditing"));
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

        /// <summary>
        /// Gets a value indicating whether this instance is editing.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is editing; otherwise, <c>false</c>.
        /// </value>
        private bool IsEditing
        {
            get
            {
                return !(ID == -1);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, e);
        }

    }

    /// <summary>
    /// Detail.xaml 的交互逻辑
    /// </summary>
    public partial class Detail : Window
    {
        private IFEOInstance IFEO = IFEOInstance.Instance;
        public DetailData Data { get; } = new DetailData();
        /// <summary>
        /// Initializes the data.
        /// </summary>
        public void InitializeData()
        {
            Data.ID = -1;
            Data.PEPath = "";
            Data.DebuggerPath = "";
            Data.Remark = "";
            Data.ManageByThis = true;
            Data.RunMethod = RunMethod.Close;
        }

        public Detail()
        {
            InitializeComponent();
            grid.DataContext = Data;
            InitializeData();
        }

        private void btnOpenPESelector_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog
            {
                DefaultExt = ".exe",
                Filter = "Execution File|*.exe"
            };
            if (ofd.ShowDialog() == true)
            {
                if (ofd.FileName != "")
                {
                    Data.PEPath = Path.GetFileName(ofd.FileName);
                }
            }

        }

        private void btnOpenDebuggerSelector_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog
            {
                DefaultExt = ".exe",
                Filter = "Execution File|*.exe"
            };
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
                Data.DebuggerPath = Global.IFEOExecution;
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            Data.PEPath = Data.PEPath.Trim();
            if (string.IsNullOrEmpty(Data.PEPath)) return;
            IFEOItem Item = new IFEOItem
            {
                ManageByThis = Data.ManageByThis,
                Debugger = Data.ManageByThis ? $@""" { Global.IFEOExecution } """ : Data.DebuggerPath,
                PEName = Data.PEPath,
                Remark = Data.Remark,
                RunMethod = Data.RunMethod
            };
            IFEO.Save(Item);
            InitializeData(); // Refresh it
        }
    }
}
