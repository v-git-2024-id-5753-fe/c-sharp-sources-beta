using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;

// Commercial use (license)
// Please study about commercial use of the code that is publicly available 

namespace ExcelFunctionsNamespace
{
    class ExcelFunctions
    {
        public static string[][] ObjectsArrayNxNToStringArrayNxN(object[,] objects_in)
        {
            string[][] for_return = new string[objects_in.GetLength(0)][];
            for (Int32 i = 0; i < for_return.Length; i++)
            {
                for_return[i] = new string[objects_in.GetLength(1)];
                for (Int32 j = 0; j < for_return[i].Length; j++)
                {
                    if (objects_in[i + 1, j + 1] == null)
                    {
                        for_return[i][j] = "";
                        continue;
                    }
                    for_return[i][j] = objects_in[i + 1, j + 1].ToString();
                }
            }
            return for_return;
        }
        public static string[][] ExcelFileToStringsNxM(string file_in, Int32 datasheet_num = 1)
        {
            Application excel_app = new Application();
            excel_app.Visible = false;
            Workbook excel_wb = excel_app.Workbooks.Open(file_in);
            Worksheet excel_ws = excel_wb.Worksheets[datasheet_num];
            Range last_cell = excel_ws.Cells.SpecialCells(XlCellType.xlCellTypeLastCell, Type.Missing);
            Range data_range = excel_ws.Range[excel_ws.Cells[1, 1], last_cell.Cells[1, 1]];
            string[][] str_arr = ObjectsArrayNxNToStringArrayNxN(data_range.Value2);
            excel_wb.Close();
            excel_app.Quit();
            return str_arr;
        }
        public static void ExcelFileToFile(string file_in, string txt_file_path, Int32 datasheet_num = 1, bool parse_special_characters = false)
        {
            string[] for_file = ExcelFileToStrings(file_in, datasheet_num, parse_special_characters);
            FileMethods.FileMyMethods.StringsToFile(for_file, txt_file_path);
        }
        public static string[] ExcelFileToStrings(string file_in, Int32 datasheet_num = 1, bool parse_special_characters = true)
        {
            Application excel_app = new Application
            {
                Visible = false
            };
            Workbook excel_wb = excel_app.Workbooks.Open(file_in);
            Worksheet excel_ws = excel_wb.Worksheets[datasheet_num];
            Range last_cell = excel_ws.Cells.SpecialCells(XlCellType.xlCellTypeLastCell, Type.Missing);
            Range data_range = excel_ws.Range[excel_ws.Cells[1, 1], last_cell.Cells[1, 1]];
            string[][] str_arr = ObjectsArrayNxNToStringArrayNxN(data_range.Value2);
            excel_wb.Close();
            excel_app.Quit();
            string[] for_return = new string[str_arr.Length];
            for (Int32 i = 0; i < str_arr.Length; i++)
            {
                for_return[i] = "";
                for (Int32 j = 0; j < str_arr[i].Length; j++)
                {
                    if (j != 0)
                    {
                        for_return[i] += "\t";
                    }
                    if (parse_special_characters == true)
                    {
                        for_return[i] += FileMethods.FileMyMethods.StringParseSpecialCharacters(str_arr[i][j], false);
                    }
                    else
                    {
                        for_return[i] += str_arr[i][j];
                    }
                }
            }
            return for_return;
        }
        public static void ExcelFileToConsole(string file_in, Int32 datasheet_num = 1)
        {
            Application excel_app = new Application();
            excel_app.Visible = false;
            Workbook excel_wb = excel_app.Workbooks.Open(file_in);
            Worksheet excel_ws = excel_wb.Worksheets[datasheet_num];
            Range last_cell = excel_ws.Cells.SpecialCells(XlCellType.xlCellTypeLastCell, Type.Missing);
            Range data_range = excel_ws.Range[excel_ws.Cells[1, 1], last_cell.Cells[1, 1]];
            string[][] str_arr = ObjectsArrayNxNToStringArrayNxN(data_range.Value2);
            Int32 ri = last_cell.Row;
            Int32 ci = last_cell.Column;
            excel_wb.Close();
            excel_app.Quit();
            for (Int32 i = 1; i <= ri; i++)
            {
                if (i != 1)
                {
                    Console.Write("\r\n");
                }
                for (Int32 j = 1; j <= ci; j++)
                {
                    if (j != 1)
                    {
                        Console.Write("\t");
                    }
                    string cell_str = str_arr[i - 1][j - 1]; // note left from previous coding
                    //string cell_str = data_range.Cells[i,j].Value;
                    Console.Write(cell_str);
                }
            }
            Console.WriteLine("reached");
        }
    }
}