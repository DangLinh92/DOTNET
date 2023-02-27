using HRMNS.Data.Interfaces;
using HRMS.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HRMNS.Data.Entities
{
    [Table("EHS_KEHOACH_ANTOAN_BUCXA")]
    public class EHS_KEHOACH_ANTOAN_BUCXA : DomainEntity<Guid>, IDateTracking
    {
        public EHS_KEHOACH_ANTOAN_BUCXA()
        {
            EHS_THOIGIAN_THUC_HIEN_ANTOAN_BUCXA = new HashSet<EHS_THOIGIAN_THUC_HIEN_ANTOAN_BUCXA>();
        }

        public EHS_KEHOACH_ANTOAN_BUCXA(Guid id,Guid maKh,int stt,string noidung,string chuky,string year,string thoigiandaotao,string nguoiphutrach,string nhathau,
            double cost1, double cost2, double cost3, double cost4, double cost5, double cost6, double cost7, double cost8, double cost9, double cost10, double cost11, double cost12,string hangmuc,
            string thoiGianCapL1,string thoiGianCapLai_L1,string thoiGianCapLai_L2,string thoiGianCapLai_L3,string yeuCau,string yeucauVBLP,string maHieu,string thoiGianCapLai_L4)
        {
            Id = id;
            MaDMKeHoach = maKh;
            STT = stt;
            NoiDung = noidung;
            ChuKyThucHien = chuky;
            Year = year;
            ThoiGianDaoTao = thoigiandaotao;
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
            HangMuc = hangmuc;
            ThoiGianCapL1 = thoiGianCapL1;
            ThoiGianCapLai_L1 = thoiGianCapLai_L1;
            ThoiGianCapLai_L2 = thoiGianCapLai_L2;
            ThoiGianCapLai_L3 = thoiGianCapLai_L3;
            YeuCau = yeuCau;
            QuyDinhVBPL = yeucauVBLP;
            MaHieu = maHieu;
            ThoiGianCapLai_L4 = thoiGianCapLai_L4;
        }

        public Guid MaDMKeHoach { get; set; }

        public int STT { get; set; }

        [StringLength(500)]
        public string HangMuc { get; set; }

        [StringLength(1000)]
        public string NoiDung { get; set; }

        [StringLength(50)]
        public string ChuKyThucHien { get; set; }

        [StringLength(10)]
        public string Year { get; set; }

        [StringLength(250)]
        public string ThoiGianDaoTao { get; set; }

        [StringLength(250)]
        public string MaHieu { get; set; }

        [StringLength(50)]
        public string NguoiPhuTrach { get; set; }

        [StringLength(50)]
        public string NhaThau { get; set; }

        [StringLength(250)]
        public string ThoiGianCapL1 { get; set; }

        [StringLength(250)]
        public string ThoiGianCapLai_L1 { get; set; }

        [StringLength(250)]
        public string ThoiGianCapLai_L2 { get; set; }

        [StringLength(250)]
        public string ThoiGianCapLai_L3 { get; set; }

        [StringLength(250)]
        public string ThoiGianCapLai_L4 { get; set; }

        [StringLength(250)]
        public string YeuCau { get; set; }

        [StringLength(250)]
        public string QuyDinhVBPL { get; set; }

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

        [ForeignKey("MaDMKeHoach")]
        public virtual EHS_DM_KEHOACH EHS_DM_KEHOACH { get; set; }
        public virtual ICollection<EHS_THOIGIAN_THUC_HIEN_ANTOAN_BUCXA> EHS_THOIGIAN_THUC_HIEN_ANTOAN_BUCXA { get; set; }
    }
}
