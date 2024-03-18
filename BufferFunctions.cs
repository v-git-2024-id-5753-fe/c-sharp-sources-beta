using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Timer = System.Windows.Forms.Timer;

// Commercial use (license)
// Please study about commercial use of the code that is publicly available 

/// Written. 2023.11.11 16:50. Moscow. Hostel Дельта.
namespace BufferNamespace
{
    /// <summary>
    /// Written. 2023.11.14 10:49. Moscow. Workplace. 
    /// </summary>
    public class CircleBufferTest
    {
        Timer WriteTimer = new Timer();
        Timer ReadTimer = new Timer();
        public CircleBuffer<int> BufferTest_Int32 = new CircleBuffer<int>();
        public CircleBuffer<string> BufferTest_String = new CircleBuffer<string>();
        public CircleBufferTest(Int32 read_time = 500, Int32 write_time = 100)
        {
            WriteTimer.Interval = write_time;
            ReadTimer.Interval = read_time;
            WriteTimer.Tick += WriteTimer_Tick;
            ReadTimer.Tick += ReadTimer_Tick;
        }
        public void Start()
        {
            WriteTimer.Start();
            ReadTimer.Start();
        }
        Int32 NumberInt32ToBuffer = 0;
        private void ReadTimer_Tick(object sender, EventArgs e)
        {
            ReadTimer.Stop();
            Int32 bytes_read = BufferTest_Int32.ReadInt32Count;

            for (Int32 i = 0; i < bytes_read; i++)
            {
                Int32 number = BufferTest_Int32.ReadInt32();
                Console.WriteLine(number + " " + BufferTest_String.ReadInt32());
            }
            ReadTimer.Start();
        }
       
        private void WriteTimer_Tick(object sender, EventArgs e)
        {
            WriteTimer.Stop();
            BufferTest_Int32.LoadInt32(NumberInt32ToBuffer);
            BufferTest_String.LoadInt32("D" + NumberInt32ToBuffer.ToString());
            NumberInt32ToBuffer++;         
            WriteTimer.Start();
        }
    }
    /// <summary>
    ///  Written. 2023.11.11 16:00 - 16:30. Moscow. Hostel Дельта. <br></br>
    ///  Tested. Works. 2024.02.05 09:44. Moscow. Workplace. <br></br>
    ///  <br></br>
    ///  Note. There is callback for buffer overflow. <br></br>
    ///  Currently, not overflowing buffer is the way to use circle buffer. Callback is currently for debug.
    /// </summary>
    // Developing. 2023.11.18 16:51. Moscow. Hostel Дельта. 
    // Tested.Worked.By using Read, Write timer. 2023.11.18 16:10. Moscow.Hostel Дельта.
    //
    // 1. 2023.11.18 16:55. Moscow. Hostel Дельта. 
    // Trouble with indexes. Restarting write gives index 0.
    // fill index indicates that previous number is ready to be read.
    // therefore there is need to write condition for fill index = 0 or work
    // with index -1.
    // 
    // Added. 2024.02.05 09:45. Moscow. Workplace.
    // It was so until there is variable BufferFillCount.
    //
    // 2. Status array. 2023.11.18 16:55. Moscow. Hostel Дельта. 
    // array with codes 1,2,3 etc. 1 is filled. 2 is read. 3 can be error code - 
    // read occured when code was 2 (read) so the same number is read 2 times.
    public class CircleBuffer<T>
    {


        // 2024.02.05 09:54. Moscow. Workplace. Generic class for circle buffer
       /*
        class Person<T>
        {
            public T Id { get; set; }
            public string Name { get; set; }
            public Person(T id, string name)
            {
                Id = id;
                Name = name;
            }
        }
       */



        public CircleBuffer()
        {
            BufferSize = 32;
        }
        T[] BufferInt32Array = new T[0];
        /// <summary>
        /// Written. 2023.11.11 16:52. Moscow. Hostel Дельта. <br></br>
        /// Sets buffer size. Do not save current data.
        /// </summary>
        public Int32 BufferSize
        {
            get
            {
                return BufferInt32Array.Length;
            }
            set
            {
                BufferInt32Array = new T[value];
                BufferFillIndex = 0;
            }
        }
        public CircleBuffer(Int32 buffer_size)
        {
            BufferSize = buffer_size;
        }
        /// <summary>
        /// Written. 2023.11.11 16:57. Moscow. Hostel Дельта.<br></br>
        /// Index of free number in array to write to
        /// </summary>
        Int32 BufferFillIndex = 0;

        // For buffer overflow. 2024.02.05 09:20. Moscow. Workplace. 
        Int32 BufferFillCount = 0;
        public delegate void BufferOverflowCallbackDeleagte(T[] buffer_data);
        public BufferOverflowCallbackDeleagte BufferOverflowCallback = null;


        bool BufferEndFilled = false;
        public void LoadInt32(T number_in)
        {
            BufferInt32Array[BufferFillIndex] = number_in;
            BufferFillIndex++;
            BufferFillCount += 1;
            if (BufferFillIndex > (BufferInt32Array.Length - 1))
            {
                BufferFillIndex = 0;
                BufferEndFilled = true;
            }

            // added. Buffer overflow. 2024.02.05 09:22. Moscow. Workplace.
            if (BufferFillCount > BufferSize)
            {
                if (BufferOverflowCallback != null)
                {
                    BufferOverflowCallback(BufferInt32Array);
                }
            }

        }
        /// <summary>
        /// Written. 2023.11.11 17:00. Moscow. Hostel Дельта. <br></br>
        /// Index of last Int32 that was read.
        /// </summary>
        Int32 BufferReadIndex = 0;
        /// <summary>
        /// Written. 2023.11.11 17:03. Moscow. Hostel Дельта. <br></br>
        /// Returns In32 from buffer if there is number available or returns 0 <br></br>
        /// Use ReadInt32Count
        /// </summary>
        /// <returns></returns>
        public T ReadInt32()
        {
            if (BufferReadIndex == BufferFillIndex)
            {                
                return BufferInt32Array[BufferReadIndex];
            }
            
            T number_return = BufferInt32Array[BufferReadIndex];
            BufferReadIndex++;
            if (BufferReadIndex > (BufferInt32Array.Length - 1))
            {
                BufferReadIndex = 0;
                BufferEndFilled = false;
            }
            BufferFillCount -= 1;
            return number_return;
        }
        /// <summary>
        /// Written. 2023.11.11 17:36. Moscow. Hostel Дельта. <br></br>
        /// Return amount of Int32 available for read.
        /// </summary>
        /// <returns></returns>
        public Int32 ReadInt32Count
        {
            // not needed. 2024.02.05 09:25. Moscow. Workplace
            /*
            get
            {
                Int32 number_return = 0;
                if (BufferEndFilled == true)
                {
                    number_return = BufferInt32Array.Length - (BufferReadIndex - 1);
                    if (BufferFillIndex != 0)
                    {
                        number_return += (BufferFillIndex - 1) + 1;
                    }
                    return number_return;
                }
                if (BufferReadIndex < BufferFillIndex)
                {
                    number_return = BufferFillIndex - BufferReadIndex;
                }
            */
            get
            {
                return BufferFillCount;
            }

        }
    }
}

