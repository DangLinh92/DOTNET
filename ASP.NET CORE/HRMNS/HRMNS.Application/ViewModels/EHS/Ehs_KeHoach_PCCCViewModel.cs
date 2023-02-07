using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HRMNS.Application.ViewModels.EHS
{
    public class Ehs_KeHoach_PCCCViewModel
    {
        public Ehs_KeHoach_PCCCViewModel()
        {
            EHS_THOIGIAN_THUC_HIEN_PCCC = new HashSet<EhsThoiGianThucHienPCCCViewModel>();
        }

        public Guid Id { get; set; }

        public Guid MaDMKeHoach { get; set; }

        public int STT { get; set; }

        [StringLength(500)]
        public string HangMuc { get; set; }

        [StringLength(1000)]
        public string NoiDung { get; set; }

        [StringLength(50)]
        public string ChuKyThucHien { get; set; }

        [StringLength(10)]
        public string Year { get; set; }

        [StringLength(250)]
        public string ThoiGianDaoTao { get; set; }

        [StringLength(50)]
        public string NguoiPhuTrach { get; set; }

        [StringLength(50)]
        public string NhaThau { get; set; }

        public double CostMonth_1 { get; set; }
        public double CostMonth_2 { get; set; }
        public double CostMonth_3 { get; set; }
        public double CostMonth_4 { get; set; }
        public double CostMonth_5 { get; set; }
        public double CostMonth_6 { get; set; }
        public double CostMonth_7 { get; set; }
        public double CostMonth_8 { get; set; }
        public double CostMonth_9 { get; set; }
        public double CostMonth_10 { get; set; }
        public double CostMonth_11 { get; set; }
        public double CostMonth_12 { get; set; }

        public double CostTotal
        {
            get => CostMonth_1 + CostMonth_2 + CostMonth_3 + CostMonth_4 + CostMonth_5 + CostMonth_6 + CostMonth_7 + CostMonth_8 + CostMonth_9 + CostMonth_10 + CostMonth_11 + CostMonth_12;
        }

        [StringLength(50)]
        public string DateCreated { get; set; }

        [StringLength(50)]
        public string DateModified { get; set; }

        [StringLength(50)]
        public string UserCreated { get; set; }

        [StringLength(50)]
        public string UserModified { get; set; }

        public EhsDMKeHoachViewModel EHS_DM_KEHOACH { get; set; }

        public ICollection<EhsThoiGianThucHienPCCCViewModel> EHS_THOIGIAN_THUC_HIEN_PCCC { get; set; }
    }
}
