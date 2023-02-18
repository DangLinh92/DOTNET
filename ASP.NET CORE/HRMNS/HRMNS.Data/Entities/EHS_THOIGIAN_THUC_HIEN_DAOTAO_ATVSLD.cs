using HRMNS.Data.Interfaces;
using HRMS.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HRMNS.Data.Entities
{
    [Table("EHS_THOIGIAN_THUC_HIEN_DAOTAO_ATVSLD")]
    public class EHS_THOIGIAN_THUC_HIEN_DAOTAO_ATVSLD : DomainEntity<int>, IDateTracking
    {
        public EHS_THOIGIAN_THUC_HIEN_DAOTAO_ATVSLD()
        {

        }

        public EHS_THOIGIAN_THUC_HIEN_DAOTAO_ATVSLD(Guid maEvent, Guid maKHDaoTaoATLD, string noidung, string ngayBatDau, string ngayKetThuc,
            string status, int progress, string priority, string isShowBoard,string actualFinish)
        {
            MaEvent = maEvent;
            MaKHDaoTaoATLD = maKHDaoTaoATLD;
            NoiDung = noidung;
            NgayKetThuc = ngayBatDau;
            NgayKetThuc = ngayKetThuc;
            Status = status;
            Progress = progress;
            Priority = priority;
            IsShowBoard = isShowBoard;
            ActualFinish = actualFinish;
        }

        public Guid MaEvent { get; set; }

        public Guid MaKHDaoTaoATLD { get; set; }

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

        [ForeignKey("MaKHDaoTaoATLD")]
        public virtual EHS_KEHOACH_DAOTAO_ANTOAN_VSLD EHS_KEHOACH_DAOTAO_ANTOAN_VSLD { get; set; }

        [ForeignKey("MaEvent")]
        public virtual EVENT_SHEDULE_PARENT EVENT_SHEDULE_PARENT { get; set; }
    }
}
