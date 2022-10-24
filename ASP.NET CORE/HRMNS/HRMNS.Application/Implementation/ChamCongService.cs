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
using System.Globalization;
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
        private string Department { get; set; }

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
            var lst = _chamCongLogRepository.FindAll(x => string.Compare(x.Ngay_ChamCong, lastMonth) >= 0).OrderByDescending(x => x.Ngay_ChamCong);
            var results = _mapper.Map<List<ChamCongLogViewModel>>(lst);

            return results;
        }

        public ResultDB ImportExcel(string filePath, DataTable employees)
        {
            ResultDB resultDB = new ResultDB();
            try
            {
                using (var packet = new ExcelPackage(new System.IO.FileInfo(filePath)))
                {
                    ExcelWorksheet worksheet = packet.Workbook.Worksheets[1];

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
                    string dept = "";
                    string firstTime = "";
                    string lastTime = "";
                    string userId = "";
                    for (int i = worksheet.Dimension.Start.Row + 1; i <= worksheet.Dimension.End.Row; i++)
                    {
                        row = table.NewRow();

                        firstTime = worksheet.Cells[i, 4].Text.NullString();
                        lastTime = worksheet.Cells[i, 5].Text.NullString();
                        userId = worksheet.Cells[i, 2].Text.NullString();

                        if (userId.ToUpper().StartsWith("H"))
                        {
                            userId = userId.ToUpper().Replace("H", "");
                        }

                        if (string.IsNullOrEmpty(userId)
                            || string.IsNullOrEmpty(worksheet.Cells[i, 1].Text.NullString())
                            || string.IsNullOrEmpty(firstTime)
                            || string.IsNullOrEmpty(lastTime))
                        {
                            continue;
                        }

                        TimeSpan fdateTime = TimeSpan.Parse(firstTime);
                        TimeSpan ldateTime = TimeSpan.Parse(lastTime);
                        DateTime dateCheck = DateTime.Parse(worksheet.Cells[i, 1].Text.NullString());

                        foreach (DataRow em in employees.Rows)
                        {
                            if (worksheet.Cells[i, 2].Text.NullString().Contains(em["sUserID"].NullString()))
                            {
                                dept = em["sDepartment"].NullString();
                                break;
                            }
                        }

                        row["Date_Check"] = dateCheck.ToString("yyyy-MM-dd");
                        row["userId"] = userId;
                        row["userName"] = worksheet.Cells[i, 3].Text.NullString();
                        row["Department"] = dept;
                        row["Shift_"] = "";
                        row["Daily_Schedule"] = "";
                        row["First_In_Time"] = fdateTime.ToString(@"hh\:mm\:ss");
                        row["Last_Out_Time"] = ldateTime.ToString(@"hh\:mm\:ss");
                        row["Result"] = "";
                        row["First_In"] = "IN";
                        row["Last_Out"] = "OUT";
                        row["HanhChinh"] = "";
                        row["TangCa"] = "";
                        row["BreakTime"] = "";
                        row["WorkTime"] = "";
                        table.Rows.Add(row);
                    }

                    Dictionary<string, string> param = new Dictionary<string, string>();
                    param.Add("A_USER_HANDLE", "Y");
                    param.Add("A_USER_UPDATE", GetUserId());
                    resultDB = _chamCongLogRepository.ExecProceduce("PKG_BUSINESS.PUT_EVENT_LOG_BY_EXCEL", param, "A_DATA", table);
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

        public string GetMaxDate()
        {
            return _chamCongLogRepository.GetMaxDate(x => x.Ngay_ChamCong);
        }

        public ResultDB InsertLogData(DataTable data)
        {
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("A_USER_HANDLE", "N");
            param.Add("A_USER_UPDATE", "sys");
            return _chamCongLogRepository.ExecProceduce("PKG_BUSINESS.PUT_EVENT_LOG", param, "A_DATA", data);
        }

        public List<ChamCongLogViewModel> Search(string result, string dept, ref string timeFrom1, ref string timeTo1)
        {
            string timeFrom, timeTo;

            if (string.IsNullOrEmpty(timeFrom1.NullString()) || string.IsNullOrEmpty(timeTo1))
            {
                timeFrom1 = DateTime.Now.AddDays(-5).ToString("yyyy-MM-dd");
                timeTo1 = DateTime.Now.ToString("yyyy-MM-dd");
            }

            timeFrom = timeFrom1;
            timeTo = timeTo1;

            if (string.IsNullOrEmpty(dept))
            {
                if (!string.IsNullOrEmpty(timeFrom) && !string.IsNullOrEmpty(timeTo))
                {
                    var lst = _chamCongLogRepository.FindAll(x => string.Compare(x.Ngay_ChamCong, timeFrom) >= 0 && string.Compare(x.Ngay_ChamCong, timeTo) <= 0).OrderByDescending(x => x.Ngay_ChamCong);
                    var vm = _mapper.Map<List<ChamCongLogViewModel>>(lst);
                    if (!string.IsNullOrEmpty(result))
                    {
                        vm = vm.Where(x => x.Result.Contains(result)).ToList();
                    }

                    return vm;
                }
                else if (string.IsNullOrEmpty(timeFrom) && string.IsNullOrEmpty(timeTo))
                {
                    var vm = GetAll("");
                    if (!string.IsNullOrEmpty(result))
                    {
                        vm = vm.Where(x => x.Result.Contains(result)).ToList();
                    }

                    return vm;
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(timeFrom) && !string.IsNullOrEmpty(timeTo))
                {
                    var lstCC = _chamCongLogRepository.FindAll(x => string.Compare(x.Ngay_ChamCong, timeFrom) >= 0 && string.Compare(x.Ngay_ChamCong, timeTo) <= 0).OrderByDescending(x => x.Ngay_ChamCong).ToList();
                    var lst = lstCC.Where(x => x.Department.Contains(dept)).OrderByDescending(x => x.Ngay_ChamCong).ToList();

                    var lstNV = _nhanvienRepository.FindAll(x => x.MaBoPhan == Department);
                    if (lstNV.Count() > 0)
                    {
                        foreach (var item in lstNV)
                        {
                            if (!lst.Any(x => item.Id.ToUpper() == "H" + x.ID_NV))
                            {
                                var nv = lstCC.Where(x => item.Id.ToUpper() == "H" + x.ID_NV).OrderByDescending(x => x.Ngay_ChamCong).ToList();
                                lst.AddRange(nv);
                            }
                        }
                    }

                    var vm = _mapper.Map<List<ChamCongLogViewModel>>(lst);

                    if (!string.IsNullOrEmpty(result))
                    {
                        vm = vm.Where(x => x.Result.Contains(result)).ToList();
                    }

                    return vm;
                }
                else if (string.IsNullOrEmpty(timeFrom) && string.IsNullOrEmpty(timeTo))
                {
                    var lst = GetAll("");
                    var vm = lst.FindAll(x => x.Department.Contains(dept));

                    var lstNV = _nhanvienRepository.FindAll(x => x.MaBoPhan == Department);
                    if (lstNV.Count() > 0)
                    {
                        foreach (var item in lstNV)
                        {
                            if (!vm.Any(x => item.Id.ToUpper() == "H" + x.ID_NV))
                            {
                                vm.AddRange(lst.FindAll(x => item.Id.ToUpper() == "H" + x.ID_NV));
                            }
                        }
                    }


                    if (!string.IsNullOrEmpty(result))
                    {
                        vm = vm.Where(x => x.Result.Contains(result)).ToList();
                    }

                    return vm;
                }
            }
            return new List<ChamCongLogViewModel>();
        }

        public ChamCongLogViewModel Update(ChamCongLogViewModel model)
        {
            var entity = _chamCongLogRepository.FindAll(x => x.ID_NV == model.ID_NV && x.Ngay_ChamCong == model.Ngay_ChamCong).FirstOrDefault();
            var isUserHandle = entity.UserHandle == "Y";

            DateTime ngayCham = DateTime.Parse(model.Ngay_ChamCong);
            string preday = ngayCham.AddDays(-1).ToString("yyyy-MM-dd");
            var entityPreday = _chamCongLogRepository.FindAll(x => x.ID_NV == model.ID_NV && x.Ngay_ChamCong == preday).FirstOrDefault();
            string ex = "";
            if (entity != null)
            {
                if (entity.FirstIn_Time != model.FirstIn_Time)
                {
                    entity.FirstIn_Time = model.FirstIn_Time;
                    entity.FirstIn = "IN";
                    ex = "-IN";
                }

                if (entity.Last_Out_Time != model.Last_Out_Time)
                {
                    entity.Last_Out_Time = model.Last_Out_Time;
                    entity.LastOut = "OUT";
                    ex += "-OUT";
                }

                if (model.FirstIn_Time == "00:00:00")
                {
                    entity.FirstIn = "";
                }

                if (model.Last_Out_Time == "00:00:00")
                {
                    entity.LastOut = "";
                }

                if (entity.FirstIn_Time == "00:00:00" && entity.Last_Out_Time == "00:00:00")
                {
                    entity.UserHandle = "N";
                }
                else
                {
                    entity.UserHandle = "Y";
                }

                if (entityPreday != null)
                {
                    entityPreday.Last_Out_Time_Update = entity.Last_Out_Time;
                }

                entity.UserModified = model.UserModified + ex;

                if (entity.UserHandle == "Y" && !isUserHandle)
                {
                    var lst = _chamCongLogRepository.FindAll(x => x.ID_NV == model.ID_NV && x.Ngay_ChamCong.Substring(0, 7) == model.Ngay_ChamCong.Substring(0, 7));
                    int count = lst.Where(x=>x.UserHandle == "Y").Count();
                    foreach (var item in lst)
                    {
                        if(item.Ngay_ChamCong != entity.Ngay_ChamCong && item.Ngay_ChamCong != entityPreday?.Ngay_ChamCong)
                        {
                            item.NumberUpdated = count + 1;
                            _chamCongLogRepository.Update(item);
                        }
                    }
                }

                if (entity.UserHandle == "Y" && !isUserHandle)
                {
                    entity.NumberUpdated += 1;

                    if(entity.Ngay_ChamCong.Substring(0,7) == entityPreday.Ngay_ChamCong.Substring(0, 7))
                    {
                        entityPreday.NumberUpdated = entity.NumberUpdated;
                    }
                }

                 _chamCongLogRepository.Update(entity);

                if (entityPreday != null)
                    _chamCongLogRepository.Update(entityPreday);
            }
            return model;
        }

        public void SetDepartment(string dept)
        {
            Department = dept;
        }

        public List<ChamCongLogViewModel> GetByTime(string fromTime, string toTime)
        {
            var lst = _chamCongLogRepository.FindAll(x => string.Compare(x.Ngay_ChamCong, fromTime) >= 0 && string.Compare(x.Ngay_ChamCong, toTime) <= 0).OrderByDescending(x => x.Ngay_ChamCong);
            var results = _mapper.Map<List<ChamCongLogViewModel>>(lst);

            return results;
        }

        public ResultDB GetLogDataCurrentDay()
        {
            Dictionary<string, string> param = new Dictionary<string, string>();
            return _chamCongLogRepository.ExecProceduce2("PKG_BUSINESS.AUTO_PUT_EVENT_LOG_2", param);
        }
    }
}
