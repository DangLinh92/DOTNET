using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HRMNS.Application.ViewModels.EHS
{
    public class EhsKeHoachQuanTracViewModel
    {
        public EhsKeHoachQuanTracViewModel()
        {
            EHS_NGAY_THUC_HIEN_CHITIET_QUANTRAC = new HashSet<EhsNgayThucHienChiTietQuanTrac>();
        }
        public int Id { get; set; }
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
        public EhsDMKeHoachViewModel EHS_DM_KEHOACH { get; set; }

        public ICollection<EhsNgayThucHienChiTietQuanTrac> EHS_NGAY_THUC_HIEN_CHITIET_QUANTRAC { get; set; }
    }
}
