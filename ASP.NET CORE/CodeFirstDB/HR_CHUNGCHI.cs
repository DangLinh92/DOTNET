namespace CodeFirstDB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("[HR.CHUNGCHI]")]
    public partial class HR_CHUNGCHI
    {
        [Key]
        public int MaChungChi { get; set; }

        [StringLength(50)]
        public string MaNV { get; set; }

        [StringLength(50)]
        public string TenChungChi { get; set; }

        public string NoiCap { get; set; }

        [StringLength(50)]
        public string NgayCap { get; set; }

        [StringLength(50)]
        public string NgayHetHan { get; set; }

        [StringLength(50)]
        public string ChuyenMon { get; set; }

        public int? LoaiChungChi { get; set; }

        [StringLength(50)]
        public string DateCreated { get; set; }

        [StringLength(50)]
        public string DateModified { get; set; }

        [StringLength(50)]
        public string UserCreated { get; set; }

        [StringLength(50)]
        public string UserModified { get; set; }

        public virtual HR_NHANVIEN HR_NHANVIEN { get; set; }

        public virtual LOAICHUNGCHI LOAICHUNGCHI1 { get; set; }
    }
}
