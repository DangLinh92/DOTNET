using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HRMNS.Application.ViewModels.Time_Attendance
{
    public class SettingTimeDiMuonVeSomViewModel
    {
        public int Id { get; set; }

        [StringLength(50)]
        public string Danhmuc_CaLviec { get; set; }

        [StringLength(50)]
        public string Time_LateCome { get; set; }

        [StringLength(50)]
        public string Time_EarlyLeave { get; set; }

        [StringLength(50)]
        public string DateCreated { get; set; }

        [StringLength(50)]
        public string DateModified { get; set; }

        [StringLength(50)]
        public string UserCreated { get; set; }

        [StringLength(50)]
        public string UserModified { get; set; }

        public DMCalamviecViewModel DM_CA_LVIEC { get; set; }
    }
}
