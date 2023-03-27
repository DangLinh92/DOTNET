using OPERATION_MNS.Data.Interfaces;
using OPERATION_MNS.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace OPERATION_MNS.Data.Entities
{
    [Table("OUTGOING_RECEIPT_WLP2")]
    public class OUTGOING_RECEIPT_WLP2 : DomainEntity<int>, IDateTracking
    {
        [StringLength(150)]
        public string Module { get; set; } // 

        [StringLength(50)]
        public string LotId { get; set; }

        [StringLength(50)]
        public string SapCode { get; set; }

        [StringLength(50)]
        public string NgayXuat { get; set; }

        public float SoLuongYeuCau { get; set; }


        public float LuongDuKien_1 { get; set; }

        [StringLength(250)]
        public string ThoiGianDuKien_1 { get; set; }

        public float LuongDuKien_2 { get; set; }

        [StringLength(250)]
        public string ThoiGianDuKien_2 { get; set; }

        public float LuongDuKien_3 { get; set; }

        [StringLength(250)]
        public string ThoiGianDuKien_3 { get; set; }

        public float LuongDuKien_4 { get; set; }

        [StringLength(250)]
        public string ThoiGianDuKien_4 { get; set; }

        public float LuongDuKien_5 { get; set; }

        [StringLength(250)]
        public string ThoiGianDuKien_5 { get; set; }

        public float LuongThucTe_1 { get; set; }

        [StringLength(250)]
        public string ThoiGianThucTe_1 { get; set; }

        public float LuongThucTe_2 { get; set; }

        [StringLength(250)]
        public string ThoiGianThucTe_2 { get; set; }

        public float LuongThucTe_3 { get; set; }

        [StringLength(250)]
        public string ThoiGianThucTe_3 { get; set; }

        public float LuongThucTe_4 { get; set; }

        [StringLength(250)]
        public string ThoiGianThucTe_4 { get; set; }

        public float LuongThucTe_5 { get; set; }

        [StringLength(250)]
        public string ThoiGianThucTe_5 { get; set; }

        [StringLength(250)]
        public string Note { get; set; }

        [StringLength(50)]
        public string NguoiGiao { get; set; }

        [StringLength(50)]
        public string NguoiNhan { get; set; }

        [StringLength(50)]
        public string DateCreated { get; set; }

        [StringLength(50)]
        public string DateModified { get; set; }

        [StringLength(50)]
        public string UserCreated { get; set; }

        [StringLength(50)]
        public string UserModified { get; set; }

        public float ChenhLechDuKien { get => LuongDuKien_1 + LuongDuKien_2 + LuongDuKien_3 + LuongDuKien_4 + LuongDuKien_5 - SoLuongYeuCau; }
        public float ChenhLechThucTe { get => LuongThucTe_1 + LuongThucTe_2 + LuongThucTe_3 + LuongThucTe_4 + LuongThucTe_5 - SoLuongYeuCau; }

        public string Key { get => SapCode +"."+ NgayXuat; }
    }
}
