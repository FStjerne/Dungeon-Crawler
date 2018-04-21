using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameWork_Game
{
    class DataScythe : Database
    {
        public int DeleteInt { get; set; }

        public int Damage { get; set; }

        public int Range { get; set; }

        public int Speed { get; set; }

        public int Acquired { get; set; }

        public int Id { get; set; }

        public DataScythe()
        {

        }

        public DataScythe(int id, int acquired, int speed, int range, int damage)
        {
            this.Id = id;
            this.Acquired = acquired;
            this.Speed = speed;
            this.Range = range;
            this.Damage = damage;
        }

        /// <summary>
        /// Generates a table with the name scythe.
        /// </summary>
        public void CreateTable()
        {
            String scytheCreateTable = "create table scythe(ID integer primary key, acquired int, speed int, range int, damage int);";
            SQLiteCommand scytheCommand = new SQLiteCommand(scytheCreateTable, DatabaseConnection.dbConnection);
            scytheCommand.ExecuteNonQuery();
        }

        /// <summary>
        /// Updates the value of acquired in a specific row in the scythe table.
        /// </summary>
        /// <param acquired="acquired">Set value of acquired (1 for true or 0 for false).</param>
        public static void ScytheUpdateAcquired(int acquired)
        {
            String scytheUpdateTable = "Update scythe set acquired = " + acquired + " where ID = 1;";
            SQLiteCommand scytheCommand = new SQLiteCommand(scytheUpdateTable, DatabaseConnection.dbConnection);
            scytheCommand.ExecuteNonQuery();
        }

        /// <summary>
        /// Inserts a new row into scythe with the stats of the weapon and acquired set to 0.
        /// </summary>
        public void ScytheInsertTable()
        {
            String scytheInsertTable = "insert into scythe values(null, 0, 5, 50, 2);";
            SQLiteCommand scytheCommand = new SQLiteCommand(scytheInsertTable, DatabaseConnection.dbConnection);
            scytheCommand.ExecuteNonQuery();
        }
    }
}
