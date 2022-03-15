using HRMNS.Data.Interfaces;
using HRMS.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HRMNS.Data.Entities
{
    [Table("DANGKY_CHAMCONG_CHITIET")]
    public class DANGKY_CHAMCONG_CHITIET : DomainEntity<int>, IDateTracking
    {
        public DANGKY_CHAMCONG_CHITIET(int id,string name,int? danhmuc,string kyhieuChamCong)
        {
            Id = id;
            TenChiTiet = name;
            PhanLoaiDM = danhmuc;
            KyHieuChamCong = kyhieuChamCong;
        }
        public DANGKY_CHAMCONG_CHITIET()
        {
            DANGKY_CHAMCONG_DACBIET = new HashSet<DANGKY_CHAMCONG_DACBIET>();
        }

        [StringLength(150)]
        public string TenChiTiet { get; set; }

        public int? PhanLoaiDM { get; set; }

        [StringLength(20)]
        public string KyHieuChamCong { get; set; }

        [ForeignKey("PhanLoaiDM")]
        public virtual DM_DANGKY_CHAMCONG DM_DANGKY_CHAMCONG { get; set; }

        [ForeignKey("KyHieuChamCong")]
        public virtual KY_HIEU_CHAM_CONG KY_HIEU_CHAM_CONG { get; set; }

        public virtual ICollection<DANGKY_CHAMCONG_DACBIET> DANGKY_CHAMCONG_DACBIET { get; set; }

        [StringLength(50)]
        public string DateCreated { get; set; }

        [StringLength(50)]
        public string DateModified { get; set; }

        [StringLength(50)]
        public string UserCreated { get; set; }

        [StringLength(50)]
        public string UserModified { get; set; }
    }
}
