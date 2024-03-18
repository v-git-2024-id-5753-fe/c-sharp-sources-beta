using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ImageFunctionsNameSpace;
using ReportFunctionsNamespace;

// Commercial use (license)
// Please study about commercial use of the code that is publicly available 

namespace SqlFunctionsNamespace
{
    class SQLFunctions
    {
        /// <summary>
        /// It does the same as ReduceSizeMDF.
        /// Database instead of MDF in the name
        /// 2023-07-26 11:53
        /// </summary>
        /// <param name="db_filepath"></param>
        /// <param name="db_con"></param>
        static public void ReduceSizeDatabase(string db_filepath, SqlConnection db_con)
        {
            ReduceSizeMDF(db_filepath, db_con);
        }
        static public void ReduceSizeMDF(string db_filepath, SqlConnection db_con)
        {
            // sql query
            //DBCC SHRINKFILE (N'DB_TEMPLATE' , 0)
            // 2023-07-25 14:17 name without .mdf
            // works from visual. does not work in app
            string sql_str = "DBCC SHRINKDATABASE(N'" + db_filepath + "', 0)";
            SqlCommand sqlcmd = new SqlCommand(sql_str, db_con);
            db_con.Open();
            sqlcmd.ExecuteNonQuery();
            db_con.Close();
        }
        static public DataRow DatabaseToDataRowByID(SqlConnection database_con, string table_name, Int32 id)
        {
            // space for code. start
            DataTable table_1_record = new DataTable();
            try // maybe without try
            {
                string[] col_names = DatabaseGetColumnNames(database_con, table_name);
                database_con.Open();
                if (database_con.State == ConnectionState.Open)
                {
                    string sql_cmd = "SELECT * FROM " + table_name + " WHERE ID = " + id.ToString();
                    using (SqlDataAdapter data_reader = new SqlDataAdapter(sql_cmd, database_con.ConnectionString))
                    {
                        data_reader.Fill(table_1_record);
                    }
                    database_con.Close();
                }
                else
                {
                    MyReportFunctions.ReportError();
                }
            }
            catch
            {
                MyReportFunctions.ReportError();
            }
            DataRow for_return = table_1_record.Rows[0];
            return for_return;
        }
        static public DataTable DatabaseToDataTableByID(SqlConnection database_con, string table_name, string[] id_arr, bool show_if_not_found = true, bool show_execution_time = false)
        {
            DataTable table_all_records = DataBaseToDataTableSorted(database_con, table_name, 1);
            DataTable for_return = new DataTable();
            bool[] not_found = new bool[id_arr.Length];
            string[] id_table = DataTableColumnGet(table_all_records, 1);
            Stopwatch time_of_search = new Stopwatch();
            time_of_search.Start();
            for (Int32 rn = 0; rn < id_arr.Length; rn++)
            {
                not_found[rn] = true;
                for (Int32 i = 0; i < id_table.Length; i++)
                {
                    if (id_table[i] == id_arr[rn])
                    {
                        for_return.Rows.Add(table_all_records.Rows[i]);
                        not_found[rn] = false;
                        break;
                    }
                }
            }
            time_of_search.Stop();
            if (show_execution_time == true)
            {
                Console.WriteLine(nameof(DatabaseToDataTableByID) + " " + "Execution time" + " " + time_of_search.ToString());
            }
            if (show_if_not_found == true)
            {
                if (not_found.Contains(true) == true)
                {
                    for (Int32 rn = 0; rn < id_arr.Length; rn++)
                    {
                        if (not_found[rn] == true)
                        {
                            Console.WriteLine(id_arr[rn] + " " + "was not found");
                        }
                    }
                }
            }
            return for_return;
        }
        static public void DataTableToDataBase(SqlConnection database_con, string db_table_name, DataTable datatable_in)
        {
            Int32 col_num = datatable_in.Columns.Count;
            string[] col_names = DatabaseGetColumnNames(database_con, db_table_name);
            foreach (DataRow row_selected in datatable_in.Rows)
            {
                string sql_cmd_str = "INSERT INTO " + db_table_name + " (";
                for (Int32 i = 0; i < col_num; i++)
                {
                    if (datatable_in.Rows[0][i].GetType() != typeof(System.DBNull))
                    {
                        if (i != 0)
                        {
                            sql_cmd_str += ",";
                        }
                        sql_cmd_str += col_names[i];
                    }
                }
                sql_cmd_str += ") VALUES(";
                for (Int32 i = 0; i < col_num; i++)
                {
                    if (datatable_in.Rows[0][i].GetType() != typeof(System.DBNull))
                    {
                        if (i != 0)
                        {
                            sql_cmd_str += ", ";
                        }
                        sql_cmd_str += "@A" + i.ToString();
                    }
                }
                sql_cmd_str += ")";
                SqlCommand sql_cmd = new SqlCommand(sql_cmd_str, database_con);
                for (Int32 i = 0; i < col_num; i++)
                {
                    if (datatable_in.Rows[0][i].GetType() != typeof(System.DBNull))
                    {
                        sql_cmd.Parameters.AddWithValue("@A" + i.ToString(), row_selected[i]);
                    }
                }
                try
                {
                    database_con.Open();
                    sql_cmd.ExecuteNonQuery();
                    database_con.Close();
                }
                catch (SqlException e)
                {
                    MyReportFunctions.ReportError(e.Message);
                    database_con.Close();
                }
            }
        }
        static public void DataBaseUpdateRow(SqlConnection database_con, string db_table_name, DataTable datatable_in)
        {
            Int32 col_num = datatable_in.Columns.Count;
            string[] col_names = DatabaseGetColumnNames(database_con, db_table_name);
            foreach (DataRow row_selected in datatable_in.Rows)
            {
                string sql_cmd_str = "UPDATE " + db_table_name + " SET ";
                for (Int32 i = 0; i < col_num; i++)
                {
                    if (datatable_in.Rows[0][i].GetType() != typeof(System.DBNull))
                    {
                        if (i != 0)
                        {
                            sql_cmd_str += ", ";
                        }
                        sql_cmd_str += (col_names[i] + " = @A" + i.ToString());
                    }
                }
                Int32 id_in = (int)datatable_in.Rows[0][0];
                sql_cmd_str += " WHERE ID = " + id_in.ToString();
                SqlCommand sql_cmd = new SqlCommand(sql_cmd_str, database_con);
                for (Int32 i = 0; i < col_num; i++)
                {
                    if (datatable_in.Rows[0][i].GetType() != typeof(System.DBNull))
                    {
                        sql_cmd.Parameters.AddWithValue("@A" + i.ToString(), row_selected[i]);
                    }
                }
                try
                {
                    database_con.Open();
                    sql_cmd.ExecuteNonQuery();
                    database_con.Close();
                }
                catch (SqlException e)
                {
                    MyReportFunctions.ReportError(e.Message);
                    database_con.Close();
                }
            }
        }
        static public DataTable DataBaseToDataTable(SqlConnection database_con, string table_name)
        {
            DataTable table_out = new DataTable();
            try // maybe without try
            {
                database_con.Open();
                if (database_con.State == ConnectionState.Open)
                {
                    using (SqlDataAdapter data_reader = new SqlDataAdapter("SELECT * FROM " + table_name, database_con.ConnectionString))
                    {
                        data_reader.Fill(table_out);
                    }
                    database_con.Close();
                }
                else
                {
                }
            }
            catch
            {
            }
            return table_out;
        }
        static public DataTable DatabaseColumnToDataTable(SqlConnection database_con, string table_name, Int32 col_num)
        {
            DataTable table_out = new DataTable();
            try // maybe without try
            {
                string[] columns_names = DatabaseGetColumnNames(database_con, table_name);
                database_con.Open();
                if (database_con.State == ConnectionState.Open)
                {
                    using (SqlDataAdapter data_reader = new SqlDataAdapter("SELECT " + columns_names[col_num - 1] + " FROM " + table_name, database_con.ConnectionString))
                    {
                        data_reader.Fill(table_out);
                    }
                    database_con.Close();
                }
                else
                {
                    MyReportFunctions.ReportError();
                }
            }
            catch
            {
                database_con.Close();
                MyReportFunctions.ReportError();
            }
            return table_out;
        }
        static public string[] DatabaseGetColumnNames(SqlConnection database_con, string table_name)
        {
            List<string> for_return = new List<string>();
            try // maybe without try
            {
                using (SqlCommand command_sql = new SqlCommand("SELECT * FROM " + table_name, database_con))
                {
                    database_con.Open();
                    using (SqlDataReader sql_reader = command_sql.ExecuteReader())
                    {
                        for (Int32 i = 0; i < sql_reader.FieldCount; i++)
                        {
                            for_return.Add(sql_reader.GetName(i));
                        }
                    }
                    database_con.Close();
                }
            }
            catch
            {
                database_con.Close();
                MyReportFunctions.ReportError();
            }
            return for_return.ToArray();
        }
        static public DataTable DataBaseToDataTableSorted(SqlConnection database_con, string table_name, Int32 col_num_for_sort, bool sort_ascending = true)
        {
            DataTable table_out = new DataTable();
            try // maybe without try
            {
                string[] col_names = DatabaseGetColumnNames(database_con, table_name);
                database_con.Open();
                if (database_con.State == ConnectionState.Open)
                {
                    string sql_cmd = "SELECT * FROM " + table_name + " ORDER BY " + col_names[col_num_for_sort - 1];
                    if (sort_ascending == true)
                    {
                        sql_cmd += " ASC";
                    }
                    else
                    {
                        sql_cmd += " DESC";
                    }
                    using (SqlDataAdapter data_reader = new SqlDataAdapter(sql_cmd, database_con.ConnectionString))
                    {
                        data_reader.Fill(table_out);
                    }
                    database_con.Close();
                }
                else
                {
                    MyReportFunctions.ReportError();
                }
            }
            catch
            {
                MyReportFunctions.ReportError();
            }
            return table_out;
        }
        static public class DatabaseColumn
        {
            static public DataTable ToDataTable(SqlConnection database_con, string table_name, Int32 col_num)
            {
                DataTable table_out = new DataTable();
                try // maybe without try
                {
                    string[] col_names = DatabaseGetColumnNames(database_con, table_name);
                    database_con.Open();
                    string sql_cmd = "SELECT " + col_names[col_num - 1] + " FROM " + table_name;
                    using (SqlDataAdapter data_reader = new SqlDataAdapter(sql_cmd, database_con.ConnectionString))
                    {
                        data_reader.Fill(table_out);
                    }
                    database_con.Close();
                }
                catch
                {
                    MyReportFunctions.ReportError();
                }
                return table_out;
            }
            static public DataTable ToDataTable(SqlConnection database_con, string table_name, string col_name)
            {
                string[] db_col_names = DatabaseGetColumnNames(database_con, table_name);
                Int32 col_number = Array.IndexOf(db_col_names, col_name);
                DataTable table_out = new DataTable();
                if (col_number != -1)
                {
                    table_out = ToDataTable(database_con, table_name, col_number + 1);
                }
                return table_out;
            }
            static public object[] ToObjectArray(SqlConnection database_con, string table_name, string col_name)
            {
                string[] db_col_names = DatabaseGetColumnNames(database_con, table_name);
                Int32 col_number = Array.IndexOf(db_col_names, col_name);
                DataTable table_db = new DataTable();
                if (col_number != -1)
                {
                    table_db = ToDataTable(database_con, table_name, col_number + 1);
                }
                return ColumnToObjectArray(table_db);
            }
            static public object[] ToObjectArray(SqlConnection database_con, string table_name, Int32 column_num)
            {
                DataTable table_db = new DataTable();
                if (column_num != -1)
                {
                    table_db = ToDataTable(database_con, table_name, column_num + 1);
                }
                return ColumnToObjectArray(table_db);
            }
        }
        static public object[][] DataTableToObjectArray(DataTable datatable_in)
        {
            object[][] arr_out = new object[datatable_in.Columns.Count][];
            Int32 trace_i = 0;
            Int32 trace_j = 0;
            try
            {
                for (Int32 i = 0; i < arr_out.Length; i++)
                {
                    arr_out[i] = new object[datatable_in.Rows.Count];
                    trace_i = i;
                    for (Int32 j = 0; i < arr_out[i].Length; j++)
                    {
                        trace_j = j;
                        arr_out[i][j] = datatable_in.Rows[j][i];
                    }
                }
            }
            catch
            {
                MyReportFunctions.ReportError("Cast failed.\r\nData in: [" + trace_i.ToString()
                    + "," + trace_j.ToString() + "]" + " was " + datatable_in.Rows[trace_j][trace_i].ToString());
            }
            return arr_out;
        }
        static public object[] ColumnToObjectArray(DataTable datatable_in)
        {
            object[] arr_out = new object[datatable_in.Rows.Count];
            Int32 trace_i = 0;
            try
            {
                for (Int32 i = 0; i < arr_out.Length; i++)
                {
                    trace_i = i;
                    arr_out[i] = datatable_in.Rows[i][0];
                }
            }
            catch
            {
                MyReportFunctions.ReportError("Cast failed.\r\nData in row " + trace_i.ToString()
                    + " was " + datatable_in.Rows[trace_i][0].ToString());
            }
            return arr_out;
        }
        static public void DataBaseDeleteRow(SqlConnection database_con, string table_name, Int32 row_id)
        {
            try
            {
                string sql_cmd_str = "DELETE FROM " + table_name + " WHERE ID = " + row_id.ToString();
                SqlCommand sql_cmd = new SqlCommand(sql_cmd_str, database_con);
                database_con.Open();
                sql_cmd.ExecuteNonQuery();
                database_con.Close();
            }
            catch (SqlException e)
            {
                MyReportFunctions.ReportError(e.Message);
            }
        }
        static public DataTable DatabaseIDGet(SqlConnection database_con, string table_name)
        {
            DataTable table_out = new DataTable();
            try // maybe without try
            {
                table_out = DatabaseColumn.ToDataTable(database_con, table_name, 1);
            }
            catch
            {
                MyReportFunctions.ReportError();
            }
            return table_out;
        }
        static public string[] DataTableColumnGet(DataTable datatable_in, Int32 col_num)
        {
            List<string> for_return = new List<string>();
            foreach (DataRow dr in datatable_in.Rows)
            {
                for_return.Add(dr[col_num].ToString());
            }
            return for_return.ToArray();
        }
        static public void DataTableToConsole(DataTable datatable_in)
        {
            DataRow[] table_rows = datatable_in.Select();
            string db_title = DataTableTitleGet(datatable_in);
            Console.WriteLine(db_title);
            for (Int32 i = 0; i < table_rows.Length; i++)
            {
                if (i != 0)
                {
                    Console.Write("\r\n");
                }
                for (Int32 j = 0; j < datatable_in.Columns.Count; j++)
                {
                    if (j != 0)
                    {
                        Console.Write("\t");
                    }
                    Console.Write(datatable_in.Rows[i].ItemArray[j].ToString());
                }
            }
            Console.Write("\r\n");
        }
        static public bool TableCreate(SqlConnection sql_con, string table_name, string[] col_names)
        {
            bool for_return = false;
            try
            {
                string for_sql_cmd = "CREATE TABLE [dbo].[" + table_name + "](";
                for_sql_cmd += "[" + col_names[0] + "] Int32 NOT NULL,";
                for (Int32 i = 1; i < col_names.Length; i++)
                {
                    for_sql_cmd += "[" + col_names[i] + "] NVARCHAR(MAX) NULL,";
                }
                for_sql_cmd += "PRIMARY KEY CLUSTERED(" + "[" + col_names[0] + "] ASC));";
                SqlCommand cmd_sql = new SqlCommand(for_sql_cmd, sql_con);
                cmd_sql.Connection.Open();
                cmd_sql.ExecuteNonQuery();
                cmd_sql.Connection.Close();
                for_return = true;
            }
            catch
            {
                sql_con.Close();
                MyReportFunctions.ReportError();
            }
            return for_return;
        }
        static public string[] GetKeys(SqlConnection db_in, string table_name, Int32 key_col_num = 1)
        {
            return DataTableToStrings(DatabaseColumnToDataTable(db_in, table_name, key_col_num));
        }
        static public string[] DataTableToStrings(DataTable datatable_in, char delimer_in = '\t')
        {
            DataRow[] table_rows = datatable_in.Select();
            string[] for_return = new string[0];
            if (table_rows.Length > 0)
            {
                for_return = new string[table_rows.Length];
                for (Int32 i = 0; i < table_rows.Length; i++)
                {
                    for (Int32 j = 0; j < datatable_in.Columns.Count; j++)
                    {
                        if (j != 0)
                        {
                            for_return[i] += Convert.ToString(delimer_in);
                        }
                        for_return[i] += datatable_in.Rows[i].ItemArray[j].ToString();
                    }
                }
            }
            else
            {
            }
            return for_return;
        }
        static public string DataTableTitleGet(DataTable datatable_in, char delimer_in = '\t')
        {
            string for_return = "";
            for (Int32 j = 0; j < datatable_in.Columns.Count; j++)
            {
                if (j != 0)
                {
                    for_return += delimer_in.ToString();
                }
                for_return += datatable_in.Columns[j].ColumnName;
            }
            return for_return;
        }
        static public Int32 StringsToDatabase(SqlConnection database_con, string table_name, string[] values_strings, bool parse_special_char = true, char delimer_in = '\t')
        {
            string[] keys_str = GetKeys(database_con, table_name);
            string[] title_string = DataBaseColumnsNamesGet(database_con, table_name);
            FileFunctionsNamespace.FileFunctions.TextFile.StringsToConsole(title_string);
            if (values_strings[0].Split('\t').Length != (title_string.Length - 1))
            {
                MyReportFunctions.ReportError("String split length != keys number");
                return 0;
            }
            return 1;
        }
        static public string[] DatabaseTablesNamesGet(SqlConnection database_con)
        {
            List<string> listacolumnas = new List<string>();
            database_con.Open();
            var table = database_con.GetSchema("Tables");
            database_con.Close();
            foreach (DataRow row_local in table.Rows)
            {
                listacolumnas.Add(row_local["TABLE_NAME"].ToString());
            }
            return listacolumnas.ToArray();
        }
        static public string[] DataBaseColumnsNamesGet(SqlConnection database_con, string table_name)
        {
            List<string> listacolumnas = new List<string>();
            using (SqlCommand command = database_con.CreateCommand())
            {
                command.CommandText = "SELECT TOP 0 * FROM " + table_name;
                database_con.Open();
                using (var reader = command.ExecuteReader())
                {
                    reader.Read();
                    var table = reader.GetSchemaTable();
                    foreach (DataRow row_local in table.Rows)
                    {
                        listacolumnas.Add(row_local["ColumnName"].ToString());
                    }
                }
            }
            return listacolumnas.ToArray();
        }
        static public bool AddArray(SqlConnection database_con, string table_name, string[] keys_in, string[] values_strings, char delimer_in = '\t')
        {
            bool for_return = true;
            try
            {
                string[] values_in = new string[0];
                database_con.Open();
                for (Int32 sn = 0; sn < values_strings.Length; sn++)
                {
                    values_in = values_strings[sn].Split(delimer_in);
                    string sql_string = "Insert into" + " " + table_name + " " + "(";
                    for (Int32 i = 0; i < keys_in.Length; i++)
                    {
                        if (i != 0)
                        {
                            sql_string += ", ";
                        }
                        sql_string += keys_in[i];
                    }
                    sql_string += ")" + " " + "Values" + " " + "(";
                    for (Int32 i = 0; i < keys_in.Length; i++)
                    {
                        if (i != 0)
                        {
                            sql_string += ", ";
                        }
                        sql_string += "@" + keys_in[i];
                    }
                    sql_string += ")";
                    SqlCommand cmd_to_do = new SqlCommand(sql_string, database_con);
                    for (Int32 i = 0; i < keys_in.Length; i++)
                    {
                        cmd_to_do.Parameters.AddWithValue(keys_in[i], values_in[i]);
                    }
                    cmd_to_do.ExecuteNonQuery();
                }
                database_con.Close();
            }
            catch
            {
                Console.WriteLine("wrong");
                for_return = false;
            }
            return for_return;
        }
    }
}
