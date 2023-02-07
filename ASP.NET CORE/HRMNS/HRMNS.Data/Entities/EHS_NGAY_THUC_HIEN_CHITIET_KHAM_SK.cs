using HRMNS.Data.Interfaces;
using HRMS.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HRMNS.Data.Entities
{
    [Table("EHS_NGAY_THUC_HIEN_CHITIET_KHAM_SK")]
    public class EHS_NGAY_THUC_HIEN_CHITIET_KHAM_SK : DomainEntity<int>, IDateTracking
    {
        public EHS_NGAY_THUC_HIEN_CHITIET_KHAM_SK()
        {

        }

        public EHS_NGAY_THUC_HIEN_CHITIET_KHAM_SK(Guid maEvent, Guid maKHKhamSK,string noidung,string ngayBatDau,string ngayKetThuc)
        {
            MaEvent = maEvent;
            MaKHKhamSK = maKHKhamSK;
            NoiDung = noidung;
            NgayKetThuc = ngayBatDau;
            NgayKetThuc = ngayKetThuc;
        }

        public Guid MaEvent { get; set; }

        public Guid MaKHKhamSK { get; set; }

        [StringLength(1000)]
        public string NoiDung { get; set; }

        [StringLength(50)]
        public string NgayBatDau { get; set; }

        [StringLength(50)]
        public string NgayKetThuc { get; set; }

        [StringLength(50)]
        public string DateCreated { get; set; }

        [StringLength(50)]
        public string DateModified { get; set; }

        [StringLength(50)]
        public string UserCreated { get; set; }

        [StringLength(50)]
        public string UserModified { get; set; }

        [ForeignKey("MaKHKhamSK")]
        public virtual EHS_KE_HOACH_KHAM_SK EHS_KE_HOACH_KHAM_SK { get; set; }

        [ForeignKey("MaEvent")]
        public virtual EVENT_SHEDULE_PARENT EVENT_SHEDULE_PARENT { get; set; }
    }
}
