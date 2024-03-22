using ArrayFunctionsNamespace;
using ReportFunctionsNamespace;
using System;
using System.IO.Ports;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;

// Commercial use (license)
// Please study about commercial use of the code that is publicly available 

namespace MyUSBFunctionsNamespace
{
    /*
    1. 2023.09.03 11:16. 5ms send time was used and the console showed correctly so I assume
    the console wrote into buffer what to show and at screen update showed the strings correctly.
    2. 2023.09.03 11:43. performance check.
    2.1. amount of messages was 2 times less than frame rate at speed 5-10 ms. 
    rx was on average in the middle of the frame.
    2.2. with send repeat time 100ms the amount of messages was 7-8.
    75Hz is 13.3ms per frame
    60Hz is 16.7ms per frame
    the main delay is from waiting to send and not from refresh rate therefore amount of rx was close
    to 10 (1000ms/100ms)
    */
    /// <summary>
    /// 2023.09.03 11:06. written. <br></br>
    /// 2023.09.03 11:06. tested. receive to console. <br></br>
    /// 2023.09.03 11:07. tested. receive until delay.
    /// <code>
    /// 2023.09.03 11:10. <br></br>
    /// important! time less than 50ms is close to refresh rate of screen (15-20ms) <br></br>
    /// function (that shows information on the screen) will be waiting until update 
    /// </code>
    /// </summary>
    public class USB_as_VCP
    {
        SerialPort USBPort = null;
        public delegate void ReceiveCallbackFunction();
        public ReceiveCallbackFunction ReceiveCallback;
        Form Form_In_Use;
        /// <summary>
        /// Requires: <br></br>
        /// Portname. <br></br>
        /// Speed. <br></br>
        /// Form. Form is required for proper function work because parallel thread in use during work. <br></br>
        /// Parallel thread does not have permission to do modification of variables that were created by main thread <br></br>
        /// 
        /// </summary>
        /// <param name="port_name"></param>
        /// <param name="speed_in"></param>
        /// <param name="form_called_from"></param>
        public USB_as_VCP(string port_name, Int32 speed_in, Form form_called_from)
        {
            Form_In_Use = form_called_from;
            USBPort = new SerialPort();
            USBPort.BaudRate = speed_in;
            USBPort.PortName = port_name;
            USBPort.Parity = Parity.None;
            USBPort.StopBits = StopBits.One;
            USBPort.RtsEnable = false;
            USBPort.DtrEnable = false;
            USB_Timer_rx_delay.Tick += USB_Timer_rx_delay_Tick;
            USB_Timer_send_period.Tick += USB_Timer_send_period_Tick;
            // 2023.09.03 11:21. check performance. tested. works.
            /*
            rx_timer_performance.Interval = 1000;
            rx_timer_performance.Tick += Rx_timer_performance_Tick;
            */
        }
        
