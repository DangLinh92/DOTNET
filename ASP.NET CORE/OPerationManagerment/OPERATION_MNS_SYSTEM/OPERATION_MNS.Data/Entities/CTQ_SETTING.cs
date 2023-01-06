using OPERATION_MNS.Data.Interfaces;
using OPERATION_MNS.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace OPERATION_MNS.Data.Entities
{
    [Table("CTQ_SETTING")]
    public class CTQ_SETTING : DomainEntity<int>, IDateTracking
    {
        public CTQ_SETTING()
        {

        }

        public CTQ_SETTING(string operationID, string operationName,double lWL,double uWL)
        {
            OperationID = operationID;
            OperationName = operationName;
            LWL = lWL;
            UWL = uWL;
        }

        [StringLength(50)]
        public string OperationID { get; set; }

        [StringLength(50)]
        public string OperationName { get; set; }

        public double LWL { get; set; }

        public double UWL { get; set; }

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
