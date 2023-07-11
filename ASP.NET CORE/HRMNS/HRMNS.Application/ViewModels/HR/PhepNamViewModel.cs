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

        // Số phép quy định
        public float SoPhepNam { get; set; }

        public DateTime? ThangBatDauDocHai { get; set; }
        public DateTime? ThangKetThucDocHai { get; set; }
        public float SoPhepDocHai { get; set; }

        public float SoPhepCongThem { get; set; }
        public float SoPhepDaUng { get; set; }

        // số phép được hưởng

        public float SoPhepDuocHuong { get; set; }

        // so phép tồn năm
        public float SoPhepConLai { get; set; }

        public float NghiThang_1 { get; set; }
        public float NghiThang_2 { get; set; }
        public float NghiThang_3 { get; set; }
        public float NghiThang_4 { get; set; }
        public float NghiThang_5 { get; set; }
        public float NghiThang_6 { get; set; }
        public float NghiThang_7 { get; set; }
        public float NghiThang_8 { get; set; }
        public float NghiThang_9 { get; set; }
        public float NghiThang_10 { get; set; }
        public float NghiThang_11 { get; set; }
        public float NghiThang_12 { get; set; }
        public float TongNgayNghi { get; set; }
        public float SoPhepKhongDuocSuDung { get; set; }
        public float SoPhepTonThang { get; set; }

        public float SoPhepThanhToanNghiViec { get; set; }

        public float MucThanhToan { get; set; }

        public int Year { get; set; }

        public decimal SoTienChiTra { get; set; }

        public string ThoiGianChiTra { get; set; }

        [StringLength(50)]
        public string DateCreated { get; set; }
        [StringLength(50)]
        public string DateModified { get; set; }
        [StringLength(50)]
        public string UserCreated { get; set; }
        [StringLength(50)]
        public string UserModified { get; set; }

        public float SoPhepTonNam { get; set; }

        [ForeignKey("MaNhanVien")]
        public virtual HR_NHANVIEN HR_NHANVIEN { get; set; }
    }
}
