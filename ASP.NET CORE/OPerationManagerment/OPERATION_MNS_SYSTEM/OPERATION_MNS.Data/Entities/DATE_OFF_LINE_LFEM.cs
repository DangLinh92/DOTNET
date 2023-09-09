using OPERATION_MNS.Data.Interfaces;
using OPERATION_MNS.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace OPERATION_MNS.Data.Entities
{
    [Table("DATE_OFF_LINE_LFEM")]
    public class DATE_OFF_LINE_LFEM : DomainEntity<int>, IDateTracking
    {
        public DATE_OFF_LINE_LFEM()
        {

        }

        public DATE_OFF_LINE_LFEM(string value, string noidung, string phanLoai)
        {
            ItemValue = value;
            NoiDung = noidung;
            DanhMuc = phanLoai;    
        }

        [StringLength(50)]
        public string ItemValue { get; set; }

        [StringLength(250)]
        public string NoiDung { get; set; }

        [StringLength(50)]
        public string DanhMuc { get; set; } // KHSX, DEMAND

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
