﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HRMNS.Application.ViewModels.EHS
{
    public class EhsLuatDinhKeHoachViewModel
    {
        public int Id { get; set; }

        [StringLength(1000)]
        public string NoiDungLuatDinh { get; set; }

        public Guid MaKeHoach { get; set; }

        [StringLength(50)]
        public string DateCreated { get; set; }

        [StringLength(50)]
        public string DateModified { get; set; }

        [StringLength(50)]
        public string UserCreated { get; set; }

        [StringLength(50)]
        public string UserModified { get; set; }

        public EhsDMKeHoachViewModel EHS_DM_KEHOACH { get; set; }
    }
}
