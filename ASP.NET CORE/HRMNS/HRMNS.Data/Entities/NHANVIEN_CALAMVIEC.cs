using HRMNS.Data.Interfaces;
using HRMS.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HRMNS.Data.Entities
{
    [Table("NHANVIEN_CALAMVIEC")]
    public class NHANVIEN_CALAMVIEC : DomainEntity<int>, IDateTracking
    {
        public NHANVIEN_CALAMVIEC()
        {

        }

        public NHANVIEN_CALAMVIEC(string maNV,string caLamviec,string batdauca,string ketthucca,string approved,string calv_db)
        {
            MaNV = maNV;
            Danhmuc_CaLviec = caLamviec;
            BatDau_TheoCa = batdauca;
            KetThuc_TheoCa = ketthucca;
            Approved = approved;
            CaLV_DB = calv_db;
        }

        [StringLength(50)]
        public string MaNV { get; set; }

         [StringLength(50)]
        public string Danhmuc_CaLviec { get; set; }

        [StringLength(50)]
        public string BatDau_TheoCa { get; set; }

        [StringLength(50)]
        public string KetThuc_TheoCa { get; set; }

        [StringLength(50)]
        public string Status { get; set; }

        [StringLength(5)]
        public string Approved { get; set; }

        [StringLength(50)]
        public string DateCreated { get; set; }

        [StringLength(50)]
        public string DateModified { get; set; }

        [StringLength(50)]
        public string UserCreated { get; set; }

        [StringLength(50)]
        public string UserModified { get; set; }

        [StringLength(50)]
        public string CaLV_DB 
        {
            get;set;
        }

        [ForeignKey("Danhmuc_CaLviec")]
        public virtual DM_CA_LVIEC DM_CA_LVIEC { get; set; }

        [ForeignKey("MaNV")]
        public virtual HR_NHANVIEN HR_NHANVIEN { get; set; }
    }
}
