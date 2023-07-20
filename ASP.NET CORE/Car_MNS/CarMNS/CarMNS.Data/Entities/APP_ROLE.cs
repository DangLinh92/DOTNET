using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CarMNS.Data.Entities
{
    [Table("APP_ROLE")]
    public class APP_ROLE:IdentityRole<Guid>
    {
        public APP_ROLE():base()
        {
        }
        public APP_ROLE(string name,string description) : base(name)
        {
            this.Description = description;
        }

        [StringLength(250)]
        public string Description { get; set; }
    }
}
