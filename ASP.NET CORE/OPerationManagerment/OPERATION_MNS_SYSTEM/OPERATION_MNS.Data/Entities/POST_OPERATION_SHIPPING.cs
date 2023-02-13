using OPERATION_MNS.Data.Interfaces;
using OPERATION_MNS.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace OPERATION_MNS.Data.Entities
{
    [Table("POST_OPERATION_SHIPPING")]
    public class POST_OPERATION_SHIPPING : DomainEntity<int>, IDateTracking
    {
        public POST_OPERATION_SHIPPING()
        {
             
        }

        public POST_OPERATION_SHIPPING(string moveOutTime,string lotID,string model,
            string cassetteID,string module,string waferId, double? defaultChipQty,
            double? outputQty, double? chipMesQty, double? diffMapMes, double? rate,
            string vanDeDacBiet, double? waferQty, double? chipQty,
            string nguoiXuat, string nguoiKiemTraFA, string nguoiNhan, string nguoiKiemTra, 
            string ghiChu_XH2, string ghiChu_XH3,double? _chipMapQty,string ketquaFA,string waferIdMes)
        {
            MoveOutTime = moveOutTime;
            LotID = lotID;
            Model = model;
            CassetteID = cassetteID;
            Module = module;
            WaferId = waferId;
            DefaultChipQty = defaultChipQty;
            OutputQty = outputQty;
            ChipMesQty = chipMesQty;
            DiffMapMes = diffMapMes;
            Rate = rate;
            VanDeDacBiet = vanDeDacBiet;
            WaferQty = waferQty;
            ChipQty = chipQty;
            NguoiXuat = nguoiXuat;
            NguoiKiemTraFA = nguoiKiemTraFA;
            NguoiNhan = nguoiNhan;
            NguoiKiemTra = nguoiKiemTra;
            GhiChu_XH2 = ghiChu_XH2;
            GhiChu_XH3 = ghiChu_XH3;
            ChipMapQty = _chipMapQty;
            KetQuaFAKiemTra = ketquaFA;
            WaferId_Mes = waferIdMes;
        }

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
        public string KetQuaFAKiemTra { get; set; }

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
    }
}
