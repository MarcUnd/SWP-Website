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
        private DbConnection _conn;

        public async Task ConnectAsync() {
            if (this._conn == null) {
                this._conn = new MySqlConnection(this._connectionString);
            }
            if (this._conn.State != ConnectionState.Open) {
                await this._conn.OpenAsync();
            }
        }

        public async Task DisconnectAsync() {
            
            if ((this._conn != null) && (this._conn.State == ConnectionState.Open)) {
                
                await this._conn.CloseAsync();
            }
        }

        public async Task<user> getUserByEmailAsync(string email) {
            user u = null;

            if (this._conn?.State == ConnectionState.Open) {
                DbCommand cmdUser = this._conn.CreateCommand();

                cmdUser.CommandText = "select * from customer where email = @em;";

                DbParameter paramEM = cmdUser.CreateParameter();
                paramEM.ParameterName = "em";
                paramEM.DbType = DbType.String;
                paramEM.Value = email;

                cmdUser.Parameters.Add(paramEM);
                using (DbDataReader reader = await cmdUser.ExecuteReaderAsync()) {
                    while (await reader.ReadAsync()) {
                        u = new user { email = Convert.ToString(reader["email"]),
                                       CustomerId = Convert.ToInt32(reader["customerID"]),
                                       vorname = Convert.ToString(reader["vorname"]), 
                                       nachname = Convert.ToString(reader["nachname"]), 
                                       password = Convert.ToString(reader["passwort"]),
                                       BirthDate = Convert.ToDateTime(reader["geburtstag"]) };
                        }
                }
            }
            return u;
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

        public async Task<List<reservation>> getReservationsByEmailAsync(String email) {
            List<reservation> reservations = new List<reservation>();
            if (this._conn?.State == ConnectionState.Open) {
                DbCommand cmdReservation = this._conn.CreateCommand();

                cmdReservation.CommandText = "select * from reservation where email = @em;";

                DbParameter paramEM = cmdReservation.CreateParameter();
                paramEM.ParameterName = "em";
                paramEM.DbType = DbType.String;
                paramEM.Value = email;

                cmdReservation.Parameters.Add(paramEM);

                using (DbDataReader reader = await cmdReservation.ExecuteReaderAsync()) {
                    while (await reader.ReadAsync()) {

                        string uhr = Convert.ToString(reader["uhrzeit"]);
                        string dat = Convert.ToString(reader["datum"]);

                        DateTime uhrzeit = DateTime.Parse(uhr);
                        DateTime date = DateTime.Parse(dat);
                        DateTime time = new DateTime(date.Year,date.Month,date.Day,uhrzeit.Hour,uhrzeit.Minute,uhrzeit.Second); 
                       


                        reservations.Add(new reservation {
                            nachname = Convert.ToString(reader["nachname"]),
                            email = Convert.ToString(reader["email"]),
                            date = date,
                            uhrzeit = time,
                            ResId = Convert.ToInt32(reader["resID"])
                        });
                    }
                }
                
            }
            return reservations;

        }

        public async Task<bool> DeleteAsync(int resId) {

            if (this._conn?.State == ConnectionState.Open) {
                DbCommand cmdDeleteRes = this._conn.CreateCommand();
                cmdDeleteRes.CommandText = "delete from reservation where resID  = @resid;";
                DbParameter paramID = cmdDeleteRes.CreateParameter();
                paramID.ParameterName = "resId";
                paramID.DbType = DbType.Int32;
                paramID.Value = resId;
                cmdDeleteRes.Parameters.Add(paramID);

                return await cmdDeleteRes.ExecuteNonQueryAsync() == 1;
            } 

                return false;
            

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
        public async Task<bool> ResValidAsync(DateTime date) {
            if (this._conn?.State == ConnectionState.Open) {
                DbCommand cmdResValid = this._conn.CreateCommand();
                cmdResValid.CommandText = "select count(*) from reservation where Uhrzeit between @time1 and @time2 and datum = @dat;";

                DateTime date2 = date.AddHours(-1);
                date = date.AddHours(1);

                DbParameter paramTStart = cmdResValid.CreateParameter();
                paramTStart.ParameterName = "time1";
                paramTStart.DbType = DbType.Time;
                paramTStart.Value = date2.TimeOfDay;

                DbParameter paramTEnd = cmdResValid.CreateParameter();
                paramTEnd.ParameterName = "time2";
                paramTEnd.DbType = DbType.Time;
                paramTEnd.Value = date.TimeOfDay;

                DbParameter paramDate = cmdResValid.CreateParameter();
                paramDate.ParameterName = "dat";
                paramDate.DbType = DbType.Date;
                paramDate.Value = date.Date;

                cmdResValid.Parameters.Add(paramTStart);
                cmdResValid.Parameters.Add(paramTEnd);
                cmdResValid.Parameters.Add(paramDate);


                int count = Convert.ToInt32(await cmdResValid.ExecuteScalarAsync());
                return count <2;
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
                paramUH.DbType = DbType.Time;
                paramUH.Value = r.uhrzeit.TimeOfDay;

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

        public async Task<bool> verifyUserByEmailAsync(string email) {

            if (this._conn?.State == ConnectionState.Open) {
                DbCommand cmdUser = this._conn.CreateCommand();

                cmdUser.CommandText = "select * from customer where email = @em;";

                DbParameter paramEM = cmdUser.CreateParameter();
                paramEM.ParameterName = "em";
                paramEM.DbType = DbType.String;
                paramEM.Value = email;

                cmdUser.Parameters.Add(paramEM);
                using (DbDataReader reader = await cmdUser.ExecuteReaderAsync()) {
                    while (await reader.ReadAsync()) {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
