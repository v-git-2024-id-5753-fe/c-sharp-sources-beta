using FileFunctionsNamespace;
using ReportFunctionsNamespace;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

// Commercial use (license)
// Please study about commercial use of the code that is publicly available 

namespace DirectoryFunctionsNamespace
{
    public static class DirectoryFunctions
    {


        // template for code execution. 2024.03.12 10:14. Moscow. Workplace. 
        /*
        double execution_time_ms_start = 0;
        if (TimeExecutionShow == true)
            {
                execution_time_ms_start = _time_execution.Elapsed.TotalMilliseconds;
            }
        if (TimeExecutionShow == true)
            {
                double execution_time_ms_stop = _time_execution.Elapsed.TotalMilliseconds;
                TimeExecutionMessage(nameof(function_name_here), execution_time_ms_stop - execution_time_ms_start);
            }
        */



        /// <summary>
        /// Added. 2024.03.12 10:14. Moscow. Workplace. 
        /// </summary>
        public static bool TimeExecutionShow
        {
            get
            {
                return _time_execution_bool;
            }

            set
            {
                if (value == true)
                {
                    _time_execution_bool = true;
                    _time_execution.Start();
                    return;
                }
                if (value == false)
                {
                    _time_execution_bool = false;
                    _time_execution.Stop();
                    _time_execution.Reset();
                    return;

                }
            }
        }

        static bool _time_execution_bool = false;

        static Stopwatch _time_execution = new Stopwatch();
        static Int32 _time_execution_count = 0;
        /// <summary>
        /// Added. 2024.03.12 10:14. Moscow. Workplace. 
        /// </summary>
        /// <param name="function_name"></param>
        /// <param name="total_ms_passed"></param>
        static void TimeExecutionMessage(string function_name, double total_ms_passed)
        {
            _time_execution_count += 1;
            Console.WriteLine(_time_execution_count.ToString() + ". " + DateTime.Now.ToString("HH:mm:ss") + " " + function_name +
                " exectuion time: " + total_ms_passed.ToString("0.000") + " ms");
        }















        /// <summary>
        /// 2023.08.26 19:21. written.
        /// 2023.08.27 14:27. tested. works.
        /// </summary>
        /// <param name="dir_path"></param>
        /// <returns></returns>
        static public string Name(string dir_path)
        {
            string str_out = "";
            if (dir_path[dir_path.Length - 1] != '\\')
            {
                MyReportFunctions.ReportError(dir_path + "\r\n" + "there were no \\ at the end");
                return "";
            }
            try
            {
                string[] str_split = dir_path.Split('\\');
                str_out = str_split[str_split.Length - 1 - 1];
            }
            catch
            {
                MyReportFunctions.ReportError(dir_path + "\r\n" + "the path was not splitted by \\");
                return "";
            }
            return str_out;
        }
        /// <summary>
        /// Written. 2023.11.01 15:28. Moscow. Workplace. 
        /// </summary>
        public static class GetDirectories
        {
            public static string[] InDirectory(string dir_path, bool should_exist = true)
            {
                if (should_exist == true)
                {
                    if (Directory.Exists(dir_path) == false)
                    {
                        MyReportFunctions.ReportError(dir_path + " does not exists");
                        return new string[0];
                    }
                }
                return Directory.GetDirectories(dir_path);
            }
            /// <summary>
            /// Written. <br></br>
            /// 2023.11.01 15:56. Moscow. Workplace.<br></br><br></br>
            /// 
            /// Tested. Works. Console message was not tested. <br></br>
            /// 2023.11.01 15:57. Moscow. Workplace. <br></br>
            ///  
            /// </summary>
            /// <param name="dir_path"></param>
            /// <param name="level_count"></param>
            /// <param name="should_exist"></param>
            /// <param name="msg_console"></param>
            /// <returns></returns>
            public static string[] ByLevelFromDirectory(string dir_path, Int32 level_count, bool should_exist = true, bool msg_console = false)
            {
                if (should_exist == true)
                {
                    if (Directory.Exists(dir_path) == false)
                    {
                        MyReportFunctions.ReportError(dir_path + " does not exists");
                        return new string[0];
                    }
                }
                string[] dirs_arr_out = new string[0];
                try
                {
                    dirs_arr_out = Directory.GetDirectories(dir_path);
                }
                catch
                {
                    if (msg_console == true)
                    {
                        Console.WriteLine("Failure to obtain directories in " + dir_path);
                    }
                    dirs_arr_out = new string[0];
                }
                return dirs_arr_out;
            }
            /// <summary>
            /// Written. 2023.11.01 15:44. Moscow. Workplace.<br></br>
            ///            
            /// Trouble. If access is denied then there is expepation but it is one function and it does not 
            /// return what was found. <br></br>
            /// 2023.11.01 15:45. Moscow. Workplace. 
            /// </summary>
            /// <param name="dir_path"></param>
            /// <param name="should_exist"></param>
            /// <returns></returns>
            public static string[] AllLevelsFromDirectory(string dir_path, bool should_exist = true)
            {
                if (should_exist == true)
                {
                    if (Directory.Exists(dir_path) == false)
                    {
                        MyReportFunctions.ReportError(dir_path + " does not exists");
                        return new string[0];
                    }
                }
                return Directory.GetDirectories(dir_path, "*", SearchOption.AllDirectories);
            }
        }
        /// <summary>
        /// 2023.08.25 10:24. started
        /// 
        /// </summary>
        public static class DirectoryPath
        {
            /// <summary>
            /// 2023-08-01 12:45. written.
            /// 2023.08.25 10:57. tested. works.
            /// </summary>
            /// <returns></returns>
            public static string Programm()
            {
                return System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "\\";
            }
            /// <summary>
            /// 2023.05.01 - 2023.06.25. 10 - 15 o'clock. written.
            /// 2023.08.25 10:57. tested. works.
            /// </summary>
            /// <param name="filename"></param>
            /// <returns></returns>
            static public string File(string filename)
            {
                return System.IO.Path.GetDirectoryName(filename) + "\\";
            }
        }
        // new
        /// <summary>
        /// Makes directory. 
        /// C# will make directory creating all the folders needed to make the last folder
        /// 2023.8.03 03:58
        /// </summary>
        /// <param name="dir_path"></param>
        static public void Make(string dir_path)
        {
            if (System.IO.Directory.Exists(dir_path) == false)
            {
                System.IO.Directory.CreateDirectory(dir_path);
            }
        }
        /// <summary>
        /// Written. 2024.03.12 10:11. Moscow. Workplace. 
        /// </summary>
        public static class GetFiles
        {

