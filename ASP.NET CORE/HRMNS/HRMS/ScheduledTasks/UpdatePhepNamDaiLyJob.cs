using HRMNS.Application.Interfaces;
using HRMNS.Application.ViewModels.HR;
using HRMNS.Application.ViewModels.Time_Attendance;
using HRMNS.Data.EF.Extensions;
using HRMNS.Data.Entities;
using HRMNS.Data.Entities.Payroll;
using HRMNS.Utilities.Common;
using HRMNS.Utilities.Constants;
using HRMS.Infrastructure.Interfaces;
using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS.ScheduledTasks
{
    public class UpdatePhepNamDaiLyJob : IJob
    {
        private IDangKyChamCongDacBietService _chamCongDacBietService;
        private IDangKyChamCongChiTietService _chamCongChiTietService;
        private INgayChotCongService _ngayChotCongService;
        private INhanVienService _nhanvienService;
        private IPhepNamService _phepNamService;
        private readonly ILogger _logger;

        private IRespository<BANGLUONGCHITIET_HISTORY, int> _bangluongChiTietHistoryRepository;
        IRespository<HR_CHUCDANH, string> _chucDanhRepository;
        IRespository<PHUCAP_DOC_HAI, int> _phucapdochaiRepository;
        IRespository<HR_SALARY_GRADE, string> _gradeRepository;
        IRespository<HR_SALARY, int> _salaryRepository;
        IRespository<NHANVIEN_INFOR_EX, int> _nhanvienInfoExRepository;
        IRespository<HR_HOPDONG, int> _hopdongRepository;
        public UpdatePhepNamDaiLyJob()
        {

        }

        public UpdatePhepNamDaiLyJob(IRespository<BANGLUONGCHITIET_HISTORY, int> bangluongChiTietHistoryRepository,
            ILogger<UpdatePhepNamDaiLyJob> logger, INgayChotCongService ngayChotCongService,
            INhanVienService nhanvienService, IPhepNamService phepNamService,
            IDangKyChamCongChiTietService chamCongChiTietService,
            IDangKyChamCongDacBietService chamCongDacBietService,
            IRespository<HR_CHUCDANH, string> chucDanhRepository,
            IRespository<PHUCAP_DOC_HAI, int> phucapdochaiRepository,
             IRespository<HR_SALARY_GRADE, string> gradeRepository,
             IRespository<HR_SALARY, int> salaryRepository,
             IRespository<NHANVIEN_INFOR_EX, int> nhanvienInfoExRepository,
             IRespository<HR_HOPDONG, int> hopdongRepository)
        {
            _bangluongChiTietHistoryRepository = bangluongChiTietHistoryRepository;
            _ngayChotCongService = ngayChotCongService;
            _chamCongDacBietService = chamCongDacBietService;
            _chamCongChiTietService = chamCongChiTietService;
            _nhanvienService = nhanvienService;
            _phepNamService = phepNamService;
            _logger = logger;
            _chucDanhRepository = chucDanhRepository;
            _phucapdochaiRepository = phucapdochaiRepository;
            _gradeRepository = gradeRepository;
            _salaryRepository = salaryRepository;
            _nhanvienInfoExRepository = nhanvienInfoExRepository;
            _hopdongRepository = hopdongRepository;
        }

        public Task Execute(IJobExecutionContext context)
        {
            try
            {
                // NGHỈ TRỪ PHÉP
                List<string> AL = new List<string>()
                {
                    "AL","AL30","AL/DS","AL/NS","AL/BF","AL/BL"
                };

                _logger.LogInformation("UpdatePhepNamDaiLyJob:Execute:Start:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                string chotCongChoThang = _ngayChotCongService.FinLastItem().ChotCongChoThang;

                string beginTime = DateTime.Parse(DateTime.Parse(chotCongChoThang).ToString("yyyy-MM") + "-01").AddMonths(1).ToString("yyyy-MM") + "-01";
                string endTime = DateTime.Parse(DateTime.Parse(chotCongChoThang).ToString("yyyy-MM") + "-01").AddMonths(2).AddDays(-1).ToString("yyyy-MM-dd");
                string Year = endTime.Substring(0, 4);

                List<NhanVienViewModel> lstNhanVien = _nhanvienService.GetAll(x => x.DANGKY_CHAMCONG_DACBIET, x => x.HR_PHEP_NAM, x => x.NHANVIEN_INFOR_EX, x => x.HR_HOPDONG).FindAll(x => x.MaBoPhan != "KOREA" && (x.NgayNghiViec.NullString() == "" || beginTime.CompareTo(x.NgayNghiViec.NullString()) <= 0));
                List<DangKyChamCongDacBietViewModel> lstChamCongDB =
                    _chamCongDacBietService
                    .GetAll(x => x.DANGKY_CHAMCONG_CHITIET)
                    .Where(x => x.NgayBatDau.CompareTo(endTime) <= 0 && x.NgayKetThuc.CompareTo(Year + "-01-01") >= 0
                               && AL.Contains(x.DANGKY_CHAMCONG_CHITIET.KyHieuChamCong)).ToList();

                List<PhepNamViewModel> lstPhepNam = new List<PhepNamViewModel>();
                lstPhepNam = _phepNamService.GetAll("").FindAll(x => x.Year.ToString() == Year);


                List<PhepNamViewModel> lstPhepNam_Update = new List<PhepNamViewModel>();
                List<PhepNamViewModel> lstPhepNam_Add = new List<PhepNamViewModel>();

                NHANVIEN_INFOR_EX nhanvienEx;
                int month;
                int day;
                float sothangDocHai;

                float nghiT1 = 0;
                float nghiT2 = 0;
                float nghiT3 = 0;
                float nghiT4 = 0;
                float nghiT5 = 0;
                float nghiT6 = 0;
                float nghiT7 = 0;
                float nghiT8 = 0;
                float nghiT9 = 0;
                float nghiT10 = 0;
                float nghiT11 = 0;
                float nghiT12 = 0;
                float totalNghi = 0;
                string ngaytinhphep;

                double BasicSalary = 0;
                double LivingAllowance = 0;
                double PositionAllowance = 0;
                double AbilityAllowance = 0;
                double SeniorityAllowance = 0;
                double HarmfulAllowance = 0;

                int songaylamviec = 0;
                int thamnien = 0;

                foreach (var item in lstNhanVien)
                {
                    nghiT1 = 0;
                    nghiT2 = 0;
                    nghiT3 = 0;
                    nghiT4 = 0;
                    nghiT5 = 0;
                    nghiT6 = 0;
                    nghiT7 = 0;
                    nghiT8 = 0;
                    nghiT9 = 0;
                    nghiT10 = 0;
                    nghiT11 = 0;
                    nghiT12 = 0;
                    totalNghi = 0;

                    ngaytinhphep = "";
                    BasicSalary = 0;
                    LivingAllowance = 0;
                    PositionAllowance = 0;
                    AbilityAllowance = 0;
                    SeniorityAllowance = 0;
                    HarmfulAllowance = 0;
                    songaylamviec = 0;
                    thamnien = 0;

                    PhepNamViewModel phepNam;
                    if (lstPhepNam.Exists(x => x.MaNhanVien == item.Id && x.Year.ToString() == Year))
                    {
                        phepNam = lstPhepNam.FirstOrDefault(x => x.MaNhanVien == item.Id && x.Year.ToString() == Year);
                        phepNam.UserModified = "Quartz-IJob";
                        lstPhepNam_Update.Add(phepNam);
                    }
                    else
                    {
                        phepNam = new PhepNamViewModel()
                        {
                            MaNhanVien = item.Id,
                            Year = int.Parse(Year)
                        };
                        phepNam.UserCreated = "Quartz-IJob";
                        phepNam.UserModified = "Quartz-IJob";
                        lstPhepNam_Add.Add(phepNam);
                    }

                    /**
                      * • Nếu năm vào làm trước năm hiện tại thì 12 ngày
                        • Nếu năm vào làm sau năm hiện tại thì số phép bằng với số tháng làm việc tính đến cuối năm
                            + Vào trước ngày 15 thì tính phép tháng đó luôn
                            + Vào sau ngày 15 thì tháng đó chưa đc phép
                    * **/
                    if (int.Parse(item.NgayVao.Substring(0, 4)) < int.Parse(Year))
                    {
                        phepNam.SoPhepNam = 12;
                    }
                    else if (int.Parse(item.NgayVao.Substring(0, 4)) == int.Parse(Year))
                    {
                        month = int.Parse(item.NgayVao.Split("-")[1]);
                        day = int.Parse(item.NgayVao.Split("-")[2]);

                        if (day <= 15)
                        {
                            phepNam.SoPhepNam = 12 - month + 1;
                        }
                        else
                        {
                            phepNam.SoPhepNam = 12 - month;
                        }
                    }

                    if (phepNam.ThangBatDauDocHai != null && phepNam.ThangKetThucDocHai != null)
                    {
                        sothangDocHai = ((phepNam.ThangKetThucDocHai.Value.Year - phepNam.ThangBatDauDocHai.Value.Year) * 12)
                                          + phepNam.ThangKetThucDocHai.Value.Month - phepNam.ThangBatDauDocHai.Value.Month + 1;

                        if (sothangDocHai >= 12)
                        {
                            if (item.MaBoPhan == "UTILITY")
                            {
                                phepNam.SoPhepDocHai = 2;
                            }
                            else
                            {
                                phepNam.SoPhepDocHai = 4;
                            }
                        }
                        else if (sothangDocHai < 12)
                        {
                            if (item.MaBoPhan == "UTILITY")
                            {
                                phepNam.SoPhepDocHai = (sothangDocHai * 14 / 12) - sothangDocHai;
                            }
                            else
                            {
                                phepNam.SoPhepDocHai = (sothangDocHai * 16 / 12) - sothangDocHai;
                            }
                        }
                    }

                    if (DateTime.Parse(item.NgayVao).Day > 15)
                    {
                        ngaytinhphep = DateTime.Parse(item.NgayVao).AddMonths(1).ToString("yyyy-MM") + "-01";
                    }
                    else
                    {
                        ngaytinhphep = DateTime.Parse(item.NgayVao).ToString("yyyy-MM") + "-01";
                    }

                    phepNam.SoPhepCongThem = DateTime.Parse(ngaytinhphep).AddYears(15).CompareTo(item.NgayNghiViec.NullString() != "" ? DateTime.Parse(item.NgayNghiViec.NullString().Substring(0, 7) + "-01") : DateTime.Parse(endTime.Substring(0, 7) + "-01")) <= 0 ? 3 : 0;

                    if (phepNam.SoPhepCongThem == 0)
                        phepNam.SoPhepCongThem = DateTime.Parse(ngaytinhphep).AddYears(10).CompareTo(item.NgayNghiViec.NullString() != "" ? DateTime.Parse(item.NgayNghiViec.NullString().Substring(0, 7) + "-01") : DateTime.Parse(endTime.Substring(0, 7) + "-01")) <= 0 ? 2 : 0;

                    if (phepNam.SoPhepCongThem == 0)
                        phepNam.SoPhepCongThem = DateTime.Parse(ngaytinhphep).AddYears(5).CompareTo(item.NgayNghiViec.NullString() != "" ? DateTime.Parse(item.NgayNghiViec.NullString().Substring(0, 7) + "-01") : DateTime.Parse(endTime.Substring(0, 7) + "-01")) <= 0 ? 1 : 0;

                    if (DateTime.Now.Year > 2023)
                    {
                        phepNam.SoPhepDaUng = _phepNamService.GetByCodeAndYear(item.Id, int.Parse(Year) - 1).SoPhepTonNam;
                    }

                    phepNam.SoPhepDuocHuong = (float)Math.Round((phepNam.SoPhepNam + phepNam.SoPhepDocHai + phepNam.SoPhepCongThem - (phepNam.SoPhepDaUng * -1)), 1);

                    var lstALUser = lstChamCongDB.FindAll(x => x.MaNV == item.Id);

                    foreach (var us in lstALUser)
                    {
                        foreach (DateTime m1 in EachDay.EachDays(DateTime.Parse(us.NgayBatDau), DateTime.Parse(us.NgayKetThuc)))
                        {
                            if (m1.ToString("yyyy-MM-dd").InRangeDateTime(Year + "-01-01", DateTime.Parse(Year + "-01-01").AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd")))
                            {
                                if ((new List<string> { "AL", "AL30" }).Contains(us.DANGKY_CHAMCONG_CHITIET.KyHieuChamCong))
                                {
                                    nghiT1 += 1;
                                }
                                else
                                if ((new List<string> { "AL/DS", "AL/NS", "AL/BF", "AL/BL", "AL/BF" }).Contains(us.DANGKY_CHAMCONG_CHITIET.KyHieuChamCong))
                                {
                                    nghiT1 += 0.5f;
                                }
                            }
                            else
                            if (m1.ToString("yyyy-MM-dd").InRangeDateTime(Year + "-02-01", DateTime.Parse(Year + "-02-01").AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd")))
                            {
                                if ((new List<string> { "AL", "AL30" }).Contains(us.DANGKY_CHAMCONG_CHITIET.KyHieuChamCong))
                                {
                                    nghiT2 += 1;
                                }
                                else
                                if ((new List<string> { "AL/DS", "AL/NS", "AL/BF", "AL/BL", "AL/BF" }).Contains(us.DANGKY_CHAMCONG_CHITIET.KyHieuChamCong))
                                {
                                    nghiT2 += 0.5f;
                                }
                            }
                            else
                            if (m1.ToString("yyyy-MM-dd").InRangeDateTime(Year + "-03-01", DateTime.Parse(Year + "-03-01").AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd")))
                            {
                                if ((new List<string> { "AL", "AL30" }).Contains(us.DANGKY_CHAMCONG_CHITIET.KyHieuChamCong))
                                {
                                    nghiT3 += 1;
                                }
                                else
                                if ((new List<string> { "AL/DS", "AL/NS", "AL/BF", "AL/BL", "AL/BF" }).Contains(us.DANGKY_CHAMCONG_CHITIET.KyHieuChamCong))
                                {
                                    nghiT3 += 0.5f;
                                }
                            }
                            else
                            if (m1.ToString("yyyy-MM-dd").InRangeDateTime(Year + "-04-01", DateTime.Parse(Year + "-04-01").AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd")))
                            {
                                if ((new List<string> { "AL", "AL30" }).Contains(us.DANGKY_CHAMCONG_CHITIET.KyHieuChamCong))
                                {
                                    nghiT4 += 1;
                                }
                                else
                                if ((new List<string> { "AL/DS", "AL/NS", "AL/BF", "AL/BL", "AL/BF" }).Contains(us.DANGKY_CHAMCONG_CHITIET.KyHieuChamCong))
                                {
                                    nghiT4 += 0.5f;
                                }
                            }
                            else
                            if (m1.ToString("yyyy-MM-dd").InRangeDateTime(Year + "-05-01", DateTime.Parse(Year + "-05-01").AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd")))
                            {
                                if ((new List<string> { "AL", "AL30" }).Contains(us.DANGKY_CHAMCONG_CHITIET.KyHieuChamCong))
                                {
                                    nghiT5 += 1;
                                }
                                else
                                if ((new List<string> { "AL/DS", "AL/NS", "AL/BF", "AL/BL", "AL/BF" }).Contains(us.DANGKY_CHAMCONG_CHITIET.KyHieuChamCong))
                                {
                                    nghiT5 += 0.5f;
                                }
                            }
                            else
                            if (m1.ToString("yyyy-MM-dd").InRangeDateTime(Year + "-06-01", DateTime.Parse(Year + "-06-01").AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd")))
                            {
                                if ((new List<string> { "AL", "AL30" }).Contains(us.DANGKY_CHAMCONG_CHITIET.KyHieuChamCong))
                                {
                                    nghiT6 += 1;
                                }
                                else
                                if ((new List<string> { "AL/DS", "AL/NS", "AL/BF", "AL/BL", "AL/BF" }).Contains(us.DANGKY_CHAMCONG_CHITIET.KyHieuChamCong))
                                {
                                    nghiT6 += 0.5f;
                                }
                            }
                            else
                            if (m1.ToString("yyyy-MM-dd").InRangeDateTime(Year + "-07-01", DateTime.Parse(Year + "-07-01").AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd")))
                            {
                                if ((new List<string> { "AL", "AL30" }).Contains(us.DANGKY_CHAMCONG_CHITIET.KyHieuChamCong))
                                {
                                    nghiT7 += 1;
                                }
                                else
                                if ((new List<string> { "AL/DS", "AL/NS", "AL/BF", "AL/BL", "AL/BF" }).Contains(us.DANGKY_CHAMCONG_CHITIET.KyHieuChamCong))
                                {
                                    nghiT7 += 0.5f;
                                }
                            }
                            else
                            if (m1.ToString("yyyy-MM-dd").InRangeDateTime(Year + "-08-01", DateTime.Parse(Year + "-08-01").AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd")))
                            {
                                if ((new List<string> { "AL", "AL30" }).Contains(us.DANGKY_CHAMCONG_CHITIET.KyHieuChamCong))
                                {
                                    nghiT8 += 1;
                                }
                                else
                                if ((new List<string> { "AL/DS", "AL/NS", "AL/BF", "AL/BL", "AL/BF" }).Contains(us.DANGKY_CHAMCONG_CHITIET.KyHieuChamCong))
                                {
                                    nghiT8 += 0.5f;
                                }
                            }
                            else
                            if (m1.ToString("yyyy-MM-dd").InRangeDateTime(Year + "-09-01", DateTime.Parse(Year + "-09-01").AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd")))
                            {
                                if ((new List<string> { "AL", "AL30" }).Contains(us.DANGKY_CHAMCONG_CHITIET.KyHieuChamCong))
                                {
                                    nghiT9 += 1;
                                }
                                else
                                if ((new List<string> { "AL/DS", "AL/NS", "AL/BF", "AL/BL", "AL/BF" }).Contains(us.DANGKY_CHAMCONG_CHITIET.KyHieuChamCong))
                                {
                                    nghiT9 += 0.5f;
                                }
                            }
                            else
                            if (m1.ToString("yyyy-MM-dd").InRangeDateTime(Year + "-10-01", DateTime.Parse(Year + "-10-01").AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd")))
                            {
                                if ((new List<string> { "AL", "AL30" }).Contains(us.DANGKY_CHAMCONG_CHITIET.KyHieuChamCong))
                                {
                                    nghiT10 += 1;
                                }
                                else
                                if ((new List<string> { "AL/DS", "AL/NS", "AL/BF", "AL/BL", "AL/BF" }).Contains(us.DANGKY_CHAMCONG_CHITIET.KyHieuChamCong))
                                {
                                    nghiT10 += 0.5f;
                                }
                            }
                            else
                            if (m1.ToString("yyyy-MM-dd").InRangeDateTime(Year + "-11-01", DateTime.Parse(Year + "-11-01").AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd")))
                            {
                                if ((new List<string> { "AL", "AL30" }).Contains(us.DANGKY_CHAMCONG_CHITIET.KyHieuChamCong))
                                {
                                    nghiT11 += 1;
                                }
                                else
                                if ((new List<string> { "AL/DS", "AL/NS", "AL/BF", "AL/BL", "AL/BF" }).Contains(us.DANGKY_CHAMCONG_CHITIET.KyHieuChamCong))
                                {
                                    nghiT11 += 0.5f;
                                }
                            }
                            else
                            if (m1.ToString("yyyy-MM-dd").InRangeDateTime(Year + "-12-01", DateTime.Parse(Year + "-12-01").AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd")))
                            {
                                if ((new List<string> { "AL", "AL30" }).Contains(us.DANGKY_CHAMCONG_CHITIET.KyHieuChamCong))
                                {
                                    nghiT12 += 1;
                                }
                                else
                                if ((new List<string> { "AL/DS", "AL/NS", "AL/BF", "AL/BL", "AL/BF" }).Contains(us.DANGKY_CHAMCONG_CHITIET.KyHieuChamCong))
                                {
                                    nghiT12 += 0.5f;
                                }
                            }
                        }
                    }

                    if (nghiT1 >= 0)
                    {
                        totalNghi += nghiT1;
                        phepNam.NghiThang_1 = nghiT1;
                    }

                    if (nghiT2 >= 0)
                    {
                        totalNghi += nghiT2;
                        phepNam.NghiThang_2 = nghiT2;
                    }

                    if (nghiT3 >= 0)
                    {
                        totalNghi += nghiT3;
                        phepNam.NghiThang_3 = nghiT3;
                    }

                    if (nghiT4 >= 0)
                    {
                        totalNghi += nghiT4;
                        phepNam.NghiThang_4 = nghiT4;
                    }

                    if (nghiT5 >= 0)
                    {
                        totalNghi += nghiT5;
                        phepNam.NghiThang_5 = nghiT5;
                    }

                    if (nghiT6 >= 0)
                    {
                        totalNghi += nghiT6;
                        phepNam.NghiThang_6 = nghiT6;
                    }

                    if (nghiT7 >= 0)
                    {
                        totalNghi += nghiT7;
                        phepNam.NghiThang_7 = nghiT7;
                    }

                    if (nghiT8 >= 0)
                    {
                        totalNghi += nghiT8;
                        phepNam.NghiThang_8 = nghiT8;
                    }

                    if (nghiT9 >= 0)
                    {
                        totalNghi += nghiT9;
                        phepNam.NghiThang_9 = nghiT9;
                    }

                    if (nghiT10 >= 0)
                    {
                        totalNghi += nghiT10;
                        phepNam.NghiThang_10 = nghiT10;
                    }

                    if (nghiT11 >= 0)
                    {
                        totalNghi += nghiT11;
                        phepNam.NghiThang_11 = nghiT11;
                    }

                    if (nghiT12 >= 0)
                    {
                        totalNghi += nghiT12;
                        phepNam.NghiThang_12 = nghiT12;
                    }

                    phepNam.TongNgayNghi = totalNghi;
                    phepNam.SoPhepTonNam = (float)Math.Round(phepNam.SoPhepDuocHuong - totalNghi, 1);

                    if (item.NgayNghiViec.NullString() == "")
                    {
                        phepNam.SoPhepKhongDuocSuDung = 12 - DateTime.Parse(endTime).Month;
                    }
                    else
                    {
                        if (item.NgayNghiViec.CompareTo(endTime) > 0)
                        {
                            phepNam.SoPhepKhongDuocSuDung = 12 - DateTime.Parse(endTime).Month;
                        }
                        else
                        {
                            phepNam.SoPhepKhongDuocSuDung = 12 - DateTime.Parse(item.NgayNghiViec.NullString()).Month;
                        }

                        if (DateTime.Parse(item.NgayVao).AddMonths(2).ToString("yyyy-MM-dd").CompareTo(item.NgayNghiViec.NullString()) >= 0)
                        {
                            HR_HOPDONG HD = _hopdongRepository.FindAll(x => x.MaNV == item.Id, x => x.HR_LOAIHOPDONG).OrderByDescending(x => x.NgayHieuLuc).FirstOrDefault();
                            if (HD != null && HD.HR_LOAIHOPDONG.ShortName.StartsWith("TV"))
                            {
                                phepNam.SoPhepKhongDuocSuDung += 2;
                            }
                        }
                    }

                    phepNam.SoPhepTonThang = (float)Math.Round((phepNam.SoPhepTonNam - phepNam.SoPhepKhongDuocSuDung), 1);

                    if (item.NgayNghiViec.NullString() != "")
                    {
                        if (DateTime.Parse(item.NgayNghiViec.NullString()).Day <= 15 && item.NgayNghiViec.CompareTo(endTime) <= 0)
                        {
                            phepNam.SoPhepThanhToanNghiViec = phepNam.SoPhepTonThang - 1;
                        }
                        else
                        {
                            phepNam.SoPhepThanhToanNghiViec = phepNam.SoPhepTonThang;
                        }

                        if (DateTime.Parse(endTime).AddDays(1).ToString("yyyy-MM-dd").CompareTo(item.NgayNghiViec) < 0)
                        {
                            phepNam.ThoiGianChiTra = DateTime.Parse(endTime).AddDays(1).ToString("yyyy-MM");
                        }
                        else
                        {
                            phepNam.ThoiGianChiTra = endTime.Substring(0, 7);
                        }

                        string beforNgaynghiviec = DateTime.Parse(item.NgayNghiViec).AddMonths(-1).ToString("yyyy-MM");

                        // nghỉ đầu tháng
                        if(DateTime.Parse(endTime).AddDays(1).ToString("yyyy-MM-dd") == item.NgayNghiViec)
                        {
                            beforNgaynghiviec = DateTime.Parse(item.NgayNghiViec).AddMonths(-2).ToString("yyyy-MM");
                        }

                        BANGLUONGCHITIET_HISTORY his = _bangluongChiTietHistoryRepository.FindAll(x => x.MaNV == item.Id && (x.ThangNam + "").Substring(0, 7) == beforNgaynghiviec).FirstOrDefault();

                        if (his != null)
                        {
                            phepNam.MucThanhToan = (float)(his.DailySalary);
                        }
                        else
                        {
                            int lastDay = DateTime.DaysInMonth(DateTime.Parse(item.NgayNghiViec).Year, DateTime.Parse(item.NgayNghiViec).Month);//DateTime.Parse(DateTime.Parse(item.NgayNghiViec).AddMonths(-1).ToString("yyyy-MM")+"-01").Day;
                            for (int i = 1; i <= lastDay; i++)
                            {
                                DateTime d = new DateTime(DateTime.Parse(item.NgayNghiViec).Year, DateTime.Parse(item.NgayNghiViec).Month, i);
                                //Compare date with sunday
                                if (d.DayOfWeek != DayOfWeek.Sunday)
                                {
                                    songaylamviec += 1;
                                }
                            }

                            nhanvienEx = _nhanvienInfoExRepository.FindAll(x => x.MaNV == item.Id && x.Year.ToString() == Year, x => x.HR_NHANVIEN).FirstOrDefault();
                            if (nhanvienEx != null)
                            {
                                HR_SALARY salary = _salaryRepository.FindSingle(x => x.MaNV == item.Id, x => x.HR_NHANVIEN);
                                HR_SALARY_GRADE grade = _gradeRepository.FindAll(x => x.Id == nhanvienEx.Grade).FirstOrDefault();
                                BasicSalary = grade != null ? grade.BasicSalary : 0;
                                LivingAllowance = grade.LivingAllowance;
                                PositionAllowance = _chucDanhRepository.FindById(item.MaChucDanh).PhuCap;
                                AbilityAllowance = (double)salary.AbilityAllowance;

                                if (grade.Id == "M1-1" || grade.Id == "P2-1")
                                {
                                    thamnien = EachDay.GetMonthDifference(new DateTime(DateTime.Parse(item.NgayNghiViec).Year, DateTime.Parse(item.NgayNghiViec).Month, 1), DateTime.Parse(item.NgayVao));

                                    // IF(DG3>=120,1250000
                                    if (thamnien >= 120)
                                    {
                                        SeniorityAllowance = 1250000;
                                    }
                                    // IF(AND(DG3<=119,DG3>=108),1150000,
                                    else if (thamnien >= 108 && thamnien <= 119)
                                    {
                                        SeniorityAllowance = 1150000;
                                    }
                                    // IF(AND(DG3<=107,DG3>=96),1050000,
                                    else if (thamnien >= 96 && thamnien <= 107)
                                    {
                                        SeniorityAllowance = 1050000;
                                    }
                                    // IF(AND(DG3<=95,DG3>=84),950000,
                                    else if (thamnien >= 84 && thamnien <= 95)
                                    {
                                        SeniorityAllowance = 950000;
                                    }
                                    // IF(AND(DG3<=83,DG3>=72),850000,
                                    else if (thamnien >= 72 && thamnien <= 83)
                                    {
                                        SeniorityAllowance = 850000;
                                    }
                                    // IF(AND(DG3<=71,DG3>=60),750000,
                                    else if (thamnien >= 60 && thamnien <= 71)
                                    {
                                        SeniorityAllowance = 750000;
                                    }
                                    // IF(AND(DG3<=59,DG3>=48),650000
                                    else if (thamnien >= 48 && thamnien <= 59)
                                    {
                                        SeniorityAllowance = 650000;
                                    }
                                    // IF(AND(DG3<=47,DG3>=36),550000,
                                    else if (thamnien >= 36 && thamnien <= 47)
                                    {
                                        SeniorityAllowance = 550000;
                                    }
                                    // IF(AND(DG3<=35,DG3>=24),450000,
                                    else if (thamnien >= 24 && thamnien <= 35)
                                    {
                                        SeniorityAllowance = 450000;
                                    }
                                    // IF(AND(DG3<=23,DG3>=18),350000,
                                    else if (thamnien >= 18 && thamnien <= 23)
                                    {
                                        SeniorityAllowance = 350000;
                                    }
                                    // IF(AND(DG3<=17,DG3>=12),250000,
                                    else if (thamnien >= 12 && thamnien <= 17)
                                    {
                                        SeniorityAllowance = 250000;
                                    }
                                    // IF(AND(DG3<=11,DG3>=2),150000,
                                    else if (thamnien >= 2 && thamnien <= 11)
                                    {
                                        SeniorityAllowance = 150000;
                                    }
                                }

                                if (salary.DoiTuongPhuCapDocHai.NullString().ToLower() == CommonConstants.X)
                                {
                                    HarmfulAllowance = _phucapdochaiRepository.FindSingle(x => x.BoPhan == item.MaBoPhan).PhuCap * BasicSalary;
                                }
                                else
                                {
                                    HarmfulAllowance = 0;
                                }

                                phepNam.MucThanhToan = (float)Math.Round((BasicSalary + LivingAllowance + PositionAllowance + AbilityAllowance + SeniorityAllowance + HarmfulAllowance) / songaylamviec, 0);
                            }
                        }
                        phepNam.SoTienChiTra = (decimal)(phepNam.MucThanhToan * phepNam.SoPhepThanhToanNghiViec);
                    }
                }

                if (lstPhepNam_Add.Count > 0)
                {
                    _phepNamService.AddRange(lstPhepNam_Add);
                }

                if (lstPhepNam_Update.Count > 0)
                {
                    _phepNamService.UpdateRange(lstPhepNam_Update);
                }

                _phepNamService.Save();

                _logger.LogInformation("UpdatePhepNamDaiLyJob:Execute:End:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                return Task.FromResult(true);
            }
            catch (Exception ex)
            {
                _logger.LogWarning("UpdatePhepNamDaiLyJob:Execute: " + ex.StackTrace);
                return Task.FromResult(false);
            }
            finally
            {
                _logger.LogInformation("UpdatePhepNamDaiLyJob:Execute:End:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            }
        }
    }
}
