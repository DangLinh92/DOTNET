﻿using AutoMapper;
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
    public class ViewModelToDomainMappingProfile : Profile
    {
        public ViewModelToDomainMappingProfile()
        {
            CreateMap<NhanVienViewModel, HR_NHANVIEN>()
                .ConstructUsing(c => new HR_NHANVIEN
                (c.Id, c.TenNV, c.MaChucDanh, c.MaBoPhan, c.GioiTinh, c.NgaySinh, c.NoiSinh, c.TinhTrangHonNhan, c.DanToc, c.TonGiao,
                c.DiaChiThuongTru, c.SoDienThoai, c.SoDienThoaiNguoiThan, c.QuanHeNguoiThan, c.CMTND, c.NgayCapCMTND, c.NoiCapCMTND,
                c.SoTaiKhoanNH, c.TenNganHang, c.TruongDaoTao, c.NgayVao, c.NguyenQuan, c.DChiHienTai, c.KyLuatLD,
                c.MaBHXH, c.MaSoThue, c.SoNguoiGiamTru, c.Email, c.Note, c.NgayNghiViec, c.Status, c.Image, c.IsDelete, c.MaBoPhanChiTiet, c.NoiTuyenDung));

            CreateMap<BHXHViewModel, HR_BHXH>()
                .ConstructUsing(c => new HR_BHXH(c.Id, c.MaNV, c.NgayThamGia, c.NgayKetThuc));

            CreateMap<PhepNamViewModel, HR_PHEP_NAM>()
               .ConstructUsing(c => new HR_PHEP_NAM(c.MaNhanVien, c.SoPhepNam, c.SoPhepConLai, c.Year));

            CreateMap<TinhTrangHoSoViewModel, HR_TINHTRANGHOSO>()
               .ConstructUsing(c => new HR_TINHTRANGHOSO(c.Id, c.MaNV, c.SoYeuLyLich, c.CMTND, c.SoHoKhau, c.GiayKhaiSinh, c.BangTotNghiep, c.XacNhanDanSu, c.AnhThe));

            CreateMap<HopDongViewModel, HR_HOPDONG>()
              .ConstructUsing(c => new HR_HOPDONG(c.MaHD, c.MaNV, c.TenHD, c.LoaiHD, c.NgayTao, c.NgayKy, c.NgayHieuLuc, c.NgayHetHieuLuc, c.Status, c.DayNumberNoti));

            CreateMap<QuaTrinhLamViecViewModel, HR_QUATRINHLAMVIEC>()
             .ConstructUsing(c => new HR_QUATRINHLAMVIEC(c.MaNV, c.TieuDe, c.Note, c.ThơiGianBatDau, c.ThoiGianKetThuc));

            CreateMap<KeKhaiBaoHiemViewModel, HR_KEKHAIBAOHIEM>()
            .ConstructUsing(c => new HR_KEKHAIBAOHIEM(c.MaNV, c.CheDoBH, c.NgayBatDau, c.NgayKetThuc, c.NgayThanhToan, c.SoTienThanhToan));

            CreateMap<NhanVien_CalamViecViewModel, NHANVIEN_CALAMVIEC>()
           .ConstructUsing(c => new NHANVIEN_CALAMVIEC(c.MaNV, c.Danhmuc_CaLviec, c.BatDau_TheoCa, c.KetThuc_TheoCa, c.Approved, c.CaLV_DB));

            CreateMap<DangKyOTNhanVienViewModel, DANGKY_OT_NHANVIEN>()
            .ConstructUsing(c => new DANGKY_OT_NHANVIEN(c.NgayOT, c.MaNV, c.DM_NgayLViec, c.Approve, c.ApproveLV2, c.ApproveLV3, c.HeSoOT, c.SoGioOT, c.NoiDung));

            CreateMap<NgayLeNamViewModel, NGAY_LE_NAM>()
            .ConstructUsing(c => new NGAY_LE_NAM(c.Id, c.TenNgayLe, c.KyHieuChamCong, c.IslastHoliday));

            CreateMap<DMucNgayLamViecViewModel, DM_NGAY_LAMVIEC>()
               .ConstructUsing(c => new DM_NGAY_LAMVIEC(c.Id, c.Ten_NgayLV));

            CreateMap<DangKyChamCongChiTietViewModel, DANGKY_CHAMCONG_CHITIET>()
               .ConstructUsing(c => new DANGKY_CHAMCONG_CHITIET(c.Id, c.TenChiTiet, c.PhanLoaiDM, c.KyHieuChamCong));

            CreateMap<DangKyChamCongDacBietViewModel, DANGKY_CHAMCONG_DACBIET>()
             .ConstructUsing(c => new DANGKY_CHAMCONG_DACBIET(c.MaNV, c.MaChamCong_ChiTiet, c.NoiDung, c.NgayBatDau, c.NgayKetThuc, c.Approve, c.ApproveLV2, c.ApproveLV3));

            CreateMap<DMDangKyChamCongViewModel, DM_DANGKY_CHAMCONG>()
            .ConstructUsing(c => new DM_DANGKY_CHAMCONG(c.Id, c.TieuDe));

            CreateMap<DMDieuChinhChamCongViewModel, DM_DIEUCHINH_CHAMCONG>()
          .ConstructUsing(c => new DM_DIEUCHINH_CHAMCONG(c.Id, c.TieuDe));

            CreateMap<DCChamCongViewModel, DC_CHAM_CONG>()
           .ConstructUsing(c => new DC_CHAM_CONG(c.Id, c.DM_DieuChinhCong, c.MaNV, c.NgayCanDieuChinh_From, c.NgayCanDieuChinh_To, c.NoiDungDC, c.GiaTriBoXung, c.TrangThaiChiTra));

            CreateMap<RoleViewModel, APP_ROLE>()
          .ConstructUsing(c => new APP_ROLE(c.Name, c.Description));

            CreateMap<PermisstionViewModel, PERMISSION>()
         .ConstructUsing(c => new PERMISSION(c.Id, c.RoleId, c.FunctionId, c.CanCreate, c.CanRead, c.CanUpdate, c.CanDelete, c.ApproveL1, c.ApproveL2, c.ApproveL3));

            CreateMap<NhanVienThaiSanViewModel, HR_THAISAN_CONNHO>()
            .ConstructUsing(c => new HR_THAISAN_CONNHO(c.Id, c.MaNV, c.CheDoThaiSan, c.FromDate, c.ToDate));

            CreateMap<EhsDMKeHoachViewModel, EHS_DM_KEHOACH>()
                .ConstructUsing(c => new EHS_DM_KEHOACH(c.Id, c.TenKeHoach_VN, c.TenKeHoach_KR, c.OrderDM));
            CreateMap<EhsLuatDinhKeHoachViewModel, EHS_LUATDINH_KEHOACH>()
                .ConstructUsing(c => new EHS_LUATDINH_KEHOACH(c.Id, c.NoiDungLuatDinh, c.MaKeHoach));

            CreateMap<TrainingTypeViewModel, TRAINING_TYPE>()
              .ConstructUsing(c => new TRAINING_TYPE(c.TrainName, c.Description, c.Status));

            CreateMap<Hr_TrainingViewModel, HR_TRAINING>()
              .ConstructUsing(c => new HR_TRAINING(c.Id, c.TrainnigType, c.Trainer, c.FromDate, c.ToDate, c.Description, c.Cost));

            CreateMap<Training_NhanVienViewModel, TRAINING_NHANVIEN>()
             .ConstructUsing(c => new TRAINING_NHANVIEN(c.MaNV, c.TrainnigId));

            CreateMap<EventScheduleParentViewModel, EVENT_SHEDULE_PARENT>()
             .ConstructUsing(c => new EVENT_SHEDULE_PARENT(c.Id, c.Subject, c.StartEvent, c.EndEvent, c.Repeat, c.Description, c.BoPhan, c.TimeAlert,
                                                           c.MaNoiDungKH, c.StartTime, c.EndTime, c.IsAllDay, c.RecurrenceRule,
                                                           c.StartTimezone, c.EndTimezone, c.RecurrenceID, c.RecurrenceException, c.ConferenceId, c.Location));

            CreateMap<NhanVienCheDoDBViewModel, HR_NHANVIEN_CHEDO_DB>()
           .ConstructUsing(c => new HR_NHANVIEN_CHEDO_DB(c.MaNhanVien, c.CheDoDB, c.Note));

            CreateMap<EhsKeHoachQuanTracViewModel, EHS_KEHOACH_QUANTRAC>()
      .ConstructUsing(c => new EHS_KEHOACH_QUANTRAC(c.STT, c.MaDMKeHoach, c.Demuc, c.LuatDinhLienQuan, c.NoiDung, c.ChuKyThucHien, c.Year, c.Month_1, c.Month_2,
      c.Month_3, c.Month_4, c.Month_5, c.Month_6, c.Month_7, c.Month_8, c.Month_9, c.Month_10, c.Month_11, c.Month_12, c.CostMonth_1, c.CostMonth_2, c.CostMonth_3, c.CostMonth_4,
      c.CostMonth_5, c.CostMonth_6, c.CostMonth_7, c.CostMonth_8, c.CostMonth_9, c.CostMonth_10, c.CostMonth_11, c.CostMonth_12, c.KhuVucLayMau, c.NhaThau, c.NguoiPhuTrach, c.TienDoHoanThanh,
      c.LayMau_Month_1, c.LayMau_Month_2, c.LayMau_Month_3, c.LayMau_Month_4, c.LayMau_Month_5, c.LayMau_Month_6, c.LayMau_Month_7, c.LayMau_Month_8, c.LayMau_Month_9, c.LayMau_Month_10, c.LayMau_Month_11, c.LayMau_Month_12));

            CreateMap<EhsNgayThucHienChiTietQuanTrac, EHS_NGAY_THUC_HIEN_CHITIET_QUANTRAC>()
         .ConstructUsing(c => new EHS_NGAY_THUC_HIEN_CHITIET_QUANTRAC(c.MaEvent, c.MaKHQuanTrac, c.NoiDung, c.NgayBatDau, c.NgayKetThuc));

            CreateMap<EhsNhanVienKhamSucKhoe, EHS_NHANVIEN_KHAM_SK>()
       .ConstructUsing(c => new EHS_NHANVIEN_KHAM_SK(c.MaKHKhamSK, c.ThoiGianKhamSK, c.MaNV, c.TenNV, c.Section, c.Note));

            CreateMap<EhsKeHoachKhamSKViewModel, EHS_KE_HOACH_KHAM_SK>()
     .ConstructUsing(c => new EHS_KE_HOACH_KHAM_SK(c.NoiDung, c.LuatDinhLienQuan, c.ChuKyThucHien, c.Year, c.CostMonth_1, c.CostMonth_2,
     c.CostMonth_3, c.CostMonth_4, c.CostMonth_5, c.CostMonth_6, c.CostMonth_7, c.CostMonth_8, c.CostMonth_9, c.CostMonth_10, c.CostMonth_11, c.CostMonth_12, c.MaDMKeHoach, c.NhaThau, c.NguoiPhuTrach));

            CreateMap<EhsNgayThucHienChiTietKhamSKViewModel, EHS_NGAY_THUC_HIEN_CHITIET_KHAM_SK>()
             .ConstructUsing(c => new EHS_NGAY_THUC_HIEN_CHITIET_KHAM_SK(c.MaEvent, c.MaKHKhamSK, c.NoiDung, c.NgayBatDau, c.NgayKetThuc));


            CreateMap<EhsKeHoachDaoTaoATLDViewModel, EHS_KEHOACH_DAOTAO_ANTOAN_VSLD>()
     .ConstructUsing(c => new EHS_KEHOACH_DAOTAO_ANTOAN_VSLD(
         c.MaDMKeHoach, c.STT, c.NoiDung, c.NguoiThamGia, c.ChuKyThucHien, c.ThoiGianCapLanDau, c.ThoiGianHuanLuyenLanDau,
     c.ThoiGianHuanLuyenLai, c.Year, c.ThoiGianDaoTao, c.NguoiPhuTrach, c.NhaThau,
     c.CostMonth_1,
     c.CostMonth_2,
     c.CostMonth_3,
     c.CostMonth_4,
     c.CostMonth_5,
     c.CostMonth_6,
     c.CostMonth_7,
     c.CostMonth_8,
     c.CostMonth_9,
     c.CostMonth_10,
     c.CostMonth_11,
     c.CostMonth_12));

            CreateMap<EhsThoiGianThucHienDaoTaoATVSViewModel, EHS_THOIGIAN_THUC_HIEN_DAOTAO_ATVSLD>()
            .ConstructUsing(c => new EHS_THOIGIAN_THUC_HIEN_DAOTAO_ATVSLD(c.MaEvent, c.MaKHDaoTaoATLD, c.NoiDung, c.NgayBatDau, c.NgayKetThuc));

            CreateMap<Ehs_KeHoach_PCCCViewModel, EHS_KEHOACH_PCCC>()
            .ConstructUsing(c => new EHS_KEHOACH_PCCC(
                c.MaDMKeHoach, c.STT, c.NoiDung, c.ChuKyThucHien, c.Year, c.ThoiGianDaoTao, c.NguoiPhuTrach, c.NhaThau,
                c.CostMonth_1, c.CostMonth_2, c.CostMonth_3, c.CostMonth_4, c.CostMonth_5,
                c.CostMonth_6, c.CostMonth_7, c.CostMonth_8, c.CostMonth_9, c.CostMonth_10,
                c.CostMonth_11, c.CostMonth_12, c.HangMuc));

            CreateMap<EhsThoiGianThucHienPCCCViewModel, EHS_THOIGIAN_THUC_HIEN_PCCC>()
            .ConstructUsing(c => new EHS_THOIGIAN_THUC_HIEN_PCCC(c.MaEvent, c.MaKH_PCCC, c.NoiDung, c.NgayBatDau, c.NgayKetThuc));

            CreateMap<EhsKeHoachAnToanBucXaViewModel, EHS_KEHOACH_ANTOAN_BUCXA>()
          .ConstructUsing(c => new EHS_KEHOACH_ANTOAN_BUCXA(c.MaDMKeHoach, c.STT, c.NoiDung, c.ChuKyThucHien, c.Year, c.ThoiGianDaoTao, c.NguoiPhuTrach,
          c.NhaThau, c.CostMonth_1, c.CostMonth_2, c.CostMonth_3, c.CostMonth_4, c.CostMonth_5, c.CostMonth_6, c.CostMonth_7, c.CostMonth_8, c.CostMonth_9, c.CostMonth_10,
          c.CostMonth_11, c.CostMonth_12, c.HangMuc, c.ThoiGianCapL1, c.ThoiGianCapLai_L1, c.ThoiGianCapLai_L2, c.ThoiGianCapLai_L3, c.YeuCau, c.QuyDinhVBPL, c.MaHieu, c.ThoiGianCapLai_L4));

            CreateMap<EhsThoiGianThucHienAnToanBucXaViewModel, EHS_THOIGIAN_THUC_HIEN_ANTOAN_BUCXA>()
           .ConstructUsing(c => new EHS_THOIGIAN_THUC_HIEN_ANTOAN_BUCXA(c.MaEvent, c.MaKH_ATBX, c.NoiDung, c.NgayBatDau, c.NgayKetThuc));

            CreateMap<EhsThoiGianKiemDinhMayMocViewModel, EHS_THOIGIAN_THUC_HIEN_KIEMDINH_MM>()
            .ConstructUsing(c => new EHS_THOIGIAN_THUC_HIEN_KIEMDINH_MM(c.MaEvent, c.MaKH_KDMM, c.NoiDung, c.NgayBatDau, c.NgayKetThuc));

            CreateMap<EhsKeHoachKiemDinhMayMocViewModel, EHS_KEHOACH_KIEMDINH_MAYMOC>()
            .ConstructUsing(c => new EHS_KEHOACH_KIEMDINH_MAYMOC(c.MaDMKeHoach, c.STT, c.TenMayMoc, c.ChuKyKiemDinh, c.SoLuongThietBi, c.ViTri, c.NguoiPhuTrach, c.NhaThau,
            c.CostMonth_1, c.CostMonth_2, c.CostMonth_3, c.CostMonth_4, c.CostMonth_5, c.CostMonth_6, c.CostMonth_7, c.CostMonth_8, c.CostMonth_9, c.CostMonth_10, c.CostMonth_11, c.CostMonth_12, c.LanKiemDinhKeTiep,
            c.LanKiemDinhKeTiep1, c.LanKiemDinhKeTiep2, c.LanKiemDinhKeTiep3,c.Year));
        }
    }
}
