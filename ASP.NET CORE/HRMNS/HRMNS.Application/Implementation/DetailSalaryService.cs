using AutoMapper;
using HRMNS.Application.Interfaces;
using HRMNS.Application.ViewModels.HR;
using HRMNS.Data.EF.Extensions;
using HRMNS.Data.Entities;
using HRMNS.Data.Entities.Payroll;
using HRMNS.Utilities.Constants;
using HRMNS.Utilities.Dtos;
using HRMS.Infrastructure.Interfaces;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
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
        IPayrollRespository<HR_SALARY_PR, int> _salarySqliteRepository;
        IRespository<DC_CHAM_CONG, int> _dieuchinhCongRepository;
        IRespository<HR_NHANVIEN, string> _nhanVienRepository;
        IPayrollRespository<BANGLUONGCHITIET_HISTORY_PR, int> _bangluongChiTietHistoryRepository;
        IRespository<HR_CHUCDANH, string> _chucDanhRepository;
        IRespository<PHUCAP_DOC_HAI, int> _phucapdochaiRepository;
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
            IPayrollRespository<BANGLUONGCHITIET_HISTORY_PR, int> bangluongChiTietRepository,
            IRespository<HR_CHUCDANH, string> chucDanhRepository,
            IRespository<PHUCAP_DOC_HAI, int> phucapdochaiRepository,
            IMapper mapper,
            IUnitOfWork unitOfWork,
            IPayrollUnitOfWork payrollUnitOfWork,
            IPayrollRespository<HR_SALARY_PR, int> salarySqliteRepository)
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
            _payrollUnitOfWork = payrollUnitOfWork;
            _salarySqliteRepository = salarySqliteRepository;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public List<BangLuongChiTietViewModel> GetBangLuongChiTiet(string thangNam)
        {
            List<BangLuongChiTietViewModel> bangLuongChiTiets = new List<BangLuongChiTietViewModel>();
            try
            {
                var lstBangCong = _bangCongExRepository.FindAll(x => x.ThangNam == thangNam, x => x.HR_NHANVIEN, x => x.HR_NHANVIEN.HR_CHUCDANH).ToList();
                BangLuongChiTietViewModel luong;
                HR_SALARY salary;
                HR_SALARY_PR salaryPR;
                List<HR_SALARY_PHATSINH> phatsinhs;
                DC_CHAM_CONG dieuChinhCong;
                HR_PHEP_NAM phepnam;
                CONGDOAN_NOT_JOIN condoan;
                NHANVIEN_INFOR_EX nhanvienEx;
                foreach (var item in lstBangCong)
                {
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
                    salary = _salaryRepository.FindSingle(x => x.MaNV == item.MaNV, x => x.HR_NHANVIEN);
                    nhanvienEx = _nhanvienInfoExRepository.FindAll(x => x.MaNV == item.MaNV && x.Year.ToString().CompareTo(thangNam.Substring(0, 4)) == 0, x => x.HR_NHANVIEN).FirstOrDefault();
                    dieuChinhCong = _dieuchinhCongRepository.FindSingle(x => x.MaNV == item.MaNV && x.ChiTraVaoLuongThang2.Substring(0, 7).CompareTo(thangNam.Substring(0, 7)) == 0);
                    phatsinhs = _luongPhatSinhRepository.FindAll(x => x.MaNV == item.MaNV && x.FromTime != null && x.ToTime != null &&
                                                                 DateTime.Parse(thangNam).ToString("yyyyMM").CompareTo(x.FromTime.Replace("-", "").Substring(0, 6)) >= 0 &&
                                                                DateTime.Parse(thangNam).ToString("yyyyMM").CompareTo(x.ToTime.Replace("-", "").Substring(0, 6)) <= 0,
                                   y => y.HR_SALARY_DANHMUC_PHATSINH, z => z.HR_NHANVIEN).ToList();

                    phepnam = _phepNamRepository.FindSingle(x => x.Year == DateTime.Parse(thangNam).Year && x.MaNhanVien == item.MaNV);

                    luong.PositionAllowance = _chucDanhRepository.FindById(item.HR_NHANVIEN.MaChucDanh).PhuCap;

                    if (nhanvienEx != null)
                    {
                        luong.HieuLucCapBac = "";
                        luong.MaBoPhan = nhanvienEx.MaBoPhanEx;
                        luong.Grade = nhanvienEx.Grade;

                        if (_nhanVienRepository.FindById(item.MaNV, x => x.HR_BO_PHAN_DETAIL) != null)
                        {
                            luong.BoPhan = _nhanVienRepository.FindById(item.MaNV, x => x.HR_BO_PHAN_DETAIL).HR_BO_PHAN_DETAIL.MaBoPhan;
                        }
                        else
                        {
                            luong.BoPhan = nhanvienEx.HR_NHANVIEN.MaBoPhan;
                        }
                    }

                    if (salary != null)
                    {
                        salaryPR = _salarySqliteRepository.FindSingle(x => x.MaNV == salary.MaNV);
                        luong.DoiTuongPhuCapDocHai = salary.DoiTuongPhuCapDocHai.NullString().ToLower();

                        if(salaryPR == null)
                        {
                            luong.BasicSalary = (double)salary.BasicSalary;
                            luong.LivingAllowance = (double)salary.LivingAllowance;
                            luong.AbilityAllowance = (double)salary.AbilityAllowance;
                        }
                        else
                        {
                            luong.BasicSalary = (double)salaryPR.BasicSalary;
                            luong.LivingAllowance = (double)salaryPR.LivingAllowance;
                            luong.AbilityAllowance = (double)salaryPR.AbilityAllowance;
                        }

                        if (salary.DoiTuongPhuCapDocHai.NullString().ToLower() == CommonConstants.X)
                        {
                            luong.HarmfulAllowance = _phucapdochaiRepository.FindSingle(x => x.BoPhan == item.HR_NHANVIEN.MaBoPhan).PhuCap * luong.BasicSalary;
                        }
                        else
                        {
                            luong.HarmfulAllowance = 0;
                        }
                    }

                    luong.TongNgayCongThucTe = item.AT_45_TotalALPaid;
                    luong.NgayCongThuViecBanNgay = item.AN_39_PD;
                    luong.NgayCongThuViecBanDem = item.AO_40_PN;
                    luong.NgayCongChinhThucBanNgay = item.AQ_42_DS;
                    luong.NgayCongChinhThucBanDem = item.AR_43_NS;
                    luong.NghiViecCoLuong = item.AS_44_AL;

                    luong.GioLamThemTrongTV_150 = item.BM_64_15;
                    luong.GioLamThemTrongTV_200 = item.BN_65_20;
                    luong.GioLamThemTrongTV_210 = item.BO_66_21;
                    luong.GioLamThemTrongTV_270 = item.BP_67_27;
                    luong.GioLamThemTrongTV_300 = item.BQ_68_30;
                    luong.GioLamThemTrongTV_390 = item.BR_69_39;

                    luong.GioLamThemTrongCT_150 = item.BS_70_15;
                    luong.GioLamThemTrongCT_200 = item.BT_71_20;
                    luong.GioLamThemTrongCT_210 = item.BU_72_21;
                    luong.GioLamThemTrongCT_270 = item.BV_73_27;
                    luong.GioLamThemTrongCT_300 = item.BW_74_30;
                    luong.GioLamThemTrongCT_390 = item.BX_75_39;

                    luong.SoNgayLamCaDemTruocLe_TV = item.AM_38_PH;
                    luong.SoNgayLamCaDemTruocLe_CT = item.AP_41_BH;

                    luong.SoNgayLamCaDem_TV = item.AV_47_LamCD_TV;
                    luong.SoNgayLamCaDem_CT = item.AW_48_LamCD_CT;

                    luong.HoTroThoiGianLamViecTV_150 = item.CB_79_150;
                    luong.HoTroThoiGianLamViecTV_200_NT = item.CC_80_200;
                    luong.HoTroThoiGianLamViecTV_200_CN = item.CD_81_200;
                    luong.HoTroThoiGianLamViecTV_270 = item.CE_82_270;
                    luong.HoTroThoiGianLamViecTV_300 = item.CF_83_300;
                    luong.HoTroThoiGianLamViecTV_390 = item.CG_84_390;

                    luong.HoTroThoiGianLamViecCT_150 = item.CH_85_150;
                    luong.HoTroThoiGianLamViecCT_200_NT = item.CI_86_200;
                    luong.HoTroThoiGianLamViecCT_200_CN = item.CJ_87_200;
                    luong.HoTroThoiGianLamViecCT_270 = item.CK_88_270;
                    luong.HoTroThoiGianLamViecCT_300 = item.CL_89_300;
                    luong.HoTroThoiGianLamViecCT_390 = item.CM_90_390;

                    luong.HoTroNgayThanhLapCty_CaNgayTV = item.BI_60_MD;
                    luong.HoTroNgayThanhLapCty_CaNgayCT = item.BJ_61_PMD;
                    luong.HoTroNgayThanhLapCty_CaDemTV_TruocLe = item.BK_62_PM;
                    luong.HoTroNgayThanhLapCty_CaDemCT_TruocLe = item.BL_63_BM;

                    luong.NghiKhamThai = item.BG_58_KT;
                    luong.NghiViecKhongThongBao = item.BE_56_NL;
                    luong.SoNgayNghiBu_AL30 = item.AY_50_AL30;
                    luong.SoNgayNghiBu_NB = item.BD_55_NB;

                    luong.NghiKhongLuong = item.AU_46_TotalUnPaid;
                    luong.Probation_Late_Come_Early_Leave_Time = item.BY_76_ELLC;
                    luong.Official_Late_Come_Early_Leave_Time = item.BZ_77_OCT;

                    luong.ThuocDoiTuong_BHXH = salary.ThuocDoiTuongBaoHiemXH;

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
                    luong.InsentiveStandard = salary.IncentiveStandard + salary.IncentiveLanguage + salary.IncentiveTechnical + salary.IncentiveOther;

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

                    condoan = _congDoanRepository.FindSingle(x => x.MaNV == item.MaNV);
                    if (condoan != null)
                    {
                        if (condoan.NgayBatDau.Value.ToString("yyyyMM").CompareTo(DateTime.Parse(thangNam).ToString("yyyyMM")) >= 0)
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
                        // Vào làm sau ngày 15 hàng tháng
                        if (DateTime.Parse(luong.NgayVao).ToString("yyyyMM").CompareTo(DateTime.Parse(thangNam).ToString("yyyyMM")) == 0 &&
                            DateTime.Parse(luong.NgayVao).Day > 15)
                        {
                            luong.DoiTuongThamGiaCD = "o";
                        }
                        // 2. Số ngày nghỉ không hưởng lương > 14 ngày
                        // 3.Nghỉ thai sản > 14 ngày
                        else if (item.AU_46_TotalUnPaid > 14)
                        {
                            luong.DoiTuongThamGiaCD = "o";
                        }
                        else
                        {
                            luong.DoiTuongThamGiaCD = "x";
                        }
                    }

                    luong.DoiTuongTruyThuBHYT = salary.DoiTuongTruyThuBHYT;
                    luong.SoConNho = salary.SoConNho;
                    luong.SoNgayNghi70 = item.BD_55_NB;
                    luong.DieuChinhCong_Total = dieuChinhCong != null ? (double)dieuChinhCong?.TongSoTien : 0;
                    luong.TraTienPhepNam_Total = phepnam != null ? (double)phepnam.SoTienChiTra : 0;

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
            List<BANGLUONGCHITIET_HISTORY_PR> lstData = _bangluongChiTietHistoryRepository.FindAll(x => x.ThangNam == time).ToList();
            BANGLUONGCHITIET_HISTORY_PR en;
            List<BANGLUONGCHITIET_HISTORY_PR> lstUpdate = new List<BANGLUONGCHITIET_HISTORY_PR>();
            List<BANGLUONGCHITIET_HISTORY_PR> lstAdd = new List<BANGLUONGCHITIET_HISTORY_PR>();

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
                    en = new BANGLUONGCHITIET_HISTORY_PR();
                    en.CopyPropertiesFrom(item, new List<string>() { });
                    lstAdd.Add(en);
                }
            }

            _bangluongChiTietHistoryRepository.AddRange(lstAdd);
            _bangluongChiTietHistoryRepository.UpdateRange(lstUpdate);
            _payrollUnitOfWork.Commit();
        }

        public List<BangLuongChiTietViewModel> GetHistoryBangLuongChiTiet(string thangNam)
        {
            List<BangLuongChiTietViewModel> result = new List<BangLuongChiTietViewModel>();
            var lstLuongChitiet = _bangluongChiTietHistoryRepository.FindAll(x => x.ThangNam == thangNam);
            BangLuongChiTietViewModel model;
            foreach (var item in lstLuongChitiet)
            {
                model = new BangLuongChiTietViewModel();
                model.CopyPropertiesFrom(item, new List<string>() { "Id" });
                result.Add(model);
            }

            return result;
        }

        public ResultDB ImportExcel(string filePath, out List<HR_SALARY> lstUpdate)
        {
            lstUpdate = new List<HR_SALARY>();
            ResultDB resultDB = new ResultDB();
            try
            {
                using (var packet = new ExcelPackage(new System.IO.FileInfo(filePath)))
                {
                    ExcelWorksheet worksheet = packet.Workbook.Worksheets[1];
                    HR_SALARY salary;
                    HR_NHANVIEN nv;
                    string manv;
                    for (int i = worksheet.Dimension.Start.Row + 1; i <= worksheet.Dimension.End.Row; i++)
                    {
                        manv = worksheet.Cells[i, 1].Text.NullString();
                        nv = _nhanVienRepository.FindById(manv);

                        if (nv == null)
                        {
                            break;
                        }

                        salary = new HR_SALARY()
                        {
                            MaNV = manv
                        };

                        if (worksheet.Cells[i, 3].Text.NullString() != "")
                        {
                            salary.BasicSalary = decimal.Parse(worksheet.Cells[i, 3].Text.IfNullIsZero());
                        }

                        if (worksheet.Cells[i, 4].Text.NullString() != "")
                        {
                            salary.LivingAllowance = decimal.Parse(worksheet.Cells[i, 4].Text.IfNullIsZero());
                        }

                        if (worksheet.Cells[i, 5].Text.NullString() != "")
                        {
                            salary.AbilityAllowance = decimal.Parse(worksheet.Cells[i, 5].Text.IfNullIsZero());
                        }
                        lstUpdate.Add(salary);
                    }

                    resultDB.ReturnInt = 0;
                }
            }
            catch (Exception ex)
            {
                resultDB.ReturnInt = -1;
                resultDB.ReturnString = ex.Message;
            }
            return resultDB;
        }
    }
}
