namespace ConsoleApp3
{
    using System.ComponentModel.DataAnnotations;

    public partial class HR_PLOAI_NHANVIEN
    {
        public int Id { get; set; }

        [StringLength(100)]
        public string TenLoaiNV { get; set; }
    }
}
