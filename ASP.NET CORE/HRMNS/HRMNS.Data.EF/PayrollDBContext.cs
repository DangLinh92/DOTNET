using HRMNS.Data.EF.Configurations;
using HRMNS.Data.EF.Extensions;
using HRMNS.Data.Entities.Payroll;
using HRMNS.Data.Interfaces;
using HRMNS.Utilities.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace HRMNS.Data.EF
{
    //public class PayrollDBContext : DbContext
    //{
    //    public IHttpContextAccessor _httpContextAccessor;
    //    public PayrollDBContext(DbContextOptions<PayrollDBContext> options,IHttpContextAccessor httpContextAccessor) : base(options)
    //    {
    //        _httpContextAccessor = httpContextAccessor;
    //    }

    //    public virtual DbSet<HR_SALARY_PR> HR_SALARY_PR { get; set; }
    //    public virtual DbSet<BANGLUONGCHITIET_HISTORY_PR> BANGLUONGCHITIET_HISTORY_PR { get; set; }
    //    public virtual DbSet<HR_SALARY_PHATSINH_PR> HR_SALARY_PHATSINH_PR { get; set; }

    //    protected override void OnModelCreating(ModelBuilder builder)
    //    {
    //        builder.AddConfiguration(new HrSalaryPRConfiguration());
    //        builder.AddConfiguration(new HrBangCongHistoryPrConfiguration());
    //        builder.AddConfiguration(new HrSalaryPhatSinhPRConfiguration());
    //    }

    //    public override int SaveChanges()
    //    {
    //        var modified = ChangeTracker.Entries().Where(e => e.State == EntityState.Modified || e.State == EntityState.Added);
    //        foreach (EntityEntry item in modified)
    //        {
    //            var changeOrAddedItem = item.Entity as IDateTracking;
    //            if (changeOrAddedItem != null)
    //            {
    //                if (item.State == EntityState.Added)
    //                {
    //                    changeOrAddedItem.DateCreated = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

    //                    if (_httpContextAccessor.HttpContext != null && changeOrAddedItem.UserCreated.NullString() == "")
    //                        changeOrAddedItem.UserCreated = _httpContextAccessor.HttpContext.User?.Identity?.Name;
    //                }

    //                changeOrAddedItem.DateModified = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

    //                if (_httpContextAccessor.HttpContext != null && changeOrAddedItem.UserModified.NullString() == "")
    //                    changeOrAddedItem.UserModified = _httpContextAccessor.HttpContext.User?.Identity?.Name;
    //            }
    //        }
    //        return base.SaveChanges();
    //    }

    //    public ResultDB ExecProceduce(string ProcName, Dictionary<string, string> Dictionary, string tableParam, DataTable table)
    //    {
    //        try
    //        {
    //            string connString = ((Microsoft.Data.SqlClient.SqlConnection)Database.GetDbConnection()).ConnectionString;

    //            using var con = new SqlConnection(connString);
    //            ResultDB resultDb = new ResultDB();
    //            DataSet dataSet = new DataSet();
    //            SqlCommand cmd = new SqlCommand();
    //            SqlDataAdapter da = new SqlDataAdapter();
    //            DataSet ds = new DataSet();

    //            cmd = new SqlCommand(ProcName.Replace('.', '@'), con);
    //            cmd.CommandType = CommandType.StoredProcedure;

    //            foreach (KeyValuePair<string, string> item in Dictionary)
    //            {
    //                cmd.Parameters.AddWithValue(item.Key, item.Value);
    //            }

    //            if (tableParam != "" && table != null)
    //            {
    //                SqlParameter sqlTableParam = cmd.Parameters.AddWithValue(tableParam, table);
    //                sqlTableParam.SqlDbType = SqlDbType.Structured;
    //            }

    //            SqlParameter N_RETURN = new SqlParameter("@N_RETURN", SqlDbType.Int);
    //            N_RETURN.Direction = ParameterDirection.Output;
    //            cmd.Parameters.Add(N_RETURN);

    //            SqlParameter V_RETURN = new SqlParameter("@V_RETURN", SqlDbType.NVarChar, 4000);
    //            V_RETURN.Direction = ParameterDirection.Output;
    //            cmd.Parameters.Add(V_RETURN);

    //            da = new SqlDataAdapter(cmd);
    //            da.Fill(ds);
    //            con.Close();
    //            resultDb.ReturnDataSet = ds;
    //            resultDb.ReturnInt = (int)N_RETURN.Value;
    //            resultDb.ReturnString = (string)V_RETURN.Value;
    //            return resultDb;
    //        }
    //        catch (Exception)
    //        {
    //            ResultDB result = new ResultDB();
    //            result.ReturnInt = -1;
    //            result.ReturnString = "서버 연결이 불가능합니다. Không kết nối được đến máy chủ.";
    //            return result;
    //        }
    //    }
    //}
}
