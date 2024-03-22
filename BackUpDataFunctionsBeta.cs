using DirectoryFunctionsNamespace;
using FileFunctionsNamespace;
using System;
using System.IO;

// Commercial use (license)
// Please study about commercial use of the code that is publicly available 

namespace DataBackUpFunctionsNamepsace
{
    class BackUpData
    {
        public BackUpData() { }
        static string _dir_back_up = "BackUp";
        public static void BackUp(string dir_path, string extension = "*.*")
        {
            string[] filenames = DirectoryFunctions.GetFiles.All(dir_path, extension);
            DirectoryFunctions.Make(_dir_back_up);
            string[] files_in_dir = Directory.GetFiles(_dir_back_up, "*.*", SearchOption.TopDirectoryOnly);
            if (files_in_dir.Length != 0)
            {
                string sourceDirectory = _dir_back_up;
                string destinationDirectory = _dir_back_up + "Temp";
                try
                {
                    Directory.Move(sourceDirectory, destinationDirectory);
                }
                catch (Exception e1)
                {
                    Console.WriteLine(e1.Message);
                }
                DirectoryFunctions.Make(_dir_back_up);
                try
                {
                    Directory.Move(destinationDirectory, _dir_back_up + "\\Previous_BackUp");
                }
                catch (Exception e1)
                {
                    Console.WriteLine(e1.Message);
                }
            }
            FileFunctions.TextFile.FilesCopyToFolder(filenames, _dir_back_up);
        }
    }
}
