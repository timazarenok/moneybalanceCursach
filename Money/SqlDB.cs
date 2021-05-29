using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Money
{
    static class SqlDB
    {
        public static int UserID = 0;
        public const string connectionString = @"Server=DESKTOP-VAR3OC6\SQLEXPRESS;Database=Money;Trusted_Connection=True;";
        public static bool Command(string expression)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            int number = 0;
            try
            {
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandText = expression;
                number = command.ExecuteNonQuery();
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
            if (number > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static DataTable Select(string selectString)
        {
            DataTable dataTable = new DataTable("database");
            try
            {
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandText = selectString;
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(dataTable);
                return dataTable;
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
                return dataTable;
            }
        }
        public static int GetId(string expression)
        {
            DataTable dt = Select(expression);
            int id = -1;
            try
            {
                foreach (DataRow dr in dt.Rows)
                {
                    id = Convert.ToInt32(dr["id"]);
                }
                return id;
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
            return id;
        }
        public static void GetUserId(string email)
        {
            DataTable find = Select($"select * from [Users] where email='{email}'");
            if (find.Rows.Count > 0)
            {
                UserID = Convert.ToInt32(find.Rows[0]["id"]);
            }
        }
        public static List<string> GetDataOneAttribute(string expression, string attribute)
        {
            DataTable dt = Select(expression);
            List<string> data = new List<string>();
            foreach (DataRow dr in dt.Rows)
            {
                data.Add(dr[attribute].ToString());
            }
            return data;
        }

    }
}
