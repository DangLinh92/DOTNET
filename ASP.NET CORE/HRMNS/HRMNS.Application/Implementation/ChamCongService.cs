using AutoMapper;
using HRMNS.Application.Interfaces;
using HRMNS.Application.ViewModels.Time_Attendance;
using HRMNS.Data.EF;
using HRMNS.Data.EF.Extensions;
using HRMNS.Data.Entities;
using HRMNS.Utilities.Dtos;
using HRMS.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMNS.Application.Implementation
{
    public class ChamCongService : BaseService, IChamCongService
    {
        private IRespository<CHAM_CONG_LOG, long> _chamCongLogRepository;
        private IRespository<HR_NHANVIEN, string> _nhanvienRepository;
        private IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ChamCongService(IRespository<CHAM_CONG_LOG, long> chamCongLogRepository, IRespository<HR_NHANVIEN, string> nhanvienRepository, IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _chamCongLogRepository = chamCongLogRepository;
            _nhanvienRepository = nhanvienRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public List<ChamCongLogViewModel> GetAll(string keyword)
        {
            string lastMonth = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd");
            var lst = _chamCongLogRepository.FindAll(x => string.Compare(x.Ngay_ChamCong, lastMonth) > 0).OrderByDescending(x=>x.DateModified);
            return _mapper.Map<List<ChamCongLogViewModel>>(lst);
        }

        public ResultDB ImportExcel(string filePath, string param)
        {
            ResultDB resultDB = new ResultDB();
            try
            {
                using (var packet = new ExcelPackage(new System.IO.FileInfo(filePath)))
                {
                    ExcelWorksheet worksheet = packet.Workbook.Worksheets[1];
                    List<CHAM_CONG_LOG> lstChamCongLog = new List<CHAM_CONG_LOG>();
                    CHAM_CONG_LOG log;

                    DataTable table = new DataTable();
                    table.Columns.Add("Date_Check");
                    table.Columns.Add("userId");
                    table.Columns.Add("userName");
                    table.Columns.Add("Department");
                    table.Columns.Add("Shift_");
                    table.Columns.Add("Daily_Schedule");
                    table.Columns.Add("First_In_Time");
                    table.Columns.Add("Last_Out_Time");
                    table.Columns.Add("Result");
                    table.Columns.Add("First_In");
                    table.Columns.Add("Last_Out");
                    table.Columns.Add("HanhChinh");
                    table.Columns.Add("TangCa");
                    table.Columns.Add("BreakTime");
                    table.Columns.Add("WorkTime");

                    DataRow row = null;
                    for (int i = worksheet.Dimension.Start.Row + 1; i <= worksheet.Dimension.End.Row; i++)
                    {
                        row = table.NewRow();
                        log = new CHAM_CONG_LOG();
                        log.ID_NV = worksheet.Cells[i, 2].Text.NullString();

                        if (string.IsNullOrEmpty(log.ID_NV))
                        {
                            continue;
                        }

                        HR_NHANVIEN nv = _nhanvienRepository.FindAll(x => x.Id.Contains(log.ID_NV)).ToList().FirstOrDefault();

                        log.Department = nv?.MaBoPhan;
                        log.Ngay_ChamCong = worksheet.Cells[i, 1].Text.NullString();
                        log.Ten_NV = worksheet.Cells[i, 3].Text.NullString();
                        log.FirstIn_Time = worksheet.Cells[i, 7].Text.NullString();
                        log.Last_Out_Time = worksheet.Cells[i, 8].Text.NullString();
                        log.FirstIn = worksheet.Cells[i, 10].Text.NullString();
                        log.LastOut = worksheet.Cells[i, 11].Text.NullString();
                        log.UserCreated = GetUserId();
                        log.UserModified = GetUserId();
                        lstChamCongLog.Add(log);

                        row["Date_Check"] = log.Ngay_ChamCong;
                        row["userId"] = log.ID_NV;
                        row["userName"] = log.Ten_NV;
                        row["Department"] = (nv?.MaBoPhan) ?? worksheet.Cells[i, 4].Text.NullString().Remove(0,6).Split('/')[0].NullString();
                        row["Shift_"] = "";
                        row["Daily_Schedule"] = "";
                        row["First_In_Time"] = log.FirstIn_Time;
                        row["Last_Out_Time"] = log.Last_Out_Time;
                        row["Result"] = "";
                        row["First_In"] = log.FirstIn;
                        row["Last_Out"] = log.LastOut;
                        row["HanhChinh"] = "";
                        row["TangCa"] = "";
                        row["BreakTime"] = "";
                        row["WorkTime"] = "";
                        table.Rows.Add(row);
                    }

                    resultDB = _chamCongLogRepository.ExecProceduce("PKG_BUSINESS.PUT_EVENT_LOG", new Dictionary<string, string>(), "A_DATA", table);
                }
                return resultDB;
            }
            catch (Exception ex)
            {
                resultDB.ReturnInt = -1;
                resultDB.ReturnString = ex.Message;
                return resultDB;
            }
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public List<ChamCongLogViewModel> Search(string condition, string param)
        {
            throw new NotImplementedException();
        }
    }
}
