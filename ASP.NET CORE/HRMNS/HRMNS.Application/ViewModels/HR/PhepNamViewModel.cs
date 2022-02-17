using HRMNS.Data.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text;

namespace HRMNS.Application.ViewModels.HR
{
    public class PhepNamViewModel
    {
        public int Id { get; set; }

        [StringLength(50)]
        public string MaNhanVien { get; set; }
        public float SoPhepNam { get; set; }
        public float SoPhepConLai { get; set; }
        public int Year { get; set; }
        [StringLength(50)]
        public string DateCreated { get; set; }
        [StringLength(50)]
        public string DateModified { get; set; }
        [StringLength(50)]
        public string UserCreated { get; set; }
        [StringLength(50)]
        public string UserModified { get; set; }

        [ForeignKey("MaNhanVien")]
        [JsonIgnore]
        [IgnoreDataMember]
        public HR_NHANVIEN HR_NHANVIEN { get; set; }
    }
}
