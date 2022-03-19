using AutoMapper;
using HRMNS.Application.Interfaces;
using HRMNS.Application.ViewModels.System;
using HRMNS.Application.ViewModels.Time_Attendance;
using HRMNS.Data.EF;
using HRMNS.Data.Entities;
using HRMS.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Text;
using HRMNS.Utilities.Dtos;
using System.Data;
using HRMNS.Data.EF.Extensions;

namespace HRMNS.Application.Implementation
{
    public class BangCongService : BaseService, IBangCongService
    {
        private IRespository<ATTENDANCE_RECORD, long> _attendanceRecordRespository;
        private IRespository<ATTENDANCE_OVERTIME, int> _attendanceOvertimeRespository;
        private IRespository<SETTING_TIME_CA_LVIEC, int> _settingTimeCalamviecRespository;
        private IRespository<NHANVIEN_CALAMVIEC, int> _nhanvienCalamviecRespository;
        private IRespository<DM_CA_LVIEC, string> _danhmucCalamviecRespository;
        private IRespository<CA_LVIEC, int> _calaviecRespository;
        private IRespository<CHAM_CONG_LOG, long> _chamCongRespository;

        private EFUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BangCongService(IRespository<ATTENDANCE_RECORD, long> attendance_record, IRespository<ATTENDANCE_OVERTIME, int> attendance_overtime, IRespository<CHAM_CONG_LOG, long> chamCongRespository, IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _attendanceRecordRespository = attendance_record;
            _attendanceOvertimeRespository = attendance_overtime;
            _chamCongRespository = chamCongRespository;
            _unitOfWork = (EFUnitOfWork)unitOfWork;

            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public AttendanceRecordViewModel Add(AttendanceRecordViewModel attendance)
        {
            attendance.UserCreated = GetUserId();
            var entity = _mapper.Map<ATTENDANCE_RECORD>(attendance);
            _attendanceRecordRespository.Add(entity);
            return attendance;
        }

        public AttendanceOvertimeViewModel AddOverTime(AttendanceOvertimeViewModel attendance)
        {
            attendance.UserCreated = GetUserId();
            var entity = _mapper.Map<ATTENDANCE_OVERTIME>(attendance);
            _attendanceOvertimeRespository.Add(entity);
            return attendance;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public List<AttendanceRecordViewModel> GetAll(params Expression<Func<ATTENDANCE_RECORD, object>>[] includeProperties)
        {
            throw new NotImplementedException();
        }

        public List<AttendanceOvertimeViewModel> GetAllAttendanceOT(params Expression<Func<ATTENDANCE_OVERTIME, object>>[] includeProperties)
        {
            throw new NotImplementedException();
        }

        public List<ChamCongDataViewModel> GetDataReport(string time)
        {
            var lstResult = new List<ChamCongDataViewModel>();
            var hsoOvertimes = _mapper.Map<List<CaLamViecViewModel>>(_calaviecRespository.FindAll());
            List<string> lstDanhMucOT = new List<string>();
            foreach (var item in hsoOvertimes)
            {
                if (item.HeSo_OT.NullString() != "" && !lstDanhMucOT.Contains(item.HeSo_OT.NullString()))
                {
                    lstDanhMucOT.Add(item.HeSo_OT.NullString() + "%");
                }
            }

            string beginMonth = "";
            string endMonth = "";
            if(DateTime.TryParse(time,out DateTime _dateTimeIn))
            {
                beginMonth = (new DateTime(_dateTimeIn.Year, _dateTimeIn.Month, 1)).ToString("yyyy-MM-dd");
                endMonth = (new DateTime(_dateTimeIn.Year, _dateTimeIn.Month, 1)).AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd");
            }

            List<ChamCongLogViewModel> chamCongLog = _mapper.Map<List<ChamCongLogViewModel>>(_chamCongRespository.FindAll(x => string.Compare(x.Ngay_ChamCong, beginMonth) >= 0 && string.Compare(x.Ngay_ChamCong, endMonth) <= 0));

            Dictionary<string, string> dicpamram = new Dictionary<string, string>();
            dicpamram.Add("A_DATE_TIME", time);

            // get info cham cong
            ResultDB resultDB = _attendanceRecordRespository.ExecProceduce("PKG_BUSINESS.GET_INFO_NHANVIEN_CHAMCONG", dicpamram, "", null);

            if(resultDB.ReturnInt == 0)
            {
                if(resultDB.ReturnDataSet.Tables.Count > 0)
                {
                    DataTable nvTable = resultDB.ReturnDataSet.Tables[0];
                    DataTable nvCaLviec = resultDB.ReturnDataSet.Tables[1];
                    DataTable chamCongDB = resultDB.ReturnDataSet.Tables[2];
                    DataTable dangKyOT = resultDB.ReturnDataSet.Tables[3];

                    ChamCongDataViewModel chamCongVM;
                    foreach (DataRow row in nvTable.Rows)
                    {
                        chamCongVM = new ChamCongDataViewModel();
                        chamCongVM.MaNV = row["MaNV"].NullString();
                        chamCongVM.TenNV = row["TenNV"].NullString();
                        chamCongVM.NgayVao = row["NgayVao"].NullString();
                        chamCongVM.BoPhanDetail = row["TenBoPhanChiTiet"].NullString();
                        chamCongVM.BoPhan = row["MaBoPhan"].NullString();
                        chamCongVM.LoaiHD =int.Parse(row["LoaiHD"].NullString());
                        chamCongVM.TenHD = row["TenLoaiHD"].NullString();
                        chamCongVM.NgayHieuLucHD = row["NgayHieuLuc"].NullString();
                        chamCongVM.NgayHetHLHD = row["NgayHetHieuLuc"].NullString();
                        chamCongVM.month_Check = time;
                        chamCongVM.lstDanhMucOT = lstDanhMucOT;
                        lstResult.Add(chamCongVM);
                    }

                    foreach (var item in lstResult)
                    {

                    }
                }
            }
            else
            {
                throw new Exception(resultDB.ReturnString);
            }


            throw new NotImplementedException();
        }

        public void Update(AttendanceRecordViewModel attendance)
        {
            attendance.UserModified = GetUserId();
            var entity = _mapper.Map<ATTENDANCE_RECORD>(attendance);
            _attendanceRecordRespository.Update(entity);
        }

        public void UpdateOverTime(AttendanceOvertimeViewModel attendance)
        {
            attendance.UserModified = GetUserId();
            var entity = _mapper.Map<ATTENDANCE_OVERTIME>(attendance);
            _attendanceOvertimeRespository.Update(entity);
        }
    }
}
