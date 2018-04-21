using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameWork_Game
{
    class Database
    {
        public bool CheckConnection { get; set; }

        /// <summary>
        /// Generates a new Database file.
        /// </summary>
        public void CreateDatabase()
        {
            SQLiteConnection.CreateFile("WeaponDataBase.db");
        }
    }
}
