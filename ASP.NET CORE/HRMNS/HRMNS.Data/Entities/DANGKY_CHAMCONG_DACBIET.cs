using HRMNS.Data.Interfaces;
using HRMS.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HRMNS.Data.Entities
{
    [Table("DANGKY_CHAMCONG_DACBIET")]
    public class DANGKY_CHAMCONG_DACBIET : DomainEntity<int>, IDateTracking
    {
        public DANGKY_CHAMCONG_DACBIET()
        {

        }
        public DANGKY_CHAMCONG_DACBIET(string maNV,int? maChamCong,string noiDung,string ngaybatdau,string ngaykethuc,string approve)
        {
            MaNV = maNV;
            MaChamCong_ChiTiet = maChamCong;
            NoiDung = noiDung;
            NgayBatDau = ngaybatdau;
            NgayKetThuc = ngaykethuc;
            Approve = approve;
        }

        [StringLength(50)]
        public string MaNV { get; set; }

        public int? MaChamCong_ChiTiet { get; set; }

        [StringLength(50)]
        public string NgayBatDau { get; set; }

        [StringLength(50)]
        public string NgayKetThuc { get; set; }

        [StringLength(300)]
        public string NoiDung { get; set; }

        [StringLength(50)]
        public string Approve { get; set; }

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
