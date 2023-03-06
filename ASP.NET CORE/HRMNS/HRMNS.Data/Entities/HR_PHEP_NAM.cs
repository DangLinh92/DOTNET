using HRMNS.Data.Interfaces;
using HRMS.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HRMNS.Data.Entities
{
    [Table("HR_PHEP_NAM")]
    public class HR_PHEP_NAM : DomainEntity<int>, IDateTracking
    {
        public HR_PHEP_NAM()
        {

        }

        public HR_PHEP_NAM(int id,string maNV,float soPhepNam,float soPhepConLai,int year,decimal sotientra,DateTime? thoigiantra)
        {
            Id = id;
            MaNhanVien = maNV;
            SoPhepNam = soPhepNam;
            SoPhepConLai = soPhepConLai;
            Year = year;
            SoTienChiTra = sotientra;
            ThoiGianChiTra = thoigiantra;
        }

        [StringLength(50)]
        public string MaNhanVien { get; set; }
        public float SoPhepNam { get; set; }
        public float SoPhepConLai { get; set; }
        public int Year { get; set; }

        public decimal SoTienChiTra { get; set; }

        public DateTime? ThoiGianChiTra { get; set; }

        [StringLength(50)]
        public string DateCreated { get; set; }
        [StringLength(50)]
        public string DateModified { get; set; }
        [StringLength(50)]
        public string UserCreated { get; set; }
        [StringLength(50)]
        public string UserModified { get; set; }

        [ForeignKey("MaNhanVien")]
        public virtual HR_NHANVIEN HR_NHANVIEN { get; set; }
    }
}
