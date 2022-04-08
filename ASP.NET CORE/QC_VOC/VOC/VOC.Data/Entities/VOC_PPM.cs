using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using VOC.Data.Interfaces;
using VOC.Infrastructure.SharedKernel;

namespace VOC.Data.Entities
{
    [Table("VOC_PPM")]
    public class VOC_PPM : DomainEntity<int>, IDateTracking
    {
        public VOC_PPM()
        {

        }
        public VOC_PPM(int id, string module, string customer, string type, int year, int month, double value, double target)
        {
            Id = id;
            Module = module;
            Customer = customer;
            Type = type;
            Year = year;
            Month = month;
            Value = value;
            TargetValue = target;
        }

        [StringLength(50)]
        public string Module { get; set; } // CSP,LFEM

        [StringLength(50)]
        public string Customer { get; set; } // SEV,SEVT

        [StringLength(50)]
        public string Type { get; set; } // Input - Defect

        public int Year { get; set; }
        public int Month { get; set; }

        public double Value { get; set; }

        public double TargetValue { get; set; } // Target PPM

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
