using OPERATION_MNS.Data.Interfaces;
using OPERATION_MNS.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace OPERATION_MNS.Data.Entities
{
    [Table("DATE_OFF_LINE_SAMPLE")]
    public class DATE_OFF_LINE_SAMPLE : DomainEntity<int>, IDateTracking
    {
        public DATE_OFF_LINE_SAMPLE()
        {

        }

        public DATE_OFF_LINE_SAMPLE(string value,string noidung)
        {
            ItemValue = value;
            NoiDung = noidung;
        }

        [StringLength(50)]
        public string ItemValue { get; set; }

        [StringLength(250)]
        public string NoiDung { get; set; }

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