        /// <summary>
        /// Written. 2024.02.01 12:27. Moscow. Workplace.
        /// </summary>
        public string PortName
        {
            get
            {
                return USBPort.PortName;
            }
        }
        
        
        
        
        /// <summary>
        /// Written. 2023.12.21 10:44. Workplace <br></br>
        /// 2023.12.21 10:54. Workplace. Did not decrease memory usage if new instance is created. <br></br>
        /// It may be because something else was connected to the instance
        /// </summary>
        public void DisposeInstance()
        {
            USBPort.Dispose();
        }
        /// <summary>
        /// Written. 2023.11.30 17:47. Moscow. Workplace. 
        /// </summary>
        /// <param name="form_called_from"></param>
        public USB_as_VCP(Form form_called_from)
        {
            Form_In_Use = form_called_from;
            USBPort = new SerialPort();
            USBPort.Parity = Parity.None;
            USBPort.StopBits = StopBits.One;
            USBPort.RtsEnable = false;
            USBPort.DtrEnable = false;
            USB_Timer_rx_delay.Tick += USB_Timer_rx_delay_Tick;
            USB_Timer_send_period.Tick += USB_Timer_send_period_Tick;
            // 2023.09.03 11:21. check performance. tested. works.
            /*
            rx_timer_performance.Interval = 1000;
            rx_timer_performance.Tick += Rx_timer_performance_Tick;
            */
        }
        private void Rx_timer_performance_Tick(object sender, EventArgs e)
        {
            Console.Clear();
            Console.WriteLine(rx_performance_total_count);
            rx_performance_total_count = 0;
        }
        private void USB_Timer_send_period_Tick(object sender, EventArgs e)
        {
            USB_Timer_send_period.Stop();
            Send(USB_send_period_bytes);
            USB_Timer_send_period.Start();
        }
        private void USB_Timer_rx_delay_Tick(object sender, EventArgs e)
        {
            USB_Timer_rx_delay.Stop();
            USB_Timer_rx_finished = true;
            Is_Bytes_Received = true;
            ReceiveCallback.Invoke();
        }
        void USB_Timer_Start(object sender, EventArgs e)
        {
            USB_Timer_rx_delay.Start();
        }
        void USB_Timer_Stop(object sender, EventArgs e)
        {
            USB_Timer_rx_delay.Stop();
        }
        // 2023.09.03 11:19. performance check variables
        Int32 rx_performance_total_count = 0;
        /// <summary>
        /// 2023.09.03 11:41. for performance check. tested. works. 
        /// </summary>
        public Timer rx_timer_performance = new Timer();
        private void USBPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (USB_Reception_Type == USB_Receive_Methods_List.NoReception)
            {
                Int32 rx_bytes_number = USBPort.BytesToRead;
                byte no_write_byte = 0;
                for (Int32 i = 0; i < rx_bytes_number; i++)
                {
                    no_write_byte = (byte)USBPort.ReadByte();
                }
            }
            // 2023.09.28 14:25. added.
            if (USB_Reception_Type == USB_Receive_Methods_List.ReceiveNumberOfBytes)
            {
                Int32 rx_bytes_number = USBPort.BytesToRead;
                byte[] rx_bytes = new byte[rx_bytes_number];
                for (Int32 i = 0; i < rx_bytes_number; i++)
                {
                    _recieve_bytes_count++;
                    _usb_rx_buffer[_recieve_bytes_size - 1] = (byte)USBPort.ReadByte();
                }
                if (_recieve_bytes_count >= _recieve_bytes_size)
                {
                    Form_In_Use.Invoke(RecieveNumberOfBytesCallback);
                    _recieve_bytes_count = 0;
                }
            }
            if (USB_Reception_Type == USB_Receive_Methods_List.ReceiveToConsole)
            {
                Int32 rx_bytes_number = USBPort.BytesToRead;
                byte[] rx_bytes = new byte[rx_bytes_number];
                /*
                2023.09.03 10:38. that code was showing mixed letter in console
                Console.write I assume takes 1 frame for update and with transfer 10 letter in 100ms
                with 15-20ms update time there were in certain moment two function updating console.
                //for (Int32 i = 0; i < rx_bytes_number; i++)
                //{
                //    rx_bytes[i] = (byte)USBPort.ReadByte();
                //}
                */
                rx_bytes_number = USBPort.Read(rx_bytes, 0, rx_bytes.Length);
                if (RecieveToConsole_OutputType == ConsoleOutput.Char)
                {
                    string str_to_console = "";
                    for (Int32 i = 0; i < rx_bytes_number; i++)
                    {
                        str_to_console += ((char)rx_bytes[i]);
                    }
                    Console.Write(str_to_console);
                    // 2023.09.03 11:40. for performance check
                    /*
                    if (str_to_console.Contains("\n") == true)
                    {
                        rx_performance_total_count += 1;
                    }
                    */
                }
                if (RecieveToConsole_OutputType == ConsoleOutput.Decimal)
                {
                    string str_to_console = "";
                    for (Int32 i = 0; i < rx_bytes_number; i++)
                    {
                        str_to_console += System.Convert.ToString(rx_bytes[i], 10) + " ";
                    }
                    Console.Write(str_to_console);
                }
                if (RecieveToConsole_OutputType == ConsoleOutput.HEX)
                {
                    string str_to_console = "";
                    for (Int32 i = 0; i < rx_bytes_number; i++)
                    {
                        str_to_console += System.Convert.ToString(rx_bytes[i], 16).PadLeft(2, '0');
                    }
                    Console.Write(str_to_console);
                }
            }
            if (USB_Reception_Type == USB_Receive_Methods_List.ReceiveUntildDelay)
            {
                Form_In_Use.Invoke(new EventHandler(USB_Timer_Stop));
                Int32 rx_bytes_number = USBPort.BytesToRead;
                if (USB_Timer_rx_finished == true)
                {
                    byte no_write_byte = 0;
                    for (Int32 i = 0; i < rx_bytes_number; i++)
                    {
                        no_write_byte = (byte)USBPort.ReadByte();
                    }
                    return;
                }
                bool buffer_is_full = false;
                for (Int32 i = 0; i < rx_bytes_number; i++)
                {
                    if (_usb_rx_buffer_count < _usb_rx_buffer_size)
                    {
                        _usb_rx_buffer_count += 1;
                        _usb_rx_buffer[_usb_rx_buffer_count - 1] = (byte)USBPort.ReadByte();
                    }
                    else
                    {
                        // note. implementation.
                        buffer_is_full = true;
                    }
                }
                if (buffer_is_full == true)
                {
                    Is_Bytes_Received = true;
                    ReportFunctions.ReportAttention(ReportFunctions.AttentionMessage.ArrayMaxLength, _usb_rx_buffer_count);
                }
                Form_In_Use.Invoke(new EventHandler(USB_Timer_Start));
            }
            if (USB_Reception_Type == USB_Receive_Methods_List.ReceiveByLengthInt8)
            {
                TimerRecieveByLength_DelayLastByte.Stop();
                Int32 rx_bytes_number = USBPort.BytesToRead;
                for (Int32 i = 0; i < rx_bytes_number; i++)
                {
                    _usb_rx_buffer_count += 1;
                    _usb_rx_buffer[_usb_rx_buffer_count - 1] = (byte)USBPort.ReadByte();
                    if (RecieveByLength_LengthIsRead == false)
                    {
                        if (_usb_rx_buffer_count >= _usb_rx_buffer_size)
                        {
                            // note on >=. == is enough and it should work. 2023.12.14 10:49. Workplace.
                            Int32 NewSize = _usb_rx_buffer[_usb_rx_buffer_count - 1];
                            BufferRxIncrease = NewSize;
                            RecieveByLength_LengthIsRead = true;
                            RecieveByLength_Length = NewSize;
                        }
                    }
                    else
                    {
                        RecieveByLength_Count += 1;
                        if (RecieveByLength_Count >= RecieveByLength_Length)
                        {
                            USB_Reception_Type = USB_Receive_Methods_List.NoReception;
                            Console.WriteLine(nameof(RecieveByLengthInt16) + " done. " + RecieveByLength_Length.ToString() + " bytes were received");
                            RecieveByLengthCallback(_usb_rx_buffer);
                        }
                    }
                }
                TimerRecieveByLength_DelayLastByte.Start();
            }
            // Written. 2023.12.14 10:53. Workplace.
            // Works. 2023.12.14 14:08. Workplace.
            if (USB_Reception_Type == USB_Receive_Methods_List.ReceiveByLengthInt16)
            {
                Form_In_Use.Invoke(new EventHandler(TimerStoptRecieveByLength));
                Int32 rx_bytes_number = USBPort.BytesToRead;
                for (Int32 i = 0; i < rx_bytes_number; i++)
                {
                    _usb_rx_buffer_count += 1;
                    _usb_rx_buffer[_usb_rx_buffer_count - 1] = (byte)USBPort.ReadByte();
                    // 2024.01.20 17:16. Moscow. Hostel.
                    // Obtaining Length is done by setting buffer size equal 3 bytes
                    // for cmd and length and once it is filled, it means there is
                    // length bytes are received.
                    if (RecieveByLength_LengthIsRead == false)
                    {
                        if (_usb_rx_buffer_count >= _usb_rx_buffer_size)
                        {
                            // note on >=. == is enough and it should work. 2023.12.14 10:49. Workplace.
                            Int32 NewSize = (_usb_rx_buffer[_usb_rx_buffer_count - 1 - 1] << 8);
                            NewSize |= (_usb_rx_buffer[_usb_rx_buffer_count - 1] << 0);
                            BufferRxIncrease = NewSize;
                            RecieveByLength_LengthIsRead = true;
                            RecieveByLength_Length = NewSize;
                        }
                    }
                    else
                    {
                        RecieveByLength_Count += 1;
                        if (RecieveByLength_Count >= RecieveByLength_Length)
                        {
                            USB_Reception_Type = USB_Receive_Methods_List.NoReception;
                            Console.WriteLine(nameof(RecieveByLengthInt16) + " done. " + RecieveByLength_Length.ToString() + " bytes were received");
                            Form_In_Use.Invoke(RecieveByLengthCallback, _usb_rx_buffer);
                            // note that it is secondary threat and it does not have access to the main threat. 2023.12.14 14:26. Workplace.
                            TimerRecieveByLength_DelayLastByte.Tick -= TimerRecieveByLength_DelayLastByte_Tick;
                            TimerRecieveByLength_NoByteDelay.Tick -= TimerRecieveByLength_NoByteDelay_Tick;
                            // skip if there are bytes. 2023.12.14 13:54. Workplace
                            byte byte_read = 0;
                            for (Int32 j = i + 1; j < rx_bytes_number; j++)
                            {
                                byte_read = (byte)USBPort.ReadByte();
                            }
                            return;
                        }
                    }
                }
                Form_In_Use.Invoke(new EventHandler(TimerStartRecieveByLength));
            }



           
            // Written. 2024.01.20 17:09. Moscow. Hostel.
            if (USB_Reception_Type == USB_Receive_Methods_List.ReceiveStrings)
            {
                Form_In_Use.Invoke(new EventHandler(TimerReceiveStrings_LastByteDelay_Stop));
                Int32 rx_bytes_number = USBPort.BytesToRead;
                for (Int32 i = 0; i < rx_bytes_number; i++)
                {
                    ReceiveStringsArray[ReceiveStringsArrIndex] += ((char)((byte)USBPort.ReadByte())).ToString();
                    if (ReceiveStringsArray[ReceiveStringsArrIndex].Contains("\r\n") == true)
                    {
                        // Certain amount of string to receive. 2024.01.30 10:39. Moscow, Workplace
                        if (ReceiveStringsArrIndex >= (ReceiveStringsCount - 1))
                        {
                            USB_Reception_Type = USB_Receive_Methods_List.NoReception;
                            Form_In_Use.Invoke(new EventHandler(ReceiveStringsCompleted));
                            return;
                        }
                        
                        
                        ReceiveStringsArrIndex += 1;
                        ReceiveStringsArray[ReceiveStringsArrIndex] = "";
                        if (ReceiveStringsArrIndex > (ReceiveStringsArray.Length - (ReceiveStringsArray.Length / 4)))
                        {
                            Array.Resize(ref ReceiveStringsArray, ReceiveStringsArray.Length * 2);
                        }
                    }
                }


                Form_In_Use.Invoke(new EventHandler(TimerReceiveStrings_LastByteDelay_Start));

            }








        }

        

