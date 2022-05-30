using HRMNS.Data.Interfaces;
using HRMS.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HRMNS.Data.Entities
{
    [Table("DANGKY_OT_NHANVIEN")]
    public class DANGKY_OT_NHANVIEN : DomainEntity<int>, IDateTracking
    {
        public DANGKY_OT_NHANVIEN()
        {

        }
        public DANGKY_OT_NHANVIEN(string ngayOT,string maNV,string ngayLviec,string approve,string approveLV2,string approveLV3)
        {
            NgayOT = ngayOT;
            MaNV = maNV;
            DM_NgayLViec = ngayLviec;
            Approve = approve;
            ApproveLV2 = approveLV2;
            ApproveLV3 = approveLV3;
        }

        [StringLength(50)]
        public string NgayOT { get; set; }

        [StringLength(50)]
        public string MaNV { get; set; }

        [StringLength(50)]
        public string DM_NgayLViec { get; set; }

        [StringLength(50)]
        public string Approve { get; set; }

        [StringLength(50)]
        public string ApproveLV2 { get; set; }

        [StringLength(50)]
        public string ApproveLV3 { get; set; }

        [StringLength(50)]
        public string DateCreated { get; set; }

        [StringLength(50)]
        public string DateModified { get; set; }

        [StringLength(50)]
        public string UserCreated { get; set; }

        [StringLength(50)]
        public string UserModified { get; set; }

        [ForeignKey("DM_NgayLViec")]
        public virtual DM_NGAY_LAMVIEC DM_NGAY_LAMVIEC { get; set; }

        [ForeignKey("MaNV")]
        public virtual HR_NHANVIEN HR_NHANVIEN { get; set; }
    }
}
