using OPERATION_MNS.Data.Interfaces;
using OPERATION_MNS.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace OPERATION_MNS.Data.Entities
{
    [Table("PHAN_LOAI_HANG_SAMPLE")]
    public class PHAN_LOAI_HANG_SAMPLE : DomainEntity<string>, IDateTracking
    {
        public PHAN_LOAI_HANG_SAMPLE()
        {
            PHAN_LOAI_MODEL_SAMPLE = new HashSet<PHAN_LOAI_MODEL_SAMPLE>();
        }

        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(50)]
        public string DateCreated { get; set; }

        [StringLength(50)]
        public string DateModified { get; set; }

        [StringLength(50)]
        public string UserCreated { get; set; }

        [StringLength(50)]
        public string UserModified { get; set; }

        public ICollection<PHAN_LOAI_MODEL_SAMPLE> PHAN_LOAI_MODEL_SAMPLE { get; set; }
    }

    [Table("PHAN_LOAI_MODEL_SAMPLE")]
    public class PHAN_LOAI_MODEL_SAMPLE : DomainEntity<string>, IDateTracking
    {
        [StringLength(50)]
        public string PhanLoai { get; set; }

        [StringLength(50)]
        public string DateCreated { get; set; }

        [StringLength(50)]
        public string DateModified { get; set; }

        [StringLength(50)]
        public string UserCreated { get; set; }

        [StringLength(50)]
        public string UserModified { get; set; }

        [ForeignKey("PhanLoai")]
        public PHAN_LOAI_HANG_SAMPLE PHAN_LOAI_HANG_SAMPLE { get; set; }
    }

    [Table("TCARD_SAMPLE")]
    public class TCARD_SAMPLE : DomainEntity<int>, IDateTracking
    {
        [StringLength(50)]
        public string Model { get; set; }

        [StringLength(50)]
        public string LotNo { get; set; }

        [StringLength(250)]
        public string WaferID { get; set; }

        [StringLength(50)]
        public string OutPutDatePlan { get; set; }

        [StringLength(50)]
        public string InputDatePlan { get; set; }

        [StringLength(50)]
        public string NguoiChiuTrachNhiem { get; set; }

        [StringLength(250)]
        public string MucDichInput { get; set; }

        [StringLength(50)]
        public string Code { get; set; } // loai hang

        public DateTime OutPutDatePlan2 { get; set; }

        public DateTime InputDatePlan2 { get; set; }

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
