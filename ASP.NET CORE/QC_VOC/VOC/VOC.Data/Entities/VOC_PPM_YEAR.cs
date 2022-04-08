using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using VOC.Data.Interfaces;
using VOC.Infrastructure.SharedKernel;

namespace VOC.Data.Entities
{
    [Table("VOC_PPM_YEAR")]
    public class VOC_PPM_YEAR : DomainEntity<int>, IDateTracking
    {
        public VOC_PPM_YEAR()
        {

        }
        public VOC_PPM_YEAR(int id,int year,string module,double value,double target)
        {
            Id = id;
            Year = year;
            Module = module;
            ValuePPM = value;
            TargetPPM = target;
        }

        public int Year { get; set; }

        [StringLength(50)]
        public string Module { get; set; } // LFEM/CSP

        public double ValuePPM { get; set; }

        public double TargetPPM { get; set; }

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
