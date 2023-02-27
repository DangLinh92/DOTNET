
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMNS.Application.ViewModels.EHS
{
    public class EhsKeHoachItemModel
    {
        public int STT { get; set; }
        public string Demuc { get; set; }
        public string NoiDung { get; set; }
        public string ThoiGian { get; set; }
        public string NguoiPhuTrach { get; set; }
        public int SoNgayConLai { get; set; }
        public int Progress { get; set; }
        public string Status { get; set; }
        public string ActualFinish { get; set; }
        public string Folder { get; set; }
        public string MaNgayThucHien { get; set; }
    }
}
