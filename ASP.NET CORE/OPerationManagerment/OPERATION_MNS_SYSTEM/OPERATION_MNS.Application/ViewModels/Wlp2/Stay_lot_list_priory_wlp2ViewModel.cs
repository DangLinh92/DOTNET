using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OPERATION_MNS.Application.ViewModels.Wlp2
{
    public class Stay_lot_list_priory_wlp2ViewModel
    {
        public int Id { get; set; }

        public int STT;

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

        public string Key { get => SapCode + "^" + OperationId + "^" + Material + "^" + CassetteID + "^" + LotID; }
    }
}
