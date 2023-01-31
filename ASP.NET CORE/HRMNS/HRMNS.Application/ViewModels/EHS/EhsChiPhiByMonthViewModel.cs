using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HRMNS.Application.ViewModels.EHS
{
    public class EhsChiPhiByMonthViewModel
    {
        public int Id { get; set; }

        [StringLength(50)]
        public string Year { get; set; }

        public Guid MaNoiDung { get; set; }

        public double ChiPhi1 { get; set; }
        public double ChiPhi2 { get; set; }
        public double ChiPhi3 { get; set; }
        public double ChiPhi4 { get; set; }
        public double ChiPhi5 { get; set; }
        public double ChiPhi6 { get; set; }
        public double ChiPhi7 { get; set; }
        public double ChiPhi8 { get; set; }
        public double ChiPhi9 { get; set; }
        public double ChiPhi10 { get; set; }
        public double ChiPhi11 { get; set; }
        public double ChiPhi12 { get; set; }

        [StringLength(50)]
        public string DateCreated { get; set; }

        [StringLength(50)]
        public string DateModified { get; set; }

        [StringLength(50)]
        public string UserCreated { get; set; }

        [StringLength(50)]
        public string UserModified { get; set; }

        public string TenNoiDung { get; set; }

        public EhsNoiDungViewModel EHS_NOIDUNG { get; set; }
    }
}
