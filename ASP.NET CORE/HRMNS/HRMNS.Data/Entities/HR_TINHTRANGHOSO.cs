using HRMNS.Data.Interfaces;
using HRMS.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HRMNS.Data.Entities
{
    [Table("HR_TINHTRANGHOSO")]
    public class HR_TINHTRANGHOSO : DomainEntity<int>, IDateTracking
    {
        public HR_TINHTRANGHOSO()
        {

        }

        public HR_TINHTRANGHOSO(int id,string maNV, bool soYeuLyLich, bool cmtnd, bool sohokhau, bool giayKhaiSinh, bool bangTotNghiep, bool xacNhanDanSu, bool anhThe)
        {
            Id = id;
            MaNV = maNV;
            SoYeuLyLich = soYeuLyLich;
            CMTND = cmtnd;
            SoHoKhau = sohokhau;
            GiayKhaiSinh = giayKhaiSinh;
            BangTotNghiep = bangTotNghiep;
            XacNhanDanSu = xacNhanDanSu;
            AnhThe = anhThe;
        }

        [StringLength(50)]
        public string MaNV { get; set; }

        public bool SoYeuLyLich { get; set; }

        public bool CMTND { get; set; }

        public bool SoHoKhau { get; set; }

        public bool GiayKhaiSinh { get; set; }

        public bool BangTotNghiep { get; set; }

        public bool XacNhanDanSu { get; set; }

        public bool AnhThe { get; set; }

        [StringLength(50)]
        public string DateCreated { get; set; }

        [StringLength(50)]
        public string DateModified { get; set; }

        [StringLength(50)]
        public string UserCreated { get; set; }

        [StringLength(50)]
        public string UserModified { get; set; }

        [StringLength(50)]
        public string Status { get; set; }

        [ForeignKey("MaNV")]
        public virtual HR_NHANVIEN HR_NHANVIEN { get; set; }
    }
}
