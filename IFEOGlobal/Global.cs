using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Principal;
using System.Text;

namespace IFEOGlobal
{
    public enum RunMethod
    {
        Close = 0,
        Popup = 1
    }
    public class Global
    {
        public static string IFEORunPath = System.AppDomain.CurrentDomain.BaseDirectory;
        public static string IFEOExecution = System.AppDomain.CurrentDomain.BaseDirectory + "IFEOExecution.exe";
        public const string IFEORegPath = @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Image File Execution Options\";
        public static bool IsAdministrator()
        {
            WindowsIdentity Identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal Principal = new WindowsPrincipal(Identity);
            return Principal.IsInRole(WindowsBuiltInRole.Administrator);
        }



    }
}
