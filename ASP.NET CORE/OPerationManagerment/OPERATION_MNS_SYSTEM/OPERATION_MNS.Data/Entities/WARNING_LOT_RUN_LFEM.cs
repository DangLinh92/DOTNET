using OPERATION_MNS.Data.Interfaces;
using OPERATION_MNS.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace OPERATION_MNS.Data.Entities
{
    [Table("WARNING_LOT_RUN_LFEM")]
    public class WARNING_LOT_RUN_LFEM : DomainEntity<int>, IDateTracking
    {
        [StringLength(50)]
        public string OperationID { get; set; }

        public string OperationName { get; set; }

        public double STBMin { get; set; }
        public double STBHour { get; set; }

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
