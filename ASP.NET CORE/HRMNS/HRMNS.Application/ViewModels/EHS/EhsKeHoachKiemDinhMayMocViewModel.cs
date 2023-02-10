using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HRMNS.Application.ViewModels.EHS
{
    public class EhsKeHoachKiemDinhMayMocViewModel
    {
        public EhsKeHoachKiemDinhMayMocViewModel()
        {
            EHS_THOIGIAN_THUC_HIEN_KIEMDINH_MM = new HashSet<EhsThoiGianKiemDinhMayMocViewModel>();
        }

        public Guid Id { get; set; }

        public Guid MaDMKeHoach { get; set; }

        public int STT { get; set; }

        [StringLength(500)]
        public string TenMayMoc { get; set; }

        [StringLength(50)]
        public string ChuKyKiemDinh { get; set; }

        public int SoLuongThietBi { get; set; }

        [StringLength(250)]
        public string ViTri { get; set; }

        [StringLength(50)]
        public string NguoiPhuTrach { get; set; }

        [StringLength(50)]
        public string NhaThau { get; set; }

        [StringLength(10)]
        public string Year { get; set; }

        [StringLength(50)]
        public string LanKiemDinhKeTiep { get; set; }

        [StringLength(50)]
        public string LanKiemDinhKeTiep1 { get; set; }

        [StringLength(50)]
        public string LanKiemDinhKeTiep2 { get; set; }

        [StringLength(50)]
        public string LanKiemDinhKeTiep3 { get; set; }

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
        public ICollection<EhsThoiGianKiemDinhMayMocViewModel> EHS_THOIGIAN_THUC_HIEN_KIEMDINH_MM { get; set; }
    }
}