            /// <summary>
            /// Return string[] with paths of files in folder which have in the names provided part of the name <br></br>
            /// Written. 2024.03.12 10:13. Moscow. Workplace. <br></br>
            /// Tested. Works. 2024.03.12 10:20. Moscow. Workplace. 
            /// </summary>
            /// <param name="name_base"></param>
            /// <param name="dir_path"></param>
            /// <param name="file_extension"></param>
            /// <param name="sub_dirs"></param>
            /// <returns></returns>
            static public string[] ByPartOfName(string name_base, string dir_path, string file_extension = ".*", bool sub_dirs = false)
            {

                double execution_time_ms_start = 0;
                if (TimeExecutionShow == true)
                {
                    execution_time_ms_start = _time_execution.Elapsed.TotalMilliseconds;
                }

                string[] all_files = GetFiles.All(dir_path, file_extension, sub_dirs);
                List<string> files_out = new List<string>();
                for (Int32 i = 0;  i < all_files.Length; i++)
                {
                    if (all_files[i].Contains(name_base) == true)
                    {
                        files_out.Add(all_files[i]);
                    }
                }

                if (TimeExecutionShow == true)
                {
                    double execution_time_ms_stop = _time_execution.Elapsed.TotalMilliseconds;
                    TimeExecutionMessage(nameof(GetFiles.ByPartOfName), execution_time_ms_stop - execution_time_ms_start);
                }

                return files_out.ToArray();
                
            }


                static public string[] All(string dir_path, string file_extension = ".*", bool sub_dirs = false)
            {

                double execution_time_ms_start = 0;
                if (TimeExecutionShow == true)
                {
                    execution_time_ms_start = _time_execution.Elapsed.TotalMilliseconds;
                }
             


                string[] files_in_dir = new string[0];
                if (sub_dirs == false)
                {
                    if (file_extension == ".*")
                    {
                        files_in_dir = System.IO.Directory.GetFiles(dir_path);
                    }
                    else
                    {
                        files_in_dir = System.IO.Directory.GetFiles(dir_path, file_extension);
                    }
                }
                else
                {
                    if (file_extension == ".*")
                    {
                        files_in_dir = System.IO.Directory.GetFiles(dir_path, "*.*", SearchOption.AllDirectories);
                    }
                    else
                    {
                        files_in_dir = System.IO.Directory.GetFiles(dir_path, file_extension, SearchOption.AllDirectories);
                    }
                }

                if (TimeExecutionShow == true)
                {
                    double execution_time_ms_stop = _time_execution.Elapsed.TotalMilliseconds;
                    TimeExecutionMessage(nameof(GetFiles.All), execution_time_ms_stop - execution_time_ms_start);
                }
                return files_in_dir;
            }
        }
        /// <summary>
        /// Shows in console disks of PC
        /// 2023.7.27 22:09
        /// </summary>
        public static void DisksListToConsole()
        {
            FileFunctions.TextFile.StringsToConsole(DisksList());
        }
        /// <summary>
        /// Get disks letters of PC
        /// 2023.7.27 22:09
        /// </summary>
        /// <returns></returns>
        public static string[] DisksList()
        {
            return System.IO.Directory.GetLogicalDrives();
        }
        static public void FilesInDirectoryToConsole(string dir_path, string file_extension = ".*", bool sub_dirs = false)
        {
            string[] files_in_dir_all = GetFiles.All(dir_path, "*.*", sub_dirs);
            string[] files_in_dir = GetFiles.All(dir_path, file_extension, sub_dirs);
            Console.WriteLine(files_in_dir.Length.ToString() + "/" + files_in_dir_all.Length.ToString()
                + " " + file_extension + " files in " + dir_path + ":");
            FileFunctions.TextFile.StringsToConsole(files_in_dir);
        }
        static public void Delete(string dir_path)
        {
            if (System.IO.Directory.Exists(dir_path) == true)
            {
                System.IO.Directory.Delete(dir_path, true);
            }
        }
    }
}
