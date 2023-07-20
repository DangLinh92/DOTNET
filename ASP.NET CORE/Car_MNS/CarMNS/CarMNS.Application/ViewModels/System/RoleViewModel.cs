using System;
using System.Collections.Generic;
using System.Text;

namespace CarMNS.Application.ViewModels.System
{
    public class RoleViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string NormalizedName { get; set; }
        public string ConcurrencyStamp { get; set; }
        public string  Description { get; set; }
    }
}