        /// <summary>
        /// Written. 2023.11.30 17:52. Moscow. Workplace. 
        /// </summary>
        public bool IsOpen
        {
            get
            {
                return USBPort.IsOpen;
            }
        }
        /// <summary>
        /// Tested. Works. 2023.12.12 16:52. Workplace. <br></br>
        /// <br></br>
        /// 2024.01.23 15:50. Moscow. Workplace. <br></br>
        /// Important. Reception starts as soon as Open() is called.
        /// It was tested with 5 seconds delay between Open() with clear buffer
        /// and adding += DataReceived.
        /// </summary>
        public bool Open(bool report_error = true)
        {
            bool result_out = false;
            try
            {
                
                USBPort.Open();            
                USBPort.DiscardInBuffer();
                USBPort.DiscardOutBuffer();               
                USBPort.DataReceived += USBPort_DataReceived;
                result_out = true;
            }
            catch (Exception e)
            {
                if (report_error == true)
                {
                    ReportFunctions.ReportError("Open VCP error.\r\n" + e.Message);
                }
            }
            return result_out;
        }

        byte[] _usb_rx_buffer = new byte[128];
        Int32 _usb_rx_buffer_count = 0;
        Int32 _usb_rx_buffer_size = 128;

        public byte[] ReceivedBytes
        {
            get
            {
                byte[] bytes_out = new byte[_usb_rx_buffer_count];
                Array.Copy(_usb_rx_buffer, bytes_out, _usb_rx_buffer_count);
                return bytes_out;
            }
        }
        /// <summary>
        /// Written. 2023.12.14 10:43. Workplace
        /// </summary>
        public Int32 BufferRxIncrease
        {
            set
            {
                _usb_rx_buffer_size += value;
                Array.Resize(ref _usb_rx_buffer, _usb_rx_buffer_size);
            }
        }
        public Int32 BufferRxSize
        {
            get
            {
                return _usb_rx_buffer_size;
            }
            set
            {
                _usb_rx_buffer = new byte[value];
                _usb_rx_buffer_size = value;
            }
        }

