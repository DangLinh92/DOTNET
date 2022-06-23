using HRMNS.Application.ViewModels.HR;
using HRMNS.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HRMNS.Application.ViewModels.Time_Attendance
{
    public class NhanVien_CalamViecViewModel
    {
        public int Id { get; set; }

        [StringLength(50)]
        public string MaNV { get; set; }

        [StringLength(50)]
        public string Danhmuc_CaLviec { get; set; }

        [StringLength(50)]
        public string BatDau_TheoCa { get; set; }

        [StringLength(50)]
        public string KetThuc_TheoCa { get; set; }

        [StringLength(50)]
        public string Status { get; set; }

        [StringLength(5)]
        public string Approved { get; set; }

        [StringLength(50)]
        public string DateCreated { get; set; }

        [StringLength(50)]
        public string DateModified { get; set; }

        [StringLength(50)]
        public string UserCreated { get; set; }

        [StringLength(50)]
        public string UserModified { get; set; }

        [StringLength(50)]
        public string CaLV_DB
        {
            get; set;
        }

        [ForeignKey("Danhmuc_CaLviec")]
        public DMCalamviecViewModel DM_CA_LVIEC { get; set; }

        [ForeignKey("MaNV")]
        public NhanVienViewModel HR_NHANVIEN { get; set; }
    }
}
