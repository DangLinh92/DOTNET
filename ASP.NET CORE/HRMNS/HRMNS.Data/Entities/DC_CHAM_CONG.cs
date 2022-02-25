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
        public int? MaChamCong_ChiTiet { get; set; }

        [StringLength(50)]
        public string MaNV { get; set; }

        [StringLength(50)]
        public string NgayCanDieuChinh_From { get; set; }

        [StringLength(50)]
        public string NgayCanDieuChinh_To { get; set; }

        [StringLength(300)]
        public string NoiDungDC { get; set; }

        public double? GiaTriSauDC { get; set; }

        public double? GiaTriTruocDC { get; set; }

        [StringLength(20)]
        public string KyHieuChamCong { get; set; }

        [StringLength(50)]
        public string DateCreated { get; set; }

        [StringLength(50)]
        public string DateModified { get; set; }

        [StringLength(50)]
        public string UserCreated { get; set; }

        [StringLength(50)]
        public string UserModified { get; set; }

        [ForeignKey("MaChamCong_ChiTiet")]
        public virtual DANGKY_CHAMCONG_CHITIET DANGKY_CHAMCONG_CHITIET { get; set; }

        [ForeignKey("MaNV")]
        public virtual HR_NHANVIEN HR_NHANVIEN { get; set; }
    }
}
