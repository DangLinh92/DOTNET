using AutoMapper;
using HRMNS.Application.ViewModels.EHS;
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
            CreateMap<CA_LVIEC, CaLamViecViewModel>();
            // CreateMap<SETTING_TIME_CA_LVIEC, SettingTimeCalamviecViewModel>();

            CreateMap<DANGKY_OT_NHANVIEN, DangKyOTNhanVienViewModel>();
            CreateMap<NGAY_LE_NAM, NgayLeNamViewModel>();
            CreateMap<DM_NGAY_LAMVIEC, DMucNgayLamViecViewModel>();

            CreateMap<DANGKY_CHAMCONG_CHITIET, DangKyChamCongChiTietViewModel>();
            CreateMap<DANGKY_CHAMCONG_DACBIET, DangKyChamCongDacBietViewModel>();
            CreateMap<DM_DANGKY_CHAMCONG, DMDangKyChamCongViewModel>();
            CreateMap<DM_DIEUCHINH_CHAMCONG, DMDieuChinhChamCongViewModel>();

            CreateMap<DC_CHAM_CONG, DCChamCongViewModel>();
            CreateMap<ATTENDANCE_OVERTIME, AttendanceOvertimeViewModel>();
            CreateMap<ATTENDANCE_RECORD, AttendanceRecordViewModel>();
            CreateMap<KY_HIEU_CHAM_CONG, KyHieuChamCongViewModel>();
            CreateMap<APP_ROLE, RoleViewModel>();
            CreateMap<PERMISSION, PermisstionViewModel>();
            CreateMap<HR_THAISAN_CONNHO, NhanVienThaiSanViewModel>();

            CreateMap<EHS_DM_KEHOACH, EhsDMKeHoachViewModel>();
            CreateMap<EHS_LUATDINH_KEHOACH, EhsLuatDinhKeHoachViewModel>();

            CreateMap<TRAINING_TYPE, TrainingTypeViewModel>();
            CreateMap<HR_TRAINING, Hr_TrainingViewModel>();
            CreateMap<TRAINING_NHANVIEN, Training_NhanVienViewModel>();
            CreateMap<EVENT_SHEDULE_PARENT, EventScheduleParentViewModel>();
            CreateMap<HR_NHANVIEN_CHEDO_DB, NhanVienCheDoDBViewModel>();
            CreateMap<EHS_KEHOACH_QUANTRAC, EhsKeHoachQuanTracViewModel>();
            CreateMap<EHS_NGAY_THUC_HIEN_CHITIET_QUANTRAC, EhsNgayThucHienChiTietQuanTrac>();

            CreateMap<EHS_NHANVIEN_KHAM_SK, EhsNhanVienKhamSucKhoe>();
            CreateMap<EHS_KE_HOACH_KHAM_SK, EhsKeHoachKhamSKViewModel>();
            CreateMap<EHS_NGAY_THUC_HIEN_CHITIET_KHAM_SK, EhsNgayThucHienChiTietKhamSKViewModel>();

            CreateMap<EHS_KEHOACH_DAOTAO_ANTOAN_VSLD, EhsKeHoachDaoTaoATLDViewModel>();
            CreateMap<EHS_THOIGIAN_THUC_HIEN_DAOTAO_ATVSLD, EhsThoiGianThucHienDaoTaoATVSViewModel>();

            CreateMap<EHS_KEHOACH_PCCC, Ehs_KeHoach_PCCCViewModel>();
            CreateMap<EHS_THOIGIAN_THUC_HIEN_PCCC, EhsThoiGianThucHienPCCCViewModel>();

            CreateMap<EHS_KEHOACH_ANTOAN_BUCXA, EhsKeHoachAnToanBucXaViewModel>();
            CreateMap<EHS_THOIGIAN_THUC_HIEN_ANTOAN_BUCXA, EhsThoiGianThucHienAnToanBucXaViewModel>();
        }
    }
}
