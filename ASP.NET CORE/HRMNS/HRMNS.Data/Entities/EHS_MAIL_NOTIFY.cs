using HRMNS.Data.Interfaces;
using HRMS.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HRMNS.Data.Entities
{
    [Table("EHS_MAIL_NOTIFY")]
    public class EHS_MAIL_NOTIFY : DomainEntity<int>, IDateTracking
    {
        public EHS_MAIL_NOTIFY()
        {
        }

        public EHS_MAIL_NOTIFY(string email, string tenUser)
        {
            Email = email;
            UserName = tenUser;
        }

        [StringLength(100)]
        public string Email { get; set; }

        [StringLength(100)]
        public string UserName { get; set; }

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
