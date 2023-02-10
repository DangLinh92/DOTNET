using AutoMapper;
using HRMNS.Application.Interfaces;
using HRMNS.Application.ViewModels.EHS;
using HRMNS.Data.EF.Extensions;
using HRMNS.Data.Entities;
using HRMNS.Utilities.Common;
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
        }

        public EhsDanhMucKeHoachPageViewModel GetDataDanhMucKeHoachPage(Guid? maKeHoach)
        {
            EhsDanhMucKeHoachPageViewModel model = new EhsDanhMucKeHoachPageViewModel();
            model.EhsDMKeHoachViewModels = GetDMKehoach();

            if (maKeHoach == null)
            {
                maKeHoach = model.EhsDMKeHoachViewModels.FirstOrDefault()?.Id;
            }

            model.MaKeHoachActive = maKeHoach;
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
            List<EHS_KEHOACH_QUANTRAC> quantrac = _ehsKeHoachQuanTracRepository.FindAll(x => x.Year == year).OrderBy(x=>x.STT).ToList();
            List<EHS_KE_HOACH_KHAM_SK> khamsuckhoe = _ehsKeHoachKhamSKRepository.FindAll(x => x.Year == year).ToList();
            List<EHS_KEHOACH_DAOTAO_ANTOAN_VSLD> atld = _ehsKeHoachDaoTaoATLDRepository.FindAll(x => x.Year == year).OrderBy(x=>x.STT).ToList();
            List<EHS_KEHOACH_KIEMDINH_MAYMOC> kiemdinhmaymoc = _ehsKiemDinhMMRepository.FindAll(x => x.Year == year).OrderBy(x=>x.STT).ToList();
            List<EHS_KEHOACH_PCCC> phongchayCC = _ehsKeHoachPCCCRepository.FindAll(x => x.Year == year).OrderBy(x=>x.STT).ToList();
            List<EHS_KEHOACH_ANTOAN_BUCXA> antoanbucxa = _ehsKehoachATBXRepository.FindAll(x => x.Year == year).OrderBy(x=>x.STT).ToList();

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
                    Month_12 = kh.CostMonth_12
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
                    Month_12 = kh.CostMonth_12
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
                    Month_12 = kh.CostMonth_12
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
                    Month_12 = kh.CostMonth_12
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
                    TenNoiDung = kh.HangMuc ,
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
                    Month_12 = kh.CostMonth_12
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
                    Month_12 = kh.CostMonth_12
                };
                report.Add(item);
            }

            return report.OrderBy(x => x.OrderItem).ThenBy(x => x.STT).ToList();
        }
    }
}
