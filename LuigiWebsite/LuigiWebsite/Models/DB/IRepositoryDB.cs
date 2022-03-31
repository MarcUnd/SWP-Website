using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LuigiWebsite.Models.DB {
    interface IRepositoryDB {

        Task ConnectAsync();
        Task DisconnectAsync();

        Task<List<MenuDB>> GetMenuAsync();

        Task<bool> InsertAsync(user u);

        Task<bool> isUserAsync(string email, string password);
        Task<bool> ResValidAsync(DateTime date);
        String getEmailById(int id);
        Task<bool> InsertResAsync(reservation r);
    }
}
