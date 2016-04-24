﻿using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace IFEOGlobal
{
    public class Launcher
    {
        [StructLayout(LayoutKind.Sequential)]
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
            Log.WriteLine("CreateProcess(" + ProcessName + ", " + CommandLine + ")");
            STARTUPINFO SInfo = new STARTUPINFO();
            PROCESS_INFORMATION PInfo = new PROCESS_INFORMATION();
            Log.WriteLine("Try Connect to DBG");
            DbgUiConnectToDbg();
            Log.WriteLine("Try CreateProcess");
            if (!CreateProcess(null, new StringBuilder(ProcessName).Append(" ").Append(CommandLine), null, null, false, 0x1 | 0x2, null, null, ref SInfo, ref PInfo))
            {
                // May be must be run under Administrator.

                if (Global.IsAdministrator())
                {
                    Log.WriteLine("Creating process failed.");
                }
                else
                {
                    RestartWithAdministrator();
                }
            }
            Log.WriteLine("Try Stop Debugging");
            DbgUiStopDebugging(PInfo.hProcess);
            CloseHandle(PInfo.hProcess);
            CloseHandle(PInfo.hThread);
            Log.WriteLine("Created Process.");
        }



        /// <summary>
        /// Restarts this instance with administrator right.
        /// Copied from .NET source code: winforms\Managed\System\WinForms\Application.cs
        /// </summary>
        /// 
        public static void RestartWithAdministrator()
        {

            string[] arguments = Environment.GetCommandLineArgs();
            Debug.Assert(arguments != null && arguments.Length > 0);
            StringBuilder sb = new StringBuilder((arguments.Length - 1) * 16);
            for (int argumentIndex = 1; argumentIndex < arguments.Length - 1; argumentIndex++)
            {
                sb.Append('"');
                sb.Append(arguments[argumentIndex]);
                sb.Append("\" ");
            }
            if (arguments.Length > 1)
            {
                sb.Append('"');
                sb.Append(arguments[arguments.Length - 1]);
                sb.Append('"');
            }
            ProcessStartInfo currentStartInfo = Process.GetCurrentProcess().StartInfo;
            currentStartInfo.FileName = Process.GetCurrentProcess().MainModule.FileName;
            // [....]: use this according to spec.
            // String executable = Assembly.GetEntryAssembly().CodeBase;
            // Debug.Assert(executable != null);
            if (sb.Length > 0)
            {
                currentStartInfo.Arguments = sb.ToString();
            }
            currentStartInfo.Verb = "runas";
            Log.WriteLine("Try runas administrator: " + currentStartInfo.Arguments);
            try
            {
                Process.Start(currentStartInfo);
            }
            catch (System.ComponentModel.Win32Exception Ex)
            {
                Log.WriteLine("Exception: " + Ex.ToString());
            }
            finally
            {
                Environment.Exit(0);
            }

        }




    }
}
