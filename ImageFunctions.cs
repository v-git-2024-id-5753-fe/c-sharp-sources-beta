using ArrayFunctionsNamespace;
using ColorFunctionsNamespace;
using FileFunctionsNamespace;
using MathFunctionsNamespace;
using ReportFunctionsNamespace;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static FileFunctionsNamespace.FileFunctions.TextFile;

// Commercial use (license)
// Please study about commercial use of the code that is publicly available 

namespace ImageFunctionsNameSpace
{
    public class ImageCompression
    {
        Bitmap _internal_Bitmap = null;
        public ImageCompression() { }
        public ImageCompression(string file_in)
        {
            _internal_Bitmap = new Bitmap(file_in);
            _internal_Pixels_Values = ImageFunctions.BitmapToInt32ArrayAxB(_internal_Bitmap);
            Size.BeforeCompression = _internal_Pixels_Values.Length * _internal_Pixels_Values[0].Length * 4;
            _size_width = _internal_Pixels_Values[0].Length;
            _size_height = _internal_Pixels_Values.Length;
        }
        public void PixelsToConsole()
        {
            ImageFunctions.ColorArrayAxBToConsole(_internal_Pixels_Values);
        }
        public class SizeOfImage
        {
            public SizeOfImage() { }
            public Int32 BeforeCompression = 0;
            public Int32 AfterCompression = 0;
        }
        public SizeOfImage Size = new SizeOfImage();
        int[][] _internal_Pixels_Values = null;
        // 2023.7.23 21:42 it should be stored as 16 bit
        Int32 _size_width = 0;
        Int32 _size_height = 0;
        // 2023.7.23 21:43 decompression relies on the information about width and heigth.
        // stage 1. pixel and line size in int
        /// <summary>
        /// .lcpic - line compress picture
        /// 2023-07-24 10:12
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="extension"></param>
        public void CompressLines_v1(string filename, string extension = ".lcpic")
        {
            int[] file_bytes = CompressLines_v1();
            FileFunctions.TextFile.Int32ToFile(file_bytes, filename + extension);
        }
        /// <summary>
        /// Not recomended.
        /// v2 is more efficient. it is left as part of development.
        /// Compresses lines and comparing to v2 write line data is the line is finished.
        /// </summary>
        /// <returns></returns>
        int[] CompressLines_v1()
        {
            int[] color_and_size = new int[(Size.BeforeCompression / 4) * 2 + 2];
            Int32 _color_size_index = 2;
            Int32 pixel_color = _internal_Pixels_Values[0][0];
            Int32 size_line = 0;
            color_and_size[0] = _size_height;
            color_and_size[1] = _size_width;
            for (Int32 i = 0; i < _internal_Pixels_Values.Length; i++)
            {
                for (Int32 j = 0; j < _internal_Pixels_Values[0].Length; j++)
                {
                    if (pixel_color == _internal_Pixels_Values[i][j])
                    {
                        size_line++;
                    }
                    else
                    {
                        color_and_size[_color_size_index] = pixel_color;
                        _color_size_index++;
                        color_and_size[_color_size_index] = size_line;
                        _color_size_index++;
                        // 2023.7.23 21:56 index will poInt32 to not used Int32 after for ends.
                        pixel_color = _internal_Pixels_Values[i][j];
                        size_line = 1;
                    }
                    if (j == (_internal_Pixels_Values[i].Length - 1))
                    {
                        // 2023.7.23 22:20 end of line
                        color_and_size[_color_size_index] = pixel_color;
                        _color_size_index++;
                        color_and_size[_color_size_index] = size_line;
                        _color_size_index++;
                        size_line = 0;
                        if (i != (_internal_Pixels_Values.Length - 1))
                        {
                            pixel_color = _internal_Pixels_Values[i + 1][0];
                        }
                    }
                    //   _internal_Pixels_Values[i][j] = -5;
                    //   Console.Clear();
                    //  MyImageMethods.ColorArrayAxBToConsole(_internal_Pixels_Values);
                    //  MyFileFunctions.Int32ArrayToConsole(color_and_size);
                    // Console.ReadKey();
                }
            }
            _color_size_index -= 1;
            Array.Resize(ref color_and_size, _color_size_index + 1);
            Size.AfterCompression = color_and_size.Length * 4;
            return color_and_size;
        }
        /// <summary>
        /// Compresses lines of image
        /// </summary>
        /// <returns></returns>
        public int[] CompressLinesMethod_v2()
        {
            int[] color_and_size = new int[(Size.BeforeCompression / 4) * 2];
            Int32 _color_size_index = 0;
            Int32 pixel_color = _internal_Pixels_Values[0][0];
            Int32 size_line = 0;
            for (Int32 i = 0; i < _internal_Pixels_Values.Length; i++)
            {
                for (Int32 j = 0; j < _internal_Pixels_Values[0].Length; j++)
                {
                    if (pixel_color == _internal_Pixels_Values[i][j])
                    {
                        size_line++;
                    }
                    else
                    {
                        color_and_size[_color_size_index] = pixel_color;
                        _color_size_index++;
                        color_and_size[_color_size_index] = size_line;
                        _color_size_index++;
                        // 2023.7.23 21:56 index will poInt32 to not used Int32 after for ends.
                        pixel_color = _internal_Pixels_Values[i][j];
                        size_line = 1;
                    }
                }
            }
            // 2023.7.23 22:00 end the end there is dot or line in analysis
            color_and_size[_color_size_index] = pixel_color;
            _color_size_index++;
            color_and_size[_color_size_index] = size_line;
            Array.Resize(ref color_and_size, _color_size_index + 1);
            Size.AfterCompression = color_and_size.Length * 4;
            return color_and_size;
        }
        /// <summary>
        /// Compresses lines. Uses v2 and then Int32 for size is replaced by Int16.
        /// Pixel color is stored in two Int16.
        /// </summary>
        /// <returns></returns>
        public Int16[] CompressLinesMethod_v3()
        {
            int[] compressed_pixels = CompressLinesMethod_v2();
            Int32 size_in_int16 = 0;
            // 2023.7.23 22:25 step by step
            // 2023.7.23 22:25 1. Int32 pixels
            size_in_int16 = compressed_pixels.Length / 2;
            // 2023.7.23 22:26 2. 2 Int16 for pixel
            size_in_int16 = size_in_int16 * 2;
            // 2023.7.23 22:26 3. size of line
            size_in_int16 += compressed_pixels.Length / 2;
            Int16[] compressed_pixels_int16 = new Int16[size_in_int16];
            Int32 _in_int16_index = 0;
            for (Int32 i = 0; i < compressed_pixels.Length; i += 2)
            {
                compressed_pixels_int16[_in_int16_index] = (Int16)(compressed_pixels[i] >> 16);
                _in_int16_index++;
                compressed_pixels_int16[_in_int16_index] = (Int16)(compressed_pixels[i] >> 0);
                _in_int16_index++;
                compressed_pixels_int16[_in_int16_index] = (Int16)compressed_pixels[i + 1];
                _in_int16_index++;
                // 2023.7.23 22:31 index will poInt32 to not used Int16
                // 2023.7.23 22:32 the size will be defined so there is no need to do anything
            }
            Size.AfterCompression = compressed_pixels_int16.Length * 2;
            return compressed_pixels_int16;
        }
        // 2023.7.23 23:09 to do.
        // file format .lcpic line coding picture.
    }
    public class ImageDecompression
    {
        public Bitmap BitmapDecompressed = null;
        string extension_compress_lines = ".lcpic";
        public ImageDecompression() { }
        public ImageDecompression(string file_in)
        {
            string extension = Path.GetExtension(file_in);
            if (extension != extension_compress_lines)
            {
                ReportFunctions.ReportError("Wrong file. File is " + extension + ". File should be " + extension_compress_lines);
                return;
            }
            _internal_File_Bytes = FileFunctions.TextFile.FileToBytes(file_in);
            Size.BeforeDecompression = _internal_File_Bytes.Length;
            byte[] height_bytes = ArrayFunctions.Extract.AboveIndex(_internal_File_Bytes, 4);
            height_bytes = ArrayFunctions.Extract.BelowIndex(height_bytes, 1);
            byte[] width_bytes = ArrayFunctions.Extract.AboveIndex(_internal_File_Bytes, 8);
            width_bytes = ArrayFunctions.Extract.BelowIndex(_internal_File_Bytes, 5);
            _size_height = MathFunctions.BytesToInt16(height_bytes);
            _size_width = MathFunctions.BytesToInt16(width_bytes);
        }
        byte[] _internal_File_Bytes = null;
        public class SizeOfImage
        {
            public SizeOfImage() { }
            public Int32 BeforeDecompression = 0;
            public Int32 AfterDecompression = 0;
        }
        public SizeOfImage Size = new SizeOfImage();
        int[][] _internal_Pixels_Values = null;
        Int32 _size_width = 0;
        Int32 _size_height = 0;
        /// <summary>
        /// Decompresses file ".lcpic" to Bitmap. Bitmap can be used after this fucntion.
        /// 2023-07-24 11:34
        /// </summary>
        public void LinesMethod_Version_1_Decompress_Bitmap()
        {
            BitmapDecompressed = ImageFunctions.Int32ArrayAxBToBitmap(LinesMethod_Version_1_Decompress_Pixels());
        }
        public void BitmapDecompressedToFileBMP(string filename)
        {
            ImageFunctions.BitmapToFileBMP(BitmapDecompressed, filename);
        }
        int[][] LinesMethod_Version_1_Decompress_Pixels()
        {
            int[][] Pixels = new int[_size_height][];
            Int32 PixelColor = 0;
            Int32 LineSize = 0;
            Int32 Pixel_height_index = 0;
            Int32 Pixel_width_index = 0;
            Pixels[0] = new int[_size_width];
            for (Int32 i = 8; i < _internal_File_Bytes.Length; i += 8)
            {
                if (i > _internal_File_Bytes.Length - 16)
                {
                    // it was for debug. 2024.02.10 18:52. Moscow. Hostel.
                    // Int32 b = 0;
                }
                byte[] pixel_color_bytes = ArrayFunctions.Extract.FromIndexToIndex(_internal_File_Bytes, i, i + 3);
                byte[] line_size_bytes = ArrayFunctions.Extract.FromIndexToIndex(_internal_File_Bytes, i + 6, i + 7);
                PixelColor = MathFunctions.Int32Number.BytesToInt32(pixel_color_bytes);
                LineSize = MathFunctions.BytesToInt16(line_size_bytes);
                for (Int32 j = 0; j < LineSize; j++)
                {
                    Pixels[Pixel_height_index][Pixel_width_index] = PixelColor;
                    Pixel_width_index++;
                }
                // 2023-07-24 09:08 after for index points at empty int. it can be out of range
                Pixel_width_index -= 1;
                if (Pixel_width_index >= (_size_width - 1))
                {
                    Pixel_width_index = 0;
                    Pixel_height_index++;
                    if (Pixel_height_index < _size_height)
                    {
                        Pixels[Pixel_height_index] = new int[_size_width];
                    }
                }
                else
                {
                    Pixel_width_index++;
                }
            }
            Size.AfterDecompression = _size_height * _size_width * 4;
            return Pixels;
        }
        public int[] CompressLinesMethod_v2()
        {
            int[] color_and_size = new int[(Size.BeforeDecompression / 4) * 2];
            Int32 _color_size_index = 0;
            Int32 pixel_color = _internal_Pixels_Values[0][0];
            Int32 size_line = 0;
            for (Int32 i = 0; i < _internal_Pixels_Values.Length; i++)
            {
                for (Int32 j = 0; j < _internal_Pixels_Values[0].Length; j++)
                {
                    if (pixel_color == _internal_Pixels_Values[i][j])
                    {
                        size_line++;
                    }
                    else
                    {
                        color_and_size[_color_size_index] = pixel_color;
                        _color_size_index++;
                        color_and_size[_color_size_index] = size_line;
                        _color_size_index++;
                        // 2023.7.23 21:56 index will poInt32 to not used Int32 after for ends.
                        pixel_color = _internal_Pixels_Values[i][j];
                        size_line = 1;
                    }
                }
            }
            // 2023.7.23 22:00 end the end there is dot or line in analysis
            color_and_size[_color_size_index] = pixel_color;
            _color_size_index++;
            color_and_size[_color_size_index] = size_line;
            Array.Resize(ref color_and_size, _color_size_index + 1);
            Size.AfterDecompression = color_and_size.Length * 4;
            return color_and_size;
        }
        public Int16[] CompressLinesMethod_v3()
        {
            int[] compressed_pixels = CompressLinesMethod_v2();
            Int32 size_in_int16 = 0;
            // 2023.7.23 22:25 step by step
            // 2023.7.23 22:25 1. Int32 pixels
            size_in_int16 = compressed_pixels.Length / 2;
            // 2023.7.23 22:26 2. 2 Int16 for pixel
            size_in_int16 = size_in_int16 * 2;
            // 2023.7.23 22:26 3. size of line
            size_in_int16 += compressed_pixels.Length / 2;
            Int16[] compressed_pixels_int16 = new Int16[size_in_int16];
            Int32 _in_int16_index = 0;
            for (Int32 i = 0; i < compressed_pixels.Length; i += 2)
            {
                compressed_pixels_int16[_in_int16_index] = (Int16)(compressed_pixels[i] >> 16);
                _in_int16_index++;
                compressed_pixels_int16[_in_int16_index] = (Int16)(compressed_pixels[i] >> 0);
                _in_int16_index++;
                compressed_pixels_int16[_in_int16_index] = (Int16)compressed_pixels[i + 1];
                _in_int16_index++;
                // 2023.7.23 22:31 index will poInt32 to not used Int16
                // 2023.7.23 22:32 the size will be defined so there is no need to do anything
            }
            Size.AfterDecompression = compressed_pixels_int16.Length * 2;
            return compressed_pixels_int16;
        }
    }
    public static class ImageFunctions
    {
        static Random _internal_random = new Random();
        // template for code execution. 2024.03.06 16:00. Moscow. Workplace. 
        /*
        double execution_time_ms_start = 0;
        if (TimeExecutionShow == true)
            {
                execution_time_ms_start = _time_execution.Elapsed.TotalMilliseconds;
            }
        if (TimeExecutionShow == true)
            {
                double execution_time_ms_stop = _time_execution.Elapsed.TotalMilliseconds;
                TimeExecutionMessage(nameof(function_name_here), execution_time_ms_stop - execution_time_ms_start);
            }
        */
        /// <summary>
        /// 1. Added. start. 2024.03.05 10:59. Moscow. Workplace. <br></br>
        /// 2. Modifed. 2024.03.06 15:58. Moscow. Workplace. 
        /// 3. Tested after 2. Works. 2024.03.06 16:36. Moscow. Workplace. 
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
        // added. end.
        /// <summary>
        /// Written. 2024.03.05 11:14. Moscow. Workplace. <br></br>
        /// Tested. Works. 2024.03.05 11:14. Moscow. Workplace. <br></br>
        /// 
        /// Note. Low time function can be excluded from messages in console. <br></br>
        /// </summary>
        /// <param name="function_name"></param>
        static void TimeExecutionMessage(string function_name, double total_ms_passed)
        {
            _time_execution_count += 1;
            Console.WriteLine(_time_execution_count.ToString() + ". " + DateTime.Now.ToString("HH:mm:ss") + " " + function_name +
                " exectuion time: " + total_ms_passed.ToString("0.000") + " ms");
        }
        /// <summary>
        /// Written. 2024.03.06 17:11. Moscow. Workplace. 
        /// </summary>
        public static class AverageColor
        {
            /// <summary>
            /// Written. 2024.03.06 17:12. Moscow. Workplace. <br></br>
            /// Tested. Works. 2024.03.07 12:45. Moscow. Workplace. <br></br>
            /// <br></br>
            /// 2024.03.07 12:48. Moscow. Workplace. <br></br> 
            /// Note. black and white level is obtained from black and white Bitmap <br></br>
            /// that is made in this function during the work.<br></br>
            /// <br></br>
            /// 2024.03.07 12:48. Moscow. Workplace. <br></br>
            /// Execution time can be found in Convert.ToBlackWhiteBitmap <br></br>
            /// which is 15-20 ms for 100x100 and 2-3 s for 1600x900
            /// </summary>
            /// <param name="bitmap_in"></param>
            /// <returns></returns>
            public static Int32 BlackWhite(Bitmap bitmap_in)
            {
                // 2024.03.06 17:32. Moscow. Workplace. 
                // during writing the code.
                // Averaging from certain color moves the color to white but via non-gray color. 
                double execution_time_ms_start = 0;
                if (TimeExecutionShow == true)
                {
                    execution_time_ms_start = _time_execution.Elapsed.TotalMilliseconds;
                }
                // 2024.03.07 12:31. Moscow. Workplace. 
                // red green blue are the same for a pixel for black and white image
                // averaging red will give the same as averaging blue, green
                Bitmap bitmap_white_black = Convert.ToBlackWhiteBitmap(bitmap_in);
                Color[] color_arr = Convert.BitmapToColorArray(bitmap_white_black);
                byte[] red_bytes = ColorFunctions.Extract.FromArray.Red(color_arr);
                byte[] green_bytes = ColorFunctions.Extract.FromArray.Green(color_arr);
                byte[] blue_bytes = ColorFunctions.Extract.FromArray.Blue(color_arr);
                byte red_average = ArrayFunctions.ByteArray.Average(red_bytes);
                byte green_average = ArrayFunctions.ByteArray.Average(green_bytes);
                byte blue_average = ArrayFunctions.ByteArray.Average(blue_bytes);
                // 2024.03.07 08:04. Moscow. Workplace. 
                // note. negative int32 can not be casted to uint.
                UInt32 uint32_for_out = (uint)0xFF << 24;
                uint32_for_out |= ((uint)red_average << 16);
                uint32_for_out |= ((uint)green_average << 8);
                uint32_for_out |= ((uint)blue_average << 0);
                if (TimeExecutionShow == true)
                {
                    double execution_time_ms_stop = _time_execution.Elapsed.TotalMilliseconds;
                    TimeExecutionMessage(nameof(AverageColor.BlackWhite), execution_time_ms_stop - execution_time_ms_start);
                }
                return (int)uint32_for_out;
            }
        }
        /// <summary>
        /// Written. 2024.02.09 10:01. Moscow. Workplace.
        /// </summary>
        public static class Generate
        {
            /// <summary>
            /// Written. 2024.02.25 19:10. Moscow. Hostel.
            /// </summary>
            public static class PointsOutput
            {
                /// <summary>
                /// Generates random points and return Point[] of defined size. <br></br>
                /// Written. 2024.02.25 19:13. Moscow. Hostel. <br></br>
                /// Tested. Works. 2024.02.25 21:38. Moscow. Hostel.
                /// </summary>
                /// <param name="points_count"></param>
                /// <param name="x_start"></param>
                /// <param name="x_end"></param>
                /// <param name="y_start"></param>
                /// <param name="y_end"></param>
                /// <returns></returns>
                public static Point[] Random(Int32 points_count = 10, UInt32 x_start = 0, UInt32 x_end = 200, UInt32 y_start = 0, UInt32 y_end = 200)
                {
                    Point[] points_out = new Point[points_count];
                    for (Int32 i = 0; i < points_out.Length; i++)
                    {
                        points_out[i] = new Point(_internal_random.Next((int)x_start, (int)x_end + 1), _internal_random.Next((int)y_start, (int)y_end + 1));
                    }
                    return points_out;
                }
            }
            // 2024.03.06 17:22. Moscow. Workplace. 
            // If there is different than Bitmap output then it is placed is seperate class
            //public static class BitmapOutput
            //{
            /// <summary>
            /// Generates Bitmap with random dots of certain size of certain color.
            /// Written. 2024.02.25 21:45. Moscow. Hostel. <br></br>
            /// Tested. Works. 2024.02.25 21:51. Moscow. Hostel.
            /// </summary>
            /// <param name="points_arr_in"></param>
            /// <param name="point_width"></param>
            /// <param name="point_color"></param>
            /// <param name="form_call_from"></param>
            /// <returns></returns>
            public static Bitmap Random_Dots_Certain_Color(Color point_color, Int32 points_count = 10, UInt32 point_width = 1, UInt32 x_start = 0, UInt32 x_end = 200, UInt32 y_start = 0, UInt32 y_end = 200)
            {
                double execution_time_ms_start = 0;
                if (TimeExecutionShow == true)
                {
                    execution_time_ms_start = _time_execution.Elapsed.TotalMilliseconds;
                }
                if (point_width == 0)
                {
                    ReportFunctions.ReportError("PoInt32 radius error");
                    Bitmap bitmap_error = Generate.Rectungular_Checkboard(200, 200);
                    return bitmap_error;
                }
                Point[] points_arr = Generate.PointsOutput.Random(points_count, x_start, x_end, y_start, y_end);
                Int32 max_x = ExtractValues.FromPoints.XValues(points_arr).Max();
                Int32 max_y = ExtractValues.FromPoints.YValues(points_arr).Max();
                Bitmap bitmap_out = new Bitmap(max_x, max_y);
                Graphics draw_bitmap = Graphics.FromImage(bitmap_out);
                Pen pen_draw = new Pen(point_color, point_width);
                for (Int32 i = 0; i < points_arr.Length; i++)
                {
                    draw_bitmap.DrawArc(pen_draw, new Rectangle(points_arr[i], new Size((int)point_width, (int)point_width)), 0, 360);
                }
                if (TimeExecutionShow == true)
                {
                    double execution_time_ms_stop = _time_execution.Elapsed.TotalMilliseconds;
                    TimeExecutionMessage(nameof(Random_Dots_Certain_Color), execution_time_ms_stop - execution_time_ms_start);
                }
                return bitmap_out;
            }
            /// <summary>
            /// Creates Bitmap with the color of each pixel white-gray-black. <br></br>
            /// Written. 2024.02.09 13:11. Moscow. Workplace. <br></br>
            /// Tested. Works. 2024.02.09 13:14. Moscow. Workplace. 
            /// </summary>
            /// <param name="side_a"></param>
            /// <param name="side_b"></param>
            /// <returns></returns>
            public static Bitmap Rectungular_Random_0_255(Int32 side_a, Int32 side_b)
            {
                Bitmap bitmap_out = new Bitmap(side_a, side_b);
                for (Int32 j = 0; j < side_b; j++)
                {
                    for (Int32 i = 0; i < side_a; i++)
                    {
                        byte byte_for_color = (byte)_internal_random.Next(0, byte.MaxValue + 1);
                        bitmap_out.SetPixel(i, j, Color.FromArgb(255, byte_for_color, byte_for_color, byte_for_color));
                    }
                }
                return bitmap_out;
            }
            /// <summary>
            /// Written. 2024.03.06 17:18. Moscow. Workplace. 
            /// </summary>
            /// <param name="color_in"></param>
            /// <param name="side_length"></param>
            /// <returns></returns>
            public static Bitmap Square_Certain_Color(Color color_in, Int32 side_length = 100)
            {
                double execution_time_ms_start = 0;
                if (TimeExecutionShow == true)
                {
                    execution_time_ms_start = _time_execution.Elapsed.TotalMilliseconds;
                }
                Bitmap bitmap_out = new Bitmap(side_length, side_length);
                // 2024.03.06 17:18. Moscow. Workplace. 
                // There is fill tool in Graphics.
                for (Int32 j = 0; j < side_length; j++)
                {
                    for (Int32 i = 0; i < side_length; i++)
                    {
                        bitmap_out.SetPixel(i, j, color_in);
                    }
                }
                if (TimeExecutionShow == true)
                {
                    double execution_time_ms_stop = _time_execution.Elapsed.TotalMilliseconds;
                    TimeExecutionMessage(nameof(Generate.Square_Certain_Color), execution_time_ms_stop - execution_time_ms_start);
                }
                return bitmap_out;
            }
            /// <summary>
            /// Creates Bitmap with random color of each pixel. <br></br>
            /// Written. 2024.02.09 13:02. Moscow. Workplace. <br></br>
            /// Tested. Works. 2024.02.09 13:08. Moscow. Workplace. 
            /// </summary>
            /// <param name="side_a"></param>
            /// <param name="side_b"></param>
            /// <returns></returns>
            public static Bitmap Rectungular_Random_Color(Int32 side_a, Int32 side_b)
            {
                Bitmap bitmap_out = new Bitmap(side_a, side_b);
                for (Int32 j = 0; j < side_b; j++)
                {
                    for (Int32 i = 0; i < side_a; i++)
                    {
                        bitmap_out.SetPixel(i, j, Color.FromArgb(255, _internal_random.Next(0, byte.MaxValue + 1),
                            _internal_random.Next(0, byte.MaxValue + 1), _internal_random.Next(0, byte.MaxValue + 1)));
                    }
                }
                return bitmap_out;
            }
            /// <summary>
            /// Creates Bitmap with black and white dots of checkboard pattern.
            /// Written. 2024.02.09 10:01. Moscow. Workplace. <br></br>
            /// Tested. Works. 2024.02.09 10:29. Moscow. Workplace. 
            /// </summary>
            /// <param name="side_a"></param>
            /// <param name="side_b"></param>
            /// <returns></returns>
            public static Bitmap Rectungular_Checkboard(Int32 side_a, Int32 side_b)
            {
                Bitmap bitmap_out = new Bitmap(side_a, side_b);
                bool color_white = false;
                for (Int32 j = 0; j < side_b; j++)
                {
                    if (j > 0)
                    {
                        if (color_white == false)
                        {
                            if (bitmap_out.GetPixel(0, j - 1).G == 0)
                            {
                                color_white = true;
                            }
                        }
                        else
                        {
                            if (bitmap_out.GetPixel(0, j - 1).G == 255)
                            {
                                color_white = false;
                            }
                        }
                    }
                    for (Int32 i = 0; i < side_a; i++)
                    {
                        if (color_white == true)
                        {
                            bitmap_out.SetPixel(i, j, Color.FromArgb(255, 255, 255, 255));
                            color_white = false;
                        }
                        else
                        {
                            bitmap_out.SetPixel(i, j, Color.FromArgb(255, 0, 0, 0));
                            color_white = true;
                        }
                    }
                }
                return bitmap_out;
            }
            /// <summary>
            /// Creates Bitmap with line - colors RGB (all equal) from 0 to 255<br></br>
            /// Written. 2024.02.09 10:38. Moscow. Workplace.  <br></br>
            /// Tested. Works. 2024.02.09 10:38. Moscow. Workplace. 
            /// </summary>
            /// <returns></returns>
            public static Bitmap Line_0_255()
            {
                Bitmap bitmap_out = new Bitmap(1, 256);
                for (Int32 i = 0; i < 256; i++)
                {
                    bitmap_out.SetPixel(0, i, Color.FromArgb(i, i, i));
                }
                return bitmap_out;
            }
            /// <summary>
            /// Written. 2024.02.10 12:58. Moscow. Hostel 
            /// </summary>
            /// <param name="circle_radius"></param>
            /// <param name="line_width"></param>
            /// <param name="outside_space"></param>
            /// <returns></returns>
            public static Bitmap CircleWithOutsideSpace(UInt32 circle_radius, UInt32 line_width, UInt32 outside_space, Color background_color)
            {
                Bitmap bitmap_out = new Bitmap((int)circle_radius * 2 + (int)outside_space * 2, (int)circle_radius * 2 + (int)outside_space * 2);
                using (Graphics graphics_draw = Graphics.FromImage(bitmap_out))
                {
                    Brush brush_draw = new SolidBrush(background_color);
                    graphics_draw.FillRectangle(brush_draw, 0, 0, ((int)circle_radius + (int)outside_space) * 2, ((int)circle_radius + (int)outside_space) * 2);
                    Pen pen_draw = new Pen(Color.Black, line_width);
                    graphics_draw.DrawArc(pen_draw, (int)outside_space, (int)outside_space,
                        (int)circle_radius * 2, (int)circle_radius * 2, 0, 360);
                }
                return bitmap_out;
            }
        }
        /// <summary>
        /// Written. 2024.03.06 16:03. Moscow. Workplace. 
        /// </summary>
        public static class ToFiles
        {
            /// <summary>
            /// Written. 2024.03.06 16:03. Moscow. Workplace. 
            /// Tested. Works. 2024.03.06 16:34. Moscow. Workplace. 
            /// </summary>
            /// <param name="bitmap_arr_in"></param>
            /// <param name="filename_base"></param>
            /// <param name="start_number"></param>
            public static void BitmapArrayToFilesBMP(Bitmap[] bitmap_arr_in, string filename_base, Int32 start_number = 0)
            {
                double execution_time_ms_start = 0;
                if (TimeExecutionShow == true)
                {
                    execution_time_ms_start = _time_execution.Elapsed.TotalMilliseconds;
                }
                for (Int32 i = 0; i < bitmap_arr_in.Length; i++)
                {
                    ToFile.ToBMP(bitmap_arr_in[i], filename_base + (start_number + i).ToString() + ".bmp");
                }
                if (TimeExecutionShow == true)
                {
                    double execution_time_ms_stop = _time_execution.Elapsed.TotalMilliseconds;
                    TimeExecutionMessage(nameof(ToFiles.BitmapArrayToFilesBMP), execution_time_ms_stop - execution_time_ms_start);
                }
            }
        }
        /// <summary>
        /// Written. 2024.03.06 15:34. Moscow. Workplace. 
        /// </summary>
        public static class ToFile
        {
            /// <summary>
            /// Written. 2024.03.06 15:34. Moscow. Workplace. 
            /// Tested. Works. 2024.03.06 16:34. Moscow. Workplace. 
            /// </summary>
            /// <param name="bitmap_in"></param>
            /// <param name="filename"></param>
            public static void ToBMP(Bitmap bitmap_in, string filename)
            {
                double execution_time_ms_start = 0;
                if (TimeExecutionShow == true)
                {
                    execution_time_ms_start = _time_execution.Elapsed.TotalMilliseconds;
                }
                FileFunctions.ImageFile.WriteFile.BitmapToFileBMP(bitmap_in, filename);
                if (TimeExecutionShow == true)
                {
                    double execution_time_ms_stop = _time_execution.Elapsed.TotalMilliseconds;
                    TimeExecutionMessage(nameof(ToFile.ToBMP), execution_time_ms_stop - execution_time_ms_start);
                }
            }
        }
        /// <summary>
        /// Written. 2024.02.25 22:05. Moscow. Hostel.
        /// </summary>
        public static class ToBitmap
        {
            /// <summary>
            /// Written. 2024.02.25 22:06. Moscow. Hostel. <br></br>
            /// Tested. Works. 2024.02.25 22:08. Moscow. Hostel.
            /// </summary>
            /// <param name="points_arr_in"></param>
            /// <param name="point_width"></param>
            /// <param name="point_color"></param>
            /// <returns></returns>
            public static Bitmap LineArray(Point[] points_arr_in, UInt32 point_width, Color point_color)
            {
                if (point_width == 0)
                {
                    ReportFunctions.ReportError("PoInt32 radius error");
                    Bitmap bitmap_error = Generate.Rectungular_Checkboard(200, 200);
                    return bitmap_error;
                }
                Int32 max_x = ExtractValues.FromPoints.XValues(points_arr_in).Max();
                Int32 max_y = ExtractValues.FromPoints.YValues(points_arr_in).Max();
                Bitmap bitmap_out = new Bitmap(max_x, max_y);
                Graphics draw_bitmap = Graphics.FromImage(bitmap_out);
                Pen pen_draw = new Pen(point_color, point_width);
                for (Int32 i = 1; i < points_arr_in.Length; i++)
                {
                    draw_bitmap.DrawLine(pen_draw, points_arr_in[i - 1], points_arr_in[i]);
                }
                return bitmap_out;
            }
        }
        /// <summary>
        /// Written. 2024.02.09 09:26. Moscow. Workplace. 
        /// </summary>
        public static class ToPictureBox
        {
            static Form FormOutput = null;
            static PictureBox PictureBoxOutput = null;
            static Int32 XYOfset = 10;
            /// <summary>
            /// Written. 2024.02.25 21:33. Moscow. Hostel. <br></br>
            /// Tested. Works. 2024.02.25 21:54. Moscow. Hostel.
            /// </summary>
            /// <param name="points_arr_in"></param>
            /// <param name="point_width"></param>
            /// <param name="point_color"></param>
            /// <param name="form_call_from"></param>
            public static void FromPointArray(Point[] points_arr_in, UInt32 point_width, Color point_color, Form form_call_from)
            {
                if (point_width == 0)
                {
                    ReportFunctions.ReportError("PoInt32 radius error");
                    Bitmap bitmap_error = Generate.Rectungular_Checkboard(200, 200);
                    FromBitmap(bitmap_error, form_call_from);
                    return;
                }
                Int32 max_x = ExtractValues.FromPoints.XValues(points_arr_in).Max();
                Int32 max_y = ExtractValues.FromPoints.YValues(points_arr_in).Max();
                Bitmap bitmap_out = new Bitmap(max_x, max_y);
                Graphics draw_bitmap = Graphics.FromImage(bitmap_out);
                Pen pen_draw = new Pen(point_color, point_width);
                for (Int32 i = 0; i < points_arr_in.Length; i++)
                {
                    draw_bitmap.DrawArc(pen_draw, new Rectangle(points_arr_in[i], new Size((int)point_width, (int)point_width)), 0, 360);
                }
                FromBitmap(bitmap_out, form_call_from);
            }
            /// <summary>
            /// Shows Forms with Picture box with provided Bitmap. <br></br>
            /// Written. 2024.02.09 10:06. Moscow. Workplace. <br></br>
            /// Tested. Works. 2024.02.09 10:35. Moscow. Workplace. 
            /// </summary>
            /// <param name="bitmap_in"></param>
            /// <param name="form_call_from"></param>
            public static void FromBitmap(Bitmap bitmap_in, Form form_call_from)
            {
                double execution_time_ms_start = 0;
                if (TimeExecutionShow == true)
                {
                    execution_time_ms_start = _time_execution.Elapsed.TotalMilliseconds;
                }
                Int32 row_length = bitmap_in.Width;
                Int32 rows_count = bitmap_in.Height;
                Form FormOutput = new Form();
                FormOutput.AutoSize = false;
                FormOutput.ClientSize = new Size(row_length + XYOfset * 2, rows_count + XYOfset * 2);
                PictureBoxOutput = new PictureBox();
                PictureBoxOutput.Location =
                    new Point(FormOutput.ClientRectangle.Location.X + XYOfset, FormOutput.ClientRectangle.Location.Y + XYOfset);
                PictureBoxOutput.ClientSize = new Size(row_length, rows_count);
                // copy of bitmap may be required. 2024.02.09 10:07. Moscow. Workplace. 
                PictureBoxOutput.Image = bitmap_in;
                FormOutput.Controls.Add(PictureBoxOutput);
                FormOutput.Show();
                if (TimeExecutionShow == true)
                {
                    double execution_time_ms_stop = _time_execution.Elapsed.TotalMilliseconds;
                    TimeExecutionMessage(nameof(FromBitmap), execution_time_ms_stop - execution_time_ms_start);
                }
            }
            /// <summary>
            /// Written. 2024.02.09 09:26. Moscow. Workplace. 
            /// </summary>
            /// <param name="color_arr_in"></param>
            /// <param name="row_length"></param>
            /// <param name="form_call_from"></param>
            public static void FromColorArray(Color[] color_arr_in, Int32 row_length, Form form_call_from)
            {
                Form FormOutput = new Form();
                FormOutput.AutoSize = false;
                FormOutput.ClientSize = new Size(row_length + XYOfset * 2, (color_arr_in.Length / row_length) + XYOfset * 2);
                PictureBoxOutput = new PictureBox();
                PictureBoxOutput.Location =
                    new Point(FormOutput.ClientRectangle.Location.X + XYOfset, FormOutput.ClientRectangle.Location.Y + XYOfset);
                PictureBoxOutput.ClientSize = new Size(1, color_arr_in.Length);
                Bitmap bitmap_out = new Bitmap(row_length, (color_arr_in.Length / row_length));
                Int32 rows_count = (color_arr_in.Length / row_length);
                for (Int32 j = 0; j < rows_count; j++)
                {
                    for (Int32 i = 0; i < row_length; i++)
                    {
                        bitmap_out.SetPixel(i, j, color_arr_in[j * row_length + i]);
                    }
                }
                PictureBoxOutput.Image = bitmap_out;
                FormOutput.Controls.Add(PictureBoxOutput);
                FormOutput.Show();
            }
        }
        /// <summary>
        /// Written. 2024.02.10 14:33. Moscow. Hostel.
        /// </summary>
        public static class Find
        {
            /// <summary>
            /// Written. 2024.02.11 09:44. Moscow. Hostel.
            /// </summary>
            public static class Location
            {
                /// <summary>
                /// Return Rectangle[] of object that are in the Bitmap. <br></br>
                /// Written. 2024.02.11 09:44. Moscow. Hostel. <br></br>
                /// Tested. Works. 2024.02.11 13:45. Moscow. Hostel. <br></br>
                /// </summary>
                public static class OfMultipleObjects
                {
                    public static Rectangle[] FromLeftToRight(Bitmap bitmap_in, Color color_background)
                    {
                        List<Rectangle> rect_found = new List<Rectangle>();
                        int[][] bitmap_int32 = Convert.BitmapToInt32ArrayAxB(bitmap_in);
                        Int32 rect_x_start = 0;
                        bool object_found = false;
                        for (Int32 i = 0; i < bitmap_in.Width; i++)
                        {
                            int[] column = ArrayFunctions.Column.Take(bitmap_int32, i);
                            if (ArrayFunctions.Int32Array.Math.ElementsTheSame(column) == true)
                            {
                                Int32 bitmap_pixel = bitmap_in.GetPixel(i, 0).ToArgb();
                                Int32 background_pixel = color_background.ToArgb();
                                if (bitmap_pixel == background_pixel)
                                {
                                    if (object_found == false)
                                    {
                                        rect_x_start = i;
                                        continue;
                                    }
                                }
                            }
                            if (object_found == false)
                            {
                                if (ArrayFunctions.Int32Array.Math.ElementsTheSame(column) == true)
                                {
                                    Int32 bitmap_pixel = bitmap_in.GetPixel(i, 0).ToArgb();
                                    Int32 background_pixel = color_background.ToArgb();
                                    if (bitmap_pixel != background_pixel)
                                    {
                                        rect_x_start = i;
                                        object_found = true;
                                        continue;
                                    }
                                }
                            }
                            if (object_found == true)
                            {
                                if (ArrayFunctions.Int32Array.Math.ElementsTheSame(column) == true)
                                {
                                    Int32 bitmap_pixel = bitmap_in.GetPixel(i, 0).ToArgb();
                                    Int32 background_pixel = color_background.ToArgb();
                                    if (bitmap_pixel == background_pixel)
                                    {
                                        object_found = false;
                                        rect_found.Add(new Rectangle(new Point(rect_x_start, 0), new Size(i - rect_x_start, bitmap_in.Height)));
                                        continue;
                                    }
                                }
                            }
                        }
                        if (object_found == true)
                        {
                            object_found = false;
                            rect_found.Add(new Rectangle(new Point(rect_x_start, 0), new Size(bitmap_in.Width - (rect_x_start + 1), bitmap_in.Height)));
                        }
                        // saved code. it does not work properly. 2024.02.11 11:04. Moscow. Hostel.
                        // it was saved to see the approach.
                        /*
                        Int32 last_x = 0;
                        bool object_start_found = false;
                        bool object_end_found = false;
                        for (Int32 i = 0; i < bitmap_in.Width; i++)
                        {
                            int[] column = MyArrayFunctions.Column.Take(bitmap_int32, i);
                            // 1st condition. pixels are different. 2024.02.11 10:36. Moscow. Hostel.
                            if (MyArrayFunctions.Int32Array.Math.ElementsTheSame(column) == false)
                            {
                                if (object_start_found == false)
                                {
                                    object_start_found = true;
                                }
                            }
                            else
                            {
                                Int32 bitmap_pixel = bitmap_in.GetPixel(i, 0).ToArgb();
                                Int32 background_pixel = color_background.ToArgb();
                                if (bitmap_pixel != background_pixel)
                                {
                                    if (object_start_found == false)
                                    {
                                        object_start_found = true;
                                    }                                    
                                }
                                else
                                {
                                    object_end_found = true;
                                }
                            }
                            if ((object_start_found == true) &&
                                (object_end_found == true))
                            {
                                object_start_found = false;
                                object_end_found = false;
                                rect_found.Add(new Rectangle(new Point(i, 0), new Size(i - last_x, bitmap_in.Height)));
                                last_x = i;
                            }
                        }
                        */
                        return rect_found.ToArray();
                    }
                }
                /// <summary>
                /// Return location of part of picture using provided color of background. <br></br>
                /// Written. 2024.02.10 19:04. Moscow. Hostel. <br></br>
                /// Tested. Works. 2024.02.10 19:38. Moscow. Hostel.
                /// </summary>
                /// <param name="bitmap_in"></param>
                /// <param name="color_background"></param>
                /// <returns></returns>
                public static Rectangle OfObject(Bitmap bitmap_in, Color color_background)
                {
                    Rectangle rectangle_out = new Rectangle();
                    Int32 left_of_rectangle = (int)LengthOfBackground.Left(bitmap_in, color_background);
                    Int32 right_of_rectangle = (int)LengthOfBackground.Right(bitmap_in, color_background);
                    Int32 top_of_rectangle = (int)LengthOfBackground.Top(bitmap_in, color_background);
                    Int32 bottom_of_rectangle = (int)LengthOfBackground.Bottom(bitmap_in, color_background);
                    rectangle_out.Location = new Point(left_of_rectangle - 1 + 1, top_of_rectangle - 1 + 1);
                    rectangle_out.Size = new Size(bitmap_in.Width - right_of_rectangle - left_of_rectangle,
                        bitmap_in.Height - bottom_of_rectangle - top_of_rectangle);
                    return rectangle_out;
                }
            }
            /// <summary>
            /// Written. 2024.02.10 14:34. Moscow. Hostel.
            /// </summary>
            public static class LengthOfBackground
            {
                /// <summary>
                /// Find the length of background using provided color of background. <br></br>
                /// Written. 2024.02.10 16:46. Moscow. Hostel. <br></br>
                /// Tested. 2024.02.10 17:06. Moscow. Hostel. 
                /// </summary>
                /// <param name="bitmap_in"></param>
                /// <param name="background_color"></param>
                /// <returns></returns>
                public static UInt32 Right(Bitmap bitmap_in, Color background_color)
                {
                    int[][] ImageInt32Array = Convert.BitmapToInt32ArrayAxB(bitmap_in);
                    Int32 columns_count = ImageInt32Array.Length;
                    Int32 length_out = 0;
                    for (Int32 i = columns_count - 1; i >= 0; i--)
                    {
                        int[] row_int32 = ArrayFunctions.Column.Take(ImageInt32Array, i);
                        if (ArrayFunctions.Int32Array.Math.ElementsTheSame(row_int32) == true)
                        {
                            length_out += 1;
                        }
                        else
                        {
                            break;
                        }
                    }
                    return (uint)length_out;
                }
                /// <summary>
                /// Find the length of background using provided color of background. <br></br>
                /// Written. 2024.02.10 16:32. Moscow. Hostel.
                /// Tested. Works. 2024.02.10 16:45. Moscow. Hostel.
                /// </summary>
                /// <param name="bitmap_in"></param>
                /// <param name="background_color"></param>
                /// <returns></returns>
                public static UInt32 Left(Bitmap bitmap_in, Color background_color)
                {
                    int[][] ImageInt32Array = Convert.BitmapToInt32ArrayAxB(bitmap_in);
                    Int32 columns_count = ImageInt32Array.Length;
                    Int32 length_out = 0;
                    for (Int32 i = 0; i < columns_count; i++)
                    {
                        int[] row_int32 = ArrayFunctions.Column.Take(ImageInt32Array, i);
                        if (ArrayFunctions.Int32Array.Math.ElementsTheSame(row_int32) == true)
                        {
                            length_out += 1;
                        }
                        else
                        {
                            break;
                        }
                    }
                    return (uint)length_out;
                }
                /// <summary>
                /// Find the length of background using provided color of background. <br></br>
                /// Written. 2024.02.10 14:52. Moscow. Hostel. <br></br>
                /// Tested. 2024.02.10 16:29. Moscow. Hostel. 
                /// </summary>
                /// <param name="bitmap_in"></param>
                /// <param name="background_color"></param>
                /// <returns></returns>
                public static UInt32 Bottom(Bitmap bitmap_in, Color background_color)
                {
                    int[][] ImageInt32Array = Convert.BitmapToInt32ArrayAxB(bitmap_in);
                    Int32 rows_count = ImageInt32Array[0].Length;
                    Int32 length_out = 0;
                    for (Int32 i = rows_count - 1; i >= 0; i--)
                    {
                        int[] row_int32 = ArrayFunctions.Row.Take(ImageInt32Array, i);
                        if (ArrayFunctions.Int32Array.Math.ElementsTheSame(row_int32) == true)
                        {
                            length_out += 1;
                        }
                        else
                        {
                            break;
                        }
                    }
                    return (uint)length_out;
                }
                /// <summary>
                /// Written. 2024.02.10 14:40. Moscow. Hostel.
                /// Tested. Works. 2024.02.10 14:51. Moscow. Hostel.
                /// </summary>
                /// <param name="bitmap_in"></param>
                /// <param name="background_color"></param>
                /// <returns></returns>
                public static UInt32 Top(Bitmap bitmap_in, Color background_color)
                {
                    int[][] ImageInt32Array = Convert.BitmapToInt32ArrayAxB(bitmap_in);
                    Int32 rows_count = ImageInt32Array[0].Length;
                    Int32 length_out = 0;
                    for (Int32 i = 0; i < rows_count; i++)
                    {
                        int[] row_int32 = ArrayFunctions.Row.Take(ImageInt32Array, i);
                        if (ArrayFunctions.Int32Array.Math.ElementsTheSame(row_int32) == true)
                        {
                            length_out += 1;
                        }
                        else
                        {
                            break;
                        }
                    }
                    return (uint)length_out;
                }
            }
        }
        /// <summary>
        /// Written. 2024.02.10 13:10. Moscow. Hostel 
        /// </summary>
        public static class Trim
        {
            /// <summary>
            /// Removes part of Bitmap from bottom according to provided length. <br></br>
            /// Written. 2024.02.10 14:55. Moscow. Hostel. <br></br>
            /// Tested. Works. 2024.02.10 14:57. Moscow. Hostel.
            /// <param name="bitmap_in"></param>
            /// <param name="trim_length"></param>
            /// <returns></returns>
            public static Bitmap Bottom(Bitmap bitmap_in, UInt32 trim_length)
            {
                Bitmap bitmap_out = new Bitmap(bitmap_in.Width, bitmap_in.Height - (int)trim_length);
                using (Graphics graphics_draw = Graphics.FromImage(bitmap_out))
                {
                    Rectangle source_rect = new Rectangle(new Point(0, 0), new Size(bitmap_in.Width, bitmap_in.Height - (int)trim_length));
                    Rectangle dest_rect = new Rectangle(new Point(0, 0), new Size(bitmap_out.Width, bitmap_out.Height));
                    graphics_draw.DrawImage(bitmap_in, dest_rect, source_rect, GraphicsUnit.Pixel);
                }
                return bitmap_out;
            }
            /// <summary>
            /// Removes part of Bitmap from left according to provided length. <br></br>
            /// Written. 2024.02.10 14:59. Moscow. Hostel. <br></br>
            /// Tested. Works. 2024.02.10 15:01. Moscow. Hostel. 
            /// </summary>
            /// <param name="bitmap_in"></param>
            /// <param name="trim_length"></param>
            /// <returns></returns>
            public static Bitmap Left(Bitmap bitmap_in, UInt32 trim_length)
            {
                Bitmap bitmap_out = new Bitmap(bitmap_in.Width - (int)trim_length, bitmap_in.Height);
                using (Graphics graphics_draw = Graphics.FromImage(bitmap_out))
                {
                    Rectangle source_rect = new Rectangle(new Point((int)trim_length - 1 + 1, 0), new Size(bitmap_in.Width - (int)trim_length, bitmap_in.Height));
                    Rectangle dest_rect = new Rectangle(new Point(0, 0), new Size(bitmap_out.Width, bitmap_out.Height));
                    graphics_draw.DrawImage(bitmap_in, dest_rect, source_rect, GraphicsUnit.Pixel);
                }
                return bitmap_out;
            }
            /// <summary>
            /// Removes part of Bitmap from right according to provided length. <br></br>
            /// Written. 2024.02.10 15:02. Moscow. Hostel. <br></br>
            /// Tested. Works. 2024.02.10 15:04. Moscow. Hostel.
            /// </summary>
            /// <param name="bitmap_in"></param>
            /// <param name="trim_length"></param>
            /// <returns></returns>
            public static Bitmap Right(Bitmap bitmap_in, UInt32 trim_length)
            {
                Bitmap bitmap_out = new Bitmap(bitmap_in.Width - (int)trim_length, bitmap_in.Height);
                using (Graphics graphics_draw = Graphics.FromImage(bitmap_out))
                {
                    Rectangle source_rect = new Rectangle(new Point(0, 0), new Size(bitmap_in.Width - (int)trim_length, bitmap_in.Height));
                    Rectangle dest_rect = new Rectangle(new Point(0, 0), new Size(bitmap_out.Width, bitmap_out.Height));
                    graphics_draw.DrawImage(bitmap_in, dest_rect, source_rect, GraphicsUnit.Pixel);
                }
                return bitmap_out;
            }
            /// <summary>
            /// Written. 2024.02.10 13:25. Moscow. Hostel.
            /// Tested. Works. 2024.02.10 14:31. Moscow. Hostel.
            /// </summary>
            /// <param name="bitmap_in"></param>
            /// <param name="trim_length"></param>
            /// <returns></returns>
            public static Bitmap Top(Bitmap bitmap_in, UInt32 trim_length)
            {
                Bitmap bitmap_out = new Bitmap(bitmap_in.Width, bitmap_in.Height - (int)trim_length);
                using (Graphics graphics_draw = Graphics.FromImage(bitmap_out))
                {
                    Rectangle source_rect = new Rectangle(new Point(0, (int)trim_length - 1 + 1), new Size(bitmap_in.Width, bitmap_in.Height - (int)trim_length));
                    Rectangle dest_rect = new Rectangle(new Point(0, 0), new Size(bitmap_out.Width, bitmap_out.Height));
                    graphics_draw.DrawImage(bitmap_in, dest_rect, source_rect, GraphicsUnit.Pixel);
                }
                return bitmap_out;
            }
        }
        /// <summary>
        /// Written. 2024.02.25 19:20. Moscow. Hostel.
        /// </summary>
        public static class ExtractValues
        {
            /// <summary>
            /// Written. 2024.02.25 19:20. Moscow. Hostel.
            /// not tested.
            /// </summary>
            public static class FromPoints
            {
                public static int[] XValues(Point[] arr_in)
                {
                    int[] arr_out = new int[arr_in.Length];
                    for (Int32 i = 0; i < arr_out.Length; i++)
                    {
                        arr_out[i] = arr_in[i].X;
                    }
                    return arr_out;
                }
                /// <summary>
                /// Written. 2024.02.25 19:23. Moscow. Hostel.
                /// not tested.
                /// </summary>
                /// <param name="arr_in"></param>
                /// <returns></returns>
                public static int[] YValues(Point[] arr_in)
                {
                    int[] arr_out = new int[arr_in.Length];
                    for (Int32 i = 0; i < arr_out.Length; i++)
                    {
                        arr_out[i] = arr_in[i].Y;
                    }
                    return arr_out;
                }
            }
        }
        public static class Crop
        {
            /// <summary>
            /// Written. 2024.03.10 16:07. Moscow. Hostel.
            /// Tested. Works. 2024.03.10 17:21. Moscow. Hostel. 
            /// </summary>
            /// <param name="bitmap_in"></param>
            /// <param name="length_of_rectangle"></param>
            /// <returns></returns>
            public static Bitmap[] HorizontallyByLength(Bitmap bitmap_in, Int32 length_of_rectangle)
            {
                double execution_time_ms_start = 0;
                if (TimeExecutionShow == true)
                {
                    execution_time_ms_start = _time_execution.Elapsed.TotalMilliseconds;
                }
                Int32 amount_rectangles = 0;
                Int32 remained_length = bitmap_in.Width;
                bool counting_rectangles = true;
                while (counting_rectangles == true)
                {
                    if (remained_length >= (length_of_rectangle + 1))
                    {
                        amount_rectangles += 1;
                        remained_length -= (length_of_rectangle);
                        continue;
                    }
                    if (remained_length >= length_of_rectangle)
                    {
                        amount_rectangles += 1;
                        counting_rectangles = false;
                        continue;
                    }
                    counting_rectangles = false;
                }
                Rectangle[] Rectangles_For_Crop = new Rectangle[amount_rectangles];
                for (Int32 i = 0; i < Rectangles_For_Crop.Length; i++)
                {
                    Rectangles_For_Crop[i] = new Rectangle(new Point(i * (length_of_rectangle - 1 + 1), 0), new Size(length_of_rectangle, bitmap_in.Height));
                }
                Bitmap[] Bitmap_Arr_Out = new Bitmap[amount_rectangles];
                Bitmap_Arr_Out = Crop.MultipleRectangles(bitmap_in, Rectangles_For_Crop);
                if (TimeExecutionShow == true)
                {
                    double execution_time_ms_stop = _time_execution.Elapsed.TotalMilliseconds;
                    TimeExecutionMessage(nameof(Crop.HorizontallyByLength), execution_time_ms_stop - execution_time_ms_start);
                }
                return Bitmap_Arr_Out;
            }
            /// <summary>
            /// Written. 2024.02.11 17:03. Moscow. Hostel.
            /// Tested. Works. 2024.02.11 18:25. Moscow. Hostel.
            /// </summary>
            /// <param name="bitmap_in"></param>
            /// <param name="mark_color"></param>
            /// <param name="color_background">
            /// 2024.03.06 16:20. Moscow. Workplace. <br></br>
            /// It is needed to replace mark color with background color in working Bitmap. 
            /// </param>
            /// <returns></returns>
            public static Bitmap[] HorizontallyByMark(Bitmap bitmap_in, Color mark_color, Color color_background)
            {
                double execution_time_ms_start = 0;
                if (TimeExecutionShow == true)
                {
                    execution_time_ms_start = _time_execution.Elapsed.TotalMilliseconds;
                }
                Bitmap bitmap_to_work = new Bitmap(bitmap_in);
                List<int> mark_values = new List<int>();
                // transparent is not error. it shows 0 can be moved to initialization. 2024.02.11 17:22. Moscow. Hostel.
                mark_values.Add(0);
                int[][] bitmap_int32 = Convert.BitmapToInt32ArrayAxB(bitmap_to_work);
                // 1st row maked difficult to put the marks. 2024.02.14 12:43. Moscow. Workplace. 
                /*
                int[] row_1st = MyArrayFunctions.Row.Take(bitmap_int32, 0);
                for (Int32 i = 0; i < row_1st.Length; i++)
                {
                */
                for (Int32 i = 0; i < bitmap_int32.Length; i++)
                {
                    int[] column_int32 = ArrayFunctions.Column.Take(bitmap_int32, i);
                    if (column_int32.Contains(mark_color.ToArgb()) == true)
                    {
                        mark_values.Add(i);
                        bitmap_to_work.SetPixel(i, Array.IndexOf(column_int32, mark_color.ToArgb()), color_background);
                    }
                }
                mark_values.Add(bitmap_to_work.Width - 1);
                Rectangle[] rectangles_crop = new Rectangle[mark_values.Count - 1];
                for (Int32 i = 1; i < mark_values.Count; i++)
                {
                    rectangles_crop[i - 1] = new Rectangle(new Point(mark_values[i - 1], 0), new Size(mark_values[i] - mark_values[i - 1], bitmap_to_work.Height));
                }
                Bitmap[] bitmaps_crop = MultipleRectangles(bitmap_to_work, rectangles_crop);
                if (TimeExecutionShow == true)
                {
                    double execution_time_ms_stop = _time_execution.Elapsed.TotalMilliseconds;
                    TimeExecutionMessage(nameof(HorizontallyByMark), execution_time_ms_stop - execution_time_ms_start);
                }
                return bitmaps_crop;
            }
            /// <summary>
            /// Return Bitmap[] after cropping image using provided crop areas array. <br></br>
            /// Written. 2024.02.11 11:39. Moscow. Hostel.
            /// </summary>
            /// <param name="image_in"></param>
            /// <param name="rectangles_in"></param>
            /// <returns></returns>
            public static Bitmap[] MultipleRectangles(Bitmap image_in, Rectangle[] rectangles_in)
            {
                Bitmap[] bitmap_arr_out = new Bitmap[rectangles_in.Length];
                for (Int32 i = 0; i < bitmap_arr_out.Length; i++)
                {
                    bitmap_arr_out[i] = Rectangle(image_in, rectangles_in[i]);
                }
                return bitmap_arr_out;
            }
            /// <summary>
            /// Return part of Bitmap. <br></br>
            /// Written. 2024.02.10 18:50. Moscow. Hostel. <br></br>
            /// Tested. Works. 2024.02.10 18:56. Moscow. Hostel. 
            /// </summary>
            /// <param name="image_in"></param>
            /// <param name="rectangle_in"></param>
            /// <returns></returns>
            public static Bitmap Rectangle(Bitmap image_in, Rectangle rectangle_in)
            {
                return Rectangle(image_in, (uint)rectangle_in.Left, (uint)rectangle_in.Top, (uint)rectangle_in.Width, (uint)rectangle_in.Height);
            }
            /// <summary>
            /// Return part of Bitmap. <br></br>
            /// Written. 2023.11.08 21:06. Moscow. Workplace. <br></br> 
            /// Tested. Works. 2024.02.10 17:43. Moscow. Hostel.
            /// </summary>
            /// <param name="image_in"></param>
            /// <param name="w_start"></param>
            /// <param name="h_start"></param>
            /// <param name="w_size"></param>
            /// <param name="h_size"></param>
            /// <returns></returns>
            public static Bitmap Rectangle(Bitmap image_in, UInt32 w_start, UInt32 h_start, UInt32 w_size, UInt32 h_size)
            {
                // 2024.02.10 17:48. Moscow. Hostel.
                // There static class Crop. There were no need to have class and therefore 
                // Crop is moved to MyImageFunctions.
                int[][] arr_image = BitmapToInt32ArrayAxB(image_in);
                int[][] arr_part_image = ArrayFunctions.Extract.PartAxBFromCxD(arr_image, w_start, w_size, h_start, h_size);
                return Int32ArrayAxBToBitmap(arr_part_image);
            }
        }
        /// <summary>
        /// Written. 2023.11.08 21:04. Moscow. Workplace. 
        /// Maybe from class to functions. There is no need for class Crop. 2024.02.10 13:09. Moscow. Hostel 
        /// </summary>
        public static class NotInUse
        {
            /// <summary>
            /// Written. 2023.11.08 21:05. Moscow. Workplace. 
            /// Not Tesed. 2024.02.10 17:46. Moscow. Hostel.
            /// There is MyArrayFunctions to with array. There is currently no need in this function.
            /// </summary>
            /// <param name="arr_in"></param>
            /// <param name="w_start"></param>
            /// <param name="h_start"></param>
            /// <param name="w_size"></param>
            /// <param name="h_size"></param>
            /// <returns></returns>
            public static Bitmap FromInt32Array(int[][] arr_in, UInt32 w_start, UInt32 h_start, UInt32 w_size, UInt32 h_size)
            {
                int[][] arr_part_image = ArrayFunctions.Extract.PartAxBFromCxD(arr_in, w_start, w_size, h_start, h_size);
                return Int32ArrayAxBToBitmap(arr_part_image);
            }
        }

