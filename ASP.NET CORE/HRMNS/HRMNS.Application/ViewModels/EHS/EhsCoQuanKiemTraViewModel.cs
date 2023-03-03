using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HRMNS.Application.ViewModels.EHS
{
    public class EhsCoQuanKiemTraViewModel
    {
        public int Id { get; set; }

        [StringLength(500)]
        public string Demuc { get; set; }

        [StringLength(200)]
        public string CoQuanKiemTra { get; set; }

        [StringLength(50)]
        public string NgayKiemTra { get; set; }

        private DateTime ngaykt;
        public DateTime NgayKiemTra2
        {
            get
            {
                if (!string.IsNullOrEmpty(NgayKiemTra))
                {
                    return DateTime.Parse(NgayKiemTra);
                }
                return ngaykt;
            }

            set
            {
                ngaykt = value;
                NgayKiemTra = ngaykt.ToString("yyyy-MM-dd");
            }
        }

        [StringLength(500)]
        public string NoiDungKiemTra { get; set; }

        [StringLength(50)]
        public string KetQua { get; set; }

        [StringLength(1000)]
        public string NoiDungNG { get; set; }

        [StringLength(1000)]
        public string NguyenNhan { get; set; }

        [StringLength(1000)]
        public string DoiSachCaiTien { get; set; }

        [StringLength(200)]
        public string TienDoCaiTien { get; set; }

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
