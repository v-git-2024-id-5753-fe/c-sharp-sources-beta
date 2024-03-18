using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using FileFunctionsNamespace;
using ReportFunctionsNamespace;

// Commercial use (license)
// Please study about commercial use of the code that is publicly available 

namespace MathFunctionsNamespace
{
    class MathOperations
    {
    }
    class RandomInt64
    {
        public RandomInt64()
        {
            DateTime num_seed = DateTime.Now;
            Int32 seed_rand = (int)num_seed.Ticks;
            random_for_numbers = new Random(seed_rand);
        }
        Random random_for_numbers = null;
        public Int64 Next(Int64 min, Int64 max)
        {
            //UInt32 h_int32_min = (UInt32)min >> 32;
            //UInt32 h_int32_max = (UInt32)max >> 32;
            //UInt32 l_int32_min = (UInt32)min >> 0;
            //UInt32 l_int32_max = (UInt32)max >> 0;
            //Int32 h_int32_rand = random.Next(h_int32_min, h_int32_max);
            //Int32 l_int32_rand = random.Next(l_int32_min, l_int32_max);
            //Int64 num_out = ((Int64)h_int32_rand << 32) | (Int64)l_int32_rand;
            // 2023.6.12 15:03 does not work because of Int32 and Int32 gives negative
            // number and min may be bigger than max.
            Int64 num_out = (Int64)min;
            for (Int32 i = 63; i >= 0; i--)
            {
                Int32 rand_num = random_for_numbers.Next(0, 2);
                if (rand_num == 1)
                {
                    Int64 num_compare = num_out;
                    num_compare |= ((Int64)1 << i);
                    if (num_compare <= max)
                    {
                        num_out = num_compare;
                    }
                }
            }
            return (Int64)num_out;
        }
    }
    // This should be moved to according places. 2024.01.20 22:12. Moscow. Hostel 
    class ArrayInt32WithMethods_CLS
    {
        // calculate by >= and then lower the number. with lower number it will include the previous counting.
        // then the proper substraction.
        public ArrayInt32WithMethods_CLS()
        {
        }
        public ArrayInt32WithMethods_CLS(Int32 elem_num)
        {
            new_size_set(ref Array_Int32, elem_num);
        }
        public void PopulateArray(ref int[] array_in, Int32 max = 1000, Int32 min = 0)
        {
            array_in = MathFunctions.Int32RandomArray(min, max, array_in.Length);
        }
        public int[] Array_Int32 = new int[0];
        void new_size_set(ref int[] array_in, Int32 elem_num)
        {
            if (elem_num < 0)
            {
                MyReportFunctions.ReportError("array size < 0");
                return;
            }
            array_in = new int[elem_num];
            for (Int32 i = 0; i < array_in.Length; i++)
            {
                array_in[i] = 0;
            }
        }
        public void NewSize(Int32 elem_num)
        {
            if (elem_num < 0)
            {
                MyReportFunctions.ReportError("array size < 0");
                return;
            }
            Array_Int32 = new int[elem_num];
            for (Int32 i = 0; i < Array_Int32.Length; i++)
            {
                Array_Int32[i] = 0;
            }
        }
        public void ToConsole(int[] array_out)
        {
            for (Int32 i = 0; i < array_out.Length; i++)
            {
                Console.WriteLine(array_out[i]);
            }
        }
        public void StringsToArray(string[] strings_in)
        {
            new_size_set(ref Array_Int32, strings_in.Length);
            for (Int32 i = 0; i < Array_Int32.Length; i++)
            {
                try
                {
                    Array_Int32[i] = System.Convert.ToInt32(strings_in[i]);
                }
                catch
                {
                    MyReportFunctions.ReportError("Conversion error. The string is " + strings_in[i]);
                }
            }
        }
        public int[] NumbersGreaterOrEqual(Int32 number_to_compare)
        {
            // 2023-04-13 15:56 work
            int[] arr_return = new int[0];
            Int32 num_found = 0;
            int[] array_true_mark = new int[0];
            new_size_set(ref array_true_mark, Array_Int32.Length);
            for (Int32 i = 0; i < Array_Int32.Length; i++)
            {
                if (Array_Int32[i] >= number_to_compare)
                {
                    num_found++;
                    array_true_mark[i] = 1;
                }
            }
            arr_return = new int[num_found];
            Int32 index_2nd_array = 0;
            for (Int32 i = 0; i < Array_Int32.Length; i++)
            {
                if (array_true_mark[i] == 1)
                {
                    arr_return[index_2nd_array] = Array_Int32[i];
                    index_2nd_array++;
                }
            }
            return arr_return;
        }
        public int[] NumbersLesserOrEqual(Int32 number_to_compare)
        {
            // 2023-04-13 15:56 work
            int[] arr_return = new int[0];
            Int32 num_found = 0;
            int[] array_true_mark = new int[0];
            new_size_set(ref array_true_mark, Array_Int32.Length);
            for (Int32 i = 0; i < Array_Int32.Length; i++)
            {
                if (Array_Int32[i] <= number_to_compare)
                {
                    num_found++;
                    array_true_mark[i] = 1;
                }
            }
            arr_return = new int[num_found];
            Int32 index_2nd_array = 0;
            for (Int32 i = 0; i < Array_Int32.Length; i++)
            {
                if (array_true_mark[i] == 1)
                {
                    arr_return[index_2nd_array] = Array_Int32[i];
                    index_2nd_array++;
                }
            }
            return arr_return;
        }
    }
    class ArraysInt32_N_number_WithMethods_CLS
    {
        public int[,] IntervalEqual(Int32 interval_in, string name_arr_2, string name_arr_1)
        {
            Int32 arr_1_num = ArrayName.IndexOf(name_arr_1);
            Int32 arr_2_num = ArrayName.IndexOf(name_arr_2);
            if (arr_1_num == -1)
            {
                MyReportFunctions.ReportError("Array 1 name was not found");
                return null;
            }
            if (arr_2_num == -1)
            {
                MyReportFunctions.ReportError("Array 2 name was not found");
                return null;
            }
            // counting 1st
            ArrayInt32WithMethods_CLS arr_0_1_condition = new ArrayInt32WithMethods_CLS(ArrayN[arr_1_num].Array_Int32.Length);
            for (Int32 i = 0; i < ArrayN[arr_2_num].Array_Int32.Length; i++)
            {
                Int32 numbers_difference = ArrayN[arr_2_num].Array_Int32[i] - ArrayN[arr_1_num].Array_Int32[i];
                if (numbers_difference == interval_in)
                {
                    arr_0_1_condition.Array_Int32[i] = 1;
                }
            }
            Int32 size_out = arr_0_1_condition.Array_Int32.Sum();
            // counting end
            // fill arr out
            Int32 index_arr_out = 0;
            int[,] arr_out = new int[2, size_out];
            for (Int32 i = 0; i < ArrayN[arr_2_num].Array_Int32.Length; i++)
            {
                if (arr_0_1_condition.Array_Int32[i] == 1)
                {
                    arr_out[0, index_arr_out] = i;
                    arr_out[1, index_arr_out] = interval_in;
                    index_arr_out++;
                }
            }
            return arr_out;
        }
        public int[,] IntervalEqualorLessByModule(Int32 interval_in, string name_arr_2, string name_arr_1)
        {
            Int32 arr_1_num = ArrayName.IndexOf(name_arr_1);
            Int32 arr_2_num = ArrayName.IndexOf(name_arr_2);
            if (arr_1_num == -1)
            {
                MyReportFunctions.ReportError("Array 1 name was not found");
                return null;
            }
            if (arr_2_num == -1)
            {
                MyReportFunctions.ReportError("Array 2 name was not found");
                return null;
            }
            // counting 1st
            ArrayInt32WithMethods_CLS arr_0_1_condition = new ArrayInt32WithMethods_CLS(ArrayN[arr_1_num].Array_Int32.Length);
            for (Int32 i = 0; i < ArrayN[arr_2_num].Array_Int32.Length; i++)
            {
                Int32 numbers_difference = ArrayN[arr_2_num].Array_Int32[i] - ArrayN[arr_1_num].Array_Int32[i];
                if (interval_in > 0)
                {
                    if (numbers_difference >= 0)
                    {
                        if (numbers_difference <= interval_in)
                        {
                            arr_0_1_condition.Array_Int32[i] = 1;
                        }
                    }
                }
                if (interval_in < 0)
                {
                    if (numbers_difference < 0)
                    {
                        if (numbers_difference >= interval_in)
                        {
                            arr_0_1_condition.Array_Int32[i] = 1;
                        }
                    }
                }
            }
            Int32 size_out = arr_0_1_condition.Array_Int32.Sum();
            // counting end
            // fill arr out
            Int32 index_arr_out = 0;
            int[,] arr_out = new int[2, size_out];
            for (Int32 i = 0; i < ArrayN[arr_2_num].Array_Int32.Length; i++)
            {
                if (arr_0_1_condition.Array_Int32[i] == 1)
                {
                    Int32 numbers_difference = ArrayN[arr_2_num].Array_Int32[i] - ArrayN[arr_1_num].Array_Int32[i];
                    arr_out[0, index_arr_out] = i;
                    arr_out[1, index_arr_out] = numbers_difference;
                    index_arr_out++;
                }
            }
            return arr_out;
        }
        string instance_name = "";
        public void ToFileIntervalDistribution(string filename = "")
        {
            if (filename == "")
            {
                string filename_out = nameof(ToFileIntervalDistribution) + "_step_" + IntervalDistributionByModule_interval_step.ToString() + "__";
                string time_str = DateTime.Now.ToString("yyyy.MM.dd HH.mm");
                filename_out = filename_out + " " + time_str + ".txt";
                MathFunctions.Int32ArrayNxM_ToFile(IntervalDistributionByModule_stored_value, filename_out);
            }
            else
            {
                string filename_out = filename;
                MathFunctions.Int32ArrayNxM_ToFile(IntervalDistributionByModule_stored_value, filename_out);
            }
        }
        int[,] IntervalDistributionByModule_stored_value = new int[0, 0];
        Int32 IntervalDistributionByModule_interval_step = 0;
        public void ToFilesIntervalDistributions(Int32 interval_step, string name_arr_2, string name_arr_1)
        {
            IntervalDistributionByModule(interval_step, name_arr_2, name_arr_1);
            ToFileIntervalDistribution();
            IntervalDistributionByModule(-interval_step, name_arr_2, name_arr_1);
            ToFileIntervalDistribution();
        }
        public int[,] IntervalDistributionByModule(Int32 interval_step, string name_arr_2, string name_arr_1)
        {
            Int32 arr_1_num = ArrayName.IndexOf(name_arr_1);
            Int32 arr_2_num = ArrayName.IndexOf(name_arr_2);
            if (arr_1_num == -1)
            {
                MyReportFunctions.ReportError("Array 1 name was not found");
                return null;
            }
            if (arr_2_num == -1)
            {
                MyReportFunctions.ReportError("Array 2 name was not found");
                return null;
            }
            int[,] arr_out = new int[0, 0];
            if (interval_step > 0)
            {
                // counting 1st
                Int32 div_rem = 0;
                int[] for_range = ArrayN[arr_2_num].Array_Int32.Zip(ArrayN[arr_1_num].Array_Int32, (a2, a1) => a2 - a1).ToArray();
                Int32 numbers_range = for_range.Max();
                System.Math.DivRem(numbers_range, interval_step, out div_rem);
                Int32 steps_of_distribution = System.Math.Abs(numbers_range / interval_step);
                if (div_rem != 0)
                {
                    steps_of_distribution += 1;
                }
                ArrayInt32WithMethods_CLS arr_distribution = new ArrayInt32WithMethods_CLS(steps_of_distribution);
                for (Int32 i = 0; i < steps_of_distribution; i++)
                {
                    Int32 interval_to_measure = (i + 1) * interval_step;
                    if (interval_to_measure > numbers_range)
                    {
                        interval_to_measure = numbers_range;
                    }
                    int[,] less_or_equal = IntervalEqualorLessByModule(interval_to_measure, name_arr_2, name_arr_1);
                    arr_distribution.Array_Int32[i] = less_or_equal.GetLength(1);
                }
                // substracting
                for (Int32 i = steps_of_distribution - 1; i > 0; i--)
                {
                    arr_distribution.Array_Int32[i] = arr_distribution.Array_Int32[i] - arr_distribution.Array_Int32[i - 1];
                }
                // fill arr out
                arr_out = new int[2, steps_of_distribution];
                for (Int32 i = 0; i < steps_of_distribution; i++)
                {
                    Int32 interval_to_measure = (i + 1) * interval_step;
                    if (interval_to_measure > numbers_range)
                    {
                        interval_to_measure = numbers_range;
                    }
                    arr_out[0, i] = interval_to_measure;
                    arr_out[1, i] = arr_distribution.Array_Int32[i];
                }
                IntervalDistributionByModule_stored_value = arr_out;
                IntervalDistributionByModule_interval_step = interval_step;
            }
            if (interval_step < 0)
            {
                // counting 1st
                Int32 div_rem = 0;
                int[] for_range = ArrayN[arr_2_num].Array_Int32.Zip(ArrayN[arr_1_num].Array_Int32, (a2, a1) => a2 - a1).ToArray();
                Int32 numbers_range = for_range.Min();
                System.Math.DivRem(numbers_range, interval_step, out div_rem);
                Int32 steps_of_distribution = System.Math.Abs(numbers_range / interval_step);
                if (div_rem != 0)
                {
                    steps_of_distribution += 1;
                }
                ArrayInt32WithMethods_CLS arr_distribution = new ArrayInt32WithMethods_CLS(steps_of_distribution);
                for (Int32 i = 0; i < steps_of_distribution; i++)
                {
                    Int32 interval_to_measure = (i + 1) * interval_step;
                    if (interval_to_measure < numbers_range)
                    {
                        interval_to_measure = numbers_range;
                    }
                    int[,] less_or_equal = IntervalEqualorLessByModule(interval_to_measure, name_arr_2, name_arr_1);
                    arr_distribution.Array_Int32[i] = less_or_equal.GetLength(1);
                }
                // substracting
                for (Int32 i = steps_of_distribution - 1; i > 0; i--)
                {
                    arr_distribution.Array_Int32[i] = arr_distribution.Array_Int32[i] - arr_distribution.Array_Int32[i - 1];
                }
                // fill arr out
                arr_out = new int[2, steps_of_distribution];
                for (Int32 i = 0; i < steps_of_distribution; i++)
                {
                    Int32 interval_to_measure = (i + 1) * interval_step;
                    if (interval_to_measure < numbers_range)
                    {
                        interval_to_measure = numbers_range;
                    }
                    arr_out[0, i] = interval_to_measure;
                    arr_out[1, i] = arr_distribution.Array_Int32[i];
                }
                IntervalDistributionByModule_stored_value = arr_out;
                IntervalDistributionByModule_interval_step = interval_step;
            }
            return arr_out;
        }
        public int[] NumbersGreaterOrEqual(Int32 number_to_compare, string arr_name)
        {
            Int32 arr_num = ArrayName.IndexOf(arr_name);
            if (arr_num == -1)
            {
                MyReportFunctions.ReportError("Array name was not found");
                return null;
            }
            return ArrayN[arr_num].NumbersGreaterOrEqual(number_to_compare);
        }
        public int[] NumbersLesserOrEqual(Int32 number_to_compare, string arr_name)
        {
            Int32 arr_num = ArrayName.IndexOf(arr_name);
            if (arr_num == -1)
            {
                MyReportFunctions.ReportError("Array name was not found");
                return null;
            }
            return ArrayN[arr_num].NumbersLesserOrEqual(number_to_compare);
        }
        public ArrayInt32WithMethods_CLS[] ArrayN = new ArrayInt32WithMethods_CLS[0];
        public ArraysInt32_N_number_WithMethods_CLS() { }
        public List<string> ArrayName = new List<string>();
        public ArraysInt32_N_number_WithMethods_CLS(Int32 array_num, Int32 arr_size = 0)
        {
            ArrayN = new ArrayInt32WithMethods_CLS[array_num];
            foreach (ArrayInt32WithMethods_CLS array in ArrayN)
            {
                array.NewSize(arr_size);
            }
        }
        public void PopulateArrays(Int32 min = 0, Int32 max = 1000)
        {
            foreach (ArrayInt32WithMethods_CLS array in ArrayN)
            {
                array.PopulateArray(ref array.Array_Int32, max, min);
            }
        }
        public void ToConsole(Int32 arr_num = -1)
        {
            if (arr_num < -1)
            {
                arr_num = -1;
                MyReportFunctions.ReportError("parameter was less than -1");
            }
            if (arr_num == -1)
            {
                if (ArrayN.Length == 0)
                {
                    Console.WriteLine("Number of arrays is 0");
                    return;
                }
                int[][] arr_out = new int[ArrayN.Length][];
                for (Int32 i = 0; i < ArrayN.Length; i++)
                {
                    arr_out[i] = ArrayN[i].Array_Int32;
                }
                MathFunctions.Int32ArrayNxM_ToConsole(arr_out);
                return;
            }
            if (arr_num >= ArrayN.Length)
            {
                MyReportFunctions.ReportError("Array number is bigger than number of arrays");
                return;
            }
            ArrayN[arr_num].ToConsole(ArrayN[arr_num].Array_Int32);
        }
        public void AddArray(Int32 size = 0, string name = "array")
        {
            Array.Resize(ref ArrayN, ArrayN.Length + 1);
            ArrayN[ArrayN.Length - 1] = new ArrayInt32WithMethods_CLS();
            ArrayN[ArrayN.Length - 1].NewSize(size);
            ArrayName.Add(name + (ArrayN.Length - 1).ToString());
        }
        public int[] GetArray(Int32 array_num)
        {
            if (array_num >= ArrayN.Length)
            {
                MyReportFunctions.ReportError("Array number is bigger than number of arrays");
                return new int[0];
            }
            return ArrayN[array_num].Array_Int32;
        }
        public int[] GetArray(string name)
        {
            Int32 arr_num = ArrayName.IndexOf(name);
            if (arr_num == -1)
            {
                MyReportFunctions.ReportError("Array name was not found");
                return new int[0];
            }
            if (arr_num >= ArrayN.Length)
            {
                MyReportFunctions.ReportError("Array number is bigger than number of arrays");
                return new int[0];
            }
            return GetArray(arr_num);
        }
        public ArrayInt32WithMethods_CLS GetArrayWithMethods(string name)
        {
            Int32 arr_num = ArrayName.IndexOf(name);
            if (arr_num == -1)
            {
                MyReportFunctions.ReportError("Array name was not found");
                return null;
            }
            if (arr_num >= ArrayN.Length)
            {
                MyReportFunctions.ReportError("Array number is bigger than number of arrays");
                return null;
            }
            return ArrayN[arr_num];
        }
    }
    class MathFunctions
    {
        /// <summary>
        /// Added. 2024.01.01 16:48. Saint-Petersburg. Home. 
        /// </summary>
        static Random random_for_numbers = new Random();
        /// <summary>
        /// Written. 2024.01.01 16:39. Saint-Petersburg. Home. 
        /// not tested
        public static class SequenceGenerate
        {
            /// <summary>
            /// Written. 2024.01.01 17:28. Saint-Petersburg. Home. 
            /// </summary>
            /// <param name="arr_in"></param>
            /// <returns></returns>
            public static string[] FromStringArrayAxB(string[][] arr_in)
            {
                if (arr_in.Length == 0)
                {
                    MyReportFunctions.ReportAttention(MyReportFunctions.AttentionMessage.ArrayZeroLength);
                    return new string[0];
                }
                string[] arr_out = new string[arr_in.Length];
                for (Int32 i = 0; i < arr_out.Length; i++)
                {
                    arr_out[i] = arr_in[i][random_for_numbers.Next(0, arr_in[i].Length)];
                }
                return arr_out;
            }
            public static T[] FromArray<T>(T[] arr_in, Int32 seq_length)
            {
                if (arr_in.Length == 0)
                {
                    MyReportFunctions.ReportAttention(MyReportFunctions.AttentionMessage.ArrayZeroLength);
                    return new T[0];
                }
                if (seq_length == 0)
                {
                    MyReportFunctions.ReportAttention(MyReportFunctions.AttentionMessage.NumberIsZero);
                    return new T[0];
                }
                T[] arr_out = new T[seq_length];
                for (Int32 i = 0; i < seq_length; i++)
                {
                    arr_out[i] = arr_in[random_for_numbers.Next(0, arr_in.Length)];
                }
                return arr_out;
            }
        }
        /// <summary>
        /// Written. 2024.01.01 17:38. Saint-Petersburg. Home. 
        /// </summary>
        public static class Volume
        {
            




