using System;
using System.Collections.Generic;
using System.Text;

namespace OPERATION_MNS.Utilities.Constants
{
    public class CommonConstants
    {
        public class AppRole
        {
            public const string AdminRole = "Admin";
        }
        public class UserClaims
        {
            public const string Roles = "Roles";
            public const string Department = "Deparment";
        }

        public const string Add_Action = "Add";
        public const string Edit_Action = "Edit";
        public const string Delete_Action = "Delete";
        public const string ConflictObjectResult_Msg = "Object has existed!";
        public const string NotFoundObjectResult_Msg = "Not Found Object";
        public const string InvalidParam = "Tham số không hợp lệ";
        public const string IsDelete = "Y";

        public const string DefaultAvatar = "/img/profiles/avatar-02.jpg";
        public const string NotFound = "NotFound";
        public const string Error404 = "Error404";

        public const string sTrue = "1";
        public const string sFalse = "0";
        public const string Y = "Y";
        public const string N = "N";

        public const string No_Approved = "N";
        public const string Approved = "Y";
        public const string Request = "R";

        public const string CHIP = "CHIP";
        public const string WAFER = "WAFER";
        public const string FAB_WLP1 = "FAB_WLP1";
        public const string FAB_WLP2 = "FAB_WLP2";
        public static string LEAD_TIME_PLAN = "6";

        public const string ON = "ON";
        public const string OFF = "OFF";
    }
}
