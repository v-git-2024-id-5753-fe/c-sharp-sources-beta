using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Commercial use (license)
// Please study about commercial use of the code that is publicly available 

// 2024.01.19 11:02. Moscow. Workplace. 
namespace ProcessFunctionsNamespace
{
    /// <summary>
    /// Written. 2024.01.19 11:02. Moscow. Workplace. 
    /// </summary>
    public static class ProcessFunctions
    {

        /// <summary>
        /// Return string[] that contains names of running processes. <br></br>
        /// Written. 2024.01.19 11:07. Moscow. Workplace. <br></br>
        /// Tested. Works. 2024.01.19 11:22. Moscow. Workplace. 
        /// </summary>
        /// <returns></returns>
        public static string[] AllProcessesNames()
        {
            Process[] process_arr = Process.GetProcesses();
            string[] arr_out = new string[process_arr.Length];
            Int32 arr_index = 0;
            foreach (Process one_process in  process_arr) 
            {
                arr_out[arr_index] = one_process.ProcessName;
                arr_index++;
            }
            return arr_out;
        }

        /// <summary>
        /// Return Process[] that contains processes with the provided name. <br></br>
        /// Written. 2024.01.19 11:09. Moscow. Workplace. <br></br>
        /// Tested. Works. 2024.01.19 11:23. Moscow. Workplace. 
        /// </summary>
        /// <param name="process_name"></param>
        /// <returns></returns>
        public static Process[] ProcessesByName(string process_name)
        {
            return Process.GetProcessesByName(process_name);
        }


        /// <summary>
        /// Kills all processes with the provided name.
        /// Written. 2024.01.19 11:12. Moscow. Workplace. 
        /// </summary>
        /// <param name="process_name"></param>
        /// <returns></returns>
        public static void ProcessesKillByName(string process_name)
        {
            Process[] process_arr = Process.GetProcessesByName(process_name);
            ProcessesKill(process_arr);
        }



        /// <summary>
        /// Kills all processes provided in Process[] <br></br>
        /// Written. 2024.01.19 11:12. Moscow. Workplace. <br></br>
        /// Tested. Works. 2024.01.19 11:27. Moscow. Workplace. 
        /// </summary>
        /// <param name="process_name"></param>
        /// <returns></returns>
        public static void ProcessesKill(Process[] process_arr)
        {            
            for (Int32 i = 0; i < process_arr.Length; i++)
            {
                process_arr[i].Kill();
            }
        }

    }
}
