using HRMNS.Data.EF.Extensions;
using HRMNS.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HRMNS.Application.ViewModels.Time_Attendance
{
    public class ChamCongLogViewModel
    {
        public long Id { get; set; }

        [StringLength(50)]
        public string Ngay_ChamCong { get; set; }

        [StringLength(50)]
        public string ID_NV { get; set; }

        [StringLength(96)]
        public string Ten_NV { get; set; }

        [StringLength(50)]
        public string FirstIn_Time { get; set; }

        [StringLength(50)]
        public string Last_Out_Time { get; set; }

        [StringLength(50)]
        public string FirstIn { get; set; }

        [StringLength(50)]
        public string LastOut { get; set; }

        [StringLength(50)]
        public string DateCreated { get; set; }

        [StringLength(50)]
        public string DateModified { get; set; }

        [StringLength(50)]
        public string UserCreated { get; set; }

        [StringLength(50)]
        public string UserModified { get; set; }

        public string Department { get; set; }

        public string Status
        {
            get
            {
                if (!string.IsNullOrEmpty(FirstIn.NullString()) && !string.IsNullOrEmpty(LastOut.NullString()))
                {
                    return ChamCongStatus.Normal.ToString();
                }
                return ChamCongStatus.Absence.ToString();
            }
        }

        public string Result
        {
            get
            {
                if (FirstIn != "IN" && LastOut != "OUT")
                {
                    return ChamCongStatus.NonWorking.ToString();
                }

                if (FirstIn != "IN" && LastOut == "OUT")
                {
                    return ChamCongStatus.Miss_In.ToString();
                }

                if (FirstIn == "IN" && LastOut != "OUT")
                {
                    return ChamCongStatus.Miss_Out.ToString();
                }

                return ChamCongStatus.InWorking.ToString();
            }
        }
    }
}
