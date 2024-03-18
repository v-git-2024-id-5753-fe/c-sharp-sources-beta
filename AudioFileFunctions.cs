using BitsFunctionsNamespace;
using ReportFunctionsNamespace;
using System;
using System.IO;

// Commercial use (license)
// Please study about commercial use of the code that is publicly available 

namespace AudioFileFunctions
{
    // 2024.03.16 15:43. Saint-Petersburg. Home. 
    // this was not completed because there were no need to do it.
    [Obsolete]
    class Mp3Frame
    {
        public Mp3Frame() { }
        enum Mp3BitrateVL_Enum
        {
            V1_L1,
            V1_L2,
            V1_L3,
            V2_L1,
            V2_L2_AND_L3,
            NoVL
        }
        Mp3BitrateVL_Enum Mp3BitrateVL = Mp3BitrateVL_Enum.V1_L1;
        Mp3BitrateVL_Enum Mp3BitrateVLGet(Mp3_MPEG_Audio_version_ID_Enum version_in, Mp3_Layer_description_Enum layer_in)
        {
            Mp3BitrateVL_Enum vl_enum_out = Mp3BitrateVL_Enum.NoVL;
            if (version_in == Mp3_MPEG_Audio_version_ID_Enum.MPEG_Version_1)
            {
                if (layer_in == Mp3_Layer_description_Enum.Layer_I)
                {
                    vl_enum_out = Mp3BitrateVL_Enum.V1_L1;
                }
                if (layer_in == Mp3_Layer_description_Enum.Layer_II)
                {
                    vl_enum_out = Mp3BitrateVL_Enum.V1_L2;
                }
                if (layer_in == Mp3_Layer_description_Enum.Layer_III)
                {
                    vl_enum_out = Mp3BitrateVL_Enum.V1_L3;
                }
            }
            if (version_in == Mp3_MPEG_Audio_version_ID_Enum.MPEG_Version_2)
            {
                if (layer_in == Mp3_Layer_description_Enum.Layer_I)
                {
                    vl_enum_out = Mp3BitrateVL_Enum.V2_L1;
                }
                if (layer_in == Mp3_Layer_description_Enum.Layer_II)
                {
                    vl_enum_out = Mp3BitrateVL_Enum.V2_L2_AND_L3;
                }
                if (layer_in == Mp3_Layer_description_Enum.Layer_III)
                {
                    vl_enum_out = Mp3BitrateVL_Enum.V2_L2_AND_L3;
                }
            }
            if (vl_enum_out == Mp3BitrateVL_Enum.NoVL)
            {
                MyReportFunctions.ReportError("failed to cast to Enum\r\n" + "Version: " + version_in.ToString() +
                    "\r\n" + "Layer: " + layer_in.ToString());
            }
            return vl_enum_out;
        }
        enum Mp3_MPEG_Audio_version_ID_Enum
        {
            MPEG_Version_2_5,
            reserved,
            MPEG_Version_2,
            MPEG_Version_1
        }
        // (20,19) bits
        Mp3_MPEG_Audio_version_ID_Enum Mp3_MPEG_Audio_version_ID = Mp3_MPEG_Audio_version_ID_Enum.reserved;
        public Int32 Bitrate = 0;
        Int32 Bitrate_index = 0;
        int[][] Bitrate_index_table = new int[16][]
        {
            new int[]{0000, -1, -1, -1, -1, -1 },
            new int[]{0001, 32, 32, 32, 32, 8},
            new int[]{0010, 64, 48, 40, 48, 16},
            new int[]{0011, 96, 56, 48, 56, 24},
            new int[]{0100, 128, 64, 56, 64, 32},
            new int[]{0101, 160, 80, 64, 80, 40},
            new int[]{0110, 192, 96, 80, 96, 48},
            new int[]{0111, 224, 112, 96, 112, 56},
            new int[]{1000, 256, 128, 112, 128, 64},
            new int[]{1001, 288, 160, 128, 144, 80},
            new int[]{1010, 320, 192, 160, 160, 96},
            new int[]{1011, 352, 224, 192, 176, 112},
            new int[]{1100, 384, 256, 224, 192, 128},
            new int[]{1101, 416, 320, 256, 224, 144},
            new int[]{1110, 448, 384, 320, 256, 160},
            new int[]{1111, -2, -2, -2, -2, -2}
        };
        // bits (15,12)
        enum Mp3_Layer_description_Enum
        {
            reserved,
            Layer_III,
            Layer_II,
            Layer_I
        }
        // (18,17) bits
        Mp3_Layer_description_Enum Mp3_Layer_description = Mp3_Layer_description_Enum.reserved;
        public void LoadBytes(byte[] bytes_in)
        {
            Bytes = bytes_in;
            FrameHeader = AudioFileMp3HeaderGet(Bytes);
            Mp3_Layer_description = (Mp3_Layer_description_Enum)BitsFunctions.BitsGet(FrameHeader, 17, 18);
            Mp3_MPEG_Audio_version_ID = (Mp3_MPEG_Audio_version_ID_Enum)BitsFunctions.BitsGet(FrameHeader, 19, 20);
            Mp3BitrateVL = Mp3BitrateVLGet(Mp3_MPEG_Audio_version_ID, Mp3_Layer_description);
            Bitrate_index = BitsFunctions.BitsGet(FrameHeader, 12, 15);
            Bitrate = Bitrate_index_table[Bitrate_index][(int)Mp3BitrateVL];
            Sampling_rate_frequency_index = BitsFunctions.BitsGet(FrameHeader, 10, 11);
            if (Mp3_MPEG_Audio_version_ID == Mp3_MPEG_Audio_version_ID_Enum.MPEG_Version_2_5)
            {
                SamplingFrequency = Sampling_rate_frequency_index_table[Sampling_rate_frequency_index][2];
            }
            if (Mp3_MPEG_Audio_version_ID == Mp3_MPEG_Audio_version_ID_Enum.MPEG_Version_2)
            {
                SamplingFrequency = Sampling_rate_frequency_index_table[Sampling_rate_frequency_index][1];
            }
            if (Mp3_MPEG_Audio_version_ID == Mp3_MPEG_Audio_version_ID_Enum.MPEG_Version_1)
            {
                SamplingFrequency = Sampling_rate_frequency_index_table[Sampling_rate_frequency_index][0];
            }
            Padding = Convert.ToBoolean(BitsFunctions.BitsGet(FrameHeader, 9, 9));
        }
        public bool Padding = false;
        // Bit 9
        public Int32 SamplingFrequency = 0;
        public Int32 FrameLength
        {
            get
            {
                Int32 num_out = 0;
                if (Padding == false)
                {
                    num_out = 144 * Bitrate / SamplingFrequency;
                }
                else
                {
                    num_out = 144 * Bitrate / SamplingFrequency + 1;
                }
                return num_out;
            }
        }
        UInt32 FrameHeader = 0;
        Int32 Sampling_rate_frequency_index = 0;
        int[][] Sampling_rate_frequency_index_table = new int[4][]
        {
            new int[]{ 44100, 22050,   11025 },
            new int[]{ 48000, 24000,   12000 },
            new int[]{ 32000,    16000,   8000},
            new int[]{-1, -1, -1 }
        };
        //bits MPEG1   MPEG2 MPEG2.5
        //00	44100	22050	11025
        //01	48000	24000	12000
        //10	32000	16000	8000
        //11	reserv.reserv.reserv.
        // (11,10) bits
        public byte[] Bytes = new byte[0];
        public static UInt32 AudioFileMp3HeaderGet(byte[] frame_in)
        {
            Int32 num_out = 0;
            num_out |= frame_in[0] << 24;
            num_out |= frame_in[1] << 16;
            num_out |= frame_in[2] << 8;
            num_out |= frame_in[3] << 0;
            return (UInt16)num_out;
        }
    }

    // 2024.03.16 15:44. Saint-Petersburg. Home.  
    // this was not completed because there were no need to do it.
    [Obsolete]
    class MyAudioFileMethods_class
    {
        public static UInt32 AudioFileMp3HeaderGet(byte[] frame_in)
        {
            Int32 num_out = 0;
            num_out |= frame_in[0] << 24;
            num_out |= frame_in[1] << 16;
            num_out |= frame_in[2] << 8;
            num_out |= frame_in[3] << 0;
            return (UInt16)num_out;
        }
        public static byte[] AudioFileMp3GetBytes(string file_path)
        {
            byte[] byte_arr_out = new byte[0];
            try
            {
                BinaryReader binaryReader = new BinaryReader(File.Open(file_path, FileMode.Open));
                byte_arr_out = binaryReader.ReadBytes((int)binaryReader.BaseStream.Length);
                binaryReader.Close();
            }
            catch
            {
                MyReportFunctions.ReportError("Error reading audio file.\r\nFile: " + file_path);
            }
            return byte_arr_out;
        }
    }
}


