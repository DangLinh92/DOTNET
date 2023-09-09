using HRMNS.Data.Interfaces;
using HRMS.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HRMNS.Data.Entities
{
    [Table("HR_BHXH")]
    public class HR_BHXH : DomainEntity<string>, IDateTracking
    {
        public HR_BHXH()
        {
        }

        public HR_BHXH(string maBHXH, string maNV, string ngayThamGia, string ngayKetThuc, string phanload, string thangThamGia)
        {
            Id = maBHXH;
            MaNV = maNV;
            NgayBatDau = ngayThamGia;
            NgayKetThuc = ngayKetThuc;
            PhanLoai = phanload;
            ThangThamGia = thangThamGia;    
        }

        [StringLength(50)]
        public string MaNV { get; set; }

        [StringLength(50)]
        public string NgayBatDau { get; set; }

        [StringLength(50)]
        public string NgayKetThuc { get; set; }

        [StringLength(50)]
        public string ThangThamGia { get; set; }

        [StringLength(50)]
        public string PhanLoai { get; set; } // Tham gia / khong tham gia

        [StringLength(50)]
        public string DateCreated { get; set; }

        [StringLength(50)]
        public string DateModified { get; set; }

        [StringLength(50)]
        public string UserCreated { get; set; }

        [StringLength(50)]
        public string UserModified { get; set; }

        [ForeignKey("MaNV")]
        public virtual HR_NHANVIEN HR_NHANVIEN { get; set; }
    }
}
