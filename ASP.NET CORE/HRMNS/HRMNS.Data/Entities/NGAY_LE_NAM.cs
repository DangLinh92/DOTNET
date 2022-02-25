using HRMNS.Data.Interfaces;
using HRMS.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HRMNS.Data.Entities
{
    [Table("NGAY_LE_NAM")]
    public class NGAY_LE_NAM : DomainEntity<string>, IDateTracking
    {

        [StringLength(150)]
        public string TenNgayLe { get; set; }

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
