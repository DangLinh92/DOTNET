using HRMNS.Data.Interfaces;
using HRMS.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HRMNS.Data.Entities
{
    [Table("KY_HIEU_CHAM_CONG")]
    public class KY_HIEU_CHAM_CONG : DomainEntity<string>, IDateTracking
    {
        public KY_HIEU_CHAM_CONG()
        {
            CA_LVIEC = new HashSet<CA_LVIEC>();
            DANGKY_CHAMCONG_CHITIET = new HashSet<DANGKY_CHAMCONG_CHITIET>();
            NGAY_LE_NAM = new HashSet<NGAY_LE_NAM>();
            NGAY_NGHI_BU_LE_NAM = new HashSet<NGAY_NGHI_BU_LE_NAM>();
        }

        [StringLength(300)]
        public string GiaiThich { get; set; }

        public double? Heso { get; set; }

        [StringLength(50)]
        public string DateCreated { get; set; }

        [StringLength(50)]
        public string DateModified { get; set; }

        [StringLength(50)]
        public string UserCreated { get; set; }

        [StringLength(50)]
        public string UserModified { get; set; }

        public virtual ICollection<CA_LVIEC> CA_LVIEC { get; set; }

        public virtual ICollection<DANGKY_CHAMCONG_CHITIET> DANGKY_CHAMCONG_CHITIET { get; set; }

        public virtual ICollection<NGAY_LE_NAM> NGAY_LE_NAM { get; set; }

        public virtual ICollection<NGAY_NGHI_BU_LE_NAM> NGAY_NGHI_BU_LE_NAM { get; set; }
    }
}
