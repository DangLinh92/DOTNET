using HRMNS.Data.Interfaces;
using HRMS.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HRMNS.Data.Entities
{
    [Table("EHS_KEHOACH_QUANTRAC")]
    public class EHS_KEHOACH_QUANTRAC : DomainEntity<int>, IDateTracking
    {
        public EHS_KEHOACH_QUANTRAC()
        {
            EHS_NGAY_THUC_HIEN_CHITIET_QUANTRAC = new HashSet<EHS_NGAY_THUC_HIEN_CHITIET_QUANTRAC>();
        }

        public EHS_KEHOACH_QUANTRAC(int stt, Guid maDMKeHoach,string demuc,string luatDinhLienQuan,string noiDung,string chuKyThucHien,
            string year,bool month1, bool month2, bool month3, bool month4, bool month5, bool month6, bool month7, bool month8, bool month9, bool month10, bool month11, bool month12,
            double costMonth_1, double costMonth_2, double costMonth_3, double costMonth_4, double costMonth_5, double costMonth_6, double costMonth_7, double costMonth_8,
            double costMonth_9, double costMonth_10, double costMonth_11, double costMonth_12,
            string khuVucLayMau,string nhaThau,string nguoiPhuTrach, double tienDoHoanThanh,
            string layMau_Month_1, string layMau_Month_2, string layMau_Month_3, string layMau_Month_4, string layMau_Month_5,
            string layMau_Month_6, string layMau_Month_7, string layMau_Month_8, string layMau_Month_9,
             string layMau_Month_10, string layMau_Month_11, string layMau_Month_12)
        {
            STT = stt;
            MaDMKeHoach = maDMKeHoach;
            Demuc = demuc;
            LuatDinhLienQuan = luatDinhLienQuan;
            NoiDung = noiDung;
            ChuKyThucHien = chuKyThucHien;
            Year = year;
            Month_1 = month1;
            Month_2 = month2;
            Month_3 = month3;
            Month_4 = month4;
            Month_5 = month5;
            Month_6 = month6;
            Month_7 = month7;
            Month_8 = month8;
            Month_9 = month9;
            Month_10 = month10;
            Month_11 = month11;
            Month_12 = month12;

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

            KhuVucLayMau = khuVucLayMau;
            NhaThau = nhaThau;
            NguoiPhuTrach = nguoiPhuTrach;
            TienDoHoanThanh = tienDoHoanThanh;

            LayMau_Month_1 = layMau_Month_1;
            LayMau_Month_2 = layMau_Month_2;
            LayMau_Month_3 = layMau_Month_3;
            LayMau_Month_4 = layMau_Month_4;
            LayMau_Month_5 = layMau_Month_5;
            LayMau_Month_6 = layMau_Month_6;
            LayMau_Month_7 = layMau_Month_7;
            LayMau_Month_8 = layMau_Month_8;
            LayMau_Month_9 = layMau_Month_9;
            LayMau_Month_10 = layMau_Month_10;
            LayMau_Month_11 = layMau_Month_11;
            LayMau_Month_12 = layMau_Month_12;
        }

        public int STT { get; set; }

        public Guid MaDMKeHoach { get; set; }

        [StringLength(500)]
        public string Demuc { get; set; }

        [StringLength(500)]
        public string LuatDinhLienQuan { get; set; }

        [StringLength(1000)]
        public string NoiDung { get; set; }

        [StringLength(100)]
        public string ChuKyThucHien { get; set; }

        [StringLength(4)]
        public string Year { get; set; }

        public bool Month_1 { get; set; }
        public bool Month_2 { get; set; }
        public bool Month_3 { get; set; }
        public bool Month_4 { get; set; }
        public bool Month_5 { get; set; }
        public bool Month_6 { get; set; }
        public bool Month_7 { get; set; }
        public bool Month_8 { get; set; }
        public bool Month_9 { get; set; }
        public bool Month_10 { get; set; }
        public bool Month_11 { get; set; }
        public bool Month_12 { get; set; }

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

        [StringLength(200)]
        public string LayMau_Month_1 { get; set; }

        [StringLength(200)]
        public string LayMau_Month_2 { get; set; }

        [StringLength(200)]
        public string LayMau_Month_3 { get; set; }

        [StringLength(200)]
        public string LayMau_Month_4 { get; set; }

        [StringLength(200)]
        public string LayMau_Month_5 { get; set; }

        [StringLength(200)]
        public string LayMau_Month_6 { get; set; }

        [StringLength(200)]
        public string LayMau_Month_7 { get; set; }

        [StringLength(200)]
        public string LayMau_Month_8 { get; set; }

        [StringLength(200)]
        public string LayMau_Month_9 { get; set; }

        [StringLength(200)]
        public string LayMau_Month_10 { get; set; }

        [StringLength(200)]
        public string LayMau_Month_11 { get; set; }

        [StringLength(200)]
        public string LayMau_Month_12 { get; set; }

        [StringLength(250)]
        public string KhuVucLayMau { get; set; }

        [StringLength(100)]
        public string NhaThau { get; set; }

        [StringLength(50)]
        public string NguoiPhuTrach { get; set; }

        // % hoan thành
        public double TienDoHoanThanh { get; set; }

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

        public virtual ICollection<EHS_NGAY_THUC_HIEN_CHITIET_QUANTRAC> EHS_NGAY_THUC_HIEN_CHITIET_QUANTRAC { get; set; }
    }
}
