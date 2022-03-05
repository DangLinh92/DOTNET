using HRMNS.Data.Interfaces;
using HRMS.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HRMNS.Data.Entities
{
    [Table("CHAM_CONG_LOG")]
    public class CHAM_CONG_LOG : DomainEntity<long>, IDateTracking
    {
        [StringLength(50)]
        public string Ngay_ChamCong { get; set; }

        [StringLength(50)]
        public string ID_NV { get; set; }

        [StringLength(96)]
        public string Ten_NV { get; set; }

        [StringLength(50)]
        public string FirstIn_Time { get; set; }

        [StringLength(50)]
        public string Last_Out_Time { get; set; }

        [StringLength(50)]
        public string FirstIn { get; set; }

        [StringLength(50)]
        public string LastOut { get; set; }

        [StringLength(50)]
        public string Department { get; set; }

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