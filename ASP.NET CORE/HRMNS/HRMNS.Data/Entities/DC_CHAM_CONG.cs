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

        public DC_CHAM_CONG(int id, string maNV, string ngayDCFrom, string noiDung, double? giatridc, string status, DateTime? thoigiantra
       ,string chitraluongthang2)
        {
            Id = id;
            MaNV = maNV;
            NgayDieuChinh = ngayDCFrom;
            NoiDungDC = noiDung;
            TongSoTien = giatridc;
            TrangThaiChiTra = status;
            ChiTraVaoLuongThang = thoigiantra;
            ChiTraVaoLuongThang2 = chitraluongthang2;
        }

        [StringLength(50)]
        public string MaNV { get; set; }

        [StringLength(150)]
        public string NgayDieuChinh { get; set; }

        [StringLength(500)]
        public string NoiDungDC { get; set; }

        public double? TongSoTien { get; set; }

        [StringLength(20)]
        public string TrangThaiChiTra { get; set; }

        public DateTime? ChiTraVaoLuongThang { get; set; }

        [StringLength(50)]
        public string ChiTraVaoLuongThang2 { get; set; }

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
