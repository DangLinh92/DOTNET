using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HRMNS.Application.ViewModels.Time_Attendance
{
    public class DMCalamviecViewModel
    {
        public string Id { get; set; }

        [StringLength(100)]
        public string TenCaLamViec { get; set; }

        [StringLength(50)]
        public string MaTruSo { get; set; }

        [StringLength(50)]
        public string DateCreated { get; set; }

        [StringLength(50)]
        public string DateModified { get; set; }

        [StringLength(50)]
        public string UserCreated { get; set; }

        [StringLength(50)]
        public string UserModified { get; set; }

        public virtual ICollection<CaLamViecViewModel> CA_LVIEC { get; set; }

        public TruSoLamViecViewModel TRU_SO_LVIEC { get; set; }

        public ICollection<NhanVien_CalamViecViewModel> NHANVIEN_CALAMVIEC { get; set; }

        public ICollection<SettingTimeDiMuonVeSomViewModel> SETTING_TIME_DIMUON_VESOM { get; set; }
    }
}
