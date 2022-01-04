namespace CodeFirstDB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("[HR.KEKHAIBAOHIEM]")]
    public partial class HR_KEKHAIBAOHIEM
    {
        [Key]
        public int MaKeKhai { get; set; }

        [StringLength(50)]
        public string MaNV { get; set; }

        [StringLength(50)]
        public string CheDoBH { get; set; }

        [StringLength(50)]
        public string NgayBatDau { get; set; }

        [StringLength(50)]
        public string NgayKetThuc { get; set; }

        [StringLength(50)]
        public string NgayThanhToan { get; set; }

        public double? SoTienThanhToan { get; set; }

        [StringLength(50)]
        public string DateCreated { get; set; }

        [StringLength(50)]
        public string DateModified { get; set; }

        [StringLength(50)]
        public string UserCreated { get; set; }

        [StringLength(50)]
        public string UserModified { get; set; }

        public virtual HR_CHEDOBH HR_CHEDOBH { get; set; }

        public virtual HR_NHANVIEN HR_NHANVIEN { get; set; }
    }
}