            /// <summary>
            /// Written. 2024.01.01 18:52. Сантк-Петерубрг. Дома. 
            /// not tested.
            /// </summary>
            /// <param name="radius"></param>
            /// <returns></returns>
            public static double Sphere(double radius)
            {
                return ((double)4 / (double)3) * Math.PI * Math.Pow(radius, 3);
            }
            
            
            /// <summary>
            /// Written. 2024.01.01 18:30. Сантк-Петерубрг. Дома. 
            /// </summary>
            public static class Cilinder
            {
                /// <summary>
                /// Written. 2024.01.01 18:30. Saint-Petersburg. Home. 
                /// not tested.
                /// </summary>
                /// <param name="area"></param>
                /// <param name="height"></param>
                /// <returns></returns>
                public static double ByAreaHeight(double area, double height)
                {
                    return area * height;
                }
                /// <summary>
                /// Written. 2024.01.01 18:32. Saint-Petersburg. Home. 
                /// not tested.
                /// </summary>
                /// <param name="radius"></param>
                /// <param name="height"></param>
                /// <returns></returns>
                public static double ByRadiusHeight(double radius, double height)
                {
                    return Area.Cicrle(radius) * height;
                }
            }
        }



        /// <summary>
        /// Written. 2024.01.14 11:35. Moscow. Hostel.
        /// </summary>
        public static class Bar
        {

