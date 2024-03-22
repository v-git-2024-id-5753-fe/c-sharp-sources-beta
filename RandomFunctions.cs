using System;

// Commercial use (license)
// Please study about commercial use of the code that is publicly available 

// 2024.01.31 20:27. Moscow. Hostel.
namespace RandomFunctionsNamespace
{
    public static class RandomFunctions
    {
        static Random _internal_random = new Random();


        /// <summary>
        /// Written. 2024.01.31 20:57. Moscow. Hostel 
        /// </summary>
        /// <param name="arr_in"></param>
        /// <param name="amount_to_change"></param>
        /// <returns></returns>
        public static byte[] RandomBytesChanged(byte[] arr_in, UInt32 amount_to_change)
        {
            byte[] arr_out = new byte[arr_in.Length];            
            Array.Copy(arr_in, arr_out, arr_out.Length);
            Int32 cycle_run_time = (int)((float)arr_in.Length * ((float)(amount_to_change)/(float)100));
            Int32 arr_length = arr_in.Length;
            for (Int32 i = 0; i < cycle_run_time; i++)
            {
                Int32 arr_index = _internal_random.Next(0, arr_length);
                byte new_byte_value = (byte)_internal_random.Next(0, byte.MaxValue + 1);
                arr_out[arr_index] = new_byte_value;
            }
            return arr_out;
        }










        /// <summary>
        /// Written. 2024.01.31 20:34. Moscow. Hostel.
        /// </summary>
        public static class RandomSelect
        {
            /// <summary>
            /// Written. 2024.01.31 20:35. Moscow. Hostel.
            /// Tested. Works. 2024.01.31 20:50. Moscow. Hostel 
            /// </summary>
            public class String
            {
                string[] StringArray = null;
                public String(string[] arr_in)
                {
                    StringArray = arr_in;
                }
                public string Select()
                {
                    return StringArray[_internal_random.Next(0, StringArray.Length)];
                }
            }


        }
    }
}

