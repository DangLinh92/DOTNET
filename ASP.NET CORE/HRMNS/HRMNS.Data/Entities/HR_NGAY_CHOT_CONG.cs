using HRMNS.Data.Interfaces;
using HRMS.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HRMNS.Data.Entities
{
    [Table("HR_NGAY_CHOT_CONG")]
    public class HR_NGAY_CHOT_CONG : DomainEntity<int>, IDateTracking
    {
        public HR_NGAY_CHOT_CONG()
        {

        }

        public HR_NGAY_CHOT_CONG(int id,string ngaychot,string chotchothang)
        {
            Id = id;
            NgayChotCong = ngaychot;
            ChotCongChoThang = chotchothang;
        }


        [StringLength(50)]
        public string NgayChotCong { get; set; }

        [StringLength(50)]
        public string ChotCongChoThang { get; set; }

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
