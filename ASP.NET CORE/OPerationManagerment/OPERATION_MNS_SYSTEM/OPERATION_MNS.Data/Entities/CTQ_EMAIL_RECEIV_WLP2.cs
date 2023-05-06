using OPERATION_MNS.Data.Interfaces;
using OPERATION_MNS.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace OPERATION_MNS.Data.Entities
{
    [Table("CTQ_EMAIL_RECEIV_WLP2")]
    public class CTQ_EMAIL_RECEIV_WLP2 : DomainEntity<string>, IDateTracking
    {
        public CTQ_EMAIL_RECEIV_WLP2()
        {

        }

        public CTQ_EMAIL_RECEIV_WLP2(string id,bool active,string department)
        {
            Active = active;
            Department = department;
            Id = id;
        }

        public bool Active { get; set; }

        public string Department { get; set; }

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
