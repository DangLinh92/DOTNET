using HRMNS.Data.Interfaces;
using HRMS.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HRMNS.Data.Entities
{
    [Table("DC_CHAM_CONG")]
    public class DC_CHAM_CONG : DomainEntity<int>, IDateTracking
    {
        public int? DM_DieuChinhCong { get; set; }

        [StringLength(50)]
        public string MaNV { get; set; }

        [StringLength(50)]
        public string NgayCanDieuChinh_From { get; set; }

        [StringLength(50)]
        public string NgayCanDieuChinh_To { get; set; }

        [StringLength(300)]
        public string NoiDungDC { get; set; }

        public double? GiaTriBoXung { get; set; }

        [StringLength(20)]
        public string TrangThaiChiTra { get; set; }

        [StringLength(50)]
        public string DateCreated { get; set; }

        [StringLength(50)]
        public string DateModified { get; set; }

        [StringLength(50)]
        public string UserCreated { get; set; }

        [StringLength(50)]
        public string UserModified { get; set; }

        [ForeignKey("DM_DieuChinhCong")]
        public virtual DM_DIEUCHINH_CHAMCONG DM_DIEUCHINH_CHAMCONG { get; set; }

        [ForeignKey("MaNV")]
        public virtual HR_NHANVIEN HR_NHANVIEN { get; set; }
    }
}
