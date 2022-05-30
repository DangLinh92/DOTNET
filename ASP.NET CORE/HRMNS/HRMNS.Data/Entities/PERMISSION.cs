using HRMS.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HRMNS.Data.Entities
{
    [Table("PERMISSION")]
    public class PERMISSION : DomainEntity<int>
    {
        public PERMISSION()
        {

        }

        public PERMISSION(int id,Guid roleId,string functionId,bool create,bool read,bool update,bool delete,bool approve1,bool approve2,bool approve3)
        {
            Id = id;
            RoleId = roleId;
            FunctionId = functionId;
            CanCreate = create;
            CanRead = read;
            CanUpdate = update;
            CanDelete = delete;
            ApproveL1 = approve1;
            ApproveL2 = approve2;
            ApproveL3 = approve3;
        }

        [Required]
        public Guid RoleId { get; set; }

        [StringLength(128)]
        [Required]
        public string FunctionId { get; set; }
        public bool CanCreate { get; set; }
        public bool CanRead { get; set; }
        public bool CanUpdate { get; set; }
        public bool CanDelete { get; set; }

        public bool ApproveL1 { get; set; } // GROUP
        public bool ApproveL2 { get; set; } // KOREA
        public bool ApproveL3 { get; set; } // HR

        [ForeignKey("RoleId")]
        public virtual APP_ROLE AppRole { get; set; }

        [ForeignKey("FunctionId")]
        public virtual FUNCTION Function { get; set; }
    }
}
