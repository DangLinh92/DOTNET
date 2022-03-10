using HRMNS.Data.Interfaces;
using HRMS.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HRMNS.Data.Entities
{
    [Table("NGAY_DAC_BIET")]
    public class NGAY_DAC_BIET : DomainEntity<string>, IDateTracking
    {

        public NGAY_DAC_BIET()
        {

        }

        public NGAY_DAC_BIET(string id,string name,string kyhieuchamcong)
        {
            Id = id;
            TenNgayDacBiet = name;
            KyHieuChamCong = kyhieuchamcong;
        }

        [StringLength(150)]
        public string TenNgayDacBiet { get; set; }

        [StringLength(20)]
        public string KyHieuChamCong { get; set; }

        [StringLength(50)]
        public string DateCreated { get; set; }

        [StringLength(50)]
        public string DateModified { get; set; }

        [StringLength(50)]
        public string UserCreated { get; set; }

        [StringLength(50)]
        public string UserModified { get; set; }

        [ForeignKey("KyHieuChamCong")]
        public virtual KY_HIEU_CHAM_CONG KY_HIEU_CHAM_CONG { get; set; }
    }
}
