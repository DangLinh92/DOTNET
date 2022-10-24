using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HRMNS.Application.ViewModels.HR
{
    public class NhanVienCheDoDBViewModel
    {
        public NhanVienCheDoDBViewModel()
        {

        }

        public NhanVienCheDoDBViewModel(string maNV, string chedoDB, string note)
        {
            MaNhanVien = maNV;
            CheDoDB = chedoDB;
            Note = note;
        }

        [StringLength(50)]
        public string MaNhanVien { get; set; }
        [StringLength(50)]
        public string CheDoDB { get; set; }
        [StringLength(250)]
        public string Note { get; set; }

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
