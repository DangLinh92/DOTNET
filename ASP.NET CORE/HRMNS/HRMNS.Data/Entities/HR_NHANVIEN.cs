using HRMNS.Data.Interfaces;
using HRMS.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HRMNS.Data.Entities
{
    [Table("HR_NHANVIEN")]
    public class HR_NHANVIEN : DomainEntity<string>, IDateTracking
    {
        public HR_NHANVIEN()
        {
            HR_BHXH = new HashSet<HR_BHXH>();
            HR_HOPDONG = new HashSet<HR_HOPDONG>();
            HR_KEKHAIBAOHIEM = new HashSet<HR_KEKHAIBAOHIEM>();
            HR_QUATRINHLAMVIEC = new HashSet<HR_QUATRINHLAMVIEC>();
            HR_TINHTRANGHOSO = new HashSet<HR_TINHTRANGHOSO>();
            HR_CHUNGCHI_NHANVIEN = new HashSet<HR_CHUNGCHI_NHANVIEN>();
            HR_PHEP_NAM = new HashSet<HR_PHEP_NAM>();
            DANGKY_CHAMCONG_DACBIET = new HashSet<DANGKY_CHAMCONG_DACBIET>();
            DANGKY_OT_NHANVIEN = new HashSet<DANGKY_OT_NHANVIEN>();
            DC_CHAM_CONG = new HashSet<DC_CHAM_CONG>();
            NHANVIEN_CALAMVIEC = new HashSet<NHANVIEN_CALAMVIEC>();
            ATTENDANCE_RECORD = new HashSet<ATTENDANCE_RECORD>();
            TRAINING_NHANVIEN = new HashSet<TRAINING_NHANVIEN>();
            HR_THAISAN_CONNHO = new HashSet<HR_THAISAN_CONNHO>();
        }

        public HR_NHANVIEN
            (string id, string tenNV, string maChucDanh, string maBoPhan, string gioiTinh, string ngaySinh, string noiSinh, string tinhTrangHonNhan, string danToc, string tonGiao, string diaChiThuongTru,
            string soDienThoai, string soDienThoaiNguoiThan, string quanHeNguoiThan, string cMTND, string ngayCapCMTND, string noiCapCMTND, string soTaiKhoanNH,
            string tenNganHang, string truongDaoTao, string ngayVao, string nguyenQuan, string dChiHienTai, string kyLuatLD, string maBHXH, string maSoThue, int soNguoiGiamTru,
            string email, string note, string ngayNghiViec, string status, string image, string isDelete, int? maBoPhanChiTiet, string noiTuyenDung)
        {
            Id = id;
            TenNV = tenNV;
            MaChucDanh = maChucDanh;
            MaBoPhan = maBoPhan;
            GioiTinh = gioiTinh;
            NgaySinh = ngaySinh;
            NoiSinh = noiSinh;
            TinhTrangHonNhan = tinhTrangHonNhan;
            DanToc = danToc;
            TonGiao = tonGiao;
            DiaChiThuongTru = diaChiThuongTru;
            SoDienThoai = soDienThoai;
            SoDienThoaiNguoiThan = soDienThoaiNguoiThan;
            QuanHeNguoiThan = quanHeNguoiThan;
            CMTND = cMTND;
            NgayCapCMTND = ngayCapCMTND;
            NoiCapCMTND = noiCapCMTND;
            SoTaiKhoanNH = soTaiKhoanNH;
            TenNganHang = tenNganHang;
            TruongDaoTao = truongDaoTao;
            NgayVao = ngayVao;
            NguyenQuan = nguyenQuan;
            DChiHienTai = dChiHienTai;
            KyLuatLD = kyLuatLD;
            MaBHXH = maBHXH;
            MaSoThue = maSoThue;
            SoNguoiGiamTru = soNguoiGiamTru;
            Email = email;
            Note = note;
            NgayNghiViec = ngayNghiViec;
            Status = status;
            Image = image;
            IsDelete = isDelete;
            MaBoPhanChiTiet = maBoPhanChiTiet;
            NoiTuyenDung = noiTuyenDung;
        }

        [StringLength(250)]
        public string TenNV { get; set; }

        [StringLength(50)]
        public string MaChucDanh { get; set; }

        [StringLength(50)]
        public string MaBoPhan { get; set; }

        public int? MaBoPhanChiTiet { get; set; }

        [StringLength(20)]
        public string GioiTinh { get; set; }

        [StringLength(50)]
        public string NgaySinh { get; set; }

        [StringLength(250)]
        public string NoiSinh { get; set; }

        [StringLength(50)]
        public string TinhTrangHonNhan { get; set; }

        [StringLength(50)]
        public string DanToc { get; set; }

        [StringLength(50)]
        public string TonGiao { get; set; }

        [StringLength(250)]
        public string DiaChiThuongTru { get; set; }

        [StringLength(50)]
        public string SoDienThoai { get; set; }

        [StringLength(50)]
        public string SoDienThoaiNguoiThan { get; set; }

        [StringLength(100)]
        public string QuanHeNguoiThan { get; set; }

        [StringLength(50)]
        public string CMTND { get; set; }

        [StringLength(50)]
        public string NgayCapCMTND { get; set; }

        [StringLength(250)]
        public string NoiCapCMTND { get; set; }

        [StringLength(50)]
        public string SoTaiKhoanNH { get; set; }

        [StringLength(50)]
        public string TenNganHang { get; set; }

        [StringLength(500)]
        public string TruongDaoTao { get; set; }

        [StringLength(50)]
        public string NgayVao { get; set; }

        [StringLength(250)]
        public string NguyenQuan { get; set; }

        [StringLength(250)]
        public string DChiHienTai { get; set; }

        [StringLength(500)]
        public string KyLuatLD { get; set; }

        [StringLength(50)]
        public string MaBHXH { get; set; }

        [StringLength(50)]
        public string MaSoThue { get; set; }

        public int SoNguoiGiamTru { get; set; }

        [StringLength(250)]
        public string Email { get; set; }

        [StringLength(250)]
        public string Note { get; set; }

        [StringLength(50)]
        public string NgayNghiViec { get; set; } = "";

        [StringLength(50)]
        public string Status { get; set; }

        [StringLength(50)]
        public string NoiTuyenDung { get; set; }

        [StringLength(500)]
        public string Image { get; set; }

        [StringLength(50)]
        public string DateCreated { get; set; }

        [StringLength(50)]
        public string DateModified { get; set; }

        [StringLength(50)]
        public string UserCreated { get; set; }

        [StringLength(50)]
        public string UserModified { get; set; }

        [StringLength(10)]
        public string IsDelete { get; set; }

        [StringLength(50)]
        public string ChucVu2 { get; set; } // OP, STAFF, STAFF PM

        [StringLength(100)]
        public string TrucTiepSX { get; set; }

        [ForeignKey("MaBoPhan")]
        public virtual BOPHAN BOPHAN { get; set; }

        [ForeignKey("MaBoPhanChiTiet")]
        public virtual HR_BO_PHAN_DETAIL HR_BO_PHAN_DETAIL { get; set; }

        public virtual ICollection<HR_BHXH> HR_BHXH { get; set; }

        [ForeignKey("MaChucDanh")]
        public virtual HR_CHUCDANH HR_CHUCDANH { get; set; }
        public virtual ICollection<HR_CHUNGCHI_NHANVIEN> HR_CHUNGCHI_NHANVIEN { get; set; }
        public virtual ICollection<HR_HOPDONG> HR_HOPDONG { get; set; }
        public virtual ICollection<HR_THAISAN_CONNHO> HR_THAISAN_CONNHO { get; set; }
        public virtual ICollection<HR_KEKHAIBAOHIEM> HR_KEKHAIBAOHIEM { get; set; }
        public virtual ICollection<HR_QUATRINHLAMVIEC> HR_QUATRINHLAMVIEC { get; set; }
        public virtual ICollection<HR_TINHTRANGHOSO> HR_TINHTRANGHOSO { get; set; }
        public virtual ICollection<HR_PHEP_NAM> HR_PHEP_NAM { get; set; }

        public virtual ICollection<DANGKY_CHAMCONG_DACBIET> DANGKY_CHAMCONG_DACBIET { get; set; }
        public virtual ICollection<DANGKY_OT_NHANVIEN> DANGKY_OT_NHANVIEN { get; set; }
        public virtual ICollection<DC_CHAM_CONG> DC_CHAM_CONG { get; set; }
        public virtual ICollection<NHANVIEN_CALAMVIEC> NHANVIEN_CALAMVIEC { get; set; }
        public virtual ICollection<ATTENDANCE_RECORD> ATTENDANCE_RECORD { get; set; }

        public virtual ICollection<TRAINING_NHANVIEN> TRAINING_NHANVIEN { get; set; }
        public virtual ICollection<DANGKY_DIMUON_VSOM_NHANVIEN> DANGKY_DIMUON_VSOM_NHANVIEN { get; set; }
    }
}
