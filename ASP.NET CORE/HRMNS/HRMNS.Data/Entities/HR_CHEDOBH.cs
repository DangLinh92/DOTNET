using HRMNS.Data.Interfaces;
using HRMS.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HRMNS.Data.Entities
{
    [Table("HR_CHEDOBH")]
    public class HR_CHEDOBH : DomainEntity<string>
    {
        public HR_CHEDOBH()
        {
            HR_KEKHAIBAOHIEM = new HashSet<HR_KEKHAIBAOHIEM>();
        }

        [StringLength(250)]
        public string TenCheDo { get; set; }

        public virtual ICollection<HR_KEKHAIBAOHIEM> HR_KEKHAIBAOHIEM { get; set; }
    }
}
