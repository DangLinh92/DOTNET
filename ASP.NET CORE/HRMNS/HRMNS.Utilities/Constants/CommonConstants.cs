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
        public const string IMPORT_CHUCVU2_EMP = "chucvu2_emp";

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

        public const string DM_OT = "5";
        public const string DM_ELLC = "6";
        public const string OT_BAT_MAY = "DSBM_OT_150";
        public static string[] arrNghiPhep = { "AL", "UL", "NL", "L70", "SL", "CT", "HL", "NH", "EL", "LC", "T","IL","NB","B" };
        public static string[] arrNVBlock10p =
        {
            "H2202060",
            "H1511001",
            "H2206002",
            "H2205001",
            "H2203071",
            "H1602007",
            "H2001001",
            "H2011038",
            "H2108005",
            "H1504002",
            "H1503001",
            "H1503004",
            "H1512025",
            "H1604008",
            "H1512019",
            "H1607009",
            "H1702004",
            "H1708035",
            "H1804009",
            "H1907001",
            "H2009013",
            "H2009045",
            "H2010031",
            "H2011026",
            "H2012010",
            "H2101001",
            "H2102003",
            "H2103028",
            "H2103044",
            "H2104001",
            "H2105001",
            "H2107060",
            "H2109002",
            "H2110021",
            "H2111008",
            "H2111013",
            "H2203072",
            "H2206003",
            "H2206004",
            "H2208002",
            "H2208004",
            "H2209003",
            "H1605004",
            "H1810005",
            "H1703009",
            "H2003040",
            "H1909007",
            "H1603011",
            "H1704008",
            "H2102026",
            "H2104032"
        };

        public static string[] arrSatudayAbNormal =
        {
            "2022-10-01",
            "2022-10-08",
            "2022-10-15",
            "2022-10-22"
        };

        public static string COMPLETED = "Completed";
        public static string INPROGRESS = "Inprogress";
        public static string TODO = "TODO";
        public static string PENDING = "Pending";
    }
}
