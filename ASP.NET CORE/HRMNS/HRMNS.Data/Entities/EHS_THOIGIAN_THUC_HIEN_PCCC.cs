using HRMNS.Data.Interfaces;
using HRMS.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HRMNS.Data.Entities
{
    [Table("EHS_THOIGIAN_THUC_HIEN_PCCC")]
    public class EHS_THOIGIAN_THUC_HIEN_PCCC : DomainEntity<int>, IDateTracking
    {
        public EHS_THOIGIAN_THUC_HIEN_PCCC()
        {

        }

        public EHS_THOIGIAN_THUC_HIEN_PCCC(int id,Guid maEvent, Guid maKHPCCC, string noidung, string ngayBatDau, string ngayKetThuc, 
            string status, int progress, string priority, string isShowBoard,
            string actualFinish, string fileName, string fileUrl,string ketqua,string doisach)
        {
            Id = id;
            MaEvent = maEvent;
            MaKH_PCCC = maKHPCCC;
            NoiDung = noidung;
            NgayKetThuc = ngayBatDau;
            NgayKetThuc = ngayKetThuc;

            Status = status;
            Progress = progress;
            Priority = priority;
            IsShowBoard = isShowBoard;
            FileNameResult = fileName;
            UrlFileNameResult = fileUrl;
            ActualFinish = actualFinish;
            KetQua = ketqua;
            DoiSachCaiTien = doisach;
        }

        public Guid MaEvent { get; set; }

        public Guid MaKH_PCCC { get; set; }

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

        [StringLength(50)]
        public string KetQua { get; set; }

        [StringLength(500)]
        public string DoiSachCaiTien { get; set; }

        [ForeignKey("MaKH_PCCC")]
        public virtual EHS_KEHOACH_PCCC EHS_KEHOACH_PCCC { get; set; }

        [ForeignKey("MaEvent")]
        public virtual EVENT_SHEDULE_PARENT EVENT_SHEDULE_PARENT { get; set; }
    }
}
