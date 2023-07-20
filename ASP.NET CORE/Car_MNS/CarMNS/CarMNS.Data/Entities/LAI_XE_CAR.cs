using CarMNS.Data.Interfaces;
using CarMNS.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CarMNS.Data.Entities
{
    [Table("LAI_XE_CAR")]
    public class LAI_XE_CAR : DomainEntity<int>, IDateTracking
    {
        public int CarId { get; set; }
        public int LaxeId { get; set; }

        [ForeignKey("CarId")]
        public CAR CAR { get; set; }

        [ForeignKey("LaxeId")]
        public LAI_XE LAI_XE { get; set; }

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
