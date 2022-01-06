using AutoMapper;
using HRMNS.Application.ViewModels.HR;
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
                (c.TenNV,c.MaChucDanh,c.MaBoPhan,c.GioiTinh,c.NgaySinh,c.NoiSinh,c.TinhTrangHonNhan,c.DanToc,c.TonGiao,
                c.DiaChiThuongTru,c.SoDienThoai,c.SoDienThoaiNguoiThan,c.QuanHeNguoiThan,c.CMTND,c.NgayCapCMTND,c.NoiCapCMTND,
                c.SoTaiKhoanNH,c.TenNganHang,c.TruongDaoTao,c.NgayVao,c.NguyenQuan,c.DChiHienTai,c.KyLuatLD,
                c.MaBHXH,c.MaSoThue,c.SoNguoiGiamTru,c.Email,c.Note,c.NgayNghiViec,c.Status,c.Image,c.IsDelete));
        }
    }
}