            /// <summary>
            /// Written. 2024.01.14 11:37. Moscow. Hostel 
            /// </summary>
            public static class Area
            {


                /// <summary>
                /// Written. 2024.01.14 11:48. Moscow. Hostel  
                /// not tested.
                /// </summary>
                /// <param name="x_values"></param>
                /// <param name="y_values"></param>
                /// <returns></returns>
                public static double AreaNBarsDouble(double[] x_values, double[] y_values)
                {
                    double area_bars = 0;
                    for (Int32 i = 0; i < x_values.Length - 1; i++)
                    {
                        area_bars += AreaDouble(x_values[i], y_values[i], x_values[i + 1], y_values[i + 1]);
                    }
                    return area_bars;

                }



                /// <summary>
                /// Written. 2024.01.14 11:43. Moscow. Hostel 
                /// not tested.
                /// </summary>
                /// <param name="x_values"></param>
                /// <param name="y_values"></param>
                /// <returns></returns>
                public static Int32 AreaNBarsInt32(int[] x_values, int[] y_values)
                {
                    Int32 area_bars = 0;
                    for (Int32 i = 0; i < x_values.Length - 1; i++)
                    {
                        area_bars += AreaInt32(x_values[i], y_values[i], x_values[i + 1], y_values[i + 1]);
                    }
                    return area_bars;

                }




                /// <summary>
                /// Written. 2024.01.14 12:14. Moscow. Hostel 
                /// /// not tested.
                /// </summary>
                /// <param name="x_values"></param>
                /// <param name="y_values"></param>
                /// <returns></returns>
                public static double AreaDouble(double x1, double y1, double x2, double y2)
                {
                    return (x2 - x1) * (y2 - y1);
                }








                /// <summary>
                /// Written. 2024.01.14 11:38. Moscow. Hostel
                /// not tested.
                /// </summary>
                /// <param name="x_values"></param>
                /// <param name="y_values"></param>
                /// <returns></returns>
                public static Int32 AreaInt32(Int32 x1, Int32 y1, Int32 x2, Int32 y2)
                {
                    return (x2 - x1) * (y2 - y1);
                }

            }



        }

            /// <summary>
            /// Written. 2024.01.12 15:56. Moscow. Workplace.
            /// </summary>
            public static class Line
        {

            // no need. 2024.01.12 16:35. Moscow. Workplace. 
            public class LineSlopeIntercept
            {
                public double Slope = 0;
                public double Intercept = 0;
            }


            /// <summary>
            /// Written. 2024.01.12 16:34. Moscow. Workplace. 
            /// </summary>
            /// <param name="x_values"></param>
            /// <param name="y_values"></param>
            /// <returns></returns>
            public static double AreaUnderNLines(int[] x_values, int[] y_values)
            {

                double area_out = 0;
                for (Int32 i = 0; i < x_values.Length - 1; i++)
                {
                    double[] line_a_b = SlopeInterceptFind(x_values[i], y_values[i], x_values[i + 1], y_values[i + 1]);
                    double area_under_line = AreaUnderLine(line_a_b, (double)x_values[i], (double)x_values[i + 1]);
                    area_out += area_under_line;
                }

                return area_out;
            }

