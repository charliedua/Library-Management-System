using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.IO;

//using Dapper;

namespace Library
{
    public class Database : IDisposable
    {
        /// <summary>
        /// The connection string
        /// </summary>
        private readonly string _connectionString;

        /// <summary>
        /// The connection
        /// </summary>
        private IDbConnection conn;

        /// <summary>
        /// Initializes a new instance of the <see cref="Database{T}"/> class.
        /// </summary>
        /// <param name="save">The save.</param>
        /// <param name="savload">The savload.</param>
        /// <param name="connStr">The connection string.</param>
        public Database(string connStr = "Data Source=.//Library.db;Version=3;")
        {
            _connectionString = connStr;
            conn = new SQLiteConnection(_connectionString);
        }

        /// <summary>
        /// The status of server.
        /// </summary>
        private bool _connected => conn.State == ConnectionState.Open;

        /// <summary>
        /// initiates the connection to server.
        /// </summary>
        /// <returns>
        /// status of <c>connection</c>
        /// </returns>
        public bool Connect()
        {
            conn.Open();
            return true;
        }

        /// <summary>
        /// Closses the connection to the server.
        /// </summary>
        /// <returns>
        /// status of <c>connection</c>
        /// </returns>
        public bool Disconnect()
        {
            conn.Close();
            return true;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            conn.Dispose();
        }

        /// <summary>
        /// Gets the last identifier.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <returns></returns>
        public int GetLastInsertedID(string tableName)
        {
            Connect();
            IDbCommand cmd = conn.CreateCommand();
            string query = string.Format("select max(ID) from `{0}`", tableName);
            cmd.CommandText = query;
            int id;
            id = (int)((long)cmd.ExecuteScalar());
            Disconnect();
            return id;
        }

        /// <summary>
        /// Loads from the specified table name.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="whereClause">The where clause.</param>
        /// <returns></returns>
        public SQLiteDataReader LoadReader(string tableName, string whereClause = "1 = 1")
        {
            Connect();
            string query = string.Format("select * from `{0}` where {1}", tableName, whereClause);
            IDbCommand cmd = conn.CreateCommand();
            cmd.CommandText = query;
            return (SQLiteDataReader)cmd.ExecuteReader();
        }

        public int Delete(string tableName, int ID)
        {
            Connect();
            IDbCommand cmd = conn.CreateCommand();
            cmd.CommandText = string.Format("delete from `{0}` where `ID` = {1}", tableName, ID.ToString());
            int rowsAffected = cmd.ExecuteNonQuery();
            Disconnect();
            return rowsAffected;
        }

        /// <summary>
        /// Saves to the specified table.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="coloumnNames">The coloumn names.</param>
        /// <param name="values">The values.</param>
        public void Save(string tableName, List<string> coloumnNames, List<string> values)
        {
            Connect();
            string cols = AggreegateAllInStrArr(coloumnNames.ToArray(), with: '`');
            string valuesStr = AggreegateAllInStrArr(values.ToArray());
            string query = string.Format("insert into `{0}` ({1}) values ({2})", tableName, cols, valuesStr);
            IDbCommand cmd = conn.CreateCommand();
            cmd.CommandText = query;
            cmd.ExecuteNonQuery();
            Disconnect();
        }

        /// <summary>
        /// Aggreegates all in string array.
        /// </summary>
        /// <param name="arr">The string array.</param>
        /// <returns></returns>
        private string AggreegateAllInStrArr(string[] arr, char with = '\'')
        {
            string final = "";
            for (int i = 0; i < arr.Length - 1; i++)
            {
                if (arr.Length > 0)
                {
                    if (int.TryParse(arr[i], out int b))
                    {
                        final += b.ToString();
                    }
                    else
                    {
                        final += with + arr[i] + with;
                    }
                    final += ", ";
                }
                else
                    break;
            }
            if (int.TryParse(arr[arr.Length - 1], out int a))
            {
                final += a.ToString();
            }
            else
            {
                final += with + arr[arr.Length - 1] + with;
            }
            return final;
        }
    }
}