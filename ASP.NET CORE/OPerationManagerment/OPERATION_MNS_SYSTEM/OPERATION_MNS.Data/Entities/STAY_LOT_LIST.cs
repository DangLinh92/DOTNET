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
    [Table("STAY_LOT_LIST")]
    public class STAY_LOT_LIST : DomainEntity<int>, IDateTracking
    {
        public STAY_LOT_LIST()
        {

        }

        public STAY_LOT_LIST(string lotId, string phuongAnXuLy,string tenLoi,string nguoixuly,string cassettleId,double seq)
        {
            LotId = lotId;
            PhuongAnXuLy = phuongAnXuLy;
            TenLoi = tenLoi;
            NguoiXuLy = nguoixuly;
            CassetteId = cassettleId;
            History_seq = seq;
        }

        [StringLength(50)]
        public string LotId { get; set; }

        [StringLength(50)]
        public string CassetteId { get; set; }

        [StringLength(1000)]
        public string PhuongAnXuLy { get; set; }

        [StringLength(250)]
        public string TenLoi { get; set; }

        [StringLength(50)]
        public string NguoiXuLy { get; set; }

        public double History_seq { get; set; }

        [StringLength(1)]
        public string ReleaseFlag { get; set; }

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
