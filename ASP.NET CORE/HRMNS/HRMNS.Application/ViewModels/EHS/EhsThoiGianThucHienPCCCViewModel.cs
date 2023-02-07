using HRMNS.Application.ViewModels.System;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HRMNS.Application.ViewModels.EHS
{
    public class EhsThoiGianThucHienPCCCViewModel
    {
        public int Id { get; set; }

        public Guid MaEvent { get; set; }

        public Guid MaKH_PCCC { get; set; }

        [StringLength(1000)]
        public string NoiDung { get; set; }

        [StringLength(50)]
        public string NgayBatDau { get; set; }

        [StringLength(50)]
        public string NgayKetThuc { get; set; }

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
        public string DateCreated { get; set; }

        [StringLength(50)]
        public string DateModified { get; set; }

        [StringLength(50)]
        public string UserCreated { get; set; }

        [StringLength(50)]
        public string UserModified { get; set; }

        public Ehs_KeHoach_PCCCViewModel EHS_KEHOACH_PCCC { get; set; }

        public EventScheduleParentViewModel EVENT_SHEDULE_PARENT { get; set; }
    }
}
