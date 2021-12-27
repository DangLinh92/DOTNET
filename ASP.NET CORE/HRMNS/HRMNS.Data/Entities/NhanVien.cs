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

        /// <summary>
        /// Chức danh
        /// </summary>
        public string TeamPosition { get; set; }

        /// <summary>
        /// Bộ phận
        /// </summary>
        public string Dept { get; set; }

        /// <summary>
        /// Họ tên
        /// </summary>
        public string Name { get; set; }
        // Giới tính
        public string Sex { get; set; }
        //Ngày sinh
        public string Age { get; set; }
        // Nơi sinh
        public string At { get; set; }
        // Tình trạng hôn nhân
        public string MarriedStatus { get; set; } // married or single
        // Dân tộc
        public string Race { get; set; }
        // Địa chỉ thường trú
        public string HomeAdd{get;set;}
        public string PhoneNumber { get; set; }
        // Người thân
        public virtual ICollection<Relative> Relatives { get; set; }
        public string CMTND { get; set; }
        // Ngay cap
        public string IssuedDate { get; set; }
        // Noi cap
        public string IssuedPlace { get; set; }
        // So TK
        public string AccountNo { get; set; }
        // Trường đào tạo
        public string Education { get; set; }
        public string UserCreated { get; set; }
        public string UserModified { get; set; }
        // Cấp bậc
        public virtual ICollection<GradeNhanVien> GradeNhanViens { get; set; }
        // Ngày vào
        public string StartingWorking { get; set; }
        // Ngày kết thúc thử việc
        public string EndDateTRContract { get; set; }
    }
}