                /// <summary>
                /// Written. 2024.01.12 16:15.  Moscow. Workplace.
                /// Tested. Works. 2024.01.13 13:47. Moscow. Hostel. 
                /// check if x1, x2 for line below x axis  
                /// </summary>
                /// <param name="LineParameters"></param>
                /// <param name="x1"></param>
                /// <param name="x2"></param>
                /// <returns></returns>
                public static double AreaUnderLine(double[] LineParameters, double x1, double x2)
            {
                double y1_find = LineParameters[0] * x1 + LineParameters[1];
                double y2_find = LineParameters[0] * x2 + LineParameters[1];
                double area_out = 0;
                // case line intercept x axis. 2024.01.13 13:52. Moscow. Hostel.
                if (((y1_find >= 0) && (y2_find < 0)) ||
                    ((y1_find < 0) && (y2_find >= 0)))
                {
                    double x_intercept = (-LineParameters[1]) / LineParameters[0];
                    double area_rec_triangle_1 = 0.5 * (x_intercept - x1) * y1_find;
                    double area_rec_triangle_2 = 0.5 * (x2 - x_intercept) * y2_find;
                    area_out = area_rec_triangle_1 + area_rec_triangle_2;
                }

                // case line above x axis. 2024.01.13 14:05. Moscow. Hostel. 
                if ((y1_find >= 0) && (y2_find >= 0)) 
                  
                {
                    double slope_area = System.Math.Abs((y2_find - y1_find)) * (x2 - x1) * 0.5;
                    double rec_area = 0;
                    if (y1_find <= y2_find)
                    {
                        rec_area = y1_find * (x2 - x1);
                    }
                    else
                    {
                        rec_area = y2_find * (x2 - x1);
                    }

                    area_out = slope_area + rec_area;
                }


                // case line below x axis. 2024.01.13 14:10. Moscow. Hostel.  
                // using System.Math.Abs I assume allows to not use this case. 2024.01.13 14:12. Moscow. Hostel. 
                if ((y1_find <= 0) && (y2_find <= 0))

                {
                    double slope_area = System.Math.Abs((y2_find - y1_find)) * (x2 - x1) * 0.5;
                    double rec_area = 0;
                    if (y1_find <= y2_find)
                    {
                        rec_area = y2_find * (x2 - x1);
                    }
                    else
                    {
                        rec_area = y1_find * (x2 - x1);
                    }

                    area_out = slope_area + rec_area;
                }

                return area_out;
            }



            /// <summary>
            /// Written. 2024.01.12 15:58. Moscow. Workplace. 
            /// </summary>
            /// <param name="x1"></param>
            /// <param name="y1"></param>
            /// <param name="x2"></param>
            /// <param name="y2"></param>
            /// <returns></returns>
            public static double[] SlopeInterceptFind(Int32 x1, Int32 y1, Int32 x2, Int32 y2)
            {
                // Slope can be small therefore calculation in double. 2024.01.12 15:58. Moscow. Workplace. 
                double x1_double = (double)x1;
                double y1_double = (double)y1;
                double x2_double = (double)x2;
                double y2_double = (double)y2;
                double slope = (y2_double - y1_double) / (x2_double - x1_double);
                double intercept = y1_double - slope*x1_double;
                double[] arr_out = new double[2];
                arr_out[0] = slope;
                arr_out[1] = intercept;
                return arr_out;
            }
        }


        /// <summary>
        /// Written. 2023.12.13 11:21. Moscow. Workplace 
        /// </summary>
        public static class Area
        {



            /// <summary>
            /// Written. 2024.01.12 16:12. Moscow. Workplace.
            /// not tested
            /// </summary>
            /// <param name="side1"></param>
            /// <param name="side2"></param>
            /// <returns></returns>
            public static double RectungularTriangle(double side1, double side2)
            {
                return (side1 * side2) / ((double)2);
            }

            /// <summary>
            /// Written. 2024.01.12 16:07. Moscow. Workplace.
            /// not tested
            /// </summary>
            /// <param name="side1"></param>
            /// <param name="side2"></param>
            /// <returns></returns>
            public static double Rectungular(double side1, double side2)
            {
                return side1 * side2;
            }


            /// <summary>
            /// Return area of n-side polygon <br></br>
            /// Written. 2024.01.12 13:55. Moscow. Workplace. <br></br>
            /// Tested. Works. 2024.01.12 14:16. Moscow. Workplace.
            /// </summary>
            /// <param name="sides_number"></param>
            /// <param name="side"></param>
            /// <returns></returns>
            public static double PolygonNSides(Int32 sides_number, double side)
            {
                // Step by step. 2024.01.12 14:00. Moscow. Workplace.
                double devident = Math.Pow(side, 2) * ((double)sides_number);
                double devisor = ((double)4) * Math.Tan((Math.PI / (double)180) * (((double)180) / ((double)sides_number)));

                double res_out = devident / devisor;
                return res_out;
            }


            /// <summary>
            /// Calculates the area of hexagon by provided length of the side <br></br>
            /// Written. 2024.01.11 15:32. Moscow. Workplace. <br></br>
            /// Tested. Works. 2024.01.12 14:16. Moscow. Workplace.
            /// </summary>
            /// <param name="side"></param>
            /// <returns></returns>
            public static double Hexagon(double side)
            {
                return (((double)3) * Math.Sqrt(3) * Math.Pow(side, 2)) / ((double)2);
            }


            /// <summary>
            /// Return area of square by using side of the square <br></br>
            /// Written. 2023.12.13 11:32. Workplace <br></br>
            /// Tested. Works. 2023.12.13 11:34. Workplace
            /// </summary>
            /// <param name="side"></param>
            /// <returns></returns>
            public static double Square(double side)
            {
                return Math.Pow(side, 2);
            }
            /// <summary>
            /// Return area of circle by using the radius of the circle <br></br>
            /// Written. 2023.12.13 11:35. Workplace <br></br>
            /// Tested. Works. 2023.12.13 11:36. Workplace
            /// </summary>
            /// <param name="side"></param>
            /// <returns></returns>
            public static double Cicrle(double radius)
            {
                if (radius < 0)
                {
                    MyReportFunctions.ReportError(MyReportFunctions.ErrorMessage.Number_Is_Less_Than_Zero);
                    return 0;
                }
                return Math.Pow(radius, 2) * Math.PI;
            }
            /// <summary>
            /// Return area of triangle by using 3 sides of the triangle <br></br>
            /// Written. 2023.12.13 11:21. Workplace <br></br>
            /// Tested. 2023.12.13 11:27. Workplace
            /// </summary>
            /// <param name="side1"></param>
            /// <param name="side2"></param>
            /// <param name="side3"></param>
            /// <returns></returns>
            public static double Triangle(double side1, double side2, double side3)
            {
                double semiperimeter = (side1 + side2 + side3) / 2;
                double area = Math.Sqrt(semiperimeter * (semiperimeter - side1) * (semiperimeter - side2) * (semiperimeter - side3));
                return area;
            }

