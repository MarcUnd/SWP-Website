using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LuigiWebsite.Models.DB {
    interface IRepositoryDB {

        void Connect();

        void Disconnect();

        List<MenuDB> getMenu();
    }
}
