using System;
using System.Collections.Generic;
using System.Text;

namespace OPERATION_MNS.Application.ViewModels.Lfem
{
    public class SanXuat_XuatHang_ViewModel
    {
        public SanXuat_XuatHang_ViewModel()
        {
            SX_XH = new List<SX_XH>();
        }
        public string MesItem { get; set; }
        public string Model { get; set; }
        public string Thang { get; set; }
        public float TonDauKy { get; set; }
        public List<SX_XH> SX_XH { get; set; }

        public float D_1 { get; set; }
        public float D_2 { get; set; }
        public float D_3 { get; set; }
        public float D_4 { get; set; }
        public float D_5 { get; set; }
        public float D_6 { get; set; }
        public float D_7 { get; set; }
        public float D_8 { get; set; }
        public float D_9 { get; set; }
        public float D_10 { get; set; }
        public float D_11 { get; set; }
        public float D_12 { get; set; }
        public float D_13 { get; set; }
        public float D_14 { get; set; }
        public float D_15 { get; set; }
        public float D_16 { get; set; }
        public float D_17 { get; set; }
        public float D_18 { get; set; }
        public float D_19 { get; set; }
        public float D_20 { get; set; }
        public float D_21 { get; set; }
        public float D_22 { get; set; }
        public float D_23 { get; set; }
        public float D_24 { get; set; }
        public float D_25 { get; set; }
        public float D_26 { get; set; }
        public float D_27 { get; set; }
        public float D_28 { get; set; }
        public float D_29 { get; set; }
        public float D_30 { get; set; }
        public float D_31 { get; set; }
    }

    public class SX_XH
    {
        public string Day { get; set; }

        // thực tế sx
        public float TTSX { get; set; }

        // kế hoạch xuất hàng
        public float KHXH { get; set; }

        // lượng thiếu hụt hàng
        public float QtyThieu { get; set; }
    }
}
