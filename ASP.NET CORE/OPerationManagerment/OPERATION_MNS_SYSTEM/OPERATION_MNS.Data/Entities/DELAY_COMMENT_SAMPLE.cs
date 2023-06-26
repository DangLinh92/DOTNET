using OPERATION_MNS.Data.Interfaces;
using OPERATION_MNS.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace OPERATION_MNS.Data.Entities
{
    [Table("DELAY_COMMENT_SAMPLE")]
    public class DELAY_COMMENT_SAMPLE : DomainEntity<int>, IDateTracking
    {
        public DELAY_COMMENT_SAMPLE()
        {

        }

        public int MaTinhHinhSX { get; set; }

        /// <summary>
        /// Công đoạn delay
        /// </summary>
        [StringLength(250)]
        public string Wall { get; set; }

        [StringLength(250)]
        public string Roof { get; set; }

        [StringLength(250)]
        public string Seed { get; set; }

        [StringLength(250)]
        public string PlatePR { get; set; }

        [StringLength(250)]
        public string Plate { get; set; }

        [StringLength(250)]
        public string PreProbe { get; set; }

        [StringLength(250)]
        public string PreDicing { get; set; }

        [StringLength(250)]
        public string AllProbe { get; set; }

        [StringLength(250)]
        public string BG { get; set; }

        [StringLength(250)]
        public string Dicing { get; set; }

        [StringLength(250)]
        public string ChipIns { get; set; }

        [StringLength(250)]
        public string Packing { get; set; }

        [StringLength(250)]
        public string OQC { get; set; }

        [StringLength(250)]
        public string Shipping { get; set; }

        [StringLength(50)]
        public string DateCreated { get; set; }

        [StringLength(50)]
        public string DateModified { get; set; }

        [StringLength(50)]
        public string UserCreated { get; set; }

        [StringLength(50)]
        public string UserModified { get; set; }

        [ForeignKey("MaTinhHinhSX")]
        public TINH_HINH_SAN_XUAT_SAMPLE TINH_HINH_SAN_XUAT_SAMPLE { get; set; }
    }
}
