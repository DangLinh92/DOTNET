using HRMNS.Application.ViewModels;
using HRMNS.Application.ViewModels.System;
using HRMNS.Data.Entities;
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
        public ICollection<NHANVIEN_INFOR_EX> NHANVIEN_INFOR_EX { get; set; }
    }

    public class PayslipItem
    {
        public string Month { get; set; }
        public string Month2 { get; set; }
        public string Code { get; set; }
        public string Ten { get; set; }
        public string BP { get; set; }
        public string Ngayvao { get; set; }
        public string LCB { get; set; }
        public string PCDS { get; set; }
        public string PCCV { get; set; }
        public string PC_NL { get; set; }
        public string Luong_D { get; set; }
        public string PhepNamTon { get; set; }
        public string PCTN { get; set; }
        public string PCDH { get; set; }
        public string Luong_H { get; set; }

        public string TV_ngay { get; set; }
        public string TV_dem { get; set; }
        public string CT_ngay { get; set; }
        public string CT_dem { get; set;}
        public string Nghi_co_luong { get; set;}
        public string Cong_ngay { get; set;}
        public string Cong_dem { get; set;}
        public string TTien_nghi_co_luong { get; set;}
        public string nghi_KL { get; set;}
        public string TT_lviec { get; set;}
        public string Luong_theo_ngay_cong { get; set;}
        public string OT_time_150 { get; set;}
        public string OT_time_200 { get; set;}
        public string OT_time_210 { get; set;}
        public string OT_time_270 { get; set;}
        public string OT_time_300 { get; set;}
        public string OT_time_390 { get; set;}
        public string OT_time_260 { get; set;}

        public string Cong15 { get; set;}
        public string Cong20 { get; set;}
        public string Cong21 { get; set;}
        public string Cong27 { get; set;}
        public string Cong30 { get; set;}
        public string COng39 { get; set;}
        public string COng26 { get; set;}

        public string HT_15_Total { get; set;}
        public string HT_200_Total { get; set;}
        public string HT_270_Total { get; set;}
        public string HT_300_Total { get; set;}
        public string HT_390_Total { get; set;}

        public string Cong151 { get; set;}
        public string Cong201 { get; set;}
        public string Cong271 { get; set;}
        public string Cong301 { get; set;}
        public string COng391 { get; set;}

        public string Tong_OT { get; set;}
        public string Tong_HTLV { get; set;}

        public string Ca_ngay_TV { get; set;}
        public string Ca_ngay_CT { get; set;}
        public string ca_dem_TV_ky_niem_truoc_le { get; set;}
        public string ca_dem_CT_ky_niem_truoc_le { get; set;}
        public string Thanh_tien { get; set;}

        public string Nghi_Bu_AL30 { get; set;}
        public string Nghi_Bu_NB { get; set;}
        public string Ho_tro_PC_NB { get; set;}
        public string Ho_tro_luong_NB { get; set;}
        public string Tong_ho_tro_NB { get; set;}

        public string so_ngay_nghi_70 { get; set;}
        public string thanh_tien_nghi_70 { get; set;}


        public string Cong_them { get; set;}
        public string Chuyencan { get; set;}
        public string Incentive { get; set;}
        public string Thanh_toan_PN { get; set;}
        public string HT_gui_tre { get; set;}
        public string HT_PCCC_co_so { get; set;}
        public string HT_ATNVSV { get; set;}
        public string HT_CD { get; set;}
        public string TN_khac { get; set;}
        public string HT_Sinh_ly { get; set;}
        public string Dem_TV { get; set;}
        public string Ttien { get; set;}
        public string Dem_CT { get; set;}
        public string Ttien1 { get; set;}

        public string Cong_tru { get; set;}
        public string BHXH { get; set;}
        public string Truy_thu_BHYT { get; set;}
        public string Cong_doan { get; set;}
        public string thue_TNCN { get; set;}
        public string hmuon { get; set;}
        public string Di_muon { get; set;}
        public string tru_khac { get; set;}
        public string Quy_PCTT { get; set;}
        public string Thuc_nhan { get; set;}

        public string password {  get; set;}
        public string NamSinh { get; set;}
    }
}
