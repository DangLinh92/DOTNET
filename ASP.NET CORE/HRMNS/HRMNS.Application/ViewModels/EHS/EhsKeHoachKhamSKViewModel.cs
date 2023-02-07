using System;
using System.Collections.Generic;
using System.Text;

namespace HRMNS.Application.ViewModels.EHS
{
    public class EhsKeHoachKhamSKViewModel
    {
        public EhsKeHoachKhamSKViewModel()
        {
            EHS_NHANVIEN_KHAM_SK = new HashSet<EhsNhanVienKhamSucKhoe>();
            EHS_NGAY_THUC_HIEN_CHITIET_KHAM_SK = new HashSet<EhsNgayThucHienChiTietKhamSKViewModel>();
        }

        public Guid Id { get; set; }

        public Guid MaDMKeHoach { get; set; }

        public string LuatDinhLienQuan { get; set; }

        public string NoiDung { get; set; }

        public string ChuKyThucHien { get; set; }

        public string Year { get; set; }

        public string NhaThau { get; set; }

        public string NguoiPhuTrach { get; set; }

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

        public string DateCreated { get; set; }

        public string DateModified { get; set; }

        public string UserCreated { get; set; }

        public string UserModified { get; set; }

        public ICollection<EhsNhanVienKhamSucKhoe> EHS_NHANVIEN_KHAM_SK { get; set; }
        public ICollection<EhsNgayThucHienChiTietKhamSKViewModel> EHS_NGAY_THUC_HIEN_CHITIET_KHAM_SK { get; set; }
    }
}
