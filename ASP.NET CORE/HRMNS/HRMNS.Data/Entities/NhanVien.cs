using HRMNS.Data.Enums;
using HRMNS.Data.Interfaces;
using HRMS.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMNS.Data.Entities
{
    public class NhanVien : DomainEntity<string>, ISortable, ISwitchable, IDateTracking
    {
        public int SortOrder { get; set; }
        public Status Status { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
    }
}
