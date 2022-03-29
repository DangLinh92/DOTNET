using VOC.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace VOC.Data.Entities
{
    [Table("PERMISSION")]
    public class PERMISSION : DomainEntity<int>
    {
        [Required]
        public Guid RoleId { get; set; }

        [StringLength(128)]
        [Required]
        public string FunctionId { get; set; }
        public bool CanCreate { get; set; }
        public bool CanRead { get; set; }
        public bool CanUpdate { get; set; }
        public bool CanDelete { get; set; }
        public bool CanImport { set; get; }
        public bool CanExport { set; get; }

        [ForeignKey("RoleId")]
        public virtual APP_ROLE AppRole { get; set; }

        [ForeignKey("FunctionId")]
        public virtual FUNCTION Function { get; set; }

    }
}
