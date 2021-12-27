using HRMNS.Data.Enums;
using HRMNS.Data.Interfaces;
using HRMS.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMNS.Data.Entities
{
    /// <summary>
    /// Quản lý cấp bậc nhân viên
    /// </summary>
    public class GradeNhanVien : DomainEntity<int>, ISortable, ISwitchable, IDateTracking
    {
        public Status Status { get; set; }
        public int SortOrder { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public string UserCreated { get; set; }
        public string UserModified { get; set; }
        // Ngày lên cấp
        public string DateOn { get; set; }
        public string NhanVienID { get; set; }
        public int GradeId { get; set; }
        public string Comment { get; set; }

    }
}
