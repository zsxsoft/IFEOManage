using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;

namespace IFEOGlobal
{
    public class Log
    {

        /// <summary>
        /// Writes to file.
        /// </summary>
        /// <param name="StringText">The string text.</param>
        private static void WriteToFile(string StringText)
        {
            if (!Config.Data.Log) return;
            StreamWriter Writer = File.AppendText(System.AppDomain.CurrentDomain.BaseDirectory + "\\Log.txt");
            Writer.Write(StringText);
            Writer.Close();
            Writer.Dispose();
        }

        /// <summary>
        /// Writes log with Lf.
        /// </summary>
        /// <param name="StringText">The string text.</param>
        /// <param name="DisplayInConsole">if set to <c>true</c> [display in console].</param>
        public static void WriteLine(string StringText, bool DisplayInConsole = false) {
            if (DisplayInConsole)
            {
                Console.WriteLine(StringText);
            }
            if (Config.Data.Log)
            {
                WriteToFile("[" + DateTime.Now + "] " + StringText + "\n");
            }
        }

        /// <summary>
        /// Writes log.
        /// </summary>
        /// <param name="StringText">The string text.</param>
        /// <param name="DisplayInConsole">if set to <c>true</c> [display in console].</param>
        public static void Write(string StringText, bool DisplayInConsole = false)
        {
            if (DisplayInConsole)
            {
                Console.Write(StringText);
            }
            if (Config.Data.Log)
            {
                WriteToFile(StringText);
            }
        }

        /// <summary>
        /// Warnings the specified string text.
        /// </summary>
        /// <param name="StringText">The string text.</param>
        /// <returns></returns>
        public static MessageBoxResult MessageBoxError(string StringText)
        {
            return MessageBox.Show(StringText, "Error", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
        }
    }
}
