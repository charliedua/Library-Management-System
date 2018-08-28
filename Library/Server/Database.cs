using System;
using System.Data;
using System.Data.SQLite;
using System.IO;

//using Dapper;

namespace Library
{
    public class Database
    {
        /// <summary>
        /// The connection string
        /// </summary>
        private string _connectionString;

        /// <summary>
        /// The connection
        /// </summary>
        private IDbConnection conn;

        /// <summary>
        /// The status of server.
        /// </summary>
        private bool _connected
        {
            get
            {
                return conn.State == ConnectionState.Open;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Database{T}"/> class.
        /// </summary>
        /// <param name="save">The save.</param>
        /// <param name="savload">The savload.</param>
        /// <param name="connStr">The connection string.</param>
        public Database(string connStr = "Data Source=.\\Library.db;Version=3;")
        {
            _connectionString = connStr;
        }

        /// <summary>
        /// initiates the connection to server.
        /// </summary>
        /// <returns>
        /// status of <c>connection</c>
        /// </returns>
        /// <exception cref="FileNotFoundException"></exception>
        public bool Connect()
        {
            conn = new SQLiteConnection(_connectionString);
            return _connected;
        }

        /// <summary>
        /// Loads from the specified table name.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="whereClause">The where clause.</param>
        /// <returns></returns>
        public IDataReader Load(string tableName, string whereClause = "1 == 1")
        {
            using (conn)
            {
                string query = string.Format("select * from `{0}` where {1}", tableName, whereClause);
                IDbCommand cmd = conn.CreateCommand();
                cmd.CommandText = query;
                return cmd.ExecuteReader();
            }
        }

        /// <summary>
        /// Saves to the specified table.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="coloumnNames">The coloumn names.</param>
        /// <param name="values">The values.</param>
        public void Save(string tableName, string[] coloumnNames, string[] values)
        {
            using (conn)
            {
                string cols = AggreegateAllInStrArr(coloumnNames);
                string valuesStr = AggreegateAllInStrArr(values);
                string query = string.Format("insert into `{0}` ({1}) values ({2})", tableName, cols, valuesStr);
                IDbCommand cmd = conn.CreateCommand();
                cmd.CommandText = query;
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Aggreegates all in string array.
        /// </summary>
        /// <param name="arr">The string array.</param>
        /// <returns></returns>
        private string AggreegateAllInStrArr(string[] arr)
        {
            string final = "";
            foreach (string item in arr)
            {
                final += item;
            }
            return final;
        }
    }
}