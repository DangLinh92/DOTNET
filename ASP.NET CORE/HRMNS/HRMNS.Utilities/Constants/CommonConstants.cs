using System;
using System.Collections.Generic;
using System.Text;

namespace HRMNS.Utilities.Constants
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
        }

        public const string Add_Action = "Add";
        public const string Edit_Action = "Edit";
        public const string Delete_Action = "Delete";
        public const string ConflictObjectResult_Msg = "Object has existed!";
        public const string NotFoundObjectResult_Msg = "Not Found Object";
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

        public const string IMPORT_BASIC_EMP = "basic_emp";
        public const string IMPORT_DETAIL_EMP = "detail_emp";
    }
}
