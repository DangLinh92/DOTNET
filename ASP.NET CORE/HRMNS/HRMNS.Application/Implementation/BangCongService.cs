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
using HRMNS.Application.ViewModels.HR;
using HRMNS.Data.Enums;
using System.Threading.Tasks;

namespace HRMNS.Application.Implementation
{
    public class BangCongService : BaseService, IBangCongService
    {
        private IRespository<ATTENDANCE_RECORD, long> _attendanceRecordRespository;
        private IRespository<CA_LVIEC, int> _calaviecRespository;
        private IRespository<CHAM_CONG_LOG, long> _chamCongRespository;
        private IRespository<NGAY_LE_NAM, string> _ngaylenamRespository;
        private IRespository<NGAY_DAC_BIET, string> _ngaydacbietRespository;
        private IRespository<NGAY_NGHI_BU_LE_NAM, int> _ngaynghibuRespository;
        private IRespository<HR_HOPDONG, int> _hopDongResponsitory;
        private IRespository<HR_NHANVIEN, string> _nhanvienResponsitory;
        private IRespository<DANGKY_CHAMCONG_DACBIET, int> _chamCongDBResponsitory;
        private IRespository<NHANVIEN_CALAMVIEC, int> _nhanvienCLviecResponsitory;
        private IRespository<HR_THAISAN_CONNHO, int> _thaisanResponsitory;
        private IRespository<DANGKY_OT_NHANVIEN, int> _overtimeResponsitory;
        private IRespository<HR_NHANVIEN_CHEDO_DB, int> _nhanvienCheDoDBResponsitory;
        private IRespository<DANGKY_DIMUON_VSOM_NHANVIEN, int> _dimuonVeSomResponsitory;

