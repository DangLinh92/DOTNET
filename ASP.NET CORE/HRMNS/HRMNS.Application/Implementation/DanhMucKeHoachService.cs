using AutoMapper;
using HRMNS.Application.Interfaces;
using HRMNS.Application.ViewModels.EHS;
using HRMNS.Data.EF.Extensions;
using HRMNS.Data.Entities;
using HRMNS.Utilities.Common;
using HRMNS.Utilities.Constants;
using HRMNS.Utilities.Dtos;
using HRMS.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace HRMNS.Application.Implementation
{
    public class DanhMucKeHoachService : BaseService, IDanhMucKeHoachService
    {
        private IRespository<EHS_DM_KEHOACH, Guid> _ehsDanhMucKHRespository;
        private IRespository<EHS_LUATDINH_KEHOACH, int> _ehsLuatDinhKHRespository;

        private IRespository<EHS_KEHOACH_QUANTRAC, int> _ehsKeHoachQuanTracRepository;
        private IRespository<EHS_KE_HOACH_KHAM_SK, Guid> _ehsKeHoachKhamSKRepository;
        private IRespository<EHS_KEHOACH_DAOTAO_ANTOAN_VSLD, Guid> _ehsKeHoachDaoTaoATLDRepository;
        private IRespository<EHS_KEHOACH_PCCC, Guid> _ehsKeHoachPCCCRepository;
        private IRespository<EHS_KEHOACH_ANTOAN_BUCXA, Guid> _ehsKehoachATBXRepository;
        private IRespository<EHS_KEHOACH_KIEMDINH_MAYMOC, Guid> _ehsKiemDinhMMRepository;

        private IRespository<EHS_NGAY_THUC_HIEN_CHITIET_QUANTRAC, int> _ngayQuanTracRepository;
        private IRespository<EHS_NGAY_THUC_HIEN_CHITIET_KHAM_SK, int> _ngayKhamSKRepository;
        private IRespository<EHS_THOIGIAN_THUC_HIEN_DAOTAO_ATVSLD, int> _ngayATVSLDRepository;
        private IRespository<EHS_THOIGIAN_THUC_HIEN_PCCC, int> _ngayPCCCRepository;
        private IRespository<EHS_THOIGIAN_THUC_HIEN_ANTOAN_BUCXA, int> _ngayATBXRepository;
        private IRespository<EHS_THOIGIAN_THUC_HIEN_KIEMDINH_MM, int> _ngayKiemDinhMMRepository;

        private IRespository<EHS_FILES, int> _ehsFileRepository;

        private IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DanhMucKeHoachService(
            IRespository<EHS_DM_KEHOACH, Guid> ehsDanhMucKHRespository,
            IRespository<EHS_LUATDINH_KEHOACH, int> ehsLuatDinhKHRespository,
            IRespository<EHS_KEHOACH_QUANTRAC, int> ehsKeHoachQuanTracRepository,
            IRespository<EHS_KE_HOACH_KHAM_SK, Guid> ehsKeHoachKhamSKRepository,
            IRespository<EHS_KEHOACH_DAOTAO_ANTOAN_VSLD, Guid> ehsKeHoachDaoTaoATLDRepository,
            IRespository<EHS_KEHOACH_PCCC, Guid> ehsKeHoachPCCCRepository,
            IRespository<EHS_KEHOACH_ANTOAN_BUCXA, Guid> ehsKehoachATBXRepository,
            IRespository<EHS_KEHOACH_KIEMDINH_MAYMOC, Guid> ehsKiemDinhMMRepository,

            IRespository<EHS_NGAY_THUC_HIEN_CHITIET_QUANTRAC, int> ngayQuanTracRepository,
            IRespository<EHS_NGAY_THUC_HIEN_CHITIET_KHAM_SK, int> ngayKhamSKRepository,
            IRespository<EHS_THOIGIAN_THUC_HIEN_DAOTAO_ATVSLD, int> ngayATVSLDRepository,
            IRespository<EHS_THOIGIAN_THUC_HIEN_PCCC, int> ngayPCCCRepository,
            IRespository<EHS_THOIGIAN_THUC_HIEN_ANTOAN_BUCXA, int> ngayATBXRepository,
            IRespository<EHS_THOIGIAN_THUC_HIEN_KIEMDINH_MM, int> ngayKiemDinhMMRepository,

            IRespository<EHS_FILES, int> ehsFileRepository,

            IHttpContextAccessor httpContextAccessor,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _ehsDanhMucKHRespository = ehsDanhMucKHRespository;
            _ehsLuatDinhKHRespository = ehsLuatDinhKHRespository;
            _httpContextAccessor = httpContextAccessor;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _ehsKeHoachQuanTracRepository = ehsKeHoachQuanTracRepository;
            _ehsKeHoachKhamSKRepository = ehsKeHoachKhamSKRepository;
            _ehsKeHoachDaoTaoATLDRepository = ehsKeHoachDaoTaoATLDRepository;
            _ehsKeHoachPCCCRepository = ehsKeHoachPCCCRepository;
            _ehsKehoachATBXRepository = ehsKehoachATBXRepository;
            _ehsKiemDinhMMRepository = ehsKiemDinhMMRepository;

            _ngayQuanTracRepository = ngayQuanTracRepository;
            _ngayKhamSKRepository = ngayKhamSKRepository;
            _ngayATVSLDRepository = ngayATVSLDRepository;
            _ngayPCCCRepository = ngayPCCCRepository;
            _ngayATBXRepository = ngayATBXRepository;
            _ngayKiemDinhMMRepository = ngayKiemDinhMMRepository;

            _ehsFileRepository = ehsFileRepository;

        }

        public EhsDanhMucKeHoachPageViewModel GetDataDanhMucKeHoachPage(Guid? maKeHoach)
        {
            EhsDanhMucKeHoachPageViewModel model = new EhsDanhMucKeHoachPageViewModel();
            model.EhsDMKeHoachViewModels = GetDMKehoach().OrderBy(x => x.OrderDM).ToList();

            if (maKeHoach == null)
            {
                maKeHoach = model.EhsDMKeHoachViewModels.FirstOrDefault()?.Id;
            }

            model.MaKeHoachActive = maKeHoach;

            model.TaskStatistics = GetStatistics(maKeHoach.ToString());
            return model;
        }

        public EhsLuatDinhKeHoachViewModel DeleteLuatDinhKeHoach(int id)
        {
            throw new NotImplementedException();
        }


        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public List<EhsDMKeHoachViewModel> GetDMKehoach()
        {
            return _mapper.Map<List<EhsDMKeHoachViewModel>>(_ehsDanhMucKHRespository.FindAll(x => x.EHS_LUATDINH_KEHOACH));
        }

        public List<EhsLuatDinhDeMucKeHoachViewModel> GetLuatDinhDeMucKeHoach()
        {
            throw new NotImplementedException();
        }

        public List<EhsLuatDinhKeHoachViewModel> GetLuatDinhKeHoach(Guid? maKeHoach)
        {
            return _mapper.Map<List<EhsLuatDinhKeHoachViewModel>>(_ehsLuatDinhKHRespository.FindAll(x => maKeHoach.Equals(x.MaKeHoach)));
        }

        public EhsDMKeHoachViewModel UpdateDMKeHoach(EhsDMKeHoachViewModel kehoach)
        {
            var khEn = _ehsDanhMucKHRespository.FindById(kehoach.Id);
            var en = _mapper.Map<EHS_DM_KEHOACH>(kehoach);
            if (khEn == null)
            {
                _ehsDanhMucKHRespository.Add(en);
            }
            else
            {
                _ehsDanhMucKHRespository.Update(en);
            }
            return kehoach;
        }

        public EhsLuatDinhKeHoachViewModel UpdateLuatDinhKeHoach(EhsLuatDinhKeHoachViewModel model)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public Guid DeleteDMKeHoach(Guid id)
        {
            throw new NotImplementedException();
        }

        public List<TotalAllItemByYear> TongHopKeHoachByYear(string year)
        {
            List<EHS_KEHOACH_QUANTRAC> quantrac = _ehsKeHoachQuanTracRepository.FindAll(x => x.Year == year).OrderBy(x => x.STT).ToList();
            List<EHS_KE_HOACH_KHAM_SK> khamsuckhoe = _ehsKeHoachKhamSKRepository.FindAll(x => x.Year == year).ToList();
            List<EHS_KEHOACH_DAOTAO_ANTOAN_VSLD> atld = _ehsKeHoachDaoTaoATLDRepository.FindAll(x => x.Year == year).OrderBy(x => x.STT).ToList();
            List<EHS_KEHOACH_KIEMDINH_MAYMOC> kiemdinhmaymoc = _ehsKiemDinhMMRepository.FindAll(x => x.Year == year).OrderBy(x => x.STT).ToList();
            List<EHS_KEHOACH_PCCC> phongchayCC = _ehsKeHoachPCCCRepository.FindAll(x => x.Year == year).OrderBy(x => x.STT).ToList();
            List<EHS_KEHOACH_ANTOAN_BUCXA> antoanbucxa = _ehsKehoachATBXRepository.FindAll(x => x.Year == year).OrderBy(x => x.STT).ToList();

            List<TotalAllItemByYear> report = new List<TotalAllItemByYear>();
            TotalAllItemByYear item;

            // kế hoạch quan trắc
            foreach (var kh in quantrac)
            {
                item = new TotalAllItemByYear()
                {
                    STT = kh.STT,
                    OrderItem = 1,
                    TenDeMuc = kh.Demuc,
                    TenNoiDung = kh.NoiDung,
                    NhaThau = kh.NhaThau,
                    ChuKy = kh.ChuKyThucHien,
                    NguoiPhuTrach = kh.NguoiPhuTrach,
                    Month_1 = kh.CostMonth_1,
                    Month_2 = kh.CostMonth_2,
                    Month_3 = kh.CostMonth_3,
                    Month_4 = kh.CostMonth_4,
                    Month_5 = kh.CostMonth_5,
                    Month_6 = kh.CostMonth_6,
                    Month_7 = kh.CostMonth_7,
                    Month_8 = kh.CostMonth_8,
                    Month_9 = kh.CostMonth_9,
                    Month_10 = kh.CostMonth_10,
                    Month_11 = kh.CostMonth_11,
                    Month_12 = kh.CostMonth_12,
                    MaKeHoach = kh.Id + "quantrac"
                };
                report.Add(item);
            }

            // Kham suc khỏe
            foreach (var kh in khamsuckhoe)
            {
                item = new TotalAllItemByYear()
                {
                    STT = 1,
                    OrderItem = 2,
                    TenDeMuc = "KẾ HOẠCH KHÁM SỨC KHỎE 정기 건강검진 계획",
                    TenNoiDung = kh.NoiDung,
                    NhaThau = kh.NhaThau,
                    ChuKy = kh.ChuKyThucHien,
                    NguoiPhuTrach = kh.NguoiPhuTrach,
                    Month_1 = kh.CostMonth_1,
                    Month_2 = kh.CostMonth_2,
                    Month_3 = kh.CostMonth_3,
                    Month_4 = kh.CostMonth_4,
                    Month_5 = kh.CostMonth_5,
                    Month_6 = kh.CostMonth_6,
                    Month_7 = kh.CostMonth_7,
                    Month_8 = kh.CostMonth_8,
                    Month_9 = kh.CostMonth_9,
                    Month_10 = kh.CostMonth_10,
                    Month_11 = kh.CostMonth_11,
                    Month_12 = kh.CostMonth_12,
                    MaKeHoach = kh.Id.ToString() + "khamsuckhoe"
                };
                report.Add(item);
            }

            // an toàn môi trường lao động
            foreach (var kh in atld)
            {
                item = new TotalAllItemByYear()
                {
                    STT = kh.STT,
                    OrderItem = 3,
                    TenDeMuc = "ĐÀO TẠO AN TOÀN LAO ĐỘNG 노동안전 교육",
                    TenNoiDung = kh.NoiDung,
                    NhaThau = kh.NhaThau,
                    ChuKy = kh.ChuKyThucHien,
                    NguoiPhuTrach = kh.NguoiPhuTrach,
                    Month_1 = kh.CostMonth_1,
                    Month_2 = kh.CostMonth_2,
                    Month_3 = kh.CostMonth_3,
                    Month_4 = kh.CostMonth_4,
                    Month_5 = kh.CostMonth_5,
                    Month_6 = kh.CostMonth_6,
                    Month_7 = kh.CostMonth_7,
                    Month_8 = kh.CostMonth_8,
                    Month_9 = kh.CostMonth_9,
                    Month_10 = kh.CostMonth_10,
                    Month_11 = kh.CostMonth_11,
                    Month_12 = kh.CostMonth_12,
                    MaKeHoach = kh.Id.ToString() + "atvsld"
                };
                report.Add(item);
            }

            // Kiểm định máy móc
            foreach (var kh in kiemdinhmaymoc)
            {
                item = new TotalAllItemByYear()
                {
                    STT = kh.STT,
                    OrderItem = 4,
                    TenDeMuc = "KIỂM ĐỊNH MÁY MÓC 기계 검교정",
                    TenNoiDung = kh.TenMayMoc,
                    NhaThau = kh.NhaThau,
                    ChuKy = kh.ChuKyKiemDinh,
                    NguoiPhuTrach = kh.NguoiPhuTrach,
                    Month_1 = kh.CostMonth_1,
                    Month_2 = kh.CostMonth_2,
                    Month_3 = kh.CostMonth_3,
                    Month_4 = kh.CostMonth_4,
                    Month_5 = kh.CostMonth_5,
                    Month_6 = kh.CostMonth_6,
                    Month_7 = kh.CostMonth_7,
                    Month_8 = kh.CostMonth_8,
                    Month_9 = kh.CostMonth_9,
                    Month_10 = kh.CostMonth_10,
                    Month_11 = kh.CostMonth_11,
                    Month_12 = kh.CostMonth_12,
                    MaKeHoach = kh.Id.ToString() + "kiemdinhmaymoc"
                };
                report.Add(item);
            }

            // PCCC
            foreach (var kh in phongchayCC)
            {
                item = new TotalAllItemByYear()
                {
                    STT = kh.STT,
                    OrderItem = 5,
                    TenDeMuc = "PCCC 소방에 관한 계획",
                    TenNoiDung = kh.HangMuc,
                    NhaThau = kh.NhaThau,
                    ChuKy = kh.ChuKyThucHien,
                    NguoiPhuTrach = kh.NguoiPhuTrach,
                    Month_1 = kh.CostMonth_1,
                    Month_2 = kh.CostMonth_2,
                    Month_3 = kh.CostMonth_3,
                    Month_4 = kh.CostMonth_4,
                    Month_5 = kh.CostMonth_5,
                    Month_6 = kh.CostMonth_6,
                    Month_7 = kh.CostMonth_7,
                    Month_8 = kh.CostMonth_8,
                    Month_9 = kh.CostMonth_9,
                    Month_10 = kh.CostMonth_10,
                    Month_11 = kh.CostMonth_11,
                    Month_12 = kh.CostMonth_12,
                    MaKeHoach = kh.Id.ToString() + "pccc"
                };
                report.Add(item);
            }

            // An toàn bức xạ
            foreach (var kh in antoanbucxa)
            {
                item = new TotalAllItemByYear()
                {
                    STT = kh.STT,
                    OrderItem = 6,
                    TenDeMuc = "AN TOÀN BỨC XẠ 매년 핵 방사 안전 실시 업무",
                    TenNoiDung = kh.HangMuc + " [ " + kh.NoiDung + " ]",
                    NhaThau = kh.NhaThau,
                    ChuKy = kh.ChuKyThucHien,
                    NguoiPhuTrach = kh.NguoiPhuTrach,
                    Month_1 = kh.CostMonth_1,
                    Month_2 = kh.CostMonth_2,
                    Month_3 = kh.CostMonth_3,
                    Month_4 = kh.CostMonth_4,
                    Month_5 = kh.CostMonth_5,
                    Month_6 = kh.CostMonth_6,
                    Month_7 = kh.CostMonth_7,
                    Month_8 = kh.CostMonth_8,
                    Month_9 = kh.CostMonth_9,
                    Month_10 = kh.CostMonth_10,
                    Month_11 = kh.CostMonth_11,
                    Month_12 = kh.CostMonth_12,
                    MaKeHoach = kh.Id.ToString() + "atbucxa"
                };
                report.Add(item);
            }

            return report.OrderBy(x => x.OrderItem).ThenBy(x => x.STT).ToList();
        }

        public List<EhsKeHoachItemModel> DanhSachKeHoachByTime(string fromTime, string ToTime)
        {
            List<EhsKeHoachItemModel> rs = new List<EhsKeHoachItemModel>();
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("A_FROM", fromTime);
            dic.Add("A_TO", ToTime);
            ResultDB resultDB = _ehsDanhMucKHRespository.ExecProceduce2("PKG_BUSINESS@GET_LIST_KEHOACH", dic);
            if (resultDB.ReturnInt == 0)
            {
                if (resultDB.ReturnDataSet.Tables[0].Rows.Count > 0)
                {
                    DataTable data = resultDB.ReturnDataSet.Tables[0];
                    EhsKeHoachItemModel item;
                    int rowN = 0;
                    int dayOff = 0;
                    string maNgayThucHien = "";
                    string folder = "";
                    foreach (DataRow row in data.Rows)
                    {
                        maNgayThucHien = row["MaNgayTH"].NullString();
                        folder = GetFolderKetQua(maNgayThucHien);

                        dayOff = int.Parse(row["DIFF"].IfNullIsZero()) >= 0 ? int.Parse(row["DIFF"].IfNullIsZero()) : 0;
                        item = new EhsKeHoachItemModel()
                        {
                            Demuc = row["Demuc"].NullString(),
                            NoiDung = row["NoiDung"].NullString(),
                            ThoiGian = row["NgayBatDau"].NullString(),
                            NguoiPhuTrach = row["NguoiPhuTrach"].NullString(),
                            SoNgayConLai = dayOff,
                            ActualFinish = row["ActualFinish"].NullString(),
                            Progress = int.Parse(row["Progress"].IfNullIsZero()),
                            Status = row["Status"].NullString(),
                            MaNgayThucHien = maNgayThucHien,
                            Folder = folder,
                            STT = ++rowN,
                            KetQua = row["KetQua"].NullString()
                        };
                        rs.Add(item);
                    }
                }
            }
            return rs;
        }

        public List<KanbanViewModel> GetKanBanBoard()
        {
            List<KanbanViewModel> rs = new List<KanbanViewModel>();
            Dictionary<string, string> dic = new Dictionary<string, string>();
            ResultDB resultDB = _ehsDanhMucKHRespository.ExecProceduce2("PKG_BUSINESS@GET_KANBAN_KEHOACH", dic);

            if (resultDB.ReturnInt == 0)
            {
                if (resultDB.ReturnDataSet.Tables[0].Rows.Count > 0)
                {
                    DataTable data = resultDB.ReturnDataSet.Tables[0];
                    KanbanViewModel item;
                    foreach (DataRow row in data.Rows)
                    {
                        item = new KanbanViewModel()
                        {
                            Id = row["Id"].NullString(),
                            Title = row["Demuc"].NullString(),
                            BeginTime = row["NgayBatDau"].NullString(),
                            Status = row["Status"].NullString(),
                            NguoiPhuTrach = row["NguoiPhuTrach"].NullString(),
                            Priority = row["Priority"].NullString(),
                            IsShowBoard = row["IsShowBoard"].NullString(),
                            Progress = int.Parse(row["Progress"].IfNullIsZero()),
                            ActualFinish = row["ActualFinish"].NullString()
                        };
                        rs.Add(item);
                    }
                }
            }

            return rs;
        }

        public int UpdateEvent(string id, string status, string priority, string progress, string actualFinish, string begindate, string action)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("A_ID", id.NullString());
            dic.Add("A_STATUS", status.NullString());
            dic.Add("A_PRIORITY", priority.NullString());
            dic.Add("A_PROGRESS", progress.NullString());
            dic.Add("A_ACTUAL_FINISH", actualFinish.NullString());
            dic.Add("A_BEGIN_DATE", begindate.NullString());
            dic.Add("A_ACTION", action.NullString());

            ResultDB resultDB = _ehsDanhMucKHRespository.ExecProceduce2("PKG_BUSINESS@UPDATE_EVENT_BY_ID", dic);

            return resultDB.ReturnInt;
        }

        public KanbanViewModel GetEvenById(string id)
        {
            throw new NotImplementedException();
        }

        public List<EhsFileKetQuaViewModel> GetFileByNoiDung(string makehoach)
        {
            throw new NotImplementedException();
        }

        public List<Ehs_ThoiGianThucHien> GetThoiGianThucHien(string maKeHoach)
        {
            List<Ehs_ThoiGianThucHien> result = new List<Ehs_ThoiGianThucHien>();
            Ehs_ThoiGianThucHien thoigian;

            if (maKeHoach.Contains("quantrac"))
            {
                // Quan Trắc
                var qtracs = _ngayQuanTracRepository.FindAll(x => x.MaKHQuanTrac.ToString() + "quantrac" == maKeHoach).ToList();
                foreach (var item in qtracs)
                {
                    thoigian = new Ehs_ThoiGianThucHien()
                    {
                        MaKeHoach = maKeHoach,
                        MaNgayChiTiet = item.Id + "quantrac",
                        ThoiGianBatDau = item.NgayBatDau,
                        ThoiGianKetThuc = item.NgayKetThuc,
                        Status = item.Status == "TODO" ? "Ready To Start" : item.Status,
                        KetQua = item.KetQua
                    };

                    result.Add(thoigian);
                }
            }
            else if (maKeHoach.Contains("khamsuckhoe"))
            {
                // Khám SK
                var qtracs = _ngayKhamSKRepository.FindAll(x => x.MaKHKhamSK.ToString() + "khamsuckhoe" == maKeHoach).ToList();
                foreach (var item in qtracs)
                {
                    thoigian = new Ehs_ThoiGianThucHien()
                    {
                        MaKeHoach = maKeHoach,
                        MaNgayChiTiet = item.Id + "khamsuckhoe",
                        ThoiGianBatDau = item.NgayBatDau,
                        ThoiGianKetThuc = item.NgayKetThuc,
                        Status = item.Status == "TODO" ? "Ready To Start" : item.Status,
                        KetQua = item.KetQua
                    };

                    result.Add(thoigian);
                }
            }
            else if (maKeHoach.Contains("atvsld"))
            {
                // ATVSLD
                var qtracs = _ngayATVSLDRepository.FindAll(x => x.MaKHDaoTaoATLD.ToString() + "atvsld" == maKeHoach).ToList();
                foreach (var item in qtracs)
                {
                    thoigian = new Ehs_ThoiGianThucHien()
                    {
                        MaKeHoach = maKeHoach,
                        MaNgayChiTiet = item.Id + "atvsld",
                        ThoiGianBatDau = item.NgayBatDau,
                        ThoiGianKetThuc = item.NgayKetThuc,
                        Status = item.Status == "TODO" ? "Ready To Start" : item.Status,
                        KetQua = item.KetQua
                    };

                    result.Add(thoigian);
                }
            }
            else if (maKeHoach.Contains("kiemdinhmaymoc"))
            {
                // KDMM
                var qtracs = _ngayKiemDinhMMRepository.FindAll(x => x.MaKH_KDMM.ToString() + "kiemdinhmaymoc" == maKeHoach).ToList();
                foreach (var item in qtracs)
                {
                    thoigian = new Ehs_ThoiGianThucHien()
                    {
                        MaKeHoach = maKeHoach,
                        MaNgayChiTiet = item.Id + "kiemdinhmaymoc",
                        ThoiGianBatDau = item.NgayBatDau,
                        ThoiGianKetThuc = item.NgayKetThuc,
                        Status = item.Status == "TODO" ? "Ready To Start" : item.Status,
                        KetQua = item.KetQua
                    };

                    result.Add(thoigian);
                }
            }
            else if (maKeHoach.Contains("pccc"))
            {
                // PCCC
                var qtracs = _ngayPCCCRepository.FindAll(x => x.MaKH_PCCC.ToString() + "pccc" == maKeHoach).ToList();
                foreach (var item in qtracs)
                {
                    thoigian = new Ehs_ThoiGianThucHien()
                    {
                        MaKeHoach = maKeHoach,
                        MaNgayChiTiet = item.Id + "pccc",
                        ThoiGianBatDau = item.NgayBatDau,
                        ThoiGianKetThuc = item.NgayKetThuc,
                        Status = item.Status == "TODO" ? "Ready To Start" : item.Status,
                        KetQua = item.KetQua
                    };

                    result.Add(thoigian);
                }
            }
            else if (maKeHoach.Contains("atbucxa"))
            {
                // ATBX
                var qtracs = _ngayATBXRepository.FindAll(x => x.MaKH_ATBX.ToString() + "atbucxa" == maKeHoach).ToList();
                foreach (var item in qtracs)
                {
                    thoigian = new Ehs_ThoiGianThucHien()
                    {
                        MaKeHoach = maKeHoach,
                        MaNgayChiTiet = item.Id + "atbucxa",
                        ThoiGianBatDau = item.NgayBatDau,
                        ThoiGianKetThuc = item.NgayKetThuc,
                        Status = item.Status == "TODO" ? "Ready To Start" : item.Status,
                        KetQua = item.KetQua
                    };

                    result.Add(thoigian);
                }
            }

            return result.OrderBy(x => x.ThoiGianBatDau).ToList();

        }

        // Lấy folder cha chung của các file
        public string GetFolderKetQua(string maNgayChitiet)
        {
            var files = _ehsFileRepository.FindAll(x => x.MaNgayChiTiet == maNgayChitiet).ToList();

            if (files.Count == 0)
            {
                return "";
            }

            string folder = "/";
            List<string> itemFolders = new List<string>();
            string[] strArr;
            int k = 0;
            foreach (var item in files)
            {
                if (k++ == 0)
                {
                    itemFolders.AddRange(item.UrlFile.Split("/"));
                }
                else
                {
                    strArr = item.UrlFile.Split("/");
                    for (int i = 0; i < strArr.Length; i++)
                    {
                        if (itemFolders.ToList().Count >= i + 1)
                        {
                            if (itemFolders[i] != strArr[i])
                            {
                                itemFolders.RemoveRange(i, itemFolders.Count - i);
                                break;
                            }
                        }
                    }
                }
            }

            foreach (var item in itemFolders.Where(x => x != ""))
            {
                folder += item + "/";
            }

            return folder;
        }

        /// <summary>
        /// Thống kê task theo kế hoạch
        /// </summary>
        /// <param name="maKeHoach"></param>
        /// <returns></returns>
        public TaskStatistics GetStatistics(string maKeHoach)
        {
            TaskStatistics statistic = new TaskStatistics();
            string year = DateTime.Now.Year.ToString();

            if (maKeHoach == "ffe65d73-1066-4f1b-af5b-0c0e33d494dd") // kham sk
            {
                var lst = _ngayKhamSKRepository.FindAll(x => x.EHS_KE_HOACH_KHAM_SK.Year == year, x => x.EHS_KE_HOACH_KHAM_SK).ToList();

                Task task;
                int i = 0;
                foreach (var item in lst)
                {
                    task = new Task()
                    {
                        STT = ++i,
                        NoiDung = item.NoiDung,
                        NgayBatDau = item.NgayBatDau,
                        NguoiPhuTrach = item.EHS_KE_HOACH_KHAM_SK.NguoiPhuTrach,
                        KetQua = item.KetQua,
                        Status = item.Status
                    };
                    statistic.Tasks.Add(task);
                }

                statistic.TotalTask = lst.Count;
                statistic.OverdueTask = lst.Where(x => (x.Status == "" || x.Status == null || x.Status == CommonConstants.TODO) && x.NgayBatDau.CompareTo(DateTime.Now.ToString("yyyy-MM-dd")) < 0).Count();
                statistic.CompleteTask = lst.Where(x => x.Status == CommonConstants.COMPLETED).Count();
                statistic.PendingTask = lst.Where(x => x.Status == CommonConstants.PENDING).Count();
                statistic.InprogressTask = lst.Where(x => x.Status == CommonConstants.INPROGRESS).Count();
                statistic.HoldTask = lst.Where(x => x.Status == "" || x.Status == null || x.Status == CommonConstants.TODO).Count();
                statistic.NGTask = lst.Where(x => x.KetQua == "NG").Count();
            }
            else if (maKeHoach == "8b5cb6e4-e925-4a8b-b14d-594b310b6a4f") // PCCC
            {
                var lst = _ngayPCCCRepository.FindAll(x => x.EHS_KEHOACH_PCCC.Year == year, x => x.EHS_KEHOACH_PCCC).ToList();

                Task task;
                int i = 0;
                foreach (var item in lst)
                {
                    task = new Task()
                    {
                        STT = ++i,
                        NoiDung = item.NoiDung,
                        NgayBatDau = item.NgayBatDau,
                        NguoiPhuTrach = item.EHS_KEHOACH_PCCC.NguoiPhuTrach,
                        KetQua = item.KetQua,
                        Status = item.Status
                    };
                    statistic.Tasks.Add(task);
                }

                statistic.TotalTask = lst.Count;
                statistic.OverdueTask = lst.Where(x => (x.Status == "" || x.Status == null || x.Status == CommonConstants.TODO) && x.NgayBatDau.CompareTo(DateTime.Now.ToString("yyyy-MM-dd")) < 0).Count();
                statistic.CompleteTask = lst.Where(x => x.Status == CommonConstants.COMPLETED).Count();
                statistic.PendingTask = lst.Where(x => x.Status == CommonConstants.PENDING).Count();
                statistic.InprogressTask = lst.Where(x => x.Status == CommonConstants.INPROGRESS).Count();
                statistic.HoldTask = lst.Where(x => x.Status == "" || x.Status == null || x.Status == CommonConstants.TODO).Count();
                statistic.NGTask = lst.Where(x => x.KetQua == "NG").Count();
            }
            else if (maKeHoach == "7c60f914-c6d2-453a-841f-5afd0fb4a3bc") // An toàn bức xạ
            {
                var lst = _ngayATBXRepository.FindAll(x => x.EHS_KEHOACH_ANTOAN_BUCXA.Year == year, x => x.EHS_KEHOACH_ANTOAN_BUCXA).ToList();

                Task task;
                int i = 0;
                foreach (var item in lst)
                {
                    task = new Task()
                    {
                        STT = ++i,
                        NoiDung = item.NoiDung,
                        NgayBatDau = item.NgayBatDau,
                        NguoiPhuTrach = item.EHS_KEHOACH_ANTOAN_BUCXA.NguoiPhuTrach,
                        KetQua = item.KetQua,
                        Status = item.Status
                    };
                    statistic.Tasks.Add(task);
                }

                statistic.TotalTask = lst.Count;
                statistic.OverdueTask = lst.Where(x => (x.Status == "" || x.Status == null || x.Status == CommonConstants.TODO) && x.NgayBatDau.CompareTo(DateTime.Now.ToString("yyyy-MM-dd")) < 0).Count();
                statistic.CompleteTask = lst.Where(x => x.Status == CommonConstants.COMPLETED).Count();
                statistic.PendingTask = lst.Where(x => x.Status == CommonConstants.PENDING).Count();
                statistic.InprogressTask = lst.Where(x => x.Status == CommonConstants.INPROGRESS).Count();
                statistic.HoldTask = lst.Where(x => x.Status == "" || x.Status == null || x.Status == CommonConstants.TODO).Count();
                statistic.NGTask = lst.Where(x => x.KetQua == "NG").Count();
            }
            else if (maKeHoach == "44ba2130-8336-4853-b226-7234e592c52c") // QUAN TRẮC
            {
                var lst = _ngayQuanTracRepository.FindAll(x => x.EHS_KEHOACH_QUANTRAC.Year == year, x => x.EHS_KEHOACH_QUANTRAC).ToList();

                Task task;
                int i = 0;
                foreach (var item in lst)
                {
                    task = new Task()
                    {
                        STT = ++i,
                        NoiDung = item.NoiDung,
                        NgayBatDau = item.NgayBatDau,
                        NguoiPhuTrach = item.EHS_KEHOACH_QUANTRAC.NguoiPhuTrach,
                        KetQua = item.KetQua,
                        Status = item.Status
                    };
                    statistic.Tasks.Add(task);
                }

                statistic.TotalTask = lst.Count;
                statistic.OverdueTask = lst.Where(x => (x.Status == "" || x.Status == null || x.Status == CommonConstants.TODO) && x.NgayBatDau.CompareTo(DateTime.Now.ToString("yyyy-MM-dd")) < 0).Count();
                statistic.CompleteTask = lst.Where(x => x.Status == CommonConstants.COMPLETED).Count();
                statistic.PendingTask = lst.Where(x => x.Status == CommonConstants.PENDING).Count();
                statistic.InprogressTask = lst.Where(x => x.Status == CommonConstants.INPROGRESS).Count();
                statistic.HoldTask = lst.Where(x => x.Status == "" || x.Status == null || x.Status == CommonConstants.TODO).Count();
                statistic.NGTask = lst.Where(x => x.KetQua == "NG").Count();
            }
            else if (maKeHoach == "5f2ad5b3-8e86-4cd8-be84-ef4e7d82212b") // VS ATLD
            {
                var lst = _ngayATVSLDRepository.FindAll(x => x.EHS_KEHOACH_DAOTAO_ANTOAN_VSLD.Year == year, x => x.EHS_KEHOACH_DAOTAO_ANTOAN_VSLD).ToList();

                Task task;
                int i = 0;
                foreach (var item in lst)
                {
                    task = new Task()
                    {
                        STT = ++i,
                        NoiDung = item.NoiDung,
                        NgayBatDau = item.NgayBatDau,
                        NguoiPhuTrach = item.EHS_KEHOACH_DAOTAO_ANTOAN_VSLD.NguoiPhuTrach,
                        KetQua = item.KetQua,
                        Status = item.Status
                    };
                    statistic.Tasks.Add(task);
                }

                statistic.TotalTask = lst.Count;
                statistic.OverdueTask = lst.Where(x => (x.Status == "" || x.Status == null || x.Status == CommonConstants.TODO) && x.NgayBatDau.CompareTo(DateTime.Now.ToString("yyyy-MM-dd")) < 0).Count();
                statistic.CompleteTask = lst.Where(x => x.Status == CommonConstants.COMPLETED).Count();
                statistic.PendingTask = lst.Where(x => x.Status == CommonConstants.PENDING).Count();
                statistic.InprogressTask = lst.Where(x => x.Status == CommonConstants.INPROGRESS).Count();
                statistic.HoldTask = lst.Where(x => x.Status == "" || x.Status == null || x.Status == CommonConstants.TODO).Count();
                statistic.NGTask = lst.Where(x => x.KetQua == "NG").Count();
            }

            else if (maKeHoach == "5bbdc8cc-fc22-4be9-a3b8-f468ef04efe0") // Kiem Dinh MM
            {
                var lst = _ngayKiemDinhMMRepository.FindAll(x => x.EHS_KEHOACH_KIEMDINH_MAYMOC.Year == year, x => x.EHS_KEHOACH_KIEMDINH_MAYMOC).ToList();

                Task task;
                int i = 0;
                foreach (var item in lst)
                {
                    task = new Task()
                    {
                        STT = ++i,
                        NoiDung = item.NoiDung,
                        NgayBatDau = item.NgayBatDau,
                        NguoiPhuTrach = item.EHS_KEHOACH_KIEMDINH_MAYMOC.NguoiPhuTrach,
                        KetQua = item.KetQua,
                        Status = item.Status
                    };
                    statistic.Tasks.Add(task);
                }

                statistic.TotalTask = lst.Count;
                statistic.OverdueTask = lst.Where(x => (x.Status == "" || x.Status == null || x.Status == CommonConstants.TODO) && x.NgayBatDau.CompareTo(DateTime.Now.ToString("yyyy-MM-dd")) < 0).Count();
                statistic.CompleteTask = lst.Where(x => x.Status == CommonConstants.COMPLETED).Count();
                statistic.PendingTask = lst.Where(x => x.Status == CommonConstants.PENDING).Count();
                statistic.InprogressTask = lst.Where(x => x.Status == CommonConstants.INPROGRESS).Count();
                statistic.HoldTask = lst.Where(x => x.Status == "" || x.Status == null || x.Status == CommonConstants.TODO).Count();
                statistic.NGTask = lst.Where(x => x.KetQua == "NG").Count();
            }

            return statistic;
        }
    }
}
