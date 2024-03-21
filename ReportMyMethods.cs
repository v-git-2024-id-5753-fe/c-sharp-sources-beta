using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Commercial use (license)
// Please study about commercial use of the code that is publicly available 

namespace ReportFunctionsNamespace
{
    static class ReportFunctions
    {
        /// <summary>
        /// PrInt32 info about error in console.
        /// 2023.01.01 - 2023.03.01. 10 - 15 o'clock. written.
        /// 2023.01.01 - 2023.03.01. 10 - 15 o'clock. tested. works.
        /// </summary>
        /// <param name="error_message"></param>
        static public void ReportError(string error_message = "", StackTrace external_trace = null)
        {
            StackTrace trace = null;
            if (external_trace != null)
            {
                trace = external_trace;
            }
            else
            {
                trace = new StackTrace(true);
            }
            string filename = trace.GetFrame(1).GetFileName();
            string method_name = trace.GetFrame(1).GetMethod().Name;
            string line_num = trace.GetFrame(1).GetFileLineNumber().ToString();
            Console.WriteLine("___ report about error ___");
            Console.WriteLine("Function" + " " + method_name);
            Console.Write("Error: ");
            Console.WriteLine(error_message);
            Console.WriteLine("File: " + filename);
            Console.WriteLine("Line number" + " " + line_num);
            Console.WriteLine("___ end of report ___");
        }
        static public void ReportError(Int32 index_error)
        {
            StackTrace trace = new StackTrace(true);
            string filename = trace.GetFrame(1).GetFileName();
            string method_name = trace.GetFrame(1).GetMethod().Name;
            string line_num = trace.GetFrame(1).GetFileLineNumber().ToString();
            Console.WriteLine("Failure at " + index_error.ToString());
            Console.WriteLine("Function: " + method_name);
            Console.WriteLine("File: " + filename);
            Console.WriteLine("Line number: " + line_num);
        }
        static public void ReportError(Type type_in)
        {
            StackTrace trace = new StackTrace(true);
            string filename = trace.GetFrame(1).GetFileName();
            string method_name = trace.GetFrame(1).GetMethod().Name;
            string line_num = trace.GetFrame(1).GetFileLineNumber().ToString();
            Console.WriteLine("Failure: Type is wrong. Type is " + type_in.Name);
            Console.WriteLine("Function: " + method_name);
            Console.WriteLine("File: " + filename);
            Console.WriteLine("Line number: " + line_num);
        }
        static public void ReportError(ErrorMessage msg_in)
        {
            StackTrace trace = new StackTrace(true);
            string filename = trace.GetFrame(1).GetFileName();
            string method_name = trace.GetFrame(1).GetMethod().Name;
            string line_num = trace.GetFrame(1).GetFileLineNumber().ToString();
            ReportError(msg_in.ToString(), trace);
            // 2023.08.29 14:05. modified
            // Console.WriteLine("Failure: " + msg_in.ToString());
            // Console.WriteLine("Function: " + method_name);
            // Console.WriteLine("File: " + filename);
            // Console.WriteLine("Line number: " + line_num);
        }
        public enum ErrorType
        {
            ArrayType,
        }
        public enum ErrorMessage
        {
            Indexes,
            LengthDifferent,
            Length_is_0,
            Length_is_exceeded,
            Index_is_wrong,
            Length_is_Wrong,
            Number_Is_Less_Than_Zero
        }
        static public void ReportError(Int32 index_error_x, Int32 index_error_y, ErrorMessage message)
        {
            StackTrace trace = new StackTrace(true);
            string filename = trace.GetFrame(1).GetFileName();
            string method_name = trace.GetFrame(1).GetMethod().Name;
            string line_num = trace.GetFrame(1).GetFileLineNumber().ToString();
            if (message == ErrorMessage.Indexes)
            {
                Console.WriteLine("Failure at [" + index_error_x.ToString() + "," + index_error_y.ToString() + "]");
                Console.WriteLine("Function: " + method_name);
                Console.WriteLine("File: " + filename);
                Console.WriteLine("Line number: " + line_num);
            }
            if (message == ErrorMessage.LengthDifferent)
            {
                Console.WriteLine("Failure");
                Console.WriteLine("Array 1 length " + index_error_x.ToString() + "\r\n" +
                    "Array 2 length " + index_error_y.ToString());
                Console.WriteLine("Function: " + method_name);
                Console.WriteLine("File: " + filename);
                Console.WriteLine("Line number: " + line_num);
            }
        }
        static public void ReportError(Int32 index_error_x, Int32 index_error_y)
        {
            StackTrace trace = new StackTrace(true);
            string filename = trace.GetFrame(1).GetFileName();
            string method_name = trace.GetFrame(1).GetMethod().Name;
            string line_num = trace.GetFrame(1).GetFileLineNumber().ToString();
            Console.WriteLine("Failure at [" + index_error_x.ToString() + "," + index_error_y.ToString() + "]");
            Console.WriteLine("Function: " + method_name);
            Console.WriteLine("File: " + filename);
            Console.WriteLine("Line number: " + line_num);
        }
        public enum AttentionMessage
        {
            ArrayZeroLength,
            StringZeroLength,
            ArrayMaxLength,
            NumberIsZero,
            WrongPath,
            FileDoesNotExist
        }
        static public void ReportAttention(AttentionMessage select_data_type)
        {
            StackTrace trace = new StackTrace(true);
            string filename = trace.GetFrame(1).GetFileName();
            string method_name = trace.GetFrame(1).GetMethod().Name;
            string line_num = trace.GetFrame(1).GetFileLineNumber().ToString();
            if (select_data_type == AttentionMessage.ArrayZeroLength)
            {
                Console.WriteLine("Attention! Array length is 0");
            }
            if (select_data_type == AttentionMessage.StringZeroLength)
            {
                Console.WriteLine("Attention! String length is 0");
            }
            if (select_data_type == AttentionMessage.NumberIsZero)
            {
                Console.WriteLine("Attention! Number is 0 and it should not be so");
            }
            Console.WriteLine("Function: " + method_name);
            Console.WriteLine("File: " + filename);
            Console.WriteLine("Line number: " + line_num);
        }
        static public void ReportAttention(AttentionMessage select_data_type, Int32 number_in)
        {
            StackTrace trace = new StackTrace(true);
            string filename = trace.GetFrame(1).GetFileName();
            string method_name = trace.GetFrame(1).GetMethod().Name;
            string line_num = trace.GetFrame(1).GetFileLineNumber().ToString();
            if (select_data_type == AttentionMessage.ArrayMaxLength)
            {
                Console.WriteLine("Attention! Array is full. Array length is " + number_in.ToString());
            }
            Console.WriteLine("Function: " + method_name);
            Console.WriteLine("File: " + filename);
            Console.WriteLine("Line number: " + line_num);
        }
        public static void ProgressToConsole(string method_name, Stopwatch stopwatch_in)
        {
            Console.WriteLine(method_name.PadRight(50, ' ') + "".PadRight(10) + stopwatch_in.ElapsedMilliseconds.ToString());
        }
        class ProgressToConsole_cls_maybe_needed
        {
            public ProgressToConsole_cls_maybe_needed()
            {
                ActionsCount = 0;
            }
            public ProgressToConsole_cls_maybe_needed(string action_name, Int32 max_actions)
            {
                ActionsCount = 0;
                MethodName = action_name;
                MaxActionsDo = max_actions;
            }
            public string MethodName;
            public Int32 MaxActionsDo;
            Int32 ActionsCount;
            bool is_to_update = true;
            public void Update(bool is_end = false)
            {
                if (is_to_update == false)
                {
                    return;
                }
                if (is_end == true)
                {
                    is_to_update = false;
                    if (ActionsCount <= MaxActionsDo)
                    {
                        Console.Write("\r\n");
                    }
                    return;
                }
                if (ActionsCount <= MaxActionsDo)
                {
                    ActionsCount++;
                    Console.Write("\r");
                    Console.Write(MethodName + " completed " + ActionsCount.ToString() + " from " + MaxActionsDo.ToString());
                    if (ActionsCount == MaxActionsDo)
                    {
                        is_to_update = false;
                        Console.Write("\r\n");
                    }
                }
            }
        }
    }
}
