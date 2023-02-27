using HRMNS.Application.ViewModels.System;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HRMNS.Application.ViewModels.EHS
{
    public class EhsNgayThucHienChiTietKhamSKViewModel
    {
        public int Id { get; set; }
        public Guid MaEvent { get; set; }

        public Guid MaKHKhamSK { get; set; }

        [StringLength(1000)]
        public string NoiDung { get; set; }

        [StringLength(50)]
        public string NgayBatDau { get; set; }

        public DateTime NgayBatDauEx
        {
            get => DateTime.Parse(NgayBatDau);
            set => NgayBatDau = value.ToString("yyyy-MM-dd");
        }

        public DateTime NgayKetThucEx
        {
            get => DateTime.Parse(NgayKetThuc);
            set => NgayKetThuc = value.ToString("yyyy-MM-dd");
        }


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


        public EhsKeHoachKhamSKViewModel EHS_KE_HOACH_KHAM_SK { get; set; }

        public EventScheduleParentViewModel EVENT_SHEDULE_PARENT { get; set; }
    }
}
