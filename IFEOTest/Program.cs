using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IFEOTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("IsAdministrator: " + IFEOGlobal.Global.IsAdministrator().ToString());
            Console.WriteLine("CommandLine: " + Environment.CommandLine);
            Console.WriteLine("CurrentDirectory: " + Environment.CurrentDirectory);
            Console.WriteLine("Args: " + string.Join(", ", args));
            Console.ReadLine();
        }
    }
}
