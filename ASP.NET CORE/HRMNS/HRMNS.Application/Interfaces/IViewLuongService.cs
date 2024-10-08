using HRMNS.Data.Entities.luong;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMNS.Application.Interfaces
{
    public interface IViewLuongService
    {
        USER_VIEW_LUONG GetUserByName (string name);
        bool Login(string userName, string password);
        bool ChangePassword(string userName, string password,string newpassword);
        bool Logout();
        void Save();
    }
}
