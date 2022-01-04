using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace CodeFirstDB
{
    public partial class Model1 : DbContext
    {
        public Model1()
            : base("name=Model1")
        {
        }

        public virtual DbSet<BOPHAN> BOPHANs { get; set; }
        public virtual DbSet<HR_BHXH> HR_BHXH { get; set; }
        public virtual DbSet<HR_CHEDOBH> HR_CHEDOBH { get; set; }
        public virtual DbSet<HR_CHUCDANH> HR_CHUCDANH { get; set; }
        public virtual DbSet<HR_CHUNGCHI> HR_CHUNGCHI { get; set; }
        public virtual DbSet<HR_HOPDONG> HR_HOPDONG { get; set; }
        public virtual DbSet<HR_KEKHAIBAOHIEM> HR_KEKHAIBAOHIEM { get; set; }
        public virtual DbSet<HR_LOAIHOPDONG> HR_LOAIHOPDONG { get; set; }
        public virtual DbSet<HR_NHANVIEN> HR_NHANVIEN { get; set; }
        public virtual DbSet<HR_QUATRINHLAMVIEC> HR_QUATRINHLAMVIEC { get; set; }
        public virtual DbSet<HR_TINHTRANGHOSO> HR_TINHTRANGHOSO { get; set; }
        public virtual DbSet<LOAICHUNGCHI> LOAICHUNGCHIs { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<HR_BHXH>()
                .HasMany(e => e.HR_NHANVIEN1)
                .WithOptional(e => e.HR_BHXH1)
                .HasForeignKey(e => e.MaBHXH);

            modelBuilder.Entity<HR_CHEDOBH>()
                .HasMany(e => e.HR_KEKHAIBAOHIEM)
                .WithOptional(e => e.HR_CHEDOBH)
                .HasForeignKey(e => e.CheDoBH);

            modelBuilder.Entity<HR_LOAIHOPDONG>()
                .HasMany(e => e.HR_HOPDONG)
                .WithOptional(e => e.HR_LOAIHOPDONG)
                .HasForeignKey(e => e.LoaiHD);

            modelBuilder.Entity<HR_NHANVIEN>()
                .HasMany(e => e.HR_BHXH)
                .WithOptional(e => e.HR_NHANVIEN)
                .HasForeignKey(e => e.MaNV);

            modelBuilder.Entity<HR_NHANVIEN>()
                .HasMany(e => e.HR_QUATRINHLAMVIEC)
                .WithRequired(e => e.HR_NHANVIEN)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<HR_NHANVIEN>()
                .HasOptional(e => e.HR_TINHTRANGHOSO)
                .WithRequired(e => e.HR_NHANVIEN);

            modelBuilder.Entity<LOAICHUNGCHI>()
                .HasMany(e => e.HR_CHUNGCHI)
                .WithOptional(e => e.LOAICHUNGCHI1)
                .HasForeignKey(e => e.LoaiChungChi);
        }
    }
}
