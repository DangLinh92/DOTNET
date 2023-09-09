using OPERATION_MNS.Data.Interfaces;
using OPERATION_MNS.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace OPERATION_MNS.Data.Entities
{
    [Table("LOT_TEST_HISTOTY_LFEM")]
    public class LOT_TEST_HISTOTY_LFEM : DomainEntity<int>, IDateTracking
    {
        public DateTime? Date { get; set; }

        [StringLength(250)]
        public string HangMuc { get; set; }

        public bool ChiLayKetQua { get; set; }

        public bool FA_Module { get; set; }

        public bool DTC_Module { get; set; }

        [StringLength(250)]
        public string MucDich { get; set; }

        [StringLength(50)]
        public string ModelName { get; set; }

        [StringLength(50)]
        public string LotNo { get; set; }

        public double Qty { get; set; }

        [StringLength(50)]
        public string Operation { get; set; }

        [StringLength(150)]
        public string KetQua { get; set; }

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
