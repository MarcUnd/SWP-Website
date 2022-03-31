﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LuigiWebsite.Models.DB {
    interface IRepositoryDB {

        void Connect();

        void Disconnect();

        List<MenuDB> getMenu();

        bool Insert(user u);

        bool isUser(string email, string password);

        bool InsertRes(reservation r);

        String getEmailById(int id);
    }
}
