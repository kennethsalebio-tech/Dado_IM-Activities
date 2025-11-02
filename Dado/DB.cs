using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.IO;
using MySql.Data.MySqlClient;

namespace CRUD
{
    public static class DB
    {
        private static string configFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "dbconn.txt");
        private static string defaultConn ="server=localhost;port=3306;database=school;uid=root;pwd=Herobrine303?;AllowPublicKeyRetrieval=True;";



        private static string GetConnStr()
        {
            try
            {
                if (File.Exists(configFile))
                {
                    var txt = File.ReadAllText(configFile).Trim();
                    if (!string.IsNullOrEmpty(txt)) return txt;
                }
            }
            catch { }
            return defaultConn;
        }
        public static bool TestConnection(string connStr, out string message)
        {
            try
            {
                using (var conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    conn.Close();
                }
                message = "Connection successful.";
                return true;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return false;
            }
        }
        public static void SaveConnectionString(string connStr)
        {
            File.WriteAllText(configFile, connStr);
        }
        public static DataTable GetDataTable(string sql, params MySqlParameter[] parameters)
        {
            var dt = new DataTable();
            string connStr = GetConnStr();
            using (var conn = new MySqlConnection(connStr))
            using (var cmd = new MySqlCommand(sql, conn))
            {
                if (parameters != null && parameters.Length > 0)
                    cmd.Parameters.AddRange(parameters);
                using (var adapter = new MySqlDataAdapter(cmd))
                {
                    adapter.Fill(dt);
                }
            }
            return dt;
        }
        public static int ExecuteNonQuery(string sql, params MySqlParameter[] parameters)
        {
            string connStr = GetConnStr();
            using (var conn = new MySqlConnection(connStr))
            using (var cmd = new MySqlCommand(sql, conn))
            {
                if (parameters != null && parameters.Length > 0)
                    cmd.Parameters.AddRange(parameters);
                conn.Open();
                return cmd.ExecuteNonQuery();
            }
        }
        public static object ExecuteScalar(string sql, params MySqlParameter[] parameters)
        {
            string connStr = GetConnStr();
            using (var conn = new MySqlConnection(connStr))
            using (var cmd = new MySqlCommand(sql, conn))
            {
                if (parameters != null && parameters.Length > 0)
                    cmd.Parameters.AddRange(parameters);
                conn.Open();
                return cmd.ExecuteScalar();
            }
        }
    }
}
