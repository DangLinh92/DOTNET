using OPERATION_MNS.Data.Interfaces;
using OPERATION_MNS.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace OPERATION_MNS.Data.Entities
{
    [Table("KHSX_ACTUAL_LFEM")]
    public class KHSX_ACTUAL_LFEM : DomainEntity<int>, IDateTracking
    {
        public KHSX_ACTUAL_LFEM()
        {

        }

        [StringLength(50)]
        public string Model { get; set; }

        [StringLength(50)]
        public string MesItemId { get; set; } // MATERIAL ID

        [StringLength(50)]
        public string OperationId { get; set; }

        [StringLength(50)]
        public string DateActual { get; set; }

        [StringLength(50)]
        public string WeekActual { get; set; }
        public double QuantityActual { get; set; }

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
