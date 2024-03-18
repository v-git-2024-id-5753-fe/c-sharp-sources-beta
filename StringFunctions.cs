
using CharFunctionsNamespace;
using FileFunctionsNamespace;
using ReportFunctionsNamespace;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

// Commercial use (license)
// Please study about commercial use of the code that is publicly available 

/// 2023.10.05 20:55. started. 
/// Moscow. Hostel. Kind Deer.
namespace StringFunctionsNamespace
{


    // 2023.11.15 20:38. Moscow. Hostel Delta.
    /// <summary>
    /// Makes a page of text and with certain refresh rate.
    /// </summary>
    class TextFrame
    {
        string[] Frame = new string[0];
        Int32 RowLength = 0;
        public TextFrame(Int32 rows_count, Int32 row_length)
        {
            Frame = new string[rows_count];
            TimerRefreshRate.Tick += TimerRefreshRate_Tick;
            RowLength = row_length;
        }
        private void TimerRefreshRate_Tick(object sender, EventArgs e)
        {
            TimerRefreshRate.Stop();
            ToConsole();
            TimerRefreshRate.Start();
        }
        // 2023.11.15 20:48. Moscow. Hostel Delta.
        public void ToConsole()
        {
            Console.Clear();
            for (Int32 i = 0; i < Frame.Length; i++)
            {
                Console.WriteLine(Frame[i]);
            }
        }
        Timer TimerRefreshRate = new Timer();
        Int32 _refresh_rate = 10;
        public Int32 RefreshRate
        {
            get
            {
                return _refresh_rate;
            }
            set
            {
                _refresh_rate = value;
                if (_refresh_rate > 50)
                {
                    _refresh_rate = 50;
                }
                TimerRefreshRate.Interval = 1000 / _refresh_rate;
            }
        }
        public void StartDrawing()
        {
            TimerRefreshRate.Start();
        }
        public void StopDrawing()
        {
            TimerRefreshRate.Stop();
        }
        char CheckBoardChar = ' ';
        public void RunSymbolCheckBoardPattern(Int32 rows, char symbol)
        {
            Frame = new string[rows];
            CheckBoardChar = symbol;
        }
        bool CheckBoardSwitch = false;
        public string[] SymbolCheckBoardPattern()
        {
            for (Int32 i = 0; i < Frame.Length; i++)
            {
                Frame[i] = "";
                char[] char_arr = new char[RowLength];
                for (Int32 j = 0; j < RowLength; j++)
                {
                    char_arr[j] = CheckBoardChar;
                }
            }
            return new string[0];
        }
    }
    /// <summary>
    /// 2023.10.06 10:06. Moved from MyFileFunctions.MyStringFunctions to MyStringFunctions
    /// 2023.11.26 03:38. Moscow. Hostel Delta. Renamed from StringCoding to CharFromStringCycling
    /// </summary>
    class CharFromStringCycling
    {
        public class NextChar
        {
            public NextChar(string string_in, Int32 start_pos = 0)
            {
                string_select_letter = string_in;
                index_letter = start_pos;
            }
            string string_select_letter = "";
            Int32 index_letter = 0;
            public char Next()
            {
                char char_out = string_select_letter[index_letter];
                index_letter++;
                if (index_letter > (string_select_letter.Length - 1))
                {
                    index_letter = 0;
                }
                return char_out;
            }
        }
    }
    /// <summary>
    /// 2023.10.05 20:56. written.
    /// </summary>
    public static class StringFunctions
    {

        static Random _internal_random = new Random();

        public enum TagEnum
        {
            NoTag,
            rln,
            rl,
            rn,
            /// <summary>
            /// datetime. Example 2020.07.24
            /// </summary>
            dd,
            /// <summary>
            /// time. Example 15-40. 
            /// </summary>
            dt
        }