            /* No need. There is another formula = 1/2*a*b. 2024.01.13 13:39. Moscow. Hostel. 
            /// <summary>
            /// Return area of rectungular triangle by using 2 sides of the triangle <br></br>
            /// Written. 2023.12.13 11:25. Workplace <br></br>
            /// Tested. 2023.12.13 11:27. Workplace
            /// </summary>
            /// <param name="side_base"></param>
            /// <param name="side_height"></param>
            /// <returns></returns>
            public static double RectungularTriangle(double side_base, double side_height)
            {
                double side3 = Math.Sqrt(Math.Pow(side_base, 2) + Math.Pow(side_height, 2));
                return Triangle(side_base, side_height, side3);
            }
        
            */
        }
        /// <summary>
        /// Written. 2023.12.21 11:49. Workplace.
        /// </summary>
        public static class TestFunctions
        {
           
            
            
            
            /// <summary>
            /// The functions test what number will be if negative number is converted into unsigned number. <br></br>
            /// UInt16, Int16 is used because the number is max 65535 and 32767 for Int16. 16 bit is in use and it causes positive to become negative.
            /// Written. 2023.12.21 11:49. Workplace.
            /// </summary>
            public static void ConversionInt16ToUInt16()
            {
                Int16 Int16Negative = -1000;
                Console.Write("Via (UInt16) ");
                Console.Write(Int16Negative);
                Console.Write(" ");
                // Tested. It does bit casting - negative number becomes positive according to bits. 2023.12.21 11:55. Workplace.
                Console.WriteLine((UInt16)Int16Negative);
                // Tested. If the Int16 is negative then there is conversion error. Conversion is ok if Int16 is positive. 2023.12.21 12:05. Workplace.
                Int16Negative = 2000;
                Console.Write("Via Convert.ToUInt16 ");
                Console.Write(Int16Negative);
                Console.Write(" ");
                UInt16 Number2 = System.Convert.ToUInt16(Int16Negative);
                Console.WriteLine(Number2);
                // Tested. Casts negative number to positive according to bits. 2023.12.21 12:14. Workplace. 
                Int16Negative = -2000;
                Console.Write("Via << ");
                Console.Write(Int16Negative);
                Console.Write(" ");
                byte[] Int16Bytes = new byte[2];
                Int16Bytes[0] = (byte)(Int16Negative >> 8);
                Int16Bytes[1] = (byte)(Int16Negative >> 0);
                Number2 = (UInt16)(Int16Bytes[0] << 8);
                Number2 |= (UInt16)(Int16Bytes[1] << 0);
                Console.WriteLine(Number2);
            }
        }
        public static class RandomDistribution
        {
            static System.Random rand_number = new System.Random(100);
            /// <summary>
            /// The function return int[][] filled with random numbers that are distirbuted
            /// according to defined distirbution <br></br>
            /// 2023.10.09 18:13. Written. Moscow. Workplace.  <br> </br>
            /// 2023.10.31 13:28. Tested. Works. Moscow. Workplace. 
            /// </summary>
            /// <param name="rows"></param>
            /// <param name="cols"></param>
            /// <param name="min_num"></param>
            /// <param name="max_num"></param>
            /// <returns></returns>
            public static int[][] DefinedDistribution2D(Int32 rows, Int32 cols, int[][] numbers_with_repeat_count)
            {
                // 1. amount of numbers
                Int32 amount_of_int32 = rows * cols;
                // 2. numbers to use in array                
                int[] numbers_for_array = numbers_with_repeat_count[0];
                // 3. counting number repeats                
                int[] repeteat_counting = new int[numbers_for_array.Length];
                // 4. repeat counter
                Int32 repeat_current = 1;
                for (Int32 i = 0; i < numbers_for_array.Length; i++)
                {
                    repeteat_counting[i] = 0;
                }
                int[][] arr_out = new int[cols][];
                // 5. filling array. 
                //
                // algorithm.
                //
                // 1. use number until repeated max times
                //
                // 2. reset repeat counter for numbers
                // if all numbers are repeated max times.
                for (Int32 i = 0; i < cols; i++)
                {
                    arr_out[i] = new int[rows];
                    for (Int32 j = 0; j < rows; j++)
                    {
                        Int32 num_for_arr = numbers_for_array[rand_number.Next(0, numbers_for_array.Length)];
                        Int32 index_num = Array.IndexOf(numbers_for_array, num_for_arr);
                        if (index_num == -1)
                        {
                            MyReportFunctions.ReportError("The error should not appear. Check the code");
                            return new int[0][];
                        }
                        if ((repeteat_counting[index_num] < repeat_current) &&
                            (repeteat_counting[index_num] < numbers_with_repeat_count[1][index_num]))
                        {
                            repeteat_counting[index_num] += 1;
                            arr_out[i][j] = num_for_arr;
                        }
                        else
                        {
                            bool down_number_found = false;
                            bool up_number_found = false;
                            // from number up to numbers
                            for (Int32 h = index_num; h < numbers_for_array.Length; h++)
                            {
                                if ((repeteat_counting[h] < repeat_current) &&
                                    (repeteat_counting[h] < numbers_with_repeat_count[1][h]))
                                {
                                    repeteat_counting[h] += 1;
                                    arr_out[i][j] = numbers_for_array[h];
                                    down_number_found = true;
                                    break;
                                }
                            }
                            // from number down to numbers
                            if (down_number_found == false)
                            {
                                for (Int32 h = index_num; h >= 0; h--)
                                {
                                    if ((repeteat_counting[h] < repeat_current) &&
                                        (repeteat_counting[h] < numbers_with_repeat_count[1][h]))
                                    {
                                        repeteat_counting[h] += 1;
                                        arr_out[i][j] = numbers_for_array[h];
                                        up_number_found = true;
                                        break;
                                    }
                                }
                            }
                            // reset counter if it is needed                            
                            if ((down_number_found == false) &&
                                    (up_number_found == false))
                            {
                                MyReportFunctions.ReportError("This error should not appear. check the code");
                                return new int[0][];
                            }
                        }
                        bool is_number_select = false;
                        for (Int32 ic = 0; ic < repeteat_counting.Length; ic++)
                        {
                            if ((repeteat_counting[ic] < repeat_current) &&
                                (numbers_with_repeat_count[1][ic] >= repeat_current))
                            {
                                is_number_select = true;
                                break;
                            }
                        }
                        if (is_number_select == false)
                        {
                            repeat_current++;
                            bool is_repeat_max = true;
                            for (Int32 ic = 0; ic < repeteat_counting.Length; ic++)
                            {
                                if (numbers_with_repeat_count[1][ic] >= repeat_current)
                                {
                                    is_repeat_max = false;
                                    break;
                                }
                            }
                            if (is_repeat_max == true)
                            {
                                repeat_current = 1;
                                for (Int32 ic = 0; ic < repeteat_counting.Length; ic++)
                                {
                                    repeteat_counting[ic] = 0;
                                }
                            }
                        }
                        /*
                            bool down_free_repeat_found = false;
                            bool up_free_repeat_found = false;
                            for (Int32 h = index_num; h < numbers_for_array.Length; h++)
                            {
                                if (repeteat_counting[index_num] < numbers_with_repeat_count[1][index_num])
                                {
                                    repeteat_counting[h] += 1;
                                    arr_out[i][j] = numbers_for_array[h];
                                    down_free_repeat_found = true;
                                    break;
                                }
                            }
                            if (down_free_repeat_found == false)
                            {
                                for (Int32 h = index_num; h > 0; h--)
                                {
                                    if (repeteat_counting[index_num] < numbers_with_repeat_count[1][index_num])
                                    {
                                        repeteat_counting[h] += 1;
                                        arr_out[i][j] = numbers_for_array[h];
                                        up_free_repeat_found = true;
                                        break;
                                    }
                                }
                            }
                            if ((down_free_repeat_found == false) &&
                                (up_free_repeat_found == false))
                            {
                                for (Int32 n = 0; n < numbers_for_array.Length; n++)
                                {
                                    repeteat_counting[n] = 1;
                                }
                                arr_out[i][j] = num_for_arr;
                                repeteat_counting[index_num] += 1;
                                repeat_current = 1;
                            }
                        */
                    }
                }
                // end of function.
                // returns array int[][]
                // array size is Rows x Cols.
                // numbers are all random. numbers are int.
                return arr_out;
            }
            /// <summary>
            /// The function return int[][] filled with random numbers that are distirbuted
            /// equally between min and max <br></br>
            /// 2023.10.08 13:00. Written. Moscow. Hostel. жд. ст. Ботанический сад. <br> </br>
            /// 2023.10.09 17:25. Tested. Works.
            /// </summary>
            /// <param name="rows"></param>
            /// <param name="cols"></param>
            /// <param name="min_num"></param>
            /// <param name="max_num"></param>
            /// <returns></returns>
            public static int[][] FlatDistribution2D(Int32 rows, Int32 cols, Int32 min_num, Int32 max_num)
            {
                // 2023.10.07 15:56. times to repeat
                Int32 number_of_int32 = rows * cols;
                // 2023.10.07 15:58. repeat couting
                int[] numbers_for_arr = new int[max_num - min_num + 1];
                int[] repeteat_count = new int[numbers_for_arr.Length];
                Int32 repeat_current = 1;
                for (Int32 i = 0; i < numbers_for_arr.Length; i++)
                {
                    repeteat_count[i] = 0;
                }
                for (Int32 i = 0; i < numbers_for_arr.Length; i++)
                {
                    numbers_for_arr[i] = min_num + i;
                }
                int[][] arr_out = new int[cols][];
                for (Int32 i = 0; i < cols; i++)
                {
                    arr_out[i] = new int[rows];
                    for (Int32 j = 0; j < rows; j++)
                    {
                        Int32 num_for_arr = rand_number.Next(min_num, max_num + 1);
                        Int32 index_num = Array.IndexOf(numbers_for_arr, num_for_arr);
                        if (index_num == -1)
                        {
                            MyReportFunctions.ReportError("The error should not appear. Check the code");
                            return new int[0][];
                        }
                        if (repeteat_count[index_num] <= repeat_current)
                        {
                            repeteat_count[index_num] += 1;
                            arr_out[i][j] = num_for_arr;
                        }
                        else
                        {
                            bool down_number_found = false;
                            bool up_number_found = false;
                            for (Int32 h = index_num; h < numbers_for_arr.Length; h++)
                            {
                                if (repeteat_count[h] <= repeat_current)
                                {
                                    repeteat_count[h] += 1;
                                    arr_out[i][j] = numbers_for_arr[h];
                                    down_number_found = true;
                                    break;
                                }
                            }
                            if (down_number_found == false)
                            {
                                for (Int32 h = index_num; h > 0; h--)
                                {
                                    if (repeteat_count[h] <= repeat_current)
                                    {
                                        repeteat_count[h] += 1;
                                        arr_out[i][j] = numbers_for_arr[h];
                                        up_number_found = true;
                                        break;
                                    }
                                }
                            }
                            if ((down_number_found == false) &&
                                    (up_number_found == false))
                            {
                                repeteat_count[index_num] += 1;
                                arr_out[i][j] = num_for_arr;
                                repeat_current += 1;
                            }
                        }
                    }
                }
                return arr_out;
            }
        }



