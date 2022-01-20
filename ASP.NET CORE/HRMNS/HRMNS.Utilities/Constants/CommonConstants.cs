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
    }
}
