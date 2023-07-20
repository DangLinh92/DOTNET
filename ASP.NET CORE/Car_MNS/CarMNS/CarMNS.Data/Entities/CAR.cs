using CarMNS.Data.Interfaces;
using CarMNS.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CarMNS.Data.Entities
{
    [Table("CAR")]
    public class CAR : DomainEntity<int>, IDateTracking
    {
        public CAR()
        {
            LAI_XE_CAR = new HashSet<LAI_XE_CAR>();
            DIEUXE_DANGKY = new HashSet<DIEUXE_DANGKY>();
        }

        [StringLength(50)]
        public string BienSoXe { get; set; }

        [StringLength(250)]
        public string LoaiXe { get; set; }

        [StringLength(50)]
        public string DateCreated { get; set; }

        [StringLength(50)]
        public string DateModified { get; set; }

        [StringLength(50)]
        public string UserCreated { get; set; }

        [StringLength(50)]
        public string UserModified { get; set; }

        public ICollection<LAI_XE_CAR> LAI_XE_CAR { get; set; }
        public ICollection<DIEUXE_DANGKY> DIEUXE_DANGKY { get; set; }
    }
}
