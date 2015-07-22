using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows;

namespace IFEOManage
{
    public class IFEOItem
    {

        /// <summary>
        /// Gets or sets the PE file's name.
        /// </summary>
        /// <value>
        /// The PE file's name.
        /// </value>
        public string PEName { get; set; } = "";

        /// <summary>
        /// Gets or sets the remark.
        /// </summary>
        /// <value>
        /// The remark.
        /// </value>
        public string Remark { get; set; } = "";

        /// <summary>
        /// Gets or sets the debugger.
        /// </summary>
        /// <value>
        /// The debugger.
        /// </value>
        public string Debugger { get; set; } = "";
        /// <summary>
        /// Gets or sets the RegKey.
        /// </summary>
        /// <value>
        /// The RegKey.
        /// </value>
        public RegistryKey RegKey { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [manage by this].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [manage by this]; otherwise, <c>false</c>.
        /// </value>
        public bool ManageByThis { get; set; } = true;

        private string _IFEOPath;
        /// <summary>
        /// Gets the IFEOManage path.
        /// </summary>
        /// <value>
        /// The ifeo path.
        /// </value>
        public string IFEOPath {
            get
            {
                return _IFEOPath;
            }
            set
            {
                if (_IFEOPath == value) return;
                if (value == "")
                {
                    _IFEOPath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
                } else
                {
                    _IFEOPath = value;
                }
            }
        }
    }

    /// <summary>
    /// IFEOInstance
    /// </summary>
    public class IFEOInstance : INotifyPropertyChanged
    {
        private static IFEOInstance instance = new IFEOInstance();
        private IFEOInstance()
        {
            Console.WriteLine("fuck");
        }
        public static IFEOInstance Instance
        {
            get
            {
                return instance;
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChangedEventHandler h = PropertyChanged;
            if (h != null)
                h(this, e);
        }

        public static string IFEORunPath = System.Environment.CurrentDirectory;
        public string IFEOExecution = System.Environment.CurrentDirectory + "\\IEFOExecution.exe";
        private const string IFEORegPath = @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Image File Execution Options\";

        /// <summary>
        /// The items
        /// </summary>
        public ObservableCollection<IFEOItem> Items = new ObservableCollection<IFEOItem>();


        private object GetValue(RegistryKey RegKey, string Key)
        {
            object Value = RegKey.GetValue(Key);
            return Value == null ? "" : Value.ToString();
        }

        private void _FormatDebuggerString(IFEOItem Debugger)
        {
            if (Debugger.ManageByThis)
            {
                Debugger.ManageByThis = true;
                Debugger.Debugger = (string)Application.Current.FindResource("cfmManagedByThis");
            }
        }

        private RegistryKey IFEOKey = null;

        /// <summary>
        /// Loads IFEO items
        /// </summary>
        /// <returns>The state of load</returns>
        public bool Load()
        {
            IFEOKey = Registry.LocalMachine.OpenSubKey(IFEORegPath, true);
            IFEOItem TempIFEO;
            if (IFEOKey != null)
            {
                Items.Clear();
                foreach (string keyName in IFEOKey.GetSubKeyNames())
                {
                    RegistryKey SubKey = IFEOKey.OpenSubKey(keyName, true);
                    if (SubKey != null)
                    {
                        if (SubKey.GetValueNames().ToList().IndexOf("debugger") > -1)
                        {
                            TempIFEO = new IFEOItem();
                            TempIFEO.ManageByThis = ((string)GetValue(SubKey, "IFEOManage_Manage") == "True");
                            TempIFEO.RegKey = SubKey;
                            TempIFEO.IFEOPath = (string)GetValue(SubKey, "IFEOManage_Path");
                            TempIFEO.PEName = SubKey.Name.Split('\\').Last();
                            TempIFEO.Debugger = SubKey.GetValue("debugger").ToString();
                            TempIFEO.Remark = (string)GetValue(SubKey, "IFEOManage_Remark");

                            _FormatDebuggerString(TempIFEO);
                            Items.Add(TempIFEO);
                        }

                    }
                }
            }
            return true;
        }



        /// <summary>
        /// Deletes IFEO items from both class and registry
        /// </summary>
        /// <param name="DeleteList">The deletion list.</param>
        /// <returns>The state of deletion</returns>
        public bool Delete(List<IFEOItem> DeleteList)
        {
            if (IFEOKey != null)
            {
                DeleteList.ForEach(delegate (IFEOItem Item)
                {
                    IFEOKey.DeleteSubKeyTree(Item.PEName);
                });
            }
            Load();
            return true;

        }
        /// <summary>
        /// Deletes IFEO items from both class and registry
        /// </summary>
        /// <param name="DeleteIDList">The deletion identifier list.</param>
        /// <returns>The state of deletion</returns>
        public bool Delete(List<int> DeleteIDList)
        {
            if (IFEOKey != null)
            {
                DeleteIDList.ForEach(delegate (int Item)
                {
                    IFEOKey.DeleteSubKeyTree(Items[Item].PEName);
                });
            }
            Load();
            return true;
        }

        /// <summary>
        /// Save IFEO Item to Registry
        /// </summary>
        /// <param name="Item">The item.</param>
        /// <returns>
        /// true
        /// </returns>
        public bool Save(IFEOItem Item)
        {
            IFEOKey = Registry.LocalMachine.OpenSubKey(IFEORegPath, true);
            Item.RegKey = IFEOKey.CreateSubKey(Item.PEName);
            if (Item.Debugger == "")
            {
                Item.RegKey.DeleteValue("debugger");
            }
            else
            {
                Item.RegKey.SetValue("debugger", Item.Debugger);
            }
            Item.RegKey.SetValue("IFEOManage_Path", IFEORunPath);
            Item.RegKey.SetValue("IFEOManage_Remark", Item.Remark);
            Item.RegKey.SetValue("IFEOManage_Manage", Item.ManageByThis);
            Item.RegKey.Close();
            Load();
            return true;
        }
    }
}