        /// <summary>
        /// Written. 2024.03.10 19:58. Moscow. Hostel. 
        /// </summary>
        public static class SpecialConversion
        {

            /// <summary>
            /// Written. 2024.03.10 20:05. Moscow. Hostel.
            /// Tested. Works. 2024.03.10 20:40. Moscow. Hostel. 
            /// </summary>
            /// <param name="bitmap_in"></param>
            /// <param name="chars_in"></param>
            /// <returns></returns>
            public static char[][] BitmapToText(Bitmap bitmap_in, char[] chars_in)
            {

                double execution_time_ms_start = 0;
                if (TimeExecutionShow == true)
                {
                    execution_time_ms_start = _time_execution.Elapsed.TotalMilliseconds;
                }


                Int32 level_count = chars_in.Length;
                Int32 level_of_color = 255 / level_count;
                Bitmap bitmap_black_white = Convert.ToBlackWhiteBitmap(bitmap_in);
                char[][] char_arr_out = new char[bitmap_in.Width][];

                for (Int32 i = 0; i < char_arr_out.Length; i++)
                {
                    char_arr_out[i] = new char[bitmap_in.Height];
                    for (Int32 j = 0; j < char_arr_out[i].Length; j++)
                    {
                        Color pixel_color = bitmap_black_white.GetPixel(i, j);

                        // 2024.03.10 20:14. Moscow. Hostel. 
                        // Math is good but with Int32 and lost of accuracy because of
                        // devision causes the code work not correctly.
                        Int32 char_of_color = 1;

                        bool is_level_found = false;
                        while (is_level_found == false)
                        {
                            if ((level_of_color * char_of_color) < ((int)pixel_color.B))
                            {
                                char_of_color += 1;
                            }
                            else
                            {
                                is_level_found = true;
                            }

                            if (char_of_color >= chars_in.Length)
                            {
                                is_level_found = true;
                            }

                        }

                        if (is_level_found == false)
                        {
                            ReportFunctions.ReportError(ReportFunctions.ErrorMessage.Length_is_Wrong);
                            return new char[0][];
                        }

                        char_arr_out[i][j] = chars_in[char_of_color - 1];

                    }
                    }


                if (TimeExecutionShow == true)
                {
                    double execution_time_ms_stop = _time_execution.Elapsed.TotalMilliseconds;
                    TimeExecutionMessage(nameof(BitmapToText), execution_time_ms_stop - execution_time_ms_start);
                }


                return char_arr_out;

            }


        }


