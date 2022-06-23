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

        public const string IMPORT_BASIC_EMP = "basic_emp";
        public const string IMPORT_DETAIL_EMP = "detail_emp";

        // Hop Dong
        public const string HD_THUVIEC_EM = "TV85";
        public const string HD_THUVIEC_OP = "TV100";
        public const string HD_MOT_NAM_L1 = "A_YEAR1";
        public const string HD_MOT_NAM_L2 = "A_YEAR2";
        public const string HD_KHONG_THOIHAN = "NO_LIMIT";

        public const string CA_NGAY = "CN_WHC";// Ca ngày/ 주간
        public const string CA_DEM = "CD_WHC"; // Ca đêm/ 야간

        public const string BEGIN_CN = "08:00:00";
        public const string END_CN_1 = "17:00:00";
        public const string END_CN_2 = "17:15:00";

        public const string BEGIN_CD = "20:00:00";
        public const string END_CD = "05:00:00";

        public const string ZERO_TIME = "00:00:00";

        public const string roleApprove1 = "GrLeader";
        public const string roleApprove2 = "KRManager";
        public const string roleApprove3 = "HR";

        public const string AssLeader_Role = "AssLeader";

        public const string NgayLe = "NL";
        public const string NgayLeCuoiCung = "NLCC";
        public const string TruocNgayLe = "TNL";
        public const string ChuNhat = "CN";
        public const string NgayThuong = "NT";

        public const string SUPPORT_DEPT = "SP";
        public const string IN = "IN";
        public const string OUT = "OUT";

        public const string VP = "VP";
        public const string SX = "SX";
    }
}
