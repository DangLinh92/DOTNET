using HRMNS.Data.Enums;
using HRMNS.Data.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HRMNS.Data.Entities
{
    [Table("APP_USER")]
    public class APP_USER : IdentityUser<Guid>, IDateTracking,ISwitchable
    {
        [StringLength(250)]
        public string FullName { get; set; }
        public DateTime? BirthDay { get; set; }

        public string Avatar { get; set; }

        [StringLength(50)]
        public string DateCreated { get; set; }

        [StringLength(50)]
        public string DateModified { get; set; }

        [StringLength(50)]
        public string UserCreated { get; set; }

        [StringLength(50)]
        public string UserModified { get; set; }
        public Status Status { get; set; }

        public string ShowPass { get; set; }

        [StringLength(50)]
        public string Department { get; set; }
    }
}
