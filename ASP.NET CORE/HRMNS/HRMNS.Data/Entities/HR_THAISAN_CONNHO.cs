using HRMNS.Data.Interfaces;
using HRMS.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HRMNS.Data.Entities
{
    [Table("HR_THAISAN_CONNHO")]
    public class HR_THAISAN_CONNHO : DomainEntity<int>, IDateTracking
    {
        public HR_THAISAN_CONNHO()
        {

        }
        public HR_THAISAN_CONNHO(int id,string manv,string chedo,string from,string to)
        {
            Id = id;
            MaNV = manv;
            CheDoThaiSan = chedo;
            FromDate = from;
            ToDate = to;
        }

        [StringLength(50)]
        public string MaNV { get; set; }

        [StringLength(50)]
        public string CheDoThaiSan { get; set; } // thai san, con nho,mang bau

        [StringLength(50)]
        public string FromDate { get; set; }

        [StringLength(50)]
        public string ToDate { get; set; }

        [StringLength(50)]
        public string DateCreated { get; set; }

        [StringLength(50)]
        public string DateModified { get; set; }

        [StringLength(50)]
        public string UserCreated { get; set; }

        [StringLength(50)]
        public string UserModified { get; set; }

        [ForeignKey("MaNV")]
        public virtual HR_NHANVIEN HR_NHANVIEN { get; set; }
    }
}
