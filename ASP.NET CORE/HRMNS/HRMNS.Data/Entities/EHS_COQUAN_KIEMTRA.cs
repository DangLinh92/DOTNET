using HRMNS.Data.Interfaces;
using HRMS.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HRMNS.Data.Entities
{
    [Table("EHS_COQUAN_KIEMTRA")]
    public class EHS_COQUAN_KIEMTRA : DomainEntity<int>, IDateTracking
    {
        public EHS_COQUAN_KIEMTRA()
        {

        }

        public EHS_COQUAN_KIEMTRA(int id, string demuc, string coquan,string ngaykiemtra,string noidung,string ketqua,string noidungNG,string nguyennhan,string doisach,string tiendo)
        {
            Id = id;
            Demuc = demuc;
            CoQuanKiemTra = coquan;
            NgayKiemTra = ngaykiemtra;
            NoiDungKiemTra = noidung;
            KetQua = ketqua;
            NoiDungNG = noidungNG;
            NguyenNhan = nguyennhan;
            DoiSachCaiTien = doisach;
            TienDoCaiTien = tiendo;
        }

        [StringLength(500)]
        public string Demuc { get; set; }

        [StringLength(200)]
        public string CoQuanKiemTra { get; set; }

        [StringLength(50)]
        public string NgayKiemTra { get; set; }

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
