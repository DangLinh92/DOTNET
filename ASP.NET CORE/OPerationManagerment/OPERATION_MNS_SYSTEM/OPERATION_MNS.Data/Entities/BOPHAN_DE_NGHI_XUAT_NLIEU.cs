using OPERATION_MNS.Data.Interfaces;
using OPERATION_MNS.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace OPERATION_MNS.Data.Entities
{
    [Table("BOPHAN_DE_NGHI_XUAT_NLIEU")]
    public class BOPHAN_DE_NGHI_XUAT_NLIEU : DomainEntity<int>, IDateTracking
    {
        public BOPHAN_DE_NGHI_XUAT_NLIEU()
        {

        }

        public BOPHAN_DE_NGHI_XUAT_NLIEU(int id,string boPhanDeNghi,string ngayDeNghi,string sanPham,string module,string sapCode,
            float dinhmuc,string donvi,float soLuongYeuCau,float luongThucTe,string node)
        {
            Id = id;
            BoPhanDeNghi = boPhanDeNghi;
            NgayDeNghi = ngayDeNghi;
            SanPham = sanPham;
            Module = module;
            SapCode = sapCode;
            DinhMuc = dinhmuc;
            DonVi = donvi;
            SoLuongYeuCau = soLuongYeuCau;
            LuongThucTe = luongThucTe;
            Note = node;
        }

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
    }
}
