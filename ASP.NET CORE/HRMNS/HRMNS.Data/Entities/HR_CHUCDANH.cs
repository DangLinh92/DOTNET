using HRMNS.Data.Interfaces;
using HRMS.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HRMNS.Data.Entities
{
    [Table("HR_CHUCDANH")]
    public class HR_CHUCDANH : DomainEntity<string>, IDateTracking
    {
        public HR_CHUCDANH()
        {
            HR_NHANVIEN = new HashSet<HR_NHANVIEN>();
        }


        public HR_CHUCDANH(string id,string tenchucdanh,double phucap)
        {
            Id = id;
            TenChucDanh = tenchucdanh;
            PhuCap = phucap;
        }

        [StringLength(50)]
        public string TenChucDanh { get; set; }

        public double PhuCap { get; set; }

        [StringLength(50)]
        public string DateCreated { get; set; }

        [StringLength(50)]
        public string DateModified { get; set; }

        [StringLength(50)]
        public string UserCreated { get; set; }

        [StringLength(50)]
        public string UserModified { get; set; }

        public virtual ICollection<HR_NHANVIEN> HR_NHANVIEN { get; set; }
    }

    [Table("HR_CHUCDANH_BY_YEAR")]
    public class HR_CHUCDANH_BY_YEAR : DomainEntity<int>, IDateTracking
    {
        public HR_CHUCDANH_BY_YEAR()
        {
        }

        [StringLength(250)]
        public string MaChucDanh { get; set; }

        [StringLength(50)]
        public string TenChucDanh { get; set; }

        public double PhuCap { get; set; }

        [StringLength(50)]
        public string DateCreated { get; set; }

        [StringLength(50)]
        public string DateModified { get; set; }

        [StringLength(50)]
        public string UserCreated { get; set; }

        public int Year { get; set; }

        [StringLength(50)]
        public string UserModified { get; set; }

    }
}
