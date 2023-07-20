using System;
using System.Collections.Generic;
using System.Text;

namespace CarMNS.Application.ViewModels.System
{
    public class PermisstionViewModel
    {
        public int Id { get; set; }

        public Guid RoleId { get; set; }

        public string FunctionId { get; set; }
        public bool CanCreate { get; set; }
        public bool CanRead { get; set; }
        public bool CanUpdate { get; set; }
        public bool CanDelete { get; set; }

        public bool ApproveL1 { get; set; } // GROUP
        public bool ApproveL2 { get; set; } // KOREA
        public bool ApproveL3 { get; set; } // HR

        public RoleViewModel AppRole { get; set; }

        public FunctionViewModel Function { get; set; }
    }
}
