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

        public EHS_NGAY_THUC_HIEN_CHITIET_KHAM_SK(int id,Guid maEvent, Guid maKHKhamSK,string noidung,string ngayBatDau,string ngayKetThuc,
            string status,int progress,string priority,string isShowBoard,string actualFinish,string fileName,string fileUrl)
        {
            Id = id;
            MaEvent = maEvent;
            MaKHKhamSK = maKHKhamSK;
            NoiDung = noidung;
            NgayKetThuc = ngayBatDau;
            NgayKetThuc = ngayKetThuc;
            Status = status;
            Progress = progress;
            Priority = priority;
            IsShowBoard = isShowBoard;
            ActualFinish = actualFinish;
            FileNameResult = fileName;
            UrlFileNameResult = fileUrl;
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

        [StringLength(50)]
        public string Status { get; set; }

        public int Progress { get; set; }

        [StringLength(50)]
        public string Priority { get; set; }

        [StringLength(10)]
        public string IsShowBoard { get; set; }

        [StringLength(50)]
        public string ActualFinish { get; set; }

        [StringLength(250)]
        public string FileNameResult { get; set; }

        public string UrlFileNameResult { get; set; }

        [ForeignKey("MaKHKhamSK")]
        public virtual EHS_KE_HOACH_KHAM_SK EHS_KE_HOACH_KHAM_SK { get; set; }

        [ForeignKey("MaEvent")]
        public virtual EVENT_SHEDULE_PARENT EVENT_SHEDULE_PARENT { get; set; }
    }
}
