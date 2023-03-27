using OPERATION_MNS.Data.Interfaces;
using OPERATION_MNS.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OPERATION_MNS.Data.Entities
{
    public class VIEW_WIP_POST_WLP : DomainEntity<int>, IDateTracking
    {
        [StringLength(50)]
        public string Material_SAP_CODE { get; set; }

        [StringLength(50)]
        public string Category { get; set; }

        [StringLength(50)]
        public string Series { get; set; }
        [StringLength(50)]
        public string Model { get; set; }

        [StringLength(50)]
        public string Material { get; set; }

        public int Total { get; set; }
        public int CassetteInputStock { get; set; }
        public int Wait { get; set; }
        public int PostOperationInputWait { get; set; }
        public int OQC_WaferInspection { get; set; }
        public int WaferCarrierPacking { get; set; }
        public int InputCheck { get; set; }
        public int WaferShipping { get; set; }
        public int Marking { get; set; }
        public int NgoaiQuanSauMarking { get; set; }
        public int TapeLamination { get; set; }
        public int NgoaiQuanSauLamination { get; set; }
        public int BackGrinding { get; set; }
        public int NgoaiQuanSauBackGrinding { get; set; }
        public int TapeDelamination { get; set; }
        public int NgoaiQuanSauMDS { get; set; }
        public int WaferOven { get; set; }
        public int WaferDicing { get; set; }
        public int UVInspection { get; set; }
        public int DeTaping { get; set; }
        public int ChipVisualInspection { get; set; }
        public int ReelPacking { get; set; }
        public int ReelOperationInput { get; set; }
        public int ReelVisualInspection { get; set; }
        public int ReelCounter { get; set; }
        public int ReelOven { get; set; }
        public int OneStPackingLabel { get; set; }
        public int OQC { get; set; }
        public int OneSTPacking { get; set; }
        public int Shipping { get; set; }

        [StringLength(5)]
        public string Hold_Flag { get; set; }

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
