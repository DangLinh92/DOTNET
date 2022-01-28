using HRMNS.Data.Entities;
using HRMNS.Data.Enums;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMNS.Data.EF
{
    public class DBInitializer
    {
        private readonly AppDBContext _context;
        private UserManager<APP_USER> _userManager;
        private RoleManager<APP_ROLE> _roleManager;

        public DBInitializer(AppDBContext context, UserManager<APP_USER> userManager, RoleManager<APP_ROLE> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task Seed()
        {
            if (!_roleManager.Roles.Any())
            {
                await _roleManager.CreateAsync(new APP_ROLE()
                {
                    Name = "Admin",
                    NormalizedName = "Admin",
                    Description = "Top manager"
                });
                await _roleManager.CreateAsync(new APP_ROLE()
                {
                    Name = "Staff",
                    NormalizedName = "Staff",
                    Description = "Staff"
                });
                await _roleManager.CreateAsync(new APP_ROLE()
                {
                    Name = "HR",
                    NormalizedName = "HR",
                    Description = "Human Resources"
                });
                await _roleManager.CreateAsync(new APP_ROLE()
                {
                    Name = "EHS",
                    NormalizedName = "EHS",
                    Description = "Health, Safety and Environment"
                });
                await _roleManager.CreateAsync(new APP_ROLE()
                {
                    Name = "GA",
                    NormalizedName = "GA",
                    Description = "General Administration"
                });
            }
            if (!_userManager.Users.Any())
            {
                await _userManager.CreateAsync(new APP_USER()
                {
                    UserName = "admin",
                    FullName = "Administrator",
                    Email = "admin@gmail.com",
                    ShowPass = "123654$",
                    Avatar = "/img/profiles/avatar-01.jpg"
                }, "123654$");
                var user = await _userManager.FindByNameAsync("admin");
                await _userManager.AddToRoleAsync(user, "Admin");
            }

            if (_context.Functions.Count() == 0)
            {
                _context.Functions.AddRange(new List<FUNCTION>()
                {
                    new FUNCTION() {Id = "EHS", Name = "EHS",ParentId = null,SortOrder = 1,Status = Status.Active,URL = "",IconCss = ""  },
                    new FUNCTION() {Id = "SAFETY", Name = "Safety",ParentId = "EHS",SortOrder = 1,Status = Status.Active,URL = "/admin/safety/index",IconCss = "la la-share-alt"  },
                    new FUNCTION() {Id = "ENVIRONMENT", Name = "Environment",ParentId = "EHS",SortOrder = 2,Status = Status.Active,URL = "/admin/inviroment/index",IconCss = "la la-puzzle-piece"  },
                    new FUNCTION() {Id = "DOCUMENT", Name = "Document",ParentId = "EHS",SortOrder =3,Status = Status.Active,URL = "/admin/document/index",IconCss = "la la-file-text"  },

                    new FUNCTION() {Id = "HR", Name = "HR",ParentId = null,SortOrder = 2,Status = Status.Active,URL = "",IconCss = ""  },
                    new FUNCTION() {Id = "NHANSU", Name = "Personnel",ParentId = "HR",SortOrder = 1,Status = Status.Active,URL = "",IconCss = "la la-user"  },
                    new FUNCTION() {Id = "NHANVIEN", Name = "Employees",ParentId = "NHANSU",SortOrder = 1,Status = Status.Active,URL = "/admin/nhanvien/index",IconCss = ""  },
                    new FUNCTION() {Id = "TIMEKEEPING", Name = "Timekeeping",ParentId = "HR",SortOrder = 2,Status = Status.Active,URL = "/admin/chamcong/index",IconCss = "la la-edit"  },

                    new FUNCTION() {Id = "GA",Name = "GA",ParentId = null,SortOrder = 3,Status = Status.Active,URL = "",IconCss = ""  },
                    new FUNCTION() {Id = "COST",Name = "Cost",ParentId = "GA",SortOrder = 1,Status = Status.Active,URL = "/admin/cost/index",IconCss = "la la-money"  },

                    new FUNCTION() {Id = "SETTINGS_TOP",Name = "Settings",ParentId = null,SortOrder = 4,Status = Status.Active,URL = "",IconCss = ""  },
                    new FUNCTION() {Id = "SETTINGS",Name = "Settings",ParentId = "SETTINGS_TOP",SortOrder = 1,Status = Status.Active,URL = "/admin/settings/index",IconCss = "la la-cog"  },
                });
            }

            if (_context.BoPhans.Count() == 0)
            {
                _context.BoPhans.AddRange(new List<BOPHAN>()
                {
                    new BOPHAN()
                    {
                        Id = "KHO",
                        TenBoPhan = "KHO"
                    },
                    new BOPHAN()
                    {
                        Id = "SMT",
                        TenBoPhan = "SMT"
                    }
                    ,
                    new BOPHAN()
                    {
                        Id = "CSP",
                        TenBoPhan = "CSP"
                    },
                     new BOPHAN()
                    {
                        Id = "WLP1",
                        TenBoPhan = "WLP1"
                    }
                    ,
                    new BOPHAN()
                    {
                        Id = "WLP2",
                        TenBoPhan = "WLP2"
                    },
                    new BOPHAN()
                    {
                        Id = "LFEM",
                        TenBoPhan = "LFEM"
                    }
                    ,
                    new BOPHAN()
                    {
                        Id = "KOREA",
                        TenBoPhan = "KOREA"
                    },
                     new BOPHAN()
                    {
                        Id = "QC",
                        TenBoPhan = "QC"
                    },
                      new BOPHAN()
                    {
                        Id = "SP",
                        TenBoPhan = "SP"
                    },
                        new BOPHAN()
                    {
                        Id = "UTILITY",
                        TenBoPhan = "UTILITY"
                    }
                });
            }

            if (_context.HrBoPhanDetail.Count() == 0)
            {
                _context.HrBoPhanDetail.AddRange(new List<HR_BO_PHAN_DETAIL>() {
                    new HR_BO_PHAN_DETAIL()
                    {
                        TenBoPhanChiTiet = "Accounting/ Kế toán",
                        MaBoPhan = "SP"
                    },
                     new HR_BO_PHAN_DETAIL()
                    {
                        TenBoPhanChiTiet = "CSP Manufacturing/ Sản xuất CSP",
                        MaBoPhan = "CSP"
                    },
                      new HR_BO_PHAN_DETAIL()
                    {
                        TenBoPhanChiTiet = "CSP Technology/ Kỹ thuật CSP",
                        MaBoPhan = "CSP"
                    },
                        new HR_BO_PHAN_DETAIL()
                    {
                        TenBoPhanChiTiet = "GOC",
                        MaBoPhan = "SP"
                    },
                        new HR_BO_PHAN_DETAIL()
                    {
                        TenBoPhanChiTiet = "Human Resource/ HCNS",
                        MaBoPhan = "SP"
                    }
                        ,
                        new HR_BO_PHAN_DETAIL()
                    {
                        TenBoPhanChiTiet = "Human Resource/ HCNS (EHS)",
                        MaBoPhan = "SP"
                    } ,
                        new HR_BO_PHAN_DETAIL()
                    {
                        TenBoPhanChiTiet = "Human Resource/ HCNS (Utility)",
                        MaBoPhan = "UTILITY"
                    },
                        new HR_BO_PHAN_DETAIL()
                    {
                        TenBoPhanChiTiet = "KOREA",
                        MaBoPhan = "KOREA"
                    },    new HR_BO_PHAN_DETAIL()
                    {
                        TenBoPhanChiTiet = "LFEM  Manufacturing/ Sản xuất LFEM",
                        MaBoPhan = "LFEM"
                    }
                    ,    new HR_BO_PHAN_DETAIL()
                    {
                        TenBoPhanChiTiet = "LFEM Technology/ Kỹ thuật LFEM",
                        MaBoPhan = "LFEM"
                    } ,    new HR_BO_PHAN_DETAIL()
                    {
                        TenBoPhanChiTiet = "Logistic/ XNK",
                        MaBoPhan = "SP"
                    }
                     ,    new HR_BO_PHAN_DETAIL()
                    {
                        TenBoPhanChiTiet = "PI",
                        MaBoPhan = "SP"
                    } ,    new HR_BO_PHAN_DETAIL()
                    {
                        TenBoPhanChiTiet = "Purchasing/ Mua hàng",
                        MaBoPhan = "SP"
                    } ,    new HR_BO_PHAN_DETAIL()
                    {
                        TenBoPhanChiTiet = "Quality Control/ QL Chất lượng",
                        MaBoPhan = "QC"
                    },    new HR_BO_PHAN_DETAIL()
                    {
                        TenBoPhanChiTiet = "Quality Control/ QL Chất lượng (CS)",
                        MaBoPhan = "QC"
                    },    new HR_BO_PHAN_DETAIL()
                    {
                        TenBoPhanChiTiet = "Quality Control/ QL Chất lượng (IQC)",
                        MaBoPhan = "QC"
                    },    new HR_BO_PHAN_DETAIL()
                    {
                        TenBoPhanChiTiet = "Quality Control/ QL Chất lượng (OQC)",
                        MaBoPhan = "QC"
                    },    new HR_BO_PHAN_DETAIL()
                    {
                        TenBoPhanChiTiet = "Quality Control/ QL Chất lượng (PQC)",
                        MaBoPhan = "QC"
                    },    new HR_BO_PHAN_DETAIL()
                    {
                        TenBoPhanChiTiet = "Quality Control/ QL Chất lượng (QQC)",
                        MaBoPhan = "QC"
                    },    new HR_BO_PHAN_DETAIL()
                    {
                        TenBoPhanChiTiet = "Quality Control/ QL Chất lượng (Reability)",
                        MaBoPhan = "QC"
                    },    new HR_BO_PHAN_DETAIL()
                    {
                        TenBoPhanChiTiet = "SMT Manufacturing/ Sản xuất SMT",
                        MaBoPhan = "SMT"
                    },    new HR_BO_PHAN_DETAIL()
                    {
                        TenBoPhanChiTiet = "SMT Innovation Group",
                        MaBoPhan = "SMT"
                    },    new HR_BO_PHAN_DETAIL()
                    {
                        TenBoPhanChiTiet = "SMT Technology/ Kỹ thuật SMT",
                        MaBoPhan = "SMT"
                    },    new HR_BO_PHAN_DETAIL()
                    {
                        TenBoPhanChiTiet = "Warehouse",
                        MaBoPhan = "KHO"
                    },    new HR_BO_PHAN_DETAIL()
                    {
                        TenBoPhanChiTiet = "WLP1 Manufacturing/ Sản xuất WLP1",
                        MaBoPhan = "WLP1"
                    }
                    ,    new HR_BO_PHAN_DETAIL()
                    {
                        TenBoPhanChiTiet = "WLP1 Technology/ Kỹ thuật WLP1",
                        MaBoPhan = "WLP1"
                    }
                     ,    new HR_BO_PHAN_DETAIL()
                    {
                        TenBoPhanChiTiet = "WLP2 Manufacturing/ Sản xuất WLP2",
                        MaBoPhan = "WLP2"
                    }
                      ,    new HR_BO_PHAN_DETAIL()
                    {
                        TenBoPhanChiTiet = "WLP2 Technology/ Kỹ thuật WLP2",
                        MaBoPhan = "WLP2"
                    }
                });
            }

            if (_context.HrNhanVien.Count() == 0)
            {
                _context.HrNhanVien.AddRange(new List<HR_NHANVIEN>() {
                    new HR_NHANVIEN()
                    {
                        Id="H2105001",TenNV="Lê Văn Đặng",MaBoPhan="PI",NgaySinh="1992-05-04",Email="danglevan.9919@gmail.com",SoDienThoai="0974628108",NgayVao="2021-05-01",Status=Status.Active.ToString(),Image="/img/profiles/avatar-01.jpg"
                    },
                    new HR_NHANVIEN()
                    {
                        Id="H2105002",TenNV="Lê Văn Đặng2",MaBoPhan="PI",NgaySinh="1992-05-04",Email="danglevan.9919@gmail.com",SoDienThoai="0974628108",NgayVao="2021-05-01",Status=Status.Active.ToString(),Image="/img/profiles/avatar-01.jpg"
                    },
                    new HR_NHANVIEN()
                    {
                        Id="H2105003",TenNV="Lê Văn Đặng3",MaBoPhan="PI",NgaySinh="1992-05-04",Email="danglevan.9919@gmail.com",SoDienThoai="0974628108",NgayVao="2021-05-01",Status=Status.Active.ToString(),Image="/img/profiles/avatar-01.jpg"
                    },
                    new HR_NHANVIEN()
                    {
                        Id="H2105004",TenNV="Lê Văn Đặng4",MaBoPhan="PI",NgaySinh="1992-05-04",Email="danglevan.9919@gmail.com",SoDienThoai="0974628108",NgayVao="2021-05-01",Status=Status.Active.ToString(),Image="/img/profiles/avatar-01.jpg"
                    },
                    new HR_NHANVIEN()
                    {
                        Id="H2105005",TenNV="Lê Văn Đặng5",MaBoPhan="PI",NgaySinh="1992-05-04",Email="danglevan.9919@gmail.com",SoDienThoai="0974628108",NgayVao="2021-05-01",Status=Status.Active.ToString(),Image="/img/profiles/avatar-01.jpg"
                    },
                    new HR_NHANVIEN()
                    {
                        Id="H2105006",TenNV="Lê Văn Đặng6",MaBoPhan="PI",NgaySinh="1992-05-04",Email="danglevan.9919@gmail.com",SoDienThoai="0974628108",NgayVao="2021-05-01",Status=Status.Active.ToString(),Image="/img/profiles/avatar-01.jpg"
                    },
                    new HR_NHANVIEN()
                    {
                        Id="H2105007",TenNV="Lê Văn Đặng7",MaBoPhan="PI",NgaySinh="1992-05-04",Email="danglevan.9919@gmail.com",SoDienThoai="0974628108",NgayVao="2021-05-01",Status=Status.Active.ToString(),Image="/img/profiles/avatar-01.jpg"
                    },
                    new HR_NHANVIEN()
                    {
                        Id="H2105008",TenNV="Lê Văn Đặng8",MaBoPhan="PI",NgaySinh="1992-05-04",Email="danglevan.9919@gmail.com",SoDienThoai="0974628108",NgayVao="2021-05-01",Status=Status.Active.ToString(),Image="/img/profiles/avatar-01.jpg"
                    },
                    new HR_NHANVIEN()
                    {
                        Id="H2105009",TenNV="Lê Văn Đặng9",MaBoPhan="PI",NgaySinh="1992-05-04",Email="danglevan.9919@gmail.com",SoDienThoai="0974628108",NgayVao="2021-05-01",Status=Status.Active.ToString(),Image="/img/profiles/avatar-01.jpg"
                    },
                    new HR_NHANVIEN()
                    {
                        Id="H2105010",TenNV="Lê Văn Đặng10",MaBoPhan="PI",NgaySinh="1992-05-04",Email="danglevan.9919@gmail.com",SoDienThoai="0974628108",NgayVao="2021-05-01",Status=Status.InActive.ToString(),Image="/img/profiles/avatar-01.jpg"
                    }
                });
            }

            if (_context.HrChucDanh.Count() == 0)
            {
                _context.HrChucDanh.AddRange(new List<HR_CHUCDANH>() {
                    new HR_CHUCDANH()
                    {
                        Id = "Staff",
                        TenChucDanh="Staff"
                    },
                    new HR_CHUCDANH()
                    {
                        Id = "Assistant leader",
                        TenChucDanh="Assistant leader/ Phó nhóm"
                    },
                    new HR_CHUCDANH()
                    {
                        Id = "Assistant Manager",
                        TenChucDanh="Assistant Manager"
                    },
                    new HR_CHUCDANH()
                    {
                        Id = "Engineer Assistant leader",
                        TenChucDanh="Engineer Assistant leader/ Phó nhóm kỹ thuật"
                    },
                    new HR_CHUCDANH()
                    {
                        Id = "Engineer shift leader",
                        TenChucDanh="Engineer shift leader/ Tổ trưởng kỹ thuật"
                    },
                    new HR_CHUCDANH()
                    {
                        Id = "Engineer supervisor",
                        TenChucDanh="Engineer supervisor/ Giám sát kỹ thuật"
                    },
                    new HR_CHUCDANH()
                    {
                        Id = "General Director",
                        TenChucDanh="General Director"
                    },
                    new HR_CHUCDANH()
                    {
                        Id = "General Manager",
                        TenChucDanh="General Manager"
                    },
                    new HR_CHUCDANH()
                    {
                        Id = "Group Leader",
                        TenChucDanh="Group Leader/ Trưởng bộ phận"
                    }
                    ,
                    new HR_CHUCDANH()
                    {
                        Id = "Manager",
                        TenChucDanh="Manager"
                    },
                    new HR_CHUCDANH()
                    {
                        Id = "Operator",
                        TenChucDanh="Operator/ Công nhân"
                    },
                    new HR_CHUCDANH()
                    {
                        Id = "Operator Radiation safety",
                        TenChucDanh="Operator/ Công nhân - An toàn bức xạ"
                    },
                    new HR_CHUCDANH()
                    {
                        Id = "Operator XRF",
                        TenChucDanh="Operator/ Công nhân (bức xạ XRF)"
                    },
                    new HR_CHUCDANH()
                    {
                        Id = "Operator TOXIC",
                        TenChucDanh="Operator/ Công nhân tiếp xúc hóa chất độc hại"
                    },
                    new HR_CHUCDANH()
                    {
                        Id = "Part Leader",
                        TenChucDanh="Part Leader/ Trưởng nhóm"
                    },
                    new HR_CHUCDANH()
                    {
                        Id = "Senior Manager",
                        TenChucDanh="Senior Manager"
                    },
                    new HR_CHUCDANH()
                    {
                        Id = "Shift leader",
                        TenChucDanh="Shift leader/ Tổ trưởng sản xuất"
                    },
                    new HR_CHUCDANH()
                    {
                        Id = "Staff Xray",
                        TenChucDanh="Staff/ Nhân viên - Bức xạ máy Xray"
                    },
                    new HR_CHUCDANH()
                    {
                        Id = "Staff Reveal",
                        TenChucDanh="Staff/ Nhân viên - Lộ quang"
                    },
                    new HR_CHUCDANH()
                    {
                        Id = "Staff Ma",
                        TenChucDanh="Staff/ Nhân viên - Mạ"
                    },
                    new HR_CHUCDANH()
                    {
                        Id = "Staff XRF",
                        TenChucDanh="Staff/ Nhân viên (bức xạ XRF)"
                    },
                    new HR_CHUCDANH()
                    {
                        Id = "Supervisor",
                        TenChucDanh="Supervisor/ Giám sát line"
                    }
                });
            }

            await _context.SaveChangesAsync();
        }
    }
}
