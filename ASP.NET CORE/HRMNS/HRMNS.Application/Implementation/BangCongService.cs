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
        private IRespository<HR_HOPDONG, int> _hopDongResponsitory;

        private EFUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BangCongService(IRespository<ATTENDANCE_RECORD, long> attendance_record, IRespository<NGAY_LE_NAM,
            string> ngayleRespository, IRespository<NGAY_NGHI_BU_LE_NAM, int> ngaynghibuRespository, IRespository<NGAY_DAC_BIET,
                string> ngayDacBietRespository, IRespository<ATTENDANCE_OVERTIME, int> attendance_overtime,
            IRespository<CHAM_CONG_LOG, long> chamCongRespository,
            IRespository<HR_HOPDONG, int> hopDongResponsitory,
            IRespository<CA_LVIEC, int> calaviecRespository,
            IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _attendanceRecordRespository = attendance_record;
            _attendanceOvertimeRespository = attendance_overtime;
            _chamCongRespository = chamCongRespository;
            _ngaylenamRespository = ngayleRespository;
            _ngaydacbietRespository = ngayDacBietRespository;
            _ngaynghibuRespository = ngaynghibuRespository;
            _hopDongResponsitory = hopDongResponsitory;
            _calaviecRespository = calaviecRespository;
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

        public List<ChamCongDataViewModel> GetDataReport(string time, string dept)
        {
            time += "-01";
            var lstResult = new List<ChamCongDataViewModel>();

            // Lấy thông tin hệ số OT của ca làm việc.
            var hsoOvertimes = _mapper.Map<List<CaLamViecViewModel>>(_calaviecRespository.FindAll());

            List<string> lstDanhMucOT = new List<string>();
            foreach (var item in hsoOvertimes.OrderBy(x => x.HeSo_OT))
            {
                if (item.HeSo_OT.NullString() != "" && !lstDanhMucOT.Contains(item.HeSo_OT.NullString() + "%") && item.HeSo_OT > 100)
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

            Dictionary<string, string> dicpamram = new Dictionary<string, string>();
            dicpamram.Add("A_DATE_TIME", time.NullString());
            dicpamram.Add("A_DEPT", dept.NullString());

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
                        foreach (DataRow row in chamCongDB.Rows)
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
                    bool isRegistedOT;
                    string kyhieuChamCongDB;

                    // thoi gian bat dau , ket thuc ngay thuong ca ngay.
                    string beginTimeCaNgay = _calaviecRespository.FindSingle(x => x.Danhmuc_CaLviec == "CN_WHC" && x.DM_NgayLViec == "NT" && x.HeSo_OT == 100).Time_BatDau;
                    string endTimeCaNgay = _calaviecRespository.FindSingle(x => x.Danhmuc_CaLviec == "CN_WHC" && x.DM_NgayLViec == "NT" && x.HeSo_OT == 100).Time_KetThuc;

                    for (int i = 1; i <= DateTime.Parse(endMonth).Day; i++) // day 1 -> 31 or end month
                    {
                        dateCheck = (new DateTime(DateTime.Parse(endMonth).Year, DateTime.Parse(endMonth).Month, i)).ToString("yyyy-MM-dd");

                        foreach (var item in lstResult)
                        {
                            // check hop dong
                            hopDong_NV = item.lstHopDong.FirstOrDefault(x => string.Compare(dateCheck, x.NgayHieuLucHD) >= 0 && string.Compare(dateCheck, x.NgayHetHLHD) <= 0);

                            if (hopDong_NV == null)
                            {
                                hopDong_NV = new HopDong_NV();

                                var hd = _hopDongResponsitory.FindAll(x => x.HR_LOAIHOPDONG).OrderByDescending(x => x.NgayHieuLuc).FirstOrDefault();
                                if (hd != null)
                                {
                                    if (hd.HR_LOAIHOPDONG.ShortName == CommonConstants.HD_THUVIEC_EM || hd.HR_LOAIHOPDONG.ShortName == CommonConstants.HD_THUVIEC_OP)
                                    {
                                        hopDong_NV.ShortName = CommonConstants.HD_MOT_NAM_L1;
                                    }
                                    else
                                    {
                                        hopDong_NV.ShortName = hd.HR_LOAIHOPDONG.ShortName;
                                    }
                                }
                                else
                                {
                                    if (item.BoPhan == CommonConstants.SUPPORT_DEPT)
                                    {
                                        hopDong_NV.ShortName = CommonConstants.HD_THUVIEC_EM;
                                    }
                                    else
                                    {
                                        hopDong_NV.ShortName = CommonConstants.HD_THUVIEC_OP;
                                    }
                                }
                            }

                            isRegistedOT = item.lstDangKyOT.Any(x => x.NgayOT == dateCheck);

                            kyhieuChamCongDB = item.lstChamCongDB.FirstOrDefault(x => string.Compare(dateCheck, x.NgayBatDau) >= 0 && string.Compare(dateCheck, x.NgayKetThuc) <= 0)?.KyHieuChamCong;

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
                                        firstTime = _chamCongLog.FirstIn_Time.NullString();
                                        lastTime = _chamCongLog.Last_Out_Time.NullString();

                                        if (_caLamViec.MaCaLaviec == CommonConstants.CA_DEM)
                                        {
                                            // bat đầu ca và kết thúc ca sẽ đc tính nguyên lương
                                            bool isBatDauCa = item.lstNhanVienCaLamViec.Any(x => string.Compare(dateCheck, x.BatDau_TheoCa) == 0);
                                            bool isKetThucCa = item.lstNhanVienCaLamViec.Any(x => string.Compare(dateCheck, x.KetThuc_TheoCa) == 0);

                                            if (isBatDauCa)
                                            {
                                                lastTime = "05:00:00";
                                                _chamCongLog.FirstIn = CommonConstants.IN;
                                                _chamCongLog.LastOut = CommonConstants.OUT;
                                            }

                                            if (isKetThucCa)
                                            {
                                                firstTime = "20:00:00";
                                                _chamCongLog.FirstIn = CommonConstants.IN;
                                                _chamCongLog.LastOut = CommonConstants.OUT;
                                            }
                                        }

                                        // Co du lieu cham cong
                                        if (_chamCongLog.FirstIn.NullString() == CommonConstants.IN && _chamCongLog.LastOut.NullString() == CommonConstants.OUT)
                                        {
                                            #region THU VIEC + CA NGAY
                                            if (_caLamViec.MaCaLaviec == CommonConstants.CA_NGAY)
                                            {
                                                // 1. CHECK NGAY CONG : thu viec + ca ngay 
                                                if (DateTime.ParseExact(firstTime, "HH:mm:ss", CultureInfo.InvariantCulture) < DateTime.ParseExact(beginTimeCaNgay, "HH:mm:ss", CultureInfo.InvariantCulture))
                                                {
                                                    firstTime = beginTimeCaNgay;
                                                }

                                                if (CheckNgayDB(dateCheck) == 0) // ngay thuong
                                                {
                                                    string newBeginOT = "17:45:00";
                                                    if (item.BoPhan != CommonConstants.SUPPORT_DEPT)
                                                    {
                                                        newBeginOT = "17:30:00";
                                                    }

                                                    if (item.BoPhan == CommonConstants.SUPPORT_DEPT && DateTime.Parse(dateCheck).DayOfWeek == DayOfWeek.Saturday)
                                                    {
                                                        newBeginOT = "13:15:00";
                                                    }

                                                    if (string.Compare(lastTime, newBeginOT) > 0)
                                                    {
                                                        var clviec = item.lstNhanVienCaLamViec.FindAll(x =>
                                                        string.Compare(dateCheck, x.BatDau_TheoCa) >= 0 &&
                                                        string.Compare(dateCheck, x.KetThuc_TheoCa) <= 0 &&
                                                        x.MaCaLaviec == CommonConstants.CA_NGAY && x.DM_NgayLViec == "NT" && x.HeSo_OT != 100).OrderByDescending(x => x.Time_BatDau).FirstOrDefault();

                                                        double timeOT = (DateTime.ParseExact(dateCheck + " " + lastTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) - DateTime.ParseExact(dateCheck + " " + newBeginOT, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)).TotalHours;

                                                        if (timeOT < 0.1)
                                                        {
                                                            timeOT = 0;
                                                        }

                                                        if (timeOT > 0)
                                                        {
                                                            item.OvertimeValues.Add(new OvertimeValue()
                                                            {
                                                                DayCheckOT = dateCheck,
                                                                DMOvertime = clviec.HeSo_OT.NullString(),
                                                                ValueOT = timeOT,
                                                                Registered = isRegistedOT
                                                            });
                                                        }

                                                    }

                                                    item.WorkingStatuses.Add(new WorkingStatus()
                                                    {
                                                        DayCheck = dateCheck,
                                                        Value = kyhieuChamCongDB.NullString() == "" ? "PD" : kyhieuChamCongDB.NullString() // PD: Probation Day shift/Thử việc ca ngày
                                                    });
                                                }
                                                else if (CheckNgayDB(dateCheck) == 5) // Ngay ki niem cty
                                                {
                                                    var clviec = item.lstNhanVienCaLamViec.FirstOrDefault(x =>
                                                        string.Compare(dateCheck, x.BatDau_TheoCa) >= 0 &&
                                                        string.Compare(dateCheck, x.KetThuc_TheoCa) <= 0 &&
                                                        x.MaCaLaviec == CommonConstants.CA_NGAY && x.DM_NgayLViec == "CN" && x.HeSo_OT != 100); // Ngay ki niem coi nhu ngay chu nhat

                                                    double timeOT = (DateTime.ParseExact(dateCheck + " " + lastTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) - DateTime.ParseExact(dateCheck + " " + firstTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)).TotalHours;
                                                    timeOT = timeOT <= 8 ? timeOT - 0.5 : timeOT - 1.5; // 1h nghi trua + 0.5h di an

                                                    if (timeOT < 0)
                                                    {
                                                        timeOT = 0;
                                                    }

                                                    item.WorkingStatuses.Add(new WorkingStatus()
                                                    {
                                                        DayCheck = dateCheck,
                                                        Value = kyhieuChamCongDB.NullString() == "" ? "PMD" : kyhieuChamCongDB.NullString() // Làm ca ngày thử việc ngay ki niem
                                                    });

                                                    item.OvertimeValues.Add(new OvertimeValue()
                                                    {
                                                        DayCheckOT = dateCheck,
                                                        DMOvertime = clviec.HeSo_OT.NullString(), // nhu OT chu nhat
                                                        ValueOT = timeOT,
                                                        Registered = isRegistedOT
                                                    });
                                                }
                                                else if (CheckNgayDB(dateCheck) == 3 || CheckNgayDB(dateCheck) == 4) // ngay chu nhat or nghi bu ngay le
                                                {
                                                    var clviec = item.lstNhanVienCaLamViec.FirstOrDefault(x =>
                                                         string.Compare(dateCheck, x.BatDau_TheoCa) >= 0 &&
                                                         string.Compare(dateCheck, x.KetThuc_TheoCa) <= 0 &&
                                                         x.MaCaLaviec == CommonConstants.CA_NGAY && x.DM_NgayLViec == "CN" && x.HeSo_OT != 100);

                                                    double timeOT = (DateTime.ParseExact(dateCheck + " " + lastTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) - DateTime.ParseExact(dateCheck + " " + firstTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)).TotalHours;
                                                    timeOT = timeOT <= 8 ? timeOT - 0.5 : timeOT - 1.5;
                                                    if (timeOT < 0)
                                                    {
                                                        timeOT = 0;
                                                    }

                                                    item.WorkingStatuses.Add(new WorkingStatus()
                                                    {
                                                        DayCheck = dateCheck,
                                                        Value = kyhieuChamCongDB.NullString() == "" ? "TV" : kyhieuChamCongDB.NullString() // TV: Thử việc làm thêm ngày chủ nhật/ Probation
                                                    });

                                                    item.OvertimeValues.Add(new OvertimeValue()
                                                    {
                                                        DayCheckOT = dateCheck,
                                                        DMOvertime = clviec.HeSo_OT.NullString(), // nhu OT chu nhat
                                                        ValueOT = timeOT,
                                                        Registered = isRegistedOT
                                                    });
                                                }
                                                else if (CheckNgayDB(dateCheck) == 1) // ngay le
                                                {
                                                    var clviec = item.lstNhanVienCaLamViec.FirstOrDefault(x =>
                                                     string.Compare(dateCheck, x.BatDau_TheoCa) >= 0 &&
                                                     string.Compare(dateCheck, x.KetThuc_TheoCa) <= 0 &&
                                                     x.MaCaLaviec == CommonConstants.CA_NGAY &&
                                                     (x.DM_NgayLViec == "NL" || x.DM_NgayLViec == "NLCC") && x.HeSo_OT != 100);

                                                    double timeOT = (DateTime.ParseExact(dateCheck + " " + lastTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) - DateTime.ParseExact(dateCheck + " " + firstTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)).TotalHours;
                                                    timeOT = timeOT <= 8 ? timeOT - 0.5 : timeOT - 1.5;
                                                    if (timeOT < 0)
                                                    {
                                                        timeOT = 0;
                                                    }

                                                    item.WorkingStatuses.Add(new WorkingStatus()
                                                    {
                                                        DayCheck = dateCheck,
                                                        Value = kyhieuChamCongDB.NullString() == "" ? "PD" : kyhieuChamCongDB.NullString(), // PD: Probation Day shift/Thử việc ca ngày
                                                    });

                                                    item.OvertimeValues.Add(new OvertimeValue()
                                                    {
                                                        DayCheckOT = dateCheck,
                                                        DMOvertime = clviec.HeSo_OT.NullString(),
                                                        ValueOT = timeOT,
                                                        Registered = isRegistedOT
                                                    });
                                                }
                                                else if (CheckNgayDB(dateCheck) == 2) // ngay truoc le
                                                {
                                                    var clviec = item.lstNhanVienCaLamViec.FirstOrDefault(x =>
                                                     string.Compare(dateCheck, x.BatDau_TheoCa) >= 0 &&
                                                     string.Compare(dateCheck, x.KetThuc_TheoCa) <= 0 &&
                                                     x.MaCaLaviec == CommonConstants.CA_NGAY &&
                                                     (x.DM_NgayLViec == "TNL") && x.HeSo_OT != 100);

                                                    string newBeginOT = "17:45:00";
                                                    if (string.Compare(lastTime, newBeginOT) > 0)
                                                    {
                                                        if (item.BoPhan != "SP")
                                                        {
                                                            newBeginOT = "17:30:00";
                                                        }

                                                        double timeOT = (DateTime.ParseExact(dateCheck + " " + lastTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) - DateTime.ParseExact(dateCheck + " " + newBeginOT, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)).TotalHours;
                                                        if (timeOT < 0.1)
                                                        {
                                                            timeOT = 0;
                                                        }

                                                        if (timeOT > 0)
                                                        {
                                                            item.OvertimeValues.Add(new OvertimeValue()
                                                            {
                                                                DayCheckOT = dateCheck,
                                                                DMOvertime = clviec.HeSo_OT.NullString(),
                                                                ValueOT = timeOT,
                                                                Registered = isRegistedOT
                                                            });
                                                        }
                                                    }

                                                    item.WorkingStatuses.Add(new WorkingStatus()
                                                    {
                                                        DayCheck = dateCheck,
                                                        Value = kyhieuChamCongDB.NullString() == "" ? "PD" : kyhieuChamCongDB.NullString() // PD: Probation Day shift/Thử việc ca ngày
                                                    });
                                                }

                                                // Di muon ve som
                                                double ELLC = 0;
                                                if (DateTime.ParseExact(firstTime, "HH:mm:ss", CultureInfo.InvariantCulture) > DateTime.ParseExact("08:05:00", "HH:mm:ss", CultureInfo.InvariantCulture))
                                                {
                                                    ELLC += (DateTime.ParseExact(firstTime, "HH:mm:ss", CultureInfo.InvariantCulture) - DateTime.ParseExact("08:05:00", "HH:mm:ss", CultureInfo.InvariantCulture)).TotalHours;
                                                }

                                                if (item.BoPhan == CommonConstants.SUPPORT_DEPT && DateTime.Parse(dateCheck).DayOfWeek == DayOfWeek.Saturday)
                                                {
                                                    if (DateTime.ParseExact(lastTime, "HH:mm:ss", CultureInfo.InvariantCulture) < DateTime.ParseExact("13:15:00", "HH:mm:ss", CultureInfo.InvariantCulture))
                                                    {
                                                        ELLC += (DateTime.ParseExact("13:15:00", "HH:mm:ss", CultureInfo.InvariantCulture) - DateTime.ParseExact(lastTime, "HH:mm:ss", CultureInfo.InvariantCulture)).TotalHours;
                                                    }
                                                }
                                                else
                                                {
                                                    if (DateTime.ParseExact(lastTime, "HH:mm:ss", CultureInfo.InvariantCulture) < DateTime.ParseExact("17:00:00", "HH:mm:ss", CultureInfo.InvariantCulture))
                                                    {
                                                        ELLC += (DateTime.ParseExact("17:00:00", "HH:mm:ss", CultureInfo.InvariantCulture) - DateTime.ParseExact(lastTime, "HH:mm:ss", CultureInfo.InvariantCulture)).TotalHours;
                                                    }
                                                }

                                                if (ELLC < 0 || !string.IsNullOrEmpty(kyhieuChamCongDB.NullString()))
                                                {
                                                    ELLC = 0;
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
                                                    var clviecs = item.lstNhanVienCaLamViec.FindAll(x =>
                                                     string.Compare(dateCheck, x.BatDau_TheoCa) >= 0 &&
                                                     string.Compare(dateCheck, x.KetThuc_TheoCa) <= 0 &&
                                                     x.MaCaLaviec == CommonConstants.CA_DEM &&
                                                     (x.DM_NgayLViec == "NT") && x.HeSo_OT != 100);

                                                    string hsOT1 = clviecs.FirstOrDefault(x => x.Time_KetThuc == "06:00:00").HeSo_OT.NullString();
                                                    string hsOT2 = clviecs.FirstOrDefault(x => x.Time_KetThuc == "08:00:00").HeSo_OT.NullString();

                                                    string newBeginOT = "05:30:00";
                                                    if (string.Compare(lastTime, newBeginOT) > 0)
                                                    {
                                                        double timeOT = (DateTime.ParseExact(dateCheck + " " + lastTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) - DateTime.ParseExact(dateCheck + " " + newBeginOT, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)).TotalHours;
                                                        if (timeOT < 0.1)
                                                        {
                                                            timeOT = 0;
                                                        }

                                                        if (timeOT > 0 && timeOT <= 0.5)
                                                        {
                                                            item.OvertimeValues.Add(new OvertimeValue()
                                                            {
                                                                DayCheckOT = dateCheck,
                                                                DMOvertime = hsOT1, // 200 % : 5H30 -> 6H
                                                                ValueOT = timeOT,
                                                                Registered = isRegistedOT
                                                            });
                                                        }
                                                        else if (timeOT > 0.5)
                                                        {
                                                            item.OvertimeValues.Add(new OvertimeValue()
                                                            {
                                                                DayCheckOT = dateCheck,
                                                                DMOvertime = hsOT1, // 200 % : 5H30 -> 6H
                                                                ValueOT = 0.5,
                                                                Registered = isRegistedOT
                                                            });

                                                            item.OvertimeValues.Add(new OvertimeValue()
                                                            {
                                                                DayCheckOT = dateCheck,
                                                                DMOvertime = hsOT2, // 150 % :6h -> 8H
                                                                ValueOT = timeOT - 0.5, // tru di 0.5h cua 5-6h
                                                                Registered = isRegistedOT
                                                            });
                                                        }
                                                    }

                                                    item.WorkingStatuses.Add(new WorkingStatus()
                                                    {
                                                        DayCheck = dateCheck,
                                                        Value = kyhieuChamCongDB.NullString() == "" ? "PN" : kyhieuChamCongDB.NullString() // PN: Probation Night shift/Thử việc ca đêm
                                                    });
                                                }
                                                else if (CheckNgayDB(dateCheck) == 5) // Ngay ki niem cty
                                                {
                                                    item.OvertimeValues.AddRange(GetOvertimeInNight(firstTime, lastTime, dateCheck, "CN", isRegistedOT));
                                                    item.WorkingStatuses.Add(new WorkingStatus()
                                                    {
                                                        DayCheck = dateCheck,
                                                        Value = kyhieuChamCongDB.NullString() == "" ? "PM" : kyhieuChamCongDB.NullString() // PM: Làm ca đêm ngày kỷ niệm  thử việc
                                                    });
                                                }
                                                else if (CheckNgayDB(dateCheck) == 3 || CheckNgayDB(dateCheck) == 4) // ngay chu nhat + ngay nghi bu
                                                {
                                                    item.OvertimeValues.AddRange(GetOvertimeInNight(firstTime, lastTime, dateCheck, "CN", isRegistedOT));
                                                    item.WorkingStatuses.Add(new WorkingStatus()
                                                    {
                                                        DayCheck = dateCheck,
                                                        Value = kyhieuChamCongDB.NullString() == "" ? "TVD" : kyhieuChamCongDB.NullString() // TVD: Thử việc làm thêm ca đêm chủ nhật
                                                    });
                                                }
                                                else if (CheckNgayDB(dateCheck) == 1) // ngay le
                                                {
                                                    var ngayle = _ngaylenamRespository.FindById(dateCheck);
                                                    if (ngayle.IslastHoliday == CommonConstants.N)
                                                    {
                                                        item.OvertimeValues.AddRange(GetOvertimeInNight(firstTime, lastTime, dateCheck, "NL", isRegistedOT));
                                                    }
                                                    else // ngay le cuoi cung
                                                    {
                                                        item.OvertimeValues.AddRange(GetOvertimeInNight(firstTime, lastTime, dateCheck, "NLCC", isRegistedOT));
                                                    }

                                                    item.WorkingStatuses.Add(new WorkingStatus()
                                                    {
                                                        DayCheck = dateCheck,
                                                        Value = kyhieuChamCongDB.NullString() == "" ? "PDD" : kyhieuChamCongDB.NullString() // PDD: Thử việc làm đêm ngày lễ
                                                    });
                                                }
                                                else if (CheckNgayDB(dateCheck) == 2) // ngay truoc le
                                                {
                                                    item.OvertimeValues.AddRange(GetOvertimeInNight(firstTime, lastTime, dateCheck, "NL", isRegistedOT));
                                                    item.WorkingStatuses.Add(new WorkingStatus()
                                                    {
                                                        DayCheck = dateCheck,
                                                        Value = kyhieuChamCongDB.NullString() == "" ? "PH" : kyhieuChamCongDB.NullString() // PH: làm ca đêm trước ngày lễ( thử việc)
                                                    });
                                                }

                                                // Di muon ve som
                                                double ELLC = 0;
                                                if (DateTime.ParseExact(firstTime, "HH:mm:ss", CultureInfo.InvariantCulture) > DateTime.ParseExact("20:05:00", "HH:mm:ss", CultureInfo.InvariantCulture))
                                                {
                                                    ELLC += (DateTime.ParseExact(firstTime, "HH:mm:ss", CultureInfo.InvariantCulture) - DateTime.ParseExact("20:05:00", "HH:mm:ss", CultureInfo.InvariantCulture)).TotalHours;
                                                }

                                                if (DateTime.ParseExact(lastTime, "HH:mm:ss", CultureInfo.InvariantCulture) < DateTime.ParseExact("05:00:00", "HH:mm:ss", CultureInfo.InvariantCulture))
                                                {
                                                    ELLC += (DateTime.ParseExact("05:00:00", "HH:mm:ss", CultureInfo.InvariantCulture) - DateTime.ParseExact(lastTime, "HH:mm:ss", CultureInfo.InvariantCulture)).TotalHours;
                                                }

                                                if (ELLC < 0 || !string.IsNullOrEmpty(kyhieuChamCongDB.NullString())) ELLC = 0;

                                                item.EL_LC_Statuses.Add(new EL_LC_Status()
                                                {
                                                    DayCheck_EL = dateCheck,
                                                    Value_EL = ELLC
                                                });
                                            }
                                        }
                                        else if (_chamCongLog.FirstIn.NullString() == "" && _chamCongLog.LastOut.NullString() == "")
                                        {
                                            int checkDate = CheckNgayDB(dateCheck);
                                            if (checkDate == 1 || checkDate == 4 || checkDate == 5) // ngay le, nghi bu,ngay db
                                            {
                                                item.WorkingStatuses.Add(new WorkingStatus()
                                                {
                                                    DayCheck = dateCheck,
                                                    Value = kyhieuChamCongDB.NullString() == "" ? "NH" : kyhieuChamCongDB.NullString() // NH: NationALHoliday/ Nghỉ lễ
                                                });
                                            }
                                            else
                                            {
                                                item.WorkingStatuses.Add(new WorkingStatus()
                                                {
                                                    DayCheck = dateCheck,
                                                    Value = kyhieuChamCongDB.NullString() == "" ? "-" : kyhieuChamCongDB.NullString()
                                                });
                                            }

                                            item.EL_LC_Statuses.Add(new EL_LC_Status()
                                            {
                                                DayCheck_EL = dateCheck,
                                                Value_EL = 0
                                            });
                                        }
                                        #endregion
                                    }
                                }
                                else // HD Chinh thuc or tviec 100%
                                {
                                    // check ca lam viec
                                    _caLamViec = item.lstNhanVienCaLamViec.FirstOrDefault(x => string.Compare(dateCheck, x.BatDau_TheoCa) >= 0 && string.Compare(dateCheck, x.KetThuc_TheoCa) <= 0);

                                    if (_caLamViec != null)
                                    {
                                        // get data cham cong log 
                                        _chamCongLog = _chamCongRespository.FindAll(x => dateCheck == x.Ngay_ChamCong && item.MaNV.ToUpper().Contains(x.ID_NV.ToUpper())).FirstOrDefault();

                                        // get first time and last time
                                        firstTime = _chamCongLog.FirstIn_Time.NullString();
                                        lastTime = _chamCongLog.Last_Out_Time.NullString();

                                        if (_caLamViec.MaCaLaviec == CommonConstants.CA_DEM)
                                        {
                                            // bat đầu ca và kết thúc ca sẽ đc tính nguyên lương
                                            bool isBatDauCa = item.lstNhanVienCaLamViec.Any(x => string.Compare(dateCheck, x.BatDau_TheoCa) == 0);
                                            bool isKetThucCa = item.lstNhanVienCaLamViec.Any(x => string.Compare(dateCheck, x.KetThuc_TheoCa) == 0);

                                            if (isBatDauCa)
                                            {
                                                lastTime = "05:00:00";
                                                _chamCongLog.FirstIn = CommonConstants.IN;
                                                _chamCongLog.LastOut = CommonConstants.OUT;
                                            }

                                            if (isKetThucCa)
                                            {
                                                firstTime = "20:00:00";
                                                _chamCongLog.FirstIn = CommonConstants.IN;
                                                _chamCongLog.LastOut = CommonConstants.OUT;
                                            }
                                        }

                                        // Co du lieu cham cong
                                        if (_chamCongLog.FirstIn.NullString() == CommonConstants.IN && _chamCongLog.LastOut.NullString() == CommonConstants.OUT)
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
                                                    if (item.BoPhan == "SP" && DateTime.Parse(dateCheck).DayOfWeek == DayOfWeek.Saturday)
                                                    {
                                                        newBeginOT = "13:15:00";
                                                    }

                                                    if (item.BoPhan != "SP")
                                                    {
                                                        newBeginOT = "17:30:00";
                                                    }

                                                    if (string.Compare(lastTime, newBeginOT) > 0)
                                                    {
                                                        double timeOT = (DateTime.ParseExact(dateCheck + " " + lastTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) - DateTime.ParseExact(dateCheck + " " + newBeginOT, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)).TotalHours;
                                                        if (timeOT < 0.1)
                                                        {
                                                            timeOT = 0;
                                                        }

                                                        var clviec = item.lstNhanVienCaLamViec.FindAll(x =>
                                                       string.Compare(dateCheck, x.BatDau_TheoCa) >= 0 &&
                                                       string.Compare(dateCheck, x.KetThuc_TheoCa) <= 0 &&
                                                       x.MaCaLaviec == CommonConstants.CA_NGAY && x.DM_NgayLViec == "NT" && x.HeSo_OT != 100).OrderByDescending(x => x.Time_BatDau).FirstOrDefault();

                                                        if (timeOT > 0)
                                                        {
                                                            item.OvertimeValues.Add(new OvertimeValue()
                                                            {
                                                                DayCheckOT = dateCheck,
                                                                DMOvertime = clviec.HeSo_OT.NullString(),
                                                                ValueOT = timeOT,
                                                                Registered = isRegistedOT
                                                            });
                                                        }
                                                    }

                                                    item.WorkingStatuses.Add(new WorkingStatus()
                                                    {
                                                        DayCheck = dateCheck,
                                                        Value = kyhieuChamCongDB.NullString() == "" ? "DS" : kyhieuChamCongDB.NullString() // DS: Day Shift/ Ca ngày 
                                                    });
                                                }
                                                else if (CheckNgayDB(dateCheck) == 5) // Ngay ki niem cty
                                                {
                                                    double timeOT = (DateTime.ParseExact(dateCheck + " " + lastTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) - DateTime.ParseExact(dateCheck + " " + firstTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)).TotalHours;
                                                    timeOT = timeOT <= 8 ? timeOT - 0.5 : timeOT - 1.5;
                                                    if (timeOT < 0)
                                                    {
                                                        timeOT = 0;
                                                    }

                                                    item.WorkingStatuses.Add(new WorkingStatus()
                                                    {
                                                        DayCheck = dateCheck,
                                                        Value = kyhieuChamCongDB.NullString() == "" ? "MD" : kyhieuChamCongDB.NullString() // làm ca ngày chính thức ngay ki niem
                                                    });

                                                    var clviec = item.lstNhanVienCaLamViec.FirstOrDefault(x =>
                                                       string.Compare(dateCheck, x.BatDau_TheoCa) >= 0 &&
                                                       string.Compare(dateCheck, x.KetThuc_TheoCa) <= 0 &&
                                                       x.MaCaLaviec == CommonConstants.CA_NGAY && x.DM_NgayLViec == "CN" && x.HeSo_OT != 100); // Ngay ki niem coi nhu ngay chu nhat

                                                    item.OvertimeValues.Add(new OvertimeValue()
                                                    {
                                                        DayCheckOT = dateCheck,
                                                        DMOvertime = clviec.HeSo_OT.NullString(), // nhu OT chu nhat
                                                        ValueOT = timeOT,
                                                        Registered = isRegistedOT
                                                    });
                                                }
                                                else if (CheckNgayDB(dateCheck) == 3 || CheckNgayDB(dateCheck) == 4) // ngay chu nhat or nghi bu ngay le
                                                {
                                                    double timeOT = (DateTime.ParseExact(dateCheck + " " + lastTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) - DateTime.ParseExact(dateCheck + " " + firstTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)).TotalHours;
                                                    timeOT = timeOT <= 8 ? timeOT - 0.5 : timeOT - 1.5;
                                                    if (timeOT < 0)
                                                    {
                                                        timeOT = 0;
                                                    }

                                                    item.WorkingStatuses.Add(new WorkingStatus()
                                                    {
                                                        DayCheck = dateCheck,
                                                        Value = kyhieuChamCongDB.NullString() == "" ? "DS" : kyhieuChamCongDB.NullString()
                                                    });

                                                    var clviec = item.lstNhanVienCaLamViec.FirstOrDefault(x =>
                                                      string.Compare(dateCheck, x.BatDau_TheoCa) >= 0 &&
                                                      string.Compare(dateCheck, x.KetThuc_TheoCa) <= 0 &&
                                                      x.MaCaLaviec == CommonConstants.CA_NGAY && x.DM_NgayLViec == "CN" && x.HeSo_OT != 100); // Ngay ki niem coi nhu ngay chu nhat

                                                    item.OvertimeValues.Add(new OvertimeValue()
                                                    {
                                                        DayCheckOT = dateCheck,
                                                        DMOvertime = clviec.HeSo_OT.NullString(), // nhu OT chu nhat
                                                        ValueOT = timeOT,
                                                        Registered = isRegistedOT
                                                    });
                                                }
                                                else if (CheckNgayDB(dateCheck) == 1) // ngay le
                                                {
                                                    double timeOT = (DateTime.ParseExact(dateCheck + " " + lastTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) - DateTime.ParseExact(dateCheck + " " + firstTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)).TotalHours;
                                                    timeOT = timeOT <= 8 ? timeOT - 0.5 : timeOT - 1.5;
                                                    if (timeOT < 0)
                                                    {
                                                        timeOT = 0;
                                                    }

                                                    item.WorkingStatuses.Add(new WorkingStatus()
                                                    {
                                                        DayCheck = dateCheck,
                                                        Value = kyhieuChamCongDB.NullString() == "" ? "DS" : kyhieuChamCongDB.NullString()
                                                    });

                                                    var clviec = item.lstNhanVienCaLamViec.FirstOrDefault(x =>
                                                     string.Compare(dateCheck, x.BatDau_TheoCa) >= 0 &&
                                                     string.Compare(dateCheck, x.KetThuc_TheoCa) <= 0 &&
                                                     x.MaCaLaviec == CommonConstants.CA_NGAY &&
                                                     (x.DM_NgayLViec == "NL" || x.DM_NgayLViec == "NLCC") && x.HeSo_OT != 100);

                                                    item.OvertimeValues.Add(new OvertimeValue()
                                                    {
                                                        DayCheckOT = dateCheck,
                                                        DMOvertime = clviec.HeSo_OT.NullString(),
                                                        ValueOT = timeOT,
                                                        Registered = isRegistedOT
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
                                                        if (timeOT < 0.1)
                                                        {
                                                            timeOT = 0;
                                                        }

                                                        var clviec = item.lstNhanVienCaLamViec.FirstOrDefault(x =>
                                                                     string.Compare(dateCheck, x.BatDau_TheoCa) >= 0 &&
                                                                     string.Compare(dateCheck, x.KetThuc_TheoCa) <= 0 &&
                                                                     x.MaCaLaviec == CommonConstants.CA_NGAY &&
                                                                     x.DM_NgayLViec == "TNL" && x.HeSo_OT != 100);


                                                        if (timeOT > 0)
                                                        {
                                                            item.OvertimeValues.Add(new OvertimeValue()
                                                            {
                                                                DayCheckOT = dateCheck,
                                                                DMOvertime = clviec.HeSo_OT.NullString(),
                                                                ValueOT = timeOT,
                                                                Registered = isRegistedOT
                                                            });
                                                        }
                                                    }

                                                    item.WorkingStatuses.Add(new WorkingStatus()
                                                    {
                                                        DayCheck = dateCheck,
                                                        Value = kyhieuChamCongDB.NullString() == "" ? "DS" : kyhieuChamCongDB.NullString() // DS: Day Shift/ Ca ngày
                                                    });
                                                }

                                                // Di muon ve som
                                                double ELLC = 0;
                                                if (DateTime.ParseExact(firstTime, "HH:mm:ss", CultureInfo.InvariantCulture) > DateTime.ParseExact("08:05:00", "HH:mm:ss", CultureInfo.InvariantCulture))
                                                {
                                                    ELLC += (DateTime.ParseExact(firstTime, "HH:mm:ss", CultureInfo.InvariantCulture) - DateTime.ParseExact("08:05:00", "HH:mm:ss", CultureInfo.InvariantCulture)).TotalHours;
                                                }

                                                if (item.BoPhan == CommonConstants.SUPPORT_DEPT && DateTime.Parse(dateCheck).DayOfWeek == DayOfWeek.Saturday)
                                                {
                                                    if (DateTime.ParseExact(lastTime, "HH:mm:ss", CultureInfo.InvariantCulture) < DateTime.ParseExact("13:15:00", "HH:mm:ss", CultureInfo.InvariantCulture))
                                                    {
                                                        ELLC += (DateTime.ParseExact("13:15:00", "HH:mm:ss", CultureInfo.InvariantCulture) - DateTime.ParseExact(lastTime, "HH:mm:ss", CultureInfo.InvariantCulture)).TotalHours;
                                                    }
                                                }
                                                else
                                                {
                                                    if (DateTime.ParseExact(lastTime, "HH:mm:ss", CultureInfo.InvariantCulture) < DateTime.ParseExact("17:00:00", "HH:mm:ss", CultureInfo.InvariantCulture))
                                                    {
                                                        ELLC += (DateTime.ParseExact("17:00:00", "HH:mm:ss", CultureInfo.InvariantCulture) - DateTime.ParseExact(lastTime, "HH:mm:ss", CultureInfo.InvariantCulture)).TotalHours;
                                                    }
                                                }

                                                if (ELLC < 0 || !string.IsNullOrEmpty(kyhieuChamCongDB.NullString())) ELLC = 0;

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
                                                        if (timeOT < 0.1)
                                                        {
                                                            timeOT = 0;
                                                        }

                                                        var clviecs = item.lstNhanVienCaLamViec.FindAll(x =>
                                                                      string.Compare(dateCheck, x.BatDau_TheoCa) >= 0 &&
                                                                      string.Compare(dateCheck, x.KetThuc_TheoCa) <= 0 &&
                                                                      x.MaCaLaviec == CommonConstants.CA_DEM &&
                                                                      (x.DM_NgayLViec == "NT") && x.HeSo_OT != 100);

                                                        string hsOT1 = clviecs.FirstOrDefault(x => x.Time_KetThuc == "06:00:00").HeSo_OT.NullString();
                                                        string hsOT2 = clviecs.FirstOrDefault(x => x.Time_KetThuc == "08:00:00").HeSo_OT.NullString();

                                                        if (timeOT > 0 && timeOT <= 0.5)
                                                        {
                                                            item.OvertimeValues.Add(new OvertimeValue()
                                                            {
                                                                DayCheckOT = dateCheck,
                                                                DMOvertime = hsOT1, // 200 % : 5H30 -> 6H
                                                                ValueOT = timeOT,
                                                                Registered = isRegistedOT
                                                            });
                                                        }
                                                        else if (timeOT > 0.5)
                                                        {
                                                            item.OvertimeValues.Add(new OvertimeValue()
                                                            {
                                                                DayCheckOT = dateCheck,
                                                                DMOvertime = hsOT1, // 200 % : 5H30 -> 6H
                                                                ValueOT = 0.5,
                                                                Registered = isRegistedOT
                                                            });

                                                            item.OvertimeValues.Add(new OvertimeValue()
                                                            {
                                                                DayCheckOT = dateCheck,
                                                                DMOvertime = hsOT2, // 150 % :6h -> 8H
                                                                ValueOT = timeOT - 0.5, // tru di 0.5h cua 5-6h
                                                                Registered = isRegistedOT
                                                            });
                                                        }
                                                    }

                                                    item.WorkingStatuses.Add(new WorkingStatus()
                                                    {
                                                        DayCheck = dateCheck,
                                                        Value = kyhieuChamCongDB.NullString() == "" ? "NS" : kyhieuChamCongDB.NullString() // NS: Night Shift/ Ca đêm
                                                    });
                                                }
                                                else if (CheckNgayDB(dateCheck) == 5) // Ngay ki niem cty
                                                {
                                                    item.OvertimeValues.AddRange(GetOvertimeInNight(firstTime, lastTime, dateCheck, "CN", isRegistedOT));
                                                    item.WorkingStatuses.Add(new WorkingStatus()
                                                    {
                                                        DayCheck = dateCheck,
                                                        Value = kyhieuChamCongDB.NullString() == "" ? "BM" : kyhieuChamCongDB.NullString() // BM: Làm ca đêm ngày kỷ niệm chính thức
                                                    });
                                                }
                                                else if (CheckNgayDB(dateCheck) == 3 || CheckNgayDB(dateCheck) == 4) // ngay chu nhat + ngay nghi bu
                                                {
                                                    item.OvertimeValues.AddRange(GetOvertimeInNight(firstTime, lastTime, dateCheck, "CN", isRegistedOT));
                                                    item.WorkingStatuses.Add(new WorkingStatus()
                                                    {
                                                        DayCheck = dateCheck,
                                                        Value = kyhieuChamCongDB.NullString() == "" ? "D" : kyhieuChamCongDB.NullString() // TVD: Thử việc làm thêm ca đêm chủ nhật
                                                    });
                                                }
                                                else if (CheckNgayDB(dateCheck) == 1) // ngay le
                                                {
                                                    var ngayle = _ngaylenamRespository.FindById(dateCheck);
                                                    if (ngayle.IslastHoliday == CommonConstants.N)
                                                    {
                                                        item.OvertimeValues.AddRange(GetOvertimeInNight(firstTime, lastTime, dateCheck, "NL", isRegistedOT));
                                                    }
                                                    else // ngay le cuoi cung
                                                    {
                                                        item.OvertimeValues.AddRange(GetOvertimeInNight(firstTime, lastTime, dateCheck, "NLCC", isRegistedOT));
                                                    }

                                                    item.WorkingStatuses.Add(new WorkingStatus()
                                                    {
                                                        DayCheck = dateCheck,
                                                        Value = kyhieuChamCongDB.NullString() == "" ? "NHD" : kyhieuChamCongDB.NullString() // NHD: làm ca đêm ngày lễ
                                                    });
                                                }
                                                else if (CheckNgayDB(dateCheck) == 2) // ngay truoc le
                                                {
                                                    item.OvertimeValues.AddRange(GetOvertimeInNight(firstTime, lastTime, dateCheck, "NL", isRegistedOT));
                                                    item.WorkingStatuses.Add(new WorkingStatus()
                                                    {
                                                        DayCheck = dateCheck,
                                                        Value = kyhieuChamCongDB.NullString() == "" ? "BH" : kyhieuChamCongDB.NullString() // BH: làm ca đêm trước ngày lễ( chính thức)
                                                    });
                                                }

                                                // Di muon ve som
                                                double ELLC = 0;
                                                if (DateTime.ParseExact(firstTime, "HH:mm:ss", CultureInfo.InvariantCulture) > DateTime.ParseExact("20:05:00", "HH:mm:ss", CultureInfo.InvariantCulture))
                                                {
                                                    ELLC += (DateTime.ParseExact(firstTime, "HH:mm:ss", CultureInfo.InvariantCulture) - DateTime.ParseExact("20:05:00", "HH:mm:ss", CultureInfo.InvariantCulture)).TotalHours;
                                                }

                                                if (DateTime.ParseExact(lastTime, "HH:mm:ss", CultureInfo.InvariantCulture) < DateTime.ParseExact("05:00:00", "HH:mm:ss", CultureInfo.InvariantCulture))
                                                {
                                                    ELLC += (DateTime.ParseExact("05:00:00", "HH:mm:ss", CultureInfo.InvariantCulture) - DateTime.ParseExact(lastTime, "HH:mm:ss", CultureInfo.InvariantCulture)).TotalHours;
                                                }

                                                if (ELLC < 0) ELLC = 0;

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
                                            int checkDate = CheckNgayDB(dateCheck);
                                            if (checkDate == 1 || checkDate == 4 || checkDate == 5) // ngay le
                                            {
                                                item.WorkingStatuses.Add(new WorkingStatus()
                                                {
                                                    DayCheck = dateCheck,
                                                    Value = kyhieuChamCongDB.NullString() == "" ? "NH" : kyhieuChamCongDB.NullString() // NH: NationALHoliday/ Nghỉ lễ
                                                });
                                            }
                                            else
                                            {
                                                item.WorkingStatuses.Add(new WorkingStatus()
                                                {
                                                    DayCheck = dateCheck,
                                                    Value = kyhieuChamCongDB.NullString() == "" ? "-" : kyhieuChamCongDB.NullString()
                                                });
                                            }

                                            item.EL_LC_Statuses.Add(new EL_LC_Status()
                                            {
                                                DayCheck_EL = dateCheck,
                                                Value_EL = 0
                                            });
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

            // thêm các ngày trong tháng không có data
            int dayOfMonth = DateTime.DaysInMonth(DateTime.Parse(beginMonth).Year, DateTime.Parse(beginMonth).Month);
            List<string> lstDayOfMonth = new List<string>();
            string day = DateTime.Parse(beginMonth).Year + "-";
            if (DateTime.Parse(beginMonth).Month < 10)
            {
                day += "0" + DateTime.Parse(beginMonth).Month;
            }
            else
            {
                day += DateTime.Parse(beginMonth).Month;
            }

            for (int i = 1; i <= dayOfMonth; i++)
            {
                lstDayOfMonth.Add(day + "-" + i);
            }

            string kyHchamcong = "";
            foreach (var item in lstResult)
            {
               
                foreach (var dayOfM in lstDayOfMonth)
                {
                    if (!item.WorkingStatuses.Any(x => x.DayCheck == dayOfM))
                    {
                        kyHchamcong = item.lstChamCongDB.FirstOrDefault(x => string.Compare(dayOfM, x.NgayBatDau) >= 0 && string.Compare(dayOfM, x.NgayKetThuc) <= 0)?.KyHieuChamCong;
                        item.WorkingStatuses.Add(new WorkingStatus()
                        {
                            DayCheck = dayOfM,
                            Value = kyHchamcong.NullString() == "" ? "-" : kyHchamcong.NullString()
                        });

                        item.EL_LC_Statuses.Add(new EL_LC_Status()
                        {
                            DayCheck_EL = dayOfM,
                            Value_EL = 0
                        });
                    }
                }
            }

            return lstResult;
        }

        private List<OvertimeValue> GetOvertimeInNight(string firstTime, string lastTime, string dateCheck, string ngayLviec, bool isRegistedOT)
        {
            // 20:00 -> 22:00
            // 22:00 -> 23:59
            // 00:00 -> 06:00
            // 06:00 -> 08:00

            var clviecs = _calaviecRespository.FindAll(x => x.DM_NgayLViec == ngayLviec && x.Danhmuc_CaLviec == CommonConstants.CA_DEM && x.HeSo_OT != 100);

            List<OvertimeValue> lstResult = new List<OvertimeValue>();

            if (DateTime.ParseExact(firstTime, "HH:mm:ss", CultureInfo.InvariantCulture) >= DateTime.ParseExact("20:00:00", "HH:mm:ss", CultureInfo.InvariantCulture) &&
            DateTime.ParseExact(lastTime, "HH:mm:ss", CultureInfo.InvariantCulture) <= DateTime.ParseExact("22:00:00", "HH:mm:ss", CultureInfo.InvariantCulture))
            {
                string hsOT = clviecs.FirstOrDefault(x => x.Time_KetThuc == "22:00:00").HeSo_OT.NullString();

                double timeOT = (DateTime.ParseExact(dateCheck + " " + lastTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) - DateTime.ParseExact(dateCheck + " " + firstTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)).TotalHours;
                if (timeOT < 0)
                {
                    timeOT = 0;
                }

                if (ngayLviec == "CN") // ngay chu nhat
                {
                    if (timeOT > 0)
                    {
                        lstResult.Add(new OvertimeValue()
                        {
                            DayCheckOT = dateCheck,
                            DMOvertime = hsOT,
                            ValueOT = timeOT,
                            Registered = isRegistedOT
                        });
                    }
                }
                else if (ngayLviec == "NLCC" || ngayLviec == "NL") // Ngay le, ngay le cuoi cung
                {
                    if (timeOT > 0)
                    {
                        lstResult.Add(new OvertimeValue()
                        {
                            DayCheckOT = dateCheck,
                            DMOvertime = hsOT,
                            ValueOT = timeOT,
                            Registered = isRegistedOT
                        });
                    }
                }
            }
            else if (DateTime.ParseExact(firstTime, "HH:mm:ss", CultureInfo.InvariantCulture) >= DateTime.ParseExact("22:00:00", "HH:mm:ss", CultureInfo.InvariantCulture) &&
               DateTime.ParseExact(lastTime, "HH:mm:ss", CultureInfo.InvariantCulture) <= DateTime.ParseExact("23:59:59", "HH:mm:ss", CultureInfo.InvariantCulture))
            {
                string hsOT = clviecs.FirstOrDefault(x => x.Time_BatDau == "22:00:00").HeSo_OT.NullString();

                double timeOT = (DateTime.ParseExact(dateCheck + " " + lastTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) - DateTime.ParseExact(dateCheck + " " + firstTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)).TotalHours;
                if (timeOT < 0)
                {
                    timeOT = 0;
                }

                if (timeOT > 0)
                {
                    if (ngayLviec == "NLCC" || ngayLviec == "NL") // ngay chu nhat
                    {
                        lstResult.Add(new OvertimeValue()
                        {
                            DayCheckOT = dateCheck,
                            DMOvertime = hsOT,
                            ValueOT = timeOT,
                            Registered = isRegistedOT
                        });
                    }
                    else if (ngayLviec == "CN")
                    {
                        lstResult.Add(new OvertimeValue()
                        {
                            DayCheckOT = dateCheck,
                            DMOvertime = hsOT,
                            ValueOT = timeOT,
                            Registered = isRegistedOT
                        });
                    }
                }
            }
            else if (DateTime.ParseExact(firstTime, "HH:mm:ss", CultureInfo.InvariantCulture) >= DateTime.ParseExact("00:00:00", "HH:mm:ss", CultureInfo.InvariantCulture) &&
                     DateTime.ParseExact(lastTime, "HH:mm:ss", CultureInfo.InvariantCulture) <= DateTime.ParseExact("06:00:00", "HH:mm:ss", CultureInfo.InvariantCulture))
            {
                string hsOT = clviecs.FirstOrDefault(x => x.Time_KetThuc.CompareTo("06:00:00") <= 0).HeSo_OT.NullString();

                double timeOT = (DateTime.ParseExact(dateCheck + " " + lastTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) - DateTime.ParseExact(dateCheck + " " + firstTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)).TotalHours;
                if (timeOT < 0)
                {
                    timeOT = 0;
                }

                if (timeOT > 0)
                {
                    if (ngayLviec == "NLCC") // ngay le cuoi cung
                    {
                        lstResult.Add(new OvertimeValue()
                        {
                            DayCheckOT = dateCheck,
                            DMOvertime = hsOT,
                            ValueOT = timeOT - 0.5,
                            Registered = isRegistedOT
                        });
                    }
                    else if (ngayLviec == "NL") // NGAY LE
                    {
                        lstResult.Add(new OvertimeValue()
                        {
                            DayCheckOT = dateCheck,
                            DMOvertime = hsOT,
                            ValueOT = timeOT - 0.5,
                            Registered = isRegistedOT
                        });
                    }
                    else if (ngayLviec == "CN")
                    {
                        lstResult.Add(new OvertimeValue()
                        {
                            DayCheckOT = dateCheck,
                            DMOvertime = hsOT,
                            ValueOT = timeOT - 0.5, // O.5H di an
                            Registered = isRegistedOT
                        });
                    }
                }
            }
            else if (DateTime.ParseExact(firstTime, "HH:mm:ss", CultureInfo.InvariantCulture) >= DateTime.ParseExact("06:00:00", "HH:mm:ss", CultureInfo.InvariantCulture) &&
                     DateTime.ParseExact(lastTime, "HH:mm:ss", CultureInfo.InvariantCulture) <= DateTime.ParseExact("08:00:00", "HH:mm:ss", CultureInfo.InvariantCulture))
            {
                double timeOT = (DateTime.ParseExact(dateCheck + " " + lastTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) - DateTime.ParseExact(dateCheck + " " + firstTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)).TotalHours;
                string hsOT = clviecs.FirstOrDefault(x => x.Time_KetThuc == "08:00:00").HeSo_OT.NullString();

                if (timeOT < 0)
                {
                    timeOT = 0;
                }

                if (timeOT > 0)
                {
                    if (ngayLviec == "NLCC") // ngay le cuoi cung
                    {
                        lstResult.Add(new OvertimeValue()
                        {
                            DayCheckOT = dateCheck,
                            DMOvertime = hsOT,
                            ValueOT = timeOT,
                            Registered = isRegistedOT
                        });
                    }
                    else if (ngayLviec == "NL") // NGAY LE
                    {
                        lstResult.Add(new OvertimeValue()
                        {
                            DayCheckOT = dateCheck,
                            DMOvertime = hsOT,
                            ValueOT = timeOT,
                            Registered = isRegistedOT
                        });
                    }
                    else if (ngayLviec == "CN")
                    {
                        lstResult.Add(new OvertimeValue()
                        {
                            DayCheckOT = dateCheck,
                            DMOvertime = hsOT,
                            ValueOT = timeOT,
                            Registered = isRegistedOT
                        });
                    }
                }
            }
            else if (DateTime.ParseExact(firstTime, "HH:mm:ss", CultureInfo.InvariantCulture) >= DateTime.ParseExact("20:00:00", "HH:mm:ss", CultureInfo.InvariantCulture) &&
                    DateTime.ParseExact(lastTime, "HH:mm:ss", CultureInfo.InvariantCulture) <= DateTime.ParseExact("23:59:59", "HH:mm:ss", CultureInfo.InvariantCulture))
            {
                double timeOT1 = (DateTime.ParseExact(dateCheck + " " + "22:00:00", "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) - DateTime.ParseExact(dateCheck + " " + firstTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)).TotalHours; // 20 ->22
                double timeOT2 = (DateTime.ParseExact(dateCheck + " " + lastTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) - DateTime.ParseExact(dateCheck + " " + "22:00:00", "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)).TotalHours; // 22 -> 00

                string hsOT1 = clviecs.FirstOrDefault(x => x.Time_KetThuc == "22:00:00").HeSo_OT.NullString();
                string hsOT2 = clviecs.FirstOrDefault(x => x.Time_BatDau == "22:00:00").HeSo_OT.NullString();

                if (timeOT1 < 0)
                {
                    timeOT1 = 0;
                }
                if (timeOT2 < 0)
                {
                    timeOT2 = 0;
                }

                if (ngayLviec == "CN") // ngay chu nhat
                {
                    lstResult.Add(new OvertimeValue()
                    {
                        DayCheckOT = dateCheck,
                        DMOvertime = hsOT1,
                        ValueOT = timeOT1,
                        Registered = isRegistedOT
                    });
                }
                else if (ngayLviec == "NLCC" || ngayLviec == "NL") // Ngay le, ngay le cuoi cung
                {
                    lstResult.Add(new OvertimeValue()
                    {
                        DayCheckOT = dateCheck,
                        DMOvertime = hsOT1,
                        ValueOT = timeOT1,
                        Registered = isRegistedOT
                    });
                }

                if (ngayLviec == "NLCC" || ngayLviec == "NL")
                {
                    lstResult.Add(new OvertimeValue()
                    {
                        DayCheckOT = dateCheck,
                        DMOvertime = hsOT2,
                        ValueOT = timeOT2,
                        Registered = isRegistedOT
                    });
                }
                else if (ngayLviec == "CN")
                {
                    lstResult.Add(new OvertimeValue()
                    {
                        DayCheckOT = dateCheck,
                        DMOvertime = hsOT2,
                        ValueOT = timeOT2,
                        Registered = isRegistedOT
                    });
                }
            }
            else if (DateTime.ParseExact(firstTime, "HH:mm:ss", CultureInfo.InvariantCulture) >= DateTime.ParseExact("20:00:00", "HH:mm:ss", CultureInfo.InvariantCulture) && // 20 -> 06
                   DateTime.ParseExact(lastTime, "HH:mm:ss", CultureInfo.InvariantCulture) <= DateTime.ParseExact("06:00:00", "HH:mm:ss", CultureInfo.InvariantCulture))
            {
                double timeOT1 = (DateTime.ParseExact(dateCheck + " " + "22:00:00", "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) - DateTime.ParseExact(dateCheck + " " + firstTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)).TotalHours; // 20 ->22
                double timeOT2 = 2; // 22 -> 00
                double timeOT3 = (DateTime.ParseExact(dateCheck + " " + lastTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) - DateTime.ParseExact(dateCheck + " " + "00:00:00", "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)).TotalHours; // 00 -> 06

                string hsOT1 = clviecs.FirstOrDefault(x => x.Time_KetThuc == "22:00:00").HeSo_OT.NullString(); // 20->22
                string hsOT2 = clviecs.FirstOrDefault(x => x.Time_BatDau == "22:00:00").HeSo_OT.NullString(); // 22->00
                string hsOT3 = clviecs.FirstOrDefault(x => x.Time_KetThuc.CompareTo("06:00:00") <= 0).HeSo_OT.NullString(); // 00 -> 6

                if (timeOT1 < 0)
                {
                    timeOT1 = 0;
                }
                if (timeOT3 < 0)
                {
                    timeOT3 = 0;
                }

                if (ngayLviec == "CN") // ngay chu nhat
                {
                    lstResult.Add(new OvertimeValue()
                    {
                        DayCheckOT = dateCheck,
                        DMOvertime = hsOT1,
                        ValueOT = timeOT1,
                        Registered = isRegistedOT
                    });
                }
                else if (ngayLviec == "NLCC" || ngayLviec == "NL") // Ngay le, ngay le cuoi cung
                {
                    lstResult.Add(new OvertimeValue()
                    {
                        DayCheckOT = dateCheck,
                        DMOvertime = hsOT1,
                        ValueOT = timeOT1,
                        Registered = isRegistedOT
                    });
                }

                if (ngayLviec == "NLCC" || ngayLviec == "NL")
                {
                    lstResult.Add(new OvertimeValue()
                    {
                        DayCheckOT = dateCheck,
                        DMOvertime = hsOT2,
                        ValueOT = timeOT2,
                        Registered = isRegistedOT
                    });
                }
                else if (ngayLviec == "CN")
                {
                    lstResult.Add(new OvertimeValue()
                    {
                        DayCheckOT = dateCheck,
                        DMOvertime = hsOT2,
                        ValueOT = timeOT2,
                        Registered = isRegistedOT
                    });
                }

                if (ngayLviec == "NLCC") // ngay le cuoi cung
                {
                    lstResult.Add(new OvertimeValue()
                    {
                        DayCheckOT = dateCheck,
                        DMOvertime = hsOT3,
                        ValueOT = timeOT3 - 0.5,
                        Registered = isRegistedOT
                    });
                }
                else if (ngayLviec == "NL") // NGAY LE
                {
                    lstResult.Add(new OvertimeValue()
                    {
                        DayCheckOT = dateCheck,
                        DMOvertime = hsOT3,
                        ValueOT = timeOT3 - 0.5,
                        Registered = isRegistedOT
                    });
                }
                else if (ngayLviec == "CN")
                {
                    lstResult.Add(new OvertimeValue()
                    {
                        DayCheckOT = dateCheck,
                        DMOvertime = hsOT3,
                        ValueOT = timeOT3 - 0.5, // O.5H di an
                        Registered = isRegistedOT
                    });
                }
            }
            else if (DateTime.ParseExact(firstTime, "HH:mm:ss", CultureInfo.InvariantCulture) >= DateTime.ParseExact("20:00:00", "HH:mm:ss", CultureInfo.InvariantCulture) && // 20 -> 08
                    DateTime.ParseExact(lastTime, "HH:mm:ss", CultureInfo.InvariantCulture) > DateTime.ParseExact("06:00:00", "HH:mm:ss", CultureInfo.InvariantCulture) &&
                    DateTime.ParseExact(lastTime, "HH:mm:ss", CultureInfo.InvariantCulture) <= DateTime.ParseExact("08:00:00", "HH:mm:ss", CultureInfo.InvariantCulture))
            {
                double timeOT1 = (DateTime.ParseExact(dateCheck + " " + "22:00:00", "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) - DateTime.ParseExact(dateCheck + " " + firstTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)).TotalHours; // 20 ->22
                double timeOT2 = 2; // 22 -> 00
                double timeOT3 = 4.5; // 00 -> 06 , 5h - tru 0.5 di an
                double timeOT4 = (DateTime.ParseExact(dateCheck + " " + lastTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) - DateTime.ParseExact(dateCheck + " " + "06:00:00", "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)).TotalHours; // 06 -> 08

                string hsOT1 = clviecs.FirstOrDefault(x => x.Time_KetThuc == "22:00:00").HeSo_OT.NullString(); // 20->22
                string hsOT2 = clviecs.FirstOrDefault(x => x.Time_BatDau == "22:00:00").HeSo_OT.NullString(); // 22->00
                string hsOT3 = clviecs.FirstOrDefault(x => x.Time_KetThuc.CompareTo("06:00:00") <= 0).HeSo_OT.NullString(); // 00 -> 6
                string hsOT4 = clviecs.FirstOrDefault(x => x.Time_KetThuc == "08:00:00").HeSo_OT.NullString(); // 6->8

                if (timeOT1 < 0)
                {
                    timeOT1 = 0;
                }
                if (timeOT4 < 0)
                {
                    timeOT4 = 0;
                }

                // 20 -> 22
                if (ngayLviec == "CN") // ngay chu nhat
                {
                    lstResult.Add(new OvertimeValue()
                    {
                        DayCheckOT = dateCheck,
                        DMOvertime = hsOT1,
                        ValueOT = timeOT1,
                        Registered = isRegistedOT
                    });
                }
                else if (ngayLviec == "NLCC" || ngayLviec == "NL") // Ngay le, ngay le cuoi cung
                {
                    lstResult.Add(new OvertimeValue()
                    {
                        DayCheckOT = dateCheck,
                        DMOvertime = hsOT1,
                        ValueOT = timeOT1,
                        Registered = isRegistedOT
                    });
                }

                // 22-> 0
                if (ngayLviec == "NLCC" || ngayLviec == "NL")
                {
                    lstResult.Add(new OvertimeValue()
                    {
                        DayCheckOT = dateCheck,
                        DMOvertime = hsOT2,
                        ValueOT = timeOT2,
                        Registered = isRegistedOT
                    });
                }
                else if (ngayLviec == "CN")
                {
                    lstResult.Add(new OvertimeValue()
                    {
                        DayCheckOT = dateCheck,
                        DMOvertime = hsOT2,
                        ValueOT = timeOT2,
                        Registered = isRegistedOT
                    });
                }

                // 0 -> 6
                if (ngayLviec == "NLCC") // ngay le cuoi cung
                {
                    lstResult.Add(new OvertimeValue()
                    {
                        DayCheckOT = dateCheck,
                        DMOvertime = hsOT3,
                        ValueOT = timeOT3 - 0.5,
                        Registered = isRegistedOT
                    });
                }
                else if (ngayLviec == "NL") // NGAY LE
                {
                    lstResult.Add(new OvertimeValue()
                    {
                        DayCheckOT = dateCheck,
                        DMOvertime = hsOT3,
                        ValueOT = timeOT3 - 0.5,
                        Registered = isRegistedOT
                    });
                }
                else if (ngayLviec == "CN")
                {
                    lstResult.Add(new OvertimeValue()
                    {
                        DayCheckOT = dateCheck,
                        DMOvertime = hsOT3,
                        ValueOT = timeOT3 - 0.5, // O.5H di an
                        Registered = isRegistedOT
                    });
                }

                // 6-8h
                if (ngayLviec == "NLCC") // ngay le cuoi cung
                {
                    lstResult.Add(new OvertimeValue()
                    {
                        DayCheckOT = dateCheck,
                        DMOvertime = hsOT4,
                        ValueOT = timeOT4,
                        Registered = isRegistedOT
                    });
                }
                else if (ngayLviec == "NL") // NGAY LE
                {
                    lstResult.Add(new OvertimeValue()
                    {
                        DayCheckOT = dateCheck,
                        DMOvertime = hsOT4,
                        ValueOT = timeOT4,
                        Registered = isRegistedOT
                    });
                }
                else if (ngayLviec == "CN")
                {
                    lstResult.Add(new OvertimeValue()
                    {
                        DayCheckOT = dateCheck,
                        DMOvertime = hsOT4,
                        ValueOT = timeOT4,
                        Registered = isRegistedOT
                    });
                }
            }
            else if (DateTime.ParseExact(firstTime, "HH:mm:ss", CultureInfo.InvariantCulture) >= DateTime.ParseExact("22:00:00", "HH:mm:ss", CultureInfo.InvariantCulture) && // 22 -> 06
                 DateTime.ParseExact(lastTime, "HH:mm:ss", CultureInfo.InvariantCulture) <= DateTime.ParseExact("06:00:00", "HH:mm:ss", CultureInfo.InvariantCulture))
            {
                double timeOT1 = (DateTime.ParseExact(dateCheck + " " + "23:59:59", "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) - DateTime.ParseExact(dateCheck + " " + firstTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)).TotalHours; // 22-> 00
                double timeOT2 = (DateTime.ParseExact(dateCheck + " " + lastTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) - DateTime.ParseExact(dateCheck + " " + "00:00:00", "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)).TotalHours; // 00 -> 06

                string hsOT1 = clviecs.FirstOrDefault(x => x.Time_BatDau == "22:00:00").HeSo_OT.NullString(); // 22->00
                string hsOT2 = clviecs.FirstOrDefault(x => x.Time_KetThuc.CompareTo("06:00:00") <= 0).HeSo_OT.NullString(); // 00 -> 6

                if (timeOT1 < 0)
                {
                    timeOT1 = 0;
                }
                if (timeOT2 < 0)
                {
                    timeOT2 = 0;
                }

                // 22 -> 0
                if (ngayLviec == "NLCC" || ngayLviec == "NL") // ngay chu nhat
                {
                    lstResult.Add(new OvertimeValue()
                    {
                        DayCheckOT = dateCheck,
                        DMOvertime = hsOT1,
                        ValueOT = timeOT1,
                        Registered = isRegistedOT
                    });
                }
                else if (ngayLviec == "CN")
                {
                    lstResult.Add(new OvertimeValue()
                    {
                        DayCheckOT = dateCheck,
                        DMOvertime = hsOT1,
                        ValueOT = timeOT1,
                        Registered = isRegistedOT
                    });
                }

                // 00 -> 06
                if (ngayLviec == "NLCC") // ngay le cuoi cung
                {
                    lstResult.Add(new OvertimeValue()
                    {
                        DayCheckOT = dateCheck,
                        DMOvertime = hsOT2,
                        ValueOT = timeOT2 - 0.5,
                        Registered = isRegistedOT
                    });
                }
                else if (ngayLviec == "NL") // NGAY LE
                {
                    lstResult.Add(new OvertimeValue()
                    {
                        DayCheckOT = dateCheck,
                        DMOvertime = hsOT2,
                        ValueOT = timeOT2 - 0.5,
                        Registered = isRegistedOT
                    });
                }
                else if (ngayLviec == "CN")
                {
                    lstResult.Add(new OvertimeValue()
                    {
                        DayCheckOT = dateCheck,
                        DMOvertime = hsOT2,
                        ValueOT = timeOT2 - 0.5, // O.5H di an
                        Registered = isRegistedOT
                    });
                }
            }
            else if (DateTime.ParseExact(firstTime, "HH:mm:ss", CultureInfo.InvariantCulture) >= DateTime.ParseExact("22:00:00", "HH:mm:ss", CultureInfo.InvariantCulture) && // 22 -> 08
                 DateTime.ParseExact(lastTime, "HH:mm:ss", CultureInfo.InvariantCulture) > DateTime.ParseExact("06:00:00", "HH:mm:ss", CultureInfo.InvariantCulture))
            {
                double timeOT2 = (DateTime.ParseExact(dateCheck + " " + "23:59:59", "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) - DateTime.ParseExact(dateCheck + " " + firstTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)).TotalHours; // 22 -> 00
                double timeOT3 = 4.5; // 00 -> 06 , 5h - tru 0.5 di an
                double timeOT4 = (DateTime.ParseExact(dateCheck + " " + lastTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) - DateTime.ParseExact(dateCheck + " " + "06:00:00", "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)).TotalHours; // 06 -> 08

                string hsOT2 = clviecs.FirstOrDefault(x => x.Time_BatDau == "22:00:00").HeSo_OT.NullString(); // 22->00
                string hsOT3 = clviecs.FirstOrDefault(x => x.Time_KetThuc.CompareTo("06:00:00") <= 0).HeSo_OT.NullString(); // 00 > 6
                string hsOT4 = clviecs.FirstOrDefault(x => x.Time_KetThuc == "08:00:00").HeSo_OT.NullString(); // 6->8

                if (timeOT4 < 0)
                {
                    timeOT4 = 0;
                }
                if (timeOT2 < 0)
                {
                    timeOT2 = 0;
                }

                // 22-> 0
                if (ngayLviec == "NLCC" || ngayLviec == "NL")
                {
                    lstResult.Add(new OvertimeValue()
                    {
                        DayCheckOT = dateCheck,
                        DMOvertime = hsOT2,
                        ValueOT = timeOT2,
                        Registered = isRegistedOT
                    });
                }
                else if (ngayLviec == "CN")
                {
                    lstResult.Add(new OvertimeValue()
                    {
                        DayCheckOT = dateCheck,
                        DMOvertime = hsOT2,
                        ValueOT = timeOT2,
                        Registered = isRegistedOT
                    });
                }

                // 0 -> 6
                if (ngayLviec == "NLCC") // ngay le cuoi cung
                {
                    lstResult.Add(new OvertimeValue()
                    {
                        DayCheckOT = dateCheck,
                        DMOvertime = hsOT3,
                        ValueOT = timeOT3 - 0.5,
                        Registered = isRegistedOT
                    });
                }
                else if (ngayLviec == "NL") // NGAY LE
                {
                    lstResult.Add(new OvertimeValue()
                    {
                        DayCheckOT = dateCheck,
                        DMOvertime = hsOT3,
                        ValueOT = timeOT3 - 0.5,
                        Registered = isRegistedOT
                    });
                }
                else if (ngayLviec == "CN")
                {
                    lstResult.Add(new OvertimeValue()
                    {
                        DayCheckOT = dateCheck,
                        DMOvertime = hsOT3,
                        ValueOT = timeOT3 - 0.5, // O.5H di an
                        Registered = isRegistedOT
                    });
                }

                // 6-8h
                if (ngayLviec == "NLCC") // ngay le cuoi cung
                {
                    lstResult.Add(new OvertimeValue()
                    {
                        DayCheckOT = dateCheck,
                        DMOvertime = hsOT4,
                        ValueOT = timeOT4,
                        Registered = isRegistedOT
                    });
                }
                else if (ngayLviec == "NL") // NGAY LE
                {
                    lstResult.Add(new OvertimeValue()
                    {
                        DayCheckOT = dateCheck,
                        DMOvertime = hsOT4,
                        ValueOT = timeOT4,
                        Registered = isRegistedOT
                    });
                }
                else if (ngayLviec == "CN")
                {
                    lstResult.Add(new OvertimeValue()
                    {
                        DayCheckOT = dateCheck,
                        DMOvertime = hsOT4,
                        ValueOT = timeOT4,
                        Registered = isRegistedOT
                    });
                }
            }
            else if (DateTime.ParseExact(firstTime, "HH:mm:ss", CultureInfo.InvariantCulture) >= DateTime.ParseExact("00:00:00", "HH:mm:ss", CultureInfo.InvariantCulture) && // 00 -> 08
               DateTime.ParseExact(lastTime, "HH:mm:ss", CultureInfo.InvariantCulture) > DateTime.ParseExact("06:00:00", "HH:mm:ss", CultureInfo.InvariantCulture))
            {
                double timeOT2 = (DateTime.ParseExact(dateCheck + " " + "06:00:00", "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) - DateTime.ParseExact(dateCheck + " " + firstTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)).TotalHours; // 00 -> 06
                double timeOT4 = (DateTime.ParseExact(dateCheck + " " + lastTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) - DateTime.ParseExact(dateCheck + " " + "06:00:00", "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)).TotalHours; // 06 -> 08

                string hsOT2 = clviecs.FirstOrDefault(x => x.Time_KetThuc.CompareTo("06:00:00") <= 0).HeSo_OT.NullString(); // 00 -> 6
                string hsOT4 = clviecs.FirstOrDefault(x => x.Time_KetThuc == "08:00:00").HeSo_OT.NullString(); //6->8

                if (timeOT4 < 0)
                {
                    timeOT4 = 0;
                }
                if (timeOT2 < 0)
                {
                    timeOT2 = 0;
                }

                // 0 -> 6
                if (ngayLviec == "NLCC") // ngay le cuoi cung
                {
                    lstResult.Add(new OvertimeValue()
                    {
                        DayCheckOT = dateCheck,
                        DMOvertime = hsOT2,
                        ValueOT = timeOT2 - 0.5,
                        Registered = isRegistedOT
                    });
                }
                else if (ngayLviec == "NL") // NGAY LE
                {
                    lstResult.Add(new OvertimeValue()
                    {
                        DayCheckOT = dateCheck,
                        DMOvertime = hsOT2,
                        ValueOT = timeOT2 - 0.5,
                        Registered = isRegistedOT
                    });
                }
                else if (ngayLviec == "CN")
                {
                    lstResult.Add(new OvertimeValue()
                    {
                        DayCheckOT = dateCheck,
                        DMOvertime = hsOT2,
                        ValueOT = timeOT2 - 0.5, // O.5H di an
                        Registered = isRegistedOT
                    });
                }

                // 6-8h
                if (ngayLviec == "NLCC") // ngay le cuoi cung
                {
                    lstResult.Add(new OvertimeValue()
                    {
                        DayCheckOT = dateCheck,
                        DMOvertime = "150",
                        ValueOT = timeOT4,
                        Registered = isRegistedOT
                    });
                }
                else if (ngayLviec == "NL") // NGAY LE
                {
                    lstResult.Add(new OvertimeValue()
                    {
                        DayCheckOT = dateCheck,
                        DMOvertime = hsOT4,
                        ValueOT = timeOT4,
                        Registered = isRegistedOT
                    });
                }
                else if (ngayLviec == "CN")
                {
                    lstResult.Add(new OvertimeValue()
                    {
                        DayCheckOT = dateCheck,
                        DMOvertime = hsOT4,
                        ValueOT = timeOT4,
                        Registered = isRegistedOT
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
