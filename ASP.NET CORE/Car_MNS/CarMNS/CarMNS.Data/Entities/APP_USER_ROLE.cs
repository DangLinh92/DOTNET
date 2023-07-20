using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CarMNS.Data.Entities
{
    [Table("APP_USER_ROLE")]
    public class APP_USER_ROLE : IdentityUserRole<Guid>
    {
        public APP_USER_ROLE() : base()
        {

        }
    }
}
