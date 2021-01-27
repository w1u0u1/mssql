using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;

namespace mssql
{
    class Program
    {
        static string _outfile = null;

        static void LogLine(string format, params object[] args)
        {
            string str = "";
            if (args == null || args.Length == 0)
                str = format;
            else
                str = string.Format(format, args);
            str += Environment.NewLine;
            if (_outfile != null)
                File.AppendAllText(_outfile, str);
            else
                Console.Write(str);
        }

        static void ConnectQuery(string connectionString, string query = null)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    Console.WriteLine("Connect OK.");

                    if (!string.IsNullOrEmpty(query))
                    {
                        DataTable dt = new DataTable();

                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            sda.SelectCommand = new SqlCommand(query, con);
                            sda.Fill(dt);
                        }

                        if (dt.Rows.Count > 0)
                        {
                            string[] columnNames = dt.Columns.Cast<DataColumn>().Select(c => c.ColumnName).ToArray();
                            LogLine(string.Join(",", columnNames));

                            foreach (DataRow dr in dt.Rows)
                            {
                                string[] values = new string[dt.Columns.Count];
                                for (int i = 0; i < dt.Columns.Count; ++i)
                                {
                                    values[i] = string.Format("{0}", dr[i]).Trim();
                                }

                                LogLine(string.Join(",", values));
                            }
                        }
                    }

                    con.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        static void Main(string[] args)
        {
            if (args.Length == 1)
                ConnectQuery(args[0]);
            if (args.Length == 2 || args.Length == 3)
            {
                if (args.Length == 3)
                    _outfile = args[2];
                ConnectQuery(args[0], args[1]);
            }
        }
    }
}