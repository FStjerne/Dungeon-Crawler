using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameWork_Game
{
    class DataGreatSword : Database
    {
        public int DeleteInt { get; set; }

        public int Damage { get; set; }

        public int Range { get; set; }

        public int Speed { get; set; }

        public int Acquired { get; set; }

        public int Id { get; set; }

        public DataGreatSword()
        {

        }

        public DataGreatSword(int id, int acquired, int speed, int range, int damage)
        {
            this.Id = id;
            this.Acquired = acquired;
            this.Speed = speed;
            this.Range = range;
            this.Damage = damage;
        }

        /// <summary>
        /// Generates a table with the name sword.
        /// </summary>
        public void CreateTable()
        {
            String greatSwordCreateTable = "create table greatSword(ID integer primary key, acquired int, speed int, range int, damage int);";
            SQLiteCommand greatSwordCommand = new SQLiteCommand(greatSwordCreateTable, DatabaseConnection.dbConnection);
            greatSwordCommand.ExecuteNonQuery();
        }

        /// <summary>
        /// Updates the value of acquired in a specific row in the sword table.
        /// </summary>
        /// <param acquired="acquired">Set value of acquired (1 for true or 0 for false).</param>
        public static void GreatSwordUpdateAcquired(int acquired)
        {
            String greatSwordUpdateTable = "Update greatSword set acquired = " + acquired + " where ID = 1;";
            SQLiteCommand greatSwordCommand = new SQLiteCommand(greatSwordUpdateTable, DatabaseConnection.dbConnection);
            greatSwordCommand.ExecuteNonQuery();
        }

        /// <summary>
        /// Inserts a new row into sword with the stats of the weapon and acquired set to 0.
        /// </summary>
        public void GreatSwordInsertTable()
        {
            String greatSwordInsertTable = "insert into greatSword values(null, 0, 20, 15, 1);";
            SQLiteCommand greatSwordCommand = new SQLiteCommand(greatSwordInsertTable, DatabaseConnection.dbConnection);
            greatSwordCommand.ExecuteNonQuery();
        }
    }
}
