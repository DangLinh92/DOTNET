using OPERATION_MNS.Data.Enums;
using OPERATION_MNS.Data.Interfaces;
using OPERATION_MNS.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace OPERATION_MNS.Data.Entities
{
    [Table("STAY_LOT_LIST_LFEM")]
    public class STAY_LOT_LIST_LFEM : DomainEntity<int>, IDateTracking
    {
        public STAY_LOT_LIST_LFEM()
        {

        }

        [StringLength(50)]
        public string MaterialCategory { get; set; }

        [StringLength(50)]
        public string MaterialGroup { get; set; }

        [StringLength(50)]
        public string Size { get; set; }

        [StringLength(50)]
        public string Material { get; set; }

        [StringLength(50)]
        public string LotId { get; set; }

        [StringLength(50)]
        public string FAID { get; set; }

        [StringLength(50)]
        public string AssyLotID { get; set; }

        [StringLength(50)]
        public string Date { get; set; }

        [StringLength(50)]
        public string OperationName { get; set; }
        public int DATE_DIFF { get; set; }
        public double ChipQty { get; set; }

        [StringLength(150)]
        public string Worker { get; set; }

        [StringLength(250)]
        public string Comment { get; set; }

        [StringLength(100)]
        public string LoaiLoi { get; set; }

        [StringLength(100)]
        public string LichTrinhXuLy { get; set; }

        [StringLength(100)]
        public string ChiuTrachNhiem { get; set; }

        public double History_seq { get; set; }

        [StringLength(1)]
        public string ReleaseFlag { get; set; }

        [StringLength(1)]
        public string History_delete_flag { get; set; }

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
