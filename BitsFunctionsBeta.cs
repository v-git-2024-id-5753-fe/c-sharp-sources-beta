
// Commercial use (license)
// Please study about commercial use of the code that is publicly available

using System;


namespace BitsFunctionsNamespace
{

//2024.03.21 14:21. Moscow. Workplace.
// Casting in C# is from number to number and not by bits. For example, UInt32 cast to Int32 is by value and not by bits.
// There may be error if value is greater than Int32 can have. (this cast was not checked)

    class BitsFunctions
    {


        /// <summary>
        /// Written. 2024.03.21 16:37. Moscow. Workplace.
        /// </summary>
        /// <param name="num_in"></param>
        /// <returns></returns>
        public static int BitsReversed(int num_in)
        {

                
                int reverse = 0;
                for (int bit = 0; bit < 32; bit++)
                {
                    reverse <<= 1;
                    reverse |= (num_in & 1);
                num_in >>= 1;
                }

                return reverse;
            }



        

        public static byte BitsGet(UInt32 num_in, Int32 start_num, Int32 end_num)
        {
            byte byte_out = 0;
            byte byte_out_index = 0;
            for (Int32 i = start_num; i < end_num; i++)
            {
                if ((num_in & (1 << i)) > 0)
                {
                    byte_out |= (byte)(1 << byte_out_index);
                    byte_out_index++;
                }
            }
            return byte_out;
        }

        static public byte BitValue(byte byte_in, Int32 bit_num)
        {
            byte num_condition = (byte)(byte_in & (1 << bit_num));
            if (num_condition > 0)
            {
                return 1;
            }
            return 0;
        }
        static public byte CycleBitsRight(byte byte_in)
        {
            byte byte_out = byte_in;
            if (BitValue(byte_in, 0) == 1)
            {
                byte_out = (byte)(byte_out >> 1);
                byte_out |= (1 << 7);
            }
            else
            {
                byte_out = (byte)(byte_out >> 1);
            }
            return byte_out;
        }
        static public byte CycleBitsLeft(byte byte_in)
        {
            byte byte_out = byte_in;
            if (BitValue(byte_in, 7) == 1)
            {
                byte_out = (byte)(byte_out << 1);
                byte_out |= 1;
            }
            else
            {
                byte_out = (byte)(byte_out << 1);
            }
            return byte_out;
        }
    }
}
