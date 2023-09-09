using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHS_Weebhook_Service
{
    public class SQLService
    {
        private string _connectionString = "";
        public SQLService(string connect)
        {
            _connectionString = connect;
        }
        public ResultDB ExecProceduce2(string ProcName, Dictionary<string, string> Dictionary)
        {
            try
            {
                LogFile.WriteErrorLog("ExecProceduce2");
                using (var con = new SqlConnection(_connectionString))
                {
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

                    SqlParameter N_RETURN = new SqlParameter("@N_RETURN", SqlDbType.Int);
                    N_RETURN.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(N_RETURN);

                    SqlParameter V_RETURN = new SqlParameter("@V_RETURN", SqlDbType.NVarChar, 4000);
                    V_RETURN.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(V_RETURN);

                    cmd.CommandTimeout = 3600;
                    da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    con.Close();
                    resultDb.ReturnDataSet = ds;
                    resultDb.ReturnInt = (int)N_RETURN.Value;
                    resultDb.ReturnString = (string)V_RETURN.Value;
                    return resultDb;
                }

                LogFile.WriteErrorLog("ExecProceduce3");

            }
            catch (Exception ex)
            {
                LogFile.WriteErrorLog(ex);
                ResultDB result = new ResultDB();
                result.ReturnInt = -1;
                result.ReturnString = "서버 연결이 불가능합니다. Không kết nối được đến máy chủ.";
                return result;
            }
        }

        public ResultDB ExecuteDB(string ProcName, Dictionary<string, string> Dictionary, string tableParam, DataTable table)
        {
            try
            {
                SqlConnection connection = new SqlConnection(_connectionString);

                connection.Open();
                ResultDB resultDb = new ResultDB();
                SqlCommand cmd = new SqlCommand();
                SqlDataAdapter da = new SqlDataAdapter();
                DataSet ds = new DataSet();
                cmd = new SqlCommand(ProcName.Replace('.', '@'), connection);
                cmd.CommandType = CommandType.StoredProcedure;

                foreach (KeyValuePair<string, string> item in Dictionary)
                {
                    cmd.Parameters.AddWithValue(item.Key, item.Value);
                }

                if (table != null && tableParam != "")
                {
                    SqlParameter sqlTableParam = cmd.Parameters.AddWithValue(tableParam, table);
                    sqlTableParam.SqlDbType = SqlDbType.Structured;
                }

                SqlParameter N_RETURN = new SqlParameter("@N_RETURN", SqlDbType.Int);
                N_RETURN.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(N_RETURN);

                SqlParameter V_RETURN = new SqlParameter("@V_RETURN", SqlDbType.NVarChar, 100);
                V_RETURN.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(V_RETURN);

                da = new SqlDataAdapter(cmd);
                da.SelectCommand.CommandTimeout = 3600;// 1h
                da.Fill(ds);

                connection.Close();
                resultDb.ReturnDataSet = ds;
                resultDb.ReturnInt = (int)N_RETURN.Value;
                resultDb.ReturnString = (string)V_RETURN.Value;
                return resultDb;
            }
            catch (Exception ex)
            {
                //LogFile.WriteErrorLog(ex);
                return null;
            }
        }
    }
}