        /// <summary>
        /// Buffer is needed to keep bytes which were declared in a function - local variable. <br></br>
        /// The bytes are lost after exit from function and not all bytes may be transfered. <br></br>
        /// <br></br>
        /// Written. 2024.02.01 12:05. Moscow. Workplace. <br></br>
        /// </summary>
        public Int32 BufferTxSize
        {
            get
            {
                return _usb_tx_buffer_size;
            }
            set
            {
                _usb_tx_buffer = new byte[value];
                _usb_tx_buffer_size = value;
            }
        }
        
        byte[] _usb_tx_buffer = new byte[128];
        Int32 _usb_tx_buffer_count = 0;
        Int32 _usb_tx_buffer_size = 128;
       

        public bool Is_Bytes_Received = false;
        Timer USB_Timer_rx_delay = new Timer();
        bool USB_Timer_rx_finished = true;
        enum USB_Receive_Methods_List
        {
            NoReception,
            ReceiveUntildDelay,
            ReceiveToConsole,
            ReceiveNumberOfBytes,
            ReceiveByLengthInt16,
            ReceiveByLengthInt8,
            ReceiveStrings,
            Recieve_MODBUS_RTU
        }
        USB_Receive_Methods_List USB_Reception_Type = USB_Receive_Methods_List.NoReception;
        //bool USB_Reception_1_byte_rx = false;
        Int32 _recieve_bytes_count = 0;
        Int32 _recieve_bytes_size = 0;
        public delegate void RecieveNumberOfBytesDelegate(byte[] arr_in);
        public RecieveNumberOfBytesDelegate RecieveNumberOfBytesCallback;
        public void RecieveNumberOfBytes(Int32 number_in, RecieveNumberOfBytesDelegate callback_in)
        {
            _recieve_bytes_size = number_in;
            _recieve_bytes_count = 0;
            USB_Reception_Type = USB_Receive_Methods_List.ReceiveNumberOfBytes;
            RecieveNumberOfBytesCallback += callback_in;
        }
        public void RecieveNumberOfBytes(Int32 number_in)
        {
            RecieveNumberOfBytes(number_in, null);
        }
        public void RecieveUntilDelay(Int32 delay_in = 200)
        {
            // 2023.08.23 13:45. delay 5 ms is not good. datareceive from serial port
            // may have delay greater than 5 ms. 15 ms is min. 
            // 50 ms is ok.
            if (USBPort.IsOpen == false)
            {
                Open();
            }
            USB_Timer_rx_delay.Interval = delay_in;
            USB_Timer_rx_finished = false;
            USB_Reception_Type = USB_Receive_Methods_List.ReceiveUntildDelay;
            Is_Bytes_Received = false;
            _usb_rx_buffer_count = 0;
            //  USB_Reception_1_byte_rx = false;
        }
        Timer USB_Timer_send_period = new Timer();
        byte[] USB_send_period_bytes = new byte[0];
        /// <summary>
        /// 2023.09.02 20:22. written.
        /// 2023.09.02 20:22. tested. works.
        /// </summary>
        /// <param name="data_in"></param>
        /// <param name="period"></param>
        public void SendPeriod(byte[] data_in, Int32 period)
        {
            USB_Timer_send_period.Interval = period;
            USB_send_period_bytes = ArrayFunctions.ByteArray.Copy(data_in);
            Send(USB_send_period_bytes);
            USB_Timer_send_period.Start();
        }
        /// <summary>
        /// 2023.09.02 20:22. written.
        /// 2023.09.02 21:54. tested. works.
        /// </summary>
        /// <param name="string_in"></param>
        /// <param name="period"></param>
        public void SendPeriod(string string_in, Int32 period)
        {
            // 2023.09.09 12:07. importance 3. Encoding is selected according to UTF of the
            // string and not which encoding bytes encoding.
            byte[] utf7_bytes = Encoding.UTF8.GetBytes(string_in);
            SendPeriod(utf7_bytes, period);
        }
        /// <summary>
        /// 2023.09.02 21:54. written.
        /// 2023.09.02 21:54. tested. works.
        /// 2023.09.02 21:55. note. implementation in the function.
        /// </summary>
        /// 
        /// 
        /// <param name="data_in"></param>
        /// <returns></returns>
        public bool Send(byte[] data_in, bool msg_to_console = false)
        {
            bool for_return = false;
            try
            {
                // 2023.09.02 21:16. skips every 2nd byte. I assume writes 2 bytes chars.
                // USBPort.BaseStream.BeginWrite. skips 2nd byte.
                // USBPort.BaseStream.WriteAsync. skips every 2nd byte.
                // 2023.09.02 21:56. works. tested.
                /*
                for (Int32 i = 0; i < data_in.Length; i++)
                {
                    USBPort.BaseStream.WriteByte(data_in[i]);
                } 
                 */

                // Added. 2024.02.01 12:09. Moscow. Workplace.
                if (BufferTxSize < data_in.Length)
                {
                    BufferTxSize = data_in.Length * 2;
                }
                Array.Copy(data_in, _usb_tx_buffer, data_in.Length);
                Int32 data_length = data_in.Length;

                for (Int32 i = 0; i < data_length; i++)
                {
                    USBPort.BaseStream.WriteByte(_usb_tx_buffer[i]);
                }
                for_return = true;
                if (msg_to_console == true)
                {
                    //Console.WriteLine();
                    Console.Write(DateTime.Now.ToString("HH:mm:ss") + " " + "Sent in " + USBPort.PortName + ":");
                    for (Int32 i = 0; i < data_length; i++)
                    {
                        Console.Write(Convert.ToString(_usb_tx_buffer[i], 16).PadLeft(2, '0') + " ");
                    }
                    Console.WriteLine();
                }
            }
            catch (Exception e)
            {
                ReportFunctions.ReportError("VCP. Send error.\r\n" + e.Message);
            }
            return for_return;
        }
        public bool Send(string string_in)
        {
            bool for_return = false;
            try
            {
                byte[] utf8_bytes = Encoding.UTF8.GetBytes(string_in);
                Send(utf8_bytes);
                for_return = true;
            }
            catch (Exception e)
            {
                ReportFunctions.ReportError("VCP. Send error.\r\n" + e.Message);
            }
            return for_return;
        }
        /// <summary>
        /// Tested. Works. 2023.12.12 16:53. Workplace.
        /// </summary>
        public bool Close(bool show_error = true)
        {
            bool result_out = false;
            try
            {
                USBPort.DataReceived -= USBPort_DataReceived;
                Thread.Sleep(50);
                USBPort.DiscardInBuffer();
                USBPort.DiscardOutBuffer();
                USBPort.Close();
                result_out = true;
            }
            catch (Exception e)
            {
                if (show_error == true)
                {
                    ReportFunctions.ReportError("Close VCP error.\r\n" + e.Message);
                }
            }
            return result_out;
        }
        enum ConsoleOutput
        {
            Char,
            HEX,
            Decimal
        }
        ConsoleOutput RecieveToConsole_OutputType = ConsoleOutput.Char;
        public void RecieveToConsoleDecimal()
        {
            USB_Reception_Type = USB_Receive_Methods_List.ReceiveToConsole;
            RecieveToConsole_OutputType = ConsoleOutput.Decimal;
        }
        public void RecieveToConsoleChar()
        {
            USB_Reception_Type = USB_Receive_Methods_List.ReceiveToConsole;
            RecieveToConsole_OutputType = ConsoleOutput.Char;
        }
        public void RecieveToConsoleHEX()
        {
            USB_Reception_Type = USB_Receive_Methods_List.ReceiveToConsole;
            RecieveToConsole_OutputType = ConsoleOutput.HEX;
        }
        Int32 RecieveByLength_Length = 0;
        Int32 RecieveByLength_Count = 0;
        Int32 RecieveByLength_1st_byte = 0;
        bool RecieveByLength_LengthIsRead = false;
        bool RecieveByLength_ReceptionError = false;
        Timer TimerRecieveByLength_DelayLastByte = new Timer();
        Timer TimerRecieveByLength_NoByteDelay = new Timer();
        public delegate void RecieveByLengthDelegate(byte[] arr_in);
        public RecieveByLengthDelegate RecieveByLengthCallback;
        public void RecieveByLengthInt16(Int32 length_first_byte, Int32 delay_last_byte = 500)
        {
            USB_Reception_Type = USB_Receive_Methods_List.ReceiveByLengthInt16;
            RecieveByLength_1st_byte = length_first_byte;
            BufferRxSize = (length_first_byte + 1) + 1;
            // +1 because 1st byte of LEN already counted. 2023.12.14 10:45. Workplace. 
            TimerRecieveByLength_DelayLastByte.Interval = delay_last_byte;
            TimerRecieveByLength_DelayLastByte.Tick += TimerRecieveByLength_DelayLastByte_Tick;
            TimerRecieveByLength_NoByteDelay.Interval += delay_last_byte * 10;
            TimerRecieveByLength_NoByteDelay.Tick += TimerRecieveByLength_NoByteDelay_Tick;
            _usb_rx_buffer_count = 0;
            RecieveByLength_Count = 0;
            RecieveByLength_LengthIsRead = false;
            RecieveByLength_ReceptionError = false;
        }



