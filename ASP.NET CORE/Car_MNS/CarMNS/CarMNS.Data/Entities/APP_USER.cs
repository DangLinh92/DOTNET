using CarMNS.Data.Enums;
using CarMNS.Data.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CarMNS.Data.Entities
{
    [Table("APP_USER")]
    public class APP_USER : IdentityUser<Guid>, IDateTracking,ISwitchable
    {
        public APP_USER()
        {
            APP_USER_TOKEN = new HashSet<APP_USER_TOKEN>();
        }
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

        [StringLength(50)]
        public string MaNhanVien { get; set; }

        public ICollection<APP_USER_TOKEN> APP_USER_TOKEN { get; set; }

    }

    [Table("APP_USER_TOKEN")]
    public class APP_USER_TOKEN : IdentityUserToken<Guid>
    {
        public APP_USER APP_USER { get; set; }
    }
}
