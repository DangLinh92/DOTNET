namespace CodeFirstDB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("LOAICHUNGCHI")]
    public partial class LOAICHUNGCHI
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public LOAICHUNGCHI()
        {
            HR_CHUNGCHI = new HashSet<HR_CHUNGCHI>();
        }

        [Key]
        public int MaLoaiChungChi { get; set; }

        [StringLength(50)]
        public string TenLoaiChungChi { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HR_CHUNGCHI> HR_CHUNGCHI { get; set; }
    }
}