        /// <summary>
        /// Written. 2024.03.11 00:44. Moscow. Hostel.
        /// </summary>
        /// <param name="tag_in"></param>
        /// <returns></returns>
        public static string FromTag(string tag_in)
        {
                     

            // 2024.03.11 00:44. Moscow. Hostel.
            // random string from letter and numbers
            if (tag_in.Contains(TagEnum.rln.ToString()) == true)
            {
                Int32 length = System.Convert.ToInt32(tag_in.Replace(TagEnum.rln.ToString(), ""));
                return Generate.EN_Letters_UpperCase_Numbers(length);
            }

            // 2024.03.11 00:50. Moscow. Hostel.
            // if there were rln this part of the code will not be executed.
            if (tag_in.Contains(TagEnum.rl.ToString()) == true)
            {
                Int32 length = System.Convert.ToInt32(tag_in.Replace(TagEnum.rl.ToString(), ""));
                return Generate.EN_Letters_UpperCase(length);
            }


            // 2024.03.11 01:00. Moscow. Hostel.
            if (tag_in.Contains(TagEnum.rn.ToString()) == true)
            {
                Int32 length = System.Convert.ToInt32(tag_in.Replace(TagEnum.rn.ToString(), ""));
                return Generate.Numbers(length);
            }

            // 2024.03.11 01:00. Moscow. Hostel.
            if (tag_in.Contains(TagEnum.dd.ToString()) == true)
            {                
                return DateTime.Now.ToString("yyyy-MM-dd");
            }

            // 2024.03.11 01:00. Moscow. Hostel.
            if (tag_in.Contains(TagEnum.dt.ToString()) == true)
            {
                return DateTime.Now.ToString("HH-mm");
            }
            

            return "";

        }



        /// <summary>
        /// Written. 2024.01.30 04:57. Moscow. Hostel. <br></br>
        /// ASCII: <br></br>
        /// 
        /// </summary>
        public static class Trim
        {
            /// <summary>
            /// Written. 2024.01.30 05:03. Moscow. Hostel.
            /// </summary>
            public static class Right
            {

                /// <summary>
                /// Was not finished because of str_in.TrimEnd(); 
                /// </summary>
                /// <param name="str_in"></param>
                /// <returns></returns>
                public static string Charchers_space(string str_in)
                {
                   return str_in.TrimEnd();
                }
            }

        }

        /// <summary>
        /// Written. 2024.01.19 14:18. Moscow. Workplace. 
        /// </summary>
        public static class Cut
        {

            /// <summary>
            /// Returns string that starts from text provided or returns "". <br></br>
            /// Written. 2024.01.19 14:21. Moscow. Workplace. <br></br>
            /// Tested. Works. 2024.01.19 14:32. Moscow. Workplace. 
            /// </summary>
            /// <returns></returns>
            public static string FromText(string str_in, string text_start)
            {
                Int32 index_start = str_in.IndexOf(text_start);
                if (index_start != -1)
                {
                    return str_in.Substring(index_start);
                }
                return "";
            }

            /// <summary>
            /// Written. 2024.01.30 04:55. Moscow. Hostel.
            /// not tested
            /// </summary>
            /// <param name="str_in"></param>
            /// <param name="text_start"></param>
            /// <returns></returns>
            public static string BeforeText(string str_in, string text_start)
            {
                Int32 index_start = str_in.IndexOf(text_start);
                if (index_start != -1)
                {
                    return str_in.Substring(0, index_start);
                }
                return "";
            }


        }



            /// <summary>
            /// Written. 2024.01.14 17:45. Moscow. Hostel 
            /// </summary>
            public static class Formating
        {


            /// <summary>
            /// Reverses characters in string <br></br>
            /// Written. 2024.01.14 17:52. Moscow. Hostel <br></br> 
            /// Tested. Works. 2024.01.14 18:15. Moscow. Hostel 
            /// </summary>
            /// <param name="str_in"></param>
            /// <returns></returns>
            public static string ReverseSymbols(string str_in)
            {
                string str_out = "";
                for (Int32 i = str_in.Length - 1; i >= 0; i--)
                {
                    str_out += str_in[i].ToString();
                }
                return str_out;
            }


            /// <summary>
            /// Converts Int64 to string and adds space between defined number of characters <br></br>
            /// Written. 2024.01.14 17:58. Moscow. Hostel <br></br>
            /// Tested. Works. 2024.01.14 18:14. Moscow. Hostel 
            /// </summary>
            /// <param name="number_in"></param>
            /// <param name="space_between_digits"></param>
            /// <returns></returns>

