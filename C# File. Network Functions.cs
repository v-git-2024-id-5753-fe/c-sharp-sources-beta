using ArrayFunctionsNamespace;
using BitsFunctionsNamespace;
using FileFunctionsNamespace;
using MathFunctionsNamespace;
using NetworkFunctionsNamespace;
using ReportFunctionsNamespace;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Threading;

// Commercial use (license)
// Please study about commercial use of the code that is publicly available 

namespace NetworkFunctionsNamespace
{
    enum MY_TCP_CMD
    {
        DO_NOTHING,
        FILE_START,
        FILE_END,
        DATA
    }

    /// <summary>
    /// Written. 2024.02.18 11:44. Moscow. Hostel.
    /// </summary>
    class CRC32_Class
    {

        // 2024.02.18 12:07. Moscow. Hostel.
        // sum is can be ok checksum. crs32 is longer to compute.



        /*
         * CRC8:

public static class Crc8
{
    private static readonly byte[] Table = new byte[256];
    private const byte Poly = 0xd5;

    public static byte ComputeChecksum(params byte[] bytes)
    {
        byte crc = 0;
        if (bytes is {Length: > 0}) crc = bytes.Aggregate(crc, (current, b) => Table[current ^ b]);
        return crc;
    }

    static Crc8()
    {
        for (var i = 0; i < 256; ++i)
        {
            var temp = i;
            for (var j = 0; j < 8; ++j)
                if ((temp & 0x80) != 0)
                    temp = (temp << 1) ^ Poly;
                else
                    temp <<= 1;
            Table[i] = (byte) temp;
        }
    }
}
*/
        // 2024.02.18 12:19. Moscow. Hostel. CRC16 needs to be found

        public const UInt32 DefaultPolynomial = 0x04C11DB7;
        UInt32 internal_polynomial = DefaultPolynomial;
        public UInt32 Polynomial
        {
            get
            {
                return internal_polynomial;
            }
            set
            {
                internal_polynomial = value;
                CreateCRCReflectedTable(internal_polynomial);
            }
        }
        public UInt32[] CRS32_Reflected_Table = null;
        public CRC32_Class(UInt32 polynomial_in)
        {
            CRS32_Reflected_Table = CreateCRCReflectedTable(polynomial_in);
        }

        private UInt32[] CreateCRCReflectedTable(UInt32 polynomial)
        {
            UInt32[] createTable = new UInt32[256];

            // 2024.03.21 16:42. Moscow. Workplace.
            // for using shift right there is reversed polynomial - that is reversed bits in the number (1st, 2nd -> 32nd, 31st)
            UInt32 polynomial_reversed = (uint)BitsFunctions.BitsReversed((int)polynomial);
            for (UInt32 i = 0; i < 256; i++)
            {
                UInt32 entry = i;
                for (UInt32 j = 0; j < 8; j++)
                {
                    if ((entry & 1) == 1)
                    {
                        entry = (entry >> 1) ^ polynomial_reversed;
                    }
                    else
                    {
                        entry = entry >> 1;
                    }

                }
                createTable[i] = entry;
            }
            return createTable;
        }


        /// <summary>
        /// Calculates CRC32.<br></br>
        /// Written. 2024.03.21. 10:00 - 15:00. Moscow. Workplace. <br></br>
        /// Tested. Works. Comparison to 2 websites was used to check the result. 2024.03.22 10:20. Moscow. Workplace.
        /// </summary>
        /// <param name="byte_arr"></param>
        /// <returns></returns>
        public UInt32 ComputeChecksum(byte[] byte_arr)
        {
            // 2024.03.22 10:31. Moscow. Workplace.
            // Uses reflected CRC32 table. 
            var crc = 0xffffffff;
            foreach (var t in byte_arr)
            {
                var index = (byte)((crc & 0xff) ^ t);
                crc = (crc >> 8) ^ CRS32_Reflected_Table[index];
            }
            return ~crc;
        }

        public byte[] ComputeChecksumBytes(byte[] byte_arr)
        {
            return BitConverter.GetBytes(ComputeChecksum(byte_arr));
        }
    }

