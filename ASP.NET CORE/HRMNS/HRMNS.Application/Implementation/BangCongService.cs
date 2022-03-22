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
using System.Globalization;

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
                    string firstTime = "";
                    string lastTime = "";
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

                                        // get first time and last time
                                        firstTime = _chamCongLog.FirstIn_Time_Update.NullString() == "" ? _chamCongLog.FirstIn_Time.NullString() : _chamCongLog.FirstIn_Time_Update.NullString();
                                        lastTime = _chamCongLog.Last_Out_Time_Update.NullString() == "" ? _chamCongLog.Last_Out_Time.NullString() : _chamCongLog.Last_Out_Time_Update.NullString();

                                        // Co du lieu cham cong
                                        if (_chamCongLog.FirstIn.NullString() == "IN" && _chamCongLog.LastOut.NullString() == "OUT")
                                        {
                                            #region THU VIEC + CA NGAY
                                            if (_caLamViec.MaCaLaviec == CommonConstants.CA_NGAY)
                                            {
                                                // 1. CHECK NGAY CONG : thu viec + ca ngay 
                                                if (DateTime.ParseExact(firstTime, "HH:mm:ss", CultureInfo.InvariantCulture) < DateTime.ParseExact("08:00:00", "HH:mm:ss", CultureInfo.InvariantCulture))
                                                {
                                                    firstTime = "08:00:00";
                                                }

                                                if (CheckNgayDB(dateCheck) == 0) // ngay thuong
                                                {
                                                    string newBeginOT = "17:45:00";
                                                    if (string.Compare(lastTime, newBeginOT) > 0)
                                                    {
                                                        if (item.BoPhan != "SP")
                                                        {
                                                            newBeginOT = "17:30:00";
                                                        }

                                                        var clviec = item.lstNhanVienCaLamViec.FindAll(x =>
                                                        string.Compare(dateCheck, x.BatDau_TheoCa) >= 0 &&
                                                        string.Compare(dateCheck, x.KetThuc_TheoCa) <= 0 &&
                                                        x.MaCaLaviec == CommonConstants.CA_NGAY && x.DM_NgayLViec == "NT").OrderByDescending(x => x.Time_BatDau).FirstOrDefault();

                                                        double timeOT = (DateTime.ParseExact(dateCheck + " " + lastTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) - DateTime.ParseExact(dateCheck + " " + newBeginOT, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)).TotalHours;

                                                        item.OvertimeValues.Add(new OvertimeValue()
                                                        {
                                                            DayCheckOT = dateCheck,
                                                            DMOvertime = clviec.HeSo_OT.NullString(),
                                                            ValueOT = timeOT
                                                        });
                                                    }

                                                    item.WorkingStatuses.Add(new WorkingStatus()
                                                    {
                                                        DayCheck = dateCheck,
                                                        Value = "PD" // PD: Probation Day shift/Thử việc ca ngày
                                                    });


                                                }
                                                else if (CheckNgayDB(dateCheck) == 5) // Ngay ki niem cty
                                                {
                                                    var clviec = item.lstNhanVienCaLamViec.FirstOrDefault(x =>
                                                        string.Compare(dateCheck, x.BatDau_TheoCa) >= 0 &&
                                                        string.Compare(dateCheck, x.KetThuc_TheoCa) <= 0 &&
                                                        x.MaCaLaviec == CommonConstants.CA_NGAY && x.DM_NgayLViec == "CN");

                                                    double timeOT = (DateTime.ParseExact(dateCheck + " " + lastTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) - DateTime.ParseExact(dateCheck + " " + firstTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)).TotalHours;
                                                    timeOT = timeOT <= 8 ? timeOT - 0.5 : timeOT - 1.5;

                                                    item.WorkingStatuses.Add(new WorkingStatus()
                                                    {
                                                        DayCheck = dateCheck,
                                                        Value = "PMD" // Làm ca ngày thử việc ngay ki niem
                                                    });

                                                    item.OvertimeValues.Add(new OvertimeValue()
                                                    {
                                                        DayCheckOT = dateCheck,
                                                        DMOvertime = clviec.HeSo_OT.NullString(), // nhu OT chu nhat
                                                        ValueOT = timeOT
                                                    });
                                                }
                                                else if (CheckNgayDB(dateCheck) == 3 || CheckNgayDB(dateCheck) == 4) // ngay chu nhat or nghi bu ngay le
                                                {
                                                    var clviec = item.lstNhanVienCaLamViec.FirstOrDefault(x =>
                                                         string.Compare(dateCheck, x.BatDau_TheoCa) >= 0 &&
                                                         string.Compare(dateCheck, x.KetThuc_TheoCa) <= 0 &&
                                                         x.MaCaLaviec == CommonConstants.CA_NGAY && x.DM_NgayLViec == "CN");

                                                    double timeOT = (DateTime.ParseExact(dateCheck + " " + lastTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) - DateTime.ParseExact(dateCheck + " " + firstTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)).TotalHours;
                                                    timeOT = timeOT <= 8 ? timeOT - 0.5 : timeOT - 1.5;

                                                    item.WorkingStatuses.Add(new WorkingStatus()
                                                    {
                                                        DayCheck = dateCheck,
                                                        Value = "TV" // TV: Thử việc làm thêm ngày chủ nhật/ Probation
                                                    });

                                                    item.OvertimeValues.Add(new OvertimeValue()
                                                    {
                                                        DayCheckOT = dateCheck,
                                                        DMOvertime = clviec.HeSo_OT.NullString(), // nhu OT chu nhat
                                                        ValueOT = timeOT
                                                    });
                                                }
                                                else if (CheckNgayDB(dateCheck) == 1) // ngay le
                                                {
                                                    var clviec = item.lstNhanVienCaLamViec.FirstOrDefault(x =>
                                                     string.Compare(dateCheck, x.BatDau_TheoCa) >= 0 &&
                                                     string.Compare(dateCheck, x.KetThuc_TheoCa) <= 0 &&
                                                     x.MaCaLaviec == CommonConstants.CA_NGAY &&
                                                     (x.DM_NgayLViec == "NL" || x.DM_NgayLViec == "NLCC"));

                                                    double timeOT = (DateTime.ParseExact(dateCheck + " " + lastTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) - DateTime.ParseExact(dateCheck + " " + firstTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)).TotalHours;
                                                    timeOT = timeOT <= 8 ? timeOT - 0.5 : timeOT - 1.5;

                                                    item.WorkingStatuses.Add(new WorkingStatus()
                                                    {
                                                        DayCheck = dateCheck,
                                                        Value = "PD" // PD: Probation Day shift/Thử việc ca ngày
                                                    });

                                                    item.OvertimeValues.Add(new OvertimeValue()
                                                    {
                                                        DayCheckOT = dateCheck,
                                                        DMOvertime = clviec.HeSo_OT.NullString(),
                                                        ValueOT = timeOT
                                                    });
                                                }
                                                else if (CheckNgayDB(dateCheck) == 2) // ngay truoc le
                                                {
                                                    var clviec = item.lstNhanVienCaLamViec.FirstOrDefault(x =>
                                                     string.Compare(dateCheck, x.BatDau_TheoCa) >= 0 &&
                                                     string.Compare(dateCheck, x.KetThuc_TheoCa) <= 0 &&
                                                     x.MaCaLaviec == CommonConstants.CA_NGAY &&
                                                     (x.DM_NgayLViec == "TNL"));

                                                    string newBeginOT = "17:45:00";
                                                    if (string.Compare(lastTime, newBeginOT) > 0)
                                                    {
                                                        if (item.BoPhan != "SP")
                                                        {
                                                            newBeginOT = "17:30:00";
                                                        }

                                                        double timeOT = (DateTime.ParseExact(dateCheck + " " + lastTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) - DateTime.ParseExact(dateCheck + " " + newBeginOT, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)).TotalHours;

                                                        item.OvertimeValues.Add(new OvertimeValue()
                                                        {
                                                            DayCheckOT = dateCheck,
                                                            DMOvertime = clviec.HeSo_OT.NullString(),
                                                            ValueOT = timeOT
                                                        });
                                                    }

                                                    item.WorkingStatuses.Add(new WorkingStatus()
                                                    {
                                                        DayCheck = dateCheck,
                                                        Value = "PD" // PD: Probation Day shift/Thử việc ca ngày
                                                    });
                                                }

                                                // Di muon ve som
                                                double ELLC = 0;
                                                if (DateTime.ParseExact(firstTime, "HH:mm:ss", CultureInfo.InvariantCulture) > DateTime.ParseExact("08:05:00", "HH:mm:ss", CultureInfo.InvariantCulture))
                                                {
                                                    ELLC += (DateTime.ParseExact(firstTime, "HH:mm:ss", CultureInfo.InvariantCulture) - DateTime.ParseExact("08:05:00", "HH:mm:ss", CultureInfo.InvariantCulture)).TotalHours;
                                                }

                                                if (DateTime.ParseExact(lastTime, "HH:mm:ss", CultureInfo.InvariantCulture) < DateTime.ParseExact("17:00:00", "HH:mm:ss", CultureInfo.InvariantCulture))
                                                {
                                                    ELLC += (DateTime.ParseExact("17:00:00", "HH:mm:ss", CultureInfo.InvariantCulture) - DateTime.ParseExact(lastTime, "HH:mm:ss", CultureInfo.InvariantCulture)).TotalHours;
                                                }

                                                item.EL_LC_Statuses.Add(new EL_LC_Status()
                                                {
                                                    DayCheck_EL = dateCheck,
                                                    Value_EL = ELLC
                                                });
                                            }
                                            #endregion

                                            #region THU VIEC + CA DEM
                                            else
                                            {
                                                if (DateTime.ParseExact(firstTime, "HH:mm:ss", CultureInfo.InvariantCulture) < DateTime.ParseExact("20:00:00", "HH:mm:ss", CultureInfo.InvariantCulture))
                                                {
                                                    firstTime = "20:00:00";
                                                }

                                                if (DateTime.ParseExact(lastTime, "HH:mm:ss", CultureInfo.InvariantCulture) > DateTime.ParseExact("08:00:00", "HH:mm:ss", CultureInfo.InvariantCulture))
                                                {
                                                    lastTime = "08:00:00";
                                                }

                                                if (CheckNgayDB(dateCheck) == 0) // ngay thuong
                                                {
                                                    string newBeginOT = "05:30:00";
                                                    if (string.Compare(lastTime, newBeginOT) > 0)
                                                    {
                                                        double timeOT = (DateTime.ParseExact(dateCheck + " " + lastTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) - DateTime.ParseExact(dateCheck + " " + newBeginOT, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)).TotalHours;

                                                        if (timeOT > 0 && timeOT <= 0.5)
                                                        {
                                                            item.OvertimeValues.Add(new OvertimeValue()
                                                            {
                                                                DayCheckOT = dateCheck,
                                                                DMOvertime = "200", // 200 % : 5H30 -> 6H
                                                                ValueOT = timeOT
                                                            });
                                                        }
                                                        else if (timeOT > 0.5)
                                                        {
                                                            item.OvertimeValues.Add(new OvertimeValue()
                                                            {
                                                                DayCheckOT = dateCheck,
                                                                DMOvertime = "200", // 200 % : 5H30 -> 6H
                                                                ValueOT = 0.5
                                                            });

                                                            item.OvertimeValues.Add(new OvertimeValue()
                                                            {
                                                                DayCheckOT = dateCheck,
                                                                DMOvertime = "150", // 150 % :6h -> 8H
                                                                ValueOT = timeOT - 0.5 // tru di 0.5h cua 5-6h
                                                            });
                                                        }
                                                    }

                                                    item.WorkingStatuses.Add(new WorkingStatus()
                                                    {
                                                        DayCheck = dateCheck,
                                                        Value = "PN" // PN: Probation Night shift/Thử việc ca đêm
                                                    });
                                                }
                                                else if (CheckNgayDB(dateCheck) == 5) // Ngay ki niem cty
                                                {
                                                    item.OvertimeValues.AddRange(GetOvertimeInNight(firstTime, lastTime, dateCheck, "CN"));
                                                    item.WorkingStatuses.Add(new WorkingStatus()
                                                    {
                                                        DayCheck = dateCheck,
                                                        Value = "PM" // PM: Làm ca đêm ngày kỷ niệm  thử việc
                                                    });
                                                }
                                                else if (CheckNgayDB(dateCheck) == 3 || CheckNgayDB(dateCheck) == 4) // ngay chu nhat + ngay nghi bu
                                                {
                                                    item.OvertimeValues.AddRange(GetOvertimeInNight(firstTime, lastTime, dateCheck, "CN"));
                                                    item.WorkingStatuses.Add(new WorkingStatus()
                                                    {
                                                        DayCheck = dateCheck,
                                                        Value = "TVD" // TVD: Thử việc làm thêm ca đêm chủ nhật
                                                    });
                                                }
                                                else if (CheckNgayDB(dateCheck) == 1) // ngay le
                                                {
                                                    var ngayle = _ngaylenamRespository.FindById(dateCheck);
                                                    if (ngayle.IslastHoliday == CommonConstants.N)
                                                    {
                                                        item.OvertimeValues.AddRange(GetOvertimeInNight(firstTime, lastTime, dateCheck, "NL"));
                                                    }
                                                    else // ngay le cuoi cung
                                                    {
                                                        item.OvertimeValues.AddRange(GetOvertimeInNight(firstTime, lastTime, dateCheck, "NLCC"));
                                                    }

                                                    item.WorkingStatuses.Add(new WorkingStatus()
                                                    {
                                                        DayCheck = dateCheck,
                                                        Value = "PDD" // PDD: Thử việc làm đêm ngày lễ
                                                    });
                                                }
                                                else if (CheckNgayDB(dateCheck) == 2) // ngay truoc le
                                                {
                                                    item.OvertimeValues.AddRange(GetOvertimeInNight(firstTime, lastTime, dateCheck, "NL"));
                                                    item.WorkingStatuses.Add(new WorkingStatus()
                                                    {
                                                        DayCheck = dateCheck,
                                                        Value = "PH" // PH: làm ca đêm trước ngày lễ( thử việc)
                                                    });
                                                }

                                                // Di muon ve som
                                                double ELLC = 0;
                                                if (DateTime.ParseExact(firstTime, "HH:mm:ss", CultureInfo.InvariantCulture) > DateTime.ParseExact("20:05:00", "HH:mm:ss", CultureInfo.InvariantCulture))
                                                {
                                                    ELLC += (DateTime.ParseExact(firstTime, "HH:mm:ss", CultureInfo.InvariantCulture) - DateTime.ParseExact("20:05:00", "HH:mm:ss", CultureInfo.InvariantCulture)).TotalHours;
                                                }

                                                if (DateTime.ParseExact(lastTime, "HH:mm:ss", CultureInfo.InvariantCulture) < DateTime.ParseExact("08:00:00", "HH:mm:ss", CultureInfo.InvariantCulture))
                                                {
                                                    ELLC += (DateTime.ParseExact("08:00:00", "HH:mm:ss", CultureInfo.InvariantCulture) - DateTime.ParseExact(lastTime, "HH:mm:ss", CultureInfo.InvariantCulture)).TotalHours;
                                                }

                                                item.EL_LC_Statuses.Add(new EL_LC_Status()
                                                {
                                                    DayCheck_EL = dateCheck,
                                                    Value_EL = ELLC
                                                });
                                            }
                                        }
                                        else if (_chamCongLog.FirstIn.NullString() == "" && _chamCongLog.LastOut.NullString() == "")
                                        {
                                            if (CheckNgayDB(dateCheck) == 1) // ngay le
                                            {
                                                item.WorkingStatuses.Add(new WorkingStatus()
                                                {
                                                    DayCheck = dateCheck,
                                                    Value = "NH" // NH: NationALHoliday/ Nghỉ lễ
                                                });
                                            }
                                        }
                                        #endregion
                                    }
                                }
                                else // HD Chinh thuc
                                {
                                    // check ca lam viec
                                    _caLamViec = item.lstNhanVienCaLamViec.FirstOrDefault(x => string.Compare(dateCheck, x.BatDau_TheoCa) >= 0 && string.Compare(dateCheck, x.KetThuc_TheoCa) <= 0);

                                    if (_caLamViec != null)
                                    {
                                        // get data cham cong log 
                                        _chamCongLog = _chamCongRespository.FindAll(x => dateCheck == x.Ngay_ChamCong && item.MaNV.ToUpper().Contains(x.ID_NV.ToUpper())).FirstOrDefault();

                                        // get first time and last time
                                        firstTime = _chamCongLog.FirstIn_Time_Update.NullString() == "" ? _chamCongLog.FirstIn_Time.NullString() : _chamCongLog.FirstIn_Time_Update.NullString();
                                        lastTime = _chamCongLog.Last_Out_Time_Update.NullString() == "" ? _chamCongLog.Last_Out_Time.NullString() : _chamCongLog.Last_Out_Time_Update.NullString();

                                        // Co du lieu cham cong
                                        if (_chamCongLog.FirstIn.NullString() == "IN" && _chamCongLog.LastOut.NullString() == "OUT")
                                        {
                                            #region CHINH THUC + CA NGAY
                                            if (_caLamViec.MaCaLaviec == CommonConstants.CA_NGAY)
                                            {
                                                if (DateTime.ParseExact(firstTime, "HH:mm:ss", CultureInfo.InvariantCulture) < DateTime.ParseExact("08:00:00", "HH:mm:ss", CultureInfo.InvariantCulture))
                                                {
                                                    firstTime = "08:00:00";
                                                }

                                                if (CheckNgayDB(dateCheck) == 0) // ngay thuong
                                                {
                                                    string newBeginOT = "17:45:00";
                                                    if (string.Compare(lastTime, newBeginOT) > 0)
                                                    {
                                                        if (item.BoPhan != "SP")
                                                        {
                                                            newBeginOT = "17:30:00";
                                                        }

                                                        double timeOT = (DateTime.ParseExact(dateCheck + " " + lastTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) - DateTime.ParseExact(dateCheck + " " + newBeginOT, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)).TotalHours;

                                                        item.OvertimeValues.Add(new OvertimeValue()
                                                        {
                                                            DayCheckOT = dateCheck,
                                                            DMOvertime = "150",
                                                            ValueOT = timeOT
                                                        });
                                                    }

                                                    item.WorkingStatuses.Add(new WorkingStatus()
                                                    {
                                                        DayCheck = dateCheck,
                                                        Value = "DS" // DS: Day Shift/ Ca ngày 
                                                    });
                                                }
                                                else if (CheckNgayDB(dateCheck) == 5) // Ngay ki niem cty
                                                {
                                                    double timeOT = (DateTime.ParseExact(dateCheck + " " + lastTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) - DateTime.ParseExact(dateCheck + " " + firstTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)).TotalHours;
                                                    timeOT = timeOT <= 8 ? timeOT - 0.5 : timeOT - 1.5;

                                                    item.WorkingStatuses.Add(new WorkingStatus()
                                                    {
                                                        DayCheck = dateCheck,
                                                        Value = "MD" // làm ca ngày chính thức ngay ki niem
                                                    });

                                                    item.OvertimeValues.Add(new OvertimeValue()
                                                    {
                                                        DayCheckOT = dateCheck,
                                                        DMOvertime = "200", // nhu OT chu nhat
                                                        ValueOT = timeOT
                                                    });
                                                }
                                                else if (CheckNgayDB(dateCheck) == 3 || CheckNgayDB(dateCheck) == 4) // ngay chu nhat or nghi bu ngay le
                                                {
                                                    double timeOT = (DateTime.ParseExact(dateCheck + " " + lastTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) - DateTime.ParseExact(dateCheck + " " + firstTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)).TotalHours;
                                                    timeOT = timeOT <= 8 ? timeOT - 0.5 : timeOT - 1.5;

                                                    item.WorkingStatuses.Add(new WorkingStatus()
                                                    {
                                                        DayCheck = dateCheck,
                                                        Value = "DS"
                                                    });

                                                    item.OvertimeValues.Add(new OvertimeValue()
                                                    {
                                                        DayCheckOT = dateCheck,
                                                        DMOvertime = "200", // nhu OT chu nhat
                                                        ValueOT = timeOT
                                                    });
                                                }
                                                else if (CheckNgayDB(dateCheck) == 1) // ngay le
                                                {
                                                    double timeOT = (DateTime.ParseExact(dateCheck + " " + lastTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) - DateTime.ParseExact(dateCheck + " " + firstTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)).TotalHours;
                                                    timeOT = timeOT <= 8 ? timeOT - 0.5 : timeOT - 1.5;

                                                    item.WorkingStatuses.Add(new WorkingStatus()
                                                    {
                                                        DayCheck = dateCheck,
                                                        Value = "DS"
                                                    });

                                                    item.OvertimeValues.Add(new OvertimeValue()
                                                    {
                                                        DayCheckOT = dateCheck,
                                                        DMOvertime = "300",
                                                        ValueOT = timeOT
                                                    });
                                                }
                                                else if (CheckNgayDB(dateCheck) == 2) // ngay truoc le
                                                {
                                                    string newBeginOT = "17:45:00";
                                                    if (string.Compare(lastTime, newBeginOT) > 0)
                                                    {
                                                        if (item.BoPhan != "SP")
                                                        {
                                                            newBeginOT = "17:30:00";
                                                        }

                                                        double timeOT = (DateTime.ParseExact(dateCheck + " " + lastTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) - DateTime.ParseExact(dateCheck + " " + newBeginOT, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)).TotalHours;

                                                        item.OvertimeValues.Add(new OvertimeValue()
                                                        {
                                                            DayCheckOT = dateCheck,
                                                            DMOvertime = "150",
                                                            ValueOT = timeOT
                                                        });
                                                    }

                                                    item.WorkingStatuses.Add(new WorkingStatus()
                                                    {
                                                        DayCheck = dateCheck,
                                                        Value = "DS" // DS: Day Shift/ Ca ngày
                                                    });
                                                }

                                                // Di muon ve som
                                                double ELLC = 0;
                                                if (DateTime.ParseExact(firstTime, "HH:mm:ss", CultureInfo.InvariantCulture) > DateTime.ParseExact("08:05:00", "HH:mm:ss", CultureInfo.InvariantCulture))
                                                {
                                                    ELLC += (DateTime.ParseExact(firstTime, "HH:mm:ss", CultureInfo.InvariantCulture) - DateTime.ParseExact("08:05:00", "HH:mm:ss", CultureInfo.InvariantCulture)).TotalHours;
                                                }

                                                if (DateTime.ParseExact(lastTime, "HH:mm:ss", CultureInfo.InvariantCulture) < DateTime.ParseExact("17:00:00", "HH:mm:ss", CultureInfo.InvariantCulture))
                                                {
                                                    ELLC += (DateTime.ParseExact("17:00:00", "HH:mm:ss", CultureInfo.InvariantCulture) - DateTime.ParseExact(lastTime, "HH:mm:ss", CultureInfo.InvariantCulture)).TotalHours;
                                                }

                                                item.EL_LC_Statuses.Add(new EL_LC_Status()
                                                {
                                                    DayCheck_EL = dateCheck,
                                                    Value_EL = ELLC
                                                });

                                            }
                                            #endregion

                                            #region CHINH THUC + CA DEM
                                            else
                                            {
                                                if (DateTime.ParseExact(firstTime, "HH:mm:ss", CultureInfo.InvariantCulture) < DateTime.ParseExact("20:00:00", "HH:mm:ss", CultureInfo.InvariantCulture))
                                                {
                                                    firstTime = "20:00:00";
                                                }

                                                if (DateTime.ParseExact(lastTime, "HH:mm:ss", CultureInfo.InvariantCulture) > DateTime.ParseExact("08:00:00", "HH:mm:ss", CultureInfo.InvariantCulture))
                                                {
                                                    lastTime = "08:00:00";
                                                }

                                                if (CheckNgayDB(dateCheck) == 0) // ngay thuong
                                                {
                                                    string newBeginOT = "05:30:00";
                                                    if (string.Compare(lastTime, newBeginOT) > 0)
                                                    {
                                                        double timeOT = (DateTime.ParseExact(dateCheck + " " + lastTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) - DateTime.ParseExact(dateCheck + " " + newBeginOT, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)).TotalHours;

                                                        if (timeOT > 0 && timeOT <= 0.5)
                                                        {
                                                            item.OvertimeValues.Add(new OvertimeValue()
                                                            {
                                                                DayCheckOT = dateCheck,
                                                                DMOvertime = "200", // 200 % : 5H30 -> 6H
                                                                ValueOT = timeOT
                                                            });
                                                        }
                                                        else if (timeOT > 0.5)
                                                        {
                                                            item.OvertimeValues.Add(new OvertimeValue()
                                                            {
                                                                DayCheckOT = dateCheck,
                                                                DMOvertime = "200", // 200 % : 5H30 -> 6H
                                                                ValueOT = 0.5
                                                            });

                                                            item.OvertimeValues.Add(new OvertimeValue()
                                                            {
                                                                DayCheckOT = dateCheck,
                                                                DMOvertime = "150", // 150 % :6h -> 8H
                                                                ValueOT = timeOT - 0.5 // tru di 0.5h cua 5-6h
                                                            });
                                                        }
                                                    }

                                                    item.WorkingStatuses.Add(new WorkingStatus()
                                                    {
                                                        DayCheck = dateCheck,
                                                        Value = "NS" // NS: Night Shift/ Ca đêm
                                                    });
                                                }
                                                else if (CheckNgayDB(dateCheck) == 5) // Ngay ki niem cty
                                                {
                                                    item.OvertimeValues.AddRange(GetOvertimeInNight(firstTime, lastTime, dateCheck, "CN"));
                                                    item.WorkingStatuses.Add(new WorkingStatus()
                                                    {
                                                        DayCheck = dateCheck,
                                                        Value = "BM" // BM: Làm ca đêm ngày kỷ niệm chính thức
                                                    });
                                                }
                                                else if (CheckNgayDB(dateCheck) == 3 || CheckNgayDB(dateCheck) == 4) // ngay chu nhat + ngay nghi bu
                                                {
                                                    item.OvertimeValues.AddRange(GetOvertimeInNight(firstTime, lastTime, dateCheck, "CN"));
                                                    item.WorkingStatuses.Add(new WorkingStatus()
                                                    {
                                                        DayCheck = dateCheck,
                                                        Value = "D" // TVD: Thử việc làm thêm ca đêm chủ nhật
                                                    });
                                                }
                                                else if (CheckNgayDB(dateCheck) == 1) // ngay le
                                                {
                                                    var ngayle = _ngaylenamRespository.FindById(dateCheck);
                                                    if (ngayle.IslastHoliday == CommonConstants.N)
                                                    {
                                                        item.OvertimeValues.AddRange(GetOvertimeInNight(firstTime, lastTime, dateCheck, "NL"));
                                                    }
                                                    else // ngay le cuoi cung
                                                    {
                                                        item.OvertimeValues.AddRange(GetOvertimeInNight(firstTime, lastTime, dateCheck, "NLCC"));
                                                    }

                                                    item.WorkingStatuses.Add(new WorkingStatus()
                                                    {
                                                        DayCheck = dateCheck,
                                                        Value = "NHD" // NHD: làm ca đêm ngày lễ
                                                    });
                                                }
                                                else if (CheckNgayDB(dateCheck) == 2) // ngay truoc le
                                                {
                                                    item.OvertimeValues.AddRange(GetOvertimeInNight(firstTime, lastTime, dateCheck, "NL"));
                                                    item.WorkingStatuses.Add(new WorkingStatus()
                                                    {
                                                        DayCheck = dateCheck,
                                                        Value = "BH" // BH: làm ca đêm trước ngày lễ( chính thức)
                                                    });
                                                }

                                                // Di muon ve som
                                                double ELLC = 0;
                                                if (DateTime.ParseExact(firstTime, "HH:mm:ss", CultureInfo.InvariantCulture) > DateTime.ParseExact("20:05:00", "HH:mm:ss", CultureInfo.InvariantCulture))
                                                {
                                                    ELLC += (DateTime.ParseExact(firstTime, "HH:mm:ss", CultureInfo.InvariantCulture) - DateTime.ParseExact("20:05:00", "HH:mm:ss", CultureInfo.InvariantCulture)).TotalHours;
                                                }

                                                if (DateTime.ParseExact(lastTime, "HH:mm:ss", CultureInfo.InvariantCulture) < DateTime.ParseExact("08:00:00", "HH:mm:ss", CultureInfo.InvariantCulture))
                                                {
                                                    ELLC += (DateTime.ParseExact("08:00:00", "HH:mm:ss", CultureInfo.InvariantCulture) - DateTime.ParseExact(lastTime, "HH:mm:ss", CultureInfo.InvariantCulture)).TotalHours;
                                                }

                                                item.EL_LC_Statuses.Add(new EL_LC_Status()
                                                {
                                                    DayCheck_EL = dateCheck,
                                                    Value_EL = ELLC
                                                });
                                            }
                                            #endregion
                                        }
                                        else if (_chamCongLog.FirstIn.NullString() == "" && _chamCongLog.LastOut.NullString() == "")
                                        {
                                            if (CheckNgayDB(dateCheck) == 1) // ngay le
                                            {
                                                item.WorkingStatuses.Add(new WorkingStatus()
                                                {
                                                    DayCheck = dateCheck,
                                                    Value = "NH" // NH: NationALHoliday/ Nghỉ lễ
                                                });
                                            }
                                        }
                                    }
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

        private List<OvertimeValue> GetOvertimeInNight(string firstTime, string lastTime, string dateCheck, string ngayLviec)
        {
            // 20:00 -> 22:00
            // 22:00 -> 23:59
            // 00:00 -> 06:00
            // 06:00 -> 08:00

            List<OvertimeValue> lstResult = new List<OvertimeValue>();

            if (DateTime.ParseExact(firstTime, "HH:mm:ss", CultureInfo.InvariantCulture) >= DateTime.ParseExact("20:00:00", "HH:mm:ss", CultureInfo.InvariantCulture) &&
            DateTime.ParseExact(lastTime, "HH:mm:ss", CultureInfo.InvariantCulture) <= DateTime.ParseExact("22:00:00", "HH:mm:ss", CultureInfo.InvariantCulture))
            {
                double timeOT = (DateTime.ParseExact(dateCheck + " " + lastTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) - DateTime.ParseExact(dateCheck + " " + firstTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)).TotalHours;
                if (ngayLviec == "CN") // ngay chu nhat
                {
                    lstResult.Add(new OvertimeValue()
                    {
                        DayCheckOT = dateCheck,
                        DMOvertime = "200",
                        ValueOT = timeOT
                    });
                }
                else if (ngayLviec == "NLCC" || ngayLviec == "NL") // Ngay le, ngay le cuoi cung
                {
                    lstResult.Add(new OvertimeValue()
                    {
                        DayCheckOT = dateCheck,
                        DMOvertime = "300",
                        ValueOT = timeOT
                    });
                }
            }
            else if (DateTime.ParseExact(firstTime, "HH:mm:ss", CultureInfo.InvariantCulture) >= DateTime.ParseExact("22:00:00", "HH:mm:ss", CultureInfo.InvariantCulture) &&
               DateTime.ParseExact(lastTime, "HH:mm:ss", CultureInfo.InvariantCulture) <= DateTime.ParseExact("23:59:59", "HH:mm:ss", CultureInfo.InvariantCulture))
            {
                double timeOT = (DateTime.ParseExact(dateCheck + " " + lastTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) - DateTime.ParseExact(dateCheck + " " + firstTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)).TotalHours;
                if (ngayLviec == "NLCC" || ngayLviec == "NL") // ngay chu nhat
                {
                    lstResult.Add(new OvertimeValue()
                    {
                        DayCheckOT = dateCheck,
                        DMOvertime = "390",
                        ValueOT = timeOT
                    });
                }
                else if (ngayLviec == "CN")
                {
                    lstResult.Add(new OvertimeValue()
                    {
                        DayCheckOT = dateCheck,
                        DMOvertime = "270",
                        ValueOT = timeOT
                    });
                }
            }
            else if (DateTime.ParseExact(firstTime, "HH:mm:ss", CultureInfo.InvariantCulture) >= DateTime.ParseExact("00:00:00", "HH:mm:ss", CultureInfo.InvariantCulture) &&
                     DateTime.ParseExact(lastTime, "HH:mm:ss", CultureInfo.InvariantCulture) <= DateTime.ParseExact("06:00:00", "HH:mm:ss", CultureInfo.InvariantCulture))
            {
                double timeOT = (DateTime.ParseExact(dateCheck + " " + lastTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) - DateTime.ParseExact(dateCheck + " " + firstTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)).TotalHours;

                if (ngayLviec == "NLCC") // ngay le cuoi cung
                {
                    lstResult.Add(new OvertimeValue()
                    {
                        DayCheckOT = dateCheck,
                        DMOvertime = "200",
                        ValueOT = timeOT - 0.5
                    });
                }
                else if (ngayLviec == "NL") // NGAY LE
                {
                    lstResult.Add(new OvertimeValue()
                    {
                        DayCheckOT = dateCheck,
                        DMOvertime = "390",
                        ValueOT = timeOT - 0.5
                    });
                }
                else if (ngayLviec == "CN")
                {
                    lstResult.Add(new OvertimeValue()
                    {
                        DayCheckOT = dateCheck,
                        DMOvertime = "270",
                        ValueOT = timeOT - 0.5 // O.5H di an
                    });
                }
            }
            else if (DateTime.ParseExact(firstTime, "HH:mm:ss", CultureInfo.InvariantCulture) >= DateTime.ParseExact("06:00:00", "HH:mm:ss", CultureInfo.InvariantCulture) &&
                     DateTime.ParseExact(lastTime, "HH:mm:ss", CultureInfo.InvariantCulture) > DateTime.ParseExact("06:00:00", "HH:mm:ss", CultureInfo.InvariantCulture))
            {
                double timeOT = (DateTime.ParseExact(dateCheck + " " + lastTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) - DateTime.ParseExact(dateCheck + " " + firstTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)).TotalHours;
                if (ngayLviec == "NLCC") // ngay le cuoi cung
                {
                    lstResult.Add(new OvertimeValue()
                    {
                        DayCheckOT = dateCheck,
                        DMOvertime = "150",
                        ValueOT = timeOT
                    });
                }
                else if (ngayLviec == "NL") // NGAY LE
                {
                    lstResult.Add(new OvertimeValue()
                    {
                        DayCheckOT = dateCheck,
                        DMOvertime = "300",
                        ValueOT = timeOT
                    });
                }
                else if (ngayLviec == "CN")
                {
                    lstResult.Add(new OvertimeValue()
                    {
                        DayCheckOT = dateCheck,
                        DMOvertime = "200",
                        ValueOT = timeOT
                    });
                }
            }
            else if (DateTime.ParseExact(firstTime, "HH:mm:ss", CultureInfo.InvariantCulture) >= DateTime.ParseExact("20:00:00", "HH:mm:ss", CultureInfo.InvariantCulture) &&
                    DateTime.ParseExact(lastTime, "HH:mm:ss", CultureInfo.InvariantCulture) <= DateTime.ParseExact("23:59:59", "HH:mm:ss", CultureInfo.InvariantCulture))
            {
                double timeOT1 = (DateTime.ParseExact(dateCheck + " " + "22:00:00", "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) - DateTime.ParseExact(dateCheck + " " + firstTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)).TotalHours; // 20 ->22
                double timeOT2 = (DateTime.ParseExact(dateCheck + " " + lastTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) - DateTime.ParseExact(dateCheck + " " + "22:00:00", "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)).TotalHours; // 22 -> 00

                if (ngayLviec == "CN") // ngay chu nhat
                {
                    lstResult.Add(new OvertimeValue()
                    {
                        DayCheckOT = dateCheck,
                        DMOvertime = "200",
                        ValueOT = timeOT1
                    });
                }
                else if (ngayLviec == "NLCC" || ngayLviec == "NL") // Ngay le, ngay le cuoi cung
                {
                    lstResult.Add(new OvertimeValue()
                    {
                        DayCheckOT = dateCheck,
                        DMOvertime = "300",
                        ValueOT = timeOT1
                    });
                }

                if (ngayLviec == "NLCC" || ngayLviec == "NL")
                {
                    lstResult.Add(new OvertimeValue()
                    {
                        DayCheckOT = dateCheck,
                        DMOvertime = "390",
                        ValueOT = timeOT2
                    });
                }
                else if (ngayLviec == "CN")
                {
                    lstResult.Add(new OvertimeValue()
                    {
                        DayCheckOT = dateCheck,
                        DMOvertime = "270",
                        ValueOT = timeOT2
                    });
                }
            }
            else if (DateTime.ParseExact(firstTime, "HH:mm:ss", CultureInfo.InvariantCulture) >= DateTime.ParseExact("20:00:00", "HH:mm:ss", CultureInfo.InvariantCulture) && // 20 -> 06
                   DateTime.ParseExact(lastTime, "HH:mm:ss", CultureInfo.InvariantCulture) <= DateTime.ParseExact("06:00:00", "HH:mm:ss", CultureInfo.InvariantCulture))
            {
                double timeOT1 = (DateTime.ParseExact(dateCheck + " " + "22:00:00", "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) - DateTime.ParseExact(dateCheck + " " + firstTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)).TotalHours; // 20 ->22
                double timeOT2 = 2; // 22 -> 00
                double timeOT3 = (DateTime.ParseExact(dateCheck + " " + lastTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) - DateTime.ParseExact(dateCheck + " " + "00:00:00", "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)).TotalHours; // 00 -> 06

                if (ngayLviec == "CN") // ngay chu nhat
                {
                    lstResult.Add(new OvertimeValue()
                    {
                        DayCheckOT = dateCheck,
                        DMOvertime = "200",
                        ValueOT = timeOT1
                    });
                }
                else if (ngayLviec == "NLCC" || ngayLviec == "NL") // Ngay le, ngay le cuoi cung
                {
                    lstResult.Add(new OvertimeValue()
                    {
                        DayCheckOT = dateCheck,
                        DMOvertime = "300",
                        ValueOT = timeOT1
                    });
                }

                if (ngayLviec == "NLCC" || ngayLviec == "NL")
                {
                    lstResult.Add(new OvertimeValue()
                    {
                        DayCheckOT = dateCheck,
                        DMOvertime = "390",
                        ValueOT = timeOT2
                    });
                }
                else if (ngayLviec == "CN")
                {
                    lstResult.Add(new OvertimeValue()
                    {
                        DayCheckOT = dateCheck,
                        DMOvertime = "270",
                        ValueOT = timeOT2
                    });
                }

                if (ngayLviec == "NLCC") // ngay le cuoi cung
                {
                    lstResult.Add(new OvertimeValue()
                    {
                        DayCheckOT = dateCheck,
                        DMOvertime = "200",
                        ValueOT = timeOT3 - 0.5
                    });
                }
                else if (ngayLviec == "NL") // NGAY LE
                {
                    lstResult.Add(new OvertimeValue()
                    {
                        DayCheckOT = dateCheck,
                        DMOvertime = "390",
                        ValueOT = timeOT3 - 0.5
                    });
                }
                else if (ngayLviec == "CN")
                {
                    lstResult.Add(new OvertimeValue()
                    {
                        DayCheckOT = dateCheck,
                        DMOvertime = "270",
                        ValueOT = timeOT3 - 0.5 // O.5H di an
                    });
                }
            }
            else if (DateTime.ParseExact(firstTime, "HH:mm:ss", CultureInfo.InvariantCulture) >= DateTime.ParseExact("20:00:00", "HH:mm:ss", CultureInfo.InvariantCulture) && // 20 -> 08
                    DateTime.ParseExact(lastTime, "HH:mm:ss", CultureInfo.InvariantCulture) > DateTime.ParseExact("06:00:00", "HH:mm:ss", CultureInfo.InvariantCulture))
            {
                double timeOT1 = (DateTime.ParseExact(dateCheck + " " + "22:00:00", "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) - DateTime.ParseExact(dateCheck + " " + firstTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)).TotalHours; // 20 ->22
                double timeOT2 = 2; // 22 -> 00
                double timeOT3 = 4.5; // 00 -> 06 , 5h - tru 0.5 di an
                double timeOT4 = (DateTime.ParseExact(dateCheck + " " + lastTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) - DateTime.ParseExact(dateCheck + " " + "06:00:00", "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)).TotalHours; // 06 -> 08

                // 20 -> 22
                if (ngayLviec == "CN") // ngay chu nhat
                {
                    lstResult.Add(new OvertimeValue()
                    {
                        DayCheckOT = dateCheck,
                        DMOvertime = "200",
                        ValueOT = timeOT1
                    });
                }
                else if (ngayLviec == "NLCC" || ngayLviec == "NL") // Ngay le, ngay le cuoi cung
                {
                    lstResult.Add(new OvertimeValue()
                    {
                        DayCheckOT = dateCheck,
                        DMOvertime = "300",
                        ValueOT = timeOT1
                    });
                }

                // 22-> 0
                if (ngayLviec == "NLCC" || ngayLviec == "NL")
                {
                    lstResult.Add(new OvertimeValue()
                    {
                        DayCheckOT = dateCheck,
                        DMOvertime = "390",
                        ValueOT = timeOT2
                    });
                }
                else if (ngayLviec == "CN")
                {
                    lstResult.Add(new OvertimeValue()
                    {
                        DayCheckOT = dateCheck,
                        DMOvertime = "270",
                        ValueOT = timeOT2
                    });
                }

                // 0 -> 6
                if (ngayLviec == "NLCC") // ngay le cuoi cung
                {
                    lstResult.Add(new OvertimeValue()
                    {
                        DayCheckOT = dateCheck,
                        DMOvertime = "200",
                        ValueOT = timeOT3 - 0.5
                    });
                }
                else if (ngayLviec == "NL") // NGAY LE
                {
                    lstResult.Add(new OvertimeValue()
                    {
                        DayCheckOT = dateCheck,
                        DMOvertime = "390",
                        ValueOT = timeOT3 - 0.5
                    });
                }
                else if (ngayLviec == "CN")
                {
                    lstResult.Add(new OvertimeValue()
                    {
                        DayCheckOT = dateCheck,
                        DMOvertime = "270",
                        ValueOT = timeOT3 - 0.5 // O.5H di an
                    });
                }

                // 6-8h
                if (ngayLviec == "NLCC") // ngay le cuoi cung
                {
                    lstResult.Add(new OvertimeValue()
                    {
                        DayCheckOT = dateCheck,
                        DMOvertime = "150",
                        ValueOT = timeOT4
                    });
                }
                else if (ngayLviec == "NL") // NGAY LE
                {
                    lstResult.Add(new OvertimeValue()
                    {
                        DayCheckOT = dateCheck,
                        DMOvertime = "300",
                        ValueOT = timeOT4
                    });
                }
                else if (ngayLviec == "CN")
                {
                    lstResult.Add(new OvertimeValue()
                    {
                        DayCheckOT = dateCheck,
                        DMOvertime = "200",
                        ValueOT = timeOT4
                    });
                }
            }
            else if (DateTime.ParseExact(firstTime, "HH:mm:ss", CultureInfo.InvariantCulture) >= DateTime.ParseExact("22:00:00", "HH:mm:ss", CultureInfo.InvariantCulture) && // 22 -> 06
                 DateTime.ParseExact(lastTime, "HH:mm:ss", CultureInfo.InvariantCulture) <= DateTime.ParseExact("06:00:00", "HH:mm:ss", CultureInfo.InvariantCulture))
            {
                double timeOT1 = (DateTime.ParseExact(dateCheck + " " + "23:59:59", "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) - DateTime.ParseExact(dateCheck + " " + firstTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)).TotalHours; // 22-> 00
                double timeOT2 = (DateTime.ParseExact(dateCheck + " " + lastTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) - DateTime.ParseExact(dateCheck + " " + "00:00:00", "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)).TotalHours; // 00 -> 06

                // 22 -> 0
                if (ngayLviec == "NLCC" || ngayLviec == "NL") // ngay chu nhat
                {
                    lstResult.Add(new OvertimeValue()
                    {
                        DayCheckOT = dateCheck,
                        DMOvertime = "390",
                        ValueOT = timeOT1
                    });
                }
                else if (ngayLviec == "CN")
                {
                    lstResult.Add(new OvertimeValue()
                    {
                        DayCheckOT = dateCheck,
                        DMOvertime = "270",
                        ValueOT = timeOT1
                    });
                }

                // 00 -> 06
                if (ngayLviec == "NLCC") // ngay le cuoi cung
                {
                    lstResult.Add(new OvertimeValue()
                    {
                        DayCheckOT = dateCheck,
                        DMOvertime = "200",
                        ValueOT = timeOT2 - 0.5
                    });
                }
                else if (ngayLviec == "NL") // NGAY LE
                {
                    lstResult.Add(new OvertimeValue()
                    {
                        DayCheckOT = dateCheck,
                        DMOvertime = "390",
                        ValueOT = timeOT2 - 0.5
                    });
                }
                else if (ngayLviec == "CN")
                {
                    lstResult.Add(new OvertimeValue()
                    {
                        DayCheckOT = dateCheck,
                        DMOvertime = "270",
                        ValueOT = timeOT2 - 0.5 // O.5H di an
                    });
                }
            }
            else if (DateTime.ParseExact(firstTime, "HH:mm:ss", CultureInfo.InvariantCulture) >= DateTime.ParseExact("22:00:00", "HH:mm:ss", CultureInfo.InvariantCulture) && // 22 -> 08
                 DateTime.ParseExact(lastTime, "HH:mm:ss", CultureInfo.InvariantCulture) > DateTime.ParseExact("06:00:00", "HH:mm:ss", CultureInfo.InvariantCulture))
            {
                double timeOT2 = (DateTime.ParseExact(dateCheck + " " + "23:59:59", "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) - DateTime.ParseExact(dateCheck + " " + firstTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)).TotalHours; // 22 -> 00
                double timeOT3 = 4.5; // 00 -> 06 , 5h - tru 0.5 di an
                double timeOT4 = (DateTime.ParseExact(dateCheck + " " + lastTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) - DateTime.ParseExact(dateCheck + " " + "06:00:00", "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)).TotalHours; // 06 -> 08

                // 22-> 0
                if (ngayLviec == "NLCC" || ngayLviec == "NL")
                {
                    lstResult.Add(new OvertimeValue()
                    {
                        DayCheckOT = dateCheck,
                        DMOvertime = "390",
                        ValueOT = timeOT2
                    });
                }
                else if (ngayLviec == "CN")
                {
                    lstResult.Add(new OvertimeValue()
                    {
                        DayCheckOT = dateCheck,
                        DMOvertime = "270",
                        ValueOT = timeOT2
                    });
                }

                // 0 -> 6
                if (ngayLviec == "NLCC") // ngay le cuoi cung
                {
                    lstResult.Add(new OvertimeValue()
                    {
                        DayCheckOT = dateCheck,
                        DMOvertime = "200",
                        ValueOT = timeOT3 - 0.5
                    });
                }
                else if (ngayLviec == "NL") // NGAY LE
                {
                    lstResult.Add(new OvertimeValue()
                    {
                        DayCheckOT = dateCheck,
                        DMOvertime = "390",
                        ValueOT = timeOT3 - 0.5
                    });
                }
                else if (ngayLviec == "CN")
                {
                    lstResult.Add(new OvertimeValue()
                    {
                        DayCheckOT = dateCheck,
                        DMOvertime = "270",
                        ValueOT = timeOT3 - 0.5 // O.5H di an
                    });
                }

                // 6-8h
                if (ngayLviec == "NLCC") // ngay le cuoi cung
                {
                    lstResult.Add(new OvertimeValue()
                    {
                        DayCheckOT = dateCheck,
                        DMOvertime = "150",
                        ValueOT = timeOT4
                    });
                }
                else if (ngayLviec == "NL") // NGAY LE
                {
                    lstResult.Add(new OvertimeValue()
                    {
                        DayCheckOT = dateCheck,
                        DMOvertime = "300",
                        ValueOT = timeOT4
                    });
                }
                else if (ngayLviec == "CN")
                {
                    lstResult.Add(new OvertimeValue()
                    {
                        DayCheckOT = dateCheck,
                        DMOvertime = "200",
                        ValueOT = timeOT4
                    });
                }
            }
            else if (DateTime.ParseExact(firstTime, "HH:mm:ss", CultureInfo.InvariantCulture) >= DateTime.ParseExact("00:00:00", "HH:mm:ss", CultureInfo.InvariantCulture) && // 00 -> 08
               DateTime.ParseExact(lastTime, "HH:mm:ss", CultureInfo.InvariantCulture) > DateTime.ParseExact("06:00:00", "HH:mm:ss", CultureInfo.InvariantCulture))
            {
                double timeOT2 = (DateTime.ParseExact(dateCheck + " " + "06:00:00", "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) - DateTime.ParseExact(dateCheck + " " + firstTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)).TotalHours; // 00 -> 06
                double timeOT4 = (DateTime.ParseExact(dateCheck + " " + lastTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) - DateTime.ParseExact(dateCheck + " " + "06:00:00", "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)).TotalHours; // 06 -> 08

                // 0 -> 6
                if (ngayLviec == "NLCC") // ngay le cuoi cung
                {
                    lstResult.Add(new OvertimeValue()
                    {
                        DayCheckOT = dateCheck,
                        DMOvertime = "200",
                        ValueOT = timeOT2 - 0.5
                    });
                }
                else if (ngayLviec == "NL") // NGAY LE
                {
                    lstResult.Add(new OvertimeValue()
                    {
                        DayCheckOT = dateCheck,
                        DMOvertime = "390",
                        ValueOT = timeOT2 - 0.5
                    });
                }
                else if (ngayLviec == "CN")
                {
                    lstResult.Add(new OvertimeValue()
                    {
                        DayCheckOT = dateCheck,
                        DMOvertime = "270",
                        ValueOT = timeOT2 - 0.5 // O.5H di an
                    });
                }

                // 6-8h
                if (ngayLviec == "NLCC") // ngay le cuoi cung
                {
                    lstResult.Add(new OvertimeValue()
                    {
                        DayCheckOT = dateCheck,
                        DMOvertime = "150",
                        ValueOT = timeOT4
                    });
                }
                else if (ngayLviec == "NL") // NGAY LE
                {
                    lstResult.Add(new OvertimeValue()
                    {
                        DayCheckOT = dateCheck,
                        DMOvertime = "300",
                        ValueOT = timeOT4
                    });
                }
                else if (ngayLviec == "CN")
                {
                    lstResult.Add(new OvertimeValue()
                    {
                        DayCheckOT = dateCheck,
                        DMOvertime = "200",
                        ValueOT = timeOT4
                    });
                }
            }

            List<OvertimeValue> overtimes = new List<OvertimeValue>();
            OvertimeValue oval;
            foreach (var item in lstResult)
            {
                if (overtimes.Any(x => x.DMOvertime != item.DMOvertime))
                {
                    oval = overtimes.Find(x => x.DMOvertime != item.DMOvertime);
                    oval.ValueOT += item.ValueOT;
                }
                else
                {
                    overtimes.Add(item);
                }
            }

            return overtimes;
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

        // ngay ki niem cong ty
        private string IsNgayDacBiet(string time)
        {
            var ngaydacbiet = _ngaydacbietRespository.FindSingle(x => time.Contains(x.TenNgayDacBiet));
            if (ngaydacbiet != null)
            {
                return time;
            }
            return "";
        }

        private int CheckNgayDB(string time)
        {
            if (IsNgayLe(time) != "")
            {
                return 1;
            }

            if (IsNgayTruocLe(time) != "")
            {
                return 2;
            }

            if (IsChuNhat(time))
            {
                return 3;
            }

            if (IsNgayNghiBuLeNam(time) != "")
            {
                return 4;
            }

            if (IsNgayDacBiet(time) != "")
            {
                return 5;
            }

            return 0;// ngay thuong
        }
    }
}
