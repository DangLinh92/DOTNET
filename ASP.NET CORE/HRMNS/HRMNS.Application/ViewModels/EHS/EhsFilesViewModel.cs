using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HRMNS.Application.ViewModels.EHS
{
    public class EhsFilesViewModel
    {
        public int Id { get; set; }
        public string MaNgayChiTiet { get; set; }

        [StringLength(250)]
        public string FileName { get; set; }

        public string UrlFile { get; set; }

        [StringLength(150)]
        public string DanhMuc { get; set; }

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
