using OPERATION_MNS.Data.Interfaces;
using OPERATION_MNS.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace OPERATION_MNS.Data.Entities
{
    [Table("THICKNET_MODEL_WLP2")]
    public  class THICKNET_MODEL_WLP2 : DomainEntity<int>, IDateTracking
    {
        public THICKNET_MODEL_WLP2()
        {

        }
        public THICKNET_MODEL_WLP2(int id,string material,float thicknet)
        {
            Id = id;
            Material = material;
            ThickNet = thicknet;
        }

        [StringLength(50)]
        public string Material { get; set; }

        public float ThickNet { get; set; }

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
