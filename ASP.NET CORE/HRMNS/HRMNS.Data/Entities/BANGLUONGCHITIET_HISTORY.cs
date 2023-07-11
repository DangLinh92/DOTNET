using HRMNS.Data.Interfaces;
using HRMS.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HRMNS.Data.Entities
{
    [Table("BANGLUONGCHITIET_HISTORY")]
    public class BANGLUONGCHITIET_HISTORY : DomainEntity<int>, IDateTracking
    {
        [StringLength(50)]
        public string MaNV { get; set; }

        [StringLength(50)]
        public string HieuLucCapBac { get; set; }

        [StringLength(50)]
        public string MaBoPhan { get; set; }

        [StringLength(50)]
        public string Grade { get; set; }

        [StringLength(50)]
        public string TenNV { get; set; }

        [StringLength(50)]
        public string BoPhan { get; set; }

        [StringLength(50)]
        public string ChucVu { get; set; }

        [StringLength(50)]
        public string NgayVao { get; set; }

        public double BasicSalary { get; set; }
        public double LivingAllowance { get; set; }
        public double PositionAllowance { get; set; }
        public double AbilityAllowance { get; set; }
        public double SeniorityAllowance { get; set; }
        public double HarmfulAllowance { get; set; }
        public double DailySalary { get; set; }
        public double TongNgayCongThucTe { get; set; }
        public double NgayCongThuViecBanNgay { get; set; }
        public double NgayCongThuViecBanDem { get; set; }
        public double NgayCongChinhThucBanNgay { get; set; }
        public double NgayCongChinhThucBanDem { get; set; }
        public double NghiViecCoLuong { get; set; }
        public double GioLamThemTrongTV_150 { get; set; }
        public double GioLamThemTrongTV_200 { get; set; }
        public double GioLamThemTrongTV_210 { get; set; }
        public double GioLamThemTrongTV_270 { get; set; }
        public double GioLamThemTrongTV_300 { get; set; }
        public double GioLamThemTrongTV_390 { get; set; }
        public double GioLamThemTrongCT_150 { get; set; }
        public double GioLamThemTrongCT_200 { get; set; }
        public double GioLamThemTrongCT_210 { get; set; }
        public double GioLamThemTrongCT_270 { get; set; }
        public double GioLamThemTrongCT_300 { get; set; }
        public double GioLamThemTrongCT_390 { get; set; }
        public double SoNgayLamCaDemTruocLe_TV { get; set; }
        public double SoNgayLamCaDemTruocLe_CT { get; set; }
        public double SoNgayLamCaDem_TV { get; set; }
        public double SoNgayLamCaDem_CT { get; set; }
        public double HoTroThoiGianLamViecTV_150 { get; set; }
        public double HoTroThoiGianLamViecTV_200_NT { get; set; }
        public double HoTroThoiGianLamViecTV_200_CN { get; set; }
        public double HoTroThoiGianLamViecTV_270 { get; set; }
        public double HoTroThoiGianLamViecTV_300 { get; set; }
        public double HoTroThoiGianLamViecTV_390 { get; set; }
        public double HoTroThoiGianLamViecCT_150 { get; set; }
        public double HoTroThoiGianLamViecCT_200_NT { get; set; }
        public double HoTroThoiGianLamViecCT_200_CN { get; set; }
        public double HoTroThoiGianLamViecCT_270 { get; set; }
        public double HoTroThoiGianLamViecCT_300 { get; set; }
        public double HoTroThoiGianLamViecCT_390 { get; set; }
        public double HoTroNgayThanhLapCty_CaNgayTV { get; set; }
        public double HoTroNgayThanhLapCty_CaNgayCT { get; set; }
        public double HoTroNgayThanhLapCty_CaDemTV_TruocLe { get; set; }
        public double HoTroNgayThanhLapCty_CaDemCT_TruocLe { get; set; }
        public double NghiKhamThai { get; set; }
        public double NghiViecKhongThongBao { get; set; }
        public double SoNgayNghiBu_AL30 { get; set; }
        public double SoNgayNghiBu_NB { get; set; }
        public double HoTroPCCC_CoSo { get; set; }
        public double HoTroAT_SinhVien { get; set; }
        public double TV_NghiKhongLuong { get; set; }
        public double NghiKhongLuong { get; set; }
        public double Probation_Late_Come_Early_Leave_Time { get; set; }
        public double Official_Late_Come_Early_Leave_Time { get; set; }

        [StringLength(50)]
        public string ThuocDoiTuong_BHXH { get; set; }

        public double TruQuyPhongChongThienTai { get; set; }
        public double Thuong { get; set; }
        public decimal SoNguoiPhuThuoc { get; set; }

        [StringLength(50)]
        public string Note { get; set; }

        public decimal InsentiveStandard { get; set; }

        [StringLength(50)]
        public string DanhGia { get; set; }

        public decimal HoTroCongDoan { get; set; }

        [StringLength(50)]
        public string SoTK { get; set; }

        [StringLength(50)]
        public string DoiTuongThamGiaCD { get; set; }

        [StringLength(50)]
        public string DoiTuongTruyThuBHYT { get; set; }

        public int SoConNho { get; set; }

        public double SoNgayNghi70 { get; set; }

        [StringLength(50)]
        public string NgayNghiViec { get; set; }

        public double DieuChinhCong_Total { get; set; }
        public double TraTienPhepNam_Total { get; set; }
        public double TT_Tien_GioiThieu { get; set; }

        [StringLength(50)]
        public string ThangNam { get; set; }

        [StringLength(50)]
        public string DoiTuongPhuCapDocHai { get; set; }

        [StringLength(50)]
        public string DateCreated { get; set; }

        [StringLength(50)]
        public string DateModified { get; set; }

        [StringLength(50)]
        public string UserCreated { get; set; }

        [StringLength(50)]
        public string UserModified { get; set; }
    }
}
