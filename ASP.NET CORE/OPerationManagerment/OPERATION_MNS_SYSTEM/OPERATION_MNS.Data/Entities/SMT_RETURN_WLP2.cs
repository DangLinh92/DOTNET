using OPERATION_MNS.Data.Interfaces;
using OPERATION_MNS.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace OPERATION_MNS.Data.Entities
{
    [Table("SMT_RETURN_WLP2")]
    public class SMT_RETURN_WLP2 : DomainEntity<int>, IDateTracking
    {
        public SMT_RETURN_WLP2()
        {

        }

        public SMT_RETURN_WLP2(int id,string sapCode,int smtReturn)
        {
            Id = id;
            SapCode = sapCode;
            SmtReturn = smtReturn;
        }

        [StringLength(50)]
        public string SapCode { get; set; }

        public int SmtReturn { get; set; }

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