        Timer TimerReceiveStrings_LastByteDelay = new Timer();
        string[] ReceiveStringsArray = new string[128];
        Int32 ReceiveStringsArrIndex = 0;
        Int32 ReceiveStringsCount = -1;
        // 2024.01.23 14:19. Moscow. Workplace. 
        // The function is called from Timer.Tick and it is from main thread.
        // There is no need to use Invoke.
        public delegate void ReceiveStringDelegate(string[] arr_in);
        public ReceiveStringDelegate ReceiveStringCallback = null;
        /// <summary>
        /// Written. 2024.01.20 16:47. Moscow. Hostel <br></br>
        /// not tested <br></br>
        /// Note. Delay from start is not implemented. 2024.01.30 11:05. Moscow. Workplace.
        /// </summary>
        /// <param name="strings_count"></param>
        /// <param name="delay_last_byte"></param>
        public void ReceiveStringsStart(Int32 strings_count = -1, Int32 delay_last_byte = 500)
        {
            USB_Reception_Type = USB_Receive_Methods_List.ReceiveStrings;
            ReceiveStringsCount = strings_count;
            ReceiveStringsArray[0] = "";
            ReceiveStringsArrIndex = 0;
            TimerReceiveStrings_LastByteDelay.Interval = delay_last_byte;
            TimerReceiveStrings_LastByteDelay.Tick += TimerReceiveStrings_LastByteDelay_Tick;
        }



