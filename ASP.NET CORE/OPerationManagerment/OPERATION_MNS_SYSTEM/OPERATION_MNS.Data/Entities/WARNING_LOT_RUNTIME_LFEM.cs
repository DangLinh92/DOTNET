using OPERATION_MNS.Data.Interfaces;
using OPERATION_MNS.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace OPERATION_MNS.Data.Entities
{
    [Table("WARNING_LOT_RUNTIME_LFEM")]
    public class WARNING_LOT_RUNTIME_LFEM : DomainEntity<int>, IDateTracking
    {
        [StringLength(50)]
        public string MaterialCategory { get; set; }

        [StringLength(50)]
        public string Size { get; set; }

        [StringLength(50)]
        public string Material { get; set; }

        [StringLength(50)]
        public string LotID { get; set; }

        [StringLength(50)]
        public string FA_ID { get; set; }

        [StringLength(50)]
        public string Date { get; set; }

        public decimal ChipQTy { get; set; }

        [StringLength(50)]
        public string Unit { get; set; }

        [StringLength(50)]
        public string StartFlg { get; set; }

        [StringLength(50)]
        public string Comment { get; set; }

        [StringLength(50)]
        public string OperationID { get; set; }

        [StringLength(50)]
        public string OperationName { get; set; }

        public decimal STBMin { get; set; }

        [StringLength(50)]
        public string TimeStart { get; set; }

        [StringLength(50)]
        public string DateNow { get; set; }

        [StringLength(50)]
        public string TimeNow { get; set; }

        [StringLength(50)]
        public string RunTime_hmm { get; set; }

        [StringLength(50)]
        public string RunTime_m { get; set; }

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
