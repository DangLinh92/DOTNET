using HRMNS.Data.EF.Extensions;
using HRMNS.Utilities.Dtos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;

namespace HRMNS.Data.EF
{
    public class BioStarDBContext: DbContext
    {
        public BioStarDBContext(DbContextOptions<BioStarDBContext> options) : base(options)
        {

        }

        public ResultDB GetChamCongLogData(string fromTime, string toTime)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("A_BEGIN_TIME", fromTime.NullString());
            dic.Add("A_END_TIME", toTime.NullString());
           return ExecProceduce("PKG_BUSINESS.GET_LOG_DATA_HRMS", dic, "", null);
        }

        public ResultDB GetChamCongAbsenceLogData(string fromTime, string toTime,string dept)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("A_BEGIN_TIME", fromTime.NullString());
            dic.Add("A_END_TIME", toTime.NullString());
            dic.Add("A_DEPT", dept.NullString());
            return ExecProceduce("PKG_BUSINESS.GET_LOG_DATA_ABSENCE_HRMS", dic, "", null);
        }

        public ResultDB GetDeparment()
        {
            return ExecProceduce("PKG_BUSINESS.GET_DEPT", new Dictionary<string, string>(), "", null);
        }

        public ResultDB ShowChamCongLogInDay(string userId,string time)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("A_USER_ID", userId.NullString());
            dic.Add("A_TIME", time.NullString());
            return ExecProceduce("PKG_BUSINESS.GET_LOG_IN_DAY", dic, "", null);
        }

        public ResultDB GetAllNVInfo()
        {
            return ExecProceduce("[PKG_BUSINESS.GET_EMP_ALL_INFO]", new Dictionary<string, string>(), "", null);
        }

        public ResultDB ExecProceduce(string ProcName, Dictionary<string, string> Dictionary, string tableParam, DataTable table)
        {
            try
            {
                string connString = ((Microsoft.Data.SqlClient.SqlConnection)Database.GetDbConnection()).ConnectionString;

                using var con = new SqlConnection(connString);
                ResultDB resultDb = new ResultDB();
                DataSet dataSet = new DataSet();
                SqlCommand cmd = new SqlCommand();
                SqlDataAdapter da = new SqlDataAdapter();
                DataSet ds = new DataSet();

                cmd = new SqlCommand(ProcName.Replace('.', '@'), con);
                cmd.CommandType = CommandType.StoredProcedure;

                foreach (KeyValuePair<string, string> item in Dictionary)
                {
                    cmd.Parameters.AddWithValue(item.Key, item.Value);
                }

                if (tableParam != "" && table != null)
                {
                    SqlParameter sqlTableParam = cmd.Parameters.AddWithValue(tableParam, table);
                    sqlTableParam.SqlDbType = SqlDbType.Structured;
                }

                SqlParameter N_RETURN = new SqlParameter("@N_RETURN", SqlDbType.Int);
                N_RETURN.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(N_RETURN);

                SqlParameter V_RETURN = new SqlParameter("@V_RETURN", SqlDbType.NVarChar, 4000);
                V_RETURN.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(V_RETURN);

                da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                con.Close();
                resultDb.ReturnDataSet = ds;
                resultDb.ReturnInt = (int)N_RETURN.Value;
                resultDb.ReturnString = (string)V_RETURN.Value;
                return resultDb;
            }
            catch (Exception)
            {
                ResultDB result = new ResultDB();
                result.ReturnInt = -1;
                result.ReturnString = "서버 연결이 불가능합니다. Không kết nối được đến máy chủ.";
                return result;
            }
        }
    }
}
