using OPERATION_MNS.Data.Interfaces;
using OPERATION_MNS.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace OPERATION_MNS.Data.Entities
{
    [Table("SETTING_ITEMS")]
    public class SETTING_ITEMS : DomainEntity<string>, IDateTracking
    {
        public SETTING_ITEMS()
        {

        }

        public SETTING_ITEMS(string value,string id)
        {
            ItemValue = value;
            Id = id;
        }

        [StringLength(50)]
        public string ItemValue { get; set; }

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
