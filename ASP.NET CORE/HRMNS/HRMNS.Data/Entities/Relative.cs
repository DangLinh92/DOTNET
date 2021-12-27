using HRMNS.Data.Enums;
using HRMNS.Data.Interfaces;
using HRMS.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMNS.Data.Entities
{
    /// <summary>
    /// Nguoi than trong gia dinh
    /// </summary>
    public class Relative : DomainEntity<int>, ISortable, ISwitchable, IDateTracking
    {
        public int SortOrder { get; set; }
        public Status Status { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public string Name { get; set; }
        // Ngày sinh
        public string Age { get; set; }
        public string Sex { get; set; }
        public string PhoneNumber { get; set; }
        // Mối quan hệ
        public string Relationship { get; set; }
        public string Race { get; set; }
        // Địa chỉ thường trú
        public string HomeAdd { get; set; }
        public string CMTND { get; set; }
        public virtual ICollection<NhanVien> NhanViens { get; set; }
        public string UserCreated { get; set; }
        public string UserModified { get; set; }
    }
}
