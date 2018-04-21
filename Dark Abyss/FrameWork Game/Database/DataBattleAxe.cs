using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameWork_Game
{
    class DataBattleAxe : Database
    {
        public int DeleteInt { get; set; }

        public int Damage { get; set; }

        public int Range { get; set; }

        public int Speed { get; set; }

        public int Acquired { get; set; }

        public int Id { get; set; }

        public DataBattleAxe()
        {

        }

        public DataBattleAxe(int id, int acquired, int speed, int range, int damage)
        {
            this.Id = id;
            this.Acquired = acquired;
            this.Speed = speed;
            this.Range = range;
            this.Damage = damage;
        }

        /// <summary>
        /// Generates a table with the name battleaxe.
        /// </summary>
        public void CreateTable()
        {
            String battleAxeCreateTable = "create table battleAxe(ID integer primary key, acquired int, speed int, range int, damage int);";
            SQLiteCommand battleAxeCommand = new SQLiteCommand(battleAxeCreateTable, DatabaseConnection.dbConnection);
            battleAxeCommand.ExecuteNonQuery();
        }

        /// <summary>
        /// Updates the value of acquired in a specific row in the battleaxe table.
        /// </summary>
        /// <param acquired="acquired">Set value of acquired (1 for true or 0 for false).</param>
        public static void BattleAxeUpdateAcquired(int acquired)
        {
            String battleAxeUpdateTable = "Update battleAxe set acquired = " + acquired + " where ID = 1;";
            SQLiteCommand battleAxeCommand = new SQLiteCommand(battleAxeUpdateTable, DatabaseConnection.dbConnection);
            battleAxeCommand.ExecuteNonQuery();
        }

        /// <summary>
        /// Inserts a new row into battleaxe with the stats of the weapon and acquired set to 0.
        /// </summary>
        public void BattleAxeInsertTable()
        {
            String battleAxeInsertTable = "insert into battleAxe values(null, 0, 10, 25, 3);";
            SQLiteCommand battleAxeCommand = new SQLiteCommand(battleAxeInsertTable, DatabaseConnection.dbConnection);
            battleAxeCommand.ExecuteNonQuery();
        }
    }
}

