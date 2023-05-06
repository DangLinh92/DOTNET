﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OPERATION_MNS.Application.ViewModels
{
    public class CTQSettingViewModel
    {
        public int Id { get; set; }

        [StringLength(50)]
        public string OperationID { get; set; }

        [StringLength(50)]
        public string OperationName { get; set; }

        public double LWL { get; set; }

        public double UWL { get; set; }

        [StringLength(50)]
        public string DateCreated { get; set; }

        [StringLength(50)]
        public string DateModified { get; set; }

        [StringLength(50)]
        public string UserCreated { get; set; }

        [StringLength(50)]
        public string UserModified { get; set; }
    }

    public class CTQSettingWLP2ViewModel
    {
        public int Id { get; set; }

        [StringLength(50)]
        public string SpeacialModel { get; set; }

        [StringLength(50)]
        public string OperationID { get; set; }

        [StringLength(50)]
        public string OperationName { get; set; }

        public float ThickNet { get; set; } // độ dày

        public double MinV { get; set; }

        public double MaxV { get; set; }

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
