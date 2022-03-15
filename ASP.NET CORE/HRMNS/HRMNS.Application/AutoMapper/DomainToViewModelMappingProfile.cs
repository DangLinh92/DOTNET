using AutoMapper;
using HRMNS.Application.ViewModels.HR;
using HRMNS.Application.ViewModels.System;
using HRMNS.Application.ViewModels.Time_Attendance;
using HRMNS.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMNS.Application.AutoMapper
{
    public class DomainToViewModelMappingProfile : Profile
    {
        public DomainToViewModelMappingProfile()
        {
            CreateMap<HR_LOAIHOPDONG, LoaiHopDongViewModel>();
            CreateMap<LOAICHUNGCHI, LoaiChungChiViewModel>();
            CreateMap<CHUNG_CHI, ChungChiViewModel>();
            CreateMap<HR_CHEDOBH, CheDoBaoHiemViewModel>();
            CreateMap<BOPHAN, BoPhanViewModel>();
            CreateMap<HR_BHXH, BHXHViewModel>();
            CreateMap<HR_CHUCDANH, ChucDanhViewModel>();
            CreateMap<HR_CHUNGCHI_NHANVIEN, ChungChiNhanVienViewModel>();
            CreateMap<HR_HOPDONG, HopDongViewModel>();
            CreateMap<HR_KEKHAIBAOHIEM, KeKhaiBaoHiemViewModel>();
            CreateMap<HR_QUATRINHLAMVIEC, QuaTrinhLamViecViewModel>();
            CreateMap<HR_TINHTRANGHOSO, TinhTrangHoSoViewModel>();
            CreateMap<HR_NHANVIEN, NhanVienViewModel>();
            CreateMap<FUNCTION, FunctionViewModel>();
            CreateMap<HR_BO_PHAN_DETAIL, BoPhanDetailViewModel>();
            CreateMap<HR_PHEP_NAM, PhepNamViewModel>();

            CreateMap<CHAM_CONG_LOG, ChamCongLogViewModel>();
            CreateMap<NHANVIEN_CALAMVIEC, NhanVien_CalamViecViewModel>();
            CreateMap<DM_CA_LVIEC, DMCalamviecViewModel>();
            CreateMap<SETTING_TIME_CA_LVIEC, SettingTimeCalamviecViewModel>();

            CreateMap<DANGKY_OT_NHANVIEN, DangKyOTNhanVienViewModel>();
            CreateMap<NGAY_LE_NAM, NgayLeNamViewModel>();
            CreateMap<DM_NGAY_LAMVIEC, DMucNgayLamViecViewModel>();

            CreateMap<DANGKY_CHAMCONG_CHITIET, DangKyChamCongChiTietViewModel>();
            CreateMap<DANGKY_CHAMCONG_DACBIET, DangKyChamCongDacBietViewModel>();
            CreateMap<DM_DANGKY_CHAMCONG, DMDangKyChamCongViewModel>();
            CreateMap<DM_DIEUCHINH_CHAMCONG, DMDieuChinhChamCongViewModel>();
        }
    }
}
