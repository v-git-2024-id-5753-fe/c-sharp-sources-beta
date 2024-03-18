using FileFunctionsNamespace;
using MathFunctionsNamespace;
using ReportFunctionsNamespace;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

// Commercial use (license)
// Please study about commercial use of the code that is publicly available 

namespace DatetimeFunctionsNamespace
{
    public static class DatetimeFunctions
    {
        public static class TestFunctions
        {
            /// <summary>
            /// The function check if -1 seconds result in substructing minutes. <br></br>
            /// Written. 2023.11.26 12:02. Moscow. Hostel Дельта. <br></br>
            /// Tested. Works. Substracting second leads to subsctracting minutes.
            /// </summary>
            public static void SubstractingSecond()
            {
                DateTime time = DateTime.Now;
                for (Int32 i = 0; i < 180; i++)
                {
                    Console.WriteLine(time.ToString());
                    time = time.AddSeconds(-1);
                }
            }
        }
        public static string[] DatetimeArrayToStringArray(DateTime[] array_in, string time_format = "yyyy.MM.dd HH:mm:ss")
        {
            string[] arr_out = new string[array_in.Length];
            for (Int32 i = 0; i < array_in.Length; i++)
            {
                arr_out[i] = array_in[i].ToString(time_format);
            }
            return arr_out;
        }
        public static string DatetimeToString(DateTime datetime_in, string time_format = "yyyy.MM.dd HH:mm:ss")
        {
            return datetime_in.ToString(time_format);
        }
        public enum DateTimeSortingEnum
        {
            Descending,
            Ascending
        }
        public static Int32 sort_condition(DateTime time_1, DateTime time_2)
        {
            Int32 res_out = 0;
            if (time_1 > time_2)
            {
                return 1;
            }
            if (time_1 == time_2)
            {
                return 0;
            }
            if (time_1 < time_2)
            {
                return -1;
            }
            return res_out;
        }
        public static DateTime[] DatetimeSortArray(DateTime[] array_in, DateTimeSortingEnum order_to_sort = DateTimeSortingEnum.Ascending)
        {
            // 2023-06-14 11:18 works checked.
            DateTime[] arr_out = array_in;
            Array.Sort(array_in, sort_condition);
            if (order_to_sort == DateTimeSortingEnum.Descending)
            {
                Array.Reverse(array_in);
            };
            return arr_out;
        }
        public static DateTime[][] DatetimeArrayToDayArrays(DateTime[] array_in)
        {
            if (array_in.Length == 0)
            {
                MyReportFunctions.ReportError(nameof(DatetimeArrayToDayArrays) + "\r\n" +
                    "array length is 0");
                return new DateTime[0][];
            }
            // counting days
            Int32 day_condition = array_in[0].Day;
            Int32 day_count = 1;
            List<int> day_found_list = new List<int>();
            day_found_list.Add(day_condition);
            for (Int32 i = 0; i < array_in.Length; i++)
            {
                if (day_condition != array_in[i].Day)
                {
                    if (day_found_list.Contains(array_in[i].Day) == true)
                    {
                        continue;
                    }
                    day_found_list.Add(array_in[i].Day);
                    day_count++;
                    day_condition = array_in[i].Day;
                }
            }
            // array of day 
            int[] days_found = day_found_list.ToArray();
            day_condition = array_in[0].Day;
            // filling
            DateTime[][] arr_out = new DateTime[day_count][];
            for (Int32 i = 0; i < arr_out.Length; i++)
            {
                arr_out[i] = DatetimeArrayAFromDatetimeArrayBCertainDay(array_in, days_found[i]);
            }
            return arr_out;
        }
        public static DateTime[][] DatetimeArrayToHourArrays(DateTime[] array_in)
        {
            if (array_in.Length == 0)
            {
                MyReportFunctions.ReportError(nameof(DatetimeArrayToHourArrays) + "\r\n" +
                    "array length is 0");
                return new DateTime[0][];
            }
            // counting hours
            Int32 hour_condition = array_in[0].Hour;
            Int32 hour_count = 1;
            List<int> hour_found_list = new List<int>();
            hour_found_list.Add(hour_condition);
            for (Int32 i = 0; i < array_in.Length; i++)
            {
                if (hour_condition != array_in[i].Hour)
                {
                    if (hour_found_list.Contains(array_in[i].Hour) == true)
                    {
                        continue;
                    }
                    hour_found_list.Add(array_in[i].Hour);
                    hour_count++;
                    hour_condition = array_in[i].Hour;
                }
            }
            // array of hours 
            int[] hour_found = hour_found_list.ToArray();
            hour_condition = array_in[0].Hour;
            // filling
            DateTime[][] arr_out = new DateTime[hour_count][];
            for (Int32 i = 0; i < arr_out.Length; i++)
            {
                arr_out[i] = DatetimeArrayAFromDatetimeArrayBCertainHour(array_in, hour_found[i]);
            }
            return arr_out;
        }
        public enum DateTimeTimeSelection
        {
            WholeTime,
            Day,
            Hour,
            Minute
        }
        public static void DateTimeArrayToFile(DateTime[] array_in, string fileName_in = "")
        {
            FileFunctions.TextFile.StringArrayToFile(DatetimeFunctions.DatetimeArrayToStringArray(array_in), fileName_in);
        }
        public static void DateTimeArrayNxMToFiles(DateTime[][] array_in, string[] filenames_in = null)
        {
            string[][] str_out_arr = new string[array_in.Length][];
            if (filenames_in == null)
            {
                filenames_in = new string[array_in.Length];
                for (Int32 i = 0; i < filenames_in.Length; i++)
                {
                    filenames_in[i] = nameof(DateTimeArrayNxMToFiles) + "_" + i.ToString() + ".txt";
                }
            }
            try
            {
                for (Int32 i = 0; i < array_in.Length; i++)
                {
                    str_out_arr[i] = DatetimeFunctions.DatetimeArrayToStringArray(array_in[i]);
                    FileFunctions.TextFile.StringArrayToFile(str_out_arr[i], filenames_in[i]);
                }
            }
            catch
            {
                MyReportFunctions.ReportError("something wrong.\r\nused methods:\r\n" + nameof(DatetimeArrayToStringArray)
                   + "\r\n" + nameof(FileFunctions.TextFile.StringArrayToFile));
            }
        }
        public static DateTime[][] DatetimeArrayToMinuteArrays(DateTime[] array_in)
        {
            if (array_in.Length == 0)
            {
                MyReportFunctions.ReportError(nameof(DatetimeArrayToMinuteArrays) + "\r\n" +
                    "array length is 0");
                return new DateTime[0][];
            }
            // counting minutes
            Int32 minute_condition = array_in[0].Minute;
            Int32 minute_count = 1;
            List<int> minute_found_list = new List<int>();
            minute_found_list.Add(minute_condition);
            for (Int32 i = 0; i < array_in.Length; i++)
            {
                if (minute_condition != array_in[i].Minute)
                {
                    if (minute_found_list.Contains(array_in[i].Minute) == true)
                    {
                        continue;
                    }
                    minute_found_list.Add(array_in[i].Minute);
                    minute_count++;
                    minute_condition = array_in[i].Minute;
                }
            }
            // array of minutes 
            int[] minute_found = minute_found_list.ToArray();
            minute_condition = array_in[0].Minute;
            // filling
            DateTime[][] arr_out = new DateTime[minute_count][];
            for (Int32 i = 0; i < arr_out.Length; i++)
            {
                arr_out[i] = DatetimeArrayAFromDatetimeArrayBCertainMinute(array_in, minute_found[i]);
            }
            return arr_out;
        }
        public static DateTime[] DatetimeArrayToStringArray(string[] array_in)
        {
            DateTime[] arr_out = new DateTime[array_in.Length];
            for (Int32 i = 0; i < array_in.Length; i++)
            {
                if (DateTime.TryParse(array_in[i], out arr_out[i]) == false)
                {
                    MyReportFunctions.ReportError("String to datetime. Conversion failed at " + i.ToString() + "\r\n" +
                        "Datetime string is " + array_in[i] + "\r\n" +
                        "Conversion was not continued");
                    break;
                }
            }
            return arr_out;
        }
        public static DateTime[] DatetimeArrayAFromDatetimeArrayBCertainDayOfWeek(DateTime[] arr_in, DayOfWeek day_in)
        {
            // counting
            Int32 arr_out_size = 0;
            int[] arr_condition = new int[arr_in.Length];
            for (Int32 i = 0; i < arr_in.Length; i++)
            {
                arr_condition[i] = 0;
                if (arr_in[i].DayOfWeek == day_in)
                {
                    arr_out_size++;
                    arr_condition[i] = 1;
                }
            }
            // filling
            Int32 arr_out_index = 0;
            DateTime[] arr_out = new DateTime[arr_out_size];
            for (Int32 i = 0; i < arr_in.Length; i++)
            {
                if (arr_condition[i] == 1)
                {
                    arr_out[arr_out_index] = arr_in[i];
                    arr_out_index++;
                }
            }
            return arr_out;
        }
        public static void DateTimeArrayToConsole(DateTime[] arr_in)
        {
            string[] str_out = DatetimeArrayToStringArray(arr_in);
            FileFunctions.TextFile.StringsToConsole(str_out);
        }
        public static DateTime[] DatetimeArrayAFromDatetimeArrayBCertainDay(DateTime[] arr_in, Int32 day_in)
        {
            // counting
            Int32 arr_out_size = 0;
            int[] arr_condition = new int[arr_in.Length];
            for (Int32 i = 0; i < arr_in.Length; i++)
            {
                arr_condition[i] = 0;
                if (arr_in[i].Day == day_in)
                {
                    arr_out_size++;
                    arr_condition[i] = 1;
                }
            }
            // filling
            Int32 arr_out_index = 0;
            DateTime[] arr_out = new DateTime[arr_out_size];
            for (Int32 i = 0; i < arr_in.Length; i++)
            {
                if (arr_condition[i] == 1)
                {
                    arr_out[arr_out_index] = arr_in[i];
                    arr_out_index++;
                }
            }
            return arr_out;
        }
        public static DateTime[] DatetimeArrayAFromDatetimeArrayBCertainHour(DateTime[] arr_in, Int32 hour_in)
        {
            // counting
            Int32 arr_out_size = 0;
            int[] arr_condition = new int[arr_in.Length];
            for (Int32 i = 0; i < arr_in.Length; i++)
            {
                arr_condition[i] = 0;
                if (arr_in[i].Hour == hour_in)
                {
                    arr_out_size++;
                    arr_condition[i] = 1;
                }
            }
            // filling
            Int32 arr_out_index = 0;
            DateTime[] arr_out = new DateTime[arr_out_size];
            for (Int32 i = 0; i < arr_in.Length; i++)
            {
                if (arr_condition[i] == 1)
                {
                    arr_out[arr_out_index] = arr_in[i];
                    arr_out_index++;
                }
            }
            return arr_out;
        }
        public static DateTime[] DatetimeArrayAFromDatetimeArrayBCertainMinute(DateTime[] arr_in, Int32 minute_in)
        {
            // counting
            Int32 arr_out_size = 0;
            int[] arr_condition = new int[arr_in.Length];
            for (Int32 i = 0; i < arr_in.Length; i++)
            {
                arr_condition[i] = 0;
                if (arr_in[i].Minute == minute_in)
                {
                    arr_out_size++;
                    arr_condition[i] = 1;
                }
            }
            // filling
            Int32 arr_out_index = 0;
            DateTime[] arr_out = new DateTime[arr_out_size];
            for (Int32 i = 0; i < arr_in.Length; i++)
            {
                if (arr_condition[i] == 1)
                {
                    arr_out[arr_out_index] = arr_in[i];
                    arr_out_index++;
                }
            }
            return arr_out;
        }
        public static DateTime[] DateTimeArrayRandomTime(DateTime min_time, DateTime max_time, Int32 count)
        {
            DateTime[] arr_out = new DateTime[count];
            RandomInt64 rand_num = new RandomInt64();
            Int64 time_min_ticks = min_time.ToBinary();
            Int64 time_max_ticks = max_time.ToBinary();
            for (Int32 i = 0; i < arr_out.Length; i++)
            {
                arr_out[i] = DateTime.FromBinary(rand_num.Next(time_min_ticks, time_max_ticks));
            }
            return arr_out;
        }
    }
}
