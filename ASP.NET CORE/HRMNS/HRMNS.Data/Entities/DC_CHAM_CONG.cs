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
        public DC_CHAM_CONG()
        {

        }

        public DC_CHAM_CONG(int id, string maNV, DateTime? ngayDCFrom, string noiDung, double? giatridc, string status, string thoigiantra
       , float? ngayCong
       , float? dSNS
       , float? nSBH
       , float? dC85
       , float? dC150
       , float? dC190
       , float? dC200
       , float? dC210
       , float? dC270
       , float? dC300
       , float? dC390
       , float? hT50
       , float? hT100
       , float? hT150
       , float? hT200
       , float? hT390
       , float? eLLC
       ,string ngayDieuChinh2)
        {
            Id = id;
            MaNV = maNV;
            NgayDieuChinh = ngayDCFrom;
            NoiDungDC = noiDung;
            TongSoTien = giatridc;
            TrangThaiChiTra = status;
            ChiTraVaoLuongThang = thoigiantra;
            NgayCong = ngayCong;
            DSNS = dSNS;
            NSBH = nSBH;
            DC85 = dC85;
            DC150 = dC150;
            DC190 = dC190;
            DC200 = dC200;
            DC210 = dC210;
            DC270 = dC270;
            DC300 = dC300;
            DC390 = dC390;
            HT50 = hT50;
            HT100 = hT100;
            HT150 = hT150;
            HT200 = hT200;
            HT390 = hT390;
            ELLC = eLLC;
            NgayDieuChinh2 = ngayDieuChinh2;
        }

        [StringLength(50)]
        public string MaNV { get; set; }

        public DateTime? NgayDieuChinh { get; set; }

        [StringLength(50)]
        public string NgayDieuChinh2 { get; set; }

        [StringLength(300)]
        public string NoiDungDC { get; set; }

        public double? TongSoTien { get; set; }

        [StringLength(20)]
        public string TrangThaiChiTra { get; set; }

        [StringLength(50)]
        public string ChiTraVaoLuongThang { get; set; }

        public float? NgayCong { get; set; }
        public float? DSNS { get; set; } // 30%
        public float? NSBH { get; set; } // 30%

        public float? DC85 { get; set; }
        public float? DC150 { get; set; }
        public float? DC190 { get; set; }
        public float? DC200 { get; set; }
        public float? DC210 { get; set; }
        public float? DC270 { get; set; }
        public float? DC300 { get; set; }
        public float? DC390 { get; set; }
        public float? HT50 { get; set; }
        public float? HT100 { get; set; }
        public float? HT150 { get; set; }
        public float? HT200 { get; set; }
        public float? HT390 { get; set; }
        public float? ELLC { get; set; }
        public float? Other { get; set; }

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
