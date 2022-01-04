namespace CodeFirstDB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("[HR.CHEDOBH]")]
    public partial class HR_CHEDOBH
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public HR_CHEDOBH()
        {
            HR_KEKHAIBAOHIEM = new HashSet<HR_KEKHAIBAOHIEM>();
        }

        [Key]
        [StringLength(50)]
        public string MaCheDo { get; set; }

        [StringLength(50)]
        public string TenCheDo { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HR_KEKHAIBAOHIEM> HR_KEKHAIBAOHIEM { get; set; }
    }
}