        /// <summary>
        /// Written. 2024.01.11 15:26. Moscow. Workplace.
        /// </summary>
        public static class FloatNumber
        {
            /// <summary>
            /// Written. 2024.01.11 15:26. Moscow. Workplace. 
            /// </summary>
            public static class Convert
            {
                /// <summary>
                /// Written. 2024.01.11 15:26. Moscow. Workplace. <br></br>
                /// Tested. Works. 2024.01.11 16:30. Moscow. Workplace. <br></br>
                /// <br></br>
                /// Note. bit for sign. for 0 there may be that bit which is important for example for 1, 20 and -1, -20. <br></br>
                /// For 0 the bit does not change the value. Zero stays 0. <br></br>
                /// 32 bit in float is sign bit.
                /// </summary>
                /// <param name="num_in"></param>
                /// <returns></returns>
                unsafe public static UInt32 ToUInt32ByBitConversion(float num_in)
                {
                    float num_bits = num_in;                    
                    uint* uint_ptr = (uint*)(&num_bits);
                    UInt32 num_out = (*uint_ptr);
                    return num_out;
                }
            }
        }


        /// <summary>
        /// Written. 2024.01.11 15:21. Moscow. Workplace.
        /// </summary>
        public static class UInt32Number
        {

            /// <summary>
            /// Converts 4 bytes into UInt32.
            /// Written. 2024.01.16 08:54. Moscow. Workplace.
            /// not tested.
            /// </summary>
            /// <param name="bytes_in"></param>
            /// <returns></returns>
            public static UInt32 BytesToUInt32(byte[] bytes_in)
            {
                if (bytes_in.Length < 4)
                {
                    MyReportFunctions.ReportError(MyReportFunctions.ErrorMessage.Length_is_Wrong);
                    return 0;
                }
                
                
                UInt32 num_out = 0;
                num_out |= (uint)(bytes_in[0] << 24);
                num_out |= (uint)(bytes_in[1] << 16);
                num_out |= (uint)(bytes_in[2] << 8);
                num_out |= (uint)(bytes_in[3] << 0);
                return num_out;
            }



            /// <summary>
            /// Written. 2024.01.11 15:21. Moscow. Workplace. 
            /// </summary>
            public static class Convert
            {
                /// <summary>
                /// Written. 2024.01.11 15:22. Moscow. Workplace.
                /// Tested. Works. 2024.01.11 16:39. Moscow. Workplace.
                /// </summary>
                /// <param name="num_in"></param>
                /// <returns></returns>
                unsafe public static float ToFloatByBitConversion(UInt32 num_in)
                {
                    UInt32 num_bits = num_in;
                    float* float_ptr = (float*)(&num_bits);
                    float num_out = *float_ptr;
                    return num_out;
                }
            }
        }


        /// <summary>
        /// 2023.09.28 13:47. written.
        /// </summary>
        public static class Int32Number
        {

            /// <summary>
            /// Written. 2024.02.18 14:16. Moscow. Hostel.
            /// </summary>
            public static class TestFunctions
            {
                
                /// <summary>
                /// Written. 2024.02.18 14:16. Moscow. Hostel. <br></br>
                /// Tested. There were no overflow error. <br></br>
                /// Note. There were overflow error. Maybe it was from function Array.Sum()
                /// </summary>
                public static void Int32Overflow()
                {
                    Int32 test_int32 = int.MaxValue - 2;
                    for (Int32 i = 0; i < 10; i++)
                    {
                        try
                        {
                            test_int32 += (int.MaxValue - 1000);
                            Console.WriteLine(test_int32.ToString());
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                            Console.WriteLine("Exception. Number is " + test_int32.ToString());
                        }

                    }
                }
            }


            /// <summary>          
            /// Tested. Works. 2023.12.15 14:39. Workplace.
            /// </summary>
            /// Moved from MyMathFunction to MyMathFunction.Int32Number. 2024.01.16 08:53. Moscow. Workplace.
            /// <param name="bytes_in"></param>
            /// <returns></returns>
            public static Int32 BytesToInt32(byte[] bytes_in)
            {
                Int32 num_out = 0;
                num_out |= (bytes_in[0] << 24);
                num_out |= (bytes_in[1] << 16);
                num_out |= (bytes_in[2] << 8);
                num_out |= (bytes_in[3] << 0);
                return num_out;
            }
        }




        /// <summary>
        /// 2023.09.28 13:42. written.
        /// 2023.09.29 10:00. tested. works.
        /// </summary>
        public static class Convert
            {
                unsafe public static float ToFloat(Int32 num_in)
                {
                    UInt32 num_bits = (uint)num_in;
                    float* float_ptr = (float*)(&num_bits);
                    float num_out = *float_ptr;
                    return *float_ptr;
                }
            }
        