            /// <summary>
            /// Written. 2023.11.08 20:13. Moscow. Workplace. 
            /// </summary>
            public static class Convert
        {
            /// <summary>
            /// Written. 2024.03.07 12:06. Moscow. Workplace. <br></br>
            /// Tested. Works. 2024.03.07 12:28. Moscow. Workplace. <br></br> 
            /// <br></br>
            /// Note. 1600x900. 5s execution time with 3 times GetPixel. 2024.03.07 12:25. Moscow. Workplace. <br></br>
            /// 2.5s execution time with 1 time GetPixel <br></br>
            /// 15-20 ms for 100x100 image. 2024.03.07 12:43. Moscow. Workplace. 
            /// </summary>
            /// <param name="image_in"></param>
            /// <returns></returns>
            static public Bitmap ToBlackWhiteBitmap(Bitmap image_in)
            {
                double execution_time_ms_start = 0;
                if (TimeExecutionShow == true)
                {
                    execution_time_ms_start = _time_execution.Elapsed.TotalMilliseconds;
                }
                Bitmap bitmap_out = new Bitmap(image_in.Width, image_in.Height);
                for (Int32 i = 0; i < image_in.Height; i++)
                {
                    for (Int32 j = 0; j < image_in.Width; j++)
                    {
                        Color pixel_color = image_in.GetPixel(j, i);
                        byte average_pixels =
                            (byte)(((uint)pixel_color.R +
                            (uint)pixel_color.G +
                            (uint)pixel_color.B) / 3);
                        bitmap_out.SetPixel(j, i, Color.FromArgb(average_pixels, average_pixels, average_pixels));
                    }
                }
                if (TimeExecutionShow == true)
                {
                    double execution_time_ms_stop = _time_execution.Elapsed.TotalMilliseconds;
                    TimeExecutionMessage(nameof(ToBlackWhiteBitmap), execution_time_ms_stop - execution_time_ms_start);
                }
                return bitmap_out;
            }
            /// <summary>
            /// Written. 2024.03.07 07:46. Moscow. Workplace. <br></br>
            /// Tested. Works. 2024.03.07 07:53. Moscow. Workplace. 
            /// </summary>
            /// <param name="image_in"></param>
            /// <returns></returns>
            static public Color[] BitmapToColorArray(Bitmap image_in)
            {
                Color[] pixels_out = new Color[image_in.Width * image_in.Height];
                for (Int32 i = 0; i < image_in.Height; i++)
                {
                    for (Int32 j = 0; j < image_in.Width; j++)
                    {
                        pixels_out[i * image_in.Width + j] = image_in.GetPixel(j, i);
                    }
                }
                return pixels_out;
            }
            /// <summary>
            /// Written. 2024.03.06 17:07. Moscow. Workplace. 
            /// </summary>
            /// <param name="image_in"></param>
            /// <param name="alpha_ch"></param>
            /// <returns></returns>
            public static int[] BitmapToInt32Array(Bitmap image_in, Int32 alpha_ch = -1)
            {
                double execution_time_ms_start = 0;
                if (TimeExecutionShow == true)
                {
                    execution_time_ms_start = _time_execution.Elapsed.TotalMilliseconds;
                }
                int[][] bitmap_int32_2D_array = BitmapToInt32ArrayAxB(image_in, alpha_ch);
                int[] arr_out = ArrayFunctions.Int32Array.Merge.AxB_To_C(bitmap_int32_2D_array);
                if (TimeExecutionShow == true)
                {
                    double execution_time_ms_stop = _time_execution.Elapsed.TotalMilliseconds;
                    TimeExecutionMessage(nameof(BitmapToInt32Array), execution_time_ms_stop - execution_time_ms_start);
                }
                return arr_out;
            }
            /// <summary>
            /// Tested. Works. 2024.02.09 10:35. Moscow. Workplace. <br></br>
            /// Modified. Added alpha_ch. 2024.02.14 13:13. Moscow. Workplace. 
            /// </summary>
            /// <param name="image_in"></param>
            /// <returns></returns>
            public static int[][] BitmapToInt32ArrayAxB(Bitmap image_in, Int32 alpha_ch = -1)
            {
                int[][] arr_out = new int[image_in.Width][];
                for (Int32 i = 0; i < arr_out.Length; i++)
                {
                    arr_out[i] = new int[image_in.Height];
                    for (Int32 j = 0; j < arr_out[i].Length; j++)
                    {
                        Color pixel_color = image_in.GetPixel(i, j);
                        arr_out[i][j] = pixel_color.ToArgb();
                        if (alpha_ch != -1)
                        {
                            arr_out[i][j] = Color.FromArgb(alpha_ch, (int)pixel_color.R, (int)pixel_color.G, (int)pixel_color.B).ToArgb();
                        }
                    }
                }
                return arr_out;
            }
            /// <summary>
            /// Written. 2024.02.09 09:33. Moscow. Workplace. 
            /// </summary>
            /// <param name="arr_in"></param>
            /// <returns></returns>
            public static Bitmap ColorArrayToBitmap(Color[][] arr_in)
            {
                Bitmap image_out = new Bitmap(arr_in.Length, arr_in[0].Length);
                for (Int32 i = 0; i < arr_in.Length; i++)
                {
                    for (Int32 j = 0; j < arr_in[i].Length; j++)
                    {
                        image_out.SetPixel(i, j, arr_in[i][j]);
                    }
                }
                return image_out;
            }
            /// <summary>
            /// Written. 2024.02.09 09:37. Moscow. Workplace. 
            /// </summary>
            /// <param name="color_arr_in"></param>
            /// <returns></returns>
            public static Bitmap ColorArrayToBitmap(Color[] color_arr_in, Int32 row_length)
            {
                Bitmap bitmap_out = new Bitmap(row_length, (color_arr_in.Length / row_length));
                Int32 rows_count = (color_arr_in.Length / row_length);
                for (Int32 j = 0; j < rows_count; j++)
                {
                    for (Int32 i = 0; i < row_length; i++)
                    {
                        bitmap_out.SetPixel(i, j, color_arr_in[j * row_length + i]);
                    }
                }
                return bitmap_out;
            }
            /// <summary>
            /// Written. 2023.11.08 20:26. Moscow. Workplace. 
            /// </summary>
            /// <param name="arr_in"></param>
            /// <returns></returns>
            public static Bitmap Int32ArrayToBitmap(int[][] arr_in)
            {
                Bitmap image_out = new Bitmap(arr_in.Length, arr_in[0].Length);
                for (Int32 i = 0; i < arr_in.Length; i++)
                {
                    for (Int32 j = 0; j < arr_in[i].Length; j++)
                    {
                        image_out.SetPixel(i, j, Color.FromArgb(arr_in[i][j]));
                    }
                }
                return image_out;
            }
        }
        public static Bitmap Screenshot()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height);
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.CopyFromScreen(Point.Empty, Point.Empty, bounds.Size);
            }
            return bitmap;
        }
        /// <summary>
        /// JPG file as byte array to JPG file
        /// 2023.8.12 15:43
        /// </summary>
        /// <param name="arr_in"></param>
        /// <param name="filename"></param>
        static public void FileJPGByteArrayToFileJPG(byte[] arr_in, string filename)
        {
            try
            {
                FileStream file_read = System.IO.File.Open(filename, FileMode.OpenOrCreate);
                file_read.Write(arr_in, 0, arr_in.Length);
                file_read.Close();
            }
            catch
            {
                ReportFunctions.ReportError();
            }
        }
        static public Color Int32ToColor(Int32 value_in)
        {
            return Color.FromArgb(value_in);
        }
        static public Color Int32ToColor(byte[] bytes_in)
        {
            Int32 value = MathFunctions.Int32Number.BytesToInt32(bytes_in);
            return Color.FromArgb(value);
        }
        static public Color[][] Int32ArrayAxBToColorsAxB(int[][] values_in)
        {
            Color[][] colors_out = new Color[values_in.Length][];
            for (Int32 i = 0; i < values_in.Length; i++)
            {
                colors_out[i] = new Color[values_in[i].Length];
                for (Int32 j = 0; j < values_in[i].Length; j++)
                {
                    colors_out[i][j] = Color.FromArgb(values_in[i][j]);
                }
            }
            return colors_out;
        }
        
        static public void BitmapToConsole(string filename, Int32 spaces = 5)
        {
            Bitmap bitmap_from_file = FileBMPToBitmap(filename);
            BitmapToConsole(bitmap_from_file, spaces);
        }
        static public void BitmapToConsole(Bitmap image_in, Int32 spaces = 5)
        {
            Color[][] colors = BitmapToColorArrayAxB(image_in);
            ColorArrayAxBToConsole(colors, spaces);
        }
        static public void ColorArrayAxBToConsole(int[][] colors_in, Int32 spaces = 5)
        {
            ColorArrayAxBToConsole(Int32ArrayAxBToColorsAxB(colors_in));
        }
        static public void ColorArrayAxBToConsole(Color[][] colors_in, Int32 spaces = 5)
        {
            if (colors_in.Length == 0)
            {
                ReportFunctions.ReportError("Trouble. No colors\r\n" + "Colors Height is " + colors_in.Length.ToString() + "\r\n");
                return;
            }
            string str_out = "";
            str_out += "Colors. Height " + colors_in.Length.ToString() + ". Width " + colors_in[0].Length.ToString() + ".\r\n";
            string spaces_str = "".PadRight(5, ' ');
            for (Int32 i = 0; i < colors_in.Length; i++)
            {
                if (i != 0)
                {
                    str_out += "\r\n";
                }
                for (Int32 j = 0; j < colors_in[i].Length; j++)
                {
                    str_out += colors_in[i][j].ToArgb().ToString().PadLeft(10, ' ') + spaces_str;
                }
            }
            str_out += "\r\n";
            str_out += "\r\n";
            Console.Write(str_out);
        }
        static public void ColorToConsole(Color color_in)
        {
            Console.WriteLine("Color " + color_in.ToArgb().ToString());
            Console.WriteLine("Color bytes " + color_in.A.ToString() +
                color_in.R.ToString() + color_in.G.ToString() + color_in.B.ToString());
            Console.WriteLine();
        }
        static public Bitmap Int32ArrayAxBToBitmap(int[][] values_in)
        {
            if (values_in.Length == 0)
            {
                ReportFunctions.ReportError("Array is empty. Array size is " + values_in.Length.ToString());
                // Bitmap(0,0) gives error that such values can not be used. 2024.02.10 19:13. Moscow. Hostel.
                return new Bitmap(1, 1);
            }
            Bitmap image_out = new Bitmap(values_in[0].Length, values_in.Length);
            for (Int32 i = 0; i < image_out.Height; i++)
            {
                for (Int32 j = 0; j < image_out.Width; j++)
                {
                    image_out.SetPixel(j, i, Color.FromArgb(values_in[i][j]));
                }
            }
            return image_out;
        }
        static public int[][] BitmapToInt32ArrayAxB(Bitmap image_in)
        {
            int[][] pixels_out = new int[image_in.Height][];
            for (Int32 i = 0; i < image_in.Height; i++)
            {
                pixels_out[i] = new int[image_in.Width];
                for (Int32 j = 0; j < image_in.Width; j++)
                {
                    pixels_out[i][j] = image_in.GetPixel(j, i).ToArgb();
                }
            }
            return pixels_out;
        }
        static public Color[][] BitmapToColorArrayAxB(Bitmap image_in)
        {
            Color[][] pixels_out = new Color[image_in.Height][];
            for (Int32 i = 0; i < image_in.Height; i++)
            {
                pixels_out[i] = new Color[image_in.Width];
                for (Int32 j = 0; j < image_in.Width; j++)
                {
                    pixels_out[i][j] = image_in.GetPixel(j, i);
                }
            }
            return pixels_out;
        }
        static public byte[] BitmapToFileJPGByteArray(Bitmap bitmap_in)
        {
            byte[] arr_out = new byte[0];
            try
            {
                string filename = "picture" + "_" + nameof(BitmapToFileJPGByteArray) + ".jpg";
                BitmapToFileJPG(bitmap_in, filename);
                arr_out = FileJPGToFileJPGByteArray(filename);
            }
            catch
            {
                ReportFunctions.ReportError();
            }
            return arr_out;
        }
        // 2024.02.09 20:34. Moscow. Hostel 
        [Obsolete]
        static public void BitmapToFileBMP(Bitmap bitmap_in, string filename)
        {
            // code moved. 2024.02.09 20:35. Moscow. Hostel 
            FileFunctions.ImageFile.WriteFile.BitmapToFileBMP(bitmap_in, filename);
        }
        static public void BitmapToFileJPG(Bitmap bitmap_in, string filename)
        {
            try
            {
                ImageCodecInfo jgpEncoder = GetEncoder(ImageFormat.Jpeg);
                System.Drawing.Imaging.Encoder myEncoder =
                System.Drawing.Imaging.Encoder.Quality;
                EncoderParameters myEncoderParameters = new EncoderParameters(1);
                EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder,
                    90L);
                myEncoderParameters.Param[0] = myEncoderParameter;
                //  img.Save(filename, jgpEncoder,   myEncoderParameters);
                bitmap_in.Save(filename, jgpEncoder, myEncoderParameters);
            }
            catch
            {
                ReportFunctions.ReportError();
            }
        }
        public static ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }
        static public byte[] FileJPGToFileJPGByteArray(string filename)
        {
            byte[] arr_out = new byte[0];
            try
            {
                FileStream file_read = System.IO.File.Open(filename, FileMode.OpenOrCreate);
                arr_out = new byte[file_read.Length];
                file_read.Read(arr_out, 0, arr_out.Length);
                file_read.Close();
            }
            catch
            {
                ReportFunctions.ReportError();
            }
            return arr_out;
        }
        public static Bitmap JPGByteArrayToBitmap(byte[] arr_in)
        {
            // not tested 2023-01-24 16:03
            Bitmap bitmap_for_return = null;
            try
            {
                MemoryStream memory_write = new MemoryStream();
                memory_write.Write(arr_in, 0, (int)arr_in.Length);
                bitmap_for_return = Bitmap.FromStream(memory_write) as Bitmap;
            }
            catch
            {
                ReportFunctions.ReportError();
            }
            return bitmap_for_return;
        }
        /// <summary>
        /// JPG image as byte array to Bitmap
        /// 2023.8.12 15:36
        /// </summary>
        /// <param name="arr_in"></param>
        /// <returns></returns>
        public static Bitmap JPGFileByteArrayToBitmap(byte[] arr_in)
        {
            Bitmap bitmap_for_return = null;
            try
            {
                MemoryStream memory_write = new MemoryStream();
                memory_write.Write(arr_in, 0, (int)arr_in.Length);
                bitmap_for_return = Bitmap.FromStream(memory_write) as Bitmap;
            }
            catch
            {
                ReportFunctions.ReportError();
            }
            return bitmap_for_return;
        }
        //public static byte[] BitmapToFileJPGByteArray(Bitmap bitmap_in)
        //{
        //    byte[] arr_out = new byte[0];
        //    try
        //    {
        //        using (MemoryStream memory_write = new MemoryStream())
        //        {
        //            bitmap_in.Save(memory_write, System.Drawing.Imaging.ImageFormat.Bmp);
        //            arr_out = new byte[memory_write.Length];
        //            memory_write.Read(arr_out, 0, (int)memory_write.Length);
        //        }
        //    }
        //    catch
        //    {
        //        MyReportErrorMethods.ReportError();
        //    }
        //    return arr_out;
        //}

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


            double execution_time_ms_start = 0;
            if (TimeExecutionShow == true)
            {
                execution_time_ms_start = _time_execution.Elapsed.TotalMilliseconds;
            }
            Bitmap bitmap_return = new Bitmap(filename);
            if (TimeExecutionShow == true)
            {
                double execution_time_ms_stop = _time_execution.Elapsed.TotalMilliseconds;
                TimeExecutionMessage(nameof(FileBMPToBitmap), execution_time_ms_stop - execution_time_ms_start);
            }
            return bitmap_return;
        }



    }
}
