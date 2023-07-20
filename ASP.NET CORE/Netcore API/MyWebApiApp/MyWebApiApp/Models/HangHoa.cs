using System;

namespace MyWebApiApp.Models
{
    public class HangHoaVM
    {
        public string TenHangHoa { get; set; }
        public float DonGia { get; set; }

    }

    public class HangHoa : HangHoaVM
    {
        public Guid MaHangHoa { get; set; }
    }
}
