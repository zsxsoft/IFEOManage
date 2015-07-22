using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace IFEOGlobal
{
    public class Launcher
    {
        [System.Runtime.InteropServices.StructLayout(LayoutKind.Sequential)]
        public class SECURITY_ATTRIBUTES
        {
            public int nLength;
            public string lpSecurityDescriptor;
            public bool bInheritHandle;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct STARTUPINFO
        {
            public int cb;
            public string lpReserved;
            public string lpDesktop;
            public int lpTitle;
            public int dwX;
            public int dwY;
            public int dwXSize;
            public int dwYSize;
            public int dwXCountChars;
            public int dwYCountChars;
            public int dwFillAttribute;
            public int dwFlags;
            public int wShowWindow;
            public int cbReserved2;
            public byte lpReserved2;
            public IntPtr hStdInput;
            public IntPtr hStdOutput;
            public IntPtr hStdError;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct PROCESS_INFORMATION
        {
            public IntPtr hProcess;
            public IntPtr hThread;
            public int dwProcessId;
            public int dwThreadId;
        }

        [DllImport("Kernel32.dll", CharSet = CharSet.Ansi)]
        public static extern bool CreateProcess(
            StringBuilder lpApplicationName, StringBuilder lpCommandLine,
            SECURITY_ATTRIBUTES lpProcessAttributes,
            SECURITY_ATTRIBUTES lpThreadAttributes,
            bool bInheritHandles,
            int dwCreationFlags,
            StringBuilder lpEnvironment,
            StringBuilder lpCurrentDirectory,
            ref STARTUPINFO lpStartupInfo,
            ref PROCESS_INFORMATION lpProcessInformation
            );

        [DllImport("ntdll")]
        public static extern bool DbgUiConnectToDbg();

        [DllImport("ntdll")]
        public static extern int DbgUiStopDebugging(IntPtr hProcess);

        [DllImport("Kernel32.dll")]
        public static extern bool CloseHandle(IntPtr hObject);

        /// <summary>
        /// Creates the process.
        /// </summary>
        /// <param name="ProcessName">Name of the process.</param>
        /// <param name="CommandLine">The command line.</param>
        /// <exception cref="System.Exception">Failure</exception>
        public static void CreateProcess(string ProcessName, string CommandLine)
        {
            STARTUPINFO SInfo = new STARTUPINFO();
            PROCESS_INFORMATION PInfo = new PROCESS_INFORMATION();
            DbgUiConnectToDbg();
            if (!CreateProcess(null, new StringBuilder(ProcessName).Append(" ").Append(CommandLine), null, null, false, 0x1 | 0x2, null, null, ref SInfo, ref PInfo))
            {
                // May be must be run under Administrator.

                if (IFEOGlobal.Function.IsAdministrator())
                {
                    throw new Exception("Failure");
                }
                else
                {
                    // Try run as administrator.
                    ProcessStartInfo startInfo = new ProcessStartInfo();
                    startInfo.UseShellExecute = true;
                    startInfo.WorkingDirectory = Environment.CurrentDirectory;
                    startInfo.FileName = Process.GetCurrentProcess().MainModule.FileName;
                    startInfo.Arguments = string.Join(" ", Environment.CommandLine.Split(' ').Skip(1));
                    startInfo.Verb = "runas";
                    Process.Start(startInfo);
                }
            }
            DbgUiStopDebugging(PInfo.hProcess);
            CloseHandle(PInfo.hProcess);
            CloseHandle(PInfo.hThread);
        }




    }
}
