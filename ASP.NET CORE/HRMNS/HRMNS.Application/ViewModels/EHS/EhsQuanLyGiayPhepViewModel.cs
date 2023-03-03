using HRMNS.Application.ViewModels.System;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HRMNS.Application.ViewModels.EHS
{
    public class EhsQuanLyGiayPhepViewModel
    {
        public int Id { get; set; }

        [StringLength(250)]
        public string Demuc { get; set; }

        [StringLength(1000)]
        public string NoiDung { get; set; }

        [StringLength(1000)]
        public string LuatDinhLienQuan { get; set; }

        [StringLength(1000)]
        public string LyDoThucHien { get; set; }

        [StringLength(250)]
        public string TienDo { get; set; }

        [StringLength(50)]
        public string ThoiGianThucHien { get; set; }

        private DateTime? tg;
        public DateTime? ThoiGian
        {
            get
            {
                if (!string.IsNullOrEmpty(ThoiGianThucHien))
                {
                    return DateTime.Parse(ThoiGianThucHien);
                }
                return tg;
            }
            set 
            {    tg = value;
                ThoiGianThucHien = tg?.ToString("yyyy-MM-dd");
            }
        }

        public int SoNgayBaoTruoc { get; set; }

        public Guid MaEvent { get; set; }

        public EventScheduleParentViewModel EVENT_SHEDULE_PARENT { get; set; }

        [StringLength(250)]
        public string KetQua { get; set; }

        [StringLength(50)]
        public string NguoiThucHien { get; set; }

        [StringLength(50)]
        public string Status { get; set; }

        [StringLength(50)]
        public string DateCreated { get; set; }

        [StringLength(50)]
        public string DateModified { get; set; }

        [StringLength(50)]
        public string UserCreated { get; set; }

        [StringLength(50)]
        public string UserModified { get; set; }
    }
}
