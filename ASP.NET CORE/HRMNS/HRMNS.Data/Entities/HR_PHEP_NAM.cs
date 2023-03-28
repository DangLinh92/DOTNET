using HRMNS.Data.Interfaces;
using HRMS.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HRMNS.Data.Entities
{
    [Table("HR_PHEP_NAM")]
    public class HR_PHEP_NAM : DomainEntity<int>, IDateTracking
    {
        public HR_PHEP_NAM()
        {

        }

        public HR_PHEP_NAM(
            int id, string maNV, 
            float soPhepNam, float soPhepConLai,
            int year, decimal sotientra,
            DateTime? thoigiantra,
            DateTime? thangBatDauDocHai,
            DateTime? thangKetThucDocHai,
            float soPhepDocHai, 
            float soPhepCongThem, 
            float soPhepDaUng, 
            float soPhepDuongHuong,
            float nghiThang_1,
            float nghiThang_2,
            float nghiThang_3,
            float nghiThang_4,
            float nghiThang_5,
            float nghiThang_6,
            float nghiThang_7,
            float nghiThang_8,
            float nghiThang_9,
            float nghiThang_10,
            float nghiThang_11,
            float nghiThang_12,
            float soPhepKhongDuocSuDung,
            float soPhepTonThang,
            float soPhepThanhToanNghiViec,
            float mucThanhToan,
            float sophepTonNam)
        {
            Id = id;
            MaNhanVien = maNV;
            SoPhepNam = soPhepNam;
            SoPhepConLai = soPhepConLai;
            Year = year;
            SoTienChiTra = sotientra;
            ThoiGianChiTra = thoigiantra;

            ThangBatDauDocHai = thangBatDauDocHai;
            ThangKetThucDocHai = thangKetThucDocHai;
            SoPhepDocHai = soPhepDocHai;
            SoPhepCongThem = soPhepCongThem;
            SoPhepDaUng = soPhepDaUng;
            SoPhepDuocHuong = soPhepDuongHuong;
            NghiThang_1 = nghiThang_1;
            NghiThang_2 = nghiThang_2;
            NghiThang_3 = nghiThang_3;
            NghiThang_4 = nghiThang_4;
            NghiThang_5 = nghiThang_5;
            NghiThang_6 = nghiThang_6;
            NghiThang_7 = nghiThang_7;
            NghiThang_8 = nghiThang_8;
            NghiThang_9 = nghiThang_9;
            NghiThang_10 = nghiThang_10;
            NghiThang_11 = nghiThang_11;
            NghiThang_12 = nghiThang_12;
            SoPhepKhongDuocSuDung = soPhepKhongDuocSuDung;
            SoPhepTonThang = soPhepTonThang;
            SoPhepThanhToanNghiViec = soPhepThanhToanNghiViec;
            MucThanhToan = mucThanhToan;
            SoPhepTonNam = sophepTonNam;
        }

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

        public float SoPhepTonNam { get; set; }

        public float SoPhepTonThang { get; set; }

        public float SoPhepThanhToanNghiViec { get; set; }

        public float MucThanhToan { get; set; }

        public int Year { get; set; }

        public decimal SoTienChiTra { get; set; }

        public DateTime? ThoiGianChiTra { get; set; }

        [StringLength(50)]
        public string DateCreated { get; set; }
        [StringLength(50)]
        public string DateModified { get; set; }
        [StringLength(50)]
        public string UserCreated { get; set; }
        [StringLength(50)]
        public string UserModified { get; set; }

        [ForeignKey("MaNhanVien")]
        public virtual HR_NHANVIEN HR_NHANVIEN { get; set; }
    }
}
