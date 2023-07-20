using CarMNS.Data.Interfaces;
using CarMNS.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CarMNS.Data.Entities
{
    [Table("BOPHAN")]
    public class BOPHAN : DomainEntity<string>, IDateTracking
    {
        [StringLength(150)]
        public string TenBoPhan { get; set; }

        [StringLength(50)]
        public string DateCreated { get; set; }

        [StringLength(50)]
        public string DateModified { get; set; }

        [StringLength(50)]
        public string UserCreated { get; set; }

        [StringLength(50)]
        public string UserModified { get; set; }
    }
}
