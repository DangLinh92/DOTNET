using OPERATION_MNS.Data.Interfaces;
using OPERATION_MNS.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace OPERATION_MNS.Data.Entities
{
    [Table("DATE_OFF_LINE")]
    public class DATE_OFF_LINE : DomainEntity<int>, IDateTracking
    {
        public DATE_OFF_LINE()
        {

        }

        public DATE_OFF_LINE(string value,string onOff,string wlp,string danhmuc,string owner)
        {
            ItemValue = value;
            ON_OFF = onOff;
            WLP = wlp;
            DanhMuc = danhmuc;
            OWNER = owner;
        }

        [StringLength(50)]
        public string ItemValue { get; set; }

        [StringLength(10)]
        public string ON_OFF { get; set; }

        [StringLength(50)]
        public string WLP { get; set; }

        [StringLength(50)]
        public string OWNER { get; set; }

        [StringLength(50)]
        public string DanhMuc { get; set; }

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