            public static string Int64WithSpaces(long number_in, Int32 space_between_digits = 3)
            {
                string number_str = number_in.ToString();
                string str_out = ""; 
                if (number_str.Length > space_between_digits)
                {
                    Int32 chars_written = 0;
                    for (Int32 i = number_str.Length - 1; i >= 0; i--)
                    {
                        str_out += number_str[i].ToString();
                        chars_written += 1;
                    if (chars_written == space_between_digits)
                        {
                            if (i == 0)
                            {
                                // 2024.01.14 18:13. Moscow. Hostel
                                // Without it it will add space and there will be number
                                // with space at the beginning.
                                break;
                            }
                            
                            str_out += " ";
                            chars_written = 0;
                        }
                    
                    }
                    str_out = ReverseSymbols(str_out);
                }
                else
                {
                    str_out = number_str;
                }
                return str_out;
            }
            
        }



        // 2023-07-26 14:33. importance 3. default file path is project location 
        // if the app is started from visual studio otherwise it is
        // C:\Windows\System32 if file is on desktop
        public static class Generate
        {
            static string EngLettersLowerCase = "abcdefghijklmnopqrstuvwxyz";
            static string NumbersForString = "0123456789";
            static string EngLettersUpperCase = "ABCDEFGHIJKLMNOPQRSTUWXYZ";


            /// <summary>
            /// Creates string with random capital english letters and numbers <br></br>
            /// Written. 2024.01.14 14:18. Moscow. Hostel <br></br>
            /// Tested. Work. 2024.01.14 14:31. Moscow. Hostel 
            /// </summary>
            /// <param name="length_in"></param>
            /// <returns></returns>

            public static string EN_Letters_UpperCase_Numbers(Int32 length_in)
            {
                string str_out = "";
                string symbols_for_string = EngLettersUpperCase + NumbersForString;
                for (Int32 i = 0; i < length_in; i++)
                {
                    // 2024.01.14 14:30. Moscow. Hostel 
                    // It was - 1 to convert length to index and then +1 because random a <= value < b
                    // to include b. -1 and then +1 requires additional computation so it was removed.
                    Int32 letter_index = _internal_random.Next(0, symbols_for_string.Length);
                    str_out += symbols_for_string[letter_index].ToString();
                }
                return str_out;
            }





            /// <summary>
            /// Written. 2023.07.26. Saint-Petersburg.
            /// Tested. Works. 2024.03.11 01:29. Moscow. Hostel.
            /// </summary>
            /// <param name="length_in"></param>
            /// <returns></returns>
            public static string EN_Letters_UpperCase(Int32 length_in)
            {
                string str_out = "";
                for (Int32 i = 0; i < length_in; i++)
                {
                    // 2024.03.11 01:29. Moscow. Hostel.
                    // Correction.
                    // EngLettersUpperCase.Length + 1 -> EngLettersUpperCase.Length
                    Int32 letter_index = _internal_random.Next(0, EngLettersUpperCase.Length);

                    // 2024.03.11 01:30. Moscow. Hostel.
                    // Correction.
                    // [length_in] -> [letter_index]
                    // note. it looks alike and therefore mistyping occured using auto complete
                    str_out += EngLettersUpperCase[letter_index].ToString();
                }
                return str_out;
            }

            /// <summary>
            /// Written. 2023.07.26. Saint-Petersburg.
            /// </summary>
            /// <param name="length_in"></param>
            /// <returns></returns>
            public static string Numbers(Int32 length_in)
            {
                string str_out = "";
                for (Int32 i = 0; i < length_in; i++)
                {
                    // 2024.03.11 01:32. Moscow. Hostel.
                    // Correction. Correction as in EN_Letters_UpperCase
                    // It was copy/pasted and different array for random was then selected
                    // leaving the same errors of EN_Letters_UpperCase in this function
                    Int32 letter_index = _internal_random.Next(0, NumbersForString.Length);
                    str_out += NumbersForString[letter_index].ToString();
                }
                return str_out;
            }
        }






