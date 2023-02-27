using HRMNS.Data.Interfaces;
using HRMS.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HRMNS.Data.Entities
{
    [Table("EHS_NGAY_THUC_HIEN_CHITIET_QUANTRAC")]
    public class EHS_NGAY_THUC_HIEN_CHITIET_QUANTRAC : DomainEntity<int>, IDateTracking
    {
        public EHS_NGAY_THUC_HIEN_CHITIET_QUANTRAC()
        {

        }

        public EHS_NGAY_THUC_HIEN_CHITIET_QUANTRAC(int id,Guid maEvent,int maKHQuanTrac,string noidung,string ngayBatDau,string ngayKetThuc,
            string status, int progress, string priority, string isShowBoard,string actualFinish, string fileName, string fileUrl)
        {
            Id = id;
            MaEvent = maEvent;
            MaKHQuanTrac = maKHQuanTrac;
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

        public int MaKHQuanTrac { get; set; }

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

        [ForeignKey("MaKHQuanTrac")]
        public virtual EHS_KEHOACH_QUANTRAC EHS_KEHOACH_QUANTRAC { get; set; }

        [ForeignKey("MaEvent")]
        public virtual EVENT_SHEDULE_PARENT EVENT_SHEDULE_PARENT { get; set; }
    }
}
