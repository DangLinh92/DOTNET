using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OPERATION_MNS.Application.ViewModels
{
    public class PostOpeationShippingViewModel
    {
        public int Id { get; set; }

        [StringLength(50)]
        public string MoveOutTime { get; set; }

        [StringLength(50)]
        public string LotID { get; set; }

        [StringLength(50)]
        public string Model { get; set; } //Material

        [StringLength(50)]
        public string CassetteID { get; set; }

        [StringLength(100)]
        public string Module { get; set; }

        [StringLength(50)]
        public string WaferId { get; set; }

        [StringLength(100)]
        public string WaferId_Mes { get; set; }

        public double? DefaultChipQty { get; set; }

        public double? OutputQty { get; set; }

        public double? ChipMesQty { get; set; }
        public double? ChipMapQty { get; set; }

        public double? DiffMapMes { get; set; }

        public double? Rate { get; set; }

        [StringLength(250)]
        public string VanDeDacBiet { get; set; }

        public double? WaferQty { get; set; }
        public double? ChipQty { get; set; }

        [StringLength(50)]
        public string NguoiXuat { get; set; }

        [StringLength(50)]
        public string NguoiKiemTraFA { get; set; }

        [StringLength(50)]
        public string NguoiNhan { get; set; }

        [StringLength(50)]
        public string NguoiKiemTra { get; set; }

        [StringLength(250)]
        public string GhiChu_XH2 { get; set; }

        [StringLength(250)]
        public string GhiChu_XH3 { get; set; }

        [StringLength(50)]
        public string DateCreated { get; set; }

        [StringLength(50)]
        public string DateModified { get; set; }

        [StringLength(50)]
        public string UserCreated { get; set; }

        [StringLength(50)]
        public string UserModified { get; set; }

        [StringLength(50)]
        public string KetQuaFAKiemTra { get; set; }

        public string InDB { get; set; }
    }
}
