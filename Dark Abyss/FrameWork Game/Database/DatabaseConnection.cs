using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameWork_Game
{
    class DatabaseConnection
    {
        public static SQLiteConnection dbConnection;

        /// <summary>
        /// Generates and Opens a connection to the specified Database
        /// </summary>
        static DatabaseConnection()
        {
            string ConnectionString = "Data Source = WeaponDataBase.db;Version=3;";
            dbConnection = new SQLiteConnection(ConnectionString);
            dbConnection.Open();
        }
    }
}
