using OPERATION_MNS.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace OPERATION_MNS.Data.Entities
{
    [Table("VIEW_WIP_LOT_LIST_LFEM")]
    public class VIEW_WIP_LOT_LIST_LFEM : DomainEntity<int>
    {
        [StringLength(50)]
        public string MaterialCategory { get; set; }

        [StringLength(50)]
        public string MaterialGroup { get; set; }

        [StringLength(50)]
        public string Size { get; set; }

        [StringLength(50)]
        public string Material { get; set; }

        [StringLength(50)]
        public string MaterialName { get; set; }

        [StringLength(50)]
        public string LotID { get; set; }

        [StringLength(50)]
        public string ProductOrder { get; set; }

        [StringLength(50)]
        public string FAID { get; set; }

        [StringLength(50)]
        public string AssyLotID { get; set; }

        [StringLength(50)]
        public string MatVendor { get; set; }

        [StringLength(50)]
        public string LotCategory { get; set; }

        [StringLength(50)]
        public string LotType { get; set; }

        [StringLength(50)]
        public string Date { get; set; }

        [StringLength(50)]
        public string Operation { get; set; }

        [StringLength(50)]
        public string OperationName { get; set; }

        public int StayDay { get; set; }
        public decimal ChipQty { get; set; }

        [StringLength(50)]
        public string Unit { get; set; }

        [StringLength(50)]
        public string Status { get; set; }

        [StringLength(50)]
        public string Equipment { get; set; }

        [StringLength(50)]
        public string EquipmentName { get; set; }

        [StringLength(50)]
        public string Worker { get; set; }

        [StringLength(50)]
        public string Hold { get; set; }

        [StringLength(50)]
        public string Rework { get; set; }

        [StringLength(50)]
        public string ReworkCode { get; set; }

        [StringLength(50)]
        public string Site { get; set; }

        [StringLength(50)]
        public string Route { get; set; }

        [StringLength(50)]
        public string RouteName { get; set; }

        [StringLength(50)]
        public string MarkingNo { get; set; }

        [StringLength(50)]
        public string TotalInspection { get; set; }

        [StringLength(50)]
        public string IN_EX { get; set; }

        [StringLength(50)]
        public string VIEnd { get; set; }

        [StringLength(50)]
        public string ShipVendor { get; set; }

        [StringLength(50)]
        public string Comment { get; set; }

        [StringLength(50)]
        public string _Day { get; set; }

        [StringLength(50)]
        public string Time { get; set; }

        [StringLength(50)]
        public string OK_NG { get; set; }
    }
}
