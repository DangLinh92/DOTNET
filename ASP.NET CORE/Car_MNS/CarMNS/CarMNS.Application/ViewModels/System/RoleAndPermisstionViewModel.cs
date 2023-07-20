using System;
using System.Collections.Generic;
using System.Text;

namespace CarMNS.Application.ViewModels.System
{
    public class RoleAndPermisstionViewModel
    {
        public RoleAndPermisstionViewModel()
        {
            RoleModels = new List<RoleViewModel>();
            PermisstionForRoleModels = new List<PermisstionViewModel>();
            RoleActive = "Admin";
        }

        public List<RoleViewModel> RoleModels { get; set; }
        public List<PermisstionViewModel> PermisstionForRoleModels { get; set; }
        public List<FunctionViewModel> Functions { get; set; }

        public string RoleActive { get; set; }
    }
}
