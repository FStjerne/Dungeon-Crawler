using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameWork_Game
{
    class DataMageStaff : Database
    {
        public int DeleteInt { get; set; }

        public int Damage { get; set; }

        public int Range { get; set; }

        public int Speed { get; set; }

        public int Acquired { get; set; }

        public int Id { get; set; }

        public DataMageStaff()
        {

        }

        public DataMageStaff(int id, int acquired, int speed, int range, int damage)
        {
            this.Id = id;
            this.Acquired = acquired;
            this.Speed = speed;
            this.Range = range;
            this.Damage = damage;
        }

        /// <summary>
        /// Generates a table with the name battlestaff.
        /// </summary>
        public void CreateTable()
        {
            String mageStaffCreateTable = "create table mageStaff(ID integer primary key, acquired int, speed int, range int, damage int);";
            SQLiteCommand mageStaffCommand = new SQLiteCommand(mageStaffCreateTable, DatabaseConnection.dbConnection);
            mageStaffCommand.ExecuteNonQuery();
        }

        /// <summary>
        /// Updates the value of acquired in a specific row in the battlestaff table.
        /// </summary>
        /// <param acquired="acquired">Set value of acquired (1 for true or 0 for false).</param>
        public static void MageStaffUpdateAcquired(int acquired)
        {
            String mageStaffUpdateTable = "Update mageStaff set acquired = " + acquired + " where ID = 1;";
            SQLiteCommand mageStaffCommand = new SQLiteCommand(mageStaffUpdateTable, DatabaseConnection.dbConnection);
            mageStaffCommand.ExecuteNonQuery();
        }

        /// <summary>
        /// Inserts a new row into battlestaff with the stats of the weapon and acquired set to 0.
        /// </summary>
        public void MageStaffInsertTable()
        {
            String mageStaffInsertTable = "insert into mageStaff values(null, 0, 10, 100, 1);";
            SQLiteCommand mageStaffCommand = new SQLiteCommand(mageStaffInsertTable, DatabaseConnection.dbConnection);
            mageStaffCommand.ExecuteNonQuery();
        }
    }
}
