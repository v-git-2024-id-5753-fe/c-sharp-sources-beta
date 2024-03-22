using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using ReportFunctionsNamespace;
using ArrayFunctionsNamespace;
using DirectoryFunctionsNamespace;
using MathFunctionsNamespace;
using System.Diagnostics;
using System.Reflection;
using System.Drawing;
using System.Windows.Forms;
using ImageFunctionsNameSpace;


// Commercial use (license)
// Please study about commercial use of the code that is publicly available 

namespace FileFunctionsNamespace
{




    /// <summary>
    /// Written. 2023.11.27 09:13. Moscow. Workplace. 
    /// </summary>
    public class FileProtection
    {
        /// <summary>
        /// Written. 2023.11.27 09:13. Moscow. Workplace. 
        /// </summary>
        public class Binary
        {
            /// <summary>
            /// Written. 2023.11.27 09:13. Moscow. Workplace. 
            /// Tested. Works. 2023.11.29 16:30. Moscow. Workplace. 
            /// </summary>
            public class EachByteXOR
            {
                public byte[] FileBytes;
                public byte[] FileXORBytes;
                public string FilePath;
                public string FileXORPath;
                /// <summary>
                /// 2023.11.28 22:30. Moscow. Hostel Дельта. 
                /// In ASCII. 1 byte char is accepted.
                /// </summary>
                public string ProtectionString;
                /// <summary>
                /// Reads file to byte[]. FilePath should be set <br></br>
                /// Tested. Works. 2023.11.30 11:23. Moscow. Workplace. 
                /// </summary>
                public void ReadFile()
                {
                    FileBytes = FileFunctions.TextFile.FileToBytes(FilePath);
                }
                /// <summary>
                /// Reads file to byte[]. Puts filepath into FilePath. <br></br>
                /// Tested. Works. 2023.11.30 11:23. Moscow. Workplace. 
                /// </summary>
                /// <param name="filepath_in"></param>
                public void ReadFile(string filepath_in)
                {
                    FilePath = filepath_in;
                    FileBytes = FileFunctions.TextFile.FileToBytes(FilePath);
                }
                public EachByteXOR()
                {
                }
                NextChar CyclingChar = null;
                /// <summary>
                /// Applies XOR of each byte of the byte[] and byte of Protection String. <br></br>
                /// Written. 2023.11.29 16:03. Moscow. Workplace. <br></br>
                /// Tested. Works. 2023.11.30 12:38. Moscow. Workplace. 
                /// 
                /// </summary>
                public void ConvertFile()
                {
                    FileXORBytes = new byte[FileBytes.Length];
                    CyclingChar = new NextChar(ProtectionString);
                    for (Int32 i = 0; i < FileBytes.Length; i++)
                    {
                        FileXORBytes[i] = (byte)(FileBytes[i] ^ (byte)CyclingChar.Next());
                    }
                }
                public void ConvertFile(string password)
                {
                    ProtectionString = password;
                    ConvertFile();
                }
                /// <summary>
                /// Writes FileXORBytes into file using path FileXORPath. <br></br>
                /// Written. 2023.11.29 16:08. Moscow. Workplace. <br></br>
                /// Tested. Works. 2023.11.30 12:39. Moscow. Workplace. 
                /// </summary>
                public void WriteFile()
                {
                    FileFunctions.TextFile.BytesToFile(FileXORBytes, FileXORPath);
                }
                /// <summary>
                /// Writes FileXORBytes into file using provied path. Puts filepath into FileXORPath. <br></br>
                /// Written. 2023.11.29 16:08. Moscow. Workplace. <br></br>
                /// Tested. Works. 2023.11.30 12:40. Moscow. Workplace. 
                /// </summary>
                public void WriteFile(string filepath)
                {
                    FileFunctions.TextFile.BytesToFile(FileXORBytes, filepath);
                }
                class NextChar
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
        }
    }
    // 2023-07-26 14:33. importance 3. default file path is project location 
    // if the app is started from visual studio otherwise it is
    // C:\Windows\System32 if file is on desktop
    public class RandomString
    {
        public const string Eng_Letters_string_lower_case = "abcdefghijklmnopqrstuvwxyz";
        public const string Numbers_string = "0123456789";
        Random _internal_Random_Number = new Random();
        public string String_Eng_letter(Int32 length_in)
        {
            string str_out = "";
            for (Int32 i = 0; i < length_in; i++)
            {
                Int32 letter_index = _internal_Random_Number.Next(0, Eng_Letters_string_lower_case.Length);
                str_out += Eng_Letters_string_lower_case[length_in].ToString();
            }
            return str_out;
        }
        public string String_Numbers(Int32 length_in)
        {
            string str_out = "";
            for (Int32 i = 0; i < length_in; i++)
            {
                Int32 letter_index = _internal_Random_Number.Next(0, Numbers_string.Length);
                str_out += Numbers_string[length_in].ToString();
            }
            return str_out;
        }
    }
    public class FileCombine
    {
        static public void BytesToFile(byte[] byte_arr, string file_name)
        {
            BytesToFile(byte_arr, file_name);
        }
        public static byte[] MergeParts(byte[][] bytes_in)
        {
            byte[][] data_parts = new byte[bytes_in.Length][];
            for (Int32 i = 0; i < data_parts.Length; i++)
            {
                data_parts[i] = ExtractData(bytes_in[i]);
            }
            return ArrayFunctions.Merge.NxM_To_A(data_parts);
        }
        /// <summary>
        /// Return byte[] accodring to size in the first 4 bytes
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static byte[] ExtractData(byte[] bytes)
        {
            byte[] size_bytes = ArrayFunctions.Extract.AboveIndex(bytes, 4);
            Int32 arr_size = MathFunctions.Int32Number.BytesToInt32(size_bytes);
            byte[] data = ArrayFunctions.Extract.BelowIndex(bytes, 3);
            if (arr_size != data.Length)
            {
                data = ArrayFunctions.Extract.AboveIndex(data, arr_size - 1 + 1);
            }
            return data;
        }
    }
    public class FileSplitter
    {
        /// <summary>
        /// Splits to part with 4 first bytes for size. MSB first 
        /// 2023-07-19 16:10. written.
        /// 2023.09.03 12:16. to do. size in header is optional via bytes_for_size = 0;
        /// 
        /// </summary>
        /// <param name="file_in"></param>
        /// <param name="parts_num"></param>
        /// <returns></returns>
        public static byte[][] Split(string file_in, Int32 parts_num)
        {
            FileStream file_stream = File.Open(file_in, FileMode.Open);
            byte[] bytes = new byte[file_stream.Length];
            file_stream.Read(bytes, 0, bytes.Length);
            file_stream.Close();
            byte[][] arr_out = Split(bytes, parts_num);
            return arr_out;
        }
        /// <summary>
        /// Split to part with 4 first bytes for size. MSB first 
        /// 2023-07-19 16:10
        /// </summary>
        /// <param name="file_bytes"></param>
        /// <param name="parts_num"></param>
        /// <returns></returns>
        public static byte[][] Split(byte[] file_bytes, Int32 parts_num)
        {
            Int32 arr_size = file_bytes.Length / parts_num;
            while (arr_size * parts_num < file_bytes.Length)
            {
                arr_size++;
            }
            byte[][] arr_out = new byte[parts_num][];
            Int32 arr_num = 0;
            Int32 arr_2nd_index = 0;
            arr_out[arr_num] = new byte[arr_size];
            for (Int32 i = 0; i < file_bytes.Length; i++)
            {
                arr_out[arr_num][arr_2nd_index] = file_bytes[i];
                arr_2nd_index++;
                if (arr_2nd_index == arr_size)
                {
                    byte[] arr_size_bytes = MathFunctions.Int32ToBytes(arr_size);
                    arr_out[arr_num] = ArrayFunctions.Merge.A_B_To_C(arr_size_bytes, arr_out[arr_num]);
                    arr_2nd_index = 0;
                    arr_num++;
                    // part filled and size is array length
                    if (arr_num < parts_num)
                    {
                        arr_out[arr_num] = new byte[arr_size];
                    }
                }
            }
            // filled last part
            if (arr_2nd_index != 0)
            {
                arr_2nd_index -= 1; // return used value
                byte[] arr_size_bytes = MathFunctions.Int32ToBytes(arr_2nd_index + 1);
                arr_out[arr_num] = ArrayFunctions.Merge.A_B_To_C(arr_size_bytes, arr_out[arr_num]);
            }
            return arr_out;
        }
    }
    /// <summary>
    /// 2023.10.06 10:07. Moved to MyStringFunctions
    /// </summary>
    [Obsolete]
    public class StringFunctionsObsolete
    {
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
                ReportFunctions.ReportError("Letter is not in letters order string\r\n" +
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
                for (Int32 i = 0; i < StringFunctionsObsolete.English_Letters_lower_case.Length * 3; i++)
                {
                    Console.WriteLine(StringFunctionsObsolete.English_Letters_lower_case);
                    Console.WriteLine(StringFunctionsObsolete.NextCharCyclically('d', -i));
                }
            }
        }
    }
    /// <summary>
    /// 2023.10.06 10:08. Moved to MyStringFunctions
    /// </summary>
    [Obsolete]
    class StringCoding
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
    public class FileCoding
    {
        Random RandomNumber = new Random();
        public void FileClear(string filename, bool console_out = true)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            string file_content = FileFunctions.TextFile.ReadFile.ToFileString(filename);
            List<char> letters_in_file = new List<char>();
            for (Int32 i = 0; i < file_content.Length; i++)
            {
                if (letters_in_file.Contains(file_content[i]) == false)
                {
                    letters_in_file.Add(file_content[i]);
                }
            }
            StringBuilder str_built = new StringBuilder(file_content.Length);
            for (Int32 i = 0; i < file_content.Length; i++)
            {
                char symbol_in = letters_in_file[RandomNumber.Next(0, letters_in_file.Count)];
                str_built.Append(symbol_in);
            }
            FileFunctions.TextFile.WriteFile.FileStringToFile(str_built.ToString(), filename);
            FileFunctions.FileDelete(filename);
            sw.Stop();
            if (console_out == true)
            {
                Console.WriteLine("File deleted in " + sw.ElapsedMilliseconds.ToString() + "".PadRight(5, ' ') +
                    "\r\n" + "File: " + filename);
            }
        }
        public class InternalFunctions
        {
            /// <summary>
            /// XOR coding using codeword
            /// written 2023-07-18 12:18
            /// </summary>
            /// <param name="file_in"></param>
            /// <param name="file_out"></param>
            /// <param name="password_in"></param>
            /// <returns></returns>
            static public bool FileToCodedFile(string file_in, string file_out, string password_in)
            {
                bool for_return = true;
                if (System.IO.File.Exists(file_in) == true)
                {
                    string temp_file_name = FileFunctions.FileName.New.Get(file_in, "_temp");
                    FileFunctions.FileDelete(temp_file_name);
                    System.IO.File.Copy(file_in, temp_file_name);
                    StreamReader file_read = new StreamReader(temp_file_name);
                    string file_content = file_read.ReadToEnd();
                    file_read.Close();
                    FileFunctions.FileDelete(temp_file_name);
                    Int32 password_index = 0;
                    string file_content_out = "";
                    for (Int32 i = 0; i < file_content.Length; i++)
                    {
                        UInt16 char_code_num = file_content[i];
                        if (char_code_num != password_in[password_index])
                        {
                            char_code_num ^= (UInt16)password_in[password_index];
                        }
                        file_content_out += (System.Convert.ToChar(char_code_num)).ToString();
                        password_index++;
                        if (password_index > password_in.Length - 1)
                        {
                            password_index = 0;
                        }
                    }
                    FileFunctions.FileDelete(file_out);
                    StreamWriter file_write = new StreamWriter(file_out);
                    file_write.Write(file_content_out);
                    file_write.Close();
                }
                else
                {
                    ReportFunctions.ReportError();
                    for_return = false;
                }
                return for_return;
            }
            /// <summary>
            /// XOR decoding using codeword
            /// written 2023-07-18 12:18
            /// </summary>
            /// <param name="str_in"></param>
            /// <param name="password_in"></param>
            /// <returns></returns>
            static public string StringCodedToString(string str_in, string password_in)
            {
                // with XOR coding, it requires  to do the same with coded string to decode it
                return StringToCodedString(str_in, password_in);
            }
            /// <summary>
            /// XOR coding using codeword
            /// written 2023-07-18 12:18
            /// </summary>
            /// <param name="str_in"></param>
            /// <param name="password_in"></param>
            /// <returns></returns>
            static public string StringToCodedString(string str_in, string password_in)
            {
                Int32 password_index = 0;
                string str_out = "";
                for (Int32 i = 0; i < str_in.Length; i++)
                {
                    UInt16 char_code_num = str_in[i];
                    if (char_code_num != password_in[password_index])
                    {
                        char_code_num ^= (UInt16)password_in[password_index];
                    }
                    str_out += (System.Convert.ToChar(char_code_num)).ToString();
                    password_index++;
                    if (password_index > password_in.Length - 1)
                    {
                        password_index = 0;
                    }
                }
                return str_out;
            }
        }
        public static void Codeword(string file_in, string file_out, string password_in)
        {
            InternalFunctions.FileToCodedFile(file_in, file_out, password_in);
        }
        public enum ActionType
        {
            DoNothing,
            Code,
            Decode,
            CheckCoded
        }
        /// <summary>
        /// Replaces 1st letter. Does not do it if the word is Datetime.
        /// Skips several words
        /// </summary>
        /// <param name="action_apply"></param>
        /// <param name="file_in"></param>
        /// <param name="file_out"></param>
        /// <param name="password_in"></param>
        public static void Strength_1_type_1b(ActionType action_apply, string file_in, string file_out, string password_in, Int32 skip_words = 5)
        {
            byte[] password_values = UTF8Encoding.UTF8.GetBytes(password_in);
            Int32 average_password = password_values.Sum(select_method);
            average_password = average_password / password_values.Length;
            Int32 select_method(byte num_in)
            {
                return (int)num_in;
            }
            if (System.IO.File.Exists(file_in) == true)
            {
                string temp_file_name = FileFunctions.FileName.New.Get(file_in, "_temp");
                FileFunctions.FileDelete(temp_file_name);
                System.IO.File.Copy(file_in, temp_file_name);
                StreamReader file_read = new StreamReader(temp_file_name);
                string file_content = file_read.ReadToEnd();
                file_read.Close();
                FileFunctions.FileDelete(temp_file_name);
                TextProcessingClass text_processing = new TextProcessingClass();
                text_processing.Text = file_content;
                Int32 password_index = 0;
                string file_content_out = "";
                text_processing.StartTextBuilding();
                Int32 words_skipped_count = 0;
                while (text_processing.EndOfWords == false)
                {
                    TextProcessingClass.TextWord word = text_processing.NextWord();
                    words_skipped_count += 1;
                    if (words_skipped_count < skip_words)
                    {
                        text_processing.AppendText(word.Text);
                        continue;
                    }
                    else
                    {
                        words_skipped_count = 0;
                    }
                    char[] word_srt = word.Text.ToArray();
                    if ((TextProcessingClass.IsWordDate(word) == false) &&
                             (TextProcessingClass.IsWordTime(word) == false))
                    {
                        if ((byte)password_in[password_index] < average_password)
                        {
                            if (action_apply == ActionType.Code)
                            {
                                word_srt[0] = (char)((byte)word_srt[0] - 1);
                            }
                            if (action_apply == ActionType.Decode)
                            {
                                word_srt[0] = (char)((byte)word_srt[0] + 1);
                            }
                        }
                        else
                        {
                            if (action_apply == ActionType.Code)
                            {
                                word_srt[0] = (char)((byte)word_srt[0] + 1);
                            }
                            if (action_apply == ActionType.Decode)
                            {
                                word_srt[0] = (char)((byte)word_srt[0] - 1);
                            }
                        }
                    }
                    password_index++;
                    if (password_index > password_in.Length - 1)
                    {
                        password_index = 0;
                    }
                    string word_str_append = new string(word_srt);
                    text_processing.AppendText(word_str_append);
                }
                file_content_out = text_processing.EndTextBuilding();
                FileFunctions.FileDelete(file_out);
                StreamWriter file_write = new StreamWriter(file_out);
                file_write.Write(file_content_out);
                file_write.Close();
            }
            else
            {
                ReportFunctions.ReportError();
            }
        }
        /// <summary>
        /// Replaces 1st letter. Does not do it if the word is Datetime or Time
        /// 
        /// 2023-07-27 16:08 Tested it works.
        /// </summary>
        /// <param name="action_apply"></param>
        /// <param name="file_in"></param>
        /// <param name="file_out"></param>
        /// <param name="password_in"></param>
        public static void Strength_1_type_1a(ActionType action_apply, string file_in, string file_out, string password_in)
        {
            byte[] password_values = UTF8Encoding.UTF8.GetBytes(password_in);
            Int32 average_password = password_values.Sum(select_method);
            average_password = average_password / password_values.Length;
            Int32 select_method(byte num_in)
            {
                return (int)num_in;
            }
            if (System.IO.File.Exists(file_in) == true)
            {
                string temp_file_name = FileFunctions.FileName.New.Get(file_in, "_temp");
                FileFunctions.FileDelete(temp_file_name);
                System.IO.File.Copy(file_in, temp_file_name);
                StreamReader file_read = new StreamReader(temp_file_name);
                string file_content = file_read.ReadToEnd();
                file_read.Close();
                FileFunctions.FileDelete(temp_file_name);
                TextProcessingClass text_processing = new TextProcessingClass();
                text_processing.Text = file_content;
                Int32 password_index = 0;
                string file_content_out = "";
                text_processing.StartTextBuilding();
                Int32 i = 0;
                while (text_processing.EndOfWords == false)
                {
                    i++;
                    TextProcessingClass.TextWord word = text_processing.NextWord();
                    char[] word_srt = word.Text.ToArray();
                    if ((TextProcessingClass.IsWordDate(word) == false) &&
                             (TextProcessingClass.IsWordTime(word) == false))
                    {
                        if ((byte)password_in[password_index] < average_password)
                        {
                            if (action_apply == ActionType.Code)
                            {
                                word_srt[0] = (char)((byte)word_srt[0] - 1);
                            }
                            if (action_apply == ActionType.Decode)
                            {
                                word_srt[0] = (char)((byte)word_srt[0] + 1);
                            }
                        }
                        else
                        {
                            if (action_apply == ActionType.Code)
                            {
                                word_srt[0] = (char)((byte)word_srt[0] + 1);
                            }
                            if (action_apply == ActionType.Decode)
                            {
                                word_srt[0] = (char)((byte)word_srt[0] - 1);
                            }
                        }
                    }
                    password_index++;
                    if (password_index > password_in.Length - 1)
                    {
                        password_index = 0;
                    }
                    string word_str_append = new string(word_srt);
                    text_processing.AppendText(word_str_append);
                }
                file_content_out = text_processing.EndTextBuilding();
                FileFunctions.FileDelete(file_out);
                StreamWriter file_write = new StreamWriter(file_out);
                file_write.Write(file_content_out);
                file_write.Close();
            }
            else
            {
                ReportFunctions.ReportError();
            }
        }
        /// <summary>
        /// Replaces letter in the middle. Does not do it if the word is Datetime
        /// </summary>
        /// <param name="action_apply"></param>
        /// <param name="file_in"></param>
        /// <param name="file_out"></param>
        /// <param name="password_in"></param>
        public static void Strength_1_type_2a(ActionType action_apply, string file_in, string file_out, string password_in)
        {
            byte[] password_values = UTF8Encoding.UTF8.GetBytes(password_in);
            Int32 average_password = password_values.Sum(select_method);
            average_password = average_password / password_values.Length;
            Int32 select_method(byte num_in)
            {
                return (int)num_in;
            }
            if (System.IO.File.Exists(file_in) == true)
            {
                string temp_file_name = FileFunctions.FileName.New.Get(file_in, "_temp");
                FileFunctions.FileDelete(temp_file_name);
                System.IO.File.Copy(file_in, temp_file_name);
                StreamReader file_read = new StreamReader(temp_file_name);
                string file_content = file_read.ReadToEnd();
                file_read.Close();
                FileFunctions.FileDelete(temp_file_name);
                TextProcessingClass text_processing = new TextProcessingClass();
                text_processing.Text = file_content;
                Int32 password_index = 0;
                string file_content_out = "";
                text_processing.StartTextBuilding();
                while (text_processing.EndOfWords == false)
                {
                    TextProcessingClass.TextWord word = new TextProcessingClass.TextWord();
                    word = text_processing.NextWord();
                    char[] word_srt = word.Text.ToArray();
                    Int32 letter_change = word_srt.Length / 2;
                    if ((TextProcessingClass.IsWordDate(word) == false) &&
                            (TextProcessingClass.IsWordTime(word) == false))
                    {
                        if ((byte)password_in[password_index] < average_password)
                        {
                            if (action_apply == ActionType.Code)
                            {
                                word_srt[letter_change] = (char)((byte)word_srt[letter_change] - 1);
                            }
                            if (action_apply == ActionType.Decode)
                            {
                                word_srt[letter_change] = (char)((byte)word_srt[letter_change] + 1);
                            }
                        }
                        else
                        {
                            if (action_apply == ActionType.Code)
                            {
                                word_srt[letter_change] = (char)((byte)word_srt[letter_change] + 1);
                            }
                            if (action_apply == ActionType.Decode)
                            {
                                word_srt[letter_change] = (char)((byte)word_srt[letter_change] - 1);
                            }
                        }
                    }
                    password_index++;
                    if (password_index >= password_in.Length - 1)
                    {
                        password_index = 0;
                    }
                    string word_str_append = new string(word_srt);
                    text_processing.AppendText(word_str_append);
                    if ((TextProcessingClass.IsWordDate(word) == false) &&
                            (TextProcessingClass.IsWordTime(word) == false))
                    {
                        if ((TextProcessingClass.IsWordDate(word_str_append) == true) ||
                                (TextProcessingClass.IsWordTime(word_str_append) == true))
                        {
                            string dir = DirectoryFunctions.DirectoryPath.File(file_in);
                            string filename = Path.GetFileName(dir);
                            DirectoryFunctions.Make(dir);
                            File.Copy(file_in, dir + "\\" + filename);
                            Console.WriteLine("Critical. File was not coded");
                            Console.WriteLine(file_in);
                            Console.WriteLine("From " + word.Text);
                            Console.WriteLine("To " + word_str_append);
                            text_processing.EndTextBuilding();
                            return;
                        }
                    }
                }
                file_content_out = text_processing.EndTextBuilding();
                FileFunctions.FileDelete(file_out);
                StreamWriter file_write = new StreamWriter(file_out);
                file_write.Write(file_content_out);
                file_write.Close();
            }
            else
            {
                ReportFunctions.ReportError();
            }
        }
        /// <summary>
        /// Replaces letter in the middle. Does not do it if the word is Datetime
        /// skips several words
        /// </summary>
        /// <param name="action_apply"></param>
        /// <param name="file_in"></param>
        /// <param name="file_out"></param>
        /// <param name="password_in"></param>
        public static void Strength_1_type_2b(ActionType action_apply, string file_in, string file_out, string password_in)
        {
            byte[] password_values = UTF8Encoding.UTF8.GetBytes(password_in);
            Int32 average_password = password_values.Sum(select_method);
            average_password = average_password / password_values.Length;
            Int32 select_method(byte num_in)
            {
                return (int)num_in;
            }
            if (System.IO.File.Exists(file_in) == true)
            {
                string temp_file_name = FileFunctions.FileName.New.Get(file_in, "_temp");
                FileFunctions.FileDelete(temp_file_name);
                System.IO.File.Copy(file_in, temp_file_name);
                StreamReader file_read = new StreamReader(temp_file_name);
                string file_content = file_read.ReadToEnd();
                file_read.Close();
                FileFunctions.FileDelete(temp_file_name);
                TextProcessingClass text_processing = new TextProcessingClass();
                text_processing.Text = file_content;
                Int32 password_index = 0;
                string file_content_out = "";
                text_processing.StartTextBuilding();
                while (text_processing.EndOfWords == false)
                {
                    TextProcessingClass.TextWord word = new TextProcessingClass.TextWord();
                    word = text_processing.NextWord();
                    char[] word_srt = word.Text.ToArray();
                    Int32 letter_change = word_srt.Length / 2;
                    if ((TextProcessingClass.IsWordDate(word) == false) &&
                            (TextProcessingClass.IsWordTime(word) == false))
                    {
                        if ((byte)password_in[password_index] < average_password)
                        {
                            if (action_apply == ActionType.Code)
                            {
                                word_srt[letter_change] = (char)((byte)word_srt[letter_change] - 1);
                            }
                            if (action_apply == ActionType.Decode)
                            {
                                word_srt[letter_change] = (char)((byte)word_srt[letter_change] + 1);
                            }
                        }
                        else
                        {
                            if (action_apply == ActionType.Code)
                            {
                                word_srt[letter_change] = (char)((byte)word_srt[letter_change] + 1);
                            }
                            if (action_apply == ActionType.Decode)
                            {
                                word_srt[letter_change] = (char)((byte)word_srt[letter_change] - 1);
                            }
                        }
                    }
                    string word_str_append = new string(word_srt);
                    text_processing.AppendText(word_str_append);
                    Int32 diff_password_average = System.Math.Abs((int)password_in[password_index] - average_password);
                    for (Int32 i = 0; i < diff_password_average; i++)
                    {
                        text_processing.AppendText(text_processing.NextWord().Text);
                    }
                    password_index++;
                    if (password_index >= password_in.Length - 1)
                    {
                        password_index = 0;
                    }
                    if ((TextProcessingClass.IsWordDate(word) == false) &&
                            (TextProcessingClass.IsWordTime(word) == false))
                    {
                        if ((TextProcessingClass.IsWordDate(word) == true) ||
                            (TextProcessingClass.IsWordTime(word) == true))
                        {
                            string dir = DirectoryFunctions.DirectoryPath.File(file_in);
                            string filename = Path.GetFileName(dir);
                            DirectoryFunctions.Make(dir);
                            File.Copy(file_in, dir + "\\" + filename);
                            Console.WriteLine("Critical. File was not coded");
                            Console.WriteLine(file_in);
                            Console.WriteLine("From " + word.Text);
                            Console.WriteLine("To " + word_str_append);
                            text_processing.EndTextBuilding();
                            return;
                        }
                    }
                }
                file_content_out = text_processing.EndTextBuilding();
                FileFunctions.FileDelete(file_out);
                StreamWriter file_write = new StreamWriter(file_out);
                file_write.Write(file_content_out);
                file_write.Close();
            }
            else
            {
                ReportFunctions.ReportError();
            }
        }
        /// <summary>
        /// Tested it works.
        /// 2023-07-27 16:05
        /// </summary>
        /// <param name="action_apply"></param>
        /// <param name="file_in"></param>
        /// <param name="file_out"></param>
        /// <param name="password_in"></param>
        /// <returns></returns>
        public static bool Replace1stLetter_PasswordIn_1a(ActionType action_apply, string file_in, string file_out, string password_in)
        {
            bool result_out = false;
            int[] password_values = ArrayFunctions.Int32Array.ConvertToInt32(password_in.ToCharArray());
            Int32 average_password = ArrayFunctions.Int32Array.Average(password_values);
            if (System.IO.File.Exists(file_in) == true)
            {
                // 2023-07-26 15:47 read temp file.
                string temp_file_name = FileFunctions.TextFile.Filepath(Path.GetFileName(file_in));
                //temp_file_name = MyFileFunctions.FileName.New.Get(temp_file_name, "temp");
                FileFunctions.FileDelete(temp_file_name);
                System.IO.File.Copy(file_in, temp_file_name);
                // end of read temp file.
                if (action_apply == ActionType.Decode)
                {
                    StreamReader file_read = new StreamReader(temp_file_name);
                    string file_content = file_read.ReadToEnd();
                    file_read.Close();
                    FileFunctions.FileDelete(temp_file_name);
                    TextProcessingClass text_processing = new TextProcessingClass();
                    text_processing.Text = file_content;
                    Int32 password_index = 0;
                    TextProcessingClass.TextWord word = null;
                    text_processing.StartTextBuilding();
                    // 2023-07-26 15:56
                    // Length+1. 5 letters. 4 spaces to go. +1 at start, +1 at end
                    // 2023-07-26 16:10 for decode
                    string password_read_from_file = "";
                    if (text_processing.WordsCount >= (password_in.Length + 1))
                    {
                        for (Int32 p_index = 0; p_index < password_in.Length; p_index++)
                        {
                            Int32 letters_space = text_processing.WordsCount / (password_in.Length + 1);
                            for (Int32 i = 0; i < letters_space; i++)
                            {
                                text_processing.AppendText(text_processing.NextWord().Text);
                            }
                            word = text_processing.NextWord();
                            string word_str_append = "";
                            //if (action_apply == ActionType.Code)
                            //{
                            //    word_str_append = word.Text + password_in[password_index].ToString();
                            //}
                            password_read_from_file += word.Text[word.Text.Length - 1].ToString();
                            word_str_append = word.Text.Substring(0, word.Text.Length - 1);
                            text_processing.AppendText(word_str_append);
                            password_index++;
                            if (password_index > password_in.Length - 1)
                            {
                                password_index = 0;
                            }
                        }
                        // 2023-07-26 15:50 finish file
                        while (text_processing.EndOfWords == false)
                        {
                            word = text_processing.NextWord();
                            text_processing.AppendText(word.Text);
                        }
                        // 2023-07-26 16:13 protected file found.
                        if (password_read_from_file == password_in)
                        {
                            result_out = true;
                            string file_content_process = text_processing.EndTextBuilding();
                            string file_content_path = FileFunctions.TextFile.Filepath(Path.GetFileName(file_in));
                            FileFunctions.FileDelete(temp_file_name);
                            StreamWriter file_write = new StreamWriter(temp_file_name);
                            file_write.Write(file_content_process);
                            file_write.Close();
                            Strength_1_type_1a(ActionType.Decode, temp_file_name, temp_file_name, password_in);
                            FileFunctions.FileDelete(file_out);
                            System.IO.File.Copy(temp_file_name, file_out);
                        }
                        else
                        {
                            Console.WriteLine(DateTime.Now.ToString() + "".PadRight(5, ' ') + "File was not created by this application");
                        }
                    }
                }
                if (action_apply == ActionType.Code)
                {
                    Strength_1_type_1a(ActionType.Code, temp_file_name, temp_file_name, password_in);
                    StreamReader file_read = new StreamReader(temp_file_name);
                    string file_content = file_read.ReadToEnd();
                    file_read.Close();
                    FileFunctions.FileDelete(temp_file_name);
                    TextProcessingClass text_processing = new TextProcessingClass();
                    text_processing.Text = file_content;
                    Int32 password_index = 0;
                    TextProcessingClass.TextWord word = null;
                    text_processing.StartTextBuilding();
                    // 2023-07-26 15:56
                    // Length+1. 5 letters. 4 spaces to go. +1 at start, +1 at end
                    // 2023-07-26 16:10 for decode
                    if (text_processing.WordsCount >= (password_in.Length + 1))
                    {
                        for (Int32 p_index = 0; p_index < password_in.Length; p_index++)
                        {
                            Int32 letters_space = text_processing.WordsCount / (password_in.Length + 1);
                            for (Int32 i = 0; i < letters_space; i++)
                            {
                                text_processing.AppendText(text_processing.NextWord().Text);
                            }
                            word = text_processing.NextWord();
                            string word_str_append = "";
                            word_str_append = word.Text + password_in[password_index].ToString();
                            text_processing.AppendText(word_str_append);
                            password_index++;
                            if (password_index > password_in.Length - 1)
                            {
                                password_index = 0;
                            }
                        }
                        // 2023-07-27 13:59 finish file
                        while (text_processing.EndOfWords == false)
                        {
                            word = text_processing.NextWord();
                            text_processing.AppendText(word.Text);
                        }
                        string file_content_process = text_processing.EndTextBuilding();
                        StreamWriter file_write = new StreamWriter(temp_file_name);
                        file_write.Write(file_content_process);
                        file_write.Close();
                        FileFunctions.FileDelete(file_out);
                        System.IO.File.Copy(temp_file_name, file_out);
                        result_out = true;
                    }
                }
            }
            else
            {
                ReportFunctions.ReportError();
            }
            return result_out;
        }
        /// <summary>
        /// Checked it works.
        /// 2023-07-27 17:19
        /// </summary>
        /// <param name="action_apply"></param>
        /// <param name="file_in"></param>
        /// <param name="file_out"></param>
        /// <param name="password_in"></param>
        /// <param name="skip_words"></param>
        /// <returns></returns>
        public static bool Replace1stLetter_PasswordIn_1b(ActionType action_apply, string file_in, string file_out, string password_in, Int32 skip_words = 5)
        {
            bool result_out = false;
            int[] password_values = ArrayFunctions.Int32Array.ConvertToInt32(password_in.ToCharArray());
            Int32 average_password = ArrayFunctions.Int32Array.Average(password_values);
            if (System.IO.File.Exists(file_in) == true)
            {
                // 2023-07-26 15:47 read temp file.
                string temp_file_name = FileFunctions.TextFile.Filepath(Path.GetFileName(file_in));
                //temp_file_name = MyFileFunctions.FileName.New.Get(temp_file_name, "temp");
                FileFunctions.FileDelete(temp_file_name);
                System.IO.File.Copy(file_in, temp_file_name);
                // end of read temp file.
                if (action_apply == ActionType.Decode)
                {
                    StreamReader file_read = new StreamReader(temp_file_name);
                    string file_content = file_read.ReadToEnd();
                    file_read.Close();
                    FileFunctions.FileDelete(temp_file_name);
                    TextProcessingClass text_processing = new TextProcessingClass();
                    text_processing.Text = file_content;
                    Int32 password_index = 0;
                    TextProcessingClass.TextWord word = null;
                    text_processing.StartTextBuilding();
                    // 2023-07-26 15:56
                    // Length+1. 5 letters. 4 spaces to go. +1 at start, +1 at end
                    // 2023-07-26 16:10 for decode
                    string password_read_from_file = "";
                    if (text_processing.WordsCount >= (password_in.Length + 1))
                    {
                        for (Int32 p_index = 0; p_index < password_in.Length; p_index++)
                        {
                            if (p_index == 7)
                            {
                                Int32 a = 0;
                            }
                            Int32 letters_space = text_processing.WordsCount / (password_in.Length);
                            for (Int32 i = 0; i < letters_space - 1; i++)
                            {
                                TextProcessingClass.TextWord word_skip = text_processing.NextWord();
                                text_processing.AppendText(word_skip.Text);
                            }
                            word = text_processing.NextWord();
                            string word_str_append = "";
                            // 2023.7.27 19:53 additional shift 
                            char char_password = word.Text[word.Text.Length - 1];
                            // 1 or 2 by reminder
                            Int32 add_1_2_to_char = 1;
                            if (password_in[password_index] >= average_password)
                            {
                                char_password = (char)((int)char_password + add_1_2_to_char);
                                add_1_2_to_char++;
                            }
                            else
                            {
                                char_password = (char)((int)char_password - add_1_2_to_char);
                                add_1_2_to_char++;
                            }
                            if (add_1_2_to_char > 2)
                            {
                                add_1_2_to_char = 1;
                            }
                            password_read_from_file += char_password.ToString();
                            word_str_append = word.Text.Substring(0, word.Text.Length - 1);
                            text_processing.AppendText(word_str_append);
                            password_index++;
                            if (password_index > password_in.Length - 1)
                            {
                                password_index = 0;
                            }
                        }
                        // 2023-07-26 15:50 finish file
                        while (text_processing.EndOfWords == false)
                        {
                            word = text_processing.NextWord();
                            text_processing.AppendText(word.Text);
                        }
                        // 2023-07-26 16:13 protected file found.
                        if (password_read_from_file == password_in)
                        {
                            result_out = true;
                            string file_content_process = text_processing.EndTextBuilding();
                            string file_content_path = FileFunctions.TextFile.Filepath(Path.GetFileName(file_in));
                            FileFunctions.FileDelete(temp_file_name);
                            StreamWriter file_write = new StreamWriter(temp_file_name);
                            file_write.Write(file_content_process);
                            file_write.Close();
                            Strength_1_type_1b(ActionType.Decode, temp_file_name, temp_file_name, password_in, skip_words);
                            FileFunctions.FileDelete(file_out);
                            System.IO.File.Copy(temp_file_name, file_out);
                        }
                        else
                        {
                            Console.WriteLine(DateTime.Now.ToString() + "".PadRight(5, ' ') + "File was not created by this application");
                        }
                    }
                }
                bool _internal_check_coded()
                {
                    bool check_result_out = false;
                    StreamReader file_read = new StreamReader(temp_file_name);
                    string file_content = file_read.ReadToEnd();
                    file_read.Close();
                    TextProcessingClass text_processing = new TextProcessingClass();
                    text_processing.Text = file_content;
                    Int32 password_index = 0;
                    TextProcessingClass.TextWord word = null;
                    text_processing.StartTextBuilding();
                    // 2023-07-26 15:56
                    // Length+1. 5 letters. 4 spaces to go. +1 at start, +1 at end
                    // 2023-07-26 16:10 for decode
                    string password_read_from_file = "";
                    if (text_processing.WordsCount >= (password_in.Length + 1))
                    {
                        for (Int32 p_index = 0; p_index < password_in.Length; p_index++)
                        {
                            Int32 letters_space = text_processing.WordsCount / (password_in.Length);
                            for (Int32 i = 0; i < letters_space - 1; i++)
                            {
                                TextProcessingClass.TextWord word_skip = text_processing.NextWord();
                                text_processing.AppendText(word_skip.Text);
                            }
                            word = text_processing.NextWord();
                            string word_str_append = "";
                            // 2023.7.27 19:53 additional shift 
                            char char_password = word.Text[word.Text.Length - 1];
                            // 1 or 2 by reminder
                            Int32 add_1_2_to_char = 1;
                            if (password_in[password_index] >= average_password)
                            {
                                char_password = (char)((int)char_password + add_1_2_to_char);
                                add_1_2_to_char++;
                            }
                            else
                            {
                                char_password = (char)((int)char_password - add_1_2_to_char);
                                add_1_2_to_char++;
                            }
                            if (add_1_2_to_char > 2)
                            {
                                add_1_2_to_char = 1;
                            }
                            password_read_from_file += char_password.ToString();
                            word_str_append = word.Text.Substring(0, word.Text.Length - 1);
                            text_processing.AppendText(word_str_append);
                            password_index++;
                            if (password_index > password_in.Length - 1)
                            {
                                password_index = 0;
                            }
                        }
                        // 2023-07-26 15:50 finish file
                        while (text_processing.EndOfWords == false)
                        {
                            word = text_processing.NextWord();
                            text_processing.AppendText(word.Text);
                        }
                        // 2023-07-26 16:13 protected file found.
                        if (password_read_from_file == password_in)
                        {
                            check_result_out = true;
                        }
                    }
                    return check_result_out;
                }
                if (action_apply == ActionType.Code)
                {
                    if (_internal_check_coded() == true)
                    {
                        result_out = false;
                        Console.WriteLine(DateTime.Now.ToString() + "".PadRight(5, ' ') + "File is ok. There is no action needed");
                        return result_out;
                    }
                    Strength_1_type_1b(ActionType.Code, temp_file_name, temp_file_name, password_in, skip_words);
                    StreamReader file_read = new StreamReader(temp_file_name);
                    string file_content = file_read.ReadToEnd();
                    file_read.Close();
                    FileFunctions.FileDelete(temp_file_name);
                    TextProcessingClass text_processing = new TextProcessingClass();
                    text_processing.Text = file_content;
                    Int32 password_index = 0;
                    TextProcessingClass.TextWord word = null;
                    text_processing.StartTextBuilding();
                    // 2023-07-26 15:56
                    // Length+1. 5 letters. 4 spaces to go. +1 at start, +1 at end
                    // 2023-07-26 16:10 for decode
                    if (text_processing.WordsCount >= (password_in.Length + 1))
                    {
                        for (Int32 p_index = 0; p_index < password_in.Length; p_index++)
                        {
                            Int32 letters_space = text_processing.WordsCount / (password_in.Length);
                            for (Int32 i = 0; i < letters_space - 1; i++)
                            {
                                text_processing.AppendText(text_processing.NextWord().Text);
                            }
                            word = text_processing.NextWord();
                            string word_str_append = "";
                            // 2023.7.27 20:01 additional shift 
                            char char_password = password_in[password_index];
                            // 1 or 2 by reminder
                            Int32 add_1_2_to_char = 1;
                            if (password_in[password_index] >= average_password)
                            {
                                char_password = (char)((int)char_password - add_1_2_to_char);
                                add_1_2_to_char++;
                            }
                            else
                            {
                                char_password = (char)((int)char_password + add_1_2_to_char);
                                add_1_2_to_char++;
                            }
                            if (add_1_2_to_char > 2)
                            {
                                add_1_2_to_char = 1;
                            }
                            word_str_append = word.Text + char_password.ToString();
                            text_processing.AppendText(word_str_append);
                            password_index++;
                            if (password_index > password_in.Length - 1)
                            {
                                password_index = 0;
                            }
                        }
                        // 2023-07-27 13:59 finish file
                        while (text_processing.EndOfWords == false)
                        {
                            word = text_processing.NextWord();
                            text_processing.AppendText(word.Text);
                        }
                        string file_content_process = text_processing.EndTextBuilding();
                        StreamWriter file_write = new StreamWriter(temp_file_name);
                        file_write.Write(file_content_process);
                        file_write.Close();
                        FileFunctions.FileDelete(file_out);
                        System.IO.File.Copy(temp_file_name, file_out);
                        result_out = true;
                    }
                }
            }
            else
            {
                ReportFunctions.ReportError();
            }
            return result_out;
        }
    }
    static class DirectoryMyMethods
    {
        static public void Make(string dir_path)
        {
            if (System.IO.Directory.Exists(dir_path) == false)
            {
                System.IO.Directory.CreateDirectory(dir_path);
            }
        }
        static public void Delete(string dir_path)
        {
            if (System.IO.Directory.Exists(dir_path) == true)
            {
                System.IO.Directory.Delete(dir_path);
            }
        }
    }
    public class TextProcessingClass
    {
        public class TextWord
        {
            public string Text = "";
            public Int32 StartIndex = 0;
            public Int32 EndIndex = 0;
            public Int32 Length = 0;
            public TextWord()
            {
            }
        }
        public static bool IsWordDate(string str_in)
        {
            DateTime date = new DateTime();
            Int32 date_str_length = 10;
            if (DateTime.TryParse(str_in, out date) == true)
            {
                if (str_in.Length == date_str_length)
                {
                    return true;
                }
            }
            return false;
        }
        public static bool IsWordDate(TextWord word_in)
        {
            DateTime date = new DateTime();
            // 2023-07-27 15:15 there were 1. 11.29.2022. with dot in the end
            Int32 date_str_length = 10;
            if (DateTime.TryParse(word_in.Text, out date) == true)
            {
                if (word_in.Text.Length == date_str_length)
                {
                    return true;
                }
                // 2023-07-27 15:19 added . and , check
                if (word_in.Text.Length == (date_str_length + 1))
                {
                    if ((word_in.Text[word_in.Text.Length - 1] == '.') ||
                             (word_in.Text[word_in.Text.Length - 1] == ','))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public static bool IsWordTime(TextWord word_in)
        {
            DateTime date = new DateTime();
            Int32 time_str_length = 5;
            if (DateTime.TryParse(word_in.Text, out date) == true)
            {
                if (word_in.Text.Length == time_str_length)
                {
                    if (word_in.Text.Contains(':') == true)
                    {
                        return true;
                    }
                }
                // 2023-07-27 15:20 added . and , check
                if (word_in.Text.Length == (time_str_length + 1))
                {
                    if ((word_in.Text[word_in.Text.Length - 1] == '.') ||
                             (word_in.Text[word_in.Text.Length - 1] == ','))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public static bool IsWordTime(string str_in)
        {
            DateTime date = new DateTime();
            Int32 time_str_length = 5;
            if (DateTime.TryParse(str_in, out date) == true)
            {
                if (str_in.Length == time_str_length)
                {
                    if (str_in.Contains(':') == true)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public static bool IsWordIntNumber(string str_in)
        {
            try
            {
                System.Convert.ToInt32(str_in);
            }
            catch
            {
                return false;
            }
            return true;
        }
        string _text_loaded = "";
        public string Text
        {
            get
            {
                return _text_loaded;
            }
            set
            {
                _text_loaded = value;
                while (EndOfWords == false)
                {
                    //Console.WriteLine(NextWord().Text);
                    NextWord();
                    _words_count++;
                }
                _index_next_word = 0;
                EndOfWords = false;
            }
        }
        Int32 _index_next_word = 0;
        StringBuilder _string_builder = null;
        bool _string_builder_started = false;
        /// <summary>
        /// 2023-07-27 17:20. Currently trim the end of text - \r, \n, \t
        /// </summary>
        public void StartTextBuilding()
        {
            _index_next_word = 0;
            EndOfWords = false;
            _string_builder_started = true;
            _string_builder = new StringBuilder();
        }
        public void AppendText(string word_in)
        {
            _string_builder.Append(word_in);
            // Console.Clear();
            // Console.Write(_string_builder.ToString());
        }
        /// <summary>
        /// Returns string which is currently made and ends text building
        /// </summary>
        /// <returns></returns>
        public string EndTextBuilding()
        {
            _index_next_word = 0;
            _string_builder_started = false;
            string str_out = _string_builder.ToString();
            _string_builder.Clear();
            _string_builder = null;
            return str_out;
        }
        public bool EndOfWords = false;
        TextWord _word_found = null;
        public enum WordTypeEnum
        {
            NoType,
            Text,
            Number,
            Date,
            Time
        }
        public WordTypeEnum WordType = WordTypeEnum.NoType;
        WordTypeEnum WordTypeGet(TextWord word_in)
        {
            DateTime datetime = new DateTime();
            if (DateTime.TryParse(word_in.Text, out datetime) == true)
            {
                return WordTypeEnum.Date;
            }
            return WordTypeEnum.NoType;
        }
        Int32 _words_count = 0;
        public Int32 WordsCount
        {
            get
            {
                return _words_count;
            }
        }
        public TextWord NextWord()
        {
            TextWord word_return = new TextWord();
            if (EndOfWords == true)
            {
                word_return.StartIndex = -1;
                word_return.EndIndex = -1;
                word_return.Length = 0;
                return word_return;
            }
            for (Int32 i = _index_next_word; i < Text.Length; i++)
            {
                _index_next_word = i;
                if ((Text[i] == ' ') ||
                    (Text[i] == '\t') ||
                    (Text[i] == '\r') ||
                    (Text[i] == '\n'))
                //(Text[i] == '\0'))
                {
                    if (_string_builder_started == true)
                    {
                        _string_builder.Append(Text[i]);
                    }
                    continue;
                }
                word_return.StartIndex = i;
                break;
            }
            string word_str = "";
            for (Int32 i = _index_next_word; i < Text.Length; i++)
            {
                _index_next_word = i;
                if ((Text[i] == ' ') ||
                    (Text[i] == '\t') ||
                    (Text[i] == '\r') ||
                    (Text[i] == '\n'))
                // (Text[i] == '\0'))
                {
                    break;
                }
                word_str += Text[i].ToString();
            }
            word_return.EndIndex = _index_next_word - 1;
            if (_index_next_word == Text.Length - 1)
            {
                word_return.EndIndex = _index_next_word - 1;
            }
            word_return.Length = word_str.Length;
            word_return.Text = word_str;
            bool check_end = true;
            for (Int32 i = _index_next_word; i < Text.Length; i++)
            {
                if (_index_next_word == (Text.Length - 1))
                {
                    check_end = true;
                    break;
                }
                if ((Text[i] == ' ') ||
                    (Text[i] == '\t') ||
                    (Text[i] == '\r') ||
                    (Text[i] == '\n'))
                //(Text[i] == '\0'))
                {
                }
                else
                {
                    check_end = false;
                    break;
                }
            }
            if (check_end == true)
            {
                EndOfWords = true;
            }
            _word_found = word_return;
            return word_return;
        }
    }
    /// <summary>
    /// 2022.12.29 written.
    /// </summary>
    /// <remarks>
    /// 2023.09.03 12:54. Naming functions. There are notes. Go to definition. <br></br>
    /// </remarks>
    /// 
    /// With not a lot of information FileSize in MyFileFunctions looked to be ok
    /// becuase MyFileFunctions may be used via using therefore there is no need to type it.
    /// 
    /// With MyFileFuctions it is ok Size, Read and not FileSize, FileRead.
    /// I also need to show which function is my. the names still look not like in-built C# but close.
    /// in MyArrayFunction there is Int32Array, ByteArray which is not in-built while
    /// Read, Size is in-built names for usage. Size is used, Read is used.
    /// 
    /// 
    /// 
    static class FileFunctions
    {
        public static Random _internal_random = new Random();




        // template for code execution. 2024.03.08 13:36. Moscow. Hostel.
        /*
        float execution_time_ms_start = 0;
        if (TimeExecutionShow == true)
            {
                execution_time_ms_start = (float)_time_execution.Elapsed.TotalMilliseconds;
            }
        if (TimeExecutionShow == true)
            {
                float execution_time_ms_stop = (float)_time_execution.Elapsed.TotalMilliseconds;
                TimeExecutionMessage(nameof(function_name_here), execution_time_ms_stop - execution_time_ms_start);
            }
        */



        /// <summary>
        /// Written. 2024.03.08 13:36. Moscow. Hostel.
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
       /// Added. 2024.03.08 13:37. Moscow. Hostel.
       /// </summary>
       /// <param name="function_name"></param>
       /// <param name="total_ms_passed"></param>
        static void TimeExecutionMessage(string function_name, float total_ms_passed)
        {
            _time_execution_count += 1;
            Console.WriteLine(_time_execution_count.ToString() + ". " + DateTime.Now.ToString("HH:mm:ss") + " " + function_name +
                " exectuion time: " + total_ms_passed.ToString("0.000") + " ms");
        }






        /// <summary>
        /// Written. 2024.03.08 11:35. Moscow. Hostel.
        /// </summary>
        public static class ExeFile
        {

            /// <summary>        
            /// It is marked obsolete so it can be used accidentally. <br></br>
            /// Use copy code and paste. It is safer. <br></br>
            /// Written. 2024.03.08 11:29. Moscow. Hostel. <br></br> 
            /// Tested. Works. The code and via function call. <br></br>
            /// Note that .exe in that moment should not be in use. <br></br>
            /// Close() works because there is delay of 3 seconds before deletion.
            /// </summary>
            [Obsolete]
            public static void DeleteItself()
            {
                ProcessStartInfo Info = new ProcessStartInfo();
                Info.Arguments = "/C choice /C Y /N /D Y /T 3 & Del " +
                               Application.ExecutablePath;
                Info.WindowStyle = ProcessWindowStyle.Hidden;
                Info.CreateNoWindow = true;
                Info.FileName = "cmd.exe";
                Process.Start(Info);
            }
        }




        /// <summary>
        /// Written. 2024.01.14 12:50. 2024.01.14 12:50. Moscow. Hostel 
        /// </summary>
        public static class BinaryFile
        {



            /// <summary>
            /// Written. 2024.01.30 04:08. Moscow. Hostel.
            /// not tested
            /// </summary>
            /// <param name="size_in_Mb"></param>
            /// <returns></returns>
            public static long MBytesToBytes(Int32 size_in_Mb)
            {
                long size_1MB_in_bytes = 1024 * 1024;
                long size_out = size_1MB_in_bytes * (long)size_in_Mb;
                return size_out;
            }


            /// <summary>
            /// Written. 2024.01.14 21:27. Moscow. Hostel.
            /// not tested.
            /// </summary>
            /// <param name="size_in_kb"></param>
            /// <returns></returns>
            public static Int32 KBytesToBytes(Int32 size_in_kb)
            {
                // 2024.01.14 21:28. Moscow. Hostel.
                // There is table using approach.
                // Finding size is done using calculation.
                long size_in_bytes = 1;

                for (Int32 i = 0; i < 10; i++)
                {
                    size_in_bytes = size_in_bytes * 2;
                }

                long size_out = 0;
                for (Int32 i = 0; i < size_in_kb; i++)
                {
                    size_out += size_in_bytes;
                }

                return (int)size_out;

            }


            /// <summary>
            /// Written. 2024.01.14 12:51. Moscow. Hostel
            /// </summary>
            public static class Generate
            {


                /// <summary>
                /// Creates binary file of defined size with random value in each byte <br></br>
                /// Written. 2024.01.14 13:00. Moscow. Hostel <br></br>
                /// Tested. Works. 2024.01.14 13:26. Moscow. Hostel.
                /// </summary>
                /// <param name="file_size"></param>
                /// <param name="file_path"></param>
                public static void FileRandomBytes(UInt32 file_size, string file_path)
                {
                    byte[] bytes_of_file = new byte[file_size];
                    // 2024.03.08 13:35. Moscow. Hostel.
                    // correction. there were bytes_of_file.Length - 1.
                    for (Int32 i = 0; i < bytes_of_file.Length; i++)
                    {
                        bytes_of_file[i] = (byte)_internal_random.Next(0, byte.MaxValue + 1);
                    }

                    FileStream file_write = File.Create(file_path);
                    file_write.Write(bytes_of_file, 0, bytes_of_file.Length);
                    file_write.Close();

                }

                /// <summary>
                /// Creates file of certain size with bytes from 0 to 255. <br></br>
                /// Written. 2024.02.06 16:18. Moscow. Workplace. <br></br>
                /// Tested. Work. 2024.02.06 16:32. Moscow. Workplace. <br></br> 
                /// </summary>
                /// <param name="file_size"></param>
                /// <param name="file_path"></param>
                public static void FileBytes0To255(UInt32 file_size, string file_path)
                {
                    byte[] bytes_of_file = new byte[file_size];
                    Int32 byte_value = 0;
                    for (Int32 i = 0; i < bytes_of_file.Length - 1; i++)
                    {
                        bytes_of_file[i] = (byte)byte_value;
                        byte_value++;
                        if (byte_value > byte.MaxValue)
                        {
                            byte_value = 0;
                        }
                    }

                    FileStream file_write = File.Create(file_path);
                    file_write.Write(bytes_of_file, 0, bytes_of_file.Length);

                    // added Flush() for SD. 2024.02.06 16:42. Moscow. Workplace. 
                    file_write.Flush();
                    file_write.Close();


                }






            }






        }




        /// <summary>
        /// Written. 2024.02.09 20:31. Moscow. Hostel 
        /// </summary>
        public static class ImageFile
        {

            /// <summary>
            /// Written. 2024.03.12 10:36. Moscow. Workplace. 
            /// </summary>
            public static class ReadMultipleFiles
            {
                static public Bitmap[] FilesBMPToBitmapArray(string[] filenames)
                {
                    float execution_time_ms_start = 0;
                    if (TimeExecutionShow == true)
                    {
                        execution_time_ms_start = (float)_time_execution.Elapsed.TotalMilliseconds;
                    }

                    Bitmap[] bitmap_arr_out = new Bitmap[filenames.Length];
                    for (Int32 i = 0; i < filenames.Length; i++)
                    {
                        bitmap_arr_out[i] = ReadFile.FileBMPToBitmap(filenames[i]);
                    }
                    if (TimeExecutionShow == true)
                    {
                        float execution_time_ms_stop = (float)_time_execution.Elapsed.TotalMilliseconds;
                        TimeExecutionMessage(nameof(FilesBMPToBitmapArray), execution_time_ms_stop - execution_time_ms_start);
                    }


                    return bitmap_arr_out;
                }
            }

                /// <summary>
                /// Written. 2024.03.12 10:34. Moscow. Workplace. 
                /// </summary>
                public static class ReadFile
            {
                /// <summary>
                /// Tested. Works. 2024.03.06 16:58. Moscow. Workplace.<br></br>
                /// It keeps file not available for modification. 2024.03.07 07:52. Moscow. Workplace. 
                /// </summary>
                /// <param name="filename"></param>
                /// <returns></returns>
                static public Bitmap FileBMPToBitmap(string filename)
                {
                    if (System.IO.File.Exists(filename) == false)
                        {
                            ReportFunctions.ReportAttention(ReportFunctions.AttentionMessage.FileDoesNotExist);
                            return ImageFunctions.Generate.Rectungular_Checkboard(100, 100);
                        }
                        
                        
                    float execution_time_ms_start = 0;
                    if (TimeExecutionShow == true)
                    {
                        execution_time_ms_start = (float)_time_execution.Elapsed.TotalMilliseconds;
                    }
                    Bitmap bitmap_return = new Bitmap(filename);
                    if (TimeExecutionShow == true)
                    {
                        float execution_time_ms_stop = (float)_time_execution.Elapsed.TotalMilliseconds;
                        TimeExecutionMessage(nameof(FileBMPToBitmap), execution_time_ms_stop - execution_time_ms_start);
                    }
                    return bitmap_return;
                }
            }


                /// <summary>
                /// Written. 2024.02.09 20:33. Moscow. Hostel 
                /// </summary>
                public static class WriteFile
            {
                /// <summary>
                /// Saves Bitmap to file .bmp. <br></br>
                /// Written. 2024.02.09 20:33. Moscow. Hostel. <br></br> 
                /// Tested. 2024.02.10 14:41. Moscow. Hostel. 
                /// </summary>
                /// <param name="bitmap_in"></param>
                /// <param name="filename"></param>
                static public void BitmapToFileBMP(Bitmap bitmap_in, string filename)
                {
                    bitmap_in.Save(filename);
                }
            }

        }










        /// <summary>
        /// 2023.09.09. written.
        /// 2023.09.09 12:43. this will be used I expect with filename.
        /// </summary>
        public static class FilePath
        {
            /// <summary>
            /// Written. 2024.02.17 14:47. Moscow. Hostel.
            /// Tested. Works. 2024.02.17 14:56. Moscow. Hostel.
            /// </summary>
            /// <returns></returns>
            public static string Programm()
            {
                return System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
            }
        }
        /// <summary>
        /// 2023.09.09 12:33. written
        /// </summary>
        public static class FileName
        {
            /// <summary>
            /// 2023.09.09 12:33. written
            /// </summary>
            public static class New
            {
                /// <summary>
                /// Returns filepath with new name
                /// 2023.09.09 12:33. written.
                /// 2023.09.09 12:34. not tested.
                /// </summary>
                /// <param name="file_path">path of file to be used in the filepath of new file</param>
                /// <param name="new_name"></param>
                /// <returns></returns>
                static public string Get(string file_path_or_name, string new_name)
                {
                    string str_to_return = "";
                    string folder_of_file = Path.GetDirectoryName(file_path_or_name) + "\\";
                    str_to_return = folder_of_file + new_name;
                    return str_to_return;
                }
            }

            /// <summary>
            /// Written. 2024.02.17 14:47. Moscow. Hostel.
            /// Tested. Works. 2024.02.17 14:56. Moscow. Hostel.
            /// </summary>
            /// <returns></returns>
            public static string Programm(bool no_extension = true)
            {
                string filepath = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
                return FileName.Get(filepath, no_extension);
            }


            /// <summary>
            /// Return filename with or without extension. 
            /// 2023.09.09 12:18. tested.
            /// </summary>
            /// <param name="file_path_or_name"></param>
            /// <param name="no_extension">true - no extension, false - with extension</param>
            /// <returns></returns>
            static public string Get(string file_path_or_name, bool no_extension = true)
            {
                string str_to_return = "";
                string[] str_arr_for_split = new string[0];
                str_arr_for_split = file_path_or_name.Split('\\');
                str_to_return = str_arr_for_split[str_arr_for_split.Length - 1];
                if (no_extension == true)
                {
                    try
                    {
                        str_to_return = str_to_return.Replace(Path.GetExtension(file_path_or_name), "");
                    }
                    catch
                    {
                        ReportFunctions.ReportError("Extension of file is null");
                    }
                }

                return str_to_return;
            }
        }
        [Obsolete]
        // 2023.08.26 10:47
        // use Filename.Get
        static public string FilenameGet(string file_path, bool no_txt_end = true)
        {
            return FileName.Get(file_path, no_txt_end);
        }

        /// <summary>
        /// Adds to filename additional string and returns full filepath<br></br>
        /// Tested. Works. 2023.11.30 11:05. Moscow. Workplace. 
        /// </summary>
        /// <param name="file_path"></param>
        /// <param name="add_at_end"></param>
        /// <returns></returns>
        static public string FilenameNew(string file_path, string add_at_end)
        {
            string str_to_return = "";
            // Int32 to_cut = 0;
            if (System.IO.File.Exists(file_path) == true)
            {
                //for (Int32 i = file_path.Length - 1; i >= 0; i--)
                //{
                //    if (file_path[i] != '.')
                //    {
                //        to_cut++;
                //    }
                //    else
                //    {
                //        to_cut++;
                //        break;
                //    }
                //}
                string file_extension = Path.GetExtension(file_path);
                // Note. 2023.11.30 11:08. Moscow. Workplace. 
                // extension contain '.' in the string so there is no need to add '.' to the filepath
                str_to_return = file_path.Substring(0, file_path.Length - file_extension.Length);
                str_to_return = str_to_return + add_at_end + file_extension;
            }
            else
            {
                ReportFunctions.ReportError();
            }
            return str_to_return;
        }
        /// <summary>
        /// 2022.12.29. written.
        /// 2022.12.29. tested. works.
        /// </summary>
        /// <param name="file_path"></param>
        static public void FileDelete(string file_path)
        {
            // 2023.09.05 22:24. note added.
            // checking if file exist is needed to avoid error filepath = "" 
            // important. System.IO.File.Delete is not good to use without checking filepath
            // the need. processing data leads to some files are not needed
            // and the path is "".
            // File may have been deleted somewhere previously and therefore function may return
            // filepath = ""
            if (System.IO.File.Exists(file_path) == true)
            {
                System.IO.File.Delete(file_path);
            }
        }



      

        /// <summary>
        /// Deletes file and changes bytes to random bytes before deletetion.
        /// Written. 2024.03.08 13:19. Moscow. Hostel. <Br></Br>
        /// Tested. Works. 2024.03.08 11:08. Moscow. Hostel. <br></br>
        /// Note. Tested with .txt, .bmp
        /// </summary>
        /// <param name="file_path"></param>
        static public void FileDeleteSecured(string file_path)
        {


            if (System.IO.File.Exists(file_path) == false)
            {
                ReportFunctions.ReportAttention(ReportFunctions.AttentionMessage.WrongPath);
                return;
            }

            float execution_time_ms_start = 0;
            if (TimeExecutionShow == true)
            {
                execution_time_ms_start = (float)_time_execution.Elapsed.TotalMilliseconds;
            }


           

            System.IO.FileStream file_read = File.Open(file_path, FileMode.Open);
            long filesize = file_read.Length;
            file_read.Close();


            byte[] bytes_of_file = new byte[filesize];
            for (Int32 i = 0; i < bytes_of_file.Length; i++)
            {
                bytes_of_file[i] = (byte)_internal_random.Next(0, byte.MaxValue + 1);
            }
            // it overwrites file. 2024.03.08 11:10. Moscow. Hostel.
            // note. it uses the same location of file for new file.
            System.IO.FileStream file_write = File.Create(file_path);

            file_write.Write(bytes_of_file, 0, bytes_of_file.Length);
            file_write.Close();
            FileDelete(file_path);

            if (TimeExecutionShow == true)
            {
                float execution_time_ms_stop = (float)_time_execution.Elapsed.TotalMilliseconds;
                TimeExecutionMessage(nameof(FileDeleteSecured), execution_time_ms_stop - execution_time_ms_start);
            }


        }


























        public static class TextFile
        {


            static public void MergeFiles(string file_1_path, string file_2_path)
            {
                string file_content = "";
                file_content = ReadFile.ToFileString(file_1_path);
                file_content += "\r\n";
                file_content += ReadFile.ToFileString(file_2_path);
                WriteFile.FileStringToFile(file_content, file_1_path);
            }


            /// <summary>
            /// 2023.09.27 16:47. written
            /// </summary>
            public static class Generate
            {
                /// <summary>
                /// 
                /// ASCII. 33 to 126
                /// </summary>
                /// <param name="size_kb"></param>
                public static void FileTXT(Int32 size_kb, string filename)
                {
                    Int32 chars_num = size_kb * 1000;
                    chars_num = chars_num / 1;
                    StringBuilder string_make = new StringBuilder();
                    for (Int32 i = 0; i < chars_num; i++)
                    {
                        string_make.Append((char)_internal_random.Next(33, 127));
                    }
                    string str_out = string_make.ToString();
                    FileFunctions.TextFile.WriteFile.StringsToFile(new string[1] { str_out }, filename);
                }
            }
            /// <summary>
            /// 2023.09.03 13:11. written
            /// </summary>
            public static class FileSize
            {
                /// <summary>
                /// 2022.12.29. written. <br></br> 
                /// 2022.12.29. tested. works.
                /// </summary>
                /// 
                /// notes.
                /// 2023.09.03 13:12. moved to here from MyFileFunctions.FileSizeGet.
                /// 
                /// <param name="file_path"></param>
                /// <returns>Number of file lines. by "\r\n" ending <br></br>
                /// if the line does not have \r\n it will be counted if it is not empty.
                /// 
                /// 
                /// </returns>          
                public static Int32 Lines(string file_path)
                {
                    StreamReader file_read = new StreamReader(file_path);
                    Int32 lines_num = 0;
                    while (file_read.EndOfStream == false)
                    {
                        lines_num++;
                        file_read.ReadLine();
                    }
                    file_read.Close();
                    return lines_num;
                }
            }
            /// <summary>
            /// 2023.09.05 21:32. written.
            /// </summary>
            // 2023.09.05 22:07. notes about naming the class.
            // there is name Write and WriteFile.
            // Write is shorter and because it is MyFileFunctions
            // I understand it about writing to file but
            // during usage there is 1/2 of thinking that Write can be to something else than to file
            //
            // 1. write something (bytes) into image file and it is file but not txt.
            // 2. write letters or string into larger string. importance 2. 
            // 3. write numbers into file. importance 2.
            // 4. importance 3. Write is something to use and it works and it is not serious
            // while WriteFile makes me to be serious about it. Data to be written should be correct
            // and writing to file should be done without any correction after that.
            // 5. 2023.09.05 22:16. Write is used during work of programm when there is 
            // writing done in one place, in another places. Writing of numbers, strings.
            // There is different expectation from Write.
            public static class WriteFile
            {
                /// <summary>
                /// 
                /// note. 2023.11.03 22:46. Moscow.
                /// the name FileString may look like it has difficult something
                /// analysing work with string shows that there is no in the 
                /// functions usage of string that has \r\n used 4-5 times.
                /// readfile does not give \r\n after reading using 
                /// readall (read to end) or readline.
                /// 
                /// importance 3. filestring is not fully custom data type.
                /// it uses string for work and therefore all functions working with string
                /// are avalialble to be used.
                /// </summary>
                /// <param name="string_in"></param>
                /// <param name="file_path"></param>
                static public void FileStringToFile(string string_in, string file_path)
                {
                    FileDelete(file_path);
                    StreamWriter file_write = new StreamWriter(file_path);
                    file_write.Write(string_in);
                    file_write.Close();
                }

                /// <summary>
                /// Written. 2024.01.29 17:22. Moscow. Workplace.
                /// 
                /// </summary>
                public static class AddStrings
                {
                    /// <summary>
                    /// Adds strings to the start of file. <br></br>
                    /// Written. 2024.01.29 17:24. Moscow. Workplace. <br></br>
                    /// Tested. Works. 2024.01.29 17:39.  Moscow. Workplace.
                    /// </summary>
                    /// <param name="strings_in"></param>
                    /// <param name="file_path"></param>
                    public static void ToStart(string[] strings_in, string file_path)
                    {
                        string[] filelines = ReadFile.ToStrings(file_path);
                        string[] str_in_file = new string[filelines.Length + strings_in.Length];
                        if (filelines.Length > 0)
                        {
                            Array.Copy(filelines, 0, str_in_file, strings_in.Length - 1 + 1, filelines.Length);
                        }
                        Array.Copy(strings_in, str_in_file, strings_in.Length);
                        WriteFile.StringsToFile(str_in_file, file_path);
                    }

                    /// <summary>
                    /// Adds string to the end of file. <br></br>
                    /// Tested. Works. 2024.02.16 14:07. Moscow. Workplace. <br></br>
                    /// Mofidied. added amount of strings to add. 2024.02.16 14:08. Moscow. Workplace. <br></br>
                    /// </summary>
                    /// <param name="string_in"></param>
                    /// <param name="file_path"></param>
                    public static void ToEnd(string[] strings_in, string file_path, Int32 amount_add = -1)
                    {
                        if (amount_add == 0)
                        {
                            ReportFunctions.ReportAttention(ReportFunctions.AttentionMessage.NumberIsZero);
                            return;
                        }

                        string[] filelines = ReadFile.ToStrings(file_path);
                        string[] str_in_file = null;

                        if (amount_add == -1)
                        {
                            str_in_file = new string[filelines.Length + strings_in.Length];
                        }
                        else
                        {
                            str_in_file = new string[filelines.Length + amount_add];
                        }

                        if (filelines.Length > 0)
                        {
                            Array.Copy(filelines, str_in_file, filelines.Length);
                        }

                        if (amount_add == -1)
                        {
                            Array.Copy(strings_in, 0, str_in_file, filelines.Length - 1 + 1, strings_in.Length);
                        }
                        else
                        {
                            Array.Copy(strings_in, 0, str_in_file, filelines.Length - 1 + 1, amount_add);
                        }

                        WriteFile.StringsToFile(str_in_file, file_path);
                    }

                    /// <summary>
                    /// 
                    /// 
                    /// </summary>
                    /// <param name="string_in"></param>
                    /// <param name="line_number">this is not index</param>
                    /// <param name="file_path"></param>
                    public static void AfterLine(string string_in, UInt32 line_number, string file_path)
                    {
                        string[] filelines = ReadFile.ToStrings(file_path);
                        if (line_number > filelines.Length)
                        {
                            ReportFunctions.ReportError(ReportFunctions.ErrorMessage.Length_is_exceeded);
                            return;
                        }
                        string[] lines_to_file = new string[filelines.Length + 1];
                        Array.Copy(filelines, lines_to_file, line_number);
                        // 2023.09.08 19:14. line number is number to copy
                        lines_to_file[line_number - 1 + 1] = string_in;
                        Array.Copy(
                            filelines, line_number - 1 + 1,
                            lines_to_file, line_number - 1 + 1 + 1, filelines.Length - line_number);
                        // 2023.09.08 19:15.  index of line number, +1 is the added line, +1 empty line
                        WriteFile.StringsToFile(lines_to_file, file_path);
                    }
                }



                /// <summary>
                /// written. 2023.09.09 11:38.
                /// note. addstring and addstrings or alone addstrings with 1 string to add.
                /// </summary>
                public static class AddString
                {
                    /// <summary>
                    /// Adds string to the start of file.
                    /// 2023.09.09 11:15. written. <br></br>
                    /// 2023.09.09 11:33. tested. works.
                    /// </summary>
                    /// <param name="string_in"></param>
                    /// <param name="file_path"></param>
                    public static void ToStart(string string_in, string file_path)
                    {
                        string[] filelines = ReadFile.ToStrings(file_path);
                        string[] str_in_file = new string[filelines.Length + 1];
                        if (filelines.Length > 0)
                        {
                            Array.Copy(filelines, 0, str_in_file, 1, filelines.Length);
                        }
                        str_in_file[0] = string_in;
                        WriteFile.StringsToFile(str_in_file, file_path);
                    }
                    /// <summary>
                    /// Adds string to the end of file
                    /// 2023.09.09 11:17. written
                    /// 2023.09.09 11:30. tested. works.
                    /// </summary>
                    /// <param name="string_in"></param>
                    /// <param name="file_path"></param>
                    public static void ToEnd(string string_in, string file_path)
                    {
                        string[] filelines = ReadFile.ToStrings(file_path);
                        string[] str_in_file = new string[filelines.Length + 1];
                        if (filelines.Length > 0)
                        {
                            Array.Copy(filelines, str_in_file, filelines.Length);
                        }
                        str_in_file[str_in_file.Length - 1] = string_in;
                        WriteFile.StringsToFile(str_in_file, file_path);
                    }
                    /// <summary>
                    /// 2023.09.08 19:23. written <br></br>
                    /// 2023.09.09 11:24. tested. works.
                    /// </summary>
                    /// <param name="string_in"></param>
                    /// <param name="line_number">this is not index</param>
                    /// <param name="file_path"></param>
                    public static void AfterLine(string string_in, UInt32 line_number, string file_path)
                    {
                        string[] filelines = ReadFile.ToStrings(file_path);
                        if (line_number > filelines.Length)
                        {
                            ReportFunctions.ReportError(ReportFunctions.ErrorMessage.Length_is_exceeded);
                            return;
                        }
                        string[] lines_to_file = new string[filelines.Length + 1];
                        Array.Copy(filelines, lines_to_file, line_number);
                        // 2023.09.08 19:14. line number is number to copy
                        lines_to_file[line_number - 1 + 1] = string_in;
                        Array.Copy(
                            filelines, line_number - 1 + 1,
                            lines_to_file, line_number - 1 + 1 + 1, filelines.Length - line_number);
                        // 2023.09.08 19:15.  index of line number, +1 is the added line, +1 empty line
                        WriteFile.StringsToFile(lines_to_file, file_path);
                    }
                }
                /// <summary>
                /// Writes String[][] to file using delimer between strings in the row. <br></br>
                /// Written. 2023.12.20 16:20. Workplace. <br></br>
                /// Tested. Works. 2023.12.21 12:54. Workplace.
                /// </summary>
                /// <param name="strings_in"></param>
                /// <param name="filename"></param>
                /// <param name="delimer_in"></param>
                static public void StringsAxBToFile(string[][] strings_in, string filename, char delimer_in)
                {
                    string filestring = ArrayFunctions.StringArray.Convert.StringsAxBToFilestring(strings_in, delimer_in);
                    FileFunctions.TextFile.WriteFile.FileStringToFile(filestring, filename);
                }
                /// <summary>
                /// 2022.12.29. Writen. <br></br>
                /// 2022.12.29. Tested. Works.<br></br>
                /// <br></br>
                /// Note. via WriteLine so \r\n at the end of each string. 2023.11.26 10:37. Moscow. Hostel Дельта. 
                /// </summary>
                /// <param name="strings_in"></param>
                /// <param name="filename"></param>
                static public void StringsToFile(string[] strings_in, string filename)
                {
                    // 2023.09.06 20:32. note on FileDelete. 
                    // StreamWriter creates new file when overwrites file but 
                    // it may have been fixed.
                    //
                    // importance 3. noticed during programming txt file
                    // consisting from old and new text if the new text is shorter then old text.
                    FileDelete(filename);
                    StreamWriter file_writer = new StreamWriter(filename);
                    for (Int32 i = 0; i < strings_in.Length; i++)
                    {
                        file_writer.WriteLine(strings_in[i]);
                        // 2023.09.05 21:39. code now adds \r\n to the last line
                        // because line end with \r\n
                        // not \r\n was used because of split function that makes 1 more line and it is empty
                        /*
                        if (i != 0)
                        {
                            file_writer.Write("\r\n");
                        }
                        file_writer.Write(strings_in[i]);
                        */
                    }
                    file_writer.Close();
                }
            }
            /// <summary>
            /// 2023.09.03 12:29. written
            /// </summary>
            public static class ReadFile
            {
                /// <summary>
                /// Reads file to int[][] array <br></br>
                /// 2023.10.10 14:22. written. Workplace  <br></br>
                /// 2023.10.10 14:23. not tested
                /// </summary>
                /// <param name="file_path"></param>
                /// <returns></returns>
                static public int[][] ToInt32Array(string file_path, char delimer)
                {
                    // 1. get number of lines of file
                    Int32 file_size = FileSizeGet(file_path);
                    // 2. make array size of amount of lines
                    string[] str_arr = new string[file_size];
                    // 3. read files into array
                    StreamReader file_read = new StreamReader(file_path);
                    Int32 string_index = 0;
                    while (file_read.EndOfStream == false)
                    {
                        str_arr[string_index] = file_read.ReadLine();
                        string_index++;
                    }
                    file_read.Close();
                    // 4. string[] to string[][] by delimer
                    string[][] str_arr_split = ArrayFunctions.StringArray.Convert.ToStringArrayNxM(str_arr, delimer);
                    // 5. convert filelines into int[][] array splitting lines by delimer
                    int[][] arr_out = ArrayFunctions.StringArray.Convert.ToInt32Array(str_arr_split);
                    // 6. return int[][]
                    return arr_out;
                }
                /// <summary>
                /// Reads file and puts file lines into strings <br></br>
                /// 2022.12.29. written. <br></br>
                /// 2022.12.29. tested. works.
                /// </summary>
                /// <param name="file_path"></param>
                /// <returns></returns>
                static public string[] ToStrings(string file_path)
                {
                    Int32 file_size = FileSizeGet(file_path);
                    string[] str_arr = new string[file_size];
                    StreamReader file_read = new StreamReader(file_path);
                    Int32 string_index = 0;
                    while (file_read.EndOfStream == false)
                    {
                        str_arr[string_index] = file_read.ReadLine();
                        string_index++;
                    }
                    // 2023-04-25 15:49 not fast. 5-10 seconds for 80K lines.
                    //while (file_read.EndOfStream == false)
                    //{
                    //    Array.Resize(ref str_arr, str_arr.Length + 1);
                    //    str_arr[str_arr.Length - 1] = file_read.ReadLine();
                    //}
                    file_read.Close();
                    return str_arr;
                }
                /// <summary>
                /// Reads file to the end and puts everything into 1 string
                /// 2023.01.01 - 2023.03.01. 10 - 15 o'clock. written. 
                /// </summary>
                /// <param name="file_path"></param>
                /// <returns></returns>
                static public string ToFileString(string file_path)
                {
                    string str_for_return = "";
                    StreamReader file_read = new StreamReader(file_path);
                    str_for_return = file_read.ReadToEnd();
                    file_read.Close();
                    return str_for_return;
                }
            }
            /// <summary>
            /// Merge file 2 into file 1
            /// not tested
            /// created 2023.08.05 12:06
            /// </summary>
            /// <param name="file_1"></param>
            /// <param name="file_2"></param>
            /// <param name="lines_between"></param>
            public static void FilesMerge(string file_1, string file_2, Int32 lines_between = 0)
            {
                string[] files = new string[2] { file_1, file_2 };
                FilesMerge(file_1, files, lines_between);
            }
            /// <summary>
            /// Megres files into 1 file.
            /// not tested
            /// created 2023.08.04 11:56
            /// </summary>
            /// <param name="to_file"></param>
            /// <param name="files_list"></param>
            /// <param name="lines_between">Amount of empty lines between merged files</param>
            public static void FilesMerge(string to_file, string[] files_list, Int32 lines_between = 0)
            {
                if (files_list.Length == 0)
                {
                    ReportFunctions.ReportAttention(ReportFunctions.AttentionMessage.ArrayZeroLength);
                    return;
                }
                string content_to_file = "";
                for (Int32 i = 0; i < files_list.Length; i++)
                {
                    if (i != 0)
                    {
                        content_to_file += "\r\n";
                        for (Int32 i_insert = 0; i_insert < lines_between; i_insert++)
                        {
                            content_to_file += "\r\n";
                        }
                    }
                    string file_string = ReadFile.ToFileString(files_list[i]);
                    content_to_file += file_string;
                }
                WriteFile.FileStringToFile(content_to_file, to_file);
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="file_new"></param>
            /// <param name="file_to_update"></param>
            /// <returns>false if no update done, true if updated or new created</returns>
            public static bool FileUpdate(string file_new, string file_to_update, bool create_new = true)
            {
                bool result_out = false;
                if (FileCompare(file_new, file_to_update) == false)
                {
                    if (create_new == true)
                    {
                        FileDelete(file_to_update);
                        File.Copy(file_new, file_to_update);
                        result_out = true;
                    }
                    else
                    {
                        result_out = false;
                    }
                }
                return result_out;
            }
            public static bool FileCompare(string file_new, string file_comprate_with)
            {
                bool result_out = false;
                byte[] bytes_file_new = FileToBytes(file_new, true);
                byte[] bytes_file_compare = FileToBytes(file_comprate_with, true);
                if (bytes_file_new.SequenceEqual(bytes_file_compare) == true)
                {
                    result_out = true;
                }
                return result_out;
            }
            public static bool FileCompare(byte[] arr_in, string file_comprate_with)
            {
                bool result_out = false;
                byte[] bytes_file = FileToBytes(file_comprate_with, true);
                if (arr_in.SequenceEqual(bytes_file) == true)
                {
                    result_out = true;
                }
                return result_out;
            }
            /// <summary>
            /// Makes filepath for file to be in the folder of .exe of the programm
            /// 2023-07-26 15:14
            /// </summary>
            /// <param name="filename_in"></param>
            /// <returns></returns>
            public static string Filepath(string filename_in)
            {
                return Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "\\" + filename_in;
            }
            public static void Int32ArrayToConsole(int[] arr_in)
            {
                for (Int32 i = 0; i < arr_in.Length; i++)
                {
                    Console.WriteLine(arr_in[i].ToString());
                }
            }
            public static T[][] ArrayAxBAddArrayCxB<T>(T[][] array_1_in, T[][] array_2_in)
            {
                T[][] arr_out = new T[0][];
                arr_out = array_1_in;
                for (Int32 i = 0; i < array_2_in.Length; i++)
                {
                    Array.Resize(ref arr_out, arr_out.Length + 1);
                    arr_out[arr_out.Length - 1] = array_2_in[i];
                }
                return arr_out;
            }
            public static T[][] ArrayCxBFromArrayAxB<T>(T[][] array_in, Int32 col_to_remove)
            {
                T[][] arr_out = new T[0][];
                if (col_to_remove > array_in.Length)
                {
                    ReportFunctions.ReportError("column number bigger than length" + "\r\n" +
                        "column number is " + col_to_remove.ToString() + "\r\n" +
                         "array lenght is " + array_in.Length.ToString());
                    return arr_out;
                }
                arr_out = new T[array_in.Length - 1][];
                Int32 arr_out_index = 0;
                for (Int32 i = 0; i < array_in.Length; i++)
                {
                    if (i != col_to_remove)
                    {
                        arr_out[arr_out_index] = array_in[i];
                        arr_out_index++;
                    }
                }
                return arr_out;
            }
            /// <summary>
            /// decription. join element. element A + B. via whole array.
            /// </summary>
            /// <param name="arr_in"></param>
            /// <returns></returns>
            public static string[] StringArrays_A_B_Join_StringAStringB(string[] arr_in_1, string[] arr_in_2, char delimer_in = ' ')
            {
                string[] arr_out = new string[0];
                if (arr_in_1.Length != arr_in_2.Length)
                {
                    ReportFunctions.ReportError("String Arrays have different lenght" + "\r\n" +
                        "array 1 is " + arr_in_1.Length.ToString() + "\r\n" +
                        "array 2 is " + arr_in_2.Length.ToString());
                    return arr_out;
                }
                arr_out = new string[arr_in_1.Length];
                for (Int32 i = 0; i < arr_in_1.Length; i++)
                {
                    arr_out[i] = arr_in_1[i] + delimer_in.ToString() + arr_in_2[i];
                }
                return arr_out;
            }
            public static int[] StringArrayToInt32Array(string[] arr_in)
            {
                int[] arr_out = new int[arr_in.Length];
                try
                {
                    for (Int32 i = 0; i < arr_in.Length; i++)
                    {
                        arr_out[i] = System.Convert.ToInt32(arr_in[i]);
                    }
                }
                catch
                {
                    ReportFunctions.ReportError();
                }
                return arr_out;
            }
            public static string StringTrimToText(string string_in, string trim_point)
            {
                if (string_in == null)
                {
                    return string_in;
                }
                Int32 trim_index = string_in.IndexOf(trim_point);
                if (trim_index == -1)
                {
                    return string_in;
                }
                return string_in.Substring(trim_index + trim_point.Length);
            }
            public static string[] StringSplitByLength(string string_in, Int32 split_size)
            {
                Int32 div_rem = 0;
                string[] str_out = new string[0];
                try
                {
                    System.Math.DivRem(string_in.Length, split_size, out div_rem);
                }
                catch (Exception e)
                {
                    ReportFunctions.ReportError(e.Message);
                    return str_out;
                }
                if (div_rem != 0)
                {
                    ReportFunctions.ReportError("division reminder is not 0");
                    return str_out;
                }
                str_out = new string[string_in.Length / split_size];
                for (Int32 i = 0; i < string_in.Length; i++)
                {
                    str_out[i] = string_in.Substring(0, split_size);
                    string_in = string_in.Substring(split_size);
                }
                return str_out;
            }
            public static string[] StringSplitCertainAmount(string string_in, Int32 split_times, char delimer_in = ';')
            {
                string[] str_out = new string[split_times];
                try
                {
                    string[] for_split = string_in.Split(delimer_in);
                    Array.Copy(for_split, str_out, split_times);
                    string[] arr_for_one_str = new string[for_split.Length - split_times + 1];
                    Array.Copy(for_split, split_times - 1, arr_for_one_str, 0, arr_for_one_str.Length);
                    //Array.Resize(ref str_out, str_out.Length + 1);
                    str_out[str_out.Length - 1] = StringsToOneString(arr_for_one_str, delimer_in);
                }
                catch
                {
                    ReportFunctions.ReportError();
                }
                return str_out;
            }
            public static string[] TextFromTags(string text_in, string tag_in = "[m]")
            {
                Dictionary<string, string> dict_work = new Dictionary<string, string>();
                while (text_in.Contains(tag_in) == true)
                {
                    text_in = StringTrimToText(text_in, tag_in);
                    Int32 index_end = text_in.IndexOf(tag_in);
                    string data_str = text_in.Substring(0, index_end + 1 - 1);
                    text_in = StringTrimToText(text_in, tag_in);
                    dict_work.Add((dict_work.Count + 1).ToString(), data_str);
                }
                string[] str_arr_out = new string[dict_work.Count];
                for (Int32 i = 0; i < str_arr_out.Length; i++)
                {
                    str_arr_out[i] = dict_work.ElementAt(i).Value;
                }
                return str_arr_out;
            }
            public static Dictionary<string, string> TextFromTagsDict(string text_in, string tag_in = "[m]")
            {
                Dictionary<string, string> for_return = new Dictionary<string, string>();
                while (text_in.Contains(tag_in) == true)
                {
                    text_in = StringTrimToText(text_in, tag_in);
                    Int32 index_end = text_in.IndexOf(tag_in);
                    string data_str = text_in.Substring(0, index_end + 1 - 1);
                    text_in = StringTrimToText(text_in, tag_in);
                    if (text_in[0] == '[')
                    {
                        index_end = text_in.IndexOf("]");
                        string name_str = text_in.Substring(0, index_end + 1);
                        text_in = text_in.Substring(index_end + 1);
                        for_return.Add(name_str.Replace("[", "").Replace("]", ""), data_str);
                    }
                    else
                    {
                        for_return.Add((for_return.Count + 1).ToString(), data_str);
                    }
                }
                return for_return;
            }
            /// <summary>
            /// 2023.09.01 - 2023.09.03
            /// Reads file and return number of lines.
            /// </summary>
            /// <param name="file_path"></param>
            /// <returns></returns>
            static public Int32 FileSizeGet(string file_path)
            {
                StreamReader file_read = new StreamReader(file_path);
                Int32 lines_num = 0;
                // 2023.10.10 15:18. added. last line is empty.
                string fileline = "";
                while (file_read.EndOfStream == false)
                {
                    lines_num++;
                    fileline = file_read.ReadLine();
                }
                file_read.Close();
                // 2023.11.07 17:03. Moscow. Workplace. 
                // streamreader gives end of stream if the line contains \0
                // checks if there is \0
                /*
                if (fileline != "")
                {
                    MyReportFunctions.ReportError("Wrong file end. Last line is not empty");
                }
                */
                return lines_num;
            }
            [Obsolete]
            /// <summary>
            /// 2023.09.03 12:47. location changed to Read.ToStrings
            /// </summary>
            /// <param name="file_path"></param>
            /// <returns></returns>
            static public string[] FileToStrings(string file_path)
            {
                Int32 file_size = FileSizeGet(file_path);
                string[] str_arr = new string[file_size];
                StreamReader file_read = new StreamReader(file_path);
                Int32 string_index = 0;
                while (file_read.EndOfStream == false)
                {
                    str_arr[string_index] = file_read.ReadLine();
                    string_index++;
                }
                // 2023-04-25 15:49 not fast. 5-10 seconds for 80K lines.
                //while (file_read.EndOfStream == false)
                //{
                //    Array.Resize(ref str_arr, str_arr.Length + 1);
                //    str_arr[str_arr.Length - 1] = file_read.ReadLine();
                //}
                file_read.Close();
                return str_arr;
            }
            static public string[] StringArrayFromStringArrayNxM(string[][] arr_in, Int32 arr_num)
            {
                return arr_in[arr_num];
            }
            static public string[][] FileToStringArrayNxM(string file_path, char delimer = '\t')
            {
                string[] str_arr = FileToStrings(file_path);
                if (str_arr.Length == 0)
                {
                    ReportFunctions.ReportError("The file is empty");
                    return new string[0][];
                }
                // counting
                Int32 col_num = str_arr[0].Split(delimer).Length;
                string[][] arr_out = new string[col_num][];
                for (Int32 i = 0; i < col_num; i++)
                {
                    arr_out[i] = new string[str_arr.Length];
                }
                // filling
                for (Int32 i = 0; i < str_arr.Length; i++)
                {
                    string[] to_put_in_arr = str_arr[i].Split(delimer);
                    if (to_put_in_arr.Length != col_num)
                    {
                        ReportFunctions.ReportError("File text line has different amount of columns" + "\r\n" +
                            "file line is " + i.ToString() + "\r\n" +
                            "line has " + to_put_in_arr.Length.ToString() + " columns" + "\r\n" +
                            "number of columns should be " + col_num.ToString());
                        return arr_out;
                    }
                    for (Int32 j = 0; j < col_num; j++)
                    {
                        arr_out[j][i] = to_put_in_arr[j];
                    }
                }
                return arr_out;
            }
            static public string DoubleFormat(Int32 decimal_places = 5)
            {
                string double_format = "0.";
                for (Int32 i = 0; i < decimal_places; i++)
                {
                    double_format += "0";
                }
                return double_format;
            }
            static public void DoubleArrayToConsole(float[] array_in, Int32 decimal_places = 5)
            {
                string double_format = DoubleFormat();
                for (Int32 i = 0; i < array_in.Length; i++)
                {
                    Console.WriteLine(array_in[i].ToString(double_format));
                }
            }
            static public string StringFromStringsArray(string[] array_in, Int32 start_index, Int32 num_elements, char delimer_in = '\t')
            {
                string for_return = "";
                if (array_in.Length > 0)
                {
                    for (Int32 i = start_index; i < start_index + num_elements; i++)
                    {
                        if (i != start_index)
                        {
                            for_return += delimer_in.ToString();
                        }
                        for_return += array_in[i];
                    }
                }
                else
                {
                    ReportFunctions.ReportError();
                }
                return for_return;
            }
            static public string[] DictionaryToStrings(Dictionary<string, string> dictionary_in, char delimer_in = '\t')
            {
                string[] str_arr = new string[dictionary_in.Count];
                for (Int32 i = 0; i < str_arr.Length; i++)
                {
                    str_arr[i] = dictionary_in.ElementAt(i).Key + delimer_in.ToString() + dictionary_in.ElementAt(i).Value;
                }
                return str_arr;
            }
            public static string StringCutFromString(string string_in, string cut_start, string cut_end)
            {
                string str_out = string_in;
                bool until_text_end = true;
                while (until_text_end == true)
                {
                    Int32 start_ind = str_out.IndexOf(cut_start);
                    Int32 end_ind = str_out.IndexOf(cut_end);
                    if (start_ind == -1)
                    {
                        return str_out;
                    }
                    if (end_ind == -1)
                    {
                        return str_out;
                    }
                    string temp_str = StringCutOnceFromString(str_out, cut_start, CutStringOption_enum.FromStart);
                    if (start_ind >= end_ind)
                    {
                        Int32 ind_t = temp_str.IndexOf(cut_end);
                        if (ind_t != -1)
                        {
                            end_ind += (start_ind - end_ind);
                            end_ind += ind_t + 1;
                        }
                        else
                        {
                            return str_out;
                        }
                    }
                    string sub_str_1 = str_out.Substring(0, start_ind + 1 - 1);
                    string sub_str_2 = str_out.Substring(end_ind + cut_end.Length);
                    str_out = sub_str_1 + sub_str_2;
                }
                ReportFunctions.ReportError("that place can not be reached");
                return string_in;
            }
            public enum CutStringOption_enum
            {
                None,
                ToEnd,
                FromStart
            }
            public static string StringCutOnceFromString(string string_in, string cut_str, CutStringOption_enum option_in = CutStringOption_enum.ToEnd)
            {
                string str_out = string_in;
                Int32 cut_ind = str_out.IndexOf(cut_str);
                if (cut_ind == -1)
                {
                    return str_out;
                }
                if (option_in == CutStringOption_enum.ToEnd)
                {
                    str_out = str_out.Substring(0, cut_ind + 1 - 1);
                }
                if (option_in == CutStringOption_enum.FromStart)
                {
                    str_out = str_out.Substring(cut_ind + cut_str.Length);
                }
                return str_out;
            }
            public static string StringCutOnceFromString(string string_in, string cut_start, string cut_end)
            {
                string str_out = string_in;
                Int32 start_ind = str_out.IndexOf(cut_start);
                Int32 end_ind = str_out.IndexOf(cut_end);
                if (start_ind == -1)
                {
                    return str_out;
                }
                if (end_ind == -1)
                {
                    return str_out;
                }
                string sub_str_1 = str_out.Substring(0, start_ind + 1 - 1);
                string sub_str_2 = str_out.Substring(end_ind + cut_end.Length);
                str_out = sub_str_1 + sub_str_2;
                return str_out;
            }
            public static string StringFindReplaceString(string string_in, string find_str, string replace_str)
            {
                string str_out = string_in;
                str_out = str_out.Replace(find_str, replace_str);
                return str_out;
            }
            public static string StringToHEXString(string str_in)
            {
                string for_return = "";
                byte[] bytes_conv = Encoding.UTF8.GetBytes(str_in);
                for (Int32 i = 0; i < bytes_conv.Length; i++)
                {
                    for_return += System.Convert.ToString(bytes_conv[i], 16).PadLeft(2, '0');
                }
                return for_return;
            }
            public static string HEXStringToString(string str_in)
            {
                string for_return = "";
                for (Int32 i = 0; i < str_in.Length; i++)
                {
                    string byte_str = str_in[i].ToString();
                    i++;
                    byte_str += str_in[i].ToString();
                    byte byte_number = System.Convert.ToByte(byte_str, 16);
                    char char_letter = System.Convert.ToChar(byte_number);
                    for_return += char_letter.ToString();
                }
                return for_return;
            }
            static public string StringEraseEnd(string string_in, char symbol_meet, Int32 meet_times = 1)
            {
                Int32 str_to_leave = string_in.Length;
                for (Int32 i = string_in.Length - 1; i >= 0; i--)
                {
                    if (string_in[i] != symbol_meet)
                    {
                        str_to_leave--;
                    }
                    else
                    {
                        str_to_leave--;
                        meet_times--;
                        if (meet_times == 0)
                        {
                            break;
                        }
                    }
                }
                return string_in.Substring(0, str_to_leave);
            }
            static public void StringArrayToFile(string[] strings_in, string filename)
            {
                FileDelete(filename);
                StreamWriter file_writer = new StreamWriter(filename);
                for (Int32 i = 0; i < strings_in.Length; i++)
                {
                    if (i != 0)
                    {
                        file_writer.Write("\r\n");
                    }
                    file_writer.Write(strings_in[i]);
                }
                file_writer.Close();
            }
            static public void FileProcessToFile(string filename, string find_str, string replace_str, bool last_empty_str_remove = true)
            {
                StreamReader file_read = new StreamReader(filename);
                string file_content = file_read.ReadToEnd();
                file_read.Close();
                file_content = file_content.Replace(find_str, replace_str);
                string[] file_strings = FileFunctions.TextFile.FileStringToStrings(file_content);
                FileDelete(filename);
                StreamWriter file_writer = new StreamWriter(filename);
                Int32 strings_to_write_num = file_strings.Length;
                if (last_empty_str_remove == true)
                {
                    if ((file_strings[file_strings.Length - 1] == "\r\n") ||
                        (file_strings[file_strings.Length - 1] == "\r") ||
                        (file_strings[file_strings.Length - 1] == "\n"))
                    {
                        strings_to_write_num -= 1;
                    }
                }
                for (Int32 i = 0; i < strings_to_write_num; i++)
                {
                    if (i != 0)
                    {
                        file_writer.Write("\r\n");
                    }
                    file_writer.Write(file_strings[i]);
                }
                file_writer.Close();
            }
            [Obsolete]
            /// <summary>
            /// 2023.09.03 12:33. location changed to Read.ToFileString
            /// renaming was done via find/replace
            /// </summary>
            /// <param name="file_path"></param>
            /// <returns></returns>
            static public string ReadToFileString(string file_path)
            {
                return ReadFile.ToFileString(file_path);
                /* 2023.09.03 12:34. commented.
                string str_for_return = "";
                StreamReader file_read = new StreamReader(file_path);
                str_for_return = file_read.ReadToEnd();
                file_read.Close();
                // 2023.08.04 11:45 trimming
                Int32 num_trim = 0;
                for (Int32 i = str_for_return.Length - 1; i > 0; i--)
                {
                    if ((str_for_return[i] == '\r') ||
                            (str_for_return[i] == '\n'))
                    {
                        num_trim++;
                    }
                }
                if (num_trim != 0)
                {
                    str_for_return = str_for_return.Substring(0, str_for_return.Length - num_trim);
                }
                return str_for_return;
                */
            }
            static public string[] StringsToStringsNoCRLF(string[] strings_in)
            {
                string[] for_return = new string[0];
                if (strings_in.Length > 0)
                {
                    if ((strings_in[strings_in.Length - 1] == "\r\n") ||
                            (strings_in[strings_in.Length - 1] == "\n") ||
                            (strings_in[strings_in.Length - 1] == "\r") ||
                            (strings_in[strings_in.Length - 1] == "")
                            )
                    {
                        for_return = strings_in;
                        Array.Resize(ref for_return, for_return.Length - 1);
                    }
                    else
                    {
                        for_return = strings_in;
                    }
                }
                return for_return;
            }
            static public string StringParseSpecialCharacters(string string_in, bool from_text_to_symbol = true)
            {
                string for_return = string_in;
                if (from_text_to_symbol == true)
                {
                    for_return = for_return.Replace("\\r", "\r");
                    for_return = for_return.Replace("\\n", "\n");
                    for_return = for_return.Replace("\\t", "\t");
                }
                else
                {
                    for_return = for_return.Replace("\r", "\\r");
                    for_return = for_return.Replace("\n", "\\n");
                    for_return = for_return.Replace("\t", "\\t");
                }
                return for_return;
            }
            static public void FileCopyBackUp(string filename, string back_up_add_str = "_copy_back_up")
            {
                string back_up_filename = filename.Replace(".txt", "");
                back_up_filename += back_up_add_str;
                back_up_filename += ".txt";
                FileDelete(back_up_filename);
                System.IO.File.Copy(filename, back_up_filename);
            }
            [Obsolete]
            // 2023.11.26 04:36. Moscow. Hostel Дельта. Moved to
            // MyStringFunctions.Convert.FileStringToStrings
            static public string[] FileStringToStrings(string string_in)
            {
                return StringFunctionsNamespace.StringFunctions.Convert.FileStringToStrings(string_in);
            }
            static public bool FileCyrillicToFileEnglish(string filename_in)
            {
                bool for_return = true;
                string[] rus_letter_lower_case = { "ё", "1", "2", "3", "4", "5", "6", "7", "8", "9", "0", "-", "=", "й", "ц", "у", "к", "е", "н", "г", "ш", "щ", "з", "х", "ъ", "\\", "ф", "ы", "в", "а", "п", "р", "о", "л", "д", "ж", "э", "я", "ч", "с", "м", "и", "т", "ь", "б", "ю." };
                string[] eng_letter_lower_case = { "eo", "1", "2", "3", "4", "5", "6", "7", "8", "9", "0", "-", "=", "ii", "c", "u", "r", "t", "y", "u", "i", "o", "p", "[", "]", "\\", "a", "s", "d", "f", "g", "h", "j", "k", "l", ";", "'", "z", "x", "c", "v", "b", "n", "p", "b", "u" };
                //string[] rus_letter_upper_case = "Ё!\"№;%:?*()_+ЙЦУКЕНГШЩЗХЪ/ФЫВАПРОЛДЖЭЯЧСМИТЬБЮ,";
                //string[] eng_letter_upper_case = "~!@#$%^&*()_+QWERTYUIOP{}|ASDFGHJKL:\"ZXCVBNM<>?";
                if (System.IO.File.Exists(filename_in) == true)
                {
                    //string file_content = Read.ToFileString(filename_in);
                    //for (Int32 i = 0; i < rus_letter_lower_case.Length; i++)
                    //{
                    //    file_content = file_content.Replace(rus_letter_lower_case[i], eng_letter_lower_case[i]);
                    //}
                    //for (Int32 i = 0; i < rus_letter_upper_case.Length; i++)
                    //{
                    //    file_content = file_content.Replace(rus_letter_upper_case[i], eng_letter_upper_case[i]);
                    //}
                }
                else
                {
                    ReportFunctions.ReportError();
                    for_return = false;
                }
                return for_return;
            }
            static public string[][] FilestringToStringsNx2(string filestring_in, char key_value_delimer = '\t')
            {
                string[][] str_arr_for_return = new string[2][];
                string[] str_for_crlf_split = filestring_in.Replace("\n", "").Split('\r');
                string[] str_for_delimer_split = new string[2];
                for (Int32 i = 0; i < str_arr_for_return.Length; i++)
                {
                    str_arr_for_return[i] = new string[str_for_crlf_split.Length];
                }
                for (Int32 i = 0; i < str_for_crlf_split.Length; i++)
                {
                    str_for_delimer_split = str_for_crlf_split[i].Split(key_value_delimer);
                    for (Int32 j = 0; j < str_for_delimer_split.Length; j++)
                    {
                        str_arr_for_return[j][i] = str_for_delimer_split[j];
                    }
                }
                return str_arr_for_return;
            }
            static public bool FilesInFolderDeleteAll(string folder_path)
            {
                bool for_return = true;
                if (System.IO.Directory.Exists(folder_path) == true)
                {
                    string[] filelist = Directory.GetFiles(folder_path);
                    for (Int32 i = 0; i < filelist.Length; i++)
                    {
                        FileDelete(filelist[i]);
                    }
                }
                else
                {
                    ReportFunctions.ReportError();
                    for_return = false;
                }
                return for_return;
            }
            static public bool FilesInFolderDelete(string folder_in, string[] files_list)
            {
                bool for_return = true;
                if (System.IO.Directory.Exists(folder_in) == true)
                {
                    if (files_list.Length > 0)
                    {
                        for (Int32 i = 0; i < files_list.Length; i++)
                        {
                            string filename = folder_in + FileFunctions.FileName.Get(files_list[i], false);
                            FileDelete(filename);
                        }
                    }
                    else
                    {
                        ReportFunctions.ReportError();
                        for_return = false;
                    }
                }
                else
                {
                    ReportFunctions.ReportError();
                    for_return = false;
                }
                return for_return;
            }
            static public bool FilesDelete(string[] files_list)
            {
                bool for_return = true;
                if (files_list.Length > 0)
                {
                    for (Int32 i = 0; i < files_list.Length; i++)
                    {
                        FileDelete(files_list[i]);
                    }
                }
                else
                {
                    ReportFunctions.ReportError();
                    for_return = false;
                }
                return for_return;
            }
            static public void FilesCopyToFolder(string[] files_in, string folder_path)
            {
                if (System.IO.Directory.Exists(folder_path) == false)
                {
                    DirectoryFunctions.Make(folder_path);
                }
                if (files_in.Length > 0)
                {
                    for (Int32 i = 0; i < files_in.Length; i++)
                    {
                        string file_name = FilenameGet(files_in[i], false);
                        FileDelete(folder_path + "\\" + file_name);
                        System.IO.File.Copy(files_in[i], folder_path + "\\" + file_name);
                    }
                }
                else
                {
                    ReportFunctions.ReportError();
                }
            }
            static public void DictionaryToConsole(Dictionary<string, string> dictionary_in)
            {
                for (Int32 i = 0; i < dictionary_in.Count; i++)
                {
                    string key_in = dictionary_in.ElementAt(i).Key;
                    Console.WriteLine(key_in + "\t" + dictionary_in[key_in]);
                }
            }
            static public void DictionaryToConsole(Dictionary<string, int> dictionary_in)
            {
                for (Int32 i = 0; i < dictionary_in.Count; i++)
                {
                    string key_in = dictionary_in.ElementAt(i).Key;
                    Console.WriteLine(key_in + "\t" + dictionary_in[key_in].ToString());
                }
            }
            static public void DictionaryToFile(Dictionary<string, string> dictionary_in, string file_in)
            {
                FileDelete(file_in);
                string[] strings_for_file = DictionaryToStrings(dictionary_in);
                StringArrayToFile(strings_for_file, file_in);
            }
            static public Dictionary<string, string> FileToDictionary(string file_in, char delimer_in = '\t')
            {
                Dictionary<string, string> for_return = new Dictionary<string, string>();
                string[] strings_for_from_file = FileToStrings(file_in);
                for (Int32 i = 0; i < strings_for_from_file.Length; i++)
                {
                    string[] str_split = strings_for_from_file[i].Split(delimer_in);
                    if (str_split.Length != 2)
                    {
                        ReportFunctions.ReportError();
                        return for_return;
                    }
                    for_return.Add(str_split[0], str_split[1]);
                }
                return for_return;
            }
            /// <summary>
            /// Random in function is bad!
            /// Fast calling of the same function leads to Random has the same seed
            /// therefore there is no Random
            /// 2023-07-26 11:38
            /// </summary>
            /// <param name="size"></param>
            /// <returns></returns>
            [Obsolete]
            public static string RandomStringEng(Int32 size)
            {
                string for_return = "";
                string for_random = "abcdefghijklmnopqrstuvwxyz";
                if (size == 0)
                {
                    ReportFunctions.ReportError();
                    return for_return;
                }
                Random rand_num = new Random();
                for (Int32 i = 0; i < size; i++)
                {
                    for_return += for_random[rand_num.Next(0, for_random.Length)].ToString();
                }
                return for_return;
            }
            public static string[] GetWordsFromAllFiles(string[] files_list_in)
            {
                string[] arr_out = GetWordsFromFile(files_list_in[0]);
                for (Int32 i = 1; i < files_list_in.Length; i++)
                {
                    string[] for_merge = GetWordsFromFile(files_list_in[i]);
                    arr_out = ArrayFunctions.Merge.A_B_To_C(arr_out, for_merge);
                }
                return arr_out;
            }
            public enum DictionarySortEnum
            {
                NoSorting,
                Ascending,
                Desending
            }
            public static Dictionary<string, int> DictionarySort(Dictionary<string, int> dic_in, DictionarySortEnum sort_order)
            {
                Dictionary<string, int> dict_out = new Dictionary<string, int>();
                int[] dic_values = dic_in.Values.ToArray();
                ArrayFunctions.ArraySortingEnum sort_int32_order = ArrayFunctions.ArraySortingEnum.NoSorting;
                if (sort_order == DictionarySortEnum.Desending)
                {
                    sort_int32_order = ArrayFunctions.ArraySortingEnum.Descending;
                }
                if (sort_order == DictionarySortEnum.Ascending)
                {
                    sort_int32_order = ArrayFunctions.ArraySortingEnum.Ascenidng;
                }
                int[] dic_values_sorted = ArrayFunctions.Int32Array.Sort(dic_values, sort_int32_order);
                for (Int32 i = 0; i < dic_values.Length; i++)
                {
                    Int32 index_key = Array.IndexOf(dic_values, dic_values_sorted[i]);
                    dict_out.Add(dic_in.ElementAt(index_key).Key, dic_in.ElementAt(index_key).Value);
                    dic_values[index_key] = -1;
                }
                return dict_out;
            }
            public static string[] GetWordsFromFile(string file_in)
            {
                StreamReader file_read = null;
                try
                {
                    file_read = new StreamReader(file_in);
                }
                catch
                {
                    ReportFunctions.ReportError("File was not opened");
                    return new string[0];
                }
                string content_file = file_read.ReadToEnd();
                content_file = content_file.Replace("\r\n", " ");
                content_file = content_file.Replace("\t", " ");
                Int32 max_spaces = 1;
                bool do_search = true;
                while (do_search == true)
                {
                    string for_search = "";
                    for_search = for_search.PadRight(max_spaces + 1, ' ');
                    if (content_file.Contains(for_search) == false)
                    {
                        do_search = false;
                    }
                    else
                    {
                        max_spaces += 1;
                    }
                }
                for (Int32 i = max_spaces; i > 1; i--)
                {
                    string for_replace = "";
                    for_replace = for_replace.PadRight(i, ' ');
                    content_file = content_file.Replace(for_replace, " ");
                }
                return content_file.Split(' ');
            }
            public static void ReplaceInFile(string file_in, string str_to_replace, string str_new)
            {
                string content_of_file = ReadFile.ToFileString(file_in);
                content_of_file = content_of_file.Replace(str_to_replace, str_new);
                WriteFile.FileStringToFile(content_of_file, file_in);
            }
            public static void ReplaceInAllFiles(string[] files_in, string str_to_replace, string str_new)
            {
                for (Int32 i = 0; i < files_in.Length; i++)
                {
                    ReplaceInFile(files_in[i], str_to_replace, str_new);
                }
            }
            public static string[] GetTextFiles(string dir_path)
            {
                return Directory.GetFiles(dir_path, ".txt");
            }
            public static string RandomString(Int32 size)
            {
                string for_return = "";
                if (size == 0)
                {
                    ReportFunctions.ReportError();
                    return for_return;
                }
                Random rand_num = new Random();
                for (Int32 i = 0; i < size; i++)
                {
                    for_return += ((char)rand_num.Next(34, 127)).ToString();
                }
                return for_return;
            }
            public static UInt32 StringToUInt32(string string_4_symbols, bool low_byte_first_char = true)
            {
                UInt32 uint32_out = 0;
                if (string_4_symbols.Length == 4)
                {
                    if (low_byte_first_char == true)
                    {
                        uint32_out = System.Convert.ToUInt32((byte)string_4_symbols[0]);
                        uint32_out = System.Convert.ToUInt32((byte)string_4_symbols[1] << 8);
                        uint32_out = System.Convert.ToUInt32((byte)string_4_symbols[2] << 16);
                        uint32_out = System.Convert.ToUInt32((byte)string_4_symbols[3] << 24);
                    }
                    else
                    {
                        uint32_out = System.Convert.ToUInt32((byte)string_4_symbols[3]);
                        uint32_out = System.Convert.ToUInt32((byte)string_4_symbols[2] << 8);
                        uint32_out = System.Convert.ToUInt32((byte)string_4_symbols[1] << 16);
                        uint32_out = System.Convert.ToUInt32((byte)string_4_symbols[0] << 24);
                    }
                }
                else
                {
                    ReportFunctions.ReportError();
                }
                return uint32_out;
            }
            static public void StringsNx2ToConsole(string[][] strings_in, char key_value_delimer = '\t')
            {
                try
                {
                    string str_for_console = StringsNx2ToFilestring(strings_in, key_value_delimer);
                    Console.WriteLine(str_for_console);
                }
                catch
                {
                    ReportFunctions.ReportError();
                }
            }
            static public void StringsNx2ToFile(string[][] strings_in, string file_name, char key_value_delimer = '\t')
            {
                string filestring_in_file = StringsNx2ToFilestring(strings_in, key_value_delimer);
                WriteFile.FileStringToFile(filestring_in_file, file_name);
            }
            [Obsolete]
            // Moved to MyArrayFunctions.StringArray. 2023.12.20 16:17. Workplace.
            static public string StringsNx2ToFilestring(string[][] strings_in, char key_value_delimer = '\t')
            {
                return ArrayFunctions.StringArray.Convert.StringsAxBToFilestring(strings_in, key_value_delimer);
            }
            static public bool is_symbol_hex(string symbol_in)
            {
                bool check_res = false;
                if (symbol_in.Length > 1)
                {
                    return check_res;
                }
                for (Int32 i = 0; i < HEX_Symbols_string.Length; i++)
                {
                    if (symbol_in[0] == HEX_Symbols_string[i])
                    {
                        check_res = true;
                    }
                }
                return check_res;
            }
            static public bool is_symbol_hex(char symbol_in)
            {
                bool check_res = false;
                for (Int32 i = 0; i < HEX_Symbols_chars.Length; i++)
                {
                    if (symbol_in == HEX_Symbols_chars[i])
                    {
                        check_res = true;
                    }
                }
                return check_res;
            }
            static public bool is_hex_small_letter(char symbol_in)
            {
                bool check_res = false;
                if (symbol_in == 'a')
                {
                    check_res = true;
                }
                if (symbol_in == 'b')
                {
                    check_res = true;
                }
                if (symbol_in == 'c')
                {
                    check_res = true;
                }
                if (symbol_in == 'd')
                {
                    check_res = true;
                }
                if (symbol_in == 'e')
                {
                    check_res = true;
                }
                if (symbol_in == 'f')
                {
                    check_res = true;
                }
                return check_res;
            }
            static public string HEX_Symbols_string = "0123456789ABCDEFabcdef";
            static public char[] HEX_Symbols_chars = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'D', 'E', 'F', 'a', 'b', 'c', 'd', 'e', 'f' };
            //static public byte[] FileToBytes(string file_path)
            //{
            //    byte[] byte_arr = new byte[0];
            //    System.IO.FileStream file_read = File.Open(file_path, FileMode.Open);
            //    byte_arr = new byte[file_read.Length];
            //    for (Int32 bn = 0; bn < file_read.Length; bn++)
            //    {
            //        byte_arr[bn] = (byte)file_read.ReadByte();
            //    }
            //    file_read.Close();
            //    return byte_arr;
            //}
            /// <summary>
            /// Extension is not part of bytes of file during reading
            /// 2023-07-24 08:41
            /// </summary>
            /// <param name="byte_arr"></param>
            /// <param name="file_name"></param>
            static public void BytesToFile(byte[] byte_arr, string file_name)
            {
                FileDelete(file_name);
                System.IO.FileStream file_write = File.Open(file_name, FileMode.Create);
                
                for (Int32 bn = 0; bn < byte_arr.Length; bn++)
                {
                    file_write.WriteByte(byte_arr[bn]);
                }
                file_write.Close();
            }
            static public void Int32ToFile(int[] Int32_arr, string file_name)
            {
                FileDelete(file_name);
                System.IO.FileStream file_write = File.Open(file_name, FileMode.Create);
                for (Int32 i = 0; i < Int32_arr.Length; i++)
                {
                    file_write.Write(MathFunctions.Int32ToBytes(Int32_arr[i]), 0, 4);
                }
                file_write.Close();
            }
            /// <summary>
            /// Written. 2023.11.28 22:26. Moscow. Hostel Дельта. <br></br>
            /// Tested. Works. 2023.11.29 09:12. Moscow. Workplace.<br></br>
            /// <br></br>
            /// Note. It may not work with shortcut. 2023.11.29 15:35. Moscow. Workplace.
            /// </summary>
            /// <param name="filename_in"></param>
            /// <param name="filename_out"></param>
            /// <param name="string_length"></param>
            static public void FileToFileWithBytesConvertedToString(string filename_in, string filename_out, Int32 string_length = -1)
            {
                byte[] byte_arr = FileToBytes(filename_in);
                StringBuilder bytes_coverted_to_string = new StringBuilder();
                bytes_coverted_to_string.Capacity = byte_arr.Length * 4;
                Int32 char_count = 0;
                for (Int32 i = 0; i < byte_arr.Length; i++)
                {
                    bytes_coverted_to_string.Append(System.Convert.ToString(byte_arr[i], 16).PadLeft(2, '0'));
                    if (string_length != -1)
                    {
                        char_count += 2;
                        if (char_count >= string_length)
                        {
                            char_count = 0;
                            bytes_coverted_to_string.Append("\r\n");
                        }
                    }
                }
                string filetext = bytes_coverted_to_string.ToString();
                WriteFile.FileStringToFile(filetext, filename_out);
            }
            /// <summary>
            /// Extension is not part of bytes of file during reading
            /// 2023-07-24 08:41
            /// </summary>
            /// <param name="file_name"></param>
            /// <returns>if file does not exist return byte[0]. Error message will be shown. false if message is not needed</returns>
            static public byte[] FileToBytes(string file_name, bool no_error_msg = false)
            {
                byte[] for_return = new byte[0];
                if (System.IO.File.Exists(file_name) == true)
                {
                    System.IO.FileStream file_read = File.Open(file_name, FileMode.Open);
                    for_return = new byte[file_read.Length];
                    file_read.Read(for_return, 0, (int)file_read.Length);
                    file_read.Close();
                }
                else
                {
                    if (no_error_msg == false)
                    {
                        ReportFunctions.ReportError();
                    }
                }
                return for_return;
            }
            static public void StringToFile(string string_in, string file_name)
            {
                FileDelete(file_name);
                System.IO.StreamWriter file_write = new StreamWriter(file_name);
                file_write.Write(string_in);
                file_write.Close();
            }
            static public void FileToConsole(string file_path)
            {
                string[] str_arr = FileToStrings(file_path);
                StringsToConsole(str_arr);
            }
            static public void StringsToConsole(string[] str_arr)
            {
                for (Int32 sn = 0; sn < str_arr.Length; sn++)
                {
                    Console.WriteLine(str_arr[sn]);
                }
            }
            static public string StringsToFileString(string[] str_arr)
            {
                string str_out = "";
                for (Int32 sn = 0; sn < str_arr.Length; sn++)
                {
                    if (sn != 0)
                    {
                        str_out += "\r\n";
                    }
                    str_out += str_arr[sn];
                }
                return str_out;
            }
            static public string StringsToOneString(string[] str_arr, char delimer = '\t')
            {
                string str_out = "";
                for (Int32 sn = 0; sn < str_arr.Length; sn++)
                {
                    if (sn != 0)
                    {
                        str_out += delimer;
                    }
                    str_out += str_arr[sn];
                }
                return str_out;
            }
            static public void BytesToConsole(byte[] byte_arr, Int32 count_to_show = -1, Int32 line_size = 40, Int32 to_base = 16)
            {
                Int32 symbol_count = 0;
                Int32 num_to_show = count_to_show;
                if (num_to_show == -1)
                {
                    num_to_show = byte_arr.Length;
                }
                string str_to_console = "";
                // 2023.6.20 21:51 implemeted because large array was 10 seconds to show
                for (Int32 sn = 0; sn < byte_arr.Length; sn++)
                {
                    if (sn >= num_to_show)
                    {
                        Console.WriteLine();
                        break;
                    }
                    str_to_console += System.Convert.ToString(byte_arr[sn], to_base).PadLeft(2, '0');
                    symbol_count++;
                    if (symbol_count >= line_size)
                    {
                        symbol_count = 0;
                        str_to_console += "\r\n";
                    }
                }
                Console.WriteLine(str_to_console);
            }
            static public bool StringInArrayExist(string[] arr, string str_to_find)
            {
                bool res_for_return = false;
                Int32 index_of_find = Array.IndexOf(arr, str_to_find);
                if (index_of_find != -1)
                {
                    res_for_return = true;
                }
                return res_for_return;
            }

            static class DirectoryMyMethods
            {
                static public void Make(string dir_path)
                {
                    if (System.IO.Directory.Exists(dir_path) == false)
                    {
                        System.IO.Directory.CreateDirectory(dir_path);
                    }
                }
                static public void Delete(string dir_path)
                {
                    if (System.IO.Directory.Exists(dir_path) == true)
                    {
                        System.IO.Directory.Delete(dir_path);
                    }
                }
            }
            public static string RandomNumberString(Int32 size)
            {
                string for_return = "";
                if (size == 0)
                {
                    ReportFunctions.ReportError();
                    return for_return;
                }
                Random rand_num = new Random();
                for (Int32 i = 0; i < size; i++)
                {
                    for_return += ((char)rand_num.Next(34, 127)).ToString();
                }
                return for_return;
            }
            public static char[] SpecialLetterGet()
            {
                char[] for_return = new char[2];
                for_return[0] = '\t';
                for_return[1] = ' ';
                return for_return;
            }
            enum Strings_Remove_enum
            {
                DoNothing,
                Partial,
                FullString
            }
            public static string[] StringsRemoveByText(string[] strings_in, string remove_start, string remove_end)
            {
                string[] for_return = new string[0];
                Strings_Remove_enum[] to_remove = new Strings_Remove_enum[strings_in.Length];
                bool remove_start_detected = false;
                for (Int32 i = 0; i < to_remove.Length; i++)
                {
                    if (remove_start_detected == true)
                    {
                        to_remove[i] = Strings_Remove_enum.FullString;
                    }
                    else
                    {
                        to_remove[i] = Strings_Remove_enum.DoNothing;
                    }
                    if ((strings_in[i].Contains(remove_start) == true) &&
                            (remove_start_detected == false))
                    {
                        Int32 letter_start = strings_in[i].IndexOf(remove_start);
                        if (letter_start == 0)
                        {
                            to_remove[i] = Strings_Remove_enum.FullString;
                        }
                        else
                        {
                            to_remove[i] = Strings_Remove_enum.Partial;
                            strings_in[i] = strings_in[i].Remove(letter_start);
                        }
                        remove_start_detected = true;
                    }
                    if ((strings_in[i].Contains(remove_end) == true) &&
                            (remove_start_detected == true))
                    {
                        Int32 letter_start = strings_in[i].IndexOf(remove_end);
                        if (letter_start == 0)
                        {
                            to_remove[i] = Strings_Remove_enum.FullString;
                        }
                        else
                        {
                            to_remove[i] = Strings_Remove_enum.Partial;
                            strings_in[i] = strings_in[i].Remove(0, letter_start + remove_end.Length);
                        }
                        remove_start_detected = false;
                    }
                }
                Int32 substract_lines = to_remove.Count(p => p == Strings_Remove_enum.FullString);
                for_return = new string[strings_in.Length - substract_lines];
                Int32 new_arr_index = 0;
                for (Int32 i = 0; i < to_remove.Length; i++)
                {
                    if (to_remove[i] == Strings_Remove_enum.DoNothing)
                    {
                        for_return[new_arr_index] = strings_in[i];
                        new_arr_index++;
                    }
                    if (to_remove[i] == Strings_Remove_enum.Partial)
                    {
                        for_return[new_arr_index] = strings_in[i];
                        new_arr_index++;
                    }
                    if (to_remove[i] == Strings_Remove_enum.FullString)
                    {
                        // not included
                    }
                }
                return for_return;
            }
            public static string StringToTrimmedString(string string_in, char[] chars_to_trim, char delimer_in = '\t')
            {
                char[] chars_trim = chars_to_trim;
                string for_return = "";
                Int32 stage_number = 1;
                Int32 is_exist = 0;
                for (Int32 i = 0; i < string_in.Length; i++)
                {
                    switch (stage_number)
                    {
                        case 1:
                            // while special letter is found
                            is_exist = Array.IndexOf(chars_trim, string_in[i]);
                            if (is_exist == -1)
                            {
                                for_return += string_in[i].ToString();
                                stage_number = 2;
                            }
                            break;
                        case 2:
                            // while data found
                            is_exist = Array.IndexOf(chars_trim, string_in[i]);
                            if (is_exist == -1)
                            {
                                for_return += string_in[i].ToString();
                            }
                            else
                            {
                                stage_number = 3;
                            }
                            break;
                        case 3:
                            // while special letter found
                            is_exist = Array.IndexOf(chars_trim, string_in[i]);
                            if (is_exist == -1)
                            {
                                for_return += delimer_in.ToString() + string_in[i].ToString();
                                stage_number = 2;
                            }
                            break;
                        case 0:
                            break;
                    }
                }
                return for_return;
            }
            public static bool StringAddToArray(ref string[] array_in, string string_in)
            {
                bool for_return = true;
                try
                {
                    Array.Resize(ref array_in, array_in.Length + 1);
                    array_in[array_in.Length - 1] = string_in;
                }
                catch
                {
                    for_return = false;
                }
                return for_return;
            }
            public static bool FileToTrimmedFile(string file_in, string file_out)
            {
                bool for_return = false;
                StreamReader file_read = new StreamReader(file_in);
                string[] filelines = new string[0];
                while (file_read.EndOfStream == false)
                {
                    StringAddToArray(ref filelines, file_read.ReadLine());
                }
                file_read.Close();
                string[] filelines_trimmed = new string[filelines.Length];
                for (Int32 i = 0; i < filelines_trimmed.Length; i++)
                {
                    StringToTrimmedString(filelines[i], SpecialLetterGet());
                }
                StreamWriter file_write = new StreamWriter(file_out);
                for (Int32 i = 0; i < filelines_trimmed.Length; i++)
                {
                    if (i != 0)
                    {
                        file_write.Write("\r\n");
                    }
                    file_write.Write(filelines_trimmed[i]);
                }
                file_write.Close();
                return for_return;
            }
        }



    }
}
