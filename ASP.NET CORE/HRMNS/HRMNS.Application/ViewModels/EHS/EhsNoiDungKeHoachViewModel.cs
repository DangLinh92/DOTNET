using HRMNS.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HRMNS.Application.ViewModels.EHS
{
    public class EhsNoiDungKeHoachViewModel
    {
        public Guid Id { get; set; }
        public string Year { get; set; }

        public Guid MaNoiDung { get; set; }

        [StringLength(250)]
        public string NhaThau { get; set; }

        [StringLength(50)]
        public string ChuKy { get; set; } // chu ky thuc hien 1-M,1-Y

        [StringLength(50)]
        public string NgayThucHien { get; set; } // 2022-01-01

        [StringLength(50)]
        public string ThoiGian_ThucHien { get; set; } // 8h

        [StringLength(500)]
        public string ViTri { get; set; }

        public double SoLuong { get; set; }

        [StringLength(500)]
        public string YeuCau { get; set; }

        [StringLength(500)]
        public string Note { get; set; }

        [StringLength(50)]
        public string NgayKhaiBaoThietBi { get; set; }

        [StringLength(10)]
        public string ThoiGianThongBao { get; set; } // 1-W,1-M,1-Y,...

        public string ThoiGianConLai
        {
            get
            {
                if(DateTime.TryParse(NgayThucHien,out  DateTime dt))
                {
                    if(dt.Subtract(DateTime.Now).TotalDays > 0)
                    {
                        return Math.Round(dt.Subtract(DateTime.Now).TotalDays).ToString();
                    }
                }

                return "";
            }
        }

        [StringLength(50)]
        public string DateCreated { get; set; }

        [StringLength(50)]
        public string DateModified { get; set; }

        [StringLength(50)]
        public string UserCreated { get; set; }

        [StringLength(50)]
        public string UserModified { get; set; }

        [StringLength(150)]
        public string MaHieuMayKiemTra { get; set; }

        public double SoTien { get; set; }

        [StringLength(1000)]
        public string KetQua { get; set; }

        [StringLength(50)]
        public string Status { get; set; } // Active/ Inactive / Wait

        public double TienDoHoanThanh { get; set; } // 50%,100%

        [StringLength(150)]
        public string NguoiPhucTrach { get; set; }

        public EhsNoiDungViewModel EHS_NOIDUNG { get; set; }
    }
}
