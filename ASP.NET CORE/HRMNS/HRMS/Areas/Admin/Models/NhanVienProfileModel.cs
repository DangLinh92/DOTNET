using HRMNS.Application.ViewModels.HR;
using HRMNS.Utilities.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS.Areas.Admin.Models
{
    public class NhanVienProfileModel
    {
        public NhanVienProfileModel()
        {
            chungChis = new List<ChungChiNhanVienViewModel>();
            quaTrinhLamViecs = new List<QuaTrinhLamViecViewModel>();
            phepNams = new List<PhepNamViewModel>();
            hopDongs = new List<HopDongViewModel>();
        }

        // profile common
        private string _avartar;
        public string Avartar
        {
            get
            {
                if (string.IsNullOrEmpty(_avartar))
                {
                    return CommonConstants.DefaultAvatar;
                }
                else
                {
                    return _avartar;
                }
            }
            set
            {
                _avartar = value;
                if (string.IsNullOrEmpty(value))
                {
                    _avartar = CommonConstants.DefaultAvatar;
                }
            }
        }

        public string MaNhanVien { get; set; }
        public string TenNhanVien { get; set; }
        public string BoPhan { get; set; }
        public string BoPhanDetail { get; set; }
        public int? MaBoPhanDetail { get; set; }
        public string ChucDanh { get; set; }
        public string NgayVaoCongTy { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Birthday { get; set; }
        public string DCHienTai { get; set; }
        public string GioiTinh { get; set; }

        // So yeu li lich
        public string NoiSinh { get; set; }
        public string NguyenQuan { get; set; }
        public string DiaChiThuongTru { get; set; }
        public string DanToc { get; set; }
        public string TonGiao { get; set; }
        public string CMTND { get; set; }
        public string NgayCapCMTND { get; set; }
        public string NoiCapCMTND { get; set; }
        public string MaSoThue { get; set; }
        public int SoNguoiGiamTru { get; set; }
        public string TinhTrangHonNhan { get; set; }
        public string TruongDaoTao { get; set; }
        public string Note { get; set; }
        public string Status { get; set; }

        // Nghỉ viêc
        public string NgayNghiViec { get; set; }

        // Lien lac
        public string SoDienThoaiNguoiThan { get; set; }
        public string QuanHeNguoiThan { get; set; }

        // Bank Info
        public string TenNganHang { get; set; }
        public string SoTaiKhoanNH { get; set; }

        // Tinh Trang Ho So
        public TinhTrangHoSoViewModel tinhTrangHoSo { get; set; }

        // Ky Luat Lao Dong
        public string KyLuatLaoDong { get; set; }

        public string MaBHXH { get; set; }
        public string NgayThamGia { get; set; }
        public string NgayKetThuc { get; set; }

        // Bao Hiem
        public BHXHViewModel bHXHs { get; set; }

        // Bang cap , chung chi
        public List<ChungChiNhanVienViewModel> chungChis { get; set; }

        // Qua Trinh Lam Viec
        public List<QuaTrinhLamViecViewModel> quaTrinhLamViecs { get; set; }

        public List<PhepNamViewModel> phepNams { get; set; }

        public List<HopDongViewModel> hopDongs { get; set; }

        public List<KeKhaiBaoHiemViewModel> kekhaibaohiems { get; set; }
    }
}