        /// <summary>
        /// Written. 2024.01.30 11:08. Moscow. Workplace.
        /// not tested
        /// </summary>
        public void ReceiveStringsStop()
        {
            TimerReceiveStrings_LastByteDelay.Stop();
            TimerReceiveStrings_LastByteDelay.Tick -= TimerReceiveStrings_LastByteDelay_Tick;
            USB_Reception_Type = USB_Receive_Methods_List.NoReception;            
        }




        /// <summary>
        /// Callback for receive certain amount of strings <br></br>
        /// Written. 2024.01.30 10:44. Moscow. Workplace.
        /// </summary>
        /// <param name="strings_in"></param>
        private void ReceiveStringsCompleted(object sender, EventArgs e)
        {
            TimerReceiveStrings_LastByteDelay.Stop();
            TimerReceiveStrings_LastByteDelay.Tick -= TimerReceiveStrings_LastByteDelay_Tick;
            USB_Reception_Type = USB_Receive_Methods_List.NoReception;


            // 2024.01.30 10:47. Moscow. Workplace.
            // removing "\r\n"

            for (Int32 i = 0; i <= ReceiveStringsArrIndex; i++)
            {
                ReceiveStringsArray[i] = ReceiveStringsArray[i].Replace("\r\n", "");
            }

            Array.Resize(ref ReceiveStringsArray, ReceiveStringsArrIndex + 1);

            Console.WriteLine(DateTime.Now.ToString("HH:mm:ss") + " " + nameof(ReceiveStringsStart) + ". Reception is completed.\r\n" +            
            ReceiveStringsArray.Length.ToString() + " strings were received.\r\n");
            ReceiveStringCallback(ReceiveStringsArray);
        }



