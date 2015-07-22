using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace IFEOGlobal
{
    public enum RunMethod
    {
        Close = 0,
        Popup = 1
    }
    public class Function
    {
        public static bool IsAdministrator()
        {
            WindowsIdentity Identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal Principal = new WindowsPrincipal(Identity);
            return Principal.IsInRole(WindowsBuiltInRole.Administrator);
        }
    }
}
