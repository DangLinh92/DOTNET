namespace CodeFirstDB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("[HR.NHANVIEN]")]
    public partial class HR_NHANVIEN
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public HR_NHANVIEN()
        {
            HR_BHXH = new HashSet<HR_BHXH>();
            HR_CHUNGCHI = new HashSet<HR_CHUNGCHI>();
            HR_HOPDONG = new HashSet<HR_HOPDONG>();
            HR_KEKHAIBAOHIEM = new HashSet<HR_KEKHAIBAOHIEM>();
            HR_QUATRINHLAMVIEC = new HashSet<HR_QUATRINHLAMVIEC>();
        }

        [Key]
        [StringLength(50)]
        public string MaNV { get; set; }

        [StringLength(250)]
        public string TenNV { get; set; }

        [StringLength(50)]
        public string MaChucDanh { get; set; }

        [StringLength(50)]
        public string MaBoPhan { get; set; }

        [StringLength(20)]
        public string GioiTinh { get; set; }

        [StringLength(50)]
        public string NgaySinh { get; set; }

        public string NoiSinh { get; set; }

        [StringLength(50)]
        public string TinhTrangHonNhan { get; set; }

        [StringLength(50)]
        public string DanToc { get; set; }

        [StringLength(50)]
        public string TonGiao { get; set; }

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

        public string NoiCapCMTND { get; set; }

        [StringLength(50)]
        public string SoTaiKhoanNH { get; set; }

        [StringLength(50)]
        public string TenNganHang { get; set; }

        [StringLength(500)]
        public string TruongDaoTao { get; set; }

        [StringLength(50)]
        public string NgayVao { get; set; }

        public string NguyenQuan { get; set; }

        public string DChiHienTai { get; set; }

        [StringLength(50)]
        public string LoaiHD { get; set; }

        [StringLength(50)]
        public string KyLuatLD { get; set; }

        [StringLength(50)]
        public string MaBHXH { get; set; }

        [StringLength(50)]
        public string MaSoThue { get; set; }

        public int? SoNguoiGiamTru { get; set; }

        [StringLength(250)]
        public string Email { get; set; }

        public string Note { get; set; }

        [StringLength(50)]
        public string NgayNghiViec { get; set; }

        [StringLength(50)]
        public string Status { get; set; }

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

        public virtual BOPHAN BOPHAN { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HR_BHXH> HR_BHXH { get; set; }

        public virtual HR_BHXH HR_BHXH1 { get; set; }

        public virtual HR_CHUCDANH HR_CHUCDANH { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HR_CHUNGCHI> HR_CHUNGCHI { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HR_HOPDONG> HR_HOPDONG { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HR_KEKHAIBAOHIEM> HR_KEKHAIBAOHIEM { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HR_QUATRINHLAMVIEC> HR_QUATRINHLAMVIEC { get; set; }

        public virtual HR_TINHTRANGHOSO HR_TINHTRANGHOSO { get; set; }
    }
}