        /// <summary>
        /// Written. 2024.01.20 16:52. Moscow. Hostel 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TimerReceiveStrings_LastByteDelay_Start(object sender, EventArgs e)
        {
            TimerReceiveStrings_LastByteDelay.Start();
        }



        /// <summary>
        /// Written. 2024.01.20 16:54. Moscow. Hostel 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TimerReceiveStrings_LastByteDelay_Stop(object sender, EventArgs e)
        {
            TimerReceiveStrings_LastByteDelay.Stop();
        }




        /// <summary>
        /// Written. 2024.01.20 17:05. Moscow. Hostel.
        /// not tested.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TimerReceiveStrings_LastByteDelay_Tick(object sender, EventArgs e)
        {
            TimerReceiveStrings_LastByteDelay.Stop();
            TimerReceiveStrings_LastByteDelay.Tick -= TimerReceiveStrings_LastByteDelay_Tick;
            USB_Reception_Type = USB_Receive_Methods_List.NoReception;

            // 2024.01.23 15:28. Moscow. Workplace.
            // Checking if the last string is received, not fully received, VCP was waiting for string
            if (ReceiveStringsArray[ReceiveStringsArrIndex].Contains("\r\n") == false)
            {
                ReceiveStringsArrIndex -= 1;
            }


            // 2024.01.23 14:33. Moscow. Workplace.
            // removing "\r\n"

            for (Int32 i = 0; i <= ReceiveStringsArrIndex; i++)
            {
                ReceiveStringsArray[i] = ReceiveStringsArray[i].Replace("\r\n", "");
            }

            Array.Resize(ref ReceiveStringsArray, ReceiveStringsArrIndex + 1);

            if (ReceiveStringsCount == -1)
            {
                Console.WriteLine(nameof(ReceiveStringsStart) + ". Reception is completed.\r\n" +
                TimerReceiveStrings_LastByteDelay.Interval.ToString() + "ms was reached.\r\n" +
                ReceiveStringsArray.Length.ToString() + " strings were received.\r\n");
            }
            else
            {
                Console.WriteLine(nameof(ReceiveStringsStart) + ". Failure in receiving all strings.\r\n" +
                     ReceiveStringsArray.Length.ToString() + "/" + ReceiveStringsCount.ToString() + " strings were received.\r\n");
            }
            ReceiveStringCallback(ReceiveStringsArray);
        }












        /// <summary>
        /// Not finished. 2024.01.20 16:43.
        /// </summary>
        /// <param name="length_first_byte"></param>
        /// <param name="delay_last_byte"></param>
        public void RecieveByLengthInt8(Int32 length_first_byte, Int32 delay_last_byte = 500)
        {
            USB_Reception_Type = USB_Receive_Methods_List.ReceiveByLengthInt8;
            RecieveByLength_1st_byte = length_first_byte;
            BufferRxSize = (length_first_byte + 1);
            TimerRecieveByLength_DelayLastByte.Interval = delay_last_byte;
            TimerRecieveByLength_DelayLastByte.Tick += TimerRecieveByLength_DelayLastByte_Tick;
            TimerRecieveByLength_NoByteDelay.Interval += delay_last_byte * 10;
            TimerRecieveByLength_NoByteDelay.Tick += TimerRecieveByLength_NoByteDelay_Tick;
            _usb_rx_buffer_count = 0;
            RecieveByLength_Count = 0;
            RecieveByLength_LengthIsRead = false;
            RecieveByLength_ReceptionError = false;
        }



        /// <summary>
        /// Receives certain amount of bytes defined by structure of command to be received. <br></br>
        /// Written. 2023.12.27 16:27. Workplace.
        /// </summary>
        /// <param name="cmd_code"></param>
        /// <param name="delay_last_byte"></param>
        public void RecieveByCMD(Int32 cmd_code, Int32 total_bytes, Int32 delay_last_byte = 500)
        {
            // Not completed. 2024.03.22 10:46. Moscow. Workplace.
        }



