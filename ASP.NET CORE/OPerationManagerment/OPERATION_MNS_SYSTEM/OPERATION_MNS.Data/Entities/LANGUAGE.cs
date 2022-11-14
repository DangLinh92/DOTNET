using OPERATION_MNS.Data.Enums;
using OPERATION_MNS.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace OPERATION_MNS.Data.Entities
{
    [Table("LANGUAGE")]
    public class LANGUAGE : DomainEntity<string>
    {
        [Required]
        [StringLength(128)]
        public string Name { get; set; }
        public bool IsDefault { get; set; }
        public string Resources { get; set; }
        public Status Status { get; set; }
    }
}