        /// <summary>
        /// Written. 2023.11.26 04:27. Moscow. Hostel Delta. 
        /// </summary>
        public static class Pad
        {
            /// <summary>
            /// Written. 2023.11.26 04:33. Moscow. Hostel Delta.
            /// Tested. Works. 2023.11.26 10:20. Moscow. Hostel Delta. 
            /// </summary>
            /// <param name="str_in"></param>
            /// <param name="pad_char"></param>
            /// <returns></returns>
            public static string RightEachChar(string str_in, char pad_char)
            {
                StringBuilder str_make = new StringBuilder();
                str_make.Capacity = str_in.Length * 4;
                for (Int32 i = 0; i < str_in.Length; i++)
                {
                    str_make.Append(str_in[i]);
                    str_make.Append(pad_char);
                }
                string str_out = str_make.ToString();
                return str_out;
            }
        }
        /// <summary>
        /// Written. 2023.11.26 03:55. Moscow. Hostel Delta. 
        /// </summary>
        public static class Convert
        {
            /// <summary>
            /// Written. 2023.11.26 04:02. Moscow. Hostel Delta. 
            /// Tested. Works. 2023.11.26 10:33. Moscow. Hostel Delta. 
            /// </summary>
            /// <param name="string_in"></param>
            /// <param name="str_length"></param>
            /// <returns></returns>
            static public string[] StringToStrings(string string_in, Int32 str_length)
            {
                string[] arr_out = null;
                Int32 str_num = string_in.Length / str_length;
                if ((string_in.Length % str_length) != 0)
                {
                    arr_out = new string[str_num + 1];
                }
                else
                {
                    arr_out = new string[str_num];
                }
                if (str_num == 0)
                {
                    return new string[1] { string_in };
                }
                char[] char_arr = string_in.ToCharArray();
                for (Int32 i = 0; i < str_num; i++)
                {
                    arr_out[i] = new string(char_arr, i * str_length, str_length);
                }
                if ((string_in.Length % str_length) != 0)
                {
                    arr_out[arr_out.Length - 1] = new string(char_arr, (str_num - 1) * str_length, char_arr.Length - 1);
                }
                return arr_out;
            }
            /// <summary>
            /// 2023.11.26 03:40. Moscow. Hostel Delta. Moves from MyFileFunctions.FileStringToStrings
            /// to MyStringFunctions.FileStringToStrings
            /// not tested
            /// </summary>
            /// <param name="string_in"></param>
            /// <returns></returns>
            static public string[] FileStringToStrings(string string_in)
            {
                char char_arr_for_split = '\n';
                string[] str_arr = string_in.Replace("\r", "").Split(char_arr_for_split);
                string[] arr_out = new string[str_arr.Length - 1];
                Array.Copy(str_arr, arr_out, arr_out.Length);
                return arr_out;
            }
        }
        public static string English_Letters_lower_case = "abcdefghijklmnopqrstuvwxyz";
        /// <summary>
        /// 
        /// </summary>
        /// <param name="char_in"></param>
        /// <param name="shift_in"></param>
        /// <param name="letters_order"></param>
        /// <returns></returns>
        public static char NextCharCyclically(char char_in, Int32 shift_in, string letters_order = "abcdefghijklmnopqrstuvwxyz")
        {
            // 2023-07-26 12:46 Tested. It works.
            char char_out = char_in;
            Int32 char_index = letters_order.IndexOf(char_in);
            if (char_index == -1)
            {
                MyReportFunctions.ReportError("Letter is not in letters order string\r\n" +
                    "Letter: " + char_in.ToString() + "\r\n" +
                    "Letters order string: " + letters_order);
                return letters_order[0];
            }
            // reminder of full cylces should be used
            // if shift is more 1 cycle (2-3 length for example)
            // 2023-07-26 12:12.
            if (System.Math.Abs(shift_in) / letters_order.Length > 0)
            {
                bool shift_neg = false;
                if (shift_in < 0)
                {
                    shift_neg = true;
                }
                System.Math.DivRem(System.Math.Abs(shift_in), letters_order.Length, out shift_in);
                if (shift_neg == true)
                {
                    shift_in = -shift_in;
                }
            }
            if (shift_in >= 0)
            {
                if ((char_index + shift_in) < letters_order.Length)
                {
                    char_out = letters_order[char_index + shift_in];
                }
                else
                {
                    Int32 diff = (char_index + shift_in) - (letters_order.Length - 1);
                    // 2023-07-26 12:05
                    // 1 add will be used to make 0
                    diff -= 1;
                    char_index = diff;
                    char_out = letters_order[char_index];
                }
            }
            else
            {
                if ((char_index + shift_in) >= 0)
                {
                    char_out = letters_order[char_index + shift_in];
                }
                else
                {
                    Int32 diff = 0 - shift_in;
                    // 2023-07-26 12:05
                    // 1 sub will be used to make max
                    diff -= 1;
                    char_index = letters_order.Length - 1 - diff;
                    char_out = letters_order[char_index];
                }
            }
            return char_out;
        }
        static class TestFunction
        {
            /// <summary>
            /// Test passed.
            /// </summary>
            public static void NextCharCyclically_Test()
            {
                for (Int32 i = 0; i < StringFunctions.English_Letters_lower_case.Length * 3; i++)
                {
                    Console.WriteLine(StringFunctions.English_Letters_lower_case);
                    Console.WriteLine(StringFunctions.NextCharCyclically('d', -i));
                }
            }
        }
        public static class SymbolCounting
        {
            /// <summary>
            /// 2023.10.31 15:16. Moscow. Workplace. Written.
            /// </summary>
            public static class SupportFunctions
            {
                /// <summary>
                /// Converts SymbolData[] to Dictionary &lt;char,int&gt; <br></br>
                /// Written. 2023.10.31 15:18. Moscow. Workplace. <br></br>
                /// Tested. Works. 2023.10.31 15:57. Moscow. Workplace. 
                /// </summary>
                /// <param name="arr_in"></param>
                /// <returns></returns>
                public static Dictionary<char, int> SymbolDataArrayToDictionary(SymbolData[] arr_in)
                {
                    if (arr_in.Length == 0)
                    {
                        MyReportFunctions.ReportAttention(MyReportFunctions.AttentionMessage.ArrayZeroLength);
                        return new Dictionary<char, int>();
                    }
                    Dictionary<char, int> dict_out = new Dictionary<char, int>();
                    for (Int32 i = 0; i < arr_in.Length; i++)
                    {
                        dict_out.Add(arr_in[i].Symbol, arr_in[i].Count);
                    }
                    return dict_out;
                }
                /// <summary>
                /// Converts Dictionary &lt;char,int&gt; to SymbolData[]  <br></br>
                /// Written. 2023.10.31 16:13. Moscow. Workplace.  <br></br>
                /// Tested. Works. 2023.10.31 16:21. Moscow. Workplace. 
                /// </summary>
                /// <param name="dict_in"></param>
                /// <returns></returns>
                public static SymbolData[] DictionaryToSymbolDataArray(Dictionary<char, int> dict_in)
                {
                    if (dict_in.Count == 0)
                    {
                        MyReportFunctions.ReportAttention(MyReportFunctions.AttentionMessage.ArrayZeroLength);
                        return new SymbolData[0];
                    }
                    SymbolData[] arr_out = new SymbolData[dict_in.Count];
                    for (Int32 i = 0; i < arr_out.Length; i++)
                    {
                        arr_out[i] = new SymbolData();
                        arr_out[i].Symbol = dict_in.ElementAt(i).Key;
                        arr_out[i].Count = dict_in.ElementAt(i).Value;
                    }
                    return arr_out;
                }
            }
            /// <summary>
            /// 2023.10.06 10:01. Written. Moscow. <br></br>
            /// 2023.10.06 10:12. Tested. Works.
            /// </summary>
            /// <param name="str_in"></param>
            /// <returns></returns>
            public static char[] SymbolsInString(string str_in)
            {
                char[] symbols_list = new char[0];
                if (str_in.Length == 0)
                {
                    MyReportFunctions.ReportAttention(MyReportFunctions.AttentionMessage.StringZeroLength);
                    return symbols_list;
                }
                char[] symbols_in_str = str_in.ToCharArray();
                for (Int32 i = 0; i < symbols_in_str.Length; i++)
                {
                    if (symbols_list.Contains(symbols_in_str[i]) == false)
                    {
                        Array.Resize(ref symbols_list, symbols_list.Length + 1);
                        symbols_list[symbols_list.Length - 1] = symbols_in_str[i];
                    }
                }
                return symbols_list;
            }
            /// 2023.10.05 23:00. Written. <br></br>
            /// Moscow. Hostel. Вежливый лось. Метро. Ботанический сад. 
            /// </summary>
            public class SymbolData
            {
                public Int32 Count = 0;
                public char Symbol = '\0';
            }
            /// <summary>
            /// 2023.10.06 09:50. Written. Moscow. <br></br>
            /// 2023.10.06 10:15. Tested. Works.
            /// </summary>
            /// <param name="str_in"></param>
            /// <param name="symbol_in"></param>
            /// <returns></returns>
            public static SymbolData Symbol(string str_in, char symbol_in)
            {
                SymbolData data_out = new SymbolData();
                data_out.Count = 0;
                data_out.Symbol = symbol_in;
                if (str_in.Length == 0)
                {
                    MyReportFunctions.ReportAttention(MyReportFunctions.AttentionMessage.StringZeroLength);
                    return data_out;
                }
                char[] symbols_in_str = str_in.ToCharArray();
                for (Int32 i = 0; i < symbols_in_str.Length; i++)
                {
                    if (symbols_in_str[i] == symbol_in)
                    {
                        data_out.Count += 1;
                    }
                }
                return data_out;
            }
            /// <summary>
            /// 2023.10.06 11:00. Written. Moscow. <br></br>
            /// 2023.10.06 11:04. Tested. Works.
            /// </summary>
            /// <param name="arr_in"></param>
            public static void ToConsole(SymbolData[] arr_in)
            {
                Console.WriteLine("SymbolCount Array " + arr_in.Length.ToString());
                for (Int32 i = 0; i < arr_in.Length; i++)
                {
                    if ((arr_in[i].Symbol != '\r') &&
                             (arr_in[i].Symbol != '\n') &&
                             (arr_in[i].Symbol != '\t') &&
                             (arr_in[i].Symbol != '\0'))
                    {
                        Console.WriteLine(arr_in[i].Symbol.ToString().PadRight(5, ' ') +
                            ((int)arr_in[i].Symbol).ToString().PadRight(5, ' ') + "".PadRight(3, ' ') + arr_in[i].Count.ToString());
                    }
                    else
                    {
                        Console.WriteLine(CharFunctions.Convert.SpecialCharacterToString(arr_in[i].Symbol).PadRight(5, ' ') +
                           ((int)arr_in[i].Symbol).ToString().PadRight(5, ' ') + "".PadRight(3, ' ') + arr_in[i].Count.ToString());
                    }
                }
            }
            /// <summary>
            /// 2023.10.06 11:18. Written. Moscow. Workplace <br></br> 
            /// 2023.10.06 11:21. Tested. Works. Moscow. Workplace 
            /// </summary>
            /// <param name="data_in"></param>
            public static void ToConsole(SymbolData data_in)
            {
                Console.WriteLine("SymbolData: " + "'" + data_in.Symbol.ToString() + "'" + " " + data_in.Count.ToString());
            }
            /// <summary>
            /// Written. 2023.11.03 22:42. Moscow. <br></br>
            /// Tested. Work. 2023.11.05 17:35. St. Peterburg. Home.  <br></br>
            /// note. char is converted to Int32 because in the file \n brakes the lines in the file
            /// </summary>
            /// <param name="arr_in"></param>
            /// <param name="filepath"></param>
            /// <param name="delimer"></param>
            /*
            example for note.
            a 5
            c 4
                    here is \n
              4
            d 2
            e 4
            any other no letter, no digit symbol I expect will do the same.
            */
            public static void ToFile(SymbolData[] arr_in, string filepath, char delimer = ' ')
            {
                if (arr_in.Length == 0)
                {
                    MyReportFunctions.ReportAttention(MyReportFunctions.AttentionMessage.ArrayZeroLength);
                    return;
                }
                string str_for_file = "";
                for (Int32 i = 0; i < arr_in.Length; i++)
                {
                    str_for_file += System.Convert.ToInt32(arr_in[i].Symbol).ToString() + delimer.ToString() + arr_in[i].Count.ToString();
                    str_for_file += "\r\n";
                }
                FileFunctions.TextFile.WriteFile.FileStringToFile(str_for_file, filepath);
            }
            /// <summary>
            /// 2023.10.06 12:13. written.
            /// 2023.10.06 12:13. not tested.
            /// </summary>
            public static class Distribution
            {
                /// <summary>
                /// Written. 2023.10.31 14:56. Moscow. Workplace жд. ст. Мытищи <br></br>
                /// Tested. Works. 2023.11.05 12:00. St. Peterburg. 
                /// </summary>
                /// <param name="filepath"></param>
                /// <returns></returns>
                public static SymbolData[] FromFileList(string[] filelist, bool all_files_exists = true)
                {
                    if (all_files_exists == true)
                    {
                        for (Int32 i = 0; i < filelist.Length; i++)
                        {
                            string filepath = filelist[i];
                            if (System.IO.File.Exists(filepath) == false)
                            {
                                MyReportFunctions.ReportError("File does not exists. File " + filepath);
                                return new SymbolData[0];
                            }
                        }
                    }
                    SymbolData[][] arr_with_symbol_data = new SymbolData[filelist.Length][];
                    Dictionary<char, int>[] arr_dict = new Dictionary<char, int>[filelist.Length];
                    // Getting symbol distribution from each file. 2023.10.31 15:01. Moscow. Workplace. 
                    for (Int32 i = 0; i < filelist.Length; i++)
                    {
                        arr_with_symbol_data[i] = FromFile(filelist[i]);
                        arr_dict[i] = SupportFunctions.SymbolDataArrayToDictionary(arr_with_symbol_data[i]);
                    }
                    // Sum the results. 2023.11.03 22:21. Moscow. Workplace. 
                    Dictionary<char, int> sum_dic = new Dictionary<char, int>();
                    for (Int32 i = 0; i < filelist.Length; i++)
                    {
                        for (Int32 j = 0; j < arr_dict[i].Count; j++)
                        {
                            if (sum_dic.ContainsKey(arr_dict[i].ElementAt(j).Key) == true)
                            {
                                sum_dic[arr_dict[i].ElementAt(j).Key] += arr_dict[i].ElementAt(j).Value;
                            }
                            else
                            {
                                sum_dic.Add(arr_dict[i].ElementAt(j).Key, arr_dict[i].ElementAt(j).Value);
                            }
                        }
                    }
                    // Convert to SymbolData[]                
                    SymbolData[] arr_out = SupportFunctions.DictionaryToSymbolDataArray(sum_dic);
                    // 2023.10.31 15:02. Moscow. Workplace. merging results
                    return arr_out;
                }
                /// <summary>
                /// 2023.10.06 13:26. Written. Moscow. Workplace жд. ст. Мытищи <br></br>
                /// 2023.10.06 13:26. Tested. Works.
                /// </summary>
                /// <param name="filepath"></param>
                /// <returns></returns>
                public static SymbolData[] FromFile(string filepath)
                {
                    if (System.IO.File.Exists(filepath) == true)
                    {
                        string file_content = FileFunctions.TextFile.ReadFile.ToFileString(filepath);
                        SymbolData[] arr_out = SymbolCounting.Distribution.FromString(file_content);
                        return arr_out;
                    }
                    else
                    {
                        MyReportFunctions.ReportError("File does not exists. File " + filepath);
                        return new SymbolData[0];
                    }
                }
                /// <summary>
                /// 2023.10.06 11:04. Written. Moscow. <br></br>
                /// 2023.10.06 11:04. Tested. Works.
                /// 
                /// from String
                /// from File
                /// from FileList
                /// 
                /// </summary>
                /// <param name="str_in"></param>
                /// <returns></returns>
                public static SymbolData[] FromString(string str_in)
                {
                    if (str_in.Length == 0)
                    {
                        MyReportFunctions.ReportAttention(MyReportFunctions.AttentionMessage.ArrayZeroLength);
                        return new SymbolData[0];
                    }
                    char[] symbols_str = SymbolsInString(str_in);
                    SymbolData[] arr_out = new SymbolData[symbols_str.Length];
                    for (Int32 i = 0; i < symbols_str.Length; i++)
                    {
                        arr_out[i] = SymbolCounting.Symbol(str_in, symbols_str[i]);
                    }
                    return arr_out;
                }
            }
        }
    }
}
