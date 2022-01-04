namespace CodeFirstDB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("[HR.BHXH]")]
    public partial class HR_BHXH
    {
        public HR_BHXH()
        {
            HR_NHANVIEN1 = new HashSet<HR_NHANVIEN>();
        }

        [Key]
        [StringLength(50)]
        public string MaSoBHXH { get; set; }

        [StringLength(50)]
        public string MaNV { get; set; }

        [StringLength(50)]
        public string NgayThamGia { get; set; }

        [StringLength(50)]
        public string NgayKetThuc { get; set; }

        [StringLength(50)]
        public string DateCreated { get; set; }

        [StringLength(50)]
        public string DateModified { get; set; }

        [StringLength(50)]
        public string UserCreated { get; set; }

        [StringLength(50)]
        public string UserModified { get; set; }

        public virtual HR_NHANVIEN HR_NHANVIEN { get; set; }
        public virtual ICollection<HR_NHANVIEN> HR_NHANVIEN1 { get; set; }
    }
}
