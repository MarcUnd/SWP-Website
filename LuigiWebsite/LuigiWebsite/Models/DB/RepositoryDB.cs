using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace LuigiWebsite.Models.DB {

    //parallele und asynchrone Programmierung 
    //Threads sollten in C# nicht direkt verwendet werden (Sie werden allerdings immer intern verwendet) 
    //Tasks sollten statt Threads verwendet werden 
    //async/wait: verwendet im Hintergrund Task's 
    //ermöglicht, dass die asynchrone und synchrone Porgrammierung fasst identisch ist 

    public class RepositoryDB : IRepositoryDB {

        private string _connectionString = "Server=localhost;database=luigiswonderworld;user=root;password=";
        //Verbindungsobjekt zum Zugriff auf die Datenbank, hiermit können SQL Befehle an den DB-Server gesendet werden
        private DbConnection _conn;

        public async Task ConnectAsync() {
            if (this._conn == null) {
                //falls Verbindung noch nicht erzeugt, wird sie hier erstellt
                this._conn = new MySqlConnection(this._connectionString);
            }
            if (this._conn.State != ConnectionState.Open) {
                //falls Verbundung noch nicht hergesetellt, wird sie hier hergestellt
                await this._conn.OpenAsync();
            }
        }

        public async Task DisconnectAsync() {
            //falls das Verbindungsobjekt exisiert und die Verbindung offen ist
            if ((this._conn != null) && (this._conn.State == ConnectionState.Open)) {
                //Verbindung wird geschlossen
                await this._conn.CloseAsync();
            }
        }

        public async Task<List<MenuDB>> GetMenuAsync() {

            List<MenuDB> menu = new List<MenuDB>();

            if (this._conn?.State == ConnectionState.Open) {
                DbCommand cmdMenu = this._conn.CreateCommand();

                cmdMenu.CommandText = "select * from menu;";

                using(DbDataReader reader = await cmdMenu.ExecuteReaderAsync()) {
                    while (await reader.ReadAsync()) {
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


        public async Task<bool> InsertAsync(user u) {

            if (this._conn?.State == ConnectionState.Open) {
                DbCommand cmdIns = this._conn.CreateCommand();

                cmdIns.CommandText = "insert into customer values(null, @vn, @nn, sha2(@pwd,512), @bd, @email);";

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

                return await cmdIns.ExecuteNonQueryAsync() == 1;
            }
            return false;
        }


        public  async Task<bool> isUserAsync(string email, string password) {
            if (this._conn?.State == ConnectionState.Open) {
                DbCommand cmdLogin = this._conn.CreateCommand();
                cmdLogin.CommandText = "select * from customer where email = @email and passwort = sha2(@pwd,512);";

                DbParameter paramE = cmdLogin.CreateParameter();
                paramE.ParameterName = "email";
                paramE.DbType = DbType.String;
                paramE.Value = email;

                DbParameter paramP = cmdLogin.CreateParameter();
                paramP.ParameterName = "pwd";
                paramP.DbType = DbType.String;
                paramP.Value = password;

                cmdLogin.Parameters.Add(paramP);
                cmdLogin.Parameters.Add(paramE);

                using(DbDataReader reader = await cmdLogin.ExecuteReaderAsync()) {
                    if (await reader.ReadAsync()) {
                        return true;
                    }
                }
            }
            return false;
        }

        public async Task<bool> InsertResAsync(reservation r) {
            if (this._conn?.State == ConnectionState.Open) {
                DbCommand cmdRes = this._conn.CreateCommand();

                cmdRes.CommandText = "insert into reservation values(null, @nn, @email, @uhr, @dat);";

                DbParameter paramNN = cmdRes.CreateParameter();
                paramNN.ParameterName = "nn";
                paramNN.DbType = DbType.String;
                paramNN.Value = r.nachname;

                DbParameter paramEM = cmdRes.CreateParameter();
                paramEM.ParameterName = "email";
                paramEM.DbType = DbType.String;
                paramEM.Value = r.email;

                DbParameter paramUH = cmdRes.CreateParameter();
                paramUH.ParameterName = "uhr";
                paramUH.DbType = DbType.String;
                paramUH.Value = r.uhrzeit;

                DbParameter paramBD = cmdRes.CreateParameter();
                paramBD.ParameterName = "dat";
                paramBD.DbType = DbType.Date;
                paramBD.Value = r.date;

                cmdRes.Parameters.Add(paramNN);
                cmdRes.Parameters.Add(paramEM);
                cmdRes.Parameters.Add(paramUH);
                cmdRes.Parameters.Add(paramBD);

                return await cmdRes.ExecuteNonQueryAsync() == 1;
            }
            return false;
        }

        public String getEmailById(int id) {
            String email = "";

            if (this._conn?.State == ConnectionState.Open) {

                DbCommand cmdUser = this._conn.CreateCommand();

                cmdUser.CommandText = "select email from customer where customerID = @id;";

                DbParameter paramId = cmdUser.CreateParameter();
                paramId.ParameterName = "id";
                paramId.DbType = DbType.Int32;
                paramId.Value = id;

                cmdUser.Parameters.Add(paramId);
                using (DbDataReader reader = cmdUser.ExecuteReader()) {
                    //Eine Zeile Datensatz lesen
                    if (reader.Read()) {
                        email = Convert.ToString(reader["email"]);
                    }
                }
                
            }
            return email;
        }
    }
}
