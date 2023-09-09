using CarMNS.Data.Interfaces;
using CarMNS.Infrastructure.Interfaces;
using CarMNS.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CarMNS.Data.Entities
{
    [Table("BOPHAN_DUYET")]
    public class BOPHAN_DUYET : DomainEntity<int>, IDateTracking
    {
        public BOPHAN_DUYET()
        {
           
        }

        public string UserId { get; set; }
        public string BoPhan { get; set; }

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
