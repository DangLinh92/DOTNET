﻿using HRMNS.Data.Interfaces;
using HRMS.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HRMNS.Data.Entities
{
    [Table("EHS_NOIDUNG_KEHOACH")]
    public class EHS_NOIDUNG_KEHOACH : DomainEntity<Guid>, IDateTracking
    {
        public EHS_NOIDUNG_KEHOACH()
        {
        }

        public EHS_NOIDUNG_KEHOACH(Guid id,string year,Guid maNoiDung,string nhathau,string chuky,string yeucau,string note,
            string ngaythuchien,string thoigianthuchien,string vitri,double soluong,string ngaykhaibaoTB,string thoigianthongbao)
        {
            Id = id;
            Year = year;
            MaNoiDung = maNoiDung;
            NhaThau = nhathau;
            ChuKy = chuky;
            NgayThucHien = ngaythuchien;
            ThoiGian_ThucHien = thoigianthuchien;
            ViTri = vitri;
            SoLuong = soluong;
            YeuCau = yeucau;
            Note = note;
            NgayKhaiBaoThietBi = ngaykhaibaoTB;
            ThoiGianThongBao = thoigianthongbao;
        }

        public string  Year { get; set; }

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

        [StringLength(50)]
        public string DateCreated { get; set; }

        [StringLength(50)]
        public string DateModified { get; set; }

        [StringLength(50)]
        public string UserCreated { get; set; }

        [StringLength(50)]
        public string UserModified { get; set; }

        [ForeignKey("MaNoiDung")]
        public virtual EHS_NOIDUNG EHS_NOIDUNG { get; set; }
    }
}