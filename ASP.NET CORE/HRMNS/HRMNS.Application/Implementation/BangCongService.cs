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
using HRMNS.Utilities.Constants;

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
        private IRespository<NGAY_LE_NAM, string> _ngaylenamRespository;
        private IRespository<NGAY_DAC_BIET, string> _ngaydacbietRespository;
        private IRespository<NGAY_NGHI_BU_LE_NAM, int> _ngaynghibuRespository;

        private EFUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BangCongService(IRespository<ATTENDANCE_RECORD, long> attendance_record, IRespository<NGAY_LE_NAM, string> ngayleRespository, IRespository<NGAY_NGHI_BU_LE_NAM, int> ngaynghibuRespository, IRespository<NGAY_DAC_BIET, string> ngayDacBietRespository, IRespository<ATTENDANCE_OVERTIME, int> attendance_overtime, IRespository<CHAM_CONG_LOG, long> chamCongRespository, IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _attendanceRecordRespository = attendance_record;
            _attendanceOvertimeRespository = attendance_overtime;
            _chamCongRespository = chamCongRespository;
            _ngaylenamRespository = ngayleRespository;
            _ngaydacbietRespository = ngayDacBietRespository;
            _ngaynghibuRespository = ngaynghibuRespository;

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
            if (DateTime.TryParse(time, out DateTime _dateTimeIn))
            {
                beginMonth = (new DateTime(_dateTimeIn.Year, _dateTimeIn.Month, 1)).ToString("yyyy-MM-dd");
                endMonth = (new DateTime(_dateTimeIn.Year, _dateTimeIn.Month, 1)).AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd");
            }
            else
            {
                return null;
            }

            List<ChamCongLogViewModel> chamCongLog = _mapper.Map<List<ChamCongLogViewModel>>(_chamCongRespository.FindAll(x => string.Compare(x.Ngay_ChamCong, beginMonth) >= 0 && string.Compare(x.Ngay_ChamCong, endMonth) <= 0));

            Dictionary<string, string> dicpamram = new Dictionary<string, string>();
            dicpamram.Add("A_DATE_TIME", time);

            // get info cham cong
            ResultDB resultDB = _attendanceRecordRespository.ExecProceduce("PKG_BUSINESS.GET_INFO_NHANVIEN_CHAMCONG", dicpamram, "", null);

            if (resultDB.ReturnInt == 0)
            {
                if (resultDB.ReturnDataSet.Tables.Count > 0)
                {
                    DataTable nvTable = resultDB.ReturnDataSet.Tables[0];
                    DataTable nvHDTable = resultDB.ReturnDataSet.Tables[1];
                    DataTable nvCaLviec = resultDB.ReturnDataSet.Tables[2];
                    DataTable chamCongDB = resultDB.ReturnDataSet.Tables[3];
                    DataTable dangKyOT = resultDB.ReturnDataSet.Tables[4];

                    ChamCongDataViewModel chamCongVM;

                    // lay ds thong tin nhan vien 
                    foreach (DataRow row in nvTable.Rows)
                    {
                        chamCongVM = new ChamCongDataViewModel();
                        chamCongVM.MaNV = row["MaNV"].NullString();
                        chamCongVM.TenNV = row["TenNV"].NullString();
                        chamCongVM.NgayVao = row["NgayVao"].NullString();
                        chamCongVM.BoPhanDetail = row["TenBoPhanChiTiet"].NullString();
                        chamCongVM.BoPhan = row["MaBoPhan"].NullString();
                        chamCongVM.month_Check = time;
                        chamCongVM.lstDanhMucOT = lstDanhMucOT;
                        lstResult.Add(chamCongVM);
                    }

                    foreach (var item in lstResult)
                    {
                        // lay thong tin hop dong lao dong
                        foreach (DataRow row in nvHDTable.Rows)
                        {
                            if (item.MaNV == row["MaNV"].NullString())
                            {
                                item.lstHopDong.Add(new HopDong_NV()
                                {
                                    LoaiHD = int.Parse(row["LoaiHD"].IfNullIsZero()),
                                    TenHD = row["TenLoaiHD"].NullString(),
                                    ShortName = row["ShortName"].NullString(),
                                    NgayHetHLHD = row["NgayHetHieuLuc"].NullString(),
                                    NgayHieuLucHD = row["NgayHieuLuc"].NullString()
                                });
                            }
                        }

                        // Lay thong tin ca lam viec
                        foreach (DataRow row in nvCaLviec.Rows)
                        {
                            if (item.MaNV == row["MaNV"].NullString())
                            {
                                item.lstNhanVienCaLamViec.Add(new NhanVien_CaLamViec()
                                {
                                    MaCaLaviec = row["MaCaLaviec"].NullString(),
                                    TenCaLamViec = row["TenCaLamViec"].NullString(),
                                    BatDau_TheoCa = row["BatDau_TheoCa"].NullString(),
                                    KetThuc_TheoCa = row["KetThuc_TheoCa"].NullString(),
                                    DM_NgayLViec = row["DM_NgayLViec"].NullString(),
                                    HeSo_OT = float.Parse(row["HeSo_OT"].IfNullIsZero()),
                                    Ten_NgayLV = row["Ten_NgayLV"].NullString(),
                                    Time_BatDau = row["Time_BatDau"].NullString(),
                                    Time_BatDau2 = row["Time_BatDau2"].NullString(),
                                    Time_KetThuc = row["Time_KetThuc"].NullString(),
                                    Time_KetThuc2 = row["Time_KetThuc2"].NullString()
                                });
                            }
                        }

                        // lay thong tin cham cong dac biet
                        foreach (DataRow row in nvCaLviec.Rows)
                        {
                            if (item.MaNV == row["MaNV"].NullString())
                            {
                                item.lstChamCongDB.Add(new ChamCongDB()
                                {
                                    TenChiTiet = row["TenChiTiet"].NullString(),
                                    NgayBatDau = row["NgayBatDau"].NullString(),
                                    NgayKetThuc = row["NgayKetThuc"].NullString(),
                                    KyHieuChamCong = row["KyHieuChamCong"].NullString()
                                });
                            }
                        }

                        // lay thong tin cac ngay OT
                        foreach (DataRow row in dangKyOT.Rows)
                        {
                            if (item.MaNV == row["MaNV"].NullString())
                            {
                                item.lstDangKyOT.Add(new DangKyOT()
                                {
                                    Ten_NgayLV = row["Ten_NgayLV"].NullString(),
                                    DM_NgayLViec = row["DM_NgayLViec"].NullString(),
                                    NgayOT = row["NgayOT"].NullString()
                                });
                            }
                        }
                    }


                    // Update du lieu tao bang cong
                    string dateCheck = "";
                    HopDong_NV hopDong_NV;
                    NhanVien_CaLamViec _caLamViec;
                    CHAM_CONG_LOG _chamCongLog;
                    for (int i = 1; i <= DateTime.Parse(endMonth).Day; i++) // day 1 -> 31 or end month
                    {
                        dateCheck = (new DateTime(DateTime.Parse(endMonth).Year, DateTime.Parse(endMonth).Month, i)).ToString("yyyy-MM-dd");
                        foreach (var item in lstResult)
                        {
                            // check hop dong
                            hopDong_NV = item.lstHopDong.FirstOrDefault(x => string.Compare(dateCheck, x.NgayHieuLucHD) >= 0 && string.Compare(dateCheck, x.NgayHetHLHD) <= 0);

                            if (hopDong_NV != null)
                            {
                                if (hopDong_NV.ShortName.NullString() == CommonConstants.HD_THUVIEC_EM) // HD Thu viec 85% luong
                                {
                                    // check ca lam viec
                                    _caLamViec = item.lstNhanVienCaLamViec.FirstOrDefault(x => string.Compare(dateCheck, x.BatDau_TheoCa) >= 0 && string.Compare(dateCheck, x.KetThuc_TheoCa) <= 0);

                                    if (_caLamViec != null)
                                    {
                                        // get data cham cong log 
                                        _chamCongLog = _chamCongRespository.FindAll(x => dateCheck == x.Ngay_ChamCong && item.MaNV.ToUpper().Contains(x.ID_NV.ToUpper())).FirstOrDefault();

                                        if(_caLamViec.MaCaLaviec == CommonConstants.CA_NGAY)
                                        {
                                            // Co du lieu cham cong
                                            if(_chamCongLog.FirstIn.NullString() == "IN" && _chamCongLog.LastOut.NullString() == "OUT")
                                            {
                                                // 1. CHECK NGAY CONG
                                                if(IsNgayLe(dateCheck) == "" && IsNgayTruocLe(dateCheck) == "")
                                                {

                                                }
                                                else
                                                {

                                                }
                                            }
                                        }
                                        else
                                        {

                                        }
                                    }
                                }
                                else
                                {

                                }
                            }
                        }
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

        private bool IsChuNhat(string time)
        {
            return DateTime.Parse(time).DayOfWeek == DayOfWeek.Sunday;
        }

        private string IsNgayLe(string time)
        {
            var ngayle = _ngaylenamRespository.FindById(time);
            if (ngayle != null)
            {
                return time;
            }
            return "";
        }

        private string IsNgayTruocLe(string time)
        {
            string newTime = DateTime.Parse(time).AddDays(1).ToString("yyyy-MM-dd");
            if (IsNgayLe(newTime) != "")
            {
                return time;
            }
            return "";
        }

        private string IsNgayNghiBuLeNam(string time)
        {
            var ngaynghibu = _ngaynghibuRespository.FindSingle(x => x.NgayNghiBu == time);
            if (ngaynghibu != null)
            {
                return time;
            }
            return "";
        }

        private string IsNgayDacBiet(string time)
        {
            var ngaydacbiet = _ngaydacbietRespository.FindSingle(x => time.Contains(x.TenNgayDacBiet));
            if (ngaydacbiet != null)
            {
                return time;
            }
            return "";
        }
    }
}
