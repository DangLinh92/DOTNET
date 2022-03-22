using HRMNS.Application.ViewModels.Time_Attendance;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMNS.Application.ViewModels.System
{
    public class ChamCongDataViewModel
    {
        public ChamCongDataViewModel()
        {
            WorkingStatuses = new List<WorkingStatus>();
            EL_LC_Statuses = new List<EL_LC_Status>();
            OvertimeValues = new List<OvertimeValue>();
            lstHopDong = new List<HopDong_NV>();
            lstNhanVienCaLamViec = new List<NhanVien_CaLamViec>();
        }

        public string MaNV { get; set; }

        public string TenNV { get; set; }

        public string NgayVao { get; set; }

        public string BoPhan { get; set; }

        public string BoPhanDetail { get; set; }

        public List<HopDong_NV> lstHopDong { get; set; }
        public List<NhanVien_CaLamViec> lstNhanVienCaLamViec { get; set; }

        public List<ChamCongDB> lstChamCongDB { get; set; }

        public List<DangKyOT> lstDangKyOT { get; set; }

        public string month_Check { get; set; }

        public List<string> lstDanhMucOT { get; set; } // 150%,200%,....

        public List<WorkingStatus> WorkingStatuses { get; set; }
        public List<EL_LC_Status> EL_LC_Statuses { get; set; }
        public List<OvertimeValue> OvertimeValues { get; set; }
    }

    public class DangKyOT
    {
        public string NgayOT { get; set; }
        public string Ten_NgayLV { get; set; }
        public string DM_NgayLViec { get; set; }
    }

    public class ChamCongDB
    {
        public string TenChiTiet { get; set; }
        public string NgayBatDau { get; set; }
        public string NgayKetThuc { get; set; }
        public string KyHieuChamCong { get; set; }
    }

    public class NhanVien_CaLamViec
    {
        public string MaCaLaviec { get; set; }
        public string TenCaLamViec { get; set; }
        public string BatDau_TheoCa { get; set; }// thoi gian bat dau, thuong là 2 tuan
        public string KetThuc_TheoCa { get; set; }
        public string Time_BatDau { get; set; }
        public string Time_BatDau2 { get; set; }
        public string Time_KetThuc { get; set; }
        public string Time_KetThuc2 { get; set; }
        public float HeSo_OT { get; set; }

        public string DM_NgayLViec { get; set; }
        public string Ten_NgayLV { get; set; }
    }

    // Thong tin hop dong
    public class HopDong_NV
    {
        public int LoaiHD { get; set; }
        public string TenHD { get; set; }
        public string ShortName { get; set; }
        public string NgayHieuLucHD { get; set; }
        public string NgayHetHLHD { get; set; }
    }

    public class WorkingStatus
    {
        public string DayCheck { get; set; }
        public string Value { get; set; } // ky hieu cham cong
    }

    public class EL_LC_Status
    {
        public string DayCheck_EL { get; set; }
        public double Value_EL { get; set; } // ky hieu cham cong
    }

    public class OvertimeValue
    {
        public string DMOvertime { get; set; }
        public string DayCheckOT { get; set; }
        public double ValueOT { get; set; }
    }
}
