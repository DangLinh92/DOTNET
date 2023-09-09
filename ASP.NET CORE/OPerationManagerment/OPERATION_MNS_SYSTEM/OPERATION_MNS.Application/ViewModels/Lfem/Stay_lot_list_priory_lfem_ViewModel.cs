using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OPERATION_MNS.Application.ViewModels.Lfem
{
    public class Stay_lot_list_priory_lfem_ViewModel
    {
        public int STT { get; set; }
        public int Id { get; set; }
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
        public string Unit { get; set; }
        public string StartFlag { get; set; }
        public string EquipmentName { get; set; }
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

        public string Key { get => MesItem + "^" + OperationId + "^" + LotID; }
    }
}
