using HRMNS.Data.Interfaces;
using HRMS.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HRMNS.Data.Entities
{
    [Table("EHS_FILES")]
    public class EHS_FILES : DomainEntity<int>, IDateTracking
    {
        public EHS_FILES()
        {
           
        }

        public EHS_FILES(int id, string fileName,string url, string maNgayChiTiet,string danhMuc)
        {
            Id = id;
            FileName = fileName;
            UrlFile = url;
            MaNgayChiTiet = maNgayChiTiet;
            DanhMuc = danhMuc;
        }

        [StringLength(500)]
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
