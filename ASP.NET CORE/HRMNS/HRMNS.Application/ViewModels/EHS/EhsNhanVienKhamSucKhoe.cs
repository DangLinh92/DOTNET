using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HRMNS.Application.ViewModels.EHS
{
    public class EhsNhanVienKhamSucKhoe
    {
        public int Id { get; set; }

        public Guid MaKHKhamSK { get; set; }

        [StringLength(50)]
        public string ThoiGianKhamSK { get; set; }

        [StringLength(50)]
        public string MaNV { get; set; }

        [StringLength(150)]
        public string TenNV { get; set; }

        [StringLength(50)]
        public string Section { get; set; }

        [StringLength(500)]
        public string Note { get; set; }

        [StringLength(50)]
        public string DateCreated { get; set; }

        [StringLength(50)]
        public string DateModified { get; set; }

        [StringLength(50)]
        public string UserCreated { get; set; }

        [StringLength(50)]
        public string UserModified { get; set; }

        public EhsKeHoachKhamSKViewModel EHS_KE_HOACH_KHAM_SK { get; set; }
    }
}
