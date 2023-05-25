using HRMNS.Application.ViewModels;
using HRMNS.Application.ViewModels.System;
using HRMNS.Utilities.Constants;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMNS.Application.ViewModels.HR
{
    public class NhanVienViewModel
    {
        public string Id { get; set; }
        public string TenNV { get; set; }

        public string MaChucDanh { get; set; }

        public string MaBoPhan { get; set; }
        public int? MaBoPhanChiTiet { get; set; }

        public string GioiTinh { get; set; }

        public string NgaySinh { get; set; }

        public string NoiSinh { get; set; }

        public string TinhTrangHonNhan { get; set; }

        public string DanToc { get; set; }

        public string TonGiao { get; set; }

        public string DiaChiThuongTru { get; set; }

        public string SoDienThoai { get; set; }

        public string SoDienThoaiNguoiThan { get; set; }

        public string QuanHeNguoiThan { get; set; }

        public string CMTND { get; set; }

        public string NgayCapCMTND { get; set; }

        public string NoiCapCMTND { get; set; }

        public string SoTaiKhoanNH { get; set; }

        public string TenNganHang { get; set; }

        public string TruongDaoTao { get; set; }

        public string NgayVao { get; set; }

        public string NguyenQuan { get; set; }

        public string DChiHienTai { get; set; }

        public string KyLuatLD { get; set; }

        public string MaBHXH { get; set; }

        public string MaSoThue { get; set; }

        public int SoNguoiGiamTru { get; set; }

        public string Email { get; set; }

        public string Note { get; set; }

        public string NgayNghiViec { get; set; }

        public string Status { get; set; }

        public string NoiTuyenDung { get; set; }

        public string ChucVu2 { get; set; } // OP, STAFF, STAFF PM

        public string TrucTiepSX { get; set; }

        private string _image;
        public string Image
        {
            get
            {
                if (string.IsNullOrEmpty(_image))
                {
                    return CommonConstants.DefaultAvatar;
                }
                return _image;
            }
            set
            {
                _image = value;
                if (string.IsNullOrEmpty(value))
                {
                    _image = CommonConstants.DefaultAvatar;
                }
            }
        }

        public string DateCreated { get; set; }

        public string DateModified { get; set; }

        public string UserCreated { get; set; }

        public string UserModified { get; set; }

        public string IsDelete { get; set; }
        public string MaBoPhan2 { get; set; }
        
        public BoPhanViewModel BOPHAN { get; set; }
        public ICollection<BHXHViewModel> HR_BHXH { get; set; }
        public ChucDanhViewModel HR_CHUCDANH { get; set; }
        public ICollection<ChungChiNhanVienViewModel> HR_CHUNGCHI_NHANVIEN { get; set; }
        public ICollection<HopDongViewModel> HR_HOPDONG { get; set; }
        public ICollection<KeKhaiBaoHiemViewModel> HR_KEKHAIBAOHIEM { get; set; }
        public ICollection<QuaTrinhLamViecViewModel> HR_QUATRINHLAMVIEC { get; set; }
        public ICollection<TinhTrangHoSoViewModel> HR_TINHTRANGHOSO { get; set; }
        public BoPhanDetailViewModel HR_BO_PHAN_DETAIL { get; set; }
        public ICollection<PhepNamViewModel> HR_PHEP_NAM { get; set; }
        public ICollection<Training_NhanVienViewModel> TRAINING_NHANVIEN { get; set; }
        public ICollection<NhanVienThaiSanViewModel> HR_THAISAN_CONNHO { get; set; }
    }
}