    /// <summary>
    /// Written. 2024.03.21. 10:00 - 15:00. Moscow. Workplace. <br></br>
    /// - Shows in console CRC32 reflected table. <br></br>
    /// - Shows in console CRC32 checksum of provided byte[]. <br></br>
    /// </summary>
    class CRC32Test
    {
        CRC32_Class crc32 = new CRC32_Class(CRC32_Class.DefaultPolynomial);
        public void ShowTable()
        {
            Console.Write(ArrayFunctions.UInt32Array.Convert.ToFileString(crc32.CRS32_Reflected_Table, 8, 16, " "));
        }
        public void SetPolynomial(uint num_in)
        {
            crc32.Polynomial = num_in;
        }

        public void ChecksumToConsole(byte[] byte_arr)
        {
            uint crc32_value = crc32.ComputeChecksum(byte_arr);
            Console.WriteLine(crc32_value.ToString());
        }
    }



    class TCPClientFunctionsClass
    {
        TcpClient ClientTCP = null;
        IPAddress ServerAddress = null;
        public byte[] PreparePacket(byte[] data_in, MY_TCP_CMD cmd_in, UInt32 start_of_packet = 0xAFFFFFFF, UInt32 end_of_packet = 0xBFFFFFFF)
        {
            byte[] arr_out = new byte[0];
            byte[] header_bytes = MathFunctions.UInt32ToBytes(start_of_packet);
            byte[] end_of_packet_bytes = MathFunctions.UInt32ToBytes(end_of_packet);
            byte[] cmd_bytes = new byte[1];
            cmd_bytes[0] = (byte)cmd_in;
            header_bytes = ArrayFunctions.ByteArray.MergeArrays(header_bytes, cmd_bytes);
            arr_out = ArrayFunctions.ByteArray.MergeArrays(header_bytes, data_in);
            arr_out = ArrayFunctions.ByteArray.MergeArrays(data_in, end_of_packet_bytes);
            return arr_out;
        }
        public void ServerConnect(string ip_address, Int32 port)
        {
            ServerAddress = IPAddress.Parse(ip_address);
            ClientTCP = new TcpClient();
            ClientTCP.Connect(ServerAddress, port);
            while (ClientTCP.Connected == false)
            {
                Thread.Sleep(100);
                Console.WriteLine("Waiting server");
            }
        }
        public void SendBytes(byte[] bytes_in)
        {
            ClientTCP.Client.Send(bytes_in);
        }
    }
    class MyTCPServerFunctionsClass
    {
        TcpListener ServerTCP = null;
        IPAddress ServerAddress = null;
        TcpClient ServerTcpClient = null;
        public static byte[] PreparePacket(byte[] data_in, MY_TCP_CMD cmd_in, UInt32 start_of_packet = 0xAFFFFFFF, UInt32 end_of_packet = 0xBFFFFFFF)
        {
            byte[] arr_out = new byte[0];
            byte[] header_bytes = MathFunctions.UInt32ToBytes(start_of_packet);
            byte[] end_of_packet_bytes = MathFunctions.UInt32ToBytes(end_of_packet);
            byte[] cmd_bytes = new byte[1];
            cmd_bytes[0] = (byte)cmd_in;
            header_bytes = ArrayFunctions.ByteArray.MergeArrays(header_bytes, cmd_bytes);
            arr_out = ArrayFunctions.ByteArray.MergeArrays(header_bytes, data_in);
            arr_out = ArrayFunctions.ByteArray.MergeArrays(data_in, end_of_packet_bytes);
            return arr_out;
        }
        public static bool CheckPacket(byte[] packet_in, UInt32 start_of_packet = 0xAFFFFFFF, UInt32 end_of_packet = 0xBFFFFFFF)
        {
            byte[] header_bytes = MathFunctions.UInt32ToBytes(start_of_packet);
            Int32 header_num = ArrayFunctions.ByteArray.IndexOf(packet_in, header_bytes);
            byte[] end_of_packet_bytes = MathFunctions.UInt32ToBytes(end_of_packet);
            Int32 end_of_packet_num = ArrayFunctions.ByteArray.IndexOf(packet_in, end_of_packet_bytes);
            bool bool_out = false;
            if ((header_num != -1) && (end_of_packet_num != -1))
            {
                bool_out = true;
            }
            return bool_out;
        }
        public static MY_TCP_CMD CommandGet(byte[] packet_in)
        {
            return (MY_TCP_CMD)packet_in[0];
        }
        public static byte[] DataFromPacket(byte[] packet_in, UInt32 start_of_packet = 0xAFFFFFFF, UInt32 end_of_packet = 0xBFFFFFFF)
        {
            byte[] arr_out = new byte[0];
            byte[] header_bytes = MathFunctions.UInt32ToBytes(start_of_packet);
            byte[] end_of_packet_bytes = MathFunctions.UInt32ToBytes(end_of_packet);
            Int32 header_index = ArrayFunctions.ByteArray.LastIndexOf(packet_in, header_bytes);
            Int32 end_of_packet_index = ArrayFunctions.ByteArray.IndexOf(packet_in, end_of_packet_bytes);
            arr_out = ArrayFunctions.ByteArray.ExtractArray.BelowIndex(packet_in, header_index);
            arr_out = ArrayFunctions.ByteArray.ExtractArray.AboveIndex(arr_out, end_of_packet_index);
            return arr_out;
        }
        public void ServerStart(string ip_address, Int32 port, Int32 buffer_size = 0x3FFFF)
        {
            ServerAddress = IPAddress.Parse(ip_address);
            ServerTCP = new TcpListener(ServerAddress, port);
            ServerTCP.Server.ReceiveBufferSize = buffer_size;
            ServerTCP.Server.SendBufferSize = buffer_size;
            Console.WriteLine("Server started");
            Console.WriteLine(ServerAddress.ToString() + " " + port.ToString());
            ServerTCP.Start();
        }
        public void AcceptClient()
        {
            Int32 msg_cnt = 0;
            const Int32 msg_cnt_max = 10;
            //System.Windows.Input.Key key_in = Key.Space;
            bool key_pressed = false;
            //while ((ServerTCP.Pending() == false) && (key_pressed == false))
            //{
            //    Thread.Sleep(100);
            //    msg_cnt++;
            //    if (msg_cnt == msg_cnt_max)
            //    {
            //        msg_cnt = 0;
            //        Console.WriteLine("Press" + " " + key_in.ToString() + " to return to menu");
            //    }
            //    if (Keyboard.IsKeyDown(System.Windows.Input.Key.Space) == true)
            //    {
            //        Console.WriteLine(key_in.ToString() + " is pressed");
            //        key_pressed = true;
            //    }
            //}
            if (key_pressed == false)
            {
                ServerTcpClient = ServerTCP.AcceptTcpClient();
                stream_for_client = ServerTcpClient.GetStream();
                byte[] ClientBytes = AcceptBytes();
                Console.WriteLine("accept bytes length" + " " + ClientBytes.ToString());
                FileFunctions.TextFile.BytesToConsole(ClientBytes);
                ClientBytes = AcceptBytes();
                Console.WriteLine("accept bytes length" + " " + ClientBytes.ToString());
                FileFunctions.TextFile.BytesToConsole(ClientBytes);
            }
        }
        public bool RX_Bytes_Tread_Stop = false;
        private void RX_Bytes_Tread_Function()
        {
            while (RX_Bytes_Tread_Stop == false)
            {
            }
        }
        NetworkStream stream_for_client = null;
        byte[] AcceptBytes()
        {
            byte[] for_return = new byte[3];
            stream_for_client.Read(for_return, 0, for_return.Length);
            //stream_for_client.Close();
            return for_return;
        }
    }
    static class NetworkFunctions
    {
        public class CheckSum
        {
            //public static byte[] SumBytes(byte[] bytes_in)
            //{
            //    UInt32 check_sum = SumBytes(bytes_in);
            //}
            public static class SumBytes
            {
                public static byte[] Bytes(byte[] bytes_in)
                {
                    return MathFunctions.UInt32ToBytes(SumBytes.Number(bytes_in));
                }
                public static UInt32 Number(byte[] bytes_in)
                {
                    UInt32 result = 0;
                    for (Int32 i = 0; i < bytes_in.Length; i++)
                    {
                        //Console.Clear();
                        //Console.WriteLine("max UInt32 " + UInt32.MaxValue.ToString());
                        //Console.WriteLine(result.ToString());
                        //Console.WriteLine(bytes_in[i].ToString());
                        //Console.WriteLine("Difference " + (UInt32.MaxValue - result).ToString());
                        //Console.ReadKey();
                        // 2023-07-21 14:05 note. when value is max there is no number
                        // to add to get 0. any addition will give number bigger than zero
                        if ((UInt32.MaxValue - result) >= bytes_in[i])
                        {
                            result += bytes_in[i];
                        }
                        else
                        {
                            UInt32 difference = bytes_in[i] - (UInt32.MaxValue - result);
                            result = difference;
                        }
                    }
                    return result;
                }
            }
        }
        private static void ServerSendBytes(byte[] arr_in, TcpClient client_tcp)
        {
            try
            {
                NetworkStream stream_for_client = client_tcp.GetStream();
                stream_for_client.Write(arr_in, 0, arr_in.Length);
                stream_for_client.Close();
            }
            catch
            {
                ReportFunctions.ReportError("Something wrong in " + nameof(ServerSendBytes));
            }
        }
        private static byte[] ServerRecieveBytes(TcpClient client_tcp)
        {
            byte[] for_return = new byte[0];
            try
            {
                NetworkStream stream_for_client = client_tcp.GetStream();
                for_return = new byte[stream_for_client.Length];
                stream_for_client.Read(for_return, 0, for_return.Length);
                stream_for_client.Close();
            }
            catch
            {
                ReportFunctions.ReportError("Something wrong in " + nameof(ServerSendBytes));
            }
            return for_return;
        }
        public static byte[] Byte_stuffing_insert(byte[] arr_in, byte byte_to_insert)
        {
            // counting
            Int32 bytes_found = 0;
            for (Int32 i = 0; i < arr_in.Length; i++)
            {
                if (arr_in[i] == byte_to_insert)
                {
                    bytes_found++;
                }
            }
            byte[] arr_out = new byte[arr_in.Length + bytes_found];
            // filling 
            Int32 arr_out_index = 0;
            for (Int32 i = 0; i < arr_in.Length; i++)
            {
                arr_out[arr_out_index] = arr_in[i];
                arr_out_index++;
                if (arr_in[i] == byte_to_insert)
                {
                    arr_out[arr_out_index] = arr_in[i];
                    arr_out_index++;
                }
            }
            return arr_out;
        }
        public static byte[] HeaderInsertInByteArray(byte[] arr_in, byte byte_to_insert)
        {
            byte[] arr_out = new byte[arr_in.Length * 2];
            // filling 
            Int32 arr_out_index = 0;
            for (Int32 i = 0; i < arr_in.Length; i++)
            {
                arr_out[arr_out_index] = byte_to_insert;
                arr_out_index++;
                arr_out[arr_out_index] = arr_in[i];
                arr_out_index++;
            }
            return arr_out;
        }
        public static byte ByteArrayCheckSum_type_add_bytes(byte[] arr_in)
        {
            byte num_out = 0;
            // filling 
            for (Int32 i = 0; i < arr_in.Length; i++)
            {
                num_out += arr_in[i];
            }
            return num_out;
        }
        public static byte[] ByteArrayCheckSum(byte[] arr_in, Int32 num_size = 2)
        {
            byte[] num_out = new byte[0];
            if (num_size <= 0)
            {
                ReportFunctions.ReportError(nameof(num_size) + " wrong.\r\n" + nameof(num_size) + " is " + num_size.ToString());
                return num_out;
            }
            Int32 div_res = 0;
            System.Math.DivRem(num_size, 2, out div_res);
            if (div_res != 0)
            {
                ReportFunctions.ReportError(nameof(num_size) + " wrong.\r\n" + nameof(num_size) + " is " + num_size.ToString());
                return num_out;
            }
            num_out = new byte[num_size];
            UInt32 sum_calc = 0;
            // filling 
            for (Int32 i = 0; i < arr_in.Length; i++)
            {
                sum_calc += arr_in[i];
            }
            for (Int32 i = 0; i < num_size; i++)
            {
                num_out[i] = (byte)(sum_calc >> 8 * i);
            }
            return num_out;
        }
        public static byte[] Byte_stuffing_remove(byte[] arr_in, byte byte_to_remove)
        {
            // counting
            Int32 bytes_found = 0;
            for (Int32 i = 0; i < arr_in.Length; i++)
            {
                if (i == arr_in.Length - 1)
                {
                    break;
                }
                if (arr_in[i] == byte_to_remove)
                {
                    bytes_found++;
                    i++;
                }
            }
            byte[] arr_out = new byte[arr_in.Length - bytes_found];
            // filling 
            Int32 arr_out_index = 0;
            for (Int32 i = 0; i < arr_in.Length; i++)
            {
                arr_out[arr_out_index] = arr_in[i];
                arr_out_index++;
                if (arr_in[i] == byte_to_remove)
                {
                    i++;
                }
            }
            return arr_out;
        }
        public static string[] StringSplitCmdData(string string_in, char delimer_in)
        {
            return string_in.Split(new char[] { delimer_in }, 2);
        }
        class NetworkDataClass
        {
            public NetworkDataClass()
            {
                CMD_Method_Bind.Clear();
                CMD_Method_Bind.Add(commands_list_enum.cmd1, commands_list_cmd1_method);
                CMD_Method_Bind.Add(commands_list_enum.cmd2, commands_list_cmd2_method);
            }
            private void commands_list_cmd2_method()
            {
                throw new NotImplementedException();
            }
            private void commands_list_cmd1_method()
            {
                throw new NotImplementedException();
            }
            // not finished.
            // 2023-05-05 09:55 ByteArrayToVariables()        
            Dictionary<string, UInt32> data_values = new Dictionary<string, UInt32>();
            Dictionary<string, Type> data_types = new Dictionary<string, Type>();
            byte[] network_bytes_received = new byte[0];
            public void ByteArrayToVariables()
            {
                MemoryStream memory_read = new MemoryStream(network_bytes_received);
                BinaryReader binary_read = new BinaryReader(memory_read);
                var_1 = binary_read.ReadUInt32();
                var_2 = binary_read.ReadUInt32();
                binary_read.Close();
                memory_read.Close();
            }
            void VariablesToDictionaries()
            {
                data_values = new Dictionary<string, UInt32>();
                data_types = new Dictionary<string, Type>();
                data_values.Add(nameof(var_1), var_1);
                data_types.Add(nameof(var_1), var_1.GetType());
                data_values.Add(nameof(var_2), var_2);
                data_types.Add(nameof(var_2), var_2.GetType());
            }
            // template
            public UInt32 var_1 = 0;
            public UInt32 var_2 = 0;
            void DictionariesToVariables()
            {
                var_1 = data_values[nameof(var_1)];
                var_2 = data_values[nameof(var_2)];
            }
            //[StructLayout(LayoutKind.Explicit)]
            //public struct Union
            //{
            //    [FieldOffset(0)] public Int32 num_1;
            //    [FieldOffset(0)] public Int32 num_2;
            //}
            public void ToConsole()
            {
                VariablesToDictionaries();
                for (Int32 i = 0; i < data_values.Count; i++)
                {
                    Console.WriteLine(data_values.ElementAt(i).Key + " " + data_values.ElementAt(i).Value);
                }
            }
            public enum commands_list_enum
            {
                DoNothing = 0,
                NotFound = 1,
                cmd1 = 10,
                cmd2 = 40
            }
            public commands_list_enum CMD_TO_ENUM(byte cmd_in)
            {
                commands_list_enum enum_out = commands_list_enum.DoNothing;
                byte[] enum_values = (byte[])Enum.GetValues(typeof(commands_list_enum));
                if (enum_values.Contains(cmd_in))
                {
                    enum_out = (commands_list_enum)cmd_in;
                }
                else
                {
                    enum_out = commands_list_enum.NotFound;
                }
                return enum_out;
            }
            Dictionary<commands_list_enum, Action> CMD_Method_Bind = new Dictionary<commands_list_enum, Action>();
            public void CMD_Execute(commands_list_enum cmd_in)
            {
                switch (cmd_in)
                {
                    case commands_list_enum.DoNothing: return;
                    case commands_list_enum.NotFound: return;
                    case commands_list_enum.cmd1:
                        CMD_Method_Bind[commands_list_enum.cmd1]();
                        return;
                    case commands_list_enum.cmd2:
                        CMD_Method_Bind[commands_list_enum.cmd2]();
                        return;
                    default: return;
                }
            }
        }
    }
}
