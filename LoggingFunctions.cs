using DatetimeFunctionsNamespace;
using FileFunctionsNamespace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

// Commercial use (license)
// Please study about commercial use of the code that is publicly available 

namespace LoggingFunctionsNamespace
{
    /// <summary>
    /// Show/Hide Console. The project should be console application
    /// 2023.7.24 23:46
    /// </summary>
    static class ConsoleExtension
    {
        const Int32 SW_HIDE = 0;
        const Int32 SW_SHOW = 5;
        readonly static IntPtr handle = GetConsoleWindow();
        [DllImport("kernel32.dll")] static extern IntPtr GetConsoleWindow();
        [DllImport("user32.dll")] static extern bool ShowWindow(IntPtr hWnd, Int32 nCmdShow);
        public static void Hide()
        {
            ShowWindow(handle, SW_HIDE); //hide the console
        }
        public static void Show()
        {
            ShowWindow(handle, SW_SHOW); //show the console
        }
    }
    class LogDatetimeString
    {
        public LogDatetimeString() { }
        string[] log_strings = null;
        DateTime[] log_datetimes = null;
        public LogDatetimeString(string string_in)
        {
            FillLogs(string_in);
        }
        public DateTime LastLogDatetime
        {
            get
            {
                return log_datetimes[log_datetimes.Length - 1];
            }
        }
        public string LastLog
        {
            get
            {
                return log_strings[log_strings.Length - 1];
            }
        }
        public void FillLogs(string str_in)
        {
            string[] strings_saved = FileFunctions.TextFile.FileStringToStrings(str_in);
            for (Int32 i = 0; i < strings_saved.Length; i++)
            {
                string date_str = strings_saved[i].Split('\t')[0];
                string log_str = strings_saved[i].Split('\t')[1];
                log_strings[i] = log_str;
                DateTime.TryParse(date_str, out log_datetimes[i]);
            }
        }
        public void NewLog(string str_in)
        {
            Array.Resize(ref log_strings, log_strings.Length + 1);
            Array.Resize(ref log_datetimes, log_datetimes.Length + 1);
            log_datetimes[log_datetimes.Length - 1] = DateTime.Now;
            log_strings[log_strings.Length - 1] = str_in;
        }
        public string LogsOut()
        {
            string str_out = "";
            for (Int32 i = 0; i < log_datetimes.Length; i++)
            {
                if (i != 0)
                {
                    str_out += "\r\n";
                }
                string date_str = DatetimeFunctions.DatetimeToString(log_datetimes[i]);
                string log_str = log_strings[i];
                str_out += date_str + "\t" + log_str;
            }
            return str_out;
        }
    }
}
