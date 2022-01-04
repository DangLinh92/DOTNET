namespace CodeFirstDB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("[HR.LOAIHOPDONG]")]
    public partial class HR_LOAIHOPDONG
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public HR_LOAIHOPDONG()
        {
            HR_HOPDONG = new HashSet<HR_HOPDONG>();
        }

        [Key]
        public int MaLoaiHD { get; set; }

        [StringLength(500)]
        public string TenLoaiHD { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HR_HOPDONG> HR_HOPDONG { get; set; }
    }
}
