using HRMNS.Data.Interfaces;
using HRMS.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HRMNS.Data.Entities
{
    [Table("EHS_LUATDINH_KEHOACH")]
    public class EHS_LUATDINH_KEHOACH : DomainEntity<int>, IDateTracking
    {
        public EHS_LUATDINH_KEHOACH()
        {

        }

        public EHS_LUATDINH_KEHOACH(int id,string noidungluat,Guid makehoach)
        {
            Id = id;
            NoiDungLuatDinh = noidungluat;
            MaKeHoach = makehoach;
        }

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

        [ForeignKey("MaKeHoach")]
        public virtual EHS_DM_KEHOACH EHS_DM_KEHOACH { get; set; }
    }
}
