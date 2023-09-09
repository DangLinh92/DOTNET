using OPERATION_MNS.Data.Interfaces;
using OPERATION_MNS.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace OPERATION_MNS.Data.Entities
{
    [Table("STAY_LOT_LIST_PRIORY_LFEM")]
    public class STAY_LOT_LIST_PRIORY_LFEM : DomainEntity<int>, IDateTracking
    {
        public STAY_LOT_LIST_PRIORY_LFEM()
        {

        }

        public STAY_LOT_LIST_PRIORY_LFEM(int id, string mesItem, bool priory, float stayDay, string size, string lotID, string productOrder, string operationName, string operationId,
            float chipQty, string fAID, string assyLotID, string date, float dateDiff, string unit, string startFlag, string equipmentName,
            string worker, int number_Priory)
        {
            Id = id;
            MesItem = mesItem;
            Priory = priory;
            Size = size;
            StayDay = stayDay;
            LotID = lotID;
            OperationName = operationName;
            OperationId = operationId;
            ProductOrder = productOrder;
            ChipQty = chipQty;
            FAID = fAID;
            AssyLotID = assyLotID;
            Date = date;
            DateDiff = dateDiff;
            Unit = unit;
            StartFlag = startFlag;
            EquipmentName = equipmentName;
            Worker = worker;
            Number_Priory = number_Priory;
        }

        public float StayDay { get; set; }

        [StringLength(50)]
        public string Size { get; set; }

        [StringLength(50)]
        public string MesItem { get; set; } // Material

        [StringLength(50)]
        public string LotID { get; set; }

        [StringLength(50)]
        public string ProductOrder { get; set; }

        [StringLength(50)]
        public string FAID { get; set; }

        [StringLength(50)]
        public string AssyLotID { get; set; }

        [StringLength(50)]
        public string Date { get; set; }

        [StringLength(50)]
        public string OperationName { get; set; }

        [StringLength(50)]
        public string OperationId { get; set; }

        public float DateDiff { get; set; }
        public float ChipQty { get; set; }

        [StringLength(50)]
        public string Unit { get; set; }

        [StringLength(50)]
        public string StartFlag { get; set; }

        [StringLength(50)]
        public string EquipmentName { get; set; }

        [StringLength(50)]
        public string Worker { get; set; }
        public bool Priory { get; set; }
        public int Number_Priory { get; set; }

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
