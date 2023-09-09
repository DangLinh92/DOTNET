using OPERATION_MNS.Data.Interfaces;
using OPERATION_MNS.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace OPERATION_MNS.Data.Entities
{

    /// <summary>
    /// Sản xuât thực tế của SMT
    /// </summary>
    [Table("OUT_PUT_BY_LOT_SMT")]
    public class OUT_PUT_BY_LOT_SMT : DomainEntity<int>, IDateTracking
    {
        public OUT_PUT_BY_LOT_SMT()
        {
            
        }

        [StringLength(50)]
        public string Work_date { get; set; }

        [StringLength(50)]
        public string Lot_id { get; set; }

        [StringLength(50)]
        public string Material { get; set; }

        [StringLength(50)]
        public string Size { get; set; }

        [StringLength(50)]
        public string MaterialGroup { get; set; }

        public double QtyInput { get; set; }
        public double QtyOutput { get; set; }

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
