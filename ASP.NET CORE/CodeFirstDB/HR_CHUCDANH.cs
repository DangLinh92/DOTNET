namespace CodeFirstDB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("[HR.CHUCDANH]")]
    public partial class HR_CHUCDANH
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public HR_CHUCDANH()
        {
            HR_NHANVIEN = new HashSet<HR_NHANVIEN>();
        }

        [Key]
        [StringLength(50)]
        public string MaChucDanh { get; set; }

        [StringLength(50)]
        public string TenChucDanh { get; set; }

        [StringLength(50)]
        public string DateCreated { get; set; }

        [StringLength(50)]
        public string DateModified { get; set; }

        [StringLength(50)]
        public string UserCreated { get; set; }

        [StringLength(50)]
        public string UserModified { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HR_NHANVIEN> HR_NHANVIEN { get; set; }
    }
}
