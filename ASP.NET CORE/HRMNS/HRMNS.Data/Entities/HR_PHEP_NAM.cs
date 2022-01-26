using HRMNS.Data.Interfaces;
using HRMS.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HRMNS.Data.Entities
{
    [Table("HR_PHEP_NAM")]
    public class HR_PHEP_NAM : DomainEntity<int>, IDateTracking
    {
        [StringLength(50)]
        public string MaNhanVien { get; set; }
        public int SoPhepNam { get; set; }
        public int SoPhepConLai { get; set; }
        public int Year { get; set; }
        [StringLength(50)]
        public string DateCreated { get; set; }
        [StringLength(50)]
        public string DateModified { get; set; }
        [StringLength(50)]
        public string UserCreated { get; set; }
        [StringLength(50)]
        public string UserModified { get; set; }

        [ForeignKey("MaNhanVien")]
        public virtual HR_NHANVIEN HR_NHANVIEN { get; set; }
    }
}
