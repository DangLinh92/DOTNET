using HRMNS.Data.Interfaces;
using HRMS.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HRMNS.Data.Entities
{
    [Table("EHS_NHANVIEN_KHAM_SK")]
    public class EHS_NHANVIEN_KHAM_SK : DomainEntity<int>, IDateTracking
    {
        public EHS_NHANVIEN_KHAM_SK()
        {

        }

        public EHS_NHANVIEN_KHAM_SK(int id,Guid maKhamSK,string thoigian,string mavn,string tenvn,string section,string note)
        {
            Id = id;
            MaKHKhamSK = maKhamSK;
            ThoiGianKhamSK = thoigian;
            MaNV = mavn;
            TenNV = tenvn;
            Section = section;
            Note = note;
        }

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

        [ForeignKey("MaKHKhamSK")]
        public virtual EHS_KE_HOACH_KHAM_SK EHS_KE_HOACH_KHAM_SK { get;set;}
    }
}
