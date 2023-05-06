using OPERATION_MNS.Data.Interfaces;
using OPERATION_MNS.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace OPERATION_MNS.Data.Entities
{
    [Table("CTQ_SETTING_WLP2")]
    public class CTQ_SETTING_WLP2 : DomainEntity<int>, IDateTracking
    {
        public CTQ_SETTING_WLP2()
        {

        }

        public CTQ_SETTING_WLP2(string model,string operationID, string operationName,float thickNet,double min,double max)
        {
            SpeacialModel = model;
            OperationID = operationID;
            OperationName = operationName;
            ThickNet = thickNet;
            MinV = min;
            MaxV = max;
        }

        // material trên mes
        [StringLength(50)]
        public string SpeacialModel { get; set; }

        [StringLength(50)]
        public string OperationID { get; set; }

        [StringLength(50)]
        public string OperationName { get; set; }

        public float ThickNet { get; set; } // độ dày

        public double MinV { get; set; }

        public double MaxV { get; set; }

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
