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
                c.MaBHXH, c.MaSoThue, c.SoNguoiGiamTru, c.Email, c.Note, c.NgayNghiViec, c.Status, c.Image, c.IsDelete, c.MaBoPhanChiTiet));

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
                .ConstructUsing(c => new EHS_DM_KEHOACH(c.Id, c.TenKeHoach_VN, c.TenKeHoach_KR));
            CreateMap<EhsLuatDinhKeHoachViewModel, EHS_LUATDINH_KEHOACH>()
                .ConstructUsing(c => new EHS_LUATDINH_KEHOACH(c.Id, c.NoiDungLuatDinh, c.MaKeHoach));

            CreateMap<EhsDeMucKeHoachViewModel, EHS_DEMUC_KEHOACH>()
                .ConstructUsing(c => new EHS_DEMUC_KEHOACH(c.Id, c.TenKeDeMuc_VN, c.TenKeDeMuc_KR));

            CreateMap<EhsNoiDungViewModel, EHS_NOIDUNG>()
                .ConstructUsing(c => new EHS_NOIDUNG(c.Id, c.NoiDung, c.MaKeHoach, c.MaDeMucKH));

            CreateMap<EhsLuatDinhDeMucKeHoachViewModel, EHS_LUATDINH_DEMUC_KEHOACH>()
                .ConstructUsing(c => new EHS_LUATDINH_DEMUC_KEHOACH(c.Id, c.LuatDinhLienQuan, c.MaDeMuc));

            CreateMap<EhsNoiDungKeHoachViewModel, EHS_NOIDUNG_KEHOACH>()
                .ConstructUsing(c => new EHS_NOIDUNG_KEHOACH(c.Id, c.Year, c.MaNoiDung, c.NhaThau, c.ChuKy, c.YeuCau, c.Note,c.NgayThucHien,c.ThoiGian_ThucHien,c.ViTri,c.SoLuong,c.NgayKhaiBaoThietBi,c.ThoiGianThongBao));
        }
    }
}
