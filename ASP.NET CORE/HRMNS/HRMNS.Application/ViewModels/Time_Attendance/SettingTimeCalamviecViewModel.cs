using HRMNS.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HRMNS.Application.ViewModels.Time_Attendance
{
    public class SettingTimeCalamviecViewModel
    {
        public int Id { get; set; }

        [StringLength(50)]
        public string CaLamViec { get; set; }

        [StringLength(50)]
        public string NgayBatDau { get; set; }

        [StringLength(50)]
        public string NgayKetThuc { get; set; }

        [StringLength(50)]
        public string NgayBatDauDangKy { get; set; }

        [StringLength(50)]
        public string NgayKetThucDangKy { get; set; }

        [StringLength(50)]
        public string Status { get; set; }

        [StringLength(50)]
        public string DateCreated { get; set; }

        [StringLength(50)]
        public string DateModified { get; set; }

        [StringLength(50)]
        public string UserCreated { get; set; }

        [StringLength(50)]
        public string UserModified { get; set; }

        [ForeignKey("CaLamViec")]
        public DM_CA_LVIEC DM_CA_LVIEC { get; set; }
    }
}
