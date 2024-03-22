using System.IO;

// Commercial use (license)
// Please study about commercial use of the code that is publicly available 

// Written. 2024.01.14 16:40. Moscow. Hostel 
namespace HDDFunctionsNamespace
{
    public static class HDDFunctions
    {

        /// <summary>
        /// Gets the total free space of certain drive. Note. name - C:\\ <br></br>
        /// Written. 2024.01.14 16:44. Moscow. Hostel <br></br>
        /// Tested. Works. 2024.01.14 16:53. Moscow. Hostel. <br></br>
        /// Notes. See the code of function. 2024.01.17 14:39. Moscow. Workplace.
        ///
        /// </summary>
        /// <param name="drive_letter"></param>
        /// <returns></returns>

        // notes. 2024.01.17 14:38. Moscow. Workplace. 
        // 1. Folder takes 1 cluster (32kb) for name storage.
        // 2. File takes space for name storage.
        // 3. Files and folder use the same space
        // because adding 0 byte file did not increase space used but
        // adding many 0 bytes files increased space used.
        // 4. Increase and decrease in space used to store files and folders
        // names has database approach - increase by sevral clusters and decrease
        // requires deletion of more 0 bytes files than space used to store filenames
        // by 3-4 times.

        public static long HDDFreeSpace(string drive_letter)
        {

            foreach (DriveInfo drive in DriveInfo.GetDrives())
            {
                if (drive.IsReady && drive.Name == drive_letter)
                {
                    return drive.AvailableFreeSpace;
                    
                }
            }
            return -1;
        }



        /// <summary>
        /// Gets the total space of certain drive. Note. name - C:\\ <br></br>
        /// Written. 2024.01.14 16:45. Moscow. Hostel <br></br>
        /// Tested. Works. 2024.01.14 16:53. Moscow. Hostel 
        /// </summary>
        /// <param name="drive_letter"></param>
        /// <returns></returns>
        public static long HDDTotalSpace(string drive_letter)
        {

            foreach (DriveInfo drive in DriveInfo.GetDrives())
            {
                if (drive.IsReady && drive.Name == drive_letter)
                {
                    return drive.TotalSize;
                    
                }
            }
            return -1;
        }




    }


}

