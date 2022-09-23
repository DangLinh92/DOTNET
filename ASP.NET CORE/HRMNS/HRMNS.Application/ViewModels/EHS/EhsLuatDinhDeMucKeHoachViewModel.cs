using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HRMNS.Application.ViewModels.EHS
{
    public class EhsLuatDinhDeMucKeHoachViewModel
    {
        public int Id { get; set; }

        [StringLength(1000)]
        public string LuatDinhLienQuan { get; set; }

        public Guid MaDeMuc { get; set; }

        [StringLength(50)]
        public string DateCreated { get; set; }

        [StringLength(50)]
        public string DateModified { get; set; }

        [StringLength(50)]
        public string UserCreated { get; set; }

        [StringLength(50)]
        public string UserModified { get; set; }

        public EhsDeMucKeHoachViewModel EHS_DEMUC_KEHOACH { get; set; }
    }
}
