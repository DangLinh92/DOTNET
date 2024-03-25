using CarMNS.Data.Interfaces;
using CarMNS.Infrastructure.SharedKernel;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace CarMNS.Data.Entities
{
    [Table("DANG_KY_XE_TAXI")]
    public class DANG_KY_XE_TAXI : DomainEntity<int>, IDateTracking
    {
        public DANG_KY_XE_TAXI()
        {
        }

        public DateTime? NgaySuDung { get; set; }

        [StringLength(50)]
        public string TenNguoiSuDung { get; set; }

        [StringLength(50)]
        [Required]
        public string MaNV { get; set; }

        [StringLength(50)]
        public string BoPhan { get; set; }

        public DateTime? FromTimePlan { get; set; }

        public DateTime? ToTimePlan { get; set; }

        [StringLength(50)]
        public string FromTimePlan1 { get; set; }

        [StringLength(50)]
        public string ToTimePlan1 { get; set; }

        [StringLength(250)]
        public string ĐiaDiemXuatPhat { get; set; }

        [StringLength(250)]
        public string DiaDiemDen_SoNha { get; set; }

        [StringLength(250)]
        public string DiaDiemDen_Xa { get; set; }

        [StringLength(250)]
        public string DiaDiemDen_Huyen { get; set; }

        [StringLength(250)]
        public string DiaDiemDen_Tinh { get; set; }

        [StringLength(250)]
        public string MucDichSuDung { get; set; }

        [StringLength(500)]
        public string Lxe_BienSo { get; set; }

        public double? SoTien { get; set; }

        public double? SoNguoiSuDung { get; set; }

        [StringLength(50)]
        public string NguoiDangKy { get; set; }

        public bool XacNhanLV1 { get; set; }

        [StringLength(50)]
        public string Nguoi_XacNhanLV1 { get; set; }

        public bool XacNhanLV2 { get; set; }

        [StringLength(50)]
        public string Nguoi_XacNhanLV2 { get; set; }

        public bool XacNhanLV3 { get; set; }

        [StringLength(50)]
        public string Nguoi_XacNhanLV3 { get; set; }

        [StringLength(150)]
        public string MaBill { get; set; }

        [StringLength(50)]
        public string DateCreated { get; set; }

        [StringLength(50)]
        public string DateModified { get; set; }

        [StringLength(50)]
        public string UserCreated { get; set; }

        [StringLength(50)]
        public string UserModified { get; set; }

        [StringLength(50)]
        public string TaxiCardNo { get; set; }
    }

    [Table("TAXI_CARD_INFO")]
    public class TAXI_CARD_INFO : DomainEntity<int>, IDateTracking
    {
        [StringLength(50)]
        public string CardNo { get; set; }

        [StringLength(50)]
        public string CardName { get; set; }

        [StringLength(50)]
        public string DateCreated { get; set; }

        [StringLength(50)]
        public string DateModified { get; set; }

        [StringLength(50)]
        public string UserCreated { get; set; }

        [StringLength(50)]
        public string UserModified { get; set; }
    }

    [Table("MUCDICHSD_XE")]
    public class MUCDICHSD_XE : DomainEntity<int>, IDateTracking
    {
        [StringLength(250)]
        public string MucDich { get; set; }

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