        /// <summary>
        /// Written. 2024.03.22 11:06. Moscow. Workplace.
        /// For receving 1 reply by MODBUS RTU. For another reply a new instance should be made
        /// </summary>
        class MODBUS_RTU_CMD_Data
        {
            /// <summary>
            /// Slave address.
            /// </summary>
            public byte FromAddress = 0;
            public byte FunctionCode = 0;
            /// <summary>
            /// These are bytes of registers. CRC32 is not included in the bytes.<br></br>
            /// Note that register is 16 bits.
            /// </summary>
            public byte NumberOfBytes = 0;
            public UInt16 CRC16 = 0;
            public byte[] ReceivedBytes = null;
            public uint ReceivedCount = 0;
            public MODBUS_RTU_CMD_Data()
            {
                ReceivedBytes = new byte[3];
            }
            public bool IsPacketReceived = false;
            public bool CreatePacket()
            {
                if (ReceivedCount == 3)
                {
                    return false;
                }                
                byte[] arr_3_bytes = new byte[3];
                Array.Copy(ReceivedBytes, arr_3_bytes, 3);
                // 2024.03.22 11:11. Moscow. Workplace.
                // +3 - byte with address byte, byte with function code, byte with number of bytes.
                // +2 - CRC16 bytes.
                ReceivedBytes = new byte[NumberOfBytes + 3 + 2];
                Array.Copy(arr_3_bytes, ReceivedBytes, 3);
                return true;
            }

            public void LoadByte(byte byte_in)
            {
                ReceivedCount += 1;
                
                // Address from which to receive bytes
                if (ReceivedCount == 1)
                {
                    ReceivedBytes[ReceivedCount - 1] = byte_in;
                    if (byte_in != FromAddress)
                    {                        
                        ReceivedCount = 0;
                    }
                    return;
                }

                // Function code.
                if (ReceivedCount == 2)
                {
                    ReceivedBytes[ReceivedCount - 1] = byte_in;
                    if (byte_in != FunctionCode)
                    {
                        ReceivedCount = 0;
                    }
                    return ;
                }

                // Number of bytes.
                if (ReceivedCount == 3)
                {
                    ReceivedBytes[ReceivedCount - 1] = byte_in;
                    if (CreatePacket() != true)
                    {
                        ReceivedCount = 0;
                    }
                    return;
                }

                // Number of bytes.
                if ((ReceivedCount > 3) &&
                    (ReceivedCount <= (3 + NumberOfBytes)))
                {
                    ReceivedBytes[ReceivedCount - 1] = byte_in;
                    return ;
                }

                // Checksum CRC16
                if ((ReceivedCount > (3 + NumberOfBytes)) &&
                    (ReceivedCount <= (3 + NumberOfBytes + 2)))
                {
                    ReceivedBytes[ReceivedCount - 1] = byte_in;
                    return;
                }
                else
                {
                    IsPacketReceived = true;
                }
            }
        }



        /// <summary>
        /// Written. 2024.03.22 10:51. Moscow. Workplace.
        /// </summary>
        /// <param name="from_address"></param>
        /// <param name="bytes_amount"></param>
        /// <param name="delay_last_byte">1st byte is the trigger to count the delay. Each byte resets the counter</param>
        public void Receive_MODBUS_RTU(byte from_address, uint bytes_amount, Int32 delay_last_byte = 500)
        {
            USB_Reception_Type = USB_Receive_Methods_List.Recieve_MODBUS_RTU;
        }




            private void TimerStartRecieveByLength(object sender, EventArgs e)
        {
            TimerRecieveByLength_DelayLastByte.Start();
        }
        private void TimerStoptRecieveByLength(object sender, EventArgs e)
        {
            TimerRecieveByLength_DelayLastByte.Stop();
        }
        private void TimerRecieveByLength_NoByteDelay_Tick(object sender, EventArgs e)
        {
        }
        private void TimerRecieveByLength_DelayLastByte_Tick(object sender, EventArgs e)
        {
            TimerRecieveByLength_DelayLastByte.Stop();
            TimerRecieveByLength_DelayLastByte.Tick -= TimerRecieveByLength_DelayLastByte_Tick;
            TimerRecieveByLength_NoByteDelay.Tick -= TimerRecieveByLength_NoByteDelay_Tick;
            USB_Reception_Type = USB_Receive_Methods_List.NoReception;
            Console.WriteLine(nameof(RecieveByLengthInt16) + " error. Reception ended because delay " +
                TimerRecieveByLength_DelayLastByte.Interval.ToString() + " ms was reached without receiving all bytes.\r\n" +
                "Number of bytes to receive is " + RecieveByLength_Length.ToString());
        }
    }
}