        /// <summary>
        /// Check what numbers will be during overflow. <br></br>
        /// Error, Keep return max, Start from 0. <br></br>
        /// Requires Console. <br></br>
        /// 2023-07-26 11:56 <br></br>
        /// 
        /// 2023.09.09. Result. byte starts from 0
        /// 
        /// </summary>
        /// <param name="num_in"></param>
        /// <returns>2023-07-26 12:03. Start from 0 and keep adding</returns>
        public static void _Check_Byte_Overflow()
        {
            byte byte_num = 250;
            byte add_count = 10;
            byte add_step = 1;
            Console.WriteLine("To byte value " + byte_num.ToString() + "\r\n" +
                add_count.ToString() + " will be added with step " + add_step.ToString());
            Console.WriteLine(byte_num.ToString());
            for (Int32 i = 0; i < add_count; i++)
            {
                byte_num += add_step;
                Console.WriteLine(byte_num.ToString());
            }
        }
        public static Int32 DoubleInTextFormatToInt32(string num_in)
        {
            // 2023-04-20 17:33 not checked
            Int32 num_return = 0;
            string for_num = num_in.Replace(".", "").Replace(",", "");
            try
            {
                num_return = System.Convert.ToInt32(for_num);
            }
            catch
            {
                MyReportFunctions.ReportError("Conversion failed. The number is " + num_in);
                return num_return;
            }
            return num_return;
        }
        public static int[] DoubleArrayInTextFormatToArrayInt32(string[] arr_in)
        {
            // 2023-04-20 17:33 not checked
            int[] arr_out = new int[arr_in.Length];
            for (Int32 i = 0; i < arr_in.Length; i++)
            {
                Int32 num_conversion = 0;
                string for_num = arr_in[i].Replace(".", "").Replace(",", "");
                try
                {
                    num_conversion = System.Convert.ToInt32(for_num);
                }
                catch
                {
                    MyReportFunctions.ReportError("Conversion failed at " + i.ToString()
                        + ". After that no conversion is done" + "\r\n" +
                        "The number is " + arr_in[i]);
                    return arr_out;
                }
            }
            return arr_out;
        }
        public static int[] StringArrayToArrayInt32(string[] arr_in)
        {
            // 2023-04-20 17:33 not checked
            int[] arr_out = new int[0];
            for (Int32 i = 0; i < arr_in.Length; i++)
            {
                try
                {
                    arr_out[i] = System.Convert.ToInt32(arr_in[i]);
                }
                catch
                {
                    MyReportFunctions.ReportError("Conversion failed at " + i.ToString()
                        + ". After that no conversion is done");
                    return arr_out;
                }
            }
            return arr_out;
        }
        /// <summary>
        /// 2023.09.09 13:45. written.
        /// </summary>
        public static class Average
        {
            /// <summary>
            /// 2023.04.26 14:21. written. <br></br>
            /// 2023.09.09 14:37. tested. works.
            /// </summary>
            /// <param name="arr_in"></param>
            /// <returns></returns>
            /// <remarks>2023.09.10 11:39 naming not finished</remarks>
            //
            // 2023.09.09 15:15. The choice is Calculate. There is nothing difficult
            // responsability in the word.
            // I expect that function will be used after check is done and the data
            // is correct. the according words will be used in the functions.
            //
            // Get. Standard
            // GetNumber. Not serious
            // Calculate. Math direction.
            // OfInt32Array. Direction to data processing and not to math calculation
            // Find. Importance 2. this can be used for more difficult task.
            // importance 3. if the words is more serious than the task I noticed it is not good.
            //
            public static Int32 AllNumbers(int[] arr_in)
            {
                if (arr_in.Length == 0)
                {
                    MyReportFunctions.ReportError("Devide by 0");
                    return 0;
                }
                return arr_in.Sum() / arr_in.Length;
            }
            /// <summary>
            /// 2023.09.10 01:22. ?? written.
            /// 2023.09.10 01:21. tested. works.
            /// </summary>
            /// <param name="arr_in"></param>
            /// <param name="interval_step"></param>
            /// <returns></returns>
            public static int[][] Step(int[] arr_in, Int32 interval_step = 2)
            {
                if ((arr_in.Length == 0) || (interval_step == 0))
                {
                    MyReportFunctions.ReportError("Devide by 0");
                    return new int[0][];
                }
                // counting 1st
                Int32 div_rem = 0;
                System.Math.DivRem(arr_in.Length, interval_step, out div_rem);
                Int32 steps_of_distribution = System.Math.Abs(arr_in.Length / interval_step);
                if (div_rem != 0)
                {
                    steps_of_distribution += 1;
                }
                int[][] arr_out = new int[2][];
                arr_out[0] = new int[steps_of_distribution];
                arr_out[1] = new int[steps_of_distribution];
                for (Int32 i = 0; i < steps_of_distribution; i++)
                {
                    Int32 interval_to_measure = (i + 1) * interval_step;
                    Int32 sum_is = 0;
                    Int32 start_index = (i + 1 - 1) * interval_step;
                    // 2023.09.10 01:15. last step check - full or not
                    if (i == (steps_of_distribution - 1))
                    {
                        if (div_rem != 0)
                        {
                            interval_to_measure = arr_in.Length;
                            for (Int32 j = start_index; j < interval_to_measure; j++)
                            {
                                sum_is += arr_in[j];
                            }
                            arr_out[0][i] = interval_to_measure;
                            arr_out[1][i] = sum_is / div_rem;
                            break;
                        }
                    }
                    for (Int32 j = start_index; j < interval_to_measure; j++)
                    {
                        sum_is += arr_in[j];
                    }
                    arr_out[0][i] = interval_to_measure;
                    arr_out[1][i] = sum_is / interval_step;
                }
                return arr_out;
            }
            /// <summary>
            /// 2023.09.10 11:33. ?? written
            /// 2023.09.10 11:33. tested. works.
            /// </summary>
            /// <param name="arr_in"></param>
            /// <param name="interval_step"></param>
            /// <returns></returns>
            public static int[][] Dependence(int[] arr_in, Int32 interval_step = 1)
            {
                if (arr_in.Length == 0)
                {
                    MyReportFunctions.ReportError("Devide by 0");
                    return new int[0][];
                }
                // counting 1st
                Int32 div_rem = 0;
                System.Math.DivRem(arr_in.Length, interval_step, out div_rem);
                Int32 steps_of_distribution = System.Math.Abs(arr_in.Length / interval_step);
                if (div_rem != 0)
                {
                    steps_of_distribution += 1;
                }
                int[][] arr_out = new int[2][];
                arr_out[0] = new int[steps_of_distribution];
                arr_out[1] = new int[steps_of_distribution];
                for (Int32 i = 0; i < steps_of_distribution; i++)
                {
                    Int32 interval_to_measure = (i + 1) * interval_step;
                    Int32 sum_is = 0;
                    if (i == (steps_of_distribution - 1))
                    {
                        if (div_rem != 0)
                        {
                            interval_to_measure = arr_in.Length;
                            for (Int32 j = 0; j < interval_to_measure; j++)
                            {
                                sum_is += arr_in[j];
                            }
                            arr_out[0][i] = interval_to_measure;
                            arr_out[1][i] = sum_is / div_rem;
                            break;
                        }
                    }
                    for (Int32 j = 0; j < interval_to_measure; j++)
                    {
                        sum_is += arr_in[j];
                    }
                    arr_out[0][i] = interval_to_measure;
                    arr_out[1][i] = sum_is / interval_to_measure;
                }
                return arr_out;
            }
        }
        public static byte[] UInt32ToBytes(UInt32 value_in)
        {
            byte[] arr_out = new byte[4];
            arr_out[0] = (byte)(value_in >> 24);
            arr_out[1] = (byte)(value_in >> 16);
            arr_out[2] = (byte)(value_in >> 8);
            arr_out[3] = (byte)(value_in >> 0);
            return arr_out;
        }
        /// <summary>
        /// Tested. Works. 2023.12.15 12:45. Workplace.
        /// </summary>
        /// <param name="value_in"></param>
        /// <returns></returns>
        public static byte[] Int32ToBytes(Int32 value_in)
        {
            byte[] arr_out = new byte[4];
            arr_out[0] = (byte)(value_in >> 24);
            arr_out[1] = (byte)(value_in >> 16);
            arr_out[2] = (byte)(value_in >> 8);
            arr_out[3] = (byte)(value_in >> 0);
            return arr_out;
        }
        
