using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;

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
        public string PEName { get; set; }

        /// <summary>
        /// Gets or sets the remark.
        /// </summary>
        /// <value>
        /// The remark.
        /// </value>
        public string Remark { get; set; }
        
        /// <summary>
        /// Gets or sets the debugger.
        /// </summary>
        /// <value>
        /// The debugger.
        /// </value>
        public string Debugger { get; set; }
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
        public bool ManageByThis { get; set; }

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

    public class IFEO
    {
        public static string IFEORunPath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
        public static string IFEOExecution = IFEORunPath + "\\IEFOExecution.exe";
        private const string IFEORegPath = @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Image File Execution Options\";
        /// <summary>
        /// The items
        /// </summary>
        public static List<IFEOItem> Items = new List<IFEOItem>();

        private static string GetValue(RegistryKey RegKey, string Key)
        {
            object Value = RegKey.GetValue(Key);
            return Value == null ? "" : Value.ToString();
        } 

        private static void _FormatDebuggerString(IFEOItem Debugger)
        {
            string OriginalDebugger = Debugger.Debugger;
            Debugger.Debugger = Debugger.Debugger.Replace(Debugger.IFEOPath, "{%Path%}");
            if (OriginalDebugger != Debugger.Debugger)
            {
                Debugger.ManageByThis = true;
            }
        }

        /// <summary>
        /// Loads IFEO items
        /// </summary>
        /// <returns>The state of load</returns>
        public static bool Load()
        {
            RegistryKey IFEOKey = Registry.LocalMachine.OpenSubKey(IFEORegPath, false);
            IFEOItem TempIFEO;
            if (IFEOKey != null) 
            {
                foreach (string keyName in IFEOKey.GetSubKeyNames())
                {
                    RegistryKey SubKey = IFEOKey.OpenSubKey(keyName, false);
                    if (SubKey != null)
                    {
                        if (SubKey.GetValueNames().ToList().IndexOf("debugger") > -1)
                        {
                            TempIFEO = new IFEOItem();
                            TempIFEO.ManageByThis = false;
                            TempIFEO.RegKey = SubKey;
                            TempIFEO.IFEOPath = GetValue(SubKey, "IFEOManage_Path");
                            TempIFEO.PEName = SubKey.Name.Split('\\').Last();
                            TempIFEO.Debugger = SubKey.GetValue("debugger").ToString();
                            TempIFEO.Remark = GetValue(SubKey, "IFEOManage_Remark");

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
        public static bool Delete(List<IFEOItem> DeleteList)
        {

            return true;

        }
        /// <summary>
        /// Deletes IFEO items from both class and registry
        /// </summary>
        /// <param name="DeleteIDList">The deletion identifier list.</param>
        /// <returns>The state of deletion</returns>
        public static bool Delete(List<int> DeleteIDList)
        {

            return true;
        }
    }
}
