using AutoMapper;
using HRMNS.Application.ViewModels.HR;
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
           .ConstructUsing(c => new NHANVIEN_CALAMVIEC(c.MaNV, c.Danhmuc_CaLviec, c.BatDau_TheoCa, c.KetThuc_TheoCa,c.Approved));
        }
    }
}