        public static Int16 BytesToInt16(byte[] bytes_in)
        {
            Int16 num_out = 0;
            num_out |= (Int16)(bytes_in[0] << 8);
            num_out |= (Int16)(bytes_in[1] << 0);
            return num_out;
        }
        public static UInt64 Int64RandomNumberNewInArray(UInt64[] array_in)
        {
            Random rand_num = new Random();
            UInt64 number_out = (UInt64)rand_num.Next(0x11111111, int.MaxValue) + ((UInt64)rand_num.Next(0x11111111, int.MaxValue) << 32);
            while (Array.IndexOf(array_in, number_out) != -1)
            {
                number_out = (UInt64)rand_num.Next(0x11111111, int.MaxValue) + ((UInt64)rand_num.Next(0x11111111, int.MaxValue) << 32);
            }
            return number_out;
        }
        public static int[] Int32RandomArray(Int32 min_num, Int32 max_num, Int32 arr_size)
        {
            Random rand_num = new Random();
            DateTime for_seed = DateTime.Now;
            rand_num = new Random(for_seed.Second + for_seed.Millisecond);
            int[] arr_out = new int[arr_size];
            for (Int32 i = 0; i < arr_size; i++)
            {
                arr_out[i] = rand_num.Next(min_num, max_num);
            }
            Thread.Sleep(1);
            return arr_out;
        }
        public void BoolArrayToState(ref bool[] arr_in, bool state_in = false)
        {
            for (Int32 i = 0; i < arr_in.Length; i++)
            {
                arr_in[i] = state_in;
            }
        }
        public static int[] Int32SubFromInt32Array(int[] array_in, Int32 num_in, Int32 min_value = 0)
        {
            int[] for_return = new int[array_in.Length];
            try
            {
                for (Int32 i = 0; i < for_return.Length; i++)
                {
                    for_return[i] = array_in[i] - num_in;
                    if (for_return[i] < min_value)
                    {
                        for_return[i] = min_value;
                    }
                }
            }
            catch
            {
                MyReportFunctions.ReportError();
            }
            return for_return;
        }
        public static int[] Int32SubFromInt32Array(int[] array_in, Int32 num_in)
        {
            int[] for_return = new int[array_in.Length];
            try
            {
                for (Int32 i = 0; i < for_return.Length; i++)
                {
                    for_return[i] = array_in[i] - num_in;
                }
            }
            catch
            {
                MyReportFunctions.ReportError();
            }
            return for_return;
        }
        public static UInt64 Int64RandomNumberNewInArray(UInt64[] array_in, UInt64 max_num = 0)
        {
            Random rand_num = new Random();
            UInt64 number_out = 0;
            if (max_num == 0)
            {
                number_out = (UInt64)rand_num.Next(0x11111111, int.MaxValue) + ((UInt64)rand_num.Next(0x11111111, int.MaxValue) << 32);
                while (Array.IndexOf(array_in, number_out) != -1)
                {
                    number_out = (UInt64)rand_num.Next(0x11111111, int.MaxValue) + ((UInt64)rand_num.Next(0x11111111, int.MaxValue) << 32);
                }
            }
            else
            {
                number_out = Int64RandomInterval(max_num);
                while (Array.IndexOf(array_in, number_out) != -1)
                {
                    number_out = (UInt64)rand_num.Next(0x11111111, int.MaxValue) + ((UInt64)rand_num.Next(0x11111111, int.MaxValue) << 32);
                }
            }
            return number_out;
        }
        public static UInt64 Int64RandomInterval(UInt64 no_more_than = 1000)
        {
            Random rand_num = new Random();
            UInt64 number_out = (UInt64)rand_num.Next(10, (int)no_more_than);
            return number_out;
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
        public static UInt64 Char8SymbolsToUInt64(char[] char_arr_in)
        {
            UInt64 num_out = 0;
            try
            {
                for (Int32 i = 0; i < char_arr_in.Length; i++)
                {
                    num_out |= (UInt64)char_arr_in[i] << 8 * (7 - i);
                }
            }
            catch
            {
                MyReportFunctions.ReportError();
            }
            return num_out;
        }
        public static string UInt64ToDateTimeString(UInt64 number_in)
        {
            string str_out = "";
            try
            {
                string str_number = System.Convert.ToString(number_in);
                str_out += System.Convert.ToString(str_number[0]); // 2
                str_out += System.Convert.ToString(str_number[1]); // 0
                str_out += System.Convert.ToString(str_number[2]); // 2
                str_out += System.Convert.ToString(str_number[3]); // 3
                str_out += System.Convert.ToString('.');           // .
                str_out += System.Convert.ToString(str_number[4]); // 1
                str_out += System.Convert.ToString(str_number[5]); // 2
                str_out += System.Convert.ToString('.'); // .
                str_out += System.Convert.ToString(str_number[6]); // 2
                str_out += System.Convert.ToString(str_number[7]); // 3
                str_out += System.Convert.ToString(' '); // 
                str_out += System.Convert.ToString(str_number[8]); // 1
                str_out += System.Convert.ToString(str_number[9]); // 4
                str_out += System.Convert.ToString(':'); // :
                str_out += System.Convert.ToString(str_number[10]); // 5
                str_out += System.Convert.ToString(str_number[11]); // 1
            }
            catch (Exception e)
            {
                MyReportFunctions.ReportError(e.Message);
            }
            return str_out;
        }
        public static char[] ByteArrayToCharArray(byte[] arr_in)
        {
            char[] arr_out = new char[0];
            try
            {
                arr_out = new char[arr_in.Length];
                for (Int32 i = 0; i < arr_in.Length; i++)
                {
                    arr_out[i] = System.Convert.ToChar(arr_in[i]);
                }
            }
            catch
            {
                MyReportFunctions.ReportError();
            }
            return arr_out;
        }
        public static bool[,] BoolSubArrayAxBFromByteArrayNxM(bool[,] arr_in, Int32 N_start, Int32 M_start, Int32 A_size, Int32 B_size)
        {
            bool[,] arr_out = new bool[0, 0];
            try
            {
                arr_out = new bool[A_size, B_size];
                for (Int32 i = N_start; i < N_start + A_size; i++)
                {
                    for (Int32 j = M_start; j < M_start + B_size; j++)
                    {
                        arr_out[i - N_start, j - M_start] = arr_in[i, j];
                    }
                }
            }
            catch
            {
                MyReportFunctions.ReportError();
            }
            return arr_out;
        }
        public static byte[,] ByteArrayToByteArrayNxM(byte[] arr_in, Int32 N_size, Int32 M_size = -1)
        {
            byte[,] arr_out = new byte[0, 0];
            if (N_size == 0)
            {
                MyReportFunctions.ReportError("devide by 0");
                return arr_out;
            }
            if (M_size == -1)
            {
                M_size = arr_in.Length / N_size;
            }
            if ((N_size * M_size) != arr_in.Length)
            {
                MyReportFunctions.ReportError("Array size does not fit the NxM array");
                return arr_out;
            }
            arr_out = new byte[N_size, M_size];
            for (Int32 j = 0; j < M_size; j++)
            {
                for (Int32 i = 0; i < N_size; i++)
                {
                    arr_out[i, j] = arr_in[j * N_size + i];
                }
            }
            return arr_out;
        }
        public static byte[,] ByteSubArrayAxBFromByteArrayNxM(byte[,] arr_in, Int32 N_start, Int32 M_start, Int32 A_size, Int32 B_size)
        {
            byte[,] arr_out = new byte[0, 0];
            try
            {
                arr_out = new byte[A_size, B_size];
                for (Int32 i = N_start; i < N_start + A_size; i++)
                {
                    for (Int32 j = M_start; j < M_start + B_size; j++)
                    {
                        arr_out[i - N_start, j - M_start] = arr_in[i, j];
                    }
                }
            }
            catch
            {
                MyReportFunctions.ReportError();
            }
            return arr_out;
        }
        public static void ByteArrayNxM_ToConsole(byte[,] arr_in)
        {
            for (Int32 j = 0; j < arr_in.GetLength(1); j++)
            {
                if (j != 0)
                {
                    Console.Write("\r\n");
                }
                for (Int32 i = 0; i < arr_in.GetLength(0); i++)
                {
                    if (i != 0)
                    {
                        Console.Write(" ");
                    }
                    string for_out = arr_in[i, j].ToString().PadRight(3, ' ');
                    Console.Write(for_out);
                }
            }
            Console.Write("\r\n");
        }
        public static void ByteArrayToConsole(byte[] arr_in)
        {
            for (Int32 j = 0; j < arr_in.Length; j++)
            {
                if (j != 0)
                {
                    Console.Write("\r\n");
                }
                Console.Write(arr_in[j].ToString());
            }
            Console.Write("\r\n");
        }
        public static void BoolArrayNxM_ToConsole(bool[,] arr_in)
        {
            for (Int32 j = 0; j < arr_in.GetLength(1); j++)
            {
                if (j != 0)
                {
                    Console.Write("\r\n");
                }
                for (Int32 i = 0; i < arr_in.GetLength(0); i++)
                {
                    if (i != 0)
                    {
                        Console.Write(" ");
                    }
                    string for_out = arr_in[i, j].ToString().PadRight(5, ' ');
                    Console.Write(for_out);
                }
            }
            Console.Write("\r\n");
        }
        public static void Int32ArrayNxM_ToConsole(int[][] arr_in)
        {
            for (Int32 j = 0; j < arr_in[0].Length; j++)
            {
                if (j != 0)
                {
                    Console.Write("\r\n");
                }
                for (Int32 i = 0; i < arr_in.Length; i++)
                {
                    //if (i != 0)
                    //{
                    //    Console.Write(" ");
                    //}
                    string for_out = arr_in[i][j].ToString().PadRight(13, ' ');
                    Console.Write(for_out);
                }
            }
            Console.Write("\r\n");
        }
        public static void Int32ArrayNxM_ToConsole(int[,] arr_in)
        {
            for (Int32 j = 0; j < arr_in.GetLength(1); j++)
            {
                if (j != 0)
                {
                    Console.Write("\r\n");
                }
                for (Int32 i = 0; i < arr_in.GetLength(0); i++)
                {
                    if (i != 0)
                    {
                        Console.Write(" ");
                    }
                    string for_out = arr_in[i, j].ToString().PadRight(13, ' ');
                    Console.Write(for_out);
                }
            }
            Console.Write("\r\n");
        }
        public static int[] Int32ArrayFromInt32ArrayNxM(int[][] arr_in, Int32 arr_num)
        {
            return arr_in[arr_num];
        }
        public static void Int32ArrayNxM_ToFile(int[,] arr_in, string filename_in, char delimer_in = '\t')
        {
            StreamWriter file_write = new StreamWriter(filename_in);
            for (Int32 j = 0; j < arr_in.GetLength(1); j++)
            {
                if (j != 0)
                {
                    file_write.Write("\r\n");
                }
                for (Int32 i = 0; i < arr_in.GetLength(0); i++)
                {
                    if (i != 0)
                    {
                        file_write.Write(delimer_in);
                    }
                    string for_out = arr_in[i, j].ToString();
                    file_write.Write(for_out);
                }
            }
            file_write.Close();
        }
    }
}
