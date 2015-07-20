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
    }

    public class IFEO
    {

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
                            TempIFEO.RegKey = SubKey;
                            TempIFEO.PEName = SubKey.Name.Split('\\').Last();
                            TempIFEO.Debugger = SubKey.GetValue("debugger").ToString();
                            TempIFEO.Remark = GetValue(SubKey, "IFEOManage_Remark");
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
        /// <param name="DeleteIDList">The delete identifier list.</param>
        /// <returns>The state of delete</returns>
        public static bool Delete(List<int> DeleteIDList)
        {

            return true;

        }
    }
}
