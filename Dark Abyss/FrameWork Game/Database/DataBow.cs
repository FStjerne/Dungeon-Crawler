using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameWork_Game
{
    class DataBow : Database
    {
        public int DeleteInt { get; set; }

        public int Damage { get; set; }

        public int Range { get; set; }

        public int Speed { get; set; }

        public int Acquired { get; set; }

        public int Id { get; set; }

        public DataBow()
        {

        }

        public DataBow(int id, int acquired, int speed, int range, int damage)
        {
            this.Id = id;
            this.Acquired = acquired;
            this.Speed = speed;
            this.Range = range;
            this.Damage = damage;
        }

        /// <summary>
        /// Generates a table with the name bow.
        /// </summary>
        public void CreateTable()
        {
            String bowCreateTable = "create table bow(ID integer primary key, acquired int, speed int, range int, damage int);";
            SQLiteCommand bowCommand = new SQLiteCommand(bowCreateTable, DatabaseConnection.dbConnection);
            bowCommand.ExecuteNonQuery();
        }

        /// <summary>
        /// Updates the value of acquired in a specific row in the bow table.
        /// </summary>
        /// <param acquired="acquired">Set value of acquired (1 for true or 0 for false).</param>
        public static void BowUpdateAcquired(int acquired)
        {
            String bowUpdateTable = "Update bow set acquired = " + acquired + " where ID = 1;";
            SQLiteCommand bowCommand = new SQLiteCommand(bowUpdateTable, DatabaseConnection.dbConnection);
            bowCommand.ExecuteNonQuery();
        }

        /// <summary>
        /// Inserts a new row into scythe with the stats of the weapon and acquired set to 0.
        /// </summary>
        public void BowInsertTable()
        {
            String bowInsertTable = "insert into bow values(null, 0, 5, 100, 2);";
            SQLiteCommand bowCommand = new SQLiteCommand(bowInsertTable, DatabaseConnection.dbConnection);
            bowCommand.ExecuteNonQuery();
        }
    }
}
