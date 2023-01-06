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
    [Table("STAY_LOT_LIST_HISTORY")]
    public class STAY_LOT_LIST_HISTORY : IDateTracking
    {
        public STAY_LOT_LIST_HISTORY()
        {

        }

        [StringLength(50)]

        public string LotId { get; set; }

        public double History_seq { get; set; }

        [StringLength(50)]
        public string CassetteId { get; set; }

        [StringLength(50)]
        public string Material { get; set; }

        [StringLength(50)]
        public string OperationID { get; set; }

        [StringLength(100)]
        public string OperationName { get; set; }

        public decimal StayDay { get; set; }

        public decimal ChipQty { get; set; }

        [StringLength(50)]
        public string ERPProductOrder { get; set; }

        [StringLength(50)]
        public string FABLotID { get; set; }

        [StringLength(50)]
        public string HoldTime { get; set; }

        [StringLength(50)]
        public string HoldCode { get; set; }

        [StringLength(50)]
        public string HoldUserName { get; set; }

        [StringLength(50)]
        public string HoldComment { get; set; }

        [StringLength(250)]
        public string TenLoi { get; set; }

        [StringLength(50)]
        public string NguoiXuLy { get; set; }

        [StringLength(1000)]
        public string PhuongAnXuLy { get; set; }

        [StringLength(50)]
        public string ReleaseTime { get; set; }

        [StringLength(1)]
        public string ReleaseFlag { get; set; }

        [StringLength(50)]
        public string ReleaseCode { get; set; }

        [StringLength(100)]
        public string ReleaseName { get; set; }

        [StringLength(50)]
        public string ReleaseUser { get; set; }

        [StringLength(2000)]
        public string ReleaseComment { get; set; }

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
