using HRMNS.Data.Interfaces;
using HRMS.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HRMNS.Data.Entities
{
    [Table("HR_NHANVIEN_CHEDO_DB")]
    public class HR_NHANVIEN_CHEDO_DB : DomainEntity<int>, IDateTracking
    {
        public HR_NHANVIEN_CHEDO_DB()
        {

        }

        public HR_NHANVIEN_CHEDO_DB(string maNV,string chedoDB,string note)
        {
            MaNhanVien = maNV;
            CheDoDB = chedoDB;
            Note = note;
        }

        [StringLength(50)]
        public string MaNhanVien { get; set; }

        [StringLength(50)]
        public string CheDoDB { get; set; }

        [StringLength(250)]
        public string Note { get; set; }
      
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
