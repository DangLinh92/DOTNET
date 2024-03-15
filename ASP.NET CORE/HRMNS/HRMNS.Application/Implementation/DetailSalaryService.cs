using AutoMapper;
using HRMNS.Application.Interfaces;
using HRMNS.Application.ViewModels.HR;
using HRMNS.Data.EF.Extensions;
using HRMNS.Data.Entities;
using HRMNS.Data.Entities.Payroll;
using HRMNS.Utilities.Common;
using HRMNS.Utilities.Constants;
using HRMNS.Utilities.Dtos;
using HRMS.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace HRMNS.Application.Implementation
{
    public class DetailSalaryService : BaseService, IDetailSalaryService
    {
        private IUnitOfWork _unitOfWork;
        IRespository<HR_NGAY_CHOT_CONG, int> _ngayChotCongRepository;
        IRespository<NHANVIEN_INFOR_EX, int> _nhanvienInfoExRepository;
        IRespository<BANG_CONG_EXTENTION, int> _bangCongExRepository;
        IRespository<CONGDOAN_NOT_JOIN, int> _congDoanRepository;
        IRespository<HR_PHEP_NAM, int> _phepNamRepository;
        IRespository<HR_SALARY_PHATSINH, int> _luongPhatSinhRepository;
        IRespository<HR_SALARY, int> _salaryRepository;
        //IPayrollRespository<HR_SALARY_PR, int> _salarySqliteRepository;
        IRespository<DC_CHAM_CONG, int> _dieuchinhCongRepository;
        IRespository<HR_NHANVIEN, string> _nhanVienRepository;
        IRespository<BANGLUONGCHITIET_HISTORY, int> _bangluongChiTietHistoryRepository;
        IRespository<HR_CHUCDANH, string> _chucDanhRepository;
        IRespository<PHUCAP_DOC_HAI, int> _phucapdochaiRepository;
        IRespository<HR_SALARY_GRADE, string> _gradeRepository;
        IRespository<HR_BHXH, string> _BHXHRepository;
        IRespository<HR_THAISAN_CONNHO, int> _thaisanRepository;
        IRespository<HOTRO_SINH_LY, int> _hotroSinhLyRepository;
        IRespository<HR_THANHTOAN_NGHIVIEC, Guid> _thanhToanNghiViecRepository;

        private IPayrollUnitOfWork _payrollUnitOfWork;
        private readonly IMapper _mapper;

        public DetailSalaryService(
            IRespository<HR_NGAY_CHOT_CONG, int> ngayChotCongRepository,
            IRespository<NHANVIEN_INFOR_EX, int> nhanvienInfoExRepository,
            IRespository<BANG_CONG_EXTENTION, int> bangCongExRepository,
            IRespository<CONGDOAN_NOT_JOIN, int> congDoanRepository,
            IRespository<HR_PHEP_NAM, int> phepNamRepository,
            IRespository<HR_SALARY_PHATSINH, int> luongPhatSinhRepository,
            IRespository<HR_SALARY, int> salaryRepository,
            IRespository<DC_CHAM_CONG, int> dieuchinhCongRepository,
            IRespository<HR_NHANVIEN, string> nhanVienRepository,
            IRespository<BANGLUONGCHITIET_HISTORY, int> bangluongChiTietRepository,
            IRespository<HR_CHUCDANH, string> chucDanhRepository,
            IRespository<PHUCAP_DOC_HAI, int> phucapdochaiRepository,
            IRespository<HR_SALARY_GRADE, string> gradeRepository,
            IRespository<HR_BHXH, string> BHXHRepository,
            IRespository<HR_THAISAN_CONNHO, int> thaisanRepository,
            IRespository<HOTRO_SINH_LY, int> hotroSinhLyRepository,
            IRespository<HR_THANHTOAN_NGHIVIEC, Guid> thanhToanNghiViecRepository,
            IMapper mapper,
            IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _ngayChotCongRepository = ngayChotCongRepository;
            _nhanvienInfoExRepository = nhanvienInfoExRepository;
            _bangCongExRepository = bangCongExRepository;
            _congDoanRepository = congDoanRepository;
            _phepNamRepository = phepNamRepository;
            _luongPhatSinhRepository = luongPhatSinhRepository;
            _salaryRepository = salaryRepository;
            _dieuchinhCongRepository = dieuchinhCongRepository;
            _nhanVienRepository = nhanVienRepository;
            _bangluongChiTietHistoryRepository = bangluongChiTietRepository;
            _chucDanhRepository = chucDanhRepository;
            _phucapdochaiRepository = phucapdochaiRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _gradeRepository = gradeRepository;
            _BHXHRepository = BHXHRepository;
            _thaisanRepository = thaisanRepository;
            _hotroSinhLyRepository = hotroSinhLyRepository;
            _thanhToanNghiViecRepository = thanhToanNghiViecRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public List<BangLuongChiTietViewModel> GetBangLuongChiTiet(string thangNam, string chedo)
        {
            List<BangLuongChiTietViewModel> bangLuongChiTiets = new List<BangLuongChiTietViewModel>();
            try
            {
                List<BANG_CONG_EXTENTION> lstBangCong = _bangCongExRepository.FindAll(x => x.ThangNam == thangNam, x => x.HR_NHANVIEN, x => x.HR_NHANVIEN.HR_CHUCDANH).ToList();

                List<BANG_CONG_EXTENTION> bangcongTmp = new List<BANG_CONG_EXTENTION>();
                if (chedo.NullString() == "NghiViec")
                {
                    var ttnghiviec = _thanhToanNghiViecRepository.FindAll(x => x.IsPay == true && x.IsPayed == false && x.Month == thangNam && x.HR_NHANVIEN.NgayNghiViec != "" && x.HR_NHANVIEN.NgayNghiViec != null,
                                                                          x => x.HR_NHANVIEN).Select(x => x.MaNV);

                    if (ttnghiviec.Count() > 0)
                        bangcongTmp = lstBangCong.Where(x => ttnghiviec.Contains(x.MaNV)).ToList();

                    var ttnghiviecOld = _thanhToanNghiViecRepository.FindAll(x => x.IsPayed == true && x.Month == thangNam && x.HR_NHANVIEN.NgayNghiViec != "" && x.HR_NHANVIEN.NgayNghiViec != null,
                                                                       x => x.HR_NHANVIEN).Select(x => x.MaNV);

                    if (ttnghiviecOld.Count() > 0)
                        bangcongTmp.AddRange(lstBangCong.Where(x => ttnghiviecOld.Contains(x.MaNV)).ToList());

                    lstBangCong = bangcongTmp;
                }
                else if (chedo.NullString() == "LamViec")
                {
                    var ttnghiviec = _thanhToanNghiViecRepository.FindAll(x => x.IsPayed == true && x.HR_NHANVIEN.NgayNghiViec != "" && x.HR_NHANVIEN.NgayNghiViec != null,
                                                                          x => x.HR_NHANVIEN).Select(x => x.MaNV);
                    lstBangCong = lstBangCong.Where(x => !ttnghiviec.Contains(x.MaNV)).ToList();
                }

                BangLuongChiTietViewModel luong;
                HR_SALARY salary;
                List<HR_SALARY_PHATSINH> phatsinhs;
                // DC_CHAM_CONG dieuChinhCong;
                double dieuChinhCong;
                HR_PHEP_NAM phepnam;
                CONGDOAN_NOT_JOIN condoan;
                NHANVIEN_INFOR_EX nhanvienEx;
                int songaylamviec = 0;
                int lastDay = DateTime.DaysInMonth(DateTime.Parse(thangNam).Year, DateTime.Parse(thangNam).Month);//DateTime.Parse(thangNam).Day;
                string endOfMonth = DateTime.Parse(thangNam.Substring(0, 7) + "-01").AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd");

                _salaryRepository.ExecProceduce2("PKG_BUSINESS@UPDATE_BHXH_DAILY", new Dictionary<string, string>());

                List<HR_BHXH> BHXH = _BHXHRepository.FindAll(x => x.ThangThamGia == thangNam).ToList();

                for (int i = 1; i <= lastDay; i++)
                {
                    DateTime d = new DateTime(DateTime.Parse(thangNam).Year, DateTime.Parse(thangNam).Month, i);
                    //Compare date with sunday
                    if (d.DayOfWeek != DayOfWeek.Sunday)
                    {
                        songaylamviec += 1;
                    }
                }

                int thamnien = 0;
                int songaynghiThaisan = 0;

                foreach (var item in lstBangCong)
                {
                    thamnien = 0;
                    luong = new BangLuongChiTietViewModel()
                    {
                        MaNV = item.MaNV,
                        TenNV = item.HR_NHANVIEN.TenNV,
                        NgayVao = item.HR_NHANVIEN.NgayVao,
                        NgayNghiViec = item.HR_NHANVIEN.NgayNghiViec,
                        ThangNam = thangNam,
                        BoPhan = item.HR_NHANVIEN.MaBoPhan,
                        ChucVu = item.HR_NHANVIEN.HR_CHUCDANH.TenChucDanh
                    };

                    try
                    {
                        salary = _salaryRepository.FindSingle(x => x.MaNV == item.MaNV, x => x.HR_NHANVIEN);
                        nhanvienEx = _nhanvienInfoExRepository.FindAll(x => x.MaNV == item.MaNV && x.Year.ToString().CompareTo(thangNam.Substring(0, 4)) == 0, x => x.HR_NHANVIEN).FirstOrDefault();

                        dieuChinhCong = _dieuchinhCongRepository.FindAll(x => x.MaNV == item.MaNV && x.ChiTraVaoLuongThang2.Substring(0, 7).CompareTo(thangNam.Substring(0, 7)) == 0).Select(x => x.TongSoTien).Sum(x => x.Value);

                        phatsinhs = _luongPhatSinhRepository.FindAll(x => x.MaNV == item.MaNV && x.FromTime != null && x.ToTime != null &&
                                                                     DateTime.Parse(thangNam).ToString("yyyyMM").CompareTo(x.FromTime.Replace("-", "").Substring(0, 6)) >= 0 &&
                                                                    DateTime.Parse(thangNam).ToString("yyyyMM").CompareTo(x.ToTime.Replace("-", "").Substring(0, 6)) <= 0,
                                       y => y.HR_SALARY_DANHMUC_PHATSINH, z => z.HR_NHANVIEN).ToList();

                        phepnam = _phepNamRepository.FindSingle(x => x.Year == DateTime.Parse(thangNam).Year && x.MaNhanVien == item.MaNV);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }

                    luong.PositionAllowance = _chucDanhRepository.FindById(item.HR_NHANVIEN.MaChucDanh).PhuCap;

                    if (nhanvienEx != null)
                    {
                        luong.HieuLucCapBac = "";
                        luong.MaBoPhan = nhanvienEx.MaBoPhanEx;
                        luong.Grade = nhanvienEx.Grade;

                        if (_nhanVienRepository.FindById(item.MaNV, x => x.HR_BO_PHAN_DETAIL) != null)
                        {
                            luong.GioiTinh = _nhanVienRepository.FindById(item.MaNV).GioiTinh;

                            luong.BoPhan = _nhanVienRepository.FindById(item.MaNV, x => x.HR_BO_PHAN_DETAIL).HR_BO_PHAN_DETAIL.MaBoPhan;

                            if (luong.GioiTinh == "Female")
                            {
                                if (_hotroSinhLyRepository.FindSingle(x => x.MaNV == item.MaNV && x.Month.Contains(thangNam.Substring(0, 7))) != null)
                                {
                                    luong.BauThaiSan = "";
                                    luong.ThoiGianChuaNghi = _hotroSinhLyRepository.FindSingle(x => x.MaNV == item.MaNV && x.Month.Contains(thangNam.Substring(0, 7))).ThoiGianChuaNghi;
                                }
                                else
                                {
                                    luong.BauThaiSan = "o";
                                    luong.ThoiGianChuaNghi = 0;
                                }
                            }

                            if (luong.Grade == "M1-1" || luong.Grade == "P2-1")
                            {
                                thamnien = EachDay.GetMonthDifference(DateTime.Parse(thangNam), DateTime.Parse(_nhanVienRepository.FindById(item.MaNV).NgayVao));

                                // IF(DG3>=120,1250000
                                if (thamnien >= 120)
                                {
                                    luong.SeniorityAllowance = 1250000;
                                }
                                // IF(AND(DG3<=119,DG3>=108),1150000,
                                else if (thamnien >= 108 && thamnien <= 119)
                                {
                                    luong.SeniorityAllowance = 1150000;
                                }
                                // IF(AND(DG3<=107,DG3>=96),1050000,
                                else if (thamnien >= 96 && thamnien <= 107)
                                {
                                    luong.SeniorityAllowance = 1050000;
                                }
                                // IF(AND(DG3<=95,DG3>=84),950000,
                                else if (thamnien >= 84 && thamnien <= 95)
                                {
                                    luong.SeniorityAllowance = 950000;
                                }
                                // IF(AND(DG3<=83,DG3>=72),850000,
                                else if (thamnien >= 72 && thamnien <= 83)
                                {
                                    luong.SeniorityAllowance = 850000;
                                }
                                // IF(AND(DG3<=71,DG3>=60),750000,
                                else if (thamnien >= 60 && thamnien <= 71)
                                {
                                    luong.SeniorityAllowance = 750000;
                                }
                                // IF(AND(DG3<=59,DG3>=48),650000
                                else if (thamnien >= 48 && thamnien <= 59)
                                {
                                    luong.SeniorityAllowance = 650000;
                                }
                                // IF(AND(DG3<=47,DG3>=36),550000,
                                else if (thamnien >= 36 && thamnien <= 47)
                                {
                                    luong.SeniorityAllowance = 550000;
                                }
                                // IF(AND(DG3<=35,DG3>=24),450000,
                                else if (thamnien >= 24 && thamnien <= 35)
                                {
                                    luong.SeniorityAllowance = 450000;
                                }
                                // IF(AND(DG3<=23,DG3>=18),350000,
                                else if (thamnien >= 18 && thamnien <= 23)
                                {
                                    luong.SeniorityAllowance = 350000;
                                }
                                // IF(AND(DG3<=17,DG3>=12),250000,
                                else if (thamnien >= 12 && thamnien <= 17)
                                {
                                    luong.SeniorityAllowance = 250000;
                                }
                                // IF(AND(DG3<=11,DG3>=2),150000,
                                else if (thamnien >= 2 && thamnien <= 11)
                                {
                                    luong.SeniorityAllowance = 150000;
                                }
                            }
                        }
                        else
                        {
                            luong.BoPhan = nhanvienEx.HR_NHANVIEN.MaBoPhan;
                        }
                    }

                    HR_SALARY_GRADE grade = nhanvienEx != null ? _gradeRepository.FindAll(x => x.Id == nhanvienEx.Grade).FirstOrDefault() : null;

                    if (salary != null)
                    {
                        luong.DoiTuongPhuCapDocHai = salary.DoiTuongPhuCapDocHai.NullString().ToLower();
                        luong.BasicSalary = grade != null ? grade.BasicSalary : 0; //(double)salary.BasicSalary;
                        luong.LivingAllowance = grade != null ? grade.LivingAllowance : 0;//(double)salary.LivingAllowance;
                        luong.AbilityAllowance = (double)salary.AbilityAllowance;

                        if (salary.DoiTuongPhuCapDocHai.NullString().ToLower() == CommonConstants.X)
                        {
                            luong.HarmfulAllowance = _phucapdochaiRepository.FindSingle(x => x.BoPhan == item.HR_NHANVIEN.MaBoPhan).PhuCap * luong.BasicSalary;
                        }
                        else
                        {
                            luong.HarmfulAllowance = 0;
                        }
                    }

                    luong.DailySalary = Math.Round((luong.BasicSalary + luong.LivingAllowance + luong.PositionAllowance + luong.AbilityAllowance + luong.SeniorityAllowance + luong.HarmfulAllowance) / songaylamviec, 0);

                    luong.TongNgayCongThucTe = item.TP;
                    luong.NgayCongThuViecBanNgay = item.PD;
                    luong.NgayCongThuViecBanDem = item.PN;
                    luong.NgayCongChinhThucBanNgay = item.DS;
                    luong.NgayCongChinhThucBanDem = item.NS;
                    luong.NghiViecCoLuong = item.NCL;

                    luong.GioLamThemTrongTV_150 = item.OT_TV_15;
                    luong.GioLamThemTrongTV_200 = item.OT_TV_20;
                    luong.GioLamThemTrongTV_210 = item.OT_TV_21;
                    luong.GioLamThemTrongTV_270 = item.OT_TV_27;
                    luong.GioLamThemTrongTV_300 = item.OT_TV_30;
                    luong.GioLamThemTrongTV_390 = item.OT_TV_39;

                    luong.GioLamThemTrongCT_150 = item.OT_CT_15;
                    luong.GioLamThemTrongCT_200 = item.OT_CT_20;
                    luong.GioLamThemTrongCT_210 = item.OT_CT_21;
                    luong.GioLamThemTrongCT_270 = item.OT_CT_27;
                    luong.GioLamThemTrongCT_300 = item.OT_CT_30;
                    luong.GioLamThemTrongCT_390 = item.OT_CT_39;

                    luong.SoNgayLamCaDemTruocLe_TV = item.PH;
                    luong.SoNgayLamCaDemTruocLe_CT = item.BH;

                    luong.SoNgayLamCaDem_TV = item.CD_TV;
                    luong.SoNgayLamCaDem_CT = item.CD_CT;

                    luong.HoTroThoiGianLamViecTV_150 = item.TV_150;
                    luong.HoTroThoiGianLamViecTV_200_NT = item.TV_D_NT_200;
                    luong.HoTroThoiGianLamViecTV_200_CN = item.TV_CN_200;
                    luong.HoTroThoiGianLamViecTV_270 = item.TV_D_CN_270;
                    luong.HoTroThoiGianLamViecTV_300 = item.TV_NL_300;
                    luong.HoTroThoiGianLamViecTV_390 = item.TV_D_NL_390;

                    luong.HoTroThoiGianLamViecCT_150 = item.CT_150;
                    luong.HoTroThoiGianLamViecCT_200_NT = item.CT_D_NT_200;
                    luong.HoTroThoiGianLamViecCT_200_CN = item.CT_CN_200;
                    luong.HoTroThoiGianLamViecCT_270 = item.CT_D_CN_270;
                    luong.HoTroThoiGianLamViecCT_300 = item.CT_NL_300;
                    luong.HoTroThoiGianLamViecCT_390 = item.CT_D_NL_390;

                    luong.HoTroNgayThanhLapCty_CaNgayTV = item.PMD;
                    luong.HoTroNgayThanhLapCty_CaNgayCT = item.MD;
                    luong.HoTroNgayThanhLapCty_CaDemTV_TruocLe = item.PM;
                    luong.HoTroNgayThanhLapCty_CaDemCT_TruocLe = item.BM;

                    luong.NghiKhamThai = item.KT;
                    luong.NghiViecKhongThongBao = item.NL;
                    luong.SoNgayNghiBu_AL30 = item.AL30;
                    luong.SoNgayNghiBu_NB = item.NB;

                    luong.NghiKhongLuong = item.TUP;
                    luong.Probation_Late_Come_Early_Leave_Time = item.P_TV;
                    luong.Official_Late_Come_Early_Leave_Time = item.O_CT;

                    luong.HoTroPCCC_CoSo = (double)salary.PCCC_CoSo;
                    luong.HoTroAT_SinhVien = (double)salary.HoTroATVS_SinhVien;

                    if (BHXH.FirstOrDefault(x => x.MaNV == item.MaNV) != null)
                    {
                        luong.ThuocDoiTuong_BHXH = BHXH.FirstOrDefault(x => x.MaNV == item.MaNV).PhanLoai == "OK" ? "x" : "o"; //salary.ThuocDoiTuongBaoHiemXH;
                    }

                    if (phatsinhs.FirstOrDefault(x => x.HR_SALARY_DANHMUC_PHATSINH.KeyDanhMuc == "PCTT") != null)
                    {
                        luong.TruQuyPhongChongThienTai = (double)phatsinhs.FirstOrDefault(x => x.HR_SALARY_DANHMUC_PHATSINH.KeyDanhMuc == "PCTT").SoTien;
                    }
                    else
                    {
                        luong.TruQuyPhongChongThienTai = 0;
                    }

                    if (phatsinhs.FirstOrDefault(x => x.HR_SALARY_DANHMUC_PHATSINH.KeyDanhMuc == "THUONG") != null)
                    {
                        luong.Thuong = (double)phatsinhs.FirstOrDefault(x => x.HR_SALARY_DANHMUC_PHATSINH.KeyDanhMuc == "THUONG").SoTien;
                    }
                    else
                    {
                        luong.Thuong = 0;
                    }

                    luong.SoNguoiPhuThuoc = salary.SoNguoiPhuThuoc;
                    luong.Note = salary.Note;
                    luong.InsentiveStandard = (grade != null ? (decimal)grade.IncentiveStandard : 0) + salary.IncentiveLanguage + salary.IncentiveTechnical + salary.IncentiveOther;

                    if (DateTime.Parse(thangNam).ToString("yyyyMM").CompareTo(DateTime.Parse(thangNam).Year + "06") <= 0)
                    {
                        luong.DanhGia = salary.IncentiveSixMonth1;
                    }
                    else
                    {
                        luong.DanhGia = salary.IncentiveSixMonth2;
                    }
                    luong.HoTroCongDoan = salary.HoTroCongDoan;
                    luong.SoTK = salary.HR_NHANVIEN.SoTaiKhoanNH;

                    condoan = _congDoanRepository.FindAll(x => x.MaNV == item.MaNV).ToList().LastOrDefault();
                    if (condoan != null)
                    {
                        if (condoan.NgayBatDau.Value.ToString("yyyyMM").CompareTo(DateTime.Parse(thangNam).ToString("yyyyMM")) <= 0)
                        {
                            luong.DoiTuongThamGiaCD = "o";
                        }
                        else
                        {
                            luong.DoiTuongThamGiaCD = "x";
                        }
                    }
                    else
                    {
                        if (item.MaNV == "H1608013")
                        {
                            var x = 0;
                        }

                        HR_THAISAN_CONNHO thaisan = _thaisanRepository.FindAll(x => x.MaNV == item.MaNV && x.CheDoThaiSan == "ThaiSan").OrderByDescending(x => x.FromDate).FirstOrDefault();
                        songaynghiThaisan = 0;

                        if (thaisan != null)
                        {
                            if (thaisan.FromDate.Substring(0, 7) == thangNam.Substring(0, 7))
                            {
                                songaynghiThaisan = EachDay.GetWorkingDay(DateTime.Parse(thaisan.FromDate), DateTime.Parse(thangNam).AddMonths(1).AddDays(-1));
                            }
                            else if (thaisan.ToDate.Substring(0, 7) == thangNam.Substring(0, 7))
                            {
                                songaynghiThaisan = EachDay.GetWorkingDay(DateTime.Parse(thangNam.Substring(0, 7) + "-01"), DateTime.Parse(thaisan.ToDate));
                            }
                        }

                        //Người mới vào công ty: Vào từ ngày 1 đến ngày 15 của tháng sẽ trừ đoàn phí công đoàn của tháng đó luôn, sau ngày 15 sẽ trừ phí bắt đầu từ tháng sau.
                        // 2. Số ngày nghỉ không hưởng lương > 14 ngày , 3.Nghỉ thai sản > 14 ngày
                        if (songaynghiThaisan > 14 || item.TUP > 14 ||
                            _nhanVienRepository.FindById(item.MaNV).NgayVao.CompareTo(thangNam.Substring(0, 7) + "-15") > 0 ||
                            (_nhanVienRepository.FindById(item.MaNV).NgayNghiViec.NullString() != "" &&
                            _nhanVienRepository.FindById(item.MaNV).NgayNghiViec.NullString().CompareTo(thangNam.Substring(0, 7) + "-15") <= 0))
                        {
                            luong.DoiTuongThamGiaCD = "o";
                        }
                        else
                        {
                            luong.DoiTuongThamGiaCD = "x"; // có tham gia
                        }

                        // TH đặc biệt có ngoại lệ
                        foreach (var cd in CommonConstants.THAM_GIA_CD_EX)
                        {
                            if (cd.Contains(item.MaNV) && cd.Contains(thangNam))
                            {
                                luong.DoiTuongThamGiaCD = "x";
                            }
                        }
                    }

                    luong.DoiTuongTruyThuBHYT = salary.DoiTuongTruyThuBHYT;

                    if (songaynghiThaisan >= songaylamviec)
                    {
                        luong.SoConNho = 0;
                    }
                    else
                    {
                        luong.SoConNho = salary.SoConNho;
                    }
                    
                    luong.SoNgayNghi70 = item.L160; // L160
                    luong.DieuChinhCong_Total = dieuChinhCong;//!= null ? (double)dieuChinhCong?.TongSoTien : 0;

                    if (item.HR_NHANVIEN.NgayNghiViec.NullString() != "" &&
                        (item.HR_NHANVIEN.NgayNghiViec.NullString().Substring(0, 7) == thangNam.Substring(0, 7) ||
                        DateTime.Parse(thangNam).AddMonths(1).ToString("yyyy-MM") + "-01" == item.HR_NHANVIEN.NgayNghiViec.NullString())) // nghỉ việc đầu tháng
                    {
                        luong.TraTienPhepNam_Total = phepnam != null ? (double)phepnam.SoTienChiTra : 0;
                    }

                    if (phatsinhs.FirstOrDefault(x => x.HR_SALARY_DANHMUC_PHATSINH.KeyDanhMuc == "TIEN_GIOI_THIEU") != null)
                    {
                        luong.TT_Tien_GioiThieu = (double)phatsinhs.FirstOrDefault(x => x.HR_SALARY_DANHMUC_PHATSINH.KeyDanhMuc == "TIEN_GIOI_THIEU")?.SoTien;
                    }
                    else
                    {
                        luong.TT_Tien_GioiThieu = 0;
                    }

                    bangLuongChiTiets.Add(luong);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return bangLuongChiTiets;
        }


        public void ChotBangLuong(string time, List<BangLuongChiTietViewModel> data)
        {
            List<BANGLUONGCHITIET_HISTORY> lstData = _bangluongChiTietHistoryRepository.FindAll(x => x.ThangNam == time).ToList();
            BANGLUONGCHITIET_HISTORY en;
            List<BANGLUONGCHITIET_HISTORY> lstUpdate = new List<BANGLUONGCHITIET_HISTORY>();
            List<BANGLUONGCHITIET_HISTORY> lstAdd = new List<BANGLUONGCHITIET_HISTORY>();
            List<HR_SALARY> lstSalary = new List<HR_SALARY>();
            foreach (BangLuongChiTietViewModel item in data)
            {
                if (lstData.Any(x => x.MaNV == item.MaNV))
                {
                    en = lstData.FirstOrDefault(x => x.MaNV == item.MaNV);
                    en.CopyPropertiesFrom(item, new List<string>() { });

                    lstUpdate.Add(en);
                }
                else
                {
                    en = new BANGLUONGCHITIET_HISTORY();
                    en.CopyPropertiesFrom(item, new List<string>() { });
                    lstAdd.Add(en);
                }

                HR_SALARY salary = _salaryRepository.FindAll(x => x.MaNV == item.MaNV).FirstOrDefault();
                if (salary != null)
                {
                    salary.SeniorityAllowance = (decimal)item.SeniorityAllowance;
                    salary.DoiTuongTruyThuBHYT = "o";
                    lstSalary.Add(salary);
                }
            }

            _bangluongChiTietHistoryRepository.AddRange(lstAdd);
            _bangluongChiTietHistoryRepository.UpdateRange(lstUpdate);
            _salaryRepository.UpdateRange(lstSalary);
            _unitOfWork.Commit();
        }

        public List<BangLuongChiTietViewModel> GetHistoryBangLuongChiTiet(string thangNam, string chedo)
        {
            var ttnghiviec = _thanhToanNghiViecRepository.FindAll(x => x.IsPayed == true && x.Month == thangNam && x.HR_NHANVIEN.NgayNghiViec != "" && x.HR_NHANVIEN.NgayNghiViec != null,
                                                                        x => x.HR_NHANVIEN).Select(x => x.MaNV);

            List<BangLuongChiTietViewModel> result = new List<BangLuongChiTietViewModel>();
            var lstLuongChitiet = _bangluongChiTietHistoryRepository.FindAll(x => x.ThangNam == thangNam);
            BangLuongChiTietViewModel model;
            foreach (var item in lstLuongChitiet)
            {
                model = new BangLuongChiTietViewModel();
                model.CopyPropertiesFrom(item, new List<string>() { "Id" });
                result.Add(model);
            }

            if (chedo == "NghiViec")
            {
                result = result.Where(x => ttnghiviec.Contains(x.MaNV)).ToList();
            }
            else if (chedo == "LamViec")
            {
                result = result.Where(x => !ttnghiviec.Contains(x.MaNV)).ToList();
            }

            return result;
        }

        public ResultDB ImportExcel(string filePath, out List<HR_SALARY> lstUpdate)
        {
            lstUpdate = new List<HR_SALARY>();
            ResultDB resultDB = new ResultDB();
            return resultDB;
            //lstUpdate = new List<HR_SALARY>();
            //ResultDB resultDB = new ResultDB();
            //try
            //{
            //    using (var packet = new ExcelPackage(new System.IO.FileInfo(filePath)))
            //    {
            //        ExcelWorksheet worksheet = packet.Workbook.Worksheets[1];
            //        HR_SALARY salary;
            //        HR_NHANVIEN nv;
            //        string manv;
            //        for (int i = worksheet.Dimension.Start.Row + 1; i <= worksheet.Dimension.End.Row; i++)
            //        {
            //            manv = worksheet.Cells[i, 1].Text.NullString();
            //            nv = _nhanVienRepository.FindById(manv);

            //            if (nv == null)
            //            {
            //                break;
            //            }

            //            salary = new HR_SALARY()
            //            {
            //                MaNV = manv
            //            };

            //            if (worksheet.Cells[i, 3].Text.NullString() != "")
            //            {
            //                salary.BasicSalary = decimal.Parse(worksheet.Cells[i, 3].Text.IfNullIsZero());
            //            }

            //            if (worksheet.Cells[i, 4].Text.NullString() != "")
            //            {
            //                salary.LivingAllowance = decimal.Parse(worksheet.Cells[i, 4].Text.IfNullIsZero());
            //            }

            //            if (worksheet.Cells[i, 5].Text.NullString() != "")
            //            {
            //                salary.AbilityAllowance = decimal.Parse(worksheet.Cells[i, 5].Text.IfNullIsZero());
            //            }
            //            lstUpdate.Add(salary);
            //        }

            //        resultDB.ReturnInt = 0;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    resultDB.ReturnInt = -1;
            //    resultDB.ReturnString = ex.Message;
            //}
            //return resultDB;
        }

        public void XacNhanChiTra(List<string> lstMaNV)
        {
            var lstChiTra = _thanhToanNghiViecRepository.FindAll().ToList();
            foreach (var item in lstChiTra)
            {
                if (lstMaNV.Contains(item.MaNV))
                {
                    item.IsPayed = true;
                    item.UserModified = GetUserId();
                }
            }

            _thanhToanNghiViecRepository.UpdateRange(lstChiTra);
            _unitOfWork.Commit();
        }
    }
}
