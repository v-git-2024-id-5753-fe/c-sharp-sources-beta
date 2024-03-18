using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Commercial use (license)
// Please study about commercial use of the code that is publicly available 

namespace CharFunctionsNamespace
{

    // 2024.03.10 17:36. Moscow. Hostel.
    // to char data. 9x15 char of consolas 11. measured.


    public static class CharFunctions
    {

        /// <summary>
        /// Written. 2024.02.11 14:14. Moscow. Hostel.
        /// </summary>
        public static class Generate
        {
            
            /// <summary>
            /// Written. 2024.02.11 14:14. Moscow. Hostel.
            /// </summary>
            /// <returns></returns>
            public static char[] ASCIICharsArray()
            {
                char[] arr_out = new char[256];
                for (Int32 i = 0; i < arr_out.Length; i++)
                {
                    arr_out[i] = (char)i;
                }
                return arr_out;
            }

            /// <summary>
            /// Written. 2024.02.11 14:15. Moscow. Hostel.
            /// </summary>
            /// <returns></returns>
            public static string ASCIICharsString()
            {
                return new string(ASCIICharsArray());   
            }


        }



        /// <summary>
        /// 2023.10.06 13:41. Written. Moscow. Workplace жд. ст. Мытищи
        /// </summary>
        public static class Convert
        {
            public static Int16 ToInt16(char char_in)
            {
                return (Int16)char_in;
            }
            /// <summary>
            /// 2023.10.06 13:45. Written. Moscow. Workplace жд. ст. Мытищи <br></br>
            /// 2023.10.06 13:49. Tested. Works
            /// </summary>
            /// <param name="char_in"></param>
            /// <returns></returns>
            public static string SpecialCharacterToString(char char_in)
            {
                string str_out = "";
                if (char_in == '\t')
                {
                    str_out = "\\t";
                }
                if (char_in == '\n')
                {
                    str_out = "\\n";
                }
                if (char_in == '\r')
                {
                    str_out = "\\r";
                }
                if (char_in == '\0')
                {
                    str_out = "\\0";
                }
                return str_out;
            }
        }
    }
}
