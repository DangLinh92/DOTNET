using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OPERATION_MNS.Application.ViewModels.Wlp2
{
    public class BoPhanDeNghiXuatNVLViewModel
    {
        public int Id { get; set; }

        [StringLength(50)]
        public string BoPhanDeNghi { get; set; }

        [StringLength(50)]
        public string NgayDeNghi { get; set; }

        [StringLength(50)]
        public string SanPham { get; set; }

        [StringLength(50)]
        public string Module { get; set; }

        [StringLength(50)]
        public string SapCode { get; set; }

        public float DinhMuc { get; set; }

        public string DonVi { get; set; }

        public float SoLuongYeuCau { get; set; }

        public float LuongThucTe { get; set; }

        [StringLength(250)]
        public string Note { get; set; }

        [StringLength(50)]
        public string DateCreated { get; set; }

        [StringLength(50)]
        public string DateModified { get; set; }

        [StringLength(50)]
        public string UserCreated { get; set; }

        [StringLength(50)]
        public string UserModified { get; set; }

        private DateTime _ngaydenghi2;
        public DateTime NgayDeNghi2
        {
            get => DateTime.Parse(NgayDeNghi);
            set
            {
                _ngaydenghi2 = value;
                NgayDeNghi = value.ToString("yyyy-MM-dd");
            }
        }
    }
}
