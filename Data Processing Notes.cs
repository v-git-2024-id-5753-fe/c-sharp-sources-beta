/*


2023.03.01.
Notes about data processing. 



8. 2023.03.01. Data processing.
8.1. Raw data.
8.2. To string array of according dimension
8.3. Select required rows and columns and save to file.
8.4. Then this file into string array and convert into required format
8.5. Apply processing - average of rows, columns, array.
8.6. Save result into File. This file will become raw data for other function/programm.


7. 2023.03.01. Core data format.
There may be need in different conversions of arrays: UInt32 to int, Int32 to Double, Converstion Int32 to 4 times larger byte array
and it all can be done if there is conversion from certain data format. In this notes it is string format.
Any format can be converted to string array and from string array to required data format
There are a lot of functions exists for conversion from string and conversion to string

2024.03.07. Detailed.
float[] -> String[] -> File .txt. 
float[] <- String[] <-

Int32[] -> String[] -> File .txt. 
Int32[] <- String[] <-

float[][] -> String[][] -> File .txt with delimer. 
float[][] <- String[][] <-

Int32[][] -> String[][] -> File .txt with delimer. 
Int32[][] <- String[][] <-

delimer is "\t" (Tab), " " (Space), ";" (Semicolon)


6. 2023.03.01. Data storage. Recommended text format. Binary format may contain symbols (see 4.) and
may result in trouble.
Functions are written to work with text files, strings and therefore usage of such functions may result
that part of data will not be processed - the function will take 10-20 first bytes and there is '\0' (byte with zero value) 
and '\0' is the end of string and function will not work properly.
About bytes: It can be stored as text in HEX - 0F 1F 3A 4B to avoid meeting symbols (see 4.).
Be notified: Add zero '0' not '\0' if byte is 16 and lower because text is F, A, 8 and not 0F,0A,08 


5. 2023.03.01. Functions sorting. Path to functions like Window register. The amount of function can become 
large enough to keep them not sorted. Good solution to have path to functions like Windows register.
Examples:
note that extracting from Start, End is used more often and therefore there are functions for it.
ArrayInt32.Extract.FromIndex.ToIndex
ArrayInt32.Extract.FromIndex.Length
ArrayInt32.Extract.FromStart.ToIndex
ArrayInt32.Extract.FromStart.Length
ArrayInt32.Extract.FromEnd.ToIndex
ArrayInt32.Extract.FromEnd.Length


4. 2023.03.01. Symbols.
4.1. "\r\n", '\0'. Avoid to use it in data processing. These are widely spreaded in data transfer, data storage.
There may have been trouble because of it.
4.2. ';', ':', ' ' - delimers, separators between numbers. Avoid to use it in data processing.
These are widely spreaded in data transfer, data storage. There may have been trouble because of it.
4.2. ',' and '.' for double number is used interchangebly. Function to replace '.' to ',' and ',' to '.' will be usefull.


3. 2023.03.01. Data Input. Data source. It can be from any source and therefore in any format but
the data has structure and with the help of array function
the required columns, rows from that data can be taken and saved to file.

Example:
In source 1 there is need to take columns 1,3,4 and save to file. 
In source 2 there is need to take columns 5,6 and save to file.

There is needed to have such functions to extract the columns 
so that any source of data is ok. 

Additionally there may be needed to write column 3,4 first and then column 1 -
rearrange columns order.


2. 2023.03.01. Programm. 
2.1. 1st way. File in and File out. 
File out to save result.
File in to load data.
That allows to write small programms that can be combined in chain to process data.
The .exe of each programm can be used. There is need of main programm 
that will execute these programms in the needed sequence


2.1. 2nd. Array in and Array out in any function.
The programm that contains many functions can become too large and too difficult to use.


1. 2023.03.01. Array functions. 
1.1. Take row from array to process and then via cycle row by row.
1.2. Take range of rows to save it to file.
1.3. 1.1 and 1.2 for column.
1.4. Array conversion from int, double to string array so it can be saved to file. 
Conversion back from strings to Int32 array, double array.
1.5. Array write to file. Function to write array to file using delimer between numbers and 
function to read file to array.


 */