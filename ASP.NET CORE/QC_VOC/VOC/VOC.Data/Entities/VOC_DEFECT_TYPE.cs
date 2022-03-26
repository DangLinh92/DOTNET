using VOC.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using VOC.Data.Interfaces;

namespace VOC.Data.Entities
{
    [Table("VOC_DEFECT_TYPE")]
    public class VOC_DEFECT_TYPE : DomainEntity<int>, IDateTracking
    {
        public VOC_DEFECT_TYPE()
        {
        }

        public VOC_DEFECT_TYPE(string engsnotation,string koreannotation)
        {
            EngsNotation = engsnotation;
            KoreanNotation = koreannotation;
        }

        [StringLength(250)]
        public string EngsNotation { get; set; }

        [StringLength(250)]
        public string KoreanNotation { get; set; }
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
