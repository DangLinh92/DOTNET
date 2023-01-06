using HRMNS.Data.Interfaces;
using HRMS.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HRMNS.Data.Entities
{
    [Table("DANGKY_DIMUON_VSOM_NHANVIEN")]
    public class DANGKY_DIMUON_VSOM_NHANVIEN : DomainEntity<int>, IDateTracking
    {
        public DANGKY_DIMUON_VSOM_NHANVIEN()
        {

        }

        public DANGKY_DIMUON_VSOM_NHANVIEN(string ngayDangKy, string maNV,string approve,string approveLV2,string approveLV3,double soGioDangKy, string noiDung)
        {
            NgayDangKy = ngayDangKy;
            MaNV = maNV;
            Approve = approve;
            ApproveLV2 = approveLV2;
            ApproveLV3 = approveLV3;
            SoGioDangKy = soGioDangKy;
            NoiDung = noiDung;
        }

        [StringLength(50)]
        public string NgayDangKy { get; set; }

        [StringLength(50)]
        public string MaNV { get; set; }

        public double SoGioDangKy { get; set; }

        [StringLength(250)]
        public string NoiDung { get; set; }

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

        [ForeignKey("MaNV")]
        public virtual HR_NHANVIEN HR_NHANVIEN { get; set; }
    }
}
