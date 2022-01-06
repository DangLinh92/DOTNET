﻿using HRMS.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HRMNS.Data.Entities
{
    [Table("HR_TINHTRANGHOSO")]
    public class HR_TINHTRANGHOSO : DomainEntity<int>
    {
        [StringLength(50)]
        public string MaNV { get; set; }

        [StringLength(50)]
        public string SoYeuLyLich { get; set; }

        [StringLength(50)]
        public string CMTND { get; set; }

        [StringLength(50)]
        public string SoHoKhau { get; set; }

        [StringLength(50)]
        public string GiayKhaiSinh { get; set; }

        [StringLength(50)]
        public string BangTotNghiep { get; set; }

        [StringLength(50)]
        public string XacNhanDanSu { get; set; }

        [StringLength(50)]
        public string AnhThe { get; set; }

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

        [ForeignKey("MaNV")]
        public virtual HR_NHANVIEN HR_NHANVIEN { get; set; }
    }
}
