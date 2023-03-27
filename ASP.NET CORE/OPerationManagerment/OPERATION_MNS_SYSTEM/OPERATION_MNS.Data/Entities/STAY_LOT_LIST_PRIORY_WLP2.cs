using OPERATION_MNS.Data.Interfaces;
using OPERATION_MNS.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace OPERATION_MNS.Data.Entities
{
    [Table("STAY_LOT_LIST_PRIORY_WLP2")]
    public class STAY_LOT_LIST_PRIORY_WLP2 : DomainEntity<int>, IDateTracking
    {
        public STAY_LOT_LIST_PRIORY_WLP2()
        {

        }

        public STAY_LOT_LIST_PRIORY_WLP2(int id,string sapCode,bool priory,int number_Priory,string material,string cassetteID,
            string lotID,string operationName,string operationId,string eRPProductionOrder,float chipQty,float stayDay)
        {
            Id = id;
            SapCode = sapCode;
            Priory = priory;
            Number_Priory = number_Priory;
            Material = material;
            CassetteID = cassetteID;
            LotID = lotID;
            OperationName = operationName;
            OperationId = operationId;
            ERPProductionOrder = eRPProductionOrder;
            ChipQty = chipQty;
            StayDay = stayDay;
        }

        [StringLength(50)]
        public string SapCode { get; set; }

        public bool Priory { get; set; }

        public int Number_Priory { get; set; }

        [StringLength(50)]
        public string Material { get; set; }

        [StringLength(50)]
        public string CassetteID { get; set; }

        [StringLength(50)]
        public string LotID { get; set; }

        [StringLength(50)]
        public string OperationName { get; set; }

        [StringLength(50)]
        public string OperationId { get; set; }

        [StringLength(50)]
        public string ERPProductionOrder { get; set; }

        public float ChipQty { get; set; }

        public float StayDay { get; set; }

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
