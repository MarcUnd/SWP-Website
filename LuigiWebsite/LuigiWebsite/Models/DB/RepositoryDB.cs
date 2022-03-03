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

        public bool Insert(user u) {
            
            if(this._conn?.State == ConnectionState.Open) {
                DbCommand cmdIns = this._conn.CreateCommand();

                cmdIns.CommandText = "insert into customer values(null, @vn, @nn, sha2(@pwd,512), @email, @bd);";

                DbParameter paramVN = cmdIns.CreateParameter();
                paramVN.ParameterName = "vn";
                paramVN.DbType = DbType.String;
                paramVN.Value = u.vorname;

                DbParameter paramNN = cmdIns.CreateParameter();
                paramNN.ParameterName = "nn";
                paramNN.DbType = DbType.String;
                paramNN.Value = u.nachname;

                DbParameter paramPWD = cmdIns.CreateParameter();
                paramPWD.ParameterName = "pwd";
                paramPWD.DbType = DbType.String;
                paramPWD.Value = u.password;

                DbParameter paramEM = cmdIns.CreateParameter();
                paramEM.ParameterName = "email";
                paramEM.DbType = DbType.String;
                paramEM.Value = u.email;

                DbParameter paramBD = cmdIns.CreateParameter();
                paramBD.ParameterName = "bd";
                paramBD.DbType = DbType.DateTime;
                paramBD.Value = u.BirthDate;

                cmdIns.Parameters.Add(paramVN);
                cmdIns.Parameters.Add(paramPWD);
                cmdIns.Parameters.Add(paramBD);
                cmdIns.Parameters.Add(paramEM);
                cmdIns.Parameters.Add(paramNN);

                return cmdIns.ExecuteNonQuery() == 1;
            }
            return false;
        }
    }
}
