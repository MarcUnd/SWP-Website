using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace LuigiWebsite.Models.DB {
    public class RepositoryDB : IRepositoryDB {

        private string _connectionString = "Server=localhost;database=luigiswonderworld;user=root;password=";
        //Verbindungsobjekt zum Zugriff auf die Datenbank, hiermit können SQL Befehle an den DB-Server gesendet werden
        private DbConnection _conn;

        public void Connect() {
            if (this._conn == null) {
                //falls Verbindung noch nicht erzeugt, wird sie hier erstellt
                this._conn = new MySqlConnection(this._connectionString);
            }
            if (this._conn.State != ConnectionState.Open) {
                //falls Verbundung noch nicht hergesetellt, wird sie hier hergestellt
                this._conn.Open();
            }
        }

        public void Disconnect() {
            //falls das Verbindungsobjekt exisiert und die Verbindung offen ist
            if ((this._conn != null) && (this._conn.State == ConnectionState.Open)) {
                //Verbindung wird geschlossen
                this._conn.Close();
            }
        }

        public List<MenuDB> getMenu() {

            List<MenuDB> menu = new List<MenuDB>();

            if(this._conn?.State == ConnectionState.Open) {
                DbCommand cmdMenu = this._conn.CreateCommand();
            
                cmdMenu.CommandText = "select * from menu;";

                using(DbDataReader reader = cmdMenu.ExecuteReader()) {
                    while (reader.Read()) {
                        menu.Add(new MenuDB {
                            MenuId = Convert.ToInt32(reader["menuid"]),
                            Preis = Convert.ToDecimal(reader["preis"]),
                            Name = Convert.ToString(reader["name"]),
                            Zutaten = Convert.ToString(reader["zutaten"])
                        });
                    }
                }
            }
            return menu;
        }
    }
}
