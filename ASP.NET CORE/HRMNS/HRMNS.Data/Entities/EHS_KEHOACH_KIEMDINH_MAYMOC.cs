using HRMNS.Data.Interfaces;
using HRMS.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HRMNS.Data.Entities
{
    [Table("EHS_KEHOACH_KIEMDINH_MAYMOC")]
    public class EHS_KEHOACH_KIEMDINH_MAYMOC : DomainEntity<Guid>, IDateTracking
    {
        public EHS_KEHOACH_KIEMDINH_MAYMOC()
        {
            EHS_THOIGIAN_THUC_HIEN_KIEMDINH_MM = new HashSet<EHS_THOIGIAN_THUC_HIEN_KIEMDINH_MM>();
        }

        public EHS_KEHOACH_KIEMDINH_MAYMOC(
            Guid maKh,int stt,string tenMayMoc, string chuky,
            int soLuongThietBi, string vitri,string nguoiphutrach,string nhathau,
            double cost1, double cost2, double cost3, double cost4, double cost5, 
            double cost6, double cost7, double cost8, double cost9, double cost10, double cost11, double cost12,
            string lanKiemDinhKeTiep, string lanKiemDinhKeTiep1, string lanKiemDinhKeTiep2, string lanKiemDinhKeTiep3,string year)
        {
            MaDMKeHoach = maKh;
            STT = stt;
            NguoiPhuTrach = nguoiphutrach;
            NhaThau = nhathau;
            CostMonth_1 = cost1;
            CostMonth_2 = cost2;
            CostMonth_3 = cost3;
            CostMonth_4 = cost4;
            CostMonth_5 = cost5;
            CostMonth_6 = cost6;
            CostMonth_7 = cost7;
            CostMonth_8 = cost8;
            CostMonth_9 = cost9;
            CostMonth_10 = cost10;
            CostMonth_11 = cost11;
            CostMonth_12 = cost12;

            TenMayMoc = tenMayMoc;
            ChuKyKiemDinh = chuky;
            SoLuongThietBi = soLuongThietBi;
            ViTri = vitri;
            LanKiemDinhKeTiep = lanKiemDinhKeTiep;
            LanKiemDinhKeTiep1 = lanKiemDinhKeTiep1;
            LanKiemDinhKeTiep2 = lanKiemDinhKeTiep2;
            LanKiemDinhKeTiep3 = lanKiemDinhKeTiep3;
            Year = year;
        }

        public Guid MaDMKeHoach { get; set; }

        public int STT { get; set; }

        [StringLength(500)]
        public string TenMayMoc { get; set; }

        [StringLength(50)]
        public string ChuKyKiemDinh { get; set; }

        public int SoLuongThietBi { get; set; }

        [StringLength(250)]
        public string ViTri { get; set; }

        [StringLength(50)]
        public string NguoiPhuTrach { get; set; }

        [StringLength(50)]
        public string NhaThau { get; set; }

        [StringLength(50)]
        public string LanKiemDinhKeTiep { get; set; }

        [StringLength(50)]
        public string LanKiemDinhKeTiep1 { get; set; }

        [StringLength(50)]
        public string LanKiemDinhKeTiep2 { get; set; }

        [StringLength(50)]
        public string LanKiemDinhKeTiep3 { get; set; }

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

        [StringLength(10)]
        public string Year { get; set; }

        [StringLength(50)]
        public string DateCreated { get; set; }

        [StringLength(50)]
        public string DateModified { get; set; }

        [StringLength(50)]
        public string UserCreated { get; set; }

        [StringLength(50)]
        public string UserModified { get; set; }

        [ForeignKey("MaDMKeHoach")]
        public virtual EHS_DM_KEHOACH EHS_DM_KEHOACH { get; set; }
        public virtual ICollection<EHS_THOIGIAN_THUC_HIEN_KIEMDINH_MM> EHS_THOIGIAN_THUC_HIEN_KIEMDINH_MM { get; set; }
    }
}
