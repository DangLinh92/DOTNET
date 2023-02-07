using HRMNS.Data.Interfaces;
using HRMS.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HRMNS.Data.Entities
{
    [Table("EHS_KE_HOACH_KHAM_SK")]
    public class EHS_KE_HOACH_KHAM_SK : DomainEntity<Guid>, IDateTracking
    {
        public EHS_KE_HOACH_KHAM_SK()
        {
            EHS_NHANVIEN_KHAM_SK = new HashSet<EHS_NHANVIEN_KHAM_SK>();
            EHS_NGAY_THUC_HIEN_CHITIET_KHAM_SK = new HashSet<EHS_NGAY_THUC_HIEN_CHITIET_KHAM_SK>();
        }

        public EHS_KE_HOACH_KHAM_SK(string noidung,string luatDinh,string chuky,string year,
            double costMonth_1, double costMonth_2, double costMonth_3, double costMonth_4, double costMonth_5, double costMonth_6, double costMonth_7,
            double costMonth_8, double costMonth_9, double costMonth_10, double costMonth_11, double costMonth_12,Guid maDKKeHoach,string nhathau,string nguoiPhuTrach)
        {
            NoiDung = noidung;
            LuatDinhLienQuan = luatDinh;
            ChuKyThucHien = chuky;
            Year = year;
            CostMonth_1 = costMonth_1;
            CostMonth_2 = costMonth_2;
            CostMonth_3 = costMonth_3;
            CostMonth_4 = costMonth_4;
            CostMonth_5 = costMonth_5;
            CostMonth_6 = costMonth_6;
            CostMonth_7 = costMonth_7;
            CostMonth_8 = costMonth_8;
            CostMonth_9 = costMonth_9;
            CostMonth_10 = costMonth_10;
            CostMonth_11 = costMonth_11;
            CostMonth_12 = costMonth_12;
            MaDMKeHoach = maDKKeHoach;
            NhaThau = nhathau;
            NguoiPhuTrach = nguoiPhuTrach;
        }

        public Guid MaDMKeHoach { get; set; }

        [StringLength(500)]
        public string LuatDinhLienQuan { get; set; }

        [StringLength(500)]
        public string NoiDung { get; set; }

        [StringLength(100)]
        public string ChuKyThucHien { get; set; }

        [StringLength(4)]
        public string Year { get; set; }

        [StringLength(100)]
        public string NhaThau { get; set; }

        [StringLength(50)]
        public string NguoiPhuTrach { get; set; }

        public double CostMonth_1 { get; set; }
        public double CostMonth_2 { get; set; }
        public double CostMonth_3 { get; set; }
        public double CostMonth_4 { get; set; }
        public double CostMonth_5 { get; set; }
        public double CostMonth_6 { get; set; }
        public double CostMonth_7 { get; set; }
        public double CostMonth_8 { get; set; }
        public double CostMonth_9 { get; set; }
        public double CostMonth_10 { get; set; }
        public double CostMonth_11 { get; set; }
        public double CostMonth_12 { get; set; }

        [StringLength(50)]
        public string DateCreated { get; set; }

        [StringLength(50)]
        public string DateModified { get; set; }

        [StringLength(50)]
        public string UserCreated { get; set; }

        [StringLength(50)]
        public string UserModified { get; set; }

        public virtual ICollection<EHS_NHANVIEN_KHAM_SK> EHS_NHANVIEN_KHAM_SK { get; set; }
        public virtual ICollection<EHS_NGAY_THUC_HIEN_CHITIET_KHAM_SK> EHS_NGAY_THUC_HIEN_CHITIET_KHAM_SK { get; set; }

        [ForeignKey("MaDMKeHoach")]
        public virtual EHS_DM_KEHOACH EHS_DM_KEHOACH { get; set; }
    }
}
