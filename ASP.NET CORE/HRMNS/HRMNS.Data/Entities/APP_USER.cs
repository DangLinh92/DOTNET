using HRMNS.Data.Enums;
using HRMNS.Data.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HRMNS.Data.Entities
{
    [Table("APP_USER")]
    public class APP_USER : IdentityUser<Guid>, IDateTracking,ISwitchable
    {
        public string MaNV { get; set; }
        public string FullName { get; set; }
        public DateTime? BirthDay { get; set; }
        public string Avatar { get; set; }
        public string DateCreated { get; set; }
        public string DateModified { get; set; }
        public string UserCreated { get; set; }
        public string UserModified { get; set; }
        public Status Status { get; set; }
    }
}
