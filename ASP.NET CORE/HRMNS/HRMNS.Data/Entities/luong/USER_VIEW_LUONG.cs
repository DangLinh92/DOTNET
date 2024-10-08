using HRMNS.Data.Interfaces;
using HRMS.Infrastructure.SharedKernel;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HRMNS.Data.Entities.luong
{
    [Table("USER_VIEW_LUONG")]
    public class USER_VIEW_LUONG : DomainEntity<Guid>, IDateTracking
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string PasswordDefault { get; set; }
        public int FirtLogin { get; set; }
        public string MaNV { get; set; }

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