        private EFUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BangCongService(
            IRespository<ATTENDANCE_RECORD, long> attendance_record, IRespository<NGAY_LE_NAM, string> ngayleRespository,
            IRespository<NGAY_NGHI_BU_LE_NAM, int> ngaynghibuRespository, IRespository<NGAY_DAC_BIET, string> ngayDacBietRespository,
            IRespository<CHAM_CONG_LOG, long> chamCongRespository,
            IRespository<HR_HOPDONG, int> hopDongResponsitory,
            IRespository<CA_LVIEC, int> calaviecRespository,
            IRespository<HR_NHANVIEN, string> nhanvienResponsitory,
            IRespository<DANGKY_CHAMCONG_DACBIET, int> chamCongDBResponsitory,
            IRespository<NHANVIEN_CALAMVIEC, int> nhanvienCLviecResponsitory,
            IRespository<HR_THAISAN_CONNHO, int> thaisanResponsitory,
            IRespository<DANGKY_OT_NHANVIEN, int> overtimeResponsitory,
            IRespository<HR_NHANVIEN_CHEDO_DB, int> nhanvienCheDoDBResponsitory,
            IRespository<DANGKY_DIMUON_VSOM_NHANVIEN, int> dimuonVeSomResponsitory,
            IUnitOfWork unitOfWork, IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
        {
            _attendanceRecordRespository = attendance_record;
            _chamCongRespository = chamCongRespository;
            _ngaylenamRespository = ngayleRespository;
            _ngaydacbietRespository = ngayDacBietRespository;
            _ngaynghibuRespository = ngaynghibuRespository;
            _hopDongResponsitory = hopDongResponsitory;
            _calaviecRespository = calaviecRespository;
            _chamCongDBResponsitory = chamCongDBResponsitory;
            _nhanvienCLviecResponsitory = nhanvienCLviecResponsitory;
            _overtimeResponsitory = overtimeResponsitory;
            _unitOfWork = (EFUnitOfWork)unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _nhanvienResponsitory = nhanvienResponsitory;
            _thaisanResponsitory = thaisanResponsitory;
            _dimuonVeSomResponsitory = dimuonVeSomResponsitory;
            _nhanvienCheDoDBResponsitory = nhanvienCheDoDBResponsitory;
        }

        List<CHAM_CONG_LOG> CHAM_CONG_LOGs;
        List<HR_THAISAN_CONNHO> HR_THAISAN_CONNHOs;
        List<NGAY_LE_NAM> NGAY_LE_NAMs;
        List<NGAY_NGHI_BU_LE_NAM> NGAY_NGHI_BU_LE_NAMs;
        List<NGAY_DAC_BIET> NGAY_DAC_BIETs;
        List<CA_LVIEC> CA_LVIECs;
        List<HR_HOPDONG> HOPDONGs;
        List<HR_NHANVIEN_CHEDO_DB> HR_NHANVIEN_CHEDO_DBs;
        List<DANGKY_DIMUON_VSOM_NHANVIEN> DANGKY_DIMUON_VSOM_NHANVIENs;

        private void DataInit(string startTime, string endTime)
        {
            CHAM_CONG_LOGs = _chamCongRespository.FindAll(x => string.Compare(x.Ngay_ChamCong, startTime) >= 0 && string.Compare(x.Ngay_ChamCong, endTime) <= 0).ToList();
            HR_THAISAN_CONNHOs = _thaisanResponsitory.FindAll().ToList();
            NGAY_LE_NAMs = _ngaylenamRespository.FindAll().ToList();
            NGAY_NGHI_BU_LE_NAMs = _ngaynghibuRespository.FindAll().ToList();
            NGAY_DAC_BIETs = _ngaydacbietRespository.FindAll().ToList();
            CA_LVIECs = _calaviecRespository.FindAll().ToList();
            HOPDONGs = _hopDongResponsitory.FindAll(x => x.HR_LOAIHOPDONG).ToList();
            HR_NHANVIEN_CHEDO_DBs = _nhanvienCheDoDBResponsitory.FindAll().ToList();
            DANGKY_DIMUON_VSOM_NHANVIENs = _dimuonVeSomResponsitory.FindAll().ToList();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// ThaiSan : thai sản
        /// MangBau: Mang bầu
        /// ConNho1H: con nhỏ về sớm 1h
        /// ConNho : con nhỏ về đúng giờ
        /// </summary>
        /// <param name="maChedo"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        private bool CheckCheDoThaiSan(string maChedo, string date, string manv)
        {
            return HR_THAISAN_CONNHOs == null ? _thaisanResponsitory.FindAll(x => x.CheDoThaiSan == maChedo && x.MaNV == manv && date.CompareTo(x.FromDate) >= 0 && date.CompareTo(x.ToDate) <= 0).FirstOrDefault() != null : HR_THAISAN_CONNHOs.FindAll(x => x.CheDoThaiSan == maChedo && x.MaNV == manv && date.CompareTo(x.FromDate) >= 0 && date.CompareTo(x.ToDate) <= 0).FirstOrDefault() != null;
        }

        public List<ChamCongDataViewModel> GetDataReport(string timeEndUser, string status, string time, string dept, ref List<DeNghiLamThemGioModel> deNghiLamThemGios)
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
                    if (item.HeSo_OT > 200 && !lstDanhMucOT.Contains("210%"))
                    {
                        lstDanhMucOT.Add("210%");
                    }

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
                return lstResult;
            }

            DataInit(beginMonth, endMonth);

            List<string> lstBoPhan = new List<string>();

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
                        if (!lstBoPhan.Contains(row["MaBoPhan"].NullString()))
                        {
                            lstBoPhan.Add(row["MaBoPhan"].NullString());
                        }

                        if (status == Status.InActive.NullString())
                        {
                            if (row["Status"].NullString() == status && string.Compare(row["NgayNghiViec"].NullString(), timeEndUser) <= 0)
                            {
                                chamCongVM = new ChamCongDataViewModel();
                                chamCongVM.MaNV = row["MaNV"].NullString();
                                chamCongVM.TenNV = row["TenNV"].NullString();
                                chamCongVM.NgayVao = row["NgayVao"].NullString();
                                chamCongVM.BoPhanDetail = row["TenBoPhanChiTiet"].NullString();
                                chamCongVM.BoPhan = row["MaBoPhan"].NullString();
                                chamCongVM.OrderBy = row["NoiTuyenDung"].NullString();
                                chamCongVM.month_Check = time;
                                chamCongVM.lstDanhMucOT = lstDanhMucOT;
                                lstResult.Add(chamCongVM);
                            }
                        }
                        else if (status == Status.Active.NullString())
                        {
                            if (row["Status"].NullString() == status || string.Compare(row["NgayNghiViec"].NullString(), timeEndUser) > 0)
                            {
                                chamCongVM = new ChamCongDataViewModel();
                                chamCongVM.MaNV = row["MaNV"].NullString();
                                chamCongVM.TenNV = row["TenNV"].NullString();
                                chamCongVM.NgayVao = row["NgayVao"].NullString();
                                chamCongVM.BoPhanDetail = row["TenBoPhanChiTiet"].NullString();
                                chamCongVM.BoPhan = row["MaBoPhan"].NullString();
                                chamCongVM.OrderBy = row["NoiTuyenDung"].NullString();
                                chamCongVM.month_Check = time;
                                chamCongVM.lstDanhMucOT = lstDanhMucOT;
                                lstResult.Add(chamCongVM);
                            }
                        }
                        else
                        {
                            chamCongVM = new ChamCongDataViewModel();
                            chamCongVM.MaNV = row["MaNV"].NullString();
                            chamCongVM.TenNV = row["TenNV"].NullString();
                            chamCongVM.NgayVao = row["NgayVao"].NullString();
                            chamCongVM.BoPhanDetail = row["TenBoPhanChiTiet"].NullString();
                            chamCongVM.BoPhan = row["MaBoPhan"].NullString();
                            chamCongVM.OrderBy = row["NoiTuyenDung"].NullString();
                            chamCongVM.month_Check = time;
                            chamCongVM.lstDanhMucOT = lstDanhMucOT;
                            lstResult.Add(chamCongVM);
                        }
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
                                    Time_KetThuc2 = row["Time_KetThuc2"].NullString(),
                                    DateModified = row["DateModified"].NullString(),
                                    CaLV_DB = row["CaLV_DB"].NullString()
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
                                    KyHieuChamCong = row["KyHieuChamCong"].NullString(),
                                    DateModified = row["DateModified"].NullString(),
                                    PhanLoaiDM = row["PhanLoaiDM"].NullString(),
                                    HeSo = double.Parse(row["Heso"].IfNullIsZero())
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
                                    NgayOT = row["NgayOT"].NullString(),
                                    DateModified = row["DateModified"].NullString()
                                });
                            }
                        }
                    }

                    // order by datemodified
                    foreach (var item in lstResult)
                    {
                        item.lstDangKyOT.Sort((x, y) => x.DateModified.CompareTo(y.DateModified));
                        item.lstChamCongDB.Sort((x, y) => x.DateModified.CompareTo(y.DateModified));
                        item.lstNhanVienCaLamViec.Sort((x, y) => x.DateModified.CompareTo(y.DateModified));
                    }

                    // lọai những khoảng thời gian cũ, lấy thời gian mới nhất.
                    int _count = 0;
                    string _dateInMonth = "";
                    foreach (var item in lstResult)
                    {

                        for (int i = 1; i <= DateTime.Parse(endMonth).Day; i++)
                        {
                            _count = 0;
                            _dateInMonth = (new DateTime(DateTime.Parse(endMonth).Year, DateTime.Parse(endMonth).Month, i)).ToString("yyyy-MM-dd");
                            foreach (var calv in item.lstNhanVienCaLamViec)
                            {
                                if (string.Compare(_dateInMonth, calv.BatDau_TheoCa) >= 0 && string.Compare(_dateInMonth, calv.KetThuc_TheoCa) <= 0)
                                {
                                    _count += 1;
                                }
                            }

                            if (_count > 1)
                            {
                                var lstCalv = item.lstNhanVienCaLamViec.FindAll(x => string.Compare(_dateInMonth, x.BatDau_TheoCa) >= 0 && string.Compare(_dateInMonth, x.KetThuc_TheoCa) <= 0).OrderByDescending(x => x.DateModified).ToList();
                                int _numberlst = lstCalv.Count;
                                for (int j = 0; j < _numberlst; j++)
                                {
                                    if (j > 0 && (lstCalv[0].BatDau_TheoCa != lstCalv[j].BatDau_TheoCa || lstCalv[0].KetThuc_TheoCa != lstCalv[j].KetThuc_TheoCa || lstCalv[0].DateModified != lstCalv[j].DateModified))
                                    {
                                        item.lstNhanVienCaLamViec.Remove(lstCalv[j]);
                                    }
                                }
                            }
                        }
                    }

                    foreach (var item in lstResult)
                    {
                        item.lstNhanVienCaLamViec.Sort((x, y) => x.BatDau_TheoCa.CompareTo(y.BatDau_TheoCa));

                    }

                    deNghiLamThemGios.Clear();

                    // thoi gian bat dau , ket thuc ngay thuong ca ngay.
                    string beginTimeCaNgay = _calaviecRespository.FindSingle(x => x.Danhmuc_CaLviec == "CN_WHC" && x.DM_NgayLViec == "NT" && x.HeSo_OT == 100).Time_BatDau;
                    string endTimeCaNgay = _calaviecRespository.FindSingle(x => x.Danhmuc_CaLviec == "CN_WHC" && x.DM_NgayLViec == "NT" && x.HeSo_OT == 100).Time_KetThuc;

                    Task<ResultItemModelBC>[] tasks = new Task<ResultItemModelBC>[lstBoPhan.Count];
                    string bpInArr = "";
                    for (int boIndex = 0; boIndex < lstBoPhan.Count; boIndex++)
                    {
                        bpInArr = lstBoPhan[boIndex];
                        List<ChamCongDataViewModel> tmpChamCong = lstResult.FindAll(x => x.BoPhan == bpInArr).ToList();
                        tasks[boIndex] = Task.Run(() =>
                        {
                            // Update du lieu tao bang cong
                            string dateCheck = "";
                            string firstTime = "";
                            string lastTime = "";
                            HopDong_NV hopDong_NV;
                            NhanVien_CaLamViec _caLamViec;
                            CHAM_CONG_LOG _chamCongLog;
                            bool isRegistedOT;
                            string kyhieuChamCongDB;
                            List<ChamCongDB> lstOTDB = new List<ChamCongDB>(); // danh sach bổ xung giờ OT như chấm bật máy
                            bool isSetMaxOT = false;
                            DeNghiLamThemGioModel denghiOTModel;

                            List<DeNghiLamThemGioModel> deNghiLamThemGios1 = new List<DeNghiLamThemGioModel>();
                            List<ChamCongDataViewModel> chamCongDataViewModels = tmpChamCong;

                            for (int i = 1; i <= DateTime.Parse(endMonth).Day; i++) // day 1 -> 31 or end month
                            {
                                dateCheck = (new DateTime(DateTime.Parse(endMonth).Year, DateTime.Parse(endMonth).Month, i)).ToString("yyyy-MM-dd");

                                foreach (var item in chamCongDataViewModels)
                                {
                                    if (item.BoPhan == "KOREA")
                                    {
                                        var timeInout_kr = CHAM_CONG_LOGs.FindAll(x => dateCheck == x.Ngay_ChamCong && item.MaNV.ToUpper() == "H" + x.ID_NV.ToUpper()).OrderByDescending(x => x.DateModified).FirstOrDefault();
                                        if (timeInout_kr != null)
                                        {
                                            if (!item.TimeInOutModels.Any(x => x.DayCheck == dateCheck && x.MaNV == item.MaNV))
                                            {
                                                string hangmuc = item.lstChamCongDB.Where(x => string.Compare(dateCheck, x.NgayBatDau) >= 0 && string.Compare(dateCheck, x.NgayKetThuc) <= 0).FirstOrDefault()?.TenChiTiet;

                                                item.TimeInOutModels.Add(new TimeInOutModel()
                                                {
                                                    MaNV = item.MaNV,
                                                    TenNV = item.TenNV,
                                                    DayCheck = dateCheck,
                                                    InTime = timeInout_kr.FirstIn_Time,
                                                    OutTime = timeInout_kr.Last_Out_Time,
                                                    HangMuc = hangmuc.NullString()
                                                });
                                            }
                                        }
                                        else
                                        {
                                            string hangmuc = item.lstChamCongDB.Where(x => string.Compare(dateCheck, x.NgayBatDau) >= 0 && string.Compare(dateCheck, x.NgayKetThuc) <= 0).FirstOrDefault()?.TenChiTiet;

                                            if (DateTime.Parse(dateCheck).DayOfWeek == DayOfWeek.Saturday && hangmuc.NullString() == "" && DateTime.Parse(dateCheck) <= DateTime.Now)
                                            {
                                                hangmuc = "nghỉ nửa ngày";
                                            }

                                            item.TimeInOutModels.Add(new TimeInOutModel()
                                            {
                                                MaNV = item.MaNV,
                                                TenNV = item.TenNV,
                                                DayCheck = dateCheck,
                                                InTime = "",
                                                OutTime = "",
                                                HangMuc = hangmuc
                                            });
                                        }
                                    }

                                    // de nghi OT
                                    denghiOTModel = new DeNghiLamThemGioModel()
                                    {
                                        MaNV = item.MaNV,
                                        TenNV = item.TenNV,
                                        BoPhan = item.BoPhan,
                                        NgayDangKy = dateCheck
                                    };

                                    if (item.MaNV == "H2212001")
                                    {

                                    }

                                    // check hop dong
                                    hopDong_NV = item.lstHopDong.FirstOrDefault(x => string.Compare(dateCheck, x.NgayHieuLucHD) >= 0 && string.Compare(dateCheck, x.NgayHetHLHD) <= 0);

                                    if (hopDong_NV == null)
                                    {
                                        hopDong_NV = new HopDong_NV();

                                        var hd = HOPDONGs.OrderByDescending(x => x.NgayHieuLuc).FirstOrDefault();
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

                                    kyhieuChamCongDB = item.lstChamCongDB.OrderByDescending(x => x.DateModified).FirstOrDefault(x => string.Compare(dateCheck, x.NgayBatDau) >= 0 && string.Compare(dateCheck, x.NgayKetThuc) <= 0 && x.PhanLoaiDM != CommonConstants.DM_OT && x.PhanLoaiDM != CommonConstants.DM_ELLC)?.KyHieuChamCong;

                                    if (CheckCheDoThaiSan("ThaiSan", dateCheck, item.MaNV))
                                    {
                                        kyhieuChamCongDB = "IL";
                                    }

                                    lstOTDB = item.lstChamCongDB.Where(x => string.Compare(dateCheck, x.NgayBatDau) >= 0 && string.Compare(dateCheck, x.NgayKetThuc) <= 0 && x.PhanLoaiDM == CommonConstants.DM_OT).ToList();

                                    if (hopDong_NV != null)
                                    {
                                        if (hopDong_NV.ShortName.NullString() == CommonConstants.HD_THUVIEC_EM) // HD Thu viec 85% luong
                                        {
                                            // check ca lam viec
                                            _caLamViec = item.lstNhanVienCaLamViec.OrderByDescending(x => x.DateModified).FirstOrDefault(x => string.Compare(dateCheck, x.BatDau_TheoCa) >= 0 && string.Compare(dateCheck, x.KetThuc_TheoCa) <= 0);

                                            if (_caLamViec != null)
                                            {
                                                // get data cham cong log 
                                                _chamCongLog = CHAM_CONG_LOGs.FindAll(x => dateCheck == x.Ngay_ChamCong && item.MaNV.ToUpper() == "H" + x.ID_NV.ToUpper()).OrderByDescending(x => x.DateModified).FirstOrDefault();

                                                if (_chamCongLog == null)
                                                {
                                                    if (CheckNgayDB(dateCheck) == 1)
                                                    {
                                                        item.WorkingStatuses.Add(new WorkingStatus()
                                                        {
                                                            DayCheck = dateCheck,
                                                            Value = kyhieuChamCongDB.NullString() == "" ? "NH" : kyhieuChamCongDB.NullString()
                                                        });
                                                    }
                                                    continue;
                                                }


                                                item.VP_SX = _chamCongLog.Department.Contains("Support") && !_chamCongLog.Department.Contains("Utility") ? CommonConstants.VP : CommonConstants.SX;

                                                // get first time and last time
                                                firstTime = _chamCongLog.FirstIn_Time.NullString();
                                                lastTime = _chamCongLog.Last_Out_Time.NullString();



                                                if (_caLamViec.MaCaLaviec == CommonConstants.CA_DEM)
                                                {
                                                    if (_chamCongLog.Last_Out_Time_Update.NullString() != "")
                                                    {
                                                        lastTime = _chamCongLog.Last_Out_Time_Update.NullString();
                                                    }

                                                    if (dateCheck == "2023-01-18" && string.Compare(lastTime, "05:30:00") > 0)
                                                    {
                                                        lastTime = "05:30:00";
                                                    }

                                                    if (firstTime != "00:00:00" && lastTime != "00:00:00" && firstTime != lastTime)
                                                    {
                                                        _chamCongLog.FirstIn = CommonConstants.IN;
                                                        _chamCongLog.LastOut = CommonConstants.OUT;
                                                    }
                                                }

                                                // Co du lieu cham cong
                                                if (_chamCongLog.FirstIn.NullString() == CommonConstants.IN && _chamCongLog.LastOut.NullString() == CommonConstants.OUT && Math.Abs(TimeSpan.Parse(lastTime).Subtract(TimeSpan.Parse(firstTime)).TotalHours) >= 0.2)
                                                {
                                                    #region THU VIEC + CA NGAY
                                                    if (_caLamViec.MaCaLaviec == CommonConstants.CA_NGAY)
                                                    {
                                                        // 1. CHECK NGAY CONG : thu viec + ca ngay 

                                                        if (dateCheck == "2023-01-19" && item.lstNhanVienCaLamViec.Any(x => string.Compare(x.BatDau_TheoCa, dateCheck) <= 0 && string.Compare(x.KetThuc_TheoCa, dateCheck) >= 0 && x.CaLV_DB != "HC"))
                                                        {
                                                            if (DateTime.ParseExact(firstTime, "HH:mm:ss", CultureInfo.InvariantCulture) < DateTime.ParseExact("07:00:00", "HH:mm:ss", CultureInfo.InvariantCulture))
                                                            {
                                                                firstTime = "07:00:00";
                                                            }
                                                        }
                                                        else
                                                        {
                                                            if (DateTime.ParseExact(firstTime, "HH:mm:ss", CultureInfo.InvariantCulture) < DateTime.ParseExact(beginTimeCaNgay, "HH:mm:ss", CultureInfo.InvariantCulture))
                                                            {
                                                                firstTime = beginTimeCaNgay;
                                                            }
                                                        }

                                                        if (CheckNgayDB(dateCheck, CommonConstants.CA_NGAY) == 0) // ngay thuong
                                                        {
                                                            // Ngay 19 thay đổi thời gian làm việc trước nghỉ tết
                                                            if (dateCheck == "2023-01-19" && item.lstNhanVienCaLamViec.Any(x => string.Compare(x.BatDau_TheoCa, dateCheck) <= 0 && string.Compare(x.KetThuc_TheoCa, dateCheck) >= 0 && x.CaLV_DB != "HC"))
                                                            {
                                                                string newBeginOT = "15:30:00";

                                                                // Ca ngay con nhỏ, văn phòng con nhỏ thì dc về sớm 1h
                                                                if (CheckCheDoThaiSan("ConNho1H", dateCheck, item.MaNV))
                                                                {
                                                                    newBeginOT = "14:00:00";
                                                                }

                                                                if (string.Compare(lastTime, newBeginOT) > 0)
                                                                {
                                                                    var clviec = item.lstNhanVienCaLamViec.FindAll(x =>
                                                                    string.Compare(dateCheck, x.BatDau_TheoCa) >= 0 &&
                                                                    string.Compare(dateCheck, x.KetThuc_TheoCa) <= 0 &&
                                                                    x.MaCaLaviec == CommonConstants.CA_NGAY && x.DM_NgayLViec == "NT" && x.HeSo_OT != 100).OrderByDescending(x => x.Time_BatDau).ThenByDescending(x => x.DateModified).FirstOrDefault();

                                                                    double timeOT = (DateTime.ParseExact(dateCheck + " " + lastTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) - DateTime.ParseExact(dateCheck + " " + newBeginOT, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)).TotalHours;

                                                                    if (timeOT < 0.1)
                                                                    {
                                                                        timeOT = 0;
                                                                    }

                                                                    if (timeOT > 0)
                                                                    {
                                                                        timeOT = HR_NHANVIEN_CHEDO_DBs.Exists(x => x.MaNhanVien == item.MaNV && x.CheDoDB.Contains("Block_10p")) ? SimpleValueOT(timeOT) : Block30mValueOT(timeOT);

                                                                        item.OvertimeValues.Add(new OvertimeValue()
                                                                        {
                                                                            DayCheckOT = dateCheck,
                                                                            DMOvertime = clviec.HeSo_OT.NullString(),
                                                                            ValueOT = timeOT,
                                                                            Registered = isRegistedOT
                                                                        });

                                                                        denghiOTModel.From = newBeginOT;
                                                                        denghiOTModel.To = lastTime;
                                                                        denghiOTModel.Duration = timeOT.IfNullIsZero();
                                                                    }
                                                                }

                                                                if (kyhieuChamCongDB.NullString() == "" && CheckCheDoThaiSan("ConNho1H", dateCheck, item.MaNV) && string.Compare(lastTime, "11:00:00") > 0 && string.Compare(lastTime, "13:00:00") < 0)
                                                                {
                                                                    kyhieuChamCongDB = "P/DS";
                                                                }

                                                                item.WorkingStatuses.Add(new WorkingStatus()
                                                                {
                                                                    DayCheck = dateCheck,
                                                                    Value = kyhieuChamCongDB.NullString() == "" ? "PD" : kyhieuChamCongDB.NullString() // PD: Probation Day shift/Thử việc ca ngày
                                                                });
                                                            }
                                                            else
                                                            {
                                                                isSetMaxOT = true;

                                                                string newBeginOT = "17:30:00";
                                                                if (item.BoPhan != CommonConstants.SUPPORT_DEPT)
                                                                {
                                                                    newBeginOT = "17:30:00";
                                                                }
                                                                else
                                                                {
                                                                    if (string.Compare(dateCheck, "2022-10-24") >= 0)
                                                                    {
                                                                        newBeginOT = "17:45:00";
                                                                    }
                                                                }

                                                                // Ca ngay con nhỏ, văn phòng con nhỏ thì dc về sớm 1h
                                                                if (CheckCheDoThaiSan("ConNho1H", dateCheck, item.MaNV))
                                                                {
                                                                    isSetMaxOT = false;
                                                                    newBeginOT = "16:00:00";

                                                                    if (string.Compare(lastTime, "17:30:00") > 0)
                                                                    {
                                                                        newBeginOT = "16:30:00";
                                                                    }
                                                                }

                                                                if ((item.BoPhan == CommonConstants.SUPPORT_DEPT || HR_NHANVIEN_CHEDO_DBs.Exists(x => x.MaNhanVien == item.MaNV && x.CheDoDB.Contains("vp_Block_10p"))) && DateTime.Parse(dateCheck).DayOfWeek == DayOfWeek.Saturday)
                                                                {
                                                                    newBeginOT = "17:30:00";
                                                                    //#region tạm thoi k dung do nghi t7

                                                                    if (!CommonConstants.arrSatudayAbNormal.Contains(dateCheck))
                                                                    {
                                                                        newBeginOT = "13:15:00";

                                                                        if (CheckCheDoThaiSan("ConNho1H", dateCheck, item.MaNV))
                                                                        {
                                                                            newBeginOT = "13:00:00";

                                                                            if (string.Compare(lastTime, "17:30:00") > 0)
                                                                            {
                                                                                newBeginOT = "13:30:00";
                                                                            }
                                                                        }
                                                                    }
                                                                    //#endregion

                                                                    isSetMaxOT = false;
                                                                }

                                                                if (string.Compare(lastTime, newBeginOT) > 0)
                                                                {
                                                                    var clviec = item.lstNhanVienCaLamViec.FindAll(x =>
                                                                    string.Compare(dateCheck, x.BatDau_TheoCa) >= 0 &&
                                                                    string.Compare(dateCheck, x.KetThuc_TheoCa) <= 0 &&
                                                                    x.MaCaLaviec == CommonConstants.CA_NGAY && x.DM_NgayLViec == "NT" && x.HeSo_OT != 100).OrderByDescending(x => x.Time_BatDau).ThenByDescending(x => x.DateModified).FirstOrDefault();

                                                                    double timeOT = (DateTime.ParseExact(dateCheck + " " + lastTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) - DateTime.ParseExact(dateCheck + " " + newBeginOT, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)).TotalHours;

                                                                    if (timeOT < 0.1)
                                                                    {
                                                                        timeOT = 0;
                                                                    }

                                                                    if (timeOT > 0)
                                                                    {
                                                                        if (newBeginOT != "17:30:00" && !isSetMaxOT)
                                                                        {
                                                                            if (timeOT > 7.5)
                                                                            {
                                                                                timeOT = 7.5;
                                                                            }
                                                                        }

                                                                        // thơi gian OT max ngay thuong
                                                                        if (timeOT > 3.5 && isSetMaxOT)
                                                                        {
                                                                            timeOT = 3.5;
                                                                        }

                                                                        timeOT = HR_NHANVIEN_CHEDO_DBs.Exists(x => x.MaNhanVien == item.MaNV && x.CheDoDB.Contains("Block_10p")) ? SimpleValueOT(timeOT) : Block30mValueOT(timeOT);

                                                                        item.OvertimeValues.Add(new OvertimeValue()
                                                                        {
                                                                            DayCheckOT = dateCheck,
                                                                            DMOvertime = clviec.HeSo_OT.NullString(),
                                                                            ValueOT = timeOT,
                                                                            Registered = isRegistedOT
                                                                        });

                                                                        denghiOTModel.From = newBeginOT;
                                                                        denghiOTModel.To = lastTime;
                                                                        denghiOTModel.Duration = timeOT.IfNullIsZero();
                                                                    }
                                                                }

                                                                if (kyhieuChamCongDB.NullString() == "" && CheckCheDoThaiSan("ConNho1H", dateCheck, item.MaNV) && string.Compare(lastTime, "11:00:00") > 0 && string.Compare(lastTime, "13:00:00") < 0)
                                                                {
                                                                    kyhieuChamCongDB = "P/DS";
                                                                }

                                                                item.WorkingStatuses.Add(new WorkingStatus()
                                                                {
                                                                    DayCheck = dateCheck,
                                                                    Value = kyhieuChamCongDB.NullString() == "" ? "PD" : kyhieuChamCongDB.NullString() // PD: Probation Day shift/Thử việc ca ngày
                                                                });
                                                            }

                                                        }
                                                        else if (CheckNgayDB(dateCheck, CommonConstants.CA_NGAY) == 5) // Ngay ki niem cty
                                                        {
                                                            var clviec = item.lstNhanVienCaLamViec.FirstOrDefault(x =>
                                                                string.Compare(dateCheck, x.BatDau_TheoCa) >= 0 &&
                                                                string.Compare(dateCheck, x.KetThuc_TheoCa) <= 0 &&
                                                                x.MaCaLaviec == CommonConstants.CA_NGAY && x.DM_NgayLViec == "NT9" && x.HeSo_OT != 100); // Ngay ki niem coi nhu ngay chu nhat

                                                            double timeOT = (DateTime.ParseExact(dateCheck + " " + lastTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) - DateTime.ParseExact(dateCheck + " 17:30:00", "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)).TotalHours;


                                                            item.WorkingStatuses.Add(new WorkingStatus()
                                                            {
                                                                DayCheck = dateCheck,
                                                                Value = kyhieuChamCongDB.NullString() == "" ? "PMD" : kyhieuChamCongDB.NullString() // Làm ca ngày thử việc ngay ki niem
                                                            });

                                                            timeOT = HR_NHANVIEN_CHEDO_DBs.Exists(x => x.MaNhanVien == item.MaNV && x.CheDoDB.Contains("Block_10p")) ? SimpleValueOT(timeOT) : Block30mValueOT(timeOT);

                                                            item.OvertimeValues.Add(new OvertimeValue()
                                                            {
                                                                DayCheckOT = dateCheck,
                                                                DMOvertime = clviec.HeSo_OT.NullString(), // nhu OT chu nhat
                                                                ValueOT = timeOT,
                                                                Registered = isRegistedOT
                                                            });

                                                            // set time de nghi làm thêm giờ
                                                            denghiOTModel.From = "17:30:00";
                                                            denghiOTModel.To = lastTime;
                                                            denghiOTModel.Duration = timeOT.IfNullIsZero();
                                                        }
                                                        else if (CheckNgayDB(dateCheck, CommonConstants.CA_NGAY) == 3 || CheckNgayDB(dateCheck, CommonConstants.CA_NGAY) == 4) // ngay chu nhat or nghi bu ngay le
                                                        {
                                                            var clviec = item.lstNhanVienCaLamViec.FirstOrDefault(x =>
                                                                 string.Compare(dateCheck, x.BatDau_TheoCa) >= 0 &&
                                                                 string.Compare(dateCheck, x.KetThuc_TheoCa) <= 0 &&
                                                                 x.MaCaLaviec == CommonConstants.CA_NGAY && x.DM_NgayLViec == "CN" && x.HeSo_OT != 100);

                                                            double timeOT = (DateTime.ParseExact(dateCheck + " " + lastTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) - DateTime.ParseExact(dateCheck + " " + firstTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)).TotalHours;
                                                            if (timeOT > 4)
                                                                timeOT = timeOT >= 9.5 ? timeOT - 1.5 : (timeOT >= 9 ? 8 : timeOT - 0.5); // 1 h nghỉ trưa

                                                            item.WorkingStatuses.Add(new WorkingStatus()
                                                            {
                                                                DayCheck = dateCheck,
                                                                Value = kyhieuChamCongDB.NullString() == "" ? "TV" : ResetULIL(kyhieuChamCongDB.NullString(), dateCheck) // TV: Thử việc làm thêm ngày chủ nhật/ Probation
                                                            });

                                                            timeOT = HR_NHANVIEN_CHEDO_DBs.Exists(x => x.MaNhanVien == item.MaNV && x.CheDoDB.Contains("Block_10p")) ? SimpleValueOT(timeOT) : Block30mValueOT(timeOT);

                                                            item.OvertimeValues.Add(new OvertimeValue()
                                                            {
                                                                DayCheckOT = dateCheck,
                                                                DMOvertime = clviec.HeSo_OT.NullString(), // nhu OT chu nhat
                                                                ValueOT = timeOT,
                                                                Registered = isRegistedOT
                                                            });

                                                            // set time de nghi làm thêm giờ
                                                            denghiOTModel.From = firstTime;
                                                            denghiOTModel.To = lastTime;
                                                            denghiOTModel.Duration = timeOT.IfNullIsZero();
                                                        }
                                                        else if (CheckNgayDB(dateCheck, CommonConstants.CA_NGAY) == 1) // ngay le
                                                        {
                                                            var clviec = item.lstNhanVienCaLamViec.FirstOrDefault(x =>
                                                             string.Compare(dateCheck, x.BatDau_TheoCa) >= 0 &&
                                                             string.Compare(dateCheck, x.KetThuc_TheoCa) <= 0 &&
                                                             x.MaCaLaviec == CommonConstants.CA_NGAY &&
                                                             (x.DM_NgayLViec == "NL" || x.DM_NgayLViec == "NLCC") && x.HeSo_OT != 100);

                                                            double timeOT = (DateTime.ParseExact(dateCheck + " " + lastTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) - DateTime.ParseExact(dateCheck + " " + firstTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)).TotalHours;
                                                            if (timeOT > 4)
                                                                timeOT = timeOT >= 9.5 ? timeOT - 1.5 : (timeOT >= 9 ? 8 : timeOT - 1); // 1 h nghỉ trưa


                                                            item.WorkingStatuses.Add(new WorkingStatus()
                                                            {
                                                                DayCheck = dateCheck,
                                                                Value = kyhieuChamCongDB.NullString() == "" ? "PD" : kyhieuChamCongDB.NullString(), // PD  thu viec ca ngay
                                                            });

                                                            timeOT = HR_NHANVIEN_CHEDO_DBs.Exists(x => x.MaNhanVien == item.MaNV && x.CheDoDB.Contains("Block_10p")) ? SimpleValueOT(timeOT) : Block30mValueOT(timeOT);

                                                            item.OvertimeValues.Add(new OvertimeValue()
                                                            {
                                                                DayCheckOT = dateCheck,
                                                                DMOvertime = clviec.HeSo_OT.NullString(),
                                                                ValueOT = timeOT,
                                                                Registered = isRegistedOT
                                                            });

                                                            // set time de nghi làm thêm giờ
                                                            denghiOTModel.From = firstTime;
                                                            denghiOTModel.To = lastTime;
                                                            denghiOTModel.Duration = timeOT.IfNullIsZero();
                                                        }
                                                        else if (CheckNgayDB(dateCheck, CommonConstants.CA_NGAY) == 2) // ngay truoc le
                                                        {
                                                            var clviec = item.lstNhanVienCaLamViec.FirstOrDefault(x =>
                                                             string.Compare(dateCheck, x.BatDau_TheoCa) >= 0 &&
                                                             string.Compare(dateCheck, x.KetThuc_TheoCa) <= 0 &&
                                                             x.MaCaLaviec == CommonConstants.CA_NGAY &&
                                                             (x.DM_NgayLViec == "TNL") && x.HeSo_OT != 100);

                                                            string newBeginOT = "17:30:00";

                                                            // Ca ngay con nhỏ, văn phòng con nhỏ thì dc về sớm 1h
                                                            if (CheckCheDoThaiSan("ConNho1H", dateCheck, item.MaNV))
                                                            {
                                                                newBeginOT = "16:00:00";

                                                                if (string.Compare(lastTime, "17:30:00") > 0)
                                                                {
                                                                    newBeginOT = "16:30:00";
                                                                }
                                                            }

                                                            if ((item.BoPhan == CommonConstants.SUPPORT_DEPT || HR_NHANVIEN_CHEDO_DBs.Exists(x => x.MaNhanVien == item.MaNV && x.CheDoDB.Contains("vp_Block_10p"))) && DateTime.Parse(dateCheck).DayOfWeek == DayOfWeek.Saturday)
                                                            {
                                                                newBeginOT = "17:30:00";
                                                                //#region tam thoi k dung do nghỉ t7
                                                                if (!CommonConstants.arrSatudayAbNormal.Contains(dateCheck))
                                                                {
                                                                    newBeginOT = "13:15:00";

                                                                    if (CheckCheDoThaiSan("ConNho1H", dateCheck, item.MaNV))
                                                                    {
                                                                        newBeginOT = "13:00:00";

                                                                        if (string.Compare(lastTime, "17:30:00") > 0)
                                                                        {
                                                                            newBeginOT = "13:30:00";
                                                                        }
                                                                    }
                                                                }
                                                                //#endregion
                                                            }

                                                            if (string.Compare(lastTime, newBeginOT) > 0)
                                                            {
                                                                if (item.BoPhan != "SP")
                                                                {
                                                                    newBeginOT = "17:30:00";

                                                                    // Ca ngay con nhỏ, văn phòng con nhỏ thì dc về sớm 1h
                                                                    if (CheckCheDoThaiSan("ConNho1H", dateCheck, item.MaNV))
                                                                    {
                                                                        newBeginOT = "16:00:00";

                                                                        if (string.Compare(lastTime, "17:30:00") > 0)
                                                                        {
                                                                            newBeginOT = "16:30:00";
                                                                        }
                                                                    }
                                                                }

                                                                double timeOT = (DateTime.ParseExact(dateCheck + " " + lastTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) - DateTime.ParseExact(dateCheck + " " + newBeginOT, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)).TotalHours;

                                                                timeOT = HR_NHANVIEN_CHEDO_DBs.Exists(x => x.MaNhanVien == item.MaNV && x.CheDoDB.Contains("Block_10p")) ? SimpleValueOT(timeOT) : Block30mValueOT(timeOT);

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

                                                                // set time de nghi làm thêm giờ
                                                                denghiOTModel.From = newBeginOT;
                                                                denghiOTModel.To = lastTime;
                                                                denghiOTModel.Duration = timeOT.IfNullIsZero();
                                                            }

                                                            item.WorkingStatuses.Add(new WorkingStatus()
                                                            {
                                                                DayCheck = dateCheck,
                                                                Value = kyhieuChamCongDB.NullString() == "" ? "PD" : kyhieuChamCongDB.NullString() // PD: Probation Day shift/Thử việc ca ngày
                                                            });
                                                        }

                                                        deNghiLamThemGios1.Add(denghiOTModel);

                                                        // Cham them OT
                                                        if (lstOTDB.Count > 0)
                                                        {
                                                            foreach (var dB in lstOTDB)
                                                            {
                                                                if (dB.KyHieuChamCong == CommonConstants.OT_BAT_MAY)
                                                                {
                                                                    if (DateTime.ParseExact(_chamCongLog.FirstIn_Time.NullString(), "HH:mm:ss", CultureInfo.InvariantCulture) < DateTime.ParseExact("08:00:00", "HH:mm:ss", CultureInfo.InvariantCulture))
                                                                    {
                                                                        var dbOT = (DateTime.ParseExact("08:00:00", "HH:mm:ss", CultureInfo.InvariantCulture) - DateTime.ParseExact(_chamCongLog.FirstIn_Time.NullString(), "HH:mm:ss", CultureInfo.InvariantCulture)).TotalHours;

                                                                        dbOT = dbOT - 0.5;// 0.5h di ăn

                                                                        if (dbOT > 2) // tối đa OT 2h bật máy.
                                                                        {
                                                                            dbOT = 2;
                                                                        }

                                                                        //if (dbOT < 0.2)
                                                                        //{
                                                                        //    dbOT = 0;
                                                                        //}

                                                                        dbOT = HR_NHANVIEN_CHEDO_DBs.Exists(x => x.MaNhanVien == item.MaNV && x.CheDoDB.Contains("Block_10p")) ? SimpleValueOT(dbOT) : Block30mValueOT(dbOT);

                                                                        item.OvertimeValues.Add(new OvertimeValue()
                                                                        {
                                                                            DayCheckOT = dateCheck,
                                                                            DMOvertime = dB.HeSo.NullString(),
                                                                            ValueOT = dbOT,
                                                                            Registered = true
                                                                        });

                                                                        DeNghiLamThemGioModel deNghiLamThem = new DeNghiLamThemGioModel()
                                                                        {
                                                                            MaNV = item.MaNV,
                                                                            TenNV = item.TenNV,
                                                                            BoPhan = item.BoPhan,
                                                                            NgayDangKy = dateCheck,
                                                                            Note = "Đi sớm bật máy.",
                                                                            From = _chamCongLog.FirstIn_Time.NullString(),
                                                                            To = "08:00:00",
                                                                            Duration = dbOT.IfNullIsZero()
                                                                        };
                                                                        deNghiLamThemGios1.Add(deNghiLamThem);
                                                                    }
                                                                }
                                                            }
                                                        }

                                                        // Di muon ve som
                                                        double ELLC = 0;
                                                        if (DateTime.ParseExact(firstTime, "HH:mm:ss", CultureInfo.InvariantCulture) > DateTime.ParseExact("08:05:00", "HH:mm:ss", CultureInfo.InvariantCulture))
                                                        {
                                                            ELLC += (DateTime.ParseExact(firstTime, "HH:mm:ss", CultureInfo.InvariantCulture) - DateTime.ParseExact("08:05:00", "HH:mm:ss", CultureInfo.InvariantCulture)).TotalHours;
                                                        }

                                                        if ((item.BoPhan == CommonConstants.SUPPORT_DEPT || HR_NHANVIEN_CHEDO_DBs.Exists(x => x.MaNhanVien == item.MaNV && x.CheDoDB.Contains("vp_Block_10p"))) && DateTime.Parse(dateCheck).DayOfWeek == DayOfWeek.Saturday)
                                                        {
                                                            //#region tam thoi k dung do nghi t7
                                                            if (!CommonConstants.arrSatudayAbNormal.Contains(dateCheck))
                                                            {
                                                                if (CheckCheDoThaiSan("ConNho1H", dateCheck, item.MaNV)) // co con nho ve som 1h
                                                                {
                                                                    if (DateTime.ParseExact(lastTime, "HH:mm:ss", CultureInfo.InvariantCulture) < DateTime.ParseExact("12:00:00", "HH:mm:ss", CultureInfo.InvariantCulture))
                                                                    {
                                                                        ELLC += (DateTime.ParseExact("12:00:00", "HH:mm:ss", CultureInfo.InvariantCulture) - DateTime.ParseExact(lastTime, "HH:mm:ss", CultureInfo.InvariantCulture)).TotalHours;
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    if (DateTime.ParseExact(lastTime, "HH:mm:ss", CultureInfo.InvariantCulture) < DateTime.ParseExact("13:15:00", "HH:mm:ss", CultureInfo.InvariantCulture))
                                                                    {
                                                                        ELLC += ((DateTime.ParseExact("13:15:00", "HH:mm:ss", CultureInfo.InvariantCulture) - DateTime.ParseExact(lastTime, "HH:mm:ss", CultureInfo.InvariantCulture)).TotalHours - 0.6);
                                                                    }
                                                                }
                                                            }
                                                            else
                                                            {
                                                                // THÊM THẠM THOI
                                                                if (DateTime.ParseExact(lastTime, "HH:mm:ss", CultureInfo.InvariantCulture) < DateTime.ParseExact("17:00:00", "HH:mm:ss", CultureInfo.InvariantCulture))
                                                                {
                                                                    if ((DateTime.ParseExact("17:00:00", "HH:mm:ss", CultureInfo.InvariantCulture) - DateTime.ParseExact(lastTime, "HH:mm:ss", CultureInfo.InvariantCulture)).TotalHours > 4)
                                                                    {
                                                                        ELLC += ((DateTime.ParseExact("17:00:00", "HH:mm:ss", CultureInfo.InvariantCulture) - DateTime.ParseExact(lastTime, "HH:mm:ss", CultureInfo.InvariantCulture)).TotalHours - 1);
                                                                    }
                                                                    else
                                                                    {
                                                                        ELLC += ((DateTime.ParseExact("17:00:00", "HH:mm:ss", CultureInfo.InvariantCulture) - DateTime.ParseExact(lastTime, "HH:mm:ss", CultureInfo.InvariantCulture)).TotalHours);
                                                                    }
                                                                }
                                                            }
                                                            // #endregion
                                                        }
                                                        else
                                                        {

                                                            if (CheckCheDoThaiSan("ConNho1H", dateCheck, item.MaNV))
                                                            {
                                                                if (DateTime.ParseExact(lastTime, "HH:mm:ss", CultureInfo.InvariantCulture) < DateTime.ParseExact("16:00:00", "HH:mm:ss", CultureInfo.InvariantCulture))
                                                                {
                                                                    ELLC += (DateTime.ParseExact("16:00:00", "HH:mm:ss", CultureInfo.InvariantCulture) - DateTime.ParseExact(lastTime, "HH:mm:ss", CultureInfo.InvariantCulture)).TotalHours;
                                                                }
                                                            }
                                                            else
                                                            {
                                                                if (DateTime.ParseExact(lastTime, "HH:mm:ss", CultureInfo.InvariantCulture) < DateTime.ParseExact("17:00:00", "HH:mm:ss", CultureInfo.InvariantCulture))
                                                                {
                                                                    if ((DateTime.ParseExact("17:00:00", "HH:mm:ss", CultureInfo.InvariantCulture) - DateTime.ParseExact(lastTime, "HH:mm:ss", CultureInfo.InvariantCulture)).TotalHours > 4)
                                                                    {
                                                                        if (HR_NHANVIEN_CHEDO_DBs.Exists(x => x.MaNhanVien == item.MaNV && x.CheDoDB.Contains("vp_Block_10p")))
                                                                        {
                                                                            ELLC += ((DateTime.ParseExact("17:00:00", "HH:mm:ss", CultureInfo.InvariantCulture) - DateTime.ParseExact(lastTime, "HH:mm:ss", CultureInfo.InvariantCulture)).TotalHours - 0.6);
                                                                        }
                                                                        else
                                                                        {
                                                                            ELLC += ((DateTime.ParseExact("17:00:00", "HH:mm:ss", CultureInfo.InvariantCulture) - DateTime.ParseExact(lastTime, "HH:mm:ss", CultureInfo.InvariantCulture)).TotalHours - 1);
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        ELLC += ((DateTime.ParseExact("17:00:00", "HH:mm:ss", CultureInfo.InvariantCulture) - DateTime.ParseExact(lastTime, "HH:mm:ss", CultureInfo.InvariantCulture)).TotalHours);
                                                                    }
                                                                }
                                                            }
                                                        }

                                                        if (ELLC < 0 || !string.IsNullOrEmpty(kyhieuChamCongDB.NullString()))
                                                        {
                                                            ELLC = ALLC_Cal(firstTime, lastTime, kyhieuChamCongDB.NullString());
                                                        };

                                                        if (HR_NHANVIEN_CHEDO_DBs.Exists(x => x.MaNhanVien == item.MaNV && x.CheDoDB.Contains("vp_Block_10p")) && string.Compare(dateCheck, "2022-10-24") >= 0)
                                                        {
                                                            // Nghỉ thứ 7  : Nghỉ phép và nghỉ không lương mặc định trừ về sớm 40p (tương ứng 0.7h)
                                                            // Nghỉ từ thứ 2-> 6 : Nghỉ phép và nghỉ không lương 0.5days mặc định trừ về sớm 40p(tương ứng 0.7h)
                                                            if (BreakHaftDay(kyhieuChamCongDB.NullString()))
                                                            {
                                                                if (DateTime.ParseExact(lastTime, "HH:mm:ss", CultureInfo.InvariantCulture) >= DateTime.ParseExact("13:30:00", "HH:mm:ss", CultureInfo.InvariantCulture))
                                                                {
                                                                    ELLC += 0;
                                                                }
                                                                else
                                                                {
                                                                    ELLC += 0.7;
                                                                }
                                                            }
                                                            else if ((new int[] { 1, 3, 4 }).Contains(CheckNgayDB(dateCheck, CommonConstants.CA_NGAY)))
                                                            {
                                                                ELLC = 0;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            if (BreakHaftDay(kyhieuChamCongDB.NullString()) || (new int[] { 1, 3, 4 }).Contains(CheckNgayDB(dateCheck, CommonConstants.CA_NGAY)))
                                                            {
                                                                ELLC = 0;
                                                            }
                                                        }

                                                        if (dateCheck == "2023-01-19" && item.lstNhanVienCaLamViec.Any(x => string.Compare(x.BatDau_TheoCa, dateCheck) <= 0 && string.Compare(x.KetThuc_TheoCa, dateCheck) >= 0 && x.CaLV_DB != "HC"))
                                                        {
                                                            ELLC = 0;
                                                        }

                                                        var dmvs = DANGKY_DIMUON_VSOM_NHANVIENs.FirstOrDefault(x => x.MaNV == item.MaNV && x.NgayDangKy == dateCheck);
                                                        if (dmvs != null)
                                                        {
                                                            ELLC = dmvs.SoGioDangKy;
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
                                                        if (dateCheck == "2023-01-19" && item.lstNhanVienCaLamViec.Any(x => string.Compare(x.BatDau_TheoCa, dateCheck) <= 0 && string.Compare(x.KetThuc_TheoCa, dateCheck) >= 0 && x.CaLV_DB != "HC"))
                                                        {
                                                            if (DateTime.ParseExact(firstTime, "HH:mm:ss", CultureInfo.InvariantCulture) < DateTime.ParseExact("15:00:00", "HH:mm:ss", CultureInfo.InvariantCulture) &&
                                                            DateTime.ParseExact(firstTime, "HH:mm:ss", CultureInfo.InvariantCulture) > DateTime.ParseExact("08:00:00", "HH:mm:ss", CultureInfo.InvariantCulture))
                                                            {
                                                                firstTime = "15:00:00";
                                                            }
                                                        }
                                                        else
                                                        {
                                                            if (DateTime.ParseExact(firstTime, "HH:mm:ss", CultureInfo.InvariantCulture) < DateTime.ParseExact("20:00:00", "HH:mm:ss", CultureInfo.InvariantCulture))
                                                            {
                                                                firstTime = "20:00:00";
                                                            }

                                                            if (DateTime.ParseExact(lastTime, "HH:mm:ss", CultureInfo.InvariantCulture) > DateTime.ParseExact("10:00:00", "HH:mm:ss", CultureInfo.InvariantCulture) &&
                                                                DateTime.ParseExact(lastTime, "HH:mm:ss", CultureInfo.InvariantCulture) < DateTime.ParseExact("17:00:00", "HH:mm:ss", CultureInfo.InvariantCulture))
                                                            {
                                                                lastTime = "10:00:00";
                                                            }
                                                        }

                                                        if (CheckNgayDB(dateCheck, CommonConstants.CA_DEM) == 0) // ngay thuong
                                                        {

                                                            if (dateCheck == "2023-01-19" && item.lstNhanVienCaLamViec.Any(x => string.Compare(x.BatDau_TheoCa, dateCheck) <= 0 && string.Compare(x.KetThuc_TheoCa, dateCheck) >= 0 && x.CaLV_DB != "HC"))
                                                            {
                                                                if (kyhieuChamCongDB.NullString() == "" && CheckCheDoThaiSan("ConNho1H", dateCheck, item.MaNV) && string.Compare(lastTime, "18:00:00") > 0 && string.Compare(lastTime, "19:00:00") < 0)
                                                                {
                                                                    kyhieuChamCongDB = "P/NS";
                                                                }

                                                                item.WorkingStatuses.Add(new WorkingStatus()
                                                                {
                                                                    DayCheck = dateCheck,
                                                                    Value = kyhieuChamCongDB.NullString() == "" ? "PN" : kyhieuChamCongDB.NullString() // PN: Probation Night shift/Thử việc ca đêm
                                                                });
                                                            }
                                                            else
                                                            {
                                                                var clviecs = item.lstNhanVienCaLamViec.FindAll(x => string.Compare(dateCheck, x.BatDau_TheoCa) >= 0 && string.Compare(dateCheck, x.KetThuc_TheoCa) <= 0 && x.MaCaLaviec == CommonConstants.CA_DEM && (x.DM_NgayLViec == "NT") && x.HeSo_OT != 100);

                                                                string hsOT1 = clviecs.OrderByDescending(x => x.DateModified).FirstOrDefault(x => x.Time_KetThuc == "06:00:00").HeSo_OT.NullString();
                                                                string hsOT2 = clviecs.OrderByDescending(x => x.DateModified).FirstOrDefault(x => x.Time_KetThuc == "08:00:00").HeSo_OT.NullString();

                                                                string newBeginOT = "05:30:00";

                                                                // Ca dem con nhỏ, văn phòng con nhỏ thì dc về sớm 1h
                                                                if (CheckCheDoThaiSan("ConNho1H", dateCheck, item.MaNV))
                                                                {
                                                                    newBeginOT = "04:30:00";
                                                                }

                                                                if (string.Compare(lastTime, newBeginOT) > 0)
                                                                {
                                                                    double timeOT = (DateTime.ParseExact(dateCheck + " " + lastTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) - DateTime.ParseExact(dateCheck + " " + newBeginOT, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)).TotalHours;

                                                                    timeOT = HR_NHANVIEN_CHEDO_DBs.Exists(x => x.MaNhanVien == item.MaNV && x.CheDoDB.Contains("Block_10p")) ? SimpleValueOT(timeOT) : Block30mValueOT(timeOT);

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
                                                                            ValueOT = timeOT - 0.5, /*> 2 ? 2 : (timeOT - 0.5 < 0.5 ? 0 : timeOT - 0.5)*/ // tru di 0.5h cua 5-6h
                                                                            Registered = isRegistedOT
                                                                        });
                                                                    }

                                                                    // set time de nghi làm thêm giờ
                                                                    denghiOTModel.From = newBeginOT;
                                                                    denghiOTModel.To = lastTime;
                                                                    denghiOTModel.Duration = timeOT.IfNullIsZero(); //timeOT > 2.5 ? "2.5" : timeOT.IfNullIsZero();
                                                                }

                                                                if (kyhieuChamCongDB.NullString() == "" && CheckCheDoThaiSan("ConNho1H", dateCheck, item.MaNV) && string.Compare(lastTime, "23:00:00") > 0 && string.Compare(lastTime, "23:59:59") < 0)
                                                                {
                                                                    kyhieuChamCongDB = "P/NS";
                                                                }

                                                                item.WorkingStatuses.Add(new WorkingStatus()
                                                                {
                                                                    DayCheck = dateCheck,
                                                                    Value = kyhieuChamCongDB.NullString() == "" ? "PN" : kyhieuChamCongDB.NullString() // PN: Probation Night shift/Thử việc ca đêm
                                                                });
                                                            }


                                                        }
                                                        else if (CheckNgayDB(dateCheck, CommonConstants.CA_DEM) == 5) // Ngay ki niem cty
                                                        {
                                                            item.OvertimeValues.AddRange(GetOvertimeInNight(firstTime, lastTime, dateCheck, "NT9", isRegistedOT, item.MaNV, ref denghiOTModel));
                                                            item.WorkingStatuses.Add(new WorkingStatus()
                                                            {
                                                                DayCheck = dateCheck,
                                                                Value = kyhieuChamCongDB.NullString() == "" ? "PM" : kyhieuChamCongDB.NullString() // PM: Làm ca đêm ngày kỷ niệm  thử việc
                                                            });
                                                        }
                                                        else if (CheckNgayDB(dateCheck, CommonConstants.CA_DEM) == 3 || CheckNgayDB(dateCheck, CommonConstants.CA_DEM) == 4) // ngay chu nhat + ngay nghi bu
                                                        {
                                                            item.OvertimeValues.AddRange(GetOvertimeInNight(firstTime, lastTime, dateCheck, "CN", isRegistedOT, item.MaNV, ref denghiOTModel));
                                                            item.WorkingStatuses.Add(new WorkingStatus()
                                                            {
                                                                DayCheck = dateCheck,
                                                                Value = kyhieuChamCongDB.NullString() == "" ? "TVD" : ResetULIL(kyhieuChamCongDB.NullString(), dateCheck) // TVD: Thử việc làm thêm ca đêm chủ nhật
                                                            });
                                                        }
                                                        else if (CheckNgayDB(dateCheck, CommonConstants.CA_DEM) == 1) // ngay le
                                                        {
                                                            var ngayle = NGAY_LE_NAMs.FirstOrDefault(x => x.Id == dateCheck);
                                                            if (ngayle.IslastHoliday == CommonConstants.N)
                                                            {
                                                                item.OvertimeValues.AddRange(GetOvertimeInNight(firstTime, lastTime, dateCheck, "NL", isRegistedOT, item.MaNV, ref denghiOTModel));
                                                            }
                                                            else // ngay le cuoi cung
                                                            {
                                                                item.OvertimeValues.AddRange(GetOvertimeInNight(firstTime, lastTime, dateCheck, "NLCC", isRegistedOT, item.MaNV, ref denghiOTModel));
                                                            }

                                                            item.WorkingStatuses.Add(new WorkingStatus()
                                                            {
                                                                DayCheck = dateCheck,
                                                                Value = kyhieuChamCongDB.NullString() == "" ? "PDD" : kyhieuChamCongDB.NullString() // PDD: Thử việc làm đêm ngày lễ
                                                            });
                                                        }
                                                        else if (CheckNgayDB(dateCheck, CommonConstants.CA_DEM) == 2) // ngay truoc le
                                                        {
                                                            item.OvertimeValues.AddRange(GetOvertimeInNight(firstTime, lastTime, dateCheck, "TNL", isRegistedOT, item.MaNV, ref denghiOTModel));
                                                            item.WorkingStatuses.Add(new WorkingStatus()
                                                            {
                                                                DayCheck = dateCheck,
                                                                Value = kyhieuChamCongDB.NullString() == "" ? "PH" : kyhieuChamCongDB.NullString() // PH: làm ca đêm trước ngày lễ( thử việc)
                                                            });
                                                        }

                                                        deNghiLamThemGios1.Add(denghiOTModel);

                                                        // Cham them OT
                                                        if (lstOTDB.Count > 0)
                                                        {
                                                            foreach (var dB in lstOTDB)
                                                            {
                                                                if (dB.KyHieuChamCong == CommonConstants.OT_BAT_MAY)
                                                                {
                                                                    if (DateTime.ParseExact(_chamCongLog.FirstIn_Time.NullString(), "HH:mm:ss", CultureInfo.InvariantCulture) < DateTime.ParseExact("20:00:00", "HH:mm:ss", CultureInfo.InvariantCulture))
                                                                    {
                                                                        var dbOT = (DateTime.ParseExact("20:00:00", "HH:mm:ss", CultureInfo.InvariantCulture) - DateTime.ParseExact(_chamCongLog.FirstIn_Time.NullString(), "HH:mm:ss", CultureInfo.InvariantCulture)).TotalHours;

                                                                        dbOT = dbOT - 0.5;// 0.5h di ăn

                                                                        if (dbOT > 2) // tối đa OT 2h bật máy.
                                                                        {
                                                                            dbOT = 2;
                                                                        }

                                                                        dbOT = HR_NHANVIEN_CHEDO_DBs.Exists(x => x.MaNhanVien == item.MaNV && x.CheDoDB.Contains("Block_10p")) ? SimpleValueOT(dbOT) : Block30mValueOT(dbOT);

                                                                        item.OvertimeValues.Add(new OvertimeValue()
                                                                        {
                                                                            DayCheckOT = dateCheck,
                                                                            DMOvertime = dB.HeSo.NullString(),
                                                                            ValueOT = dbOT,
                                                                            Registered = true
                                                                        });

                                                                        DeNghiLamThemGioModel deNghiLamThem = new DeNghiLamThemGioModel()
                                                                        {
                                                                            MaNV = item.MaNV,
                                                                            TenNV = item.TenNV,
                                                                            BoPhan = item.BoPhan,
                                                                            NgayDangKy = dateCheck,
                                                                            Note = "Đi sớm bật máy.",
                                                                            From = _chamCongLog.FirstIn_Time.NullString(),
                                                                            To = "20:00:00",
                                                                            Duration = dbOT.IfNullIsZero()
                                                                        };
                                                                        deNghiLamThemGios1.Add(deNghiLamThem);
                                                                    }
                                                                }
                                                            }
                                                        }

                                                        // Di muon ve som
                                                        double ELLC = 0;
                                                        if (DateTime.ParseExact(firstTime, "HH:mm:ss", CultureInfo.InvariantCulture) > DateTime.ParseExact("20:05:00", "HH:mm:ss", CultureInfo.InvariantCulture))
                                                        {
                                                            ELLC += (DateTime.ParseExact(firstTime, "HH:mm:ss", CultureInfo.InvariantCulture) - DateTime.ParseExact("20:05:00", "HH:mm:ss", CultureInfo.InvariantCulture)).TotalHours;
                                                        }

                                                        // Ca dem con nhỏ, văn phòng con nhỏ thì dc về sớm 1h
                                                        if (CheckCheDoThaiSan("ConNho1H", dateCheck, item.MaNV))
                                                        {
                                                            if (DateTime.ParseExact(lastTime, "HH:mm:ss", CultureInfo.InvariantCulture) < DateTime.ParseExact("04:00:00", "HH:mm:ss", CultureInfo.InvariantCulture))
                                                            {
                                                                ELLC += (DateTime.ParseExact("04:00:00", "HH:mm:ss", CultureInfo.InvariantCulture) - DateTime.ParseExact(lastTime, "HH:mm:ss", CultureInfo.InvariantCulture)).TotalHours;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            if (DateTime.ParseExact(lastTime, "HH:mm:ss", CultureInfo.InvariantCulture) < DateTime.ParseExact("05:00:00", "HH:mm:ss", CultureInfo.InvariantCulture))
                                                            {
                                                                ELLC += (DateTime.ParseExact("05:00:00", "HH:mm:ss", CultureInfo.InvariantCulture) - DateTime.ParseExact(lastTime, "HH:mm:ss", CultureInfo.InvariantCulture)).TotalHours;
                                                            }
                                                            else if (DateTime.ParseExact(lastTime, "HH:mm:ss", CultureInfo.InvariantCulture) >= DateTime.ParseExact("20:00:00", "HH:mm:ss", CultureInfo.InvariantCulture) &&
                                                                DateTime.ParseExact(lastTime, "HH:mm:ss", CultureInfo.InvariantCulture) < DateTime.ParseExact("23:59:59", "HH:mm:ss", CultureInfo.InvariantCulture))
                                                            {
                                                                ELLC += 4 + (DateTime.ParseExact("23:59:59", "HH:mm:ss", CultureInfo.InvariantCulture) - DateTime.ParseExact(lastTime, "HH:mm:ss", CultureInfo.InvariantCulture)).TotalHours;
                                                            }
                                                        }

                                                        if (ELLC < 0 || !string.IsNullOrEmpty(kyhieuChamCongDB.NullString()))
                                                        {
                                                            ELLC = ALLC_Cal(firstTime, lastTime, kyhieuChamCongDB.NullString());
                                                        };

                                                        if (HR_NHANVIEN_CHEDO_DBs.Exists(x => x.MaNhanVien == item.MaNV && x.CheDoDB.Contains("vp_Block_10p")) && string.Compare(dateCheck, "2022-10-24") >= 0)
                                                        {
                                                            // Nghỉ thứ 7  : Nghỉ phép và nghỉ không lương mặc định trừ về sớm 40p (tương ứng 0.7h)
                                                            // Nghỉ từ thứ 2-> 6 : Nghỉ phép và nghỉ không lương 0.5days mặc định trừ về sớm 40p(tương ứng 0.7h)
                                                            if (BreakHaftDay(kyhieuChamCongDB.NullString()))
                                                            {
                                                                if (DateTime.ParseExact(lastTime, "HH:mm:ss", CultureInfo.InvariantCulture) >= DateTime.ParseExact("13:30:00", "HH:mm:ss", CultureInfo.InvariantCulture))
                                                                {
                                                                    ELLC += 0;
                                                                }
                                                                else
                                                                {
                                                                    ELLC += 0.7;
                                                                }
                                                            }
                                                            else if ((new int[] { 1, 3, 4 }).Contains(CheckNgayDB(dateCheck, CommonConstants.CA_NGAY)))
                                                            {
                                                                ELLC = 0;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            if (BreakHaftDay(kyhieuChamCongDB.NullString()) || (new int[] { 1, 3, 4 }).Contains(CheckNgayDB(dateCheck, CommonConstants.CA_DEM)))
                                                            {
                                                                ELLC = 0;
                                                            }
                                                        }

                                                        if (dateCheck == "2023-01-19" && item.lstNhanVienCaLamViec.Any(x => string.Compare(x.BatDau_TheoCa, dateCheck) <= 0 && string.Compare(x.KetThuc_TheoCa, dateCheck) >= 0 && x.CaLV_DB != "HC"))
                                                        {
                                                            ELLC = 0;
                                                        }


                                                        var dmvs = DANGKY_DIMUON_VSOM_NHANVIENs.FirstOrDefault(x => x.MaNV == item.MaNV && x.NgayDangKy == dateCheck);
                                                        if (dmvs != null)
                                                        {
                                                            ELLC = dmvs.SoGioDangKy;
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
                                                    int checkDate = CheckNgayDB(dateCheck, _caLamViec.MaCaLaviec);

                                                    if (checkDate == 1)
                                                    {
                                                        item.WorkingStatuses.Add(new WorkingStatus()
                                                        {
                                                            DayCheck = dateCheck,
                                                            Value = kyhieuChamCongDB.NullString() == "" ? "PD" : kyhieuChamCongDB.NullString() // NH: NationALHoliday/ Nghỉ lễ
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

                                                    double ELLC = 0;
                                                    var dmvs = DANGKY_DIMUON_VSOM_NHANVIENs.FirstOrDefault(x => x.MaNV == item.MaNV && x.NgayDangKy == dateCheck);
                                                    if (dmvs != null)
                                                    {
                                                        ELLC = dmvs.SoGioDangKy;
                                                    }

                                                    item.EL_LC_Statuses.Add(new EL_LC_Status()
                                                    {
                                                        DayCheck_EL = dateCheck,
                                                        Value_EL = ELLC
                                                    });
                                                }
                                                else if (_chamCongLog.FirstIn.NullString() == "" && _chamCongLog.LastOut.NullString() != "")
                                                {
                                                    int checkDate = CheckNgayDB(dateCheck, _caLamViec.MaCaLaviec);

                                                    if (checkDate == 1)
                                                    {
                                                        item.WorkingStatuses.Add(new WorkingStatus()
                                                        {
                                                            DayCheck = dateCheck,
                                                            Value = kyhieuChamCongDB.NullString() == "" ? "PD" : kyhieuChamCongDB.NullString() // NH: NationALHoliday/ Nghỉ lễ
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

                                                    double ELLC = 0;
                                                    var dmvs = DANGKY_DIMUON_VSOM_NHANVIENs.FirstOrDefault(x => x.MaNV == item.MaNV && x.NgayDangKy == dateCheck);
                                                    if (dmvs != null)
                                                    {
                                                        ELLC = dmvs.SoGioDangKy;
                                                    }

                                                    item.EL_LC_Statuses.Add(new EL_LC_Status()
                                                    {
                                                        DayCheck_EL = dateCheck,
                                                        Value_EL = ELLC
                                                    });
                                                }
                                                #endregion
                                            }
                                            else
                                            {
                                                if (CheckCheDoThaiSan("ThaiSan", dateCheck, item.MaNV) && !IsChuNhat(dateCheck))
                                                {
                                                    item.WorkingStatuses.Add(new WorkingStatus()
                                                    {
                                                        DayCheck = dateCheck,
                                                        Value = "IL"
                                                    });
                                                }
                                                else if (CheckCheDoThaiSan("ThaiSan", dateCheck, item.MaNV) && IsChuNhat(dateCheck))
                                                {
                                                    item.WorkingStatuses.Add(new WorkingStatus()
                                                    {
                                                        DayCheck = dateCheck,
                                                        Value = "-"
                                                    });
                                                }
                                            }
                                        }
                                        else // HD Chinh thuc or tviec 100%
                                        {
                                            // check ca lam viec
                                            _caLamViec = item.lstNhanVienCaLamViec.FirstOrDefault(x => string.Compare(dateCheck, x.BatDau_TheoCa) >= 0 && string.Compare(dateCheck, x.KetThuc_TheoCa) <= 0);

                                            if (_caLamViec != null)
                                            {
                                                // get data cham cong log 
                                                _chamCongLog = CHAM_CONG_LOGs.FindAll(x => dateCheck == x.Ngay_ChamCong && item.MaNV.ToUpper() == "H" + x.ID_NV.ToUpper()).FirstOrDefault();

                                                if (_chamCongLog == null)
                                                {
                                                    if (CheckNgayDB(dateCheck) == 1)
                                                    {
                                                        item.WorkingStatuses.Add(new WorkingStatus()
                                                        {
                                                            DayCheck = dateCheck,
                                                            Value = kyhieuChamCongDB.NullString() == "" ? "NH" : kyhieuChamCongDB.NullString()
                                                        });
                                                    }
                                                    continue;
                                                }

                                                item.VP_SX = _chamCongLog.Department.Contains("Support") && !_chamCongLog.Department.Contains("Utility") ? CommonConstants.VP : CommonConstants.SX;

                                                // get first time and last time
                                                firstTime = _chamCongLog.FirstIn_Time.NullString();
                                                lastTime = _chamCongLog.Last_Out_Time.NullString();

                                                if (_caLamViec.MaCaLaviec == CommonConstants.CA_DEM)
                                                {
                                                    if (_chamCongLog.Last_Out_Time_Update.NullString() != "")
                                                    {
                                                        lastTime = _chamCongLog.Last_Out_Time_Update.NullString();
                                                    }

                                                    if (dateCheck == "2023-01-18" && string.Compare(lastTime, "05:30:00") > 0)
                                                    {
                                                        lastTime = "05:30:00";
                                                    }

                                                    if (firstTime != "00:00:00" && lastTime != "00:00:00" && firstTime != lastTime)
                                                    {
                                                        _chamCongLog.FirstIn = CommonConstants.IN;
                                                        _chamCongLog.LastOut = CommonConstants.OUT;
                                                    }
                                                }

                                                // Co du lieu cham cong
                                                if (_chamCongLog.FirstIn.NullString() == CommonConstants.IN && _chamCongLog.LastOut.NullString() == CommonConstants.OUT && Math.Abs(TimeSpan.Parse(lastTime).Subtract(TimeSpan.Parse(firstTime)).TotalHours) >= 0.2)
                                                {
                                                    #region CHINH THUC + CA NGAY
                                                    if (_caLamViec.MaCaLaviec == CommonConstants.CA_NGAY)
                                                    {
                                                        if (dateCheck == "2023-01-19" && item.lstNhanVienCaLamViec.Any(x => string.Compare(x.BatDau_TheoCa, dateCheck) <= 0 && string.Compare(x.KetThuc_TheoCa, dateCheck) >= 0 && x.CaLV_DB != "HC"))
                                                        {
                                                            if (DateTime.ParseExact(firstTime, "HH:mm:ss", CultureInfo.InvariantCulture) < DateTime.ParseExact("07:00:00", "HH:mm:ss", CultureInfo.InvariantCulture))
                                                            {
                                                                firstTime = "07:00:00";
                                                            }
                                                        }
                                                        else
                                                        {
                                                            if (DateTime.ParseExact(firstTime, "HH:mm:ss", CultureInfo.InvariantCulture) < DateTime.ParseExact("08:00:00", "HH:mm:ss", CultureInfo.InvariantCulture))
                                                            {
                                                                firstTime = "08:00:00";
                                                            }
                                                        }

                                                        if (CheckNgayDB(dateCheck, CommonConstants.CA_NGAY) == 0) // ngay thuong
                                                        {
                                                            if (dateCheck == "2023-01-19" && item.lstNhanVienCaLamViec.Any(x => string.Compare(x.BatDau_TheoCa, dateCheck) <= 0 && string.Compare(x.KetThuc_TheoCa, dateCheck) >= 0 && x.CaLV_DB != "HC"))
                                                            {
                                                                string newBeginOT = "15:30:00";

                                                                // Ca ngay con nhỏ, văn phòng con nhỏ thì dc về sớm 1h
                                                                if (CheckCheDoThaiSan("ConNho1H", dateCheck, item.MaNV))
                                                                {
                                                                    newBeginOT = "14:00:00";
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
                                                                        timeOT = HR_NHANVIEN_CHEDO_DBs.Exists(x => x.MaNhanVien == item.MaNV && x.CheDoDB.Contains("Block_10p")) ? SimpleValueOT(timeOT) : Block30mValueOT(timeOT);

                                                                        item.OvertimeValues.Add(new OvertimeValue()
                                                                        {
                                                                            DayCheckOT = dateCheck,
                                                                            DMOvertime = clviec.HeSo_OT.NullString(),
                                                                            ValueOT = timeOT,
                                                                            Registered = isRegistedOT
                                                                        });

                                                                        denghiOTModel.From = newBeginOT;
                                                                        denghiOTModel.To = lastTime;
                                                                        denghiOTModel.Duration = timeOT.IfNullIsZero();
                                                                    }
                                                                }

                                                                if (kyhieuChamCongDB.NullString() == "" && CheckCheDoThaiSan("ConNho1H", dateCheck, item.MaNV) && string.Compare(lastTime, "11:00:00") > 0 && string.Compare(lastTime, "12:00:00") < 0)
                                                                {
                                                                    kyhieuChamCongDB = "AL/DS";
                                                                }

                                                                item.WorkingStatuses.Add(new WorkingStatus()
                                                                {
                                                                    DayCheck = dateCheck,
                                                                    Value = kyhieuChamCongDB.NullString() == "" ? "DS" : kyhieuChamCongDB.NullString() // DS: Day Shift/ Ca ngày 
                                                                });
                                                            }
                                                            else
                                                            {
                                                                isSetMaxOT = true;
                                                                string newBeginOT = "17:30:00";
                                                                if ((item.BoPhan == CommonConstants.SUPPORT_DEPT || HR_NHANVIEN_CHEDO_DBs.Exists(x => x.MaNhanVien == item.MaNV && x.CheDoDB.Contains("vp_Block_10p"))) && DateTime.Parse(dateCheck).DayOfWeek == DayOfWeek.Saturday)
                                                                {
                                                                    //#region tam thoi k dung do nghi t7
                                                                    if (!CommonConstants.arrSatudayAbNormal.Contains(dateCheck))
                                                                    {
                                                                        newBeginOT = "13:15:00";

                                                                        if (CheckCheDoThaiSan("ConNho1H", dateCheck, item.MaNV))
                                                                        {
                                                                            newBeginOT = "13:00:00";

                                                                            if (string.Compare(lastTime, "17:30:00") > 0)
                                                                            {
                                                                                newBeginOT = "13:30:00";
                                                                            }
                                                                        }
                                                                    }
                                                                    // #endregion

                                                                    isSetMaxOT = false;
                                                                }
                                                                else
                                                                {
                                                                    if (item.BoPhan != "SP")
                                                                    {
                                                                        newBeginOT = "17:30:00";
                                                                    }
                                                                    else
                                                                    {
                                                                        if (string.Compare(dateCheck, "2022-10-24") >= 0)
                                                                        {
                                                                            newBeginOT = "17:45:00";
                                                                        }
                                                                    }

                                                                    // Ca ngay con nhỏ, văn phòng con nhỏ thì dc về sớm 1h
                                                                    if (CheckCheDoThaiSan("ConNho1H", dateCheck, item.MaNV))
                                                                    {
                                                                        newBeginOT = "16:00:00";
                                                                        isSetMaxOT = false;

                                                                        if (string.Compare(lastTime, "17:30:00") > 0)
                                                                        {
                                                                            newBeginOT = "16:30:00";
                                                                        }
                                                                    }
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
                                                                        if (newBeginOT != "17:30:00" && !isSetMaxOT) // thu 7 neu tang ca den 10h
                                                                        {
                                                                            if (timeOT > 7.5)
                                                                            {
                                                                                timeOT = 7.5;
                                                                            }
                                                                        }

                                                                        if (timeOT > 3.5 && isSetMaxOT)
                                                                        {
                                                                            timeOT = 3.5;
                                                                        }

                                                                        timeOT = HR_NHANVIEN_CHEDO_DBs.Exists(x => x.MaNhanVien == item.MaNV && x.CheDoDB.Contains("Block_10p")) ? SimpleValueOT(timeOT) : Block30mValueOT(timeOT);

                                                                        item.OvertimeValues.Add(new OvertimeValue()
                                                                        {
                                                                            DayCheckOT = dateCheck,
                                                                            DMOvertime = clviec.HeSo_OT.NullString(),
                                                                            ValueOT = timeOT,
                                                                            Registered = isRegistedOT
                                                                        });

                                                                        denghiOTModel.From = newBeginOT;
                                                                        denghiOTModel.To = lastTime;
                                                                        denghiOTModel.Duration = timeOT.IfNullIsZero();
                                                                    }
                                                                }

                                                                if (kyhieuChamCongDB.NullString() == "" && CheckCheDoThaiSan("ConNho1H", dateCheck, item.MaNV) && string.Compare(lastTime, "11:00:00") > 0 && string.Compare(lastTime, "13:00:00") < 0)
                                                                {
                                                                    kyhieuChamCongDB = "AL/DS";
                                                                }

                                                                item.WorkingStatuses.Add(new WorkingStatus()
                                                                {
                                                                    DayCheck = dateCheck,
                                                                    Value = kyhieuChamCongDB.NullString() == "" ? "DS" : kyhieuChamCongDB.NullString() // DS: Day Shift/ Ca ngày 
                                                                });
                                                            }
                                                        }
                                                        else if (CheckNgayDB(dateCheck, CommonConstants.CA_NGAY) == 5) // Ngay ki niem cty
                                                        {
                                                            double timeOT = (DateTime.ParseExact(dateCheck + " " + lastTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) - DateTime.ParseExact(dateCheck + " " + "17:30:00", "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)).TotalHours;

                                                            item.WorkingStatuses.Add(new WorkingStatus()
                                                            {
                                                                DayCheck = dateCheck,
                                                                Value = kyhieuChamCongDB.NullString() == "" ? "MD" : kyhieuChamCongDB.NullString() // làm ca ngày chính thức ngay ki niem
                                                            });

                                                            var clviec = item.lstNhanVienCaLamViec.FirstOrDefault(x =>
                                                               string.Compare(dateCheck, x.BatDau_TheoCa) >= 0 &&
                                                               string.Compare(dateCheck, x.KetThuc_TheoCa) <= 0 &&
                                                               x.MaCaLaviec == CommonConstants.CA_NGAY && x.DM_NgayLViec == "NT9" && x.HeSo_OT != 100);

                                                            timeOT = HR_NHANVIEN_CHEDO_DBs.Exists(x => x.MaNhanVien == item.MaNV && x.CheDoDB.Contains("Block_10p")) ? SimpleValueOT(timeOT) : Block30mValueOT(timeOT);

                                                            item.OvertimeValues.Add(new OvertimeValue()
                                                            {
                                                                DayCheckOT = dateCheck,
                                                                DMOvertime = clviec.HeSo_OT.NullString(), // nhu OT chu nhat
                                                                ValueOT = timeOT,
                                                                Registered = isRegistedOT
                                                            });

                                                            denghiOTModel.From = "17:30:00";
                                                            denghiOTModel.To = lastTime;
                                                            denghiOTModel.Duration = timeOT.IfNullIsZero();
                                                        }
                                                        else if (CheckNgayDB(dateCheck, CommonConstants.CA_NGAY) == 3 || CheckNgayDB(dateCheck, CommonConstants.CA_NGAY) == 4) // ngay chu nhat or nghi bu ngay le
                                                        {
                                                            double timeOT = (DateTime.ParseExact(dateCheck + " " + lastTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) - DateTime.ParseExact(dateCheck + " " + firstTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)).TotalHours;

                                                            if (timeOT > 4)
                                                            {
                                                                timeOT = timeOT >= 9.5 ? timeOT - 1.5 : (timeOT >= 9 ? 8 : timeOT - 0.5); // 1 h nghỉ trưa
                                                            }

                                                            item.WorkingStatuses.Add(new WorkingStatus()
                                                            {
                                                                DayCheck = dateCheck,
                                                                Value = kyhieuChamCongDB.NullString() == "" ? "-" : ResetULIL(kyhieuChamCongDB.NullString(), dateCheck) // lam chu nhat ca ngay k dung ky hieu DS
                                                            });

                                                            var clviec = item.lstNhanVienCaLamViec.FirstOrDefault(x =>
                                                              string.Compare(dateCheck, x.BatDau_TheoCa) >= 0 &&
                                                              string.Compare(dateCheck, x.KetThuc_TheoCa) <= 0 &&
                                                              x.MaCaLaviec == CommonConstants.CA_NGAY && x.DM_NgayLViec == "CN" && x.HeSo_OT != 100); // Ngay ki niem coi nhu ngay chu nhat

                                                            timeOT = HR_NHANVIEN_CHEDO_DBs.Exists(x => x.MaNhanVien == item.MaNV && x.CheDoDB.Contains("Block_10p")) ? SimpleValueOT(timeOT) : Block30mValueOT(timeOT);

                                                            item.OvertimeValues.Add(new OvertimeValue()
                                                            {
                                                                DayCheckOT = dateCheck,
                                                                DMOvertime = clviec.HeSo_OT.NullString(), // nhu OT chu nhat
                                                                ValueOT = timeOT,
                                                                Registered = isRegistedOT
                                                            });

                                                            denghiOTModel.From = firstTime;
                                                            denghiOTModel.To = lastTime;
                                                            denghiOTModel.Duration = timeOT.IfNullIsZero();
                                                        }
                                                        else if (CheckNgayDB(dateCheck, CommonConstants.CA_NGAY) == 1) // ngay le
                                                        {
                                                            double timeOT = (DateTime.ParseExact(dateCheck + " " + lastTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) - DateTime.ParseExact(dateCheck + " " + firstTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)).TotalHours;
                                                            if (timeOT > 4)
                                                                timeOT = timeOT >= 9.5 ? timeOT - 1.5 : (timeOT >= 9 ? 8 : timeOT - 1); // 1 h nghỉ trưa

                                                            item.WorkingStatuses.Add(new WorkingStatus()
                                                            {
                                                                DayCheck = dateCheck,
                                                                Value = kyhieuChamCongDB.NullString() == "" ? "NH" : kyhieuChamCongDB.NullString()
                                                            });

                                                            var clviec = item.lstNhanVienCaLamViec.FirstOrDefault(x =>
                                                             string.Compare(dateCheck, x.BatDau_TheoCa) >= 0 &&
                                                             string.Compare(dateCheck, x.KetThuc_TheoCa) <= 0 &&
                                                             x.MaCaLaviec == CommonConstants.CA_NGAY &&
                                                             (x.DM_NgayLViec == "NL" || x.DM_NgayLViec == "NLCC") && x.HeSo_OT != 100);

                                                            timeOT = HR_NHANVIEN_CHEDO_DBs.Exists(x => x.MaNhanVien == item.MaNV && x.CheDoDB.Contains("Block_10p")) ? SimpleValueOT(timeOT) : Block30mValueOT(timeOT);

                                                            item.OvertimeValues.Add(new OvertimeValue()
                                                            {
                                                                DayCheckOT = dateCheck,
                                                                DMOvertime = clviec.HeSo_OT.NullString(),
                                                                ValueOT = timeOT,
                                                                Registered = isRegistedOT
                                                            });

                                                            denghiOTModel.From = firstTime;
                                                            denghiOTModel.To = lastTime;
                                                            denghiOTModel.Duration = timeOT.IfNullIsZero();
                                                        }
                                                        else if (CheckNgayDB(dateCheck, CommonConstants.CA_NGAY) == 2) // ngay truoc le
                                                        {
                                                            string newBeginOT = "17:30:00";
                                                            if (string.Compare(lastTime, newBeginOT) > 0)
                                                            {
                                                                if (item.BoPhan != "SP")
                                                                {
                                                                    newBeginOT = "17:30:00";
                                                                }

                                                                double timeOT = (DateTime.ParseExact(dateCheck + " " + lastTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) - DateTime.ParseExact(dateCheck + " " + newBeginOT, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)).TotalHours;

                                                                if (timeOT > 1)
                                                                    timeOT -= 0.5;

                                                                var clviec = item.lstNhanVienCaLamViec.FirstOrDefault(x =>
                                                                             string.Compare(dateCheck, x.BatDau_TheoCa) >= 0 &&
                                                                             string.Compare(dateCheck, x.KetThuc_TheoCa) <= 0 &&
                                                                             x.MaCaLaviec == CommonConstants.CA_NGAY &&
                                                                             x.DM_NgayLViec == "TNL" && x.HeSo_OT != 100);

                                                                if (timeOT > 0)
                                                                {
                                                                    timeOT = HR_NHANVIEN_CHEDO_DBs.Exists(x => x.MaNhanVien == item.MaNV && x.CheDoDB.Contains("Block_10p")) ? SimpleValueOT(timeOT) : Block30mValueOT(timeOT);

                                                                    item.OvertimeValues.Add(new OvertimeValue()
                                                                    {
                                                                        DayCheckOT = dateCheck,
                                                                        DMOvertime = clviec.HeSo_OT.NullString(),
                                                                        ValueOT = timeOT,
                                                                        Registered = isRegistedOT
                                                                    });

                                                                    denghiOTModel.From = newBeginOT;
                                                                    denghiOTModel.To = lastTime;
                                                                    denghiOTModel.Duration = timeOT.IfNullIsZero();
                                                                }
                                                            }

                                                            item.WorkingStatuses.Add(new WorkingStatus()
                                                            {
                                                                DayCheck = dateCheck,
                                                                Value = kyhieuChamCongDB.NullString() == "" ? "DS" : kyhieuChamCongDB.NullString() // DS: Day Shift/ Ca ngày
                                                            });
                                                        }

                                                        deNghiLamThemGios1.Add(denghiOTModel);

                                                        // thêm OT đi sớm bật máy
                                                        if (lstOTDB.Count > 0)
                                                        {
                                                            foreach (var dB in lstOTDB)
                                                            {
                                                                if (dB.KyHieuChamCong == CommonConstants.OT_BAT_MAY)
                                                                {
                                                                    if (DateTime.ParseExact(_chamCongLog.FirstIn_Time.NullString(), "HH:mm:ss", CultureInfo.InvariantCulture) < DateTime.ParseExact("08:00:00", "HH:mm:ss", CultureInfo.InvariantCulture))
                                                                    {
                                                                        var dbOT = (DateTime.ParseExact("08:00:00", "HH:mm:ss", CultureInfo.InvariantCulture) - DateTime.ParseExact(_chamCongLog.FirstIn_Time.NullString(), "HH:mm:ss", CultureInfo.InvariantCulture)).TotalHours;

                                                                        dbOT = dbOT - 0.5;// 0.5h di ăn

                                                                        if (dbOT > 2) // tối đa OT 2h bật máy.
                                                                        {
                                                                            dbOT = 2;
                                                                        }

                                                                        //if (dbOT < 0.5)
                                                                        //{
                                                                        //    dbOT = 0;
                                                                        //}

                                                                        dbOT = HR_NHANVIEN_CHEDO_DBs.Exists(x => x.MaNhanVien == item.MaNV && x.CheDoDB.Contains("Block_10p")) ? SimpleValueOT(dbOT) : Block30mValueOT(dbOT);

                                                                        item.OvertimeValues.Add(new OvertimeValue()
                                                                        {
                                                                            DayCheckOT = dateCheck,
                                                                            DMOvertime = dB.HeSo.NullString(),
                                                                            ValueOT = dbOT,
                                                                            Registered = true
                                                                        });

                                                                        DeNghiLamThemGioModel deNghiLamThem = new DeNghiLamThemGioModel()
                                                                        {
                                                                            MaNV = item.MaNV,
                                                                            TenNV = item.TenNV,
                                                                            BoPhan = item.BoPhan,
                                                                            NgayDangKy = dateCheck,
                                                                            Note = "Đi sớm bật máy.",
                                                                            From = _chamCongLog.FirstIn_Time.NullString(),
                                                                            To = "08:00:00",
                                                                            Duration = dbOT.IfNullIsZero()
                                                                        };
                                                                        deNghiLamThemGios1.Add(deNghiLamThem);
                                                                    }
                                                                }
                                                            }
                                                        }

                                                        // Di muon ve som
                                                        double ELLC = 0;
                                                        if (DateTime.ParseExact(firstTime, "HH:mm:ss", CultureInfo.InvariantCulture) > DateTime.ParseExact("08:05:00", "HH:mm:ss", CultureInfo.InvariantCulture))
                                                        {
                                                            ELLC += (DateTime.ParseExact(firstTime, "HH:mm:ss", CultureInfo.InvariantCulture) - DateTime.ParseExact("08:05:00", "HH:mm:ss", CultureInfo.InvariantCulture)).TotalHours;
                                                        }

                                                        if ((item.BoPhan == CommonConstants.SUPPORT_DEPT || HR_NHANVIEN_CHEDO_DBs.Exists(x => x.MaNhanVien == item.MaNV && x.CheDoDB.Contains("vp_Block_10p"))) && DateTime.Parse(dateCheck).DayOfWeek == DayOfWeek.Saturday)
                                                        {
                                                            //#region tam thoi k dung do nghi T7
                                                            if (!CommonConstants.arrSatudayAbNormal.Contains(dateCheck))
                                                            {
                                                                if (CheckCheDoThaiSan("ConNho1H", dateCheck, item.MaNV))
                                                                {
                                                                    if (DateTime.ParseExact(lastTime, "HH:mm:ss", CultureInfo.InvariantCulture) < DateTime.ParseExact("12:00:00", "HH:mm:ss", CultureInfo.InvariantCulture))
                                                                    {
                                                                        ELLC += (DateTime.ParseExact("12:00:00", "HH:mm:ss", CultureInfo.InvariantCulture) - DateTime.ParseExact(lastTime, "HH:mm:ss", CultureInfo.InvariantCulture)).TotalHours;
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    if (DateTime.ParseExact(lastTime, "HH:mm:ss", CultureInfo.InvariantCulture) < DateTime.ParseExact("13:15:00", "HH:mm:ss", CultureInfo.InvariantCulture))
                                                                    {
                                                                        ELLC += ((DateTime.ParseExact("13:15:00", "HH:mm:ss", CultureInfo.InvariantCulture) - DateTime.ParseExact(lastTime, "HH:mm:ss", CultureInfo.InvariantCulture)).TotalHours - 0.6);
                                                                    }
                                                                }
                                                            }
                                                            else
                                                            {
                                                                // THÊM THẠM THOI
                                                                if (DateTime.ParseExact(lastTime, "HH:mm:ss", CultureInfo.InvariantCulture) < DateTime.ParseExact("17:00:00", "HH:mm:ss", CultureInfo.InvariantCulture))
                                                                {
                                                                    if ((DateTime.ParseExact("17:00:00", "HH:mm:ss", CultureInfo.InvariantCulture) - DateTime.ParseExact(lastTime, "HH:mm:ss", CultureInfo.InvariantCulture)).TotalHours > 4)
                                                                    {
                                                                        ELLC += ((DateTime.ParseExact("17:00:00", "HH:mm:ss", CultureInfo.InvariantCulture) - DateTime.ParseExact(lastTime, "HH:mm:ss", CultureInfo.InvariantCulture)).TotalHours - 1);
                                                                    }
                                                                    else
                                                                    {
                                                                        ELLC += ((DateTime.ParseExact("17:00:00", "HH:mm:ss", CultureInfo.InvariantCulture) - DateTime.ParseExact(lastTime, "HH:mm:ss", CultureInfo.InvariantCulture)).TotalHours);
                                                                    }
                                                                }
                                                            }

                                                            //#endregion
                                                        }
                                                        else
                                                        {
                                                            if (CheckCheDoThaiSan("ConNho1H", dateCheck, item.MaNV))
                                                            {
                                                                if (DateTime.ParseExact(lastTime, "HH:mm:ss", CultureInfo.InvariantCulture) < DateTime.ParseExact("16:00:00", "HH:mm:ss", CultureInfo.InvariantCulture))
                                                                {
                                                                    ELLC += (DateTime.ParseExact("16:00:00", "HH:mm:ss", CultureInfo.InvariantCulture) - DateTime.ParseExact(lastTime, "HH:mm:ss", CultureInfo.InvariantCulture)).TotalHours;
                                                                }
                                                            }
                                                            else
                                                            {
                                                                if (DateTime.ParseExact(lastTime, "HH:mm:ss", CultureInfo.InvariantCulture) < DateTime.ParseExact("17:00:00", "HH:mm:ss", CultureInfo.InvariantCulture))
                                                                {
                                                                    if ((DateTime.ParseExact("17:00:00", "HH:mm:ss", CultureInfo.InvariantCulture) - DateTime.ParseExact(lastTime, "HH:mm:ss", CultureInfo.InvariantCulture)).TotalHours > 4)
                                                                    {
                                                                        if (HR_NHANVIEN_CHEDO_DBs.Exists(x => x.MaNhanVien == item.MaNV && x.CheDoDB.Contains("vp_Block_10p")))
                                                                        {
                                                                            // 0.6h đi ăn
                                                                            ELLC += ((DateTime.ParseExact("17:00:00", "HH:mm:ss", CultureInfo.InvariantCulture) - DateTime.ParseExact(lastTime, "HH:mm:ss", CultureInfo.InvariantCulture)).TotalHours - 0.6);
                                                                        }
                                                                        else
                                                                        {
                                                                            ELLC += ((DateTime.ParseExact("17:00:00", "HH:mm:ss", CultureInfo.InvariantCulture) - DateTime.ParseExact(lastTime, "HH:mm:ss", CultureInfo.InvariantCulture)).TotalHours - 1);
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        ELLC += ((DateTime.ParseExact("17:00:00", "HH:mm:ss", CultureInfo.InvariantCulture) - DateTime.ParseExact(lastTime, "HH:mm:ss", CultureInfo.InvariantCulture)).TotalHours);
                                                                    }
                                                                }
                                                            }
                                                        }

                                                        if (ELLC < 0 || !string.IsNullOrEmpty(kyhieuChamCongDB.NullString()))
                                                        {
                                                            ELLC = ALLC_Cal(firstTime, lastTime, kyhieuChamCongDB.NullString());
                                                        };

                                                        if (HR_NHANVIEN_CHEDO_DBs.Exists(x => x.MaNhanVien == item.MaNV && x.CheDoDB.Contains("vp_Block_10p")) && string.Compare(dateCheck, "2022-10-24") >= 0)
                                                        {
                                                            if (BreakHaftDay(kyhieuChamCongDB.NullString()))
                                                            {
                                                                if (DateTime.ParseExact(lastTime, "HH:mm:ss", CultureInfo.InvariantCulture) >= DateTime.ParseExact("13:30:00", "HH:mm:ss", CultureInfo.InvariantCulture))
                                                                {
                                                                    ELLC += 0;
                                                                }
                                                                else
                                                                {
                                                                    ELLC += 0.7;
                                                                }
                                                            }
                                                            else if ((new int[] { 1, 3, 4 }).Contains(CheckNgayDB(dateCheck, CommonConstants.CA_NGAY)))
                                                            {
                                                                ELLC = 0;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            if (BreakHaftDay(kyhieuChamCongDB.NullString()) || (new int[] { 1, 3, 4 }).Contains(CheckNgayDB(dateCheck, CommonConstants.CA_NGAY)))
                                                            {
                                                                ELLC = 0;
                                                            }
                                                        }

                                                        if (dateCheck == "2023-01-19" && item.lstNhanVienCaLamViec.Any(x => string.Compare(x.BatDau_TheoCa, dateCheck) <= 0 && string.Compare(x.KetThuc_TheoCa, dateCheck) >= 0 && x.CaLV_DB != "HC"))
                                                        {
                                                            ELLC = 0;
                                                        }

                                                        var dmvs = DANGKY_DIMUON_VSOM_NHANVIENs.FirstOrDefault(x => x.MaNV == item.MaNV && x.NgayDangKy == dateCheck);
                                                        if (dmvs != null)
                                                        {
                                                            ELLC = dmvs.SoGioDangKy;
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
                                                        if (dateCheck == "2023-01-19" && item.lstNhanVienCaLamViec.Any(x => string.Compare(x.BatDau_TheoCa, dateCheck) <= 0 && string.Compare(x.KetThuc_TheoCa, dateCheck) >= 0 && x.CaLV_DB != "HC"))
                                                        {
                                                            if (DateTime.ParseExact(firstTime, "HH:mm:ss", CultureInfo.InvariantCulture) < DateTime.ParseExact("15:00:00", "HH:mm:ss", CultureInfo.InvariantCulture) &&
                                                                                                                       (DateTime.ParseExact(firstTime, "HH:mm:ss", CultureInfo.InvariantCulture) > DateTime.ParseExact("08:00:00", "HH:mm:ss", CultureInfo.InvariantCulture)))
                                                            {
                                                                firstTime = "15:00:00";
                                                            }
                                                        }
                                                        else
                                                        {
                                                            if (DateTime.ParseExact(firstTime, "HH:mm:ss", CultureInfo.InvariantCulture) < DateTime.ParseExact("20:00:00", "HH:mm:ss", CultureInfo.InvariantCulture) &&
                                                                                                                       (DateTime.ParseExact(firstTime, "HH:mm:ss", CultureInfo.InvariantCulture) > DateTime.ParseExact("17:00:00", "HH:mm:ss", CultureInfo.InvariantCulture)))
                                                            {
                                                                firstTime = "20:00:00";
                                                            }

                                                            if (DateTime.ParseExact(lastTime, "HH:mm:ss", CultureInfo.InvariantCulture) > DateTime.ParseExact("10:00:00", "HH:mm:ss", CultureInfo.InvariantCulture) &&
                                                                DateTime.ParseExact(lastTime, "HH:mm:ss", CultureInfo.InvariantCulture) < DateTime.ParseExact("17:00:00", "HH:mm:ss", CultureInfo.InvariantCulture)) // change 8h-> 10h
                                                            {
                                                                lastTime = "10:00:00";
                                                            }
                                                        }

                                                        if (CheckNgayDB(dateCheck, CommonConstants.CA_DEM) == 0) // ngay thuong
                                                        {
                                                            if (dateCheck == "2023-01-19" && item.lstNhanVienCaLamViec.Any(x => string.Compare(x.BatDau_TheoCa, dateCheck) <= 0 && string.Compare(x.KetThuc_TheoCa, dateCheck) >= 0 && x.CaLV_DB != "HC"))
                                                            {
                                                                if (kyhieuChamCongDB.NullString() == "" && CheckCheDoThaiSan("ConNho1H", dateCheck, item.MaNV) && string.Compare(lastTime, "18:00:00") > 0 && string.Compare(lastTime, "18:59:59") < 0)
                                                                {
                                                                    kyhieuChamCongDB = "AL/NS";
                                                                }

                                                                item.WorkingStatuses.Add(new WorkingStatus()
                                                                {
                                                                    DayCheck = dateCheck,
                                                                    Value = kyhieuChamCongDB.NullString() == "" ? "NS" : kyhieuChamCongDB.NullString() // NS: Night Shift/ Ca đêm
                                                                });
                                                            }
                                                            else
                                                            {
                                                                string newBeginOT = "05:30:00";
                                                                if (CheckCheDoThaiSan("ConNho1H", dateCheck, item.MaNV))
                                                                {
                                                                    newBeginOT = "04:30:00";
                                                                }

                                                                if ((string.Compare(lastTime, newBeginOT) > 0) && (string.Compare(lastTime, "17:00:00") < 0))
                                                                {
                                                                    double timeOT = (DateTime.ParseExact(dateCheck + " " + lastTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) - DateTime.ParseExact(dateCheck + " " + newBeginOT, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)).TotalHours;

                                                                    var clviecs = item.lstNhanVienCaLamViec.FindAll(x =>
                                                                                  string.Compare(dateCheck, x.BatDau_TheoCa) >= 0 &&
                                                                                  string.Compare(dateCheck, x.KetThuc_TheoCa) <= 0 &&
                                                                                  x.MaCaLaviec == CommonConstants.CA_DEM &&
                                                                                  (x.DM_NgayLViec == "NT") && x.HeSo_OT != 100);

                                                                    string hsOT1 = clviecs.FirstOrDefault(x => x.Time_KetThuc == "06:00:00")?.HeSo_OT.IfNullIsZero();
                                                                    string hsOT2 = clviecs.FirstOrDefault(x => x.Time_KetThuc == "08:00:00")?.HeSo_OT.IfNullIsZero();

                                                                    timeOT = HR_NHANVIEN_CHEDO_DBs.Exists(x => x.MaNhanVien == item.MaNV && x.CheDoDB.Contains("Block_10p")) ? SimpleValueOT(timeOT) : Block30mValueOT(timeOT);

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
                                                                            ValueOT = timeOT - 0.5,/*> 2 ? 2 : (timeOT - 0.5 < 0.5 ? 0 : timeOT - 0.5)*/ // tru di 0.5h cua 5-6h
                                                                            Registered = isRegistedOT
                                                                        });
                                                                    }

                                                                    // set time de nghi làm thêm giờ
                                                                    denghiOTModel.From = newBeginOT;
                                                                    denghiOTModel.To = lastTime;
                                                                    denghiOTModel.Duration = timeOT.IfNullIsZero(); //timeOT > 2.5 ? "2.5" : timeOT.IfNullIsZero();
                                                                }

                                                                if (kyhieuChamCongDB.NullString() == "" && CheckCheDoThaiSan("ConNho1H", dateCheck, item.MaNV) && string.Compare(lastTime, "23:00:00") > 0 && string.Compare(lastTime, "23:59:59") < 0)
                                                                {
                                                                    kyhieuChamCongDB = "AL/NS";
                                                                }

                                                                item.WorkingStatuses.Add(new WorkingStatus()
                                                                {
                                                                    DayCheck = dateCheck,
                                                                    Value = kyhieuChamCongDB.NullString() == "" ? "NS" : kyhieuChamCongDB.NullString() // NS: Night Shift/ Ca đêm
                                                                });
                                                            }


                                                        }
                                                        else if (CheckNgayDB(dateCheck, CommonConstants.CA_DEM) == 5) // Ngay ki niem cty
                                                        {
                                                            item.OvertimeValues.AddRange(GetOvertimeInNight(firstTime, lastTime, dateCheck, "NT9", isRegistedOT, item.MaNV, ref denghiOTModel));
                                                            item.WorkingStatuses.Add(new WorkingStatus()
                                                            {
                                                                DayCheck = dateCheck,
                                                                Value = kyhieuChamCongDB.NullString() == "" ? "BM" : kyhieuChamCongDB.NullString() // BM: Làm ca đêm ngày kỷ niệm chính thức
                                                            });
                                                        }
                                                        else if (CheckNgayDB(dateCheck, CommonConstants.CA_DEM) == 3 || CheckNgayDB(dateCheck, CommonConstants.CA_DEM) == 4) // ngay chu nhat + ngay nghi bu
                                                        {
                                                            item.OvertimeValues.AddRange(GetOvertimeInNight(firstTime, lastTime, dateCheck, "CN", isRegistedOT, item.MaNV, ref denghiOTModel));
                                                            item.WorkingStatuses.Add(new WorkingStatus()
                                                            {
                                                                DayCheck = dateCheck,
                                                                Value = kyhieuChamCongDB.NullString() == "" ? "D" : ResetULIL(kyhieuChamCongDB.NullString(), dateCheck) // TVD: Thử việc làm thêm ca đêm chủ nhật
                                                            });
                                                        }
                                                        else if (CheckNgayDB(dateCheck, CommonConstants.CA_DEM) == 1) // ngay le
                                                        {
                                                            var ngayle = NGAY_LE_NAMs.FirstOrDefault(x => x.Id == dateCheck);
                                                            if (ngayle.IslastHoliday == CommonConstants.N)
                                                            {
                                                                item.OvertimeValues.AddRange(GetOvertimeInNight(firstTime, lastTime, dateCheck, "NL", isRegistedOT, item.MaNV, ref denghiOTModel));
                                                            }
                                                            else // ngay le cuoi cung
                                                            {
                                                                item.OvertimeValues.AddRange(GetOvertimeInNight(firstTime, lastTime, dateCheck, "NLCC", isRegistedOT, item.MaNV, ref denghiOTModel));
                                                            }

                                                            item.WorkingStatuses.Add(new WorkingStatus()
                                                            {
                                                                DayCheck = dateCheck,
                                                                Value = kyhieuChamCongDB.NullString() == "" ? "NHD" : kyhieuChamCongDB.NullString() // NHD: làm ca đêm ngày lễ
                                                            });
                                                        }
                                                        else if (CheckNgayDB(dateCheck, CommonConstants.CA_DEM) == 2) // ngay truoc le
                                                        {
                                                            item.OvertimeValues.AddRange(GetOvertimeInNight(firstTime, lastTime, dateCheck, "TNL", isRegistedOT, item.MaNV, ref denghiOTModel));
                                                            item.WorkingStatuses.Add(new WorkingStatus()
                                                            {
                                                                DayCheck = dateCheck,
                                                                Value = kyhieuChamCongDB.NullString() == "" ? "BH" : kyhieuChamCongDB.NullString() // BH: làm ca đêm trước ngày lễ( chính thức)
                                                            });
                                                        }

                                                        deNghiLamThemGios1.Add(denghiOTModel);

                                                        // thêm OT đi sớm bật máy
                                                        if (lstOTDB.Count > 0)
                                                        {
                                                            foreach (var dB in lstOTDB)
                                                            {
                                                                if (dB.KyHieuChamCong == CommonConstants.OT_BAT_MAY)
                                                                {
                                                                    if (DateTime.ParseExact(_chamCongLog.FirstIn_Time.NullString(), "HH:mm:ss", CultureInfo.InvariantCulture) < DateTime.ParseExact("20:00:00", "HH:mm:ss", CultureInfo.InvariantCulture))
                                                                    {
                                                                        var dbOT = (DateTime.ParseExact("20:00:00", "HH:mm:ss", CultureInfo.InvariantCulture) - DateTime.ParseExact(_chamCongLog.FirstIn_Time.NullString(), "HH:mm:ss", CultureInfo.InvariantCulture)).TotalHours;

                                                                        dbOT = dbOT - 0.5;// 0.5h di ăn

                                                                        if (dbOT > 2) // tối đa OT 2h bật máy.
                                                                        {
                                                                            dbOT = 2;
                                                                        }

                                                                        // if (dbOT < 0.5) dbOT = 0;

                                                                        dbOT = HR_NHANVIEN_CHEDO_DBs.Exists(x => x.MaNhanVien == item.MaNV && x.CheDoDB.Contains("Block_10p")) ? SimpleValueOT(dbOT) : Block30mValueOT(dbOT);

                                                                        item.OvertimeValues.Add(new OvertimeValue()
                                                                        {
                                                                            DayCheckOT = dateCheck,
                                                                            DMOvertime = dB.HeSo.NullString(),
                                                                            ValueOT = dbOT,
                                                                            Registered = true
                                                                        });

                                                                        DeNghiLamThemGioModel deNghiLamThem = new DeNghiLamThemGioModel()
                                                                        {
                                                                            MaNV = item.MaNV,
                                                                            TenNV = item.TenNV,
                                                                            BoPhan = item.BoPhan,
                                                                            NgayDangKy = dateCheck,
                                                                            Note = "Đi sớm bật máy.",
                                                                            From = _chamCongLog.FirstIn_Time.NullString(),
                                                                            To = "20:00:00",
                                                                            Duration = dbOT.IfNullIsZero()
                                                                        };
                                                                        deNghiLamThemGios1.Add(deNghiLamThem);
                                                                    }
                                                                }
                                                            }
                                                        }


                                                        // Di muon ve som
                                                        double ELLC = 0;
                                                        if (DateTime.ParseExact(firstTime, "HH:mm:ss", CultureInfo.InvariantCulture) > DateTime.ParseExact("20:05:00", "HH:mm:ss", CultureInfo.InvariantCulture))
                                                        {
                                                            ELLC += (DateTime.ParseExact(firstTime, "HH:mm:ss", CultureInfo.InvariantCulture) - DateTime.ParseExact("20:05:00", "HH:mm:ss", CultureInfo.InvariantCulture)).TotalHours;
                                                        }

                                                        if (CheckCheDoThaiSan("ConNho1H", dateCheck, item.MaNV))
                                                        {
                                                            if (DateTime.ParseExact(lastTime, "HH:mm:ss", CultureInfo.InvariantCulture) < DateTime.ParseExact("04:00:00", "HH:mm:ss", CultureInfo.InvariantCulture))
                                                            {
                                                                ELLC += (DateTime.ParseExact("04:00:00", "HH:mm:ss", CultureInfo.InvariantCulture) - DateTime.ParseExact(lastTime, "HH:mm:ss", CultureInfo.InvariantCulture)).TotalHours;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            if (DateTime.ParseExact(lastTime, "HH:mm:ss", CultureInfo.InvariantCulture) < DateTime.ParseExact("05:00:00", "HH:mm:ss", CultureInfo.InvariantCulture))
                                                            {
                                                                ELLC += (DateTime.ParseExact("05:00:00", "HH:mm:ss", CultureInfo.InvariantCulture) - DateTime.ParseExact(lastTime, "HH:mm:ss", CultureInfo.InvariantCulture)).TotalHours;
                                                            }
                                                            else if (DateTime.ParseExact(lastTime, "HH:mm:ss", CultureInfo.InvariantCulture) >= DateTime.ParseExact("20:00:00", "HH:mm:ss", CultureInfo.InvariantCulture) &&
                                                               DateTime.ParseExact(lastTime, "HH:mm:ss", CultureInfo.InvariantCulture) < DateTime.ParseExact("23:59:59", "HH:mm:ss", CultureInfo.InvariantCulture))
                                                            {
                                                                ELLC += 4 + (DateTime.ParseExact("23:59:59", "HH:mm:ss", CultureInfo.InvariantCulture) - DateTime.ParseExact(lastTime, "HH:mm:ss", CultureInfo.InvariantCulture)).TotalHours;
                                                            }
                                                        }

                                                        if (ELLC < 0 || !string.IsNullOrEmpty(kyhieuChamCongDB.NullString()))
                                                        {
                                                            ELLC = ALLC_Cal(firstTime, lastTime, kyhieuChamCongDB.NullString());
                                                        };

                                                        if (HR_NHANVIEN_CHEDO_DBs.Exists(x => x.MaNhanVien == item.MaNV && x.CheDoDB.Contains("vp_Block_10p")) && string.Compare(dateCheck, "2022-10-24") >= 0)
                                                        {
                                                            if (BreakHaftDay(kyhieuChamCongDB.NullString()))
                                                            {
                                                                if (DateTime.ParseExact(lastTime, "HH:mm:ss", CultureInfo.InvariantCulture) >= DateTime.ParseExact("13:30:00", "HH:mm:ss", CultureInfo.InvariantCulture))
                                                                {
                                                                    ELLC += 0;
                                                                }
                                                                else
                                                                {
                                                                    ELLC += 0.7;
                                                                }
                                                            }
                                                            else if ((new int[] { 1, 3, 4 }).Contains(CheckNgayDB(dateCheck, CommonConstants.CA_NGAY)))
                                                            {
                                                                ELLC = 0;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            //if (BreakHaftDay(kyhieuChamCongDB.NullString()))
                                                            //{
                                                            //    if (DateTime.ParseExact(firstTime, "HH:mm:ss", CultureInfo.InvariantCulture) > DateTime.ParseExact("01:00:00", "HH:mm:ss", CultureInfo.InvariantCulture))
                                                            //    {
                                                            //        ELLC += 4 + (DateTime.ParseExact(firstTime, "HH:mm:ss", CultureInfo.InvariantCulture) - DateTime.ParseExact("01:00:00", "HH:mm:ss", CultureInfo.InvariantCulture)).TotalHours;
                                                            //    }
                                                            //    else
                                                            //    {
                                                            //        ELLC = 0;
                                                            //    }

                                                            //    if (DateTime.ParseExact(lastTime, "HH:mm:ss", CultureInfo.InvariantCulture) < DateTime.ParseExact("05:00:00", "HH:mm:ss", CultureInfo.InvariantCulture))
                                                            //    {
                                                            //        ELLC += (DateTime.ParseExact("05:00:00", "HH:mm:ss", CultureInfo.InvariantCulture) - DateTime.ParseExact(lastTime, "HH:mm:ss", CultureInfo.InvariantCulture)).TotalHours;
                                                            //    }
                                                            //}

                                                            if ((new int[] { 1, 3, 4 }).Contains(CheckNgayDB(dateCheck, CommonConstants.CA_DEM)))
                                                            {
                                                                ELLC = 0;
                                                            }
                                                        }

                                                        if (dateCheck == "2023-01-19" && item.lstNhanVienCaLamViec.Any(x => string.Compare(x.BatDau_TheoCa, dateCheck) <= 0 && string.Compare(x.KetThuc_TheoCa, dateCheck) >= 0 && x.CaLV_DB != "HC"))
                                                        {
                                                            ELLC = 0;
                                                        }

                                                        var dmvs = DANGKY_DIMUON_VSOM_NHANVIENs.FirstOrDefault(x => x.MaNV == item.MaNV && x.NgayDangKy == dateCheck);
                                                        if (dmvs != null)
                                                        {
                                                            ELLC = dmvs.SoGioDangKy;
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
                                                    int checkDate = CheckNgayDB(dateCheck, _caLamViec.MaCaLaviec);
                                                    if (checkDate == 1 || checkDate == 4 || checkDate == 5) // ngay le
                                                    {
                                                        if (checkDate == 1)
                                                        {
                                                            item.WorkingStatuses.Add(new WorkingStatus()
                                                            {
                                                                DayCheck = dateCheck,
                                                                Value = kyhieuChamCongDB.NullString() == "" ? "NH" : kyhieuChamCongDB.NullString() // NH: NationALHoliday/ Nghỉ lễ
                                                            });
                                                        }
                                                        else if (checkDate == 4 || checkDate == 5)
                                                        {
                                                            item.WorkingStatuses.Add(new WorkingStatus()
                                                            {
                                                                DayCheck = dateCheck,
                                                                Value = kyhieuChamCongDB.NullString() == "" ? "-" : kyhieuChamCongDB.NullString()
                                                            });
                                                        }
                                                    }
                                                    else
                                                    {
                                                        item.WorkingStatuses.Add(new WorkingStatus()
                                                        {
                                                            DayCheck = dateCheck,
                                                            Value = kyhieuChamCongDB.NullString() == "" ? "-" : ResetULIL(kyhieuChamCongDB.NullString(), dateCheck)
                                                        });
                                                    }

                                                    double el = 0;
                                                    if (HR_NHANVIEN_CHEDO_DBs.Exists(x => x.MaNhanVien == item.MaNV && x.CheDoDB.Contains("vp_Block_10p")) && string.Compare(dateCheck, "2022-10-24") >= 0)
                                                    {
                                                        if (BreakHaftDay(kyhieuChamCongDB.NullString()))
                                                        {
                                                            el += 0.7;
                                                        }
                                                        else if ((new int[] { 1, 3, 4 }).Contains(CheckNgayDB(dateCheck, CommonConstants.CA_NGAY)))
                                                        {
                                                            el = 0;
                                                        }
                                                    }

                                                    var dmvs = DANGKY_DIMUON_VSOM_NHANVIENs.FirstOrDefault(x => x.MaNV == item.MaNV && x.NgayDangKy == dateCheck);
                                                    if (dmvs != null)
                                                    {
                                                        el = dmvs.SoGioDangKy;
                                                    }

                                                    item.EL_LC_Statuses.Add(new EL_LC_Status()
                                                    {
                                                        DayCheck_EL = dateCheck,
                                                        Value_EL = el
                                                    });
                                                }
                                                else if (_chamCongLog.FirstIn.NullString() == "" && _chamCongLog.LastOut.NullString() != "")
                                                {
                                                    int checkDate = CheckNgayDB(dateCheck, _caLamViec.MaCaLaviec);
                                                    if (checkDate == 1 || checkDate == 4 || checkDate == 5) // ngay le
                                                    {
                                                        if (checkDate == 1)
                                                        {
                                                            item.WorkingStatuses.Add(new WorkingStatus()
                                                            {
                                                                DayCheck = dateCheck,
                                                                Value = kyhieuChamCongDB.NullString() == "" ? "NH" : kyhieuChamCongDB.NullString() // NH: NationALHoliday/ Nghỉ lễ
                                                            });
                                                        }
                                                        else if (checkDate == 4 || checkDate == 5)
                                                        {
                                                            item.WorkingStatuses.Add(new WorkingStatus()
                                                            {
                                                                DayCheck = dateCheck,
                                                                Value = kyhieuChamCongDB.NullString() == "" ? "-" : kyhieuChamCongDB.NullString()
                                                            });
                                                        }
                                                    }
                                                    else
                                                    {
                                                        item.WorkingStatuses.Add(new WorkingStatus()
                                                        {
                                                            DayCheck = dateCheck,
                                                            Value = kyhieuChamCongDB.NullString() == "" ? "-" : ResetULIL(kyhieuChamCongDB.NullString(), dateCheck)
                                                        });
                                                    }

                                                    double el = 0;
                                                    if (HR_NHANVIEN_CHEDO_DBs.Exists(x => x.MaNhanVien == item.MaNV && x.CheDoDB.Contains("vp_Block_10p")) && string.Compare(dateCheck, "2022-10-24") >= 0)
                                                    {
                                                        if (BreakHaftDay(kyhieuChamCongDB.NullString()))
                                                        {
                                                            if (DateTime.ParseExact(lastTime, "HH:mm:ss", CultureInfo.InvariantCulture) >= DateTime.ParseExact("13:30:00", "HH:mm:ss", CultureInfo.InvariantCulture))
                                                            {
                                                                el += 0;
                                                            }
                                                            else
                                                            {
                                                                el += 0.7;
                                                            }
                                                        }
                                                        else if ((new int[] { 1, 3, 4 }).Contains(CheckNgayDB(dateCheck, CommonConstants.CA_NGAY)))
                                                        {
                                                            el = 0;
                                                        }
                                                    }

                                                    var dmvs = DANGKY_DIMUON_VSOM_NHANVIENs.FirstOrDefault(x => x.MaNV == item.MaNV && x.NgayDangKy == dateCheck);
                                                    if (dmvs != null)
                                                    {
                                                        el = dmvs.SoGioDangKy;
                                                    }

                                                    item.EL_LC_Statuses.Add(new EL_LC_Status()
                                                    {
                                                        DayCheck_EL = dateCheck,
                                                        Value_EL = el
                                                    });
                                                }
                                            }
                                            else
                                            {
                                                if (CheckCheDoThaiSan("ThaiSan", dateCheck, item.MaNV) && !IsChuNhat(dateCheck))
                                                {
                                                    item.WorkingStatuses.Add(new WorkingStatus()
                                                    {
                                                        DayCheck = dateCheck,
                                                        Value = "IL"
                                                    });
                                                }
                                                else if (CheckCheDoThaiSan("ThaiSan", dateCheck, item.MaNV) && IsChuNhat(dateCheck))
                                                {
                                                    item.WorkingStatuses.Add(new WorkingStatus()
                                                    {
                                                        DayCheck = dateCheck,
                                                        Value = "-"
                                                    });
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            return new ResultItemModelBC(chamCongDataViewModels, deNghiLamThemGios1);
                        });
                    }

                    Task.WaitAll(tasks);
                    List<ChamCongDataViewModel> lstResult1 = new List<ChamCongDataViewModel>();
                    foreach (var t in tasks)
                    {
                        lstResult1.AddRange(t.Result.chamCongDataViewModels);
                        deNghiLamThemGios.AddRange(t.Result.deNghiLamThemGioModels);
                    }

                    lstResult = lstResult1;
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
                if (i < 10)
                {
                    lstDayOfMonth.Add(day + "-0" + i);
                }
                else
                {
                    lstDayOfMonth.Add(day + "-" + i);
                }
            }

            string kyHchamcong = "";
            foreach (var item in lstResult)
            {
                foreach (var dayOfM in lstDayOfMonth)
                {
                    if (!item.WorkingStatuses.Any(x => x.DayCheck == dayOfM))
                    {
                        kyHchamcong = item.lstChamCongDB.FirstOrDefault(x => string.Compare(dayOfM, x.NgayBatDau) >= 0 && string.Compare(dayOfM, x.NgayKetThuc) <= 0 && x.PhanLoaiDM != CommonConstants.DM_OT && x.PhanLoaiDM != CommonConstants.DM_ELLC)?.KyHieuChamCong;
                        item.WorkingStatuses.Add(new WorkingStatus()
                        {
                            DayCheck = dayOfM,
                            Value = kyHchamcong.NullString() == "" ? "-" : ResetULIL(kyHchamcong.NullString(), dayOfM)
                        });

                        item.EL_LC_Statuses.Add(new EL_LC_Status()
                        {
                            DayCheck_EL = dayOfM,
                            Value_EL = 0
                        });
                    }
                }
            }

            // Group OT
            List<OvertimeValue> overtimes = new List<OvertimeValue>();
            OvertimeValue oval;
            foreach (var item in lstResult)
            {
                overtimes = new List<OvertimeValue>();

                if (item.OvertimeValues.Count > 0)
                {
                    // truong hop OT co ma trong overtime value k co he so do
                    var regisOTsFirstCheck = _overtimeResponsitory.FindAll(x => x.MaNV == item.MaNV && x.ApproveLV3 == CommonConstants.Approved && beginMonth.CompareTo(x.NgayOT) <= 0 && endMonth.CompareTo(x.NgayOT) >= 0).ToList();
                    if (regisOTsFirstCheck.Count > 0)
                    {
                        foreach (var checkItem in regisOTsFirstCheck)
                        {
                            if (!item.OvertimeValues.Any(x => x.DMOvertime == checkItem.HeSoOT && x.DayCheckOT == checkItem.NgayOT))
                            {
                                item.OvertimeValues.Add(new OvertimeValue()
                                {
                                    DayCheckOT = checkItem.NgayOT,
                                    DMOvertime = checkItem.HeSoOT,
                                    ValueOT = 0,
                                    Registered = true
                                });
                            }
                        }
                    }

                    foreach (var ot in item.OvertimeValues)
                    {
                        if (overtimes.Any(x => x.DMOvertime == ot.DMOvertime && x.DayCheckOT == ot.DayCheckOT)) // DMOvertime = HeSo_OT
                        {
                            oval = overtimes.Find(x => x.DMOvertime == ot.DMOvertime && x.DayCheckOT == ot.DayCheckOT);
                            oval.ValueOT += ot.ValueOT;
                        }
                        else
                        {
                            // add them gio OT dac biet neu co
                            var regisOTs = _overtimeResponsitory.FindAll(x => x.MaNV == item.MaNV && x.NgayOT == ot.DayCheckOT && x.HeSoOT == ot.DMOvertime && x.ApproveLV3 == CommonConstants.Approved).ToList();
                            if (regisOTs.Count > 0)
                            {
                                foreach (var ad in regisOTs)
                                {
                                    ot.ValueOT += ad.SoGioOT;
                                }
                            }
                            overtimes.Add(ot);
                        }
                    }
                }
                else
                {
                    var regisOTs = _overtimeResponsitory.FindAll(x => x.MaNV == item.MaNV && x.ApproveLV3 == CommonConstants.Approved && beginMonth.CompareTo(x.NgayOT) <= 0 && endMonth.CompareTo(x.NgayOT) >= 0).ToList();
                    if (regisOTs.Count > 0)
                    {
                        foreach (var ad in regisOTs)
                        {
                            if (overtimes.Any(x => x.DMOvertime == ad.HeSoOT && x.DayCheckOT == ad.NgayOT))
                            {
                                oval = overtimes.Find(x => x.DMOvertime == ad.HeSoOT && x.DayCheckOT == ad.NgayOT);
                                oval.ValueOT += ad.SoGioOT;
                            }
                            else
                            {
                                overtimes.Add(new OvertimeValue()
                                {
                                    DayCheckOT = ad.NgayOT,
                                    DMOvertime = ad.HeSoOT,
                                    ValueOT = ad.SoGioOT,
                                    Registered = true
                                });
                            }
                        }
                    }
                }

                if (overtimes.Count > 0)
                {
                    item.OvertimeValues = overtimes;
                }
                else
                {
                    item.OvertimeValues = new List<OvertimeValue>();
                }
            }

            DeNghiLamThemGioModel dnghiModel;
            var regisOTBX = _overtimeResponsitory.FindAll(x => x.ApproveLV3 == CommonConstants.Approved && beginMonth.CompareTo(x.NgayOT) <= 0 && endMonth.CompareTo(x.NgayOT) >= 0, y => y.HR_NHANVIEN).ToList();
            foreach (var bx in regisOTBX)
            {
                dnghiModel = new DeNghiLamThemGioModel()
                {
                    MaNV = bx.MaNV,
                    BoPhan = bx.HR_NHANVIEN.MaBoPhan,
                    Duration = bx.SoGioOT.NullString(),
                    NgayDangKy = bx.NgayOT,
                    TenNV = bx.HR_NHANVIEN.TenNV,
                    Note = bx.NoiDung
                };

                deNghiLamThemGios.Add(dnghiModel);
            }

            foreach (var item in deNghiLamThemGios.ToList())
            {
                if (string.IsNullOrEmpty(item.Duration) || item.Duration == "0")
                {
                    deNghiLamThemGios.Remove(item);
                }
            }

            foreach (var item in lstResult)
            {
                item.WorkingStatuses.Sort((x, y) => x.DayCheck.CompareTo(y.DayCheck));
                item.OvertimeValues.Sort((x, y) => x.DayCheckOT.CompareTo(y.DayCheckOT));
                item.EL_LC_Statuses.Sort((x, y) => x.DayCheck_EL.CompareTo(y.DayCheck_EL));
            }

            return lstResult.OrderBy(x => x.BoPhan).ToList();
        }

        /// <summary>
        /// Khong tính đi muộn về sớm khi nghỉ nửa ngày.
        /// </summary>
        /// <param name="kyhieuchamcong"></param>
        /// <returns></returns>
        private bool BreakHaftDay(string kyhieuchamcong)
        {
            return (new string[] { "B", "NB", "L70", "P/DS", "P/NS", "PH/F", "PH/L", "AL/DS", "AL/NS", "UL/DS", "UL/NS", "AL/BF", "AL/BL", "UL/BF", "UL/BL", "AL", "UL" }).Contains(kyhieuchamcong);
        }

        /// <summary>
        /// Làm nửa ngày nếu về sơm thì k dc tính đủ công.
        /// </summary>
        /// <param name="firstTime"></param>
        /// <param name="lastTime"></param>
        /// <param name="kytuchamcong"></param>
        /// <returns></returns>
        private double ALLC_Cal(string firstTime, string lastTime, string kytuchamcong)
        {
            List<string> ktu = new List<string>()
            {
                "P/DS","P/NS","PH/F","PH/L","AL/DS","AL/NS","UL/DS","UL/NS","AL/BF","AL/BL","UL/BF","UL/BL"
            };
            double rs = 0;

            if (ktu.Contains(kytuchamcong))
            {
                if (DateTime.ParseExact(firstTime, "HH:mm:ss", CultureInfo.InvariantCulture) <= DateTime.ParseExact(lastTime, "HH:mm:ss", CultureInfo.InvariantCulture))
                {
                    rs = (DateTime.ParseExact(lastTime, "HH:mm:ss", CultureInfo.InvariantCulture) - DateTime.ParseExact(firstTime, "HH:mm:ss", CultureInfo.InvariantCulture)).TotalHours;
                }
                else
                {
                    rs = 24 + (DateTime.ParseExact(lastTime, "HH:mm:ss", CultureInfo.InvariantCulture) - DateTime.ParseExact(firstTime, "HH:mm:ss", CultureInfo.InvariantCulture)).TotalHours;
                }
            }

            if (rs > 0 && rs < 4)
            {
                rs = 4 - rs;
            }
            else
            {
                rs = 0;
            }

            return SimpleValueOT(rs);
        }

        private List<OvertimeValue> GetOvertimeInNight(string firstTime, string lastTime, string dateCheck, string ngayLviec, bool isRegistedOT, string maNV, ref DeNghiLamThemGioModel themGioModel)
        {
            // 20:00 -> 22:00
            // 22:00 -> 23:59
            // 00:00 -> 06:00
            // 06:00 -> 08:00

            var clviecs = CA_LVIECs.FindAll(x => x.DM_NgayLViec == ngayLviec && x.Danhmuc_CaLviec == CommonConstants.CA_DEM && x.HeSo_OT != 100);

            List<OvertimeValue> lstResult = new List<OvertimeValue>();

            if (DateTime.ParseExact(firstTime, "HH:mm:ss", CultureInfo.InvariantCulture) >= DateTime.ParseExact("20:00:00", "HH:mm:ss", CultureInfo.InvariantCulture) &&
                DateTime.ParseExact(lastTime, "HH:mm:ss", CultureInfo.InvariantCulture) >= DateTime.ParseExact("20:00:00", "HH:mm:ss", CultureInfo.InvariantCulture) &&
                DateTime.ParseExact(lastTime, "HH:mm:ss", CultureInfo.InvariantCulture) <= DateTime.ParseExact("22:00:00", "HH:mm:ss", CultureInfo.InvariantCulture))
            {
                string hsOT = clviecs.FirstOrDefault(x => x.Time_KetThuc == "22:00:00")?.HeSo_OT.IfNullIsZero();

                double timeOT = (DateTime.ParseExact(dateCheck + " " + lastTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) - DateTime.ParseExact(dateCheck + " " + firstTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)).TotalHours;

                if (ngayLviec == "CN") // ngay chu nhat
                {
                    if (timeOT > 0)
                    {
                        lstResult.Add(new OvertimeValue()
                        {
                            DayCheckOT = dateCheck,
                            DMOvertime = hsOT,
                            ValueOT = timeOT,
                            Registered = isRegistedOT,
                            fromTime = firstTime,
                            toTime = lastTime,
                            orderTime = 1
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
                            Registered = isRegistedOT,
                            fromTime = firstTime,
                            toTime = lastTime,
                            orderTime = 1
                        });
                    }
                }
            }
            else if (
                DateTime.ParseExact(firstTime, "HH:mm:ss", CultureInfo.InvariantCulture) >= DateTime.ParseExact("22:00:00", "HH:mm:ss", CultureInfo.InvariantCulture) &&
                DateTime.ParseExact(lastTime, "HH:mm:ss", CultureInfo.InvariantCulture) >= DateTime.ParseExact("22:00:00", "HH:mm:ss", CultureInfo.InvariantCulture) &&
                DateTime.ParseExact(lastTime, "HH:mm:ss", CultureInfo.InvariantCulture) <= DateTime.ParseExact("23:59:59", "HH:mm:ss", CultureInfo.InvariantCulture))
            {
                string hsOT = clviecs.FirstOrDefault(x => x.Time_BatDau == "22:00:00")?.HeSo_OT.IfNullIsZero();

                double timeOT = (DateTime.ParseExact(dateCheck + " " + lastTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) - DateTime.ParseExact(dateCheck + " " + firstTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)).TotalHours;

                if (timeOT > 0)
                {
                    if (ngayLviec == "NLCC" || ngayLviec == "NL") // ngay chu nhat
                    {
                        lstResult.Add(new OvertimeValue()
                        {
                            DayCheckOT = dateCheck,
                            DMOvertime = hsOT,
                            ValueOT = timeOT,
                            Registered = isRegistedOT,
                            fromTime = firstTime,
                            toTime = lastTime,
                            orderTime = 1
                        });
                    }
                    else if (ngayLviec == "CN")
                    {
                        lstResult.Add(new OvertimeValue()
                        {
                            DayCheckOT = dateCheck,
                            DMOvertime = hsOT,
                            ValueOT = timeOT,
                            Registered = isRegistedOT,
                            fromTime = firstTime,
                            toTime = lastTime,
                            orderTime = 1
                        });
                    }
                }
            }
            else if (DateTime.ParseExact(firstTime, "HH:mm:ss", CultureInfo.InvariantCulture) >= DateTime.ParseExact("00:00:00", "HH:mm:ss", CultureInfo.InvariantCulture) && // 00-> 06
                    DateTime.ParseExact(firstTime, "HH:mm:ss", CultureInfo.InvariantCulture) <= DateTime.ParseExact("06:00:00", "HH:mm:ss", CultureInfo.InvariantCulture) &&
                     DateTime.ParseExact(lastTime, "HH:mm:ss", CultureInfo.InvariantCulture) > DateTime.ParseExact("00:00:00", "HH:mm:ss", CultureInfo.InvariantCulture) &&
                     DateTime.ParseExact(lastTime, "HH:mm:ss", CultureInfo.InvariantCulture) <= DateTime.ParseExact("06:00:00", "HH:mm:ss", CultureInfo.InvariantCulture))
            {
                string hsOT = clviecs.FirstOrDefault(x => x.Time_KetThuc.CompareTo("06:00:00") <= 0)?.HeSo_OT.IfNullIsZero();

                double timeOT = (DateTime.ParseExact(dateCheck + " " + lastTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) - DateTime.ParseExact(dateCheck + " " + firstTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)).TotalHours;

                if (timeOT > 0)
                {
                    if (ngayLviec == "NLCC") // ngay le cuoi cung
                    {
                        lstResult.Add(new OvertimeValue()
                        {
                            DayCheckOT = dateCheck,
                            DMOvertime = hsOT,
                            ValueOT = timeOT - 0.5,
                            Registered = isRegistedOT,
                            fromTime = firstTime,
                            toTime = lastTime,
                            orderTime = 1
                        });
                    }
                    else if (ngayLviec == "NL" || ngayLviec == "TNL") // NGAY LE
                    {
                        lstResult.Add(new OvertimeValue()
                        {
                            DayCheckOT = dateCheck,
                            DMOvertime = hsOT,
                            ValueOT = timeOT - 0.5,
                            Registered = isRegistedOT,
                            fromTime = firstTime,
                            toTime = lastTime,
                            orderTime = 1
                        });
                    }
                    else if (ngayLviec == "CN")
                    {
                        lstResult.Add(new OvertimeValue()
                        {
                            DayCheckOT = dateCheck,
                            DMOvertime = hsOT,
                            ValueOT = timeOT - 0.5, // O.5H di an
                            Registered = isRegistedOT,
                            fromTime = firstTime,
                            toTime = lastTime,
                            orderTime = 1
                        });
                    }
                    else if (ngayLviec == "NT9")
                    {
                        if (DateTime.ParseExact(lastTime, "HH:mm:ss", CultureInfo.InvariantCulture) >= DateTime.ParseExact("05:30:00", "HH:mm:ss", CultureInfo.InvariantCulture))
                        {
                            lstResult.Add(new OvertimeValue()
                            {
                                DayCheckOT = dateCheck,
                                DMOvertime = hsOT,
                                ValueOT = 0.5,
                                Registered = isRegistedOT,
                                fromTime = "05:30:00",
                                toTime = "06:00:00",
                                orderTime = 1
                            });
                        }
                    }
                }
            }
            else if (DateTime.ParseExact(firstTime, "HH:mm:ss", CultureInfo.InvariantCulture) >= DateTime.ParseExact("06:00:00", "HH:mm:ss", CultureInfo.InvariantCulture) && // 06->08
                     DateTime.ParseExact(firstTime, "HH:mm:ss", CultureInfo.InvariantCulture) <= DateTime.ParseExact("08:00:00", "HH:mm:ss", CultureInfo.InvariantCulture) &&
                     DateTime.ParseExact(lastTime, "HH:mm:ss", CultureInfo.InvariantCulture) >= DateTime.ParseExact("06:00:00", "HH:mm:ss", CultureInfo.InvariantCulture) &&
                     DateTime.ParseExact(lastTime, "HH:mm:ss", CultureInfo.InvariantCulture) <= DateTime.ParseExact("10:00:00", "HH:mm:ss", CultureInfo.InvariantCulture)) // change 8:00=> 10:00 
            {
                double timeOT = (DateTime.ParseExact(dateCheck + " " + lastTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) - DateTime.ParseExact(dateCheck + " " + firstTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)).TotalHours;
                string hsOT = clviecs.FirstOrDefault(x => x.Time_KetThuc == "08:00:00")?.HeSo_OT.IfNullIsZero();

                if (timeOT > 0)
                {
                    if (ngayLviec == "NLCC") // ngay le cuoi cung
                    {
                        lstResult.Add(new OvertimeValue()
                        {
                            DayCheckOT = dateCheck,
                            DMOvertime = hsOT,
                            ValueOT = timeOT,
                            Registered = isRegistedOT,
                            fromTime = firstTime,
                            toTime = lastTime,
                            orderTime = 1
                        });
                    }
                    else if (ngayLviec == "NL" || ngayLviec == "TNL") // NGAY LE
                    {
                        lstResult.Add(new OvertimeValue()
                        {
                            DayCheckOT = dateCheck,
                            DMOvertime = hsOT,
                            ValueOT = timeOT,
                            Registered = isRegistedOT,
                            fromTime = firstTime,
                            toTime = lastTime,
                            orderTime = 1
                        });
                    }
                    else if (ngayLviec == "CN")
                    {
                        lstResult.Add(new OvertimeValue()
                        {
                            DayCheckOT = dateCheck,
                            DMOvertime = hsOT,
                            ValueOT = timeOT,
                            Registered = isRegistedOT,
                            fromTime = firstTime,
                            toTime = lastTime,
                            orderTime = 1
                        });
                    }
                    else if (ngayLviec == "NT9")
                    {
                        if (DateTime.ParseExact(lastTime, "HH:mm:ss", CultureInfo.InvariantCulture) > DateTime.ParseExact("06:00:00", "HH:mm:ss", CultureInfo.InvariantCulture))
                        {
                            lstResult.Add(new OvertimeValue()
                            {
                                DayCheckOT = dateCheck,
                                DMOvertime = hsOT,
                                ValueOT = timeOT,
                                Registered = isRegistedOT,
                                fromTime = "06:00:00",
                                toTime = "08:00:00",
                                orderTime = 1
                            });
                        }
                    }
                }
            }
            else if (DateTime.ParseExact(firstTime, "HH:mm:ss", CultureInfo.InvariantCulture) >= DateTime.ParseExact("20:00:00", "HH:mm:ss", CultureInfo.InvariantCulture) &&
                     DateTime.ParseExact(firstTime, "HH:mm:ss", CultureInfo.InvariantCulture) <= DateTime.ParseExact("23:59:59", "HH:mm:ss", CultureInfo.InvariantCulture) &&
                    DateTime.ParseExact(lastTime, "HH:mm:ss", CultureInfo.InvariantCulture) > DateTime.ParseExact("22:00:00", "HH:mm:ss", CultureInfo.InvariantCulture) &&
                    DateTime.ParseExact(lastTime, "HH:mm:ss", CultureInfo.InvariantCulture) <= DateTime.ParseExact("23:59:59", "HH:mm:ss", CultureInfo.InvariantCulture))
            {
                double timeOT1 = (DateTime.ParseExact(dateCheck + " " + "22:00:00", "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) - DateTime.ParseExact(dateCheck + " " + firstTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)).TotalHours; // 20 ->22
                double timeOT2 = (DateTime.ParseExact(dateCheck + " " + lastTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) - DateTime.ParseExact(dateCheck + " " + "22:00:00", "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)).TotalHours; // 22 -> 00

                string hsOT1 = clviecs.FirstOrDefault(x => x.Time_KetThuc == "22:00:00")?.HeSo_OT.IfNullIsZero();
                string hsOT2 = clviecs.FirstOrDefault(x => x.Time_BatDau == "22:00:00")?.HeSo_OT.IfNullIsZero();

                //if (timeOT1 < 0.5)
                //{
                //    timeOT1 = 0;
                //}
                //if (timeOT2 < 0.5)
                //{
                //    timeOT2 = 0;
                //}

                if (ngayLviec == "CN") // ngay chu nhat
                {
                    lstResult.Add(new OvertimeValue()
                    {
                        DayCheckOT = dateCheck,
                        DMOvertime = hsOT1,
                        ValueOT = timeOT1,
                        Registered = isRegistedOT,
                        fromTime = firstTime,
                        toTime = "22:00:00",
                        orderTime = 1
                    });
                }
                else if (ngayLviec == "NLCC" || ngayLviec == "NL") // Ngay le, ngay le cuoi cung
                {
                    lstResult.Add(new OvertimeValue()
                    {
                        DayCheckOT = dateCheck,
                        DMOvertime = hsOT1,
                        ValueOT = timeOT1,
                        Registered = isRegistedOT,
                        fromTime = firstTime,
                        toTime = "22:00:00",
                        orderTime = 1
                    });
                }

                if (ngayLviec == "NLCC" || ngayLviec == "NL")
                {
                    lstResult.Add(new OvertimeValue()
                    {
                        DayCheckOT = dateCheck,
                        DMOvertime = hsOT2,
                        ValueOT = timeOT2,
                        Registered = isRegistedOT,
                        fromTime = "22:00:00",
                        toTime = lastTime,
                        orderTime = 2
                    });
                }
                else if (ngayLviec == "CN")
                {
                    lstResult.Add(new OvertimeValue()
                    {
                        DayCheckOT = dateCheck,
                        DMOvertime = hsOT2,
                        ValueOT = timeOT2,
                        Registered = isRegistedOT,
                        fromTime = "22:00:00",
                        toTime = lastTime,
                        orderTime = 2
                    });
                }
            }
            else if (DateTime.ParseExact(firstTime, "HH:mm:ss", CultureInfo.InvariantCulture) >= DateTime.ParseExact("20:00:00", "HH:mm:ss", CultureInfo.InvariantCulture) && // 20 -> 06
                     DateTime.ParseExact(firstTime, "HH:mm:ss", CultureInfo.InvariantCulture) <= DateTime.ParseExact("23:59:59", "HH:mm:ss", CultureInfo.InvariantCulture) &&
                     DateTime.ParseExact(lastTime, "HH:mm:ss", CultureInfo.InvariantCulture) > DateTime.ParseExact("00:00:00", "HH:mm:ss", CultureInfo.InvariantCulture) &&
                     DateTime.ParseExact(lastTime, "HH:mm:ss", CultureInfo.InvariantCulture) <= DateTime.ParseExact("06:00:00", "HH:mm:ss", CultureInfo.InvariantCulture))
            {
                double timeOT1 = (DateTime.ParseExact(dateCheck + " " + "22:00:00", "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) - DateTime.ParseExact(dateCheck + " " + firstTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)).TotalHours; // 20 ->22
                double timeOT2 = 2; // 22 -> 00
                double timeOT3 = (DateTime.ParseExact(dateCheck + " " + lastTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) - DateTime.ParseExact(dateCheck + " " + "00:00:00", "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)).TotalHours; // 00 -> 06

                string hsOT1 = clviecs.FirstOrDefault(x => x.Time_KetThuc == "22:00:00")?.HeSo_OT.IfNullIsZero(); // 20->22
                string hsOT2 = clviecs.FirstOrDefault(x => x.Time_BatDau == "22:00:00")?.HeSo_OT.IfNullIsZero(); // 22->00
                string hsOT3 = clviecs.FirstOrDefault(x => x.Time_KetThuc.CompareTo("06:00:00") <= 0)?.HeSo_OT.IfNullIsZero(); // 00 -> 6

                //if (timeOT1 < 0.5)
                //{
                //    timeOT1 = 0;
                //}
                //if (timeOT3 < 0.5)
                //{
                //    timeOT3 = 0;
                //}

                if (ngayLviec == "CN") // ngay chu nhat
                {
                    lstResult.Add(new OvertimeValue()
                    {
                        DayCheckOT = dateCheck,
                        DMOvertime = hsOT1,
                        ValueOT = timeOT1,
                        Registered = isRegistedOT,
                        fromTime = firstTime,
                        toTime = "22:00:00",
                        orderTime = 1
                    });
                }
                else if (ngayLviec == "NLCC" || ngayLviec == "NL") // Ngay le, ngay le cuoi cung
                {
                    lstResult.Add(new OvertimeValue()
                    {
                        DayCheckOT = dateCheck,
                        DMOvertime = hsOT1,
                        ValueOT = timeOT1,
                        Registered = isRegistedOT,
                        fromTime = firstTime,
                        toTime = "22:00:00",
                        orderTime = 1
                    });
                }

                if (ngayLviec == "NLCC" || ngayLviec == "NL")
                {
                    lstResult.Add(new OvertimeValue()
                    {
                        DayCheckOT = dateCheck,
                        DMOvertime = hsOT2,
                        ValueOT = timeOT2,
                        Registered = isRegistedOT,
                        fromTime = "22:00:00",
                        toTime = "23:59:59",
                        orderTime = 2
                    });
                }
                else if (ngayLviec == "CN")
                {
                    lstResult.Add(new OvertimeValue()
                    {
                        DayCheckOT = dateCheck,
                        DMOvertime = hsOT2,
                        ValueOT = timeOT2,
                        Registered = isRegistedOT,
                        fromTime = "22:00:00",
                        toTime = "23:59:59",
                        orderTime = 2
                    });
                }

                if (ngayLviec == "NLCC") // ngay le cuoi cung
                {
                    lstResult.Add(new OvertimeValue()
                    {
                        DayCheckOT = dateCheck,
                        DMOvertime = hsOT3,
                        ValueOT = timeOT3 - 0.5,
                        Registered = isRegistedOT,
                        fromTime = "00:00:00",
                        toTime = "06:00:00",
                        orderTime = 3
                    });
                }
                else if (ngayLviec == "NL" || ngayLviec == "TNL") // NGAY LE
                {
                    lstResult.Add(new OvertimeValue()
                    {
                        DayCheckOT = dateCheck,
                        DMOvertime = hsOT3,
                        ValueOT = timeOT3 - 0.5,
                        Registered = isRegistedOT,
                        fromTime = "00:00:00",
                        toTime = "06:00:00",
                        orderTime = 3
                    });
                }
                else if (ngayLviec == "CN")
                {
                    lstResult.Add(new OvertimeValue()
                    {
                        DayCheckOT = dateCheck,
                        DMOvertime = hsOT3,
                        ValueOT = timeOT3 - 0.5, // O.5H di an
                        Registered = isRegistedOT,
                        fromTime = "00:00:00",
                        toTime = "06:00:00",
                        orderTime = 3
                    });
                }
                else if (ngayLviec == "NT9")
                {
                    if (DateTime.ParseExact(lastTime, "HH:mm:ss", CultureInfo.InvariantCulture) >= DateTime.ParseExact("05:30:00", "HH:mm:ss", CultureInfo.InvariantCulture))
                    {
                        lstResult.Add(new OvertimeValue()
                        {
                            DayCheckOT = dateCheck,
                            DMOvertime = hsOT3,
                            ValueOT = 0.5,
                            Registered = isRegistedOT,
                            fromTime = "05:30:00",
                            toTime = "06:00:00",
                            orderTime = 3
                        });
                    }
                }
            }
            else if (DateTime.ParseExact(firstTime, "HH:mm:ss", CultureInfo.InvariantCulture) >= DateTime.ParseExact("20:00:00", "HH:mm:ss", CultureInfo.InvariantCulture) && // 20 -> 08
                    DateTime.ParseExact(firstTime, "HH:mm:ss", CultureInfo.InvariantCulture) <= DateTime.ParseExact("23:59:59", "HH:mm:ss", CultureInfo.InvariantCulture) &&
                    DateTime.ParseExact(lastTime, "HH:mm:ss", CultureInfo.InvariantCulture) > DateTime.ParseExact("06:00:00", "HH:mm:ss", CultureInfo.InvariantCulture) &&
                    DateTime.ParseExact(lastTime, "HH:mm:ss", CultureInfo.InvariantCulture) <= DateTime.ParseExact("10:00:00", "HH:mm:ss", CultureInfo.InvariantCulture))
            {
                double timeOT1 = (DateTime.ParseExact(dateCheck + " " + "22:00:00", "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) - DateTime.ParseExact(dateCheck + " " + firstTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)).TotalHours; // 20 ->22
                double timeOT2 = 2; // 22 -> 00
                double timeOT3 = 4.5; // 00 -> 06 , 5h - tru 0.5 di an
                double timeOT4 = (DateTime.ParseExact(dateCheck + " " + lastTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) - DateTime.ParseExact(dateCheck + " " + "06:00:00", "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)).TotalHours; // 06 -> 08

                string hsOT1 = clviecs.FirstOrDefault(x => x.Time_KetThuc == "22:00:00")?.HeSo_OT.IfNullIsZero(); // 20->22
                string hsOT2 = clviecs.FirstOrDefault(x => x.Time_BatDau == "22:00:00")?.HeSo_OT.IfNullIsZero(); // 22->00
                string hsOT3 = clviecs.FirstOrDefault(x => x.Time_KetThuc.CompareTo("06:00:00") <= 0)?.HeSo_OT.IfNullIsZero(); // 00 -> 6
                string hsOT4 = clviecs.FirstOrDefault(x => x.Time_KetThuc == "08:00:00")?.HeSo_OT.IfNullIsZero(); // 6->8

                //if (timeOT1 < 0.5)
                //{
                //    timeOT1 = 0;
                //}
                //if (timeOT4 < 0.5)
                //{
                //    timeOT4 = 0;
                //}

                // 20 -> 22
                if (ngayLviec == "CN") // ngay chu nhat
                {
                    lstResult.Add(new OvertimeValue()
                    {
                        DayCheckOT = dateCheck,
                        DMOvertime = hsOT1,
                        ValueOT = timeOT1,
                        Registered = isRegistedOT,
                        fromTime = "20:00:00",
                        toTime = "22:00:00",
                        orderTime = 1
                    });
                }
                else if (ngayLviec == "NLCC" || ngayLviec == "NL") // Ngay le, ngay le cuoi cung
                {
                    lstResult.Add(new OvertimeValue()
                    {
                        DayCheckOT = dateCheck,
                        DMOvertime = hsOT1,
                        ValueOT = timeOT1,
                        Registered = isRegistedOT,
                        fromTime = "20:00:00",
                        toTime = "22:00:00",
                        orderTime = 1
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
                        Registered = isRegistedOT,
                        fromTime = "22:00:00",
                        toTime = "23:59:59",
                        orderTime = 2
                    });
                }
                else if (ngayLviec == "CN")
                {
                    lstResult.Add(new OvertimeValue()
                    {
                        DayCheckOT = dateCheck,
                        DMOvertime = hsOT2,
                        ValueOT = timeOT2,
                        Registered = isRegistedOT,
                        fromTime = "22:00:00",
                        toTime = "23:59:59",
                        orderTime = 2
                    });
                }

                // 0 -> 6
                if (ngayLviec == "NLCC") // ngay le cuoi cung
                {
                    lstResult.Add(new OvertimeValue()
                    {
                        DayCheckOT = dateCheck,
                        DMOvertime = hsOT3,
                        ValueOT = timeOT3,
                        Registered = isRegistedOT,
                        fromTime = "00:00:00",
                        toTime = "05:30:00",
                        orderTime = 3
                    });
                }
                else if (ngayLviec == "NL" || ngayLviec == "TNL") // NGAY LE
                {
                    lstResult.Add(new OvertimeValue()
                    {
                        DayCheckOT = dateCheck,
                        DMOvertime = hsOT3,
                        ValueOT = timeOT3,
                        Registered = isRegistedOT,
                        fromTime = "00:00:00",
                        toTime = "05:30:00",
                        orderTime = 3
                    });
                }
                else if (ngayLviec == "CN")
                {
                    lstResult.Add(new OvertimeValue()
                    {
                        DayCheckOT = dateCheck,
                        DMOvertime = hsOT3,
                        ValueOT = timeOT3, // O.5H di an
                        Registered = isRegistedOT,
                        fromTime = "00:00:00",
                        toTime = "05:30:00",
                        orderTime = 3
                    });
                }
                else if (ngayLviec == "NT9")
                {
                    if (DateTime.ParseExact(lastTime, "HH:mm:ss", CultureInfo.InvariantCulture) >= DateTime.ParseExact("05:30:00", "HH:mm:ss", CultureInfo.InvariantCulture))
                    {
                        lstResult.Add(new OvertimeValue()
                        {
                            DayCheckOT = dateCheck,
                            DMOvertime = hsOT3,
                            ValueOT = 0.5,
                            Registered = isRegistedOT,
                            fromTime = "05:00:00",
                            toTime = "06:00:00",
                            orderTime = 3
                        });
                    }
                }

                // 6-8h
                if (ngayLviec == "NLCC") // ngay le cuoi cung
                {
                    lstResult.Add(new OvertimeValue()
                    {
                        DayCheckOT = dateCheck,
                        DMOvertime = hsOT4,
                        ValueOT = timeOT4,
                        Registered = isRegistedOT,
                        fromTime = "06:00:00",
                        toTime = lastTime,
                        orderTime = 4
                    });
                }
                else if (ngayLviec == "NL" || ngayLviec == "TNL") // NGAY LE
                {
                    lstResult.Add(new OvertimeValue()
                    {
                        DayCheckOT = dateCheck,
                        DMOvertime = hsOT4,
                        ValueOT = timeOT4,
                        Registered = isRegistedOT,
                        fromTime = "06:00:00",
                        toTime = lastTime,
                        orderTime = 4
                    });
                }
                else if (ngayLviec == "CN")
                {
                    lstResult.Add(new OvertimeValue()
                    {
                        DayCheckOT = dateCheck,
                        DMOvertime = hsOT4,
                        ValueOT = timeOT4,
                        Registered = isRegistedOT,
                        fromTime = "06:00:00",
                        toTime = lastTime,
                        orderTime = 4
                    });
                }
                else if (ngayLviec == "NT9")
                {
                    if (DateTime.ParseExact(lastTime, "HH:mm:ss", CultureInfo.InvariantCulture) >= DateTime.ParseExact("06:00:00", "HH:mm:ss", CultureInfo.InvariantCulture))
                    {
                        lstResult.Add(new OvertimeValue()
                        {
                            DayCheckOT = dateCheck,
                            DMOvertime = hsOT4,
                            ValueOT = timeOT4,
                            Registered = isRegistedOT,
                            fromTime = "06:00:00",
                            toTime = lastTime,
                            orderTime = 4
                        });
                    }
                }
            }
            else if (DateTime.ParseExact(firstTime, "HH:mm:ss", CultureInfo.InvariantCulture) >= DateTime.ParseExact("22:00:00", "HH:mm:ss", CultureInfo.InvariantCulture) && // 22 -> 06
                DateTime.ParseExact(firstTime, "HH:mm:ss", CultureInfo.InvariantCulture) <= DateTime.ParseExact("23:59:59", "HH:mm:ss", CultureInfo.InvariantCulture) &&
                DateTime.ParseExact(lastTime, "HH:mm:ss", CultureInfo.InvariantCulture) > DateTime.ParseExact("00:00:00", "HH:mm:ss", CultureInfo.InvariantCulture) &&
                 DateTime.ParseExact(lastTime, "HH:mm:ss", CultureInfo.InvariantCulture) <= DateTime.ParseExact("06:00:00", "HH:mm:ss", CultureInfo.InvariantCulture))
            {
                double timeOT1 = (DateTime.ParseExact(dateCheck + " " + "23:59:59", "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) - DateTime.ParseExact(dateCheck + " " + firstTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)).TotalHours; // 22-> 00
                double timeOT2 = (DateTime.ParseExact(dateCheck + " " + lastTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) - DateTime.ParseExact(dateCheck + " " + "00:00:00", "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)).TotalHours; // 00 -> 06

                string hsOT1 = clviecs.FirstOrDefault(x => x.Time_BatDau == "22:00:00")?.HeSo_OT.IfNullIsZero(); // 22->00
                string hsOT2 = clviecs.FirstOrDefault(x => x.Time_KetThuc.CompareTo("06:00:00") <= 0)?.HeSo_OT.IfNullIsZero(); // 00 -> 6

                //if (timeOT1 < 0.5)
                //{
                //    timeOT1 = 0;
                //}
                //if (timeOT2 < 0.5)
                //{
                //    timeOT2 = 0;
                //}

                // 22 -> 0
                if (ngayLviec == "NLCC" || ngayLviec == "NL") // ngay chu nhat
                {
                    lstResult.Add(new OvertimeValue()
                    {
                        DayCheckOT = dateCheck,
                        DMOvertime = hsOT1,
                        ValueOT = timeOT1,
                        Registered = isRegistedOT,
                        fromTime = firstTime,
                        toTime = "23:59:59",
                        orderTime = 1
                    });
                }
                else if (ngayLviec == "CN")
                {
                    lstResult.Add(new OvertimeValue()
                    {
                        DayCheckOT = dateCheck,
                        DMOvertime = hsOT1,
                        ValueOT = timeOT1,
                        Registered = isRegistedOT,
                        fromTime = firstTime,
                        toTime = "23:59:59",
                        orderTime = 1
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
                        Registered = isRegistedOT,
                        fromTime = "00:00:00",
                        toTime = lastTime,
                        orderTime = 2
                    });
                }
                else if (ngayLviec == "NL" || ngayLviec == "TNL") // NGAY LE
                {
                    lstResult.Add(new OvertimeValue()
                    {
                        DayCheckOT = dateCheck,
                        DMOvertime = hsOT2,
                        ValueOT = timeOT2 - 0.5,
                        Registered = isRegistedOT,
                        fromTime = "00:00:00",
                        toTime = lastTime,
                        orderTime = 2
                    });
                }
                else if (ngayLviec == "CN")
                {
                    lstResult.Add(new OvertimeValue()
                    {
                        DayCheckOT = dateCheck,
                        DMOvertime = hsOT2,
                        ValueOT = timeOT2 - 0.5, // O.5H di an
                        Registered = isRegistedOT,
                        fromTime = "00:00:00",
                        toTime = lastTime,
                        orderTime = 2
                    });
                }
                else if (ngayLviec == "NT9")
                {
                    if (DateTime.ParseExact(lastTime, "HH:mm:ss", CultureInfo.InvariantCulture) > DateTime.ParseExact("05:30:00", "HH:mm:ss", CultureInfo.InvariantCulture))
                    {
                        lstResult.Add(new OvertimeValue()
                        {
                            DayCheckOT = dateCheck,
                            DMOvertime = hsOT2,
                            ValueOT = 0.5,
                            Registered = isRegistedOT,
                            fromTime = "05:00:00",
                            toTime = "06:00:00",
                            orderTime = 2
                        });
                    }
                }
            }
            else if (
                 DateTime.ParseExact(firstTime, "HH:mm:ss", CultureInfo.InvariantCulture) >= DateTime.ParseExact("22:00:00", "HH:mm:ss", CultureInfo.InvariantCulture) && // 22 -> 08
                 DateTime.ParseExact(firstTime, "HH:mm:ss", CultureInfo.InvariantCulture) < DateTime.ParseExact("23:59:59", "HH:mm:ss", CultureInfo.InvariantCulture) &&
                 DateTime.ParseExact(lastTime, "HH:mm:ss", CultureInfo.InvariantCulture) > DateTime.ParseExact("06:00:00", "HH:mm:ss", CultureInfo.InvariantCulture))
            {
                double timeOT2 = (DateTime.ParseExact(dateCheck + " " + "23:59:59", "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) - DateTime.ParseExact(dateCheck + " " + firstTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)).TotalHours; // 22 -> 00
                double timeOT3 = 4.5; // 00 -> 06 , 5h - tru 0.5 di an
                double timeOT4 = (DateTime.ParseExact(dateCheck + " " + lastTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) - DateTime.ParseExact(dateCheck + " " + "06:00:00", "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)).TotalHours; // 06 -> 08

                string hsOT2 = clviecs.FirstOrDefault(x => x.Time_BatDau == "22:00:00")?.HeSo_OT.IfNullIsZero(); // 22->00
                string hsOT3 = clviecs.FirstOrDefault(x => x.Time_KetThuc.CompareTo("06:00:00") <= 0)?.HeSo_OT.IfNullIsZero(); // 00 > 6
                string hsOT4 = clviecs.FirstOrDefault(x => x.Time_KetThuc == "08:00:00")?.HeSo_OT.IfNullIsZero(); // 6->8

                //if (timeOT4 < 0.5)
                //{
                //    timeOT4 = 0;
                //}

                //if (timeOT2 < 0.5)
                //{
                //    timeOT2 = 0;
                //}

                // 22-> 0
                if (ngayLviec == "NLCC" || ngayLviec == "NL")
                {
                    lstResult.Add(new OvertimeValue()
                    {
                        DayCheckOT = dateCheck,
                        DMOvertime = hsOT2,
                        ValueOT = timeOT2,
                        Registered = isRegistedOT,
                        fromTime = "22:00:00",
                        toTime = "23:59:59",
                        orderTime = 1
                    });
                }
                else if (ngayLviec == "CN")
                {
                    lstResult.Add(new OvertimeValue()
                    {
                        DayCheckOT = dateCheck,
                        DMOvertime = hsOT2,
                        ValueOT = timeOT2,
                        Registered = isRegistedOT,
                        fromTime = "22:00:00",
                        toTime = "23:59:59",
                        orderTime = 1
                    });
                }

                // 0 -> 6
                if (ngayLviec == "NLCC") // ngay le cuoi cung
                {
                    lstResult.Add(new OvertimeValue()
                    {
                        DayCheckOT = dateCheck,
                        DMOvertime = hsOT3,
                        ValueOT = timeOT3,
                        Registered = isRegistedOT,
                        fromTime = "00:00:00",
                        toTime = "05:30:30",
                        orderTime = 2
                    });
                }
                else if (ngayLviec == "NL" || ngayLviec == "TNL") // NGAY LE
                {
                    lstResult.Add(new OvertimeValue()
                    {
                        DayCheckOT = dateCheck,
                        DMOvertime = hsOT3,
                        ValueOT = timeOT3,
                        Registered = isRegistedOT,
                        fromTime = "00:00:00",
                        toTime = "05:30:30",
                        orderTime = 2
                    });
                }
                else if (ngayLviec == "CN")
                {
                    lstResult.Add(new OvertimeValue()
                    {
                        DayCheckOT = dateCheck,
                        DMOvertime = hsOT3,
                        ValueOT = timeOT3, // O.5H di an
                        Registered = isRegistedOT,
                        fromTime = "00:00:00",
                        toTime = "05:30:30",
                        orderTime = 2
                    });
                }
                else if (ngayLviec == "NT9")
                {
                    if (DateTime.ParseExact(lastTime, "HH:mm:ss", CultureInfo.InvariantCulture) > DateTime.ParseExact("05:30:00", "HH:mm:ss", CultureInfo.InvariantCulture))
                    {
                        lstResult.Add(new OvertimeValue()
                        {
                            DayCheckOT = dateCheck,
                            DMOvertime = hsOT3,
                            ValueOT = 0.5,
                            Registered = isRegistedOT,
                            fromTime = "05:00:00",
                            toTime = "06:00:00",
                            orderTime = 2
                        });
                    }
                }

                // 6-8h
                if (ngayLviec == "NLCC") // ngay le cuoi cung
                {
                    lstResult.Add(new OvertimeValue()
                    {
                        DayCheckOT = dateCheck,
                        DMOvertime = hsOT4,
                        ValueOT = timeOT4,
                        Registered = isRegistedOT,
                        fromTime = "06:00:00",
                        toTime = lastTime,
                        orderTime = 3
                    });
                }
                else if (ngayLviec == "NL" || ngayLviec == "TNL") // NGAY LE
                {
                    lstResult.Add(new OvertimeValue()
                    {
                        DayCheckOT = dateCheck,
                        DMOvertime = hsOT4,
                        ValueOT = timeOT4,
                        Registered = isRegistedOT,
                        fromTime = "06:00:00",
                        toTime = lastTime,
                        orderTime = 3
                    });
                }
                else if (ngayLviec == "CN")
                {
                    lstResult.Add(new OvertimeValue()
                    {
                        DayCheckOT = dateCheck,
                        DMOvertime = hsOT4,
                        ValueOT = timeOT4,
                        Registered = isRegistedOT,
                        fromTime = "06:00:00",
                        toTime = lastTime,
                        orderTime = 3
                    });
                }
                else if (ngayLviec == "NT9")
                {
                    if (DateTime.ParseExact(lastTime, "HH:mm:ss", CultureInfo.InvariantCulture) > DateTime.ParseExact("06:00:00", "HH:mm:ss", CultureInfo.InvariantCulture))
                    {
                        lstResult.Add(new OvertimeValue()
                        {
                            DayCheckOT = dateCheck,
                            DMOvertime = hsOT4,
                            ValueOT = timeOT4,
                            Registered = isRegistedOT,
                            fromTime = "06:00:00",
                            toTime = "08:00:00",
                            orderTime = 2
                        });
                    }
                }
            }
            else if (DateTime.ParseExact(firstTime, "HH:mm:ss", CultureInfo.InvariantCulture) >= DateTime.ParseExact("00:00:00", "HH:mm:ss", CultureInfo.InvariantCulture) && // 00 -> 08
                DateTime.ParseExact(firstTime, "HH:mm:ss", CultureInfo.InvariantCulture) < DateTime.ParseExact("06:00:00", "HH:mm:ss", CultureInfo.InvariantCulture) &&
               DateTime.ParseExact(lastTime, "HH:mm:ss", CultureInfo.InvariantCulture) > DateTime.ParseExact("06:00:00", "HH:mm:ss", CultureInfo.InvariantCulture))
            {
                double timeOT2 = (DateTime.ParseExact(dateCheck + " " + "06:00:00", "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) - DateTime.ParseExact(dateCheck + " " + firstTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)).TotalHours; // 00 -> 06
                double timeOT4 = (DateTime.ParseExact(dateCheck + " " + lastTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) - DateTime.ParseExact(dateCheck + " " + "06:00:00", "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)).TotalHours; // 06 -> 08

                string hsOT2 = clviecs.FirstOrDefault(x => x.Time_KetThuc.CompareTo("06:00:00") <= 0)?.HeSo_OT.IfNullIsZero(); // 00 -> 6
                string hsOT4 = clviecs.FirstOrDefault(x => x.Time_KetThuc == "08:00:00")?.HeSo_OT.IfNullIsZero(); //6->8

                //if (timeOT4 < 0.5)
                //{
                //    timeOT4 = 0;
                //}
                //if (timeOT2 < 0.5)
                //{
                //    timeOT2 = 0;
                //}

                // 0 -> 6
                if (ngayLviec == "NLCC") // ngay le cuoi cung
                {
                    lstResult.Add(new OvertimeValue()
                    {
                        DayCheckOT = dateCheck,
                        DMOvertime = hsOT2,
                        ValueOT = timeOT2 - 0.5,
                        Registered = isRegistedOT,
                        fromTime = firstTime,
                        toTime = "05:30:00",
                        orderTime = 1
                    });
                }
                else if (ngayLviec == "NL" || ngayLviec == "TNL") // NGAY LE
                {
                    lstResult.Add(new OvertimeValue()
                    {
                        DayCheckOT = dateCheck,
                        DMOvertime = hsOT2,
                        ValueOT = timeOT2 - 0.5,
                        Registered = isRegistedOT,
                        fromTime = firstTime,
                        toTime = "05:30:00",
                        orderTime = 1
                    });
                }
                else if (ngayLviec == "CN")
                {
                    lstResult.Add(new OvertimeValue()
                    {
                        DayCheckOT = dateCheck,
                        DMOvertime = hsOT2,
                        ValueOT = timeOT2 - 0.5, // O.5H di an
                        Registered = isRegistedOT,
                        fromTime = firstTime,
                        toTime = "05:30:00",
                        orderTime = 1
                    });
                }
                else if (ngayLviec == "NT9")
                {
                    if (DateTime.ParseExact(lastTime, "HH:mm:ss", CultureInfo.InvariantCulture) > DateTime.ParseExact("05:30:00", "HH:mm:ss", CultureInfo.InvariantCulture))
                    {
                        lstResult.Add(new OvertimeValue()
                        {
                            DayCheckOT = dateCheck,
                            DMOvertime = hsOT2,
                            ValueOT = 0.5,
                            Registered = isRegistedOT,
                            fromTime = "05:00:00",
                            toTime = "06:00:00",
                            orderTime = 1
                        });
                    }
                }

                // 6-8h
                if (ngayLviec == "NLCC") // ngay le cuoi cung
                {
                    lstResult.Add(new OvertimeValue()
                    {
                        DayCheckOT = dateCheck,
                        DMOvertime = "150",
                        ValueOT = timeOT4,
                        Registered = isRegistedOT,
                        fromTime = "06:00:00",
                        toTime = lastTime,
                        orderTime = 2
                    });
                }
                else if (ngayLviec == "NL" || ngayLviec == "TNL") // NGAY LE
                {
                    lstResult.Add(new OvertimeValue()
                    {
                        DayCheckOT = dateCheck,
                        DMOvertime = hsOT4,
                        ValueOT = timeOT4,
                        Registered = isRegistedOT,
                        fromTime = "06:00:00",
                        toTime = lastTime,
                        orderTime = 2
                    });
                }
                else if (ngayLviec == "CN")
                {
                    lstResult.Add(new OvertimeValue()
                    {
                        DayCheckOT = dateCheck,
                        DMOvertime = hsOT4,
                        ValueOT = timeOT4,
                        Registered = isRegistedOT,
                        fromTime = "06:00:00",
                        toTime = lastTime,
                        orderTime = 2
                    });
                }
                else if (ngayLviec == "NT9")
                {
                    if (DateTime.ParseExact(lastTime, "HH:mm:ss", CultureInfo.InvariantCulture) > DateTime.ParseExact("06:00:00", "HH:mm:ss", CultureInfo.InvariantCulture))
                    {
                        lstResult.Add(new OvertimeValue()
                        {
                            DayCheckOT = dateCheck,
                            DMOvertime = hsOT4,
                            ValueOT = timeOT4,
                            Registered = isRegistedOT,
                            fromTime = "06:00:00",
                            toTime = "08:00:00",
                            orderTime = 2
                        });
                    }
                }
            }

            List<OvertimeValue> overtimes = new List<OvertimeValue>();
            OvertimeValue oval;
            foreach (var item in lstResult)
            {
                if (overtimes.Any(x => x.DMOvertime == item.DMOvertime))
                {
                    oval = overtimes.Find(x => x.DMOvertime == item.DMOvertime);
                    oval.ValueOT += item.ValueOT;
                }
                else
                {
                    overtimes.Add(item);
                }
            }

            foreach (var item in overtimes)
            {
                item.ValueOT = HR_NHANVIEN_CHEDO_DBs.Exists(x => x.MaNhanVien == maNV && x.CheDoDB.Contains("Block_10p")) ? SimpleValueOT(item.ValueOT) : Block30mValueOT(item.ValueOT);
                //if ((new List<int> { 1, 5, 3, 4 }).Contains(CheckNgayDB(item.DayCheckOT))) // chu nhat ngay le tinh block 30p. ngay khac 10p
                //{
                //    item.ValueOT = Block30mValueOT(item.ValueOT);
                //}
                //else
                //{
                //    item.ValueOT = Block30mValueOT(item.ValueOT);
                //}
            }

            double totalDuration = 0;
            foreach (var item in overtimes.OrderBy(x => x.orderTime))
            {
                totalDuration += item.ValueOT;
            }

            themGioModel.Duration = totalDuration.IfNullIsZero();
            themGioModel.From = overtimes.OrderBy(x => x.orderTime).FirstOrDefault()?.fromTime;
            themGioModel.To = overtimes.OrderBy(x => x.orderTime).LastOrDefault()?.toTime;

            return overtimes;
        }

        private string ResetULIL(string kyhieuChamCongDB, string dateCheck)
        {

            // CHU NHẬT THÌ IL,UL k chấm
            if (IsChuNhat(dateCheck) && (kyhieuChamCongDB.NullString() == "IL" || kyhieuChamCongDB.NullString() == "UL" || kyhieuChamCongDB.NullString() == "T"))
            {
                kyhieuChamCongDB = "-";
            }
            return kyhieuChamCongDB;
        }

        private bool IsChuNhat(string time)
        {
            return DateTime.Parse(time).DayOfWeek == DayOfWeek.Sunday;
        }

        private string IsNgayLe(string time)
        {
            var ngayle = NGAY_LE_NAMs == null ? _ngaylenamRespository.FindById(time) : NGAY_LE_NAMs.FirstOrDefault(x => x.Id == time);
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
            var ngaynghibu = NGAY_NGHI_BU_LE_NAMs == null ? _ngaynghibuRespository.FindSingle(x => x.NgayNghiBu == time) : NGAY_NGHI_BU_LE_NAMs.FirstOrDefault(x => x.NgayNghiBu == time);
            if (ngaynghibu != null)
            {
                return time;
            }
            return "";
        }

        // ngay ki niem cong ty
        private string IsNgayDacBiet(string time)
        {
            var ngaydacbiet = NGAY_DAC_BIETs == null ? _ngaydacbietRespository.FindSingle(x => time.Contains(x.Id)) : NGAY_DAC_BIETs.FirstOrDefault(x => time.Contains(x.Id));
            if (ngaydacbiet != null)
            {
                return time;
            }
            return "";
        }

        private int CheckNgayDB(string time, string calviec = "")
        {
            if (IsNgayLe(time) != "")
            {
                return 1;
            }

            if (IsChuNhat(time))
            {
                return 3;
            }

            //if(calviec == CommonConstants.CA_NGAY)
            //{
            if (IsNgayDacBiet(time) != "")
            {
                return 5;
            }
            //}

            if (IsNgayTruocLe(time) != "")
            {
                return 2;
            }

            if (IsNgayNghiBuLeNam(time) != "")
            {
                return 4;
            }

            //if (calviec == CommonConstants.CA_DEM)
            //{
            //    if (IsNgayDacBiet(time) != "")
            //    {
            //        return 5;
            //    }
            //}

            return 0;// ngay thuong
        }

        /// <summary>
        /// Tông hợp nhân sự đi làm, nghỉ phép theo tháng, bộ phận.
        /// </summary>
        /// <param name="time"></param>
        /// <param name="dept"></param>
        /// <returns></returns>
        public List<TongHopNhanSuDailyViewModel> TongHopNhanSuReport(string time, string dept)
        {
            string newTime = time + "-01";
            int dayInMonth = DateTime.DaysInMonth(DateTime.Parse(newTime).Year, DateTime.Parse(newTime).Month);
            string _time = time;
            List<TongHopNhanSuDailyViewModel> dailyViewModels = new List<TongHopNhanSuDailyViewModel>();

            for (int i = 1; i <= dayInMonth; i++)
            {
                _time = time;

                if (i <= 9)
                {
                    _time += "-0" + i;
                }
                else
                {
                    _time += "-" + i;
                }

                if (DateTime.Parse(_time).DayOfWeek == DayOfWeek.Sunday || DateTime.Parse(_time).CompareTo(DateTime.Now) > 0)
                {
                    continue;
                }

                var lstNV = _nhanvienResponsitory.FindAll(x => x.MaBoPhan == dept && (x.Status == Status.Active.NullString() || string.Compare(x.NgayNghiViec, _time) > 0));
                var lstGr = lstNV.GroupBy(x => x.MaBoPhan).Select(gr => new { BoPhan = gr.Key, Count = gr.Count() });

                TongHopNhanSuDailyViewModel model;
                foreach (var item in lstGr)
                {
                    model = new TongHopNhanSuDailyViewModel()
                    {
                        BoPhan = item.BoPhan,
                        TongNV = item.Count,
                        NgayBaoCao = _time
                    };
                    dailyViewModels.Add(model);
                }
            }

            CaLamViecSL caLamViec;
            double al = 0;
            double ul = 0;
            double nl = 0;
            double l70 = 0;
            double sl = 0;
            double ct = 0;
            double hl = 0;
            double t = 0;
            double nh = 0;
            double el = 0;
            double lc = 0;
            double il = 0;
            double nb = 0;
            double b = 0;
            string note = "";

            var lstThaiSan = _thaisanResponsitory.FindAll().ToList();
            var lstNVAll = _nhanvienResponsitory.FindAll(x => x.Status == Status.Active.NullString()).ToList();
            foreach (var item in dailyViewModels) // duyet tung ngay với bo phan
            {
                note = "";
                var lstTS = lstThaiSan.Where(x => x.CheDoThaiSan == "ThaiSan" && string.Compare(item.NgayBaoCao, x.FromDate) >= 0 && string.Compare(item.NgayBaoCao, x.ToDate) <= 0);
                var lstNV = lstNVAll.Where(x => x.MaBoPhan == item.BoPhan).Select(x => x.Id);

                item.NghiTS = 0;
                foreach (var ts in lstTS)
                {
                    if (lstNV.Contains(ts.MaNV))
                    {
                        item.NghiTS += 1;
                    }
                }

                //item.NghiTS = _chamCongDBResponsitory.FindAll(
                //    x => (x.HR_NHANVIEN.Status == Status.Active.NullString() || string.Compare(x.HR_NHANVIEN.NgayNghiViec, item.NgayBaoCao) >= 0) &&
                //    x.HR_NHANVIEN.MaBoPhan == item.BoPhan &&
                //    x.DANGKY_CHAMCONG_CHITIET.KyHieuChamCong == "IL" &&
                //    string.Compare(item.NgayBaoCao, x.NgayBatDau) >= 0 &&
                //    string.Compare(item.NgayBaoCao, x.NgayKetThuc) <= 0,
                //    x => x.DANGKY_CHAMCONG_CHITIET, y => y.HR_NHANVIEN).
                //    Select(x => x.MaNV).Distinct().Count();

                var lstnv = _nhanvienCLviecResponsitory.FindAll(
                    x => x.HR_NHANVIEN.MaBoPhan == item.BoPhan &&
                     (x.HR_NHANVIEN.Status == Status.Active.NullString() || string.Compare(x.HR_NHANVIEN.NgayNghiViec, item.NgayBaoCao) >= 0) &&
                     string.Compare(item.NgayBaoCao, x.BatDau_TheoCa) >= 0 &&
                     string.Compare(item.NgayBaoCao, x.KetThuc_TheoCa) <= 0,
                     x => x.HR_NHANVIEN).ToList();

                var nvGr = lstnv.GroupBy(x => x.Danhmuc_CaLviec).Select(gr => gr);

                foreach (var gr in nvGr)
                {
                    if (item.CaLamViec_Value.Any(x => x.CalamViec == gr.Key))
                    {
                        caLamViec = item.CaLamViec_Value.FirstOrDefault(x => x.CalamViec == gr.Key);
                    }
                    else
                    {
                        caLamViec = new CaLamViecSL()
                        {
                            CalamViec = gr.Key
                        };
                    }

                    TrucTiepGianTiepSL tgianTiep = new TrucTiepGianTiepSL();

                    foreach (var sub in gr)// duyet qua so nguoi là OP, STAFF, STAFF PM
                    {
                        note = "";

                        al = _chamCongDBResponsitory.FindAll(
                                x => x.HR_NHANVIEN.Id == sub.MaNV &&
                                x.DANGKY_CHAMCONG_CHITIET.KyHieuChamCong == "AL" &&
                                string.Compare(item.NgayBaoCao, x.NgayBatDau) >= 0 &&
                                string.Compare(item.NgayBaoCao, x.NgayKetThuc) <= 0,
                                x => x.DANGKY_CHAMCONG_CHITIET, y => y.HR_NHANVIEN).
                                Select(x => x.MaNV).Distinct().Count();

                        note += _chamCongDBResponsitory.FindAll(
                                x => x.HR_NHANVIEN.Id == sub.MaNV &&
                                x.DANGKY_CHAMCONG_CHITIET.KyHieuChamCong == "AL" &&
                                string.Compare(item.NgayBaoCao, x.NgayBatDau) >= 0 &&
                                string.Compare(item.NgayBaoCao, x.NgayKetThuc) <= 0,
                                x => x.DANGKY_CHAMCONG_CHITIET, y => y.HR_NHANVIEN).FirstOrDefault()?.NoiDung.AddString("AL") + "\n";

                        ul = _chamCongDBResponsitory.FindAll(
                                x => x.HR_NHANVIEN.Id == sub.MaNV &&
                                "UL NB B".Contains(x.DANGKY_CHAMCONG_CHITIET.KyHieuChamCong) &&
                                string.Compare(item.NgayBaoCao, x.NgayBatDau) >= 0 &&
                                string.Compare(item.NgayBaoCao, x.NgayKetThuc) <= 0,
                                x => x.DANGKY_CHAMCONG_CHITIET, y => y.HR_NHANVIEN).
                                Select(x => x.MaNV).Distinct().Count();

                        note += _chamCongDBResponsitory.FindAll(
                                x => x.HR_NHANVIEN.Id == sub.MaNV &&
                                "UL".Contains(x.DANGKY_CHAMCONG_CHITIET.KyHieuChamCong) &&
                                string.Compare(item.NgayBaoCao, x.NgayBatDau) >= 0 &&
                                string.Compare(item.NgayBaoCao, x.NgayKetThuc) <= 0,
                                x => x.DANGKY_CHAMCONG_CHITIET, y => y.HR_NHANVIEN).FirstOrDefault()?.NoiDung.AddString("UL") + "\n"

                                + _chamCongDBResponsitory.FindAll(
                                x => x.HR_NHANVIEN.Id == sub.MaNV &&
                                "NB".Contains(x.DANGKY_CHAMCONG_CHITIET.KyHieuChamCong) &&
                                string.Compare(item.NgayBaoCao, x.NgayBatDau) >= 0 &&
                                string.Compare(item.NgayBaoCao, x.NgayKetThuc) <= 0,
                                x => x.DANGKY_CHAMCONG_CHITIET, y => y.HR_NHANVIEN).FirstOrDefault()?.NoiDung.AddString("NB") + "\n"

                                + _chamCongDBResponsitory.FindAll(
                                x => x.HR_NHANVIEN.Id == sub.MaNV &&
                                "B".Contains(x.DANGKY_CHAMCONG_CHITIET.KyHieuChamCong) &&
                                string.Compare(item.NgayBaoCao, x.NgayBatDau) >= 0 &&
                                string.Compare(item.NgayBaoCao, x.NgayKetThuc) <= 0,
                                x => x.DANGKY_CHAMCONG_CHITIET, y => y.HR_NHANVIEN).FirstOrDefault()?.NoiDung.AddString("B") + "\n";

                        nl = _chamCongDBResponsitory.FindAll(
                                x => x.HR_NHANVIEN.Id == sub.MaNV &&
                                x.DANGKY_CHAMCONG_CHITIET.KyHieuChamCong == "NL" &&
                                string.Compare(item.NgayBaoCao, x.NgayBatDau) >= 0 &&
                                string.Compare(item.NgayBaoCao, x.NgayKetThuc) <= 0,
                                x => x.DANGKY_CHAMCONG_CHITIET, y => y.HR_NHANVIEN).
                                Select(x => x.MaNV).Distinct().Count();

                        note += _chamCongDBResponsitory.FindAll(
                                x => x.HR_NHANVIEN.Id == sub.MaNV &&
                                x.DANGKY_CHAMCONG_CHITIET.KyHieuChamCong == "NL" &&
                                string.Compare(item.NgayBaoCao, x.NgayBatDau) >= 0 &&
                                string.Compare(item.NgayBaoCao, x.NgayKetThuc) <= 0,
                                x => x.DANGKY_CHAMCONG_CHITIET, y => y.HR_NHANVIEN).FirstOrDefault()?.NoiDung.AddString("NL") + "\n";

                        l70 = _chamCongDBResponsitory.FindAll(
                                x => x.HR_NHANVIEN.Id == sub.MaNV &&
                                x.DANGKY_CHAMCONG_CHITIET.KyHieuChamCong == "L70" &&
                                string.Compare(item.NgayBaoCao, x.NgayBatDau) >= 0 &&
                                string.Compare(item.NgayBaoCao, x.NgayKetThuc) <= 0,
                                x => x.DANGKY_CHAMCONG_CHITIET, y => y.HR_NHANVIEN).
                                Select(x => x.MaNV).Distinct().Count();

                        note += _chamCongDBResponsitory.FindAll(
                                x => x.HR_NHANVIEN.Id == sub.MaNV &&
                                x.DANGKY_CHAMCONG_CHITIET.KyHieuChamCong == "L70" &&
                                string.Compare(item.NgayBaoCao, x.NgayBatDau) >= 0 &&
                                string.Compare(item.NgayBaoCao, x.NgayKetThuc) <= 0,
                                x => x.DANGKY_CHAMCONG_CHITIET, y => y.HR_NHANVIEN).FirstOrDefault()?.NoiDung.AddString("L70") + "\n";

                        sl = _chamCongDBResponsitory.FindAll(
                               x => x.HR_NHANVIEN.Id == sub.MaNV &&
                               "SL IL".Contains(x.DANGKY_CHAMCONG_CHITIET.KyHieuChamCong) &&
                               string.Compare(item.NgayBaoCao, x.NgayBatDau) >= 0 &&
                               string.Compare(item.NgayBaoCao, x.NgayKetThuc) <= 0,
                               x => x.DANGKY_CHAMCONG_CHITIET, y => y.HR_NHANVIEN).
                               Select(x => x.MaNV).Distinct().Count();

                        note += _chamCongDBResponsitory.FindAll(
                               x => x.HR_NHANVIEN.Id == sub.MaNV &&
                                "SL".Contains(x.DANGKY_CHAMCONG_CHITIET.KyHieuChamCong) &&
                               string.Compare(item.NgayBaoCao, x.NgayBatDau) >= 0 &&
                               string.Compare(item.NgayBaoCao, x.NgayKetThuc) <= 0,
                               x => x.DANGKY_CHAMCONG_CHITIET, y => y.HR_NHANVIEN).FirstOrDefault()?.NoiDung.AddString("SL") + "\n"

                               + _chamCongDBResponsitory.FindAll(
                               x => x.HR_NHANVIEN.Id == sub.MaNV &&
                                "IL".Contains(x.DANGKY_CHAMCONG_CHITIET.KyHieuChamCong) &&
                               string.Compare(item.NgayBaoCao, x.NgayBatDau) >= 0 &&
                               string.Compare(item.NgayBaoCao, x.NgayKetThuc) <= 0,
                               x => x.DANGKY_CHAMCONG_CHITIET, y => y.HR_NHANVIEN).FirstOrDefault()?.NoiDung.AddString("IL") + "\n";

                        ct = _chamCongDBResponsitory.FindAll(
                               x => x.HR_NHANVIEN.Id == sub.MaNV &&
                               x.DANGKY_CHAMCONG_CHITIET.KyHieuChamCong == "CT" &&
                               string.Compare(item.NgayBaoCao, x.NgayBatDau) >= 0 &&
                               string.Compare(item.NgayBaoCao, x.NgayKetThuc) <= 0,
                               x => x.DANGKY_CHAMCONG_CHITIET, y => y.HR_NHANVIEN).
                               Select(x => x.MaNV).Distinct().Count();

                        note += _chamCongDBResponsitory.FindAll(
                               x => x.HR_NHANVIEN.Id == sub.MaNV &&
                               x.DANGKY_CHAMCONG_CHITIET.KyHieuChamCong == "CT" &&
                               string.Compare(item.NgayBaoCao, x.NgayBatDau) >= 0 &&
                               string.Compare(item.NgayBaoCao, x.NgayKetThuc) <= 0,
                               x => x.DANGKY_CHAMCONG_CHITIET, y => y.HR_NHANVIEN).FirstOrDefault()?.NoiDung.AddString("CT") + "\n";

                        hl = _chamCongDBResponsitory.FindAll(
                                x => x.HR_NHANVIEN.Id == sub.MaNV &&
                                x.DANGKY_CHAMCONG_CHITIET.KyHieuChamCong == "HL" &&
                                string.Compare(item.NgayBaoCao, x.NgayBatDau) >= 0 &&
                                string.Compare(item.NgayBaoCao, x.NgayKetThuc) <= 0,
                                x => x.DANGKY_CHAMCONG_CHITIET, y => y.HR_NHANVIEN).
                                Select(x => x.MaNV).Distinct().Count();

                        note += _chamCongDBResponsitory.FindAll(
                                x => x.HR_NHANVIEN.Id == sub.MaNV &&
                                x.DANGKY_CHAMCONG_CHITIET.KyHieuChamCong == "HL" &&
                                string.Compare(item.NgayBaoCao, x.NgayBatDau) >= 0 &&
                                string.Compare(item.NgayBaoCao, x.NgayKetThuc) <= 0,
                                x => x.DANGKY_CHAMCONG_CHITIET, y => y.HR_NHANVIEN).FirstOrDefault()?.NoiDung.AddString("HL") + "\n";

                        nh = _chamCongDBResponsitory.FindAll(
                                x => x.HR_NHANVIEN.Id == sub.MaNV &&
                                x.DANGKY_CHAMCONG_CHITIET.KyHieuChamCong == "NH" &&
                                string.Compare(item.NgayBaoCao, x.NgayBatDau) >= 0 &&
                                string.Compare(item.NgayBaoCao, x.NgayKetThuc) <= 0,
                                x => x.DANGKY_CHAMCONG_CHITIET, y => y.HR_NHANVIEN).
                                Select(x => x.MaNV).Distinct().Count();

                        note += _chamCongDBResponsitory.FindAll(
                                x => x.HR_NHANVIEN.Id == sub.MaNV &&
                                x.DANGKY_CHAMCONG_CHITIET.KyHieuChamCong == "NH" &&
                                string.Compare(item.NgayBaoCao, x.NgayBatDau) >= 0 &&
                                string.Compare(item.NgayBaoCao, x.NgayKetThuc) <= 0,
                                x => x.DANGKY_CHAMCONG_CHITIET, y => y.HR_NHANVIEN).FirstOrDefault()?.NoiDung.AddString("NH") + "\n";

                        el = _chamCongDBResponsitory.FindAll(
                               x => x.HR_NHANVIEN.Id == sub.MaNV &&
                               x.DANGKY_CHAMCONG_CHITIET.KyHieuChamCong == "EL" &&
                               string.Compare(item.NgayBaoCao, x.NgayBatDau) >= 0 &&
                               string.Compare(item.NgayBaoCao, x.NgayKetThuc) <= 0,
                               x => x.DANGKY_CHAMCONG_CHITIET, y => y.HR_NHANVIEN).
                               Select(x => x.MaNV).Distinct().Count();

                        note += _chamCongDBResponsitory.FindAll(
                               x => x.HR_NHANVIEN.Id == sub.MaNV &&
                               x.DANGKY_CHAMCONG_CHITIET.KyHieuChamCong == "EL" &&
                               string.Compare(item.NgayBaoCao, x.NgayBatDau) >= 0 &&
                               string.Compare(item.NgayBaoCao, x.NgayKetThuc) <= 0,
                               x => x.DANGKY_CHAMCONG_CHITIET, y => y.HR_NHANVIEN).FirstOrDefault()?.NoiDung.AddString("EL") + "\n";

                        lc = _chamCongDBResponsitory.FindAll(
                              x => x.HR_NHANVIEN.Id == sub.MaNV &&
                              x.DANGKY_CHAMCONG_CHITIET.KyHieuChamCong == "LC" &&
                              string.Compare(item.NgayBaoCao, x.NgayBatDau) >= 0 &&
                              string.Compare(item.NgayBaoCao, x.NgayKetThuc) <= 0,
                              x => x.DANGKY_CHAMCONG_CHITIET, y => y.HR_NHANVIEN).
                              Select(x => x.MaNV).Distinct().Count();

                        note += _chamCongDBResponsitory.FindAll(
                               x => x.HR_NHANVIEN.Id == sub.MaNV &&
                               x.DANGKY_CHAMCONG_CHITIET.KyHieuChamCong == "LC" &&
                               string.Compare(item.NgayBaoCao, x.NgayBatDau) >= 0 &&
                               string.Compare(item.NgayBaoCao, x.NgayKetThuc) <= 0,
                               x => x.DANGKY_CHAMCONG_CHITIET, y => y.HR_NHANVIEN).FirstOrDefault()?.NoiDung.AddString("LC") + "\n";

                        t = _nhanvienResponsitory.FindAll(x => x.Id == sub.MaNV && x.NgayNghiViec == item.NgayBaoCao).Count();

                        if (t > 0)
                        {
                            note += _nhanvienResponsitory.FindById(sub.MaNV).TenNV + " nghỉ việc.";
                        }

                        if (caLamViec.ThongTins.Any(x => x.TrucTiepGianTiep == sub.HR_NHANVIEN.TrucTiepSX && x.ChucVu == sub.HR_NHANVIEN.ChucVu2))
                        {
                            tgianTiep = caLamViec.ThongTins.FirstOrDefault(x => x.TrucTiepGianTiep == sub.HR_NHANVIEN.TrucTiepSX && x.ChucVu == sub.HR_NHANVIEN.ChucVu2);
                            tgianTiep.TongSoNguoi += 1;
                            tgianTiep.NghiPhep += al;
                            tgianTiep.NghiKhongLuong += ul;
                            tgianTiep.NghiKhongThongBao += nl;
                            tgianTiep.NghiHuongLuong70 += l70;
                            tgianTiep.NghiDacBiet += sl;
                            tgianTiep.DiCongTac += ct;
                            tgianTiep.NghiOm += hl;
                            tgianTiep.NghiViec += t;
                            tgianTiep.NghiLe += nh;
                            tgianTiep.DiMuon += lc;
                            tgianTiep.VeSom += el;

                            if (note.NullString() != "")
                            {
                                tgianTiep.Note += "\n" + note.Trim();
                            }
                        }
                        else
                        {
                            tgianTiep = new TrucTiepGianTiepSL();
                            tgianTiep.TrucTiepGianTiep = sub.HR_NHANVIEN.TrucTiepSX;
                            tgianTiep.ChucVu = sub.HR_NHANVIEN.ChucVu2;
                            tgianTiep.TongSoNguoi = 1;
                            tgianTiep.NghiPhep = al;
                            tgianTiep.NghiKhongLuong = ul;
                            tgianTiep.NghiKhongThongBao = nl;
                            tgianTiep.NghiHuongLuong70 = l70;
                            tgianTiep.NghiDacBiet = sl;
                            tgianTiep.DiCongTac = ct;
                            tgianTiep.NghiOm = hl;
                            tgianTiep.NghiViec = t;
                            tgianTiep.NghiLe = nh;
                            tgianTiep.DiMuon = lc;
                            tgianTiep.VeSom = el;

                            if (note.NullString() != "")
                            {
                                tgianTiep.Note += "\n" + note.Trim();
                            }

                            if (caLamViec.CalamViec == "CN_WHC")
                            {
                                if (sub.HR_NHANVIEN.TrucTiepSX == "TrucTiepSX" && sub.HR_NHANVIEN.ChucVu2 == "OP")
                                {
                                    tgianTiep.order = 0;
                                }
                                else
                                if (sub.HR_NHANVIEN.TrucTiepSX == "TrucTiepSX" && sub.HR_NHANVIEN.ChucVu2 == "STAFF")
                                {
                                    tgianTiep.order = 1;
                                }
                                else
                                if (sub.HR_NHANVIEN.TrucTiepSX == "GianTiepSX" && sub.HR_NHANVIEN.ChucVu2 == "STAFF PM")
                                {
                                    tgianTiep.order = 2;
                                }
                            }
                            else
                            if (caLamViec.CalamViec == "CD_WHC")
                            {
                                if (sub.HR_NHANVIEN.TrucTiepSX == "TrucTiepSX" && sub.HR_NHANVIEN.ChucVu2 == "OP")
                                {
                                    tgianTiep.order = 3;
                                }

                                if (sub.HR_NHANVIEN.TrucTiepSX == "TrucTiepSX" && sub.HR_NHANVIEN.ChucVu2 == "STAFF")
                                {
                                    tgianTiep.order = 4;
                                }
                            }

                            caLamViec.ThongTins.Add(tgianTiep);

                            caLamViec.ThongTins.Sort((x, y) => x.order - y.order);
                        }
                    }

                    if (!item.CaLamViec_Value.Any(x => x.CalamViec == gr.Key))
                    {
                        item.CaLamViec_Value.Add(caLamViec);
                    }
                }
            }

            return dailyViewModels;
        }

        private Dictionary<int, double> blockTime = new Dictionary<int, double>()
        {
            { 0,0},{10,0.2}, {20,0.3}, {30,0.5},{40,0.7},{50,0.8},{60,1}
        };

        public double SimpleValueOT(double value)
        {
            int pnguyen = (int)value;
            double pthaphan = value - pnguyen;

            int minute = (int)(pthaphan * 6);

            double newVal = pthaphan;
            if (blockTime.ContainsKey(minute * 10))
            {
                newVal = blockTime[minute * 10];
            }

            return pnguyen + newVal;
        }

        public double Block30mValueOT(double value)
        {
            Dictionary<int, double> blockTime = new Dictionary<int, double>()
            {
                { 0,0},{10,0},
                {20,0},{30,0.5},
                {40,0.5},{50,0.5},
                {60,1}
            };

            if (value < 0.5)
                return 0;

            int pnguyen = (int)value;
            double pthaphan = value - pnguyen;

            int minute = (int)(pthaphan * 6);

            double newVal = pthaphan;
            if (blockTime.ContainsKey(minute * 10))
            {
                newVal = blockTime[minute * 10];
            }

            return pnguyen + newVal;
        }
    }

    public class ResultItemModelBC
    {
        public ResultItemModelBC(List<ChamCongDataViewModel> chamCongs, List<DeNghiLamThemGioModel> denghis)
        {
            chamCongDataViewModels = chamCongs;
            deNghiLamThemGioModels = denghis;
        }
        public List<ChamCongDataViewModel> chamCongDataViewModels { get; set; }
        public List<DeNghiLamThemGioModel> deNghiLamThemGioModels { get; set; }
    }
}
