using CarMNS.Data.Interfaces;
using CarMNS.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CarMNS.Data.Entities
{
    [Table("DIEUXE_DANGKY")]
    public class DIEUXE_DANGKY : DomainEntity<int>, IDateTracking
    {
        public int MaDangKyXe { get; set; }
        public int MaXe { get; set; }
        public int MaLaiXe { get; set; }

        [ForeignKey("MaXe")]
        public CAR Car { get; set; }

        [ForeignKey("MaLaiXe")]
        public LAI_XE LaiXe { get; set; }

        [ForeignKey("MaDangKyXe")]
        public DANG_KY_XE DangKy { get; set; }

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
