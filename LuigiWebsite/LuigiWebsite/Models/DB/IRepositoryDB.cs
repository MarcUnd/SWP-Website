using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LuigiWebsite.Models.DB {
    interface IRepositoryDB {
        Task<bool> DeleteAsync(int id);
        Task ConnectAsync();
        Task DisconnectAsync();

        Task<List<MenuDB>> GetMenuAsync();

        Task<bool> InsertAsync(user u);

        Task<bool> isUserAsync(string email, string password);
        Task<bool> InsertResAsync(reservation r);
        Task<bool> ResValidAsync(DateTime dat);

        Task<user> getUserByEmailAsync(string email);

        Task<List<reservation>> getReservationsByEmailAsync(string email);
        Task<bool> verifyUserByEmailAsync(string email);
    }

}
