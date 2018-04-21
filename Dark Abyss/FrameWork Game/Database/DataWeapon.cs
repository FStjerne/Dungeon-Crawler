using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameWork_Game
{
    class DataWeapon : Database
    {
        public int DeleteInt { get; set; }

        public static int HaveScythe { get; set; }

        public static int HaveBow { get; set; }

        public static int HaveGreatSword { get; set; }

        public static int HaveBattleAxe { get; set; }

        public static int HaveMageStaff { get; set; }

        public int Id { get; set; }

        public DataWeapon()
        {

        }

        /// <summary>
        /// Generates a table with the name weapon.
        /// </summary>
        public void CreateTable()
        {
            String weaponCreateTable = "create table weapon(ID integer primary key, HaveMageStaff integer, HaveBattleAxe integer, HaveGreatSword integer, HaveScythe integer, HaveBow integer," +
                "Foreign Key(HaveMageStaff) References MageStaff(Acquired), Foreign Key(HaveBattleAxe) References BattleAxe(Acquired), Foreign Key(HaveGreatSword) References GreatSword(acquired), Foreign Key(HaveScythe) References Scythe(Acquired), Foreign Key(HaveBow) References Bow(Acquired));";
            SQLiteCommand weaponCommand = new SQLiteCommand(weaponCreateTable, DatabaseConnection.dbConnection);
            weaponCommand.ExecuteNonQuery();
        }

        /// <summary>
        /// Updates the value of all Items in a specific row in the weapon table.
        /// </summary>
        public static void WeaponUpdateAcquired()
        {
            String weaponUpdate = "Update weapon set HaveMageStaff = (Select acquired from mageStaff), HaveBattleAxe = (Select acquired from battleAxe), HaveGreatSword = (Select acquired from greatSword), HaveBow = (Select acquired from bow), HaveScythe = (Select acquired from scythe) where ID = 1;";
            SQLiteCommand weaponCommand = new SQLiteCommand(weaponUpdate, DatabaseConnection.dbConnection);
            weaponCommand.ExecuteNonQuery();
        }

        public static void WeaponUpdateReset()
        {
            String weaponUpdate = "Update weapon set HaveMageStaff = 0, HaveBattleAxe = 0, HaveGreatSword = 0, HaveBow = 0, HaveScythe = 0 where ID = 1;";
            SQLiteCommand weaponCommand = new SQLiteCommand(weaponUpdate, DatabaseConnection.dbConnection);
            weaponCommand.ExecuteNonQuery();
        }

        public static void WeaponSelectFields()
        {
            String weaponSelect = "Select HaveMageStaff, HaveBattleAxe, HaveGreatSword, HaveScythe, HaveBow from weapon where ID = 1;";
            SQLiteCommand weaponCommand = new SQLiteCommand(weaponSelect, DatabaseConnection.dbConnection);
            SQLiteDataReader weaponReader = weaponCommand.ExecuteReader();
            while (weaponReader.Read())
            {
                HaveMageStaff = int.Parse(weaponReader["HaveMageStaff"].ToString());
                HaveBattleAxe = int.Parse(weaponReader["HaveBattleAxe"].ToString());
                HaveGreatSword = int.Parse(weaponReader["HaveGreatSword"].ToString());
                HaveScythe = int.Parse(weaponReader["HaveScythe"].ToString());
                HaveBow = int.Parse(weaponReader["HaveBow"].ToString());
            }
        }

        /// <summary>
        /// Inserts a new row into weapon with the acquired weapons.
        /// </summary>
        public void WeaponInsertTable()
        {
            String weaponInsertTable = "insert into weapon values(null, (Select Acquired from mageStaff), (Select Acquired from battleAxe), (Select Acquired from greatSword), (Select Acquired from scythe), (Select Acquired from bow));";
            SQLiteCommand weaponCommand = new SQLiteCommand(weaponInsertTable, DatabaseConnection.dbConnection);
            weaponCommand.ExecuteNonQuery();
        }
    }
}
