using HRMNS.Data.Interfaces;
using HRMS.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HRMNS.Data.Entities
{
    [Table("EHS_HANGMUC_NG")]
    public class EHS_HANGMUC_NG : DomainEntity<int>, IDateTracking
    {
        public EHS_HANGMUC_NG()
        {
        }

        public EHS_HANGMUC_NG(int id, string maNgayChiTiet, string hangmucNG, string vande, string nguyennhan, string doisach, string tinhhinh,string demuc)
        {
            Id = id;
            MaNgayChiTiet = maNgayChiTiet;
            HangMucNG = hangmucNG;
            NoiDungVanDeNG = vande;
            NguyenNhan = nguyennhan;
            DoiSachCaiTien = doisach;
            TinhHinhCaiTien = tinhhinh;
            DeMuc = demuc;
        }

        [StringLength(50)]
        public string MaNgayChiTiet { get; set; }

        [StringLength(500)]
        public string HangMucNG { get; set; }

        [StringLength(1000)]
        public string NoiDungVanDeNG { get; set; }

        [StringLength(500)]
        public string NguyenNhan { get; set; }

        [StringLength(1000)]
        public string DoiSachCaiTien { get; set; }

        [StringLength(250)]
        public string TinhHinhCaiTien { get; set; }

        [StringLength(250)]
        public string DeMuc { get; set; }

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
