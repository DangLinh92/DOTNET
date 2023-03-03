using HRMNS.Data.Interfaces;
using HRMS.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HRMNS.Data.Entities
{
    [Table("EHS_QUANLY_GIAY_PHEP")]
    public class EHS_QUANLY_GIAY_PHEP : DomainEntity<int>, IDateTracking
    {
        public EHS_QUANLY_GIAY_PHEP()
        {
        }

        public EHS_QUANLY_GIAY_PHEP(int id,string demuc,string noidung,string luatdinh,
            string lydo,string tiendo,string thoigian,int songay,string ketqua,string nguoiThucHien,Guid maevt,string status)
        {
            Id = id;
            Demuc = demuc;
            NoiDung = noidung;
            LuatDinhLienQuan = luatdinh;
            LyDoThucHien = lydo;
            TienDo = tiendo;
            ThoiGianThucHien = thoigian;
            SoNgayBaoTruoc = songay;
            KetQua = ketqua;
            NguoiThucHien = nguoiThucHien;
            MaEvent = maevt;
            Status = status;
        }

        [StringLength(250)]
        public string Demuc { get; set; }

        [StringLength(1000)]
        public string NoiDung { get; set; }

        [StringLength(1000)]
        public string LuatDinhLienQuan { get; set; }

        [StringLength(1000)]
        public string LyDoThucHien { get; set; }

        [StringLength(250)]
        public string TienDo { get; set; }

        [StringLength(50)]
        public string ThoiGianThucHien { get; set; }

        public int SoNgayBaoTruoc { get; set; }

        [StringLength(250)]
        public string KetQua { get; set; }

        [StringLength(50)]
        public string NguoiThucHien { get; set; }

        [StringLength(50)]
        public string Status { get; set; }

        public Guid MaEvent { get; set; }

        [ForeignKey("MaEvent")]
        public virtual EVENT_SHEDULE_PARENT EVENT_SHEDULE_PARENT { get; set; }


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
