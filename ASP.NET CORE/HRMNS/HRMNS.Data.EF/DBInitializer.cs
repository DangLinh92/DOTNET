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

                await _roleManager.CreateAsync(new APP_ROLE()
                {
                    Name = "GrLeader",
                    NormalizedName = "GroupLeader",
                    Description = "Group Leader"
                });

                await _roleManager.CreateAsync(new APP_ROLE()
                {
                    Name = "AssLeader",
                    NormalizedName = "AssLeader",
                    Description = "Assistance Leader"
                });

                await _roleManager.CreateAsync(new APP_ROLE()
                {
                    Name = "KRManager",
                    NormalizedName = "KRManager",
                    Description = "Korea Manager"
                });

                await _roleManager.CreateAsync(new APP_ROLE()
                {
                    Name = "HR_View",
                    NormalizedName = "HR_View",
                    Description = "Human Resources Only View"
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

                await _userManager.CreateAsync(new APP_USER()
                {
                    UserName = "HRSub",
                    FullName = "HRSub",
                    Email = "HRSub@gmail.com",
                    ShowPass = "543120$",
                    Avatar = "/img/profiles/avatar-01.jpg"
                }, "543120$");
                var user1 = await _userManager.FindByNameAsync("HRSub");
                await _userManager.AddToRoleAsync(user1, "HRS");
            }

            if (_context.Functions.Count() == 0)
            {
                _context.Functions.AddRange(new List<FUNCTION>()
                {
                    new FUNCTION() {Id = "EHS", Name = "EHS",ParentId = null,SortOrder = 1,Status = Status.Active,URL = "",IconCss = ""  },
                   new FUNCTION() {Id = "KEHOACH", Name = "Kế Hoạch",ParentId = "EHS",SortOrder = 1,Status = Status.Active,URL = "",IconCss = "la la-share-alt"  },
                    new FUNCTION() {Id = "DMKEHOACH", Name = "Danh Mục Kế Hoạch",ParentId = "KEHOACH",SortOrder = 1,Status = Status.Active,URL = "/admin/ehsdanhmuckehoach/index",IconCss = ""  },
                    new FUNCTION() {Id = "KEHOACHTHEONAM", Name = "Kế Hoạch Theo Năm",ParentId = "KEHOACH",SortOrder = 2,Status = Status.Active,URL = "/admin/kehoachtheonam/index",IconCss = ""  },
                    new FUNCTION() {Id = "TONGHOPKEHOACH", Name = "Tổng hợp Kế Hoạch",ParentId = "KEHOACH",SortOrder = 3,Status = Status.Active,URL = "/admin/tonghopkehoach/index",IconCss = ""  },
                    new FUNCTION() {Id = "DOCUMENT", Name = "Tài Liệu",ParentId = "EHS",SortOrder =2,Status = Status.Active,URL = "/admin/ehsdocument/index",IconCss = "la la-file-text"  },

                    new FUNCTION() {Id = "HR", Name = "HR",ParentId = null,SortOrder = 2,Status = Status.Active,URL = "",IconCss = ""  },
                    new FUNCTION() {Id = "NHANSU", Name = "Nhân Sự",ParentId = "HR",SortOrder = 1,Status = Status.Active,URL = "",IconCss = "la la-user"  },
                    new FUNCTION() {Id = "NHANVIEN", Name = "Danh Sách Nhân Viên",ParentId = "NHANSU",SortOrder = 1,Status = Status.Active,URL = "/admin/nhanvien/index",IconCss = ""  },
                    new FUNCTION() {Id = "NHANVIEN_NGHIVIEC", Name = "Danh Sách Nghỉ Việc",ParentId = "NHANSU",SortOrder = 2,Status = Status.Active,URL = "/admin/nhanvien/nhanviennghiviec",IconCss = ""  },

                    new FUNCTION() {Id = "CHAMCONG", Name = "Chấm Công",ParentId = "HR",SortOrder = 2,Status = Status.Active,URL = "",IconCss = "la la-info"  },
                    new FUNCTION() {Id = "SHIFT_SCHEDULE", Name = "Phân ca",ParentId = "CHAMCONG",SortOrder = 1,Status = Status.Active,URL = "/admin/nhanvien_calamviec/index",IconCss = ""  },
                    new FUNCTION() {Id = "REGIS_TIMEKEEPING", Name = "Chấm công bổ sung",ParentId = "CHAMCONG",SortOrder = 2,Status = Status.Active,URL = "/admin/chamcongdacbiet/index",IconCss = ""  },
                    new FUNCTION() {Id = "OVERTIME", Name = "Đăng ký tăng ca",ParentId = "CHAMCONG",SortOrder = 3,Status = Status.Active,URL = "/admin/dangkyot/index",IconCss = ""  },
                    new FUNCTION() {Id = "THAISAN", Name = "Thai sản",ParentId = "CHAMCONG",SortOrder = 4,Status = Status.Active,URL = "/admin/NhanVienThaiSan/index",IconCss = ""  },
                    new FUNCTION() {Id = "TIMEKEEPING", Name = "Dữ liệu vào/ra",ParentId = "CHAMCONG",SortOrder = 5,Status = Status.Active,URL = "/admin/chamcong/index",IconCss = ""  },
                    new FUNCTION() {Id = "ATTENDANCE_RECORD", Name = "Bảng Công",ParentId = "CHAMCONG",SortOrder = 6,Status = Status.Active,URL = "/admin/bangcong/index",IconCss = ""  },
                    
                    //new FUNCTION() {Id = "ADJ_TIMEKEEPING", Name = "Ghi Chú Công",ParentId = "CHAMCONG",SortOrder = 6,Status = Status.Active,URL = "/admin/dcchamcong/index",IconCss = ""  },

                    new FUNCTION() {Id = "GA",Name = "GA",ParentId = null,SortOrder = 3,Status = Status.Active,URL = "",IconCss = ""  },
                    new FUNCTION() {Id = "COST",Name = "Cost",ParentId = "GA",SortOrder = 1,Status = Status.Active,URL = "/admin/cost/index",IconCss = "la la-money"  },

                    new FUNCTION() {Id = "OTHER",Name = "Others",ParentId = null,SortOrder = 5,Status = Status.Active,URL = "",IconCss = ""  },
                    new FUNCTION() {Id = "DOCUMENT_SUB",Name = "Documents",ParentId = "OTHER",SortOrder = 1,Status = Status.Active,URL = "/admin/documentall/index",IconCss = "la la-file-text"  },
                    new FUNCTION() {Id = "CALENDAR",Name = "Calendar",ParentId = "OTHER",SortOrder = 2,Status = Status.Active,URL = "/admin/calendar/index",IconCss = "la la-table"  },
                    new FUNCTION() {Id = "SETTINGS",Name = "Settings",ParentId = "OTHER",SortOrder = 3,Status = Status.Active,URL = "/admin/settings/index",IconCss = "la la-cog"  },
                    new FUNCTION() {Id = "ROLE_PERMISSTION",Name = "Phân Quyền User",ParentId = "SETTINGS",SortOrder = 1,Status = Status.Active,URL = "/admin/roleandpermisstion/index",IconCss = "la la-key"  },

                    new FUNCTION() {Id = "DAO_TAO_EVENT", Name = "Performance",ParentId = null,SortOrder = 4,Status = Status.Active,URL = "",IconCss = ""  },
                    new FUNCTION() {Id = "TRAINING",Name = "Đào Tạo",ParentId = "DAO_TAO_EVENT",SortOrder = 1,Status = Status.Active,URL = "",IconCss = "la la-edit"  },
                    new FUNCTION() {Id = "TRAINING_LIST",Name = "Kế Hoạch Đào Tạo",ParentId = "TRAINING",SortOrder = 1,Status = Status.Active,URL = "/admin/traininglist/index",IconCss = ""  },
                    new FUNCTION() {Id = "TRAINING_TYPE",Name = "Danh Mục Đào Tạo",ParentId = "TRAINING",SortOrder = 2,Status = Status.Active,URL = "/admin/trainingtype/index",IconCss = ""  },
                    new FUNCTION() {Id = "SAMSUNG_TRAINING",Name = "Samsung traning",ParentId = "TRAINING",SortOrder = 3,Status = Status.Active,URL = "/admin/samsungtraining/index",IconCss = ""  },
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
                        Id = "CSP1",
                        TenBoPhan = "CSP1"
                    },
                     new BOPHAN()
                    {
                        Id = "CSP2",
                        TenBoPhan = "CSP2"
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

            if (_context.HrLoaiHopDong.Count() == 0)
            {
                _context.HrLoaiHopDong.AddRange(new List<HR_LOAIHOPDONG>()
                {
                   new HR_LOAIHOPDONG(){ TenLoaiHD = "Hợp Đồng Thử Việc(85%)",ShortName = "TV85" },
                   new HR_LOAIHOPDONG(){ TenLoaiHD = "Hợp Đồng Thử Việc Cho OP(100%)",ShortName = "TV100" },
                   new HR_LOAIHOPDONG(){ TenLoaiHD = "Hợp Đồng 1 năm lần 1",ShortName = "A_YEAR1" },
                   new HR_LOAIHOPDONG(){ TenLoaiHD = "Hợp Đồng 1 năm lần 2",ShortName = "A_YEAR2" },
                   new HR_LOAIHOPDONG(){ TenLoaiHD = "Hợp Đồng Không Thời Hạn",ShortName = "NO_LIMIT" }
                });
            }

            if (_context.HrCheDoBH.Count() == 0)
            {
                _context.HrCheDoBH.AddRange(new List<HR_CHEDOBH>()
                {
                    new HR_CHEDOBH()
                    {
                        Id = "HT",
                        TenCheDo = "Hưu trí-tử tuất"
                    },
                    new HR_CHEDOBH()
                    {
                        Id = "TS",
                        TenCheDo = "Ốm đau-thai sản"
                    },
                     new HR_CHEDOBH()
                    {
                        Id = "TNLD-BNN",
                        TenCheDo = "Tai nạn lao động, bệnh nghề nghiệp"
                    },
                      new HR_CHEDOBH()
                    {
                        Id = "BHTN",
                        TenCheDo = "BHTN"
                    },
                      new HR_CHEDOBH()
                    {
                        Id = "BHYT",
                        TenCheDo = "BHYT"
                    },
                });
            }


            // Cham Cong
            if (_context.TRU_SO_LVIEC.Count() == 0)
            {
                _context.TRU_SO_LVIEC.AddRange(new List<TRU_SO_LVIEC>() {
                    new TRU_SO_LVIEC()
                    {
                        Id = "WHC",
                        TenTruSo="Wisol Ha Noi",
                        DiaChi = "Số 26 , Đường số 05 , KCN VSIP Bắc Ninh T.xã, Bắc Ninh"
                    }
                });
            }

            if (_context.DM_NGAY_LAMVIEC.Count() == 0)
            {
                _context.DM_NGAY_LAMVIEC.AddRange(new List<DM_NGAY_LAMVIEC>() {
                    new DM_NGAY_LAMVIEC()
                    {
                        Id = "NT",
                        Ten_NgayLV = "Ngày thường 평일"
                    },
                     new DM_NGAY_LAMVIEC()
                    {
                        Id= "TNL",
                        Ten_NgayLV = "Trước ngày lễ 명절전"
                    },
                      new DM_NGAY_LAMVIEC()
                    {
                         Id="NL",
                        Ten_NgayLV = "Ngày lễ 휴일에"
                    },
                       new DM_NGAY_LAMVIEC()
                    {
                        Id="NLCC",
                        Ten_NgayLV = "Ngày lễ cuối cùng 명절"
                    },
                        new DM_NGAY_LAMVIEC()
                    {
                         Id= "CN",
                        Ten_NgayLV = "Chủ nhật 일요일"
                    },
                });
            }


            if (_context.DM_CA_LVIEC.Count() == 0)
            {
                _context.DM_CA_LVIEC.AddRange(new List<DM_CA_LVIEC>()
                {
                    new DM_CA_LVIEC()
                    {
                        Id = "CN_WHC",
                        MaTruSo = "WHC",
                        TenCaLamViec = "Ca ngày/ 주간"
                    },
                    new DM_CA_LVIEC()
                    {
                        Id="CD_WHC",
                        MaTruSo = "WHC",
                        TenCaLamViec = "Ca đêm/ 야간"
                    },
                     new DM_CA_LVIEC()
                    {
                        Id="CD_CN",
                        MaTruSo = "WHC",
                        TenCaLamViec = "Cdem-ConNho"
                    },
                     new DM_CA_LVIEC()
                    {
                        Id="CN_CN",
                        MaTruSo = "WHC",
                        TenCaLamViec = "Cngay-ConNho"
                    },
                     new DM_CA_LVIEC()
                    {
                        Id="TS",
                        MaTruSo = "WHC",
                        TenCaLamViec = "ThaiSan"
                    },
                     new DM_CA_LVIEC()
                    {
                        Id="VP_CN",
                        MaTruSo = "WHC",
                        TenCaLamViec = "VP-ConNho"
                    }
                });
            }

            if (_context.KY_HIEU_CHAM_CONG.Count() == 0)
            {
                _context.KY_HIEU_CHAM_CONG.AddRange(new List<KY_HIEU_CHAM_CONG>() {
                    new KY_HIEU_CHAM_CONG()
                    {
                        Id = "PD",
                        GiaiThich = "Probation Day shift/Thử việc ca ngày",
                    },
                    new KY_HIEU_CHAM_CONG()
                    {
                        Id = "PN",
                        GiaiThich = " Probation Night shift/Thử việc ca đêm",
                    },
                    new KY_HIEU_CHAM_CONG()
                    {
                        Id = "P/DS",
                        GiaiThich = "dayshift and take 1/2 probation/ làm ca ngày và nghỉ 1/2 ngày thử việc",
                    },
                    new KY_HIEU_CHAM_CONG()
                    {
                        Id = "P/NS",
                        GiaiThich = "nightshift and take 1/2 probation/ làm ca đêm và nghỉ 1/2 ca đêm thử việc",
                    },
                    new KY_HIEU_CHAM_CONG()
                    {
                        Id = "TV",
                        GiaiThich = "Thử việc làm thêm ngày chủ nhật/ Probation",
                    },
                    new KY_HIEU_CHAM_CONG()
                    {
                        Id = "TVD",
                        GiaiThich = "Thử việc làm thêm ca đêm chủ nhật",
                    },
                    new KY_HIEU_CHAM_CONG()
                    {
                        Id = "PDD",
                        GiaiThich = "Thử việc làm đêm ngày lễ",
                    }
                    ,
                    new KY_HIEU_CHAM_CONG()
                    {
                        Id = "PH",
                        GiaiThich = "Làm ca đêm trước ngày lễ(thử việc)",
                    }
                    ,
                    new KY_HIEU_CHAM_CONG()
                    {
                        Id = "PH/F",
                        GiaiThich = "Nghỉ 1/2 ca đêm trước ngày lễ và làm đầu ca đêm trước ngày lễ (thử việc)",
                    },
                     new KY_HIEU_CHAM_CONG()
                    {
                        Id = "PH/L",
                        GiaiThich = "Nghỉ 1/2 ca đêm trước ngày lễ và làm cuối ca đêm trước ngày lễ (thử việc)",
                    },
                      new KY_HIEU_CHAM_CONG()
                    {
                        Id = "DS",
                        GiaiThich = "Day Shift/ Ca ngày",
                    },
                       new KY_HIEU_CHAM_CONG()
                    {
                        Id = "NS",
                        GiaiThich = "Night Shift/ Ca đêm",
                    },
                         new KY_HIEU_CHAM_CONG()
                    {
                        Id = "AL",
                        GiaiThich = "AnnuALLeave/ Nghỉ phép",
                    },
                      new KY_HIEU_CHAM_CONG()
                    {
                        Id = "AL/DS",
                        GiaiThich = "dayshift and take 1/2 annuALleave/ làm ca ngày và nghỉ phép 1/2 ngày",
                    },
                        new KY_HIEU_CHAM_CONG()
                    {
                        Id = "AL/NS",
                        GiaiThich = "nightshift and take 1/2 annuALleave/ làm ca đêm và nghỉ phép 1/2 ngày",
                    },
                          new KY_HIEU_CHAM_CONG()
                    {
                        Id = "UL",
                        GiaiThich = "Unpaid Leave/ Nghỉ không lương (bao gồm cả thử việc)",
                    },
                    new KY_HIEU_CHAM_CONG()
                    {
                        Id = "UL/DS",
                        GiaiThich = "day shift and take 1/2 unpaid leave/ làm ca ngày và nghỉ không lương 1/2 ngày",
                    },

                    new KY_HIEU_CHAM_CONG()
                    {
                        Id = "UL/NS",
                        GiaiThich = "night shift and take 1/2 unpaid leave/ làm ca đêm và nghỉ không lương 1/2 ngày",
                    },
                    new KY_HIEU_CHAM_CONG()
                    {
                        Id = "D",
                        GiaiThich = "Làm ca đêm chủ nhật",
                    },
                    new KY_HIEU_CHAM_CONG()
                    {
                        Id = "NHD",
                        GiaiThich = "Làm ca đêm ngày lễ",
                    },

                    new KY_HIEU_CHAM_CONG()
                    {
                        Id = "BH",
                        GiaiThich = "Làm ca đêm trước ngày lễ(chính thức)",
                    },
                    new KY_HIEU_CHAM_CONG()
                    {
                        Id = "AL/BF",
                        GiaiThich = "First nightshift and take 1/2 annuALleave/ làm đầu ca đêm trước ngày lễ và nghỉ phép 1/2 ngày",
                    },
                    new KY_HIEU_CHAM_CONG()
                    {
                        Id = "AL/BL",
                        GiaiThich = "Last nightshift and take 1/2 annuALleave/ làm cuối ca đêm trước ngày lễ và nghỉ phép 1/2 ngày",
                    },

                    new KY_HIEU_CHAM_CONG()
                    {
                        Id = "UL/BF",
                        GiaiThich = "First night shift and take 1/2 unpaid leave/ làm đầu ca đêm trước ngày lễ và nghỉ không lương 1/2 ngày",
                    },
                    new KY_HIEU_CHAM_CONG()
                    {
                        Id = "UL/BL",
                        GiaiThich = "Last night shift and take 1/2 unpaid leave/ làm cuối ca đêm trước ngày lễ và nghỉ không lương 1/2 ngày",
                    },
                    new KY_HIEU_CHAM_CONG()
                    {
                        Id = "NH",
                        GiaiThich = "NationALHoliday/ Nghỉ lễ",
                    },
                    new KY_HIEU_CHAM_CONG()
                    {
                        Id = "SL",
                        GiaiThich = "SpeciALLeave/ Nghỉ đặc biệt (nghi huong luong)",
                    },

                    new KY_HIEU_CHAM_CONG()
                    {
                        Id = "HL",
                        GiaiThich = "nghỉ ốm hưởng lương theo quy định cty",
                    },

                    new KY_HIEU_CHAM_CONG()
                    {
                        Id = "B",
                        GiaiThich = "nghỉ bù trực",
                    },
                    new KY_HIEU_CHAM_CONG()
                    {
                        Id = "CT",
                        GiaiThich = "Đi Công tác",
                    },
                    new KY_HIEU_CHAM_CONG()
                    {
                        Id = "NB",
                        GiaiThich = "Nghỉ bù luân phiên",
                    },
                    new KY_HIEU_CHAM_CONG()
                    {
                        Id = "L70",
                        GiaiThich = "Nghỉ hưởng 70% lương vùng",
                    },

                    new KY_HIEU_CHAM_CONG()
                    {
                        Id = "KT",
                        GiaiThich = "Khám Thai",
                    },

                    new KY_HIEU_CHAM_CONG()
                    {
                        Id = "IL",
                        GiaiThich = "Insurance Leave/ Nghỉ bảo hiểm",
                    },
                    new KY_HIEU_CHAM_CONG()
                    {
                        Id = "NL",
                        GiaiThich = "No report Leave/ Nghỉ không thông báo",
                    },

                    new KY_HIEU_CHAM_CONG()
                    {
                        Id = "EL",
                        GiaiThich = "Early Leave/về sớm",
                    },
                    new KY_HIEU_CHAM_CONG()
                    {
                        Id = "LC",
                        GiaiThich = "Late Come/ Đi muộn",
                    },
                    new KY_HIEU_CHAM_CONG()
                    {
                        Id = "T",
                        GiaiThich = "Nghỉ việc",
                    },
                    new KY_HIEU_CHAM_CONG()
                    {
                        Id = "MD",
                        GiaiThich = "Làm ca ngày chính thức ,kỉ niệm 1-9",
                    },
                    new KY_HIEU_CHAM_CONG()
                    {
                        Id = "PMD",
                        GiaiThich = "Làm ca ngày thử việc,kỉ niệm 1-9",
                    },

                    new KY_HIEU_CHAM_CONG()
                    {
                        Id = "PM",
                        GiaiThich = "Làm ca đêm ngày kỷ niệm 1-9 trước lễ thử việc",
                    },
                    new KY_HIEU_CHAM_CONG()
                    {
                        Id = "BM",
                        GiaiThich = "Làm ca đêm ngày kỷ niệm 1-9 trước lễ chính thức",
                    },
                    new KY_HIEU_CHAM_CONG()
                    {
                        Id = "KDA_OT_05",
                        GiaiThich = "Chấm không đi ăn trong giờ OT",
                    },
                     new KY_HIEU_CHAM_CONG()
                    {
                        Id = "DSBM_OT_150",
                        GiaiThich = "Chấm đi sớm bật máy",
                    },
                });
            }

            if (_context.CA_LVIEC.Count() == 0)
            {
                _context.CA_LVIEC.AddRange(new List<CA_LVIEC>() {

                    // ngay thuong
                    new CA_LVIEC(){
                        Danhmuc_CaLviec = "CN_WHC",// ca ngay
                        DM_NgayLViec = "NT",// ngay thuong
                        TenCa = "Ca sáng ngày thường",
                        Time_BatDau = "08:00:00",
                        Time_BatDau2 = "08:00:00",
                        Time_KetThuc = "17:00:00",
                        Time_KetThuc2 = "17:15:00",
                        HeSo_OT = 100
                    },
                    new CA_LVIEC(){
                        Danhmuc_CaLviec = "CN_WHC",// ot ca ngay
                        DM_NgayLViec = "NT",// ngay thuong
                        TenCa = "Overtime ca ngày, ngày thường",
                        Time_BatDau = "17:30:00",
                        Time_BatDau2 = "17:45:00", // for support
                        Time_KetThuc = "20:00:00",
                        Time_KetThuc2 = "20:00:00",
                        HeSo_OT = 150
                    },
                    new CA_LVIEC(){
                        Danhmuc_CaLviec = "CD_WHC",// ca dem
                        DM_NgayLViec = "NT",// ngay thuong
                        TenCa = "Ca đêm, ngày thường 20h->05h",
                        Time_BatDau = "20:00:00",
                        Time_BatDau2 = "20:00:00",
                        Time_KetThuc = "05:00:00",
                        Time_KetThuc2 = "05:00:00",
                        HeSo_OT = 100
                    },
                    new CA_LVIEC(){
                        Danhmuc_CaLviec = "CD_WHC",// ca dem
                        DM_NgayLViec = "NT",// ngay thuong
                        TenCa = "Ca đêm, ngày thường 5h:6h",
                        Time_BatDau = "05:30:00",
                        Time_BatDau2 = "05:30:00",
                        Time_KetThuc = "06:00:00",
                        Time_KetThuc2 = "06:00:00",
                        HeSo_OT = 200
                    },
                    new CA_LVIEC(){
                        Danhmuc_CaLviec = "CD_WHC",// ca dem
                        DM_NgayLViec = "NT",// ngay thuong
                        TenCa = "Ca đêm, ngày thường 6h->8h",
                        Time_BatDau = "06:00:00",
                        Time_BatDau2 = "06:00:00",
                        Time_KetThuc = "08:00:00",
                        Time_KetThuc2 = "08:00:00",
                        HeSo_OT = 150
                    },

                    // Truoc ngay le
                    new CA_LVIEC(){
                        Danhmuc_CaLviec = "CN_WHC",// ca ngay
                        DM_NgayLViec = "TNL",// truoc ngay le
                        TenCa = "Ca ngày, trước ngày lễ",
                        Time_BatDau = "08:00:00",
                        Time_BatDau2 = "08:00:00",
                        Time_KetThuc = "17:00:00",
                        Time_KetThuc2 = "17:15:00",
                        HeSo_OT = 100
                    },
                    new CA_LVIEC(){
                        Danhmuc_CaLviec = "CN_WHC",// ca ngay
                        DM_NgayLViec = "TNL",// truoc ngay le
                        TenCa = "Overtime Ca ngày, trước ngày lễ",
                        Time_BatDau = "17:30:00",
                        Time_BatDau2 = "17:45:00",
                        Time_KetThuc = "20:00:00",
                        Time_KetThuc2 = "20:00:00",
                        HeSo_OT = 150
                    },
                    new CA_LVIEC(){
                        Danhmuc_CaLviec = "CD_WHC",// ca dem
                        DM_NgayLViec = "TNL",// truoc ngay le
                        TenCa = "Ca đêm, trước ngày lễ 20h->00h",
                        Time_BatDau = "20:00:00",
                        Time_BatDau2 = "20:00:00",
                        Time_KetThuc = "23:59:59",
                        Time_KetThuc2 = "23:59:59",
                        HeSo_OT = 100
                    },
                    new CA_LVIEC(){
                        Danhmuc_CaLviec = "CD_WHC",// ca dem
                        DM_NgayLViec = "TNL",// truoc ngay le
                        TenCa = "Ca đêm, trước ngày lễ 00h->6h",
                        Time_BatDau = "00:00:00",
                        Time_BatDau2 = "00:00:00",
                        Time_KetThuc = "05:30:00",
                        Time_KetThuc2 = "05:30:00",
                        HeSo_OT =390
                    },
                    new CA_LVIEC(){
                        Danhmuc_CaLviec = "CD_WHC",// ca dem
                        DM_NgayLViec = "TNL",// truoc ngay le
                        TenCa = "Ca đêm, trước ngày lễ 6h->8h",
                        Time_BatDau = "06:00:00",
                        Time_BatDau2 = "06:00:00",
                        Time_KetThuc = "08:00:00",
                        Time_KetThuc2 = "08:00:00",
                        HeSo_OT = 300
                    },

                    // Ngay le
                    new CA_LVIEC(){
                        Danhmuc_CaLviec = "CN_WHC",// ca ngay
                        DM_NgayLViec = "NL",//  ngay le
                        TenCa = "Ca ngày, ngày lễ 8h->20h",
                        Time_BatDau = "08:00:00",
                        Time_BatDau2 = "08:00:00",
                        Time_KetThuc = "19:30:00",
                        Time_KetThuc2 = "19:30:00",
                        HeSo_OT = 300
                    },
                    new CA_LVIEC(){
                        Danhmuc_CaLviec = "CD_WHC",// ca đêm
                        DM_NgayLViec = "NL",//  ngay le
                        TenCa = "Ca đem, ngày lễ 20h->22h",
                        Time_BatDau = "20:00:00",
                        Time_BatDau2 = "20:00:00",
                        Time_KetThuc = "22:00:00",
                        Time_KetThuc2 = "22:00:00",
                        HeSo_OT = 300
                    },
                    new CA_LVIEC(){
                        Danhmuc_CaLviec = "CD_WHC",// ca đêm
                        DM_NgayLViec = "NL",// ngay le
                        TenCa = "Ca đem, ngày lễ 22h->6h",
                        Time_BatDau = "22:00:00",
                        Time_BatDau2 = "22:00:00",
                        Time_KetThuc = "05:30:00",
                        Time_KetThuc2 = "05:30:00",
                        HeSo_OT = 390
                    },
                    new CA_LVIEC(){
                        Danhmuc_CaLviec = "CD_WHC",// ca đêm
                        DM_NgayLViec = "NL",// ngay le
                        TenCa = "Ca đem, ngày lễ 6h->8h",
                        Time_BatDau = "06:00:00",
                        Time_BatDau2 = "06:00:00",
                        Time_KetThuc = "08:00:00",
                        Time_KetThuc2 = "08:00:00",
                        HeSo_OT = 300
                    },

                    // ngay le cuoi cung
                    new CA_LVIEC(){
                        Danhmuc_CaLviec = "CN_WHC",// ca ngay
                        DM_NgayLViec = "NLCC",// ngay le cuoi cung
                        TenCa = "Ca ngày, ngày lễ cuối cùng",
                        Time_BatDau = "08:00:00",
                        Time_BatDau2 = "08:00:00",
                        Time_KetThuc = "19:30:00",
                        Time_KetThuc2 = "19:30:00",
                        HeSo_OT = 300
                    },
                     new CA_LVIEC(){
                        Danhmuc_CaLviec = "CD_WHC",// ca đêm
                        DM_NgayLViec = "NLCC",// ngay le cuoi cung
                        TenCa = "Ca đêm, ngày lễ cuối cùng 20h->22h",
                        Time_BatDau = "20:00:00",
                        Time_BatDau2 = "20:00:00",
                        Time_KetThuc = "22:00:00",
                        Time_KetThuc2 = "22:00:00",
                        HeSo_OT = 300
                    },
                     new CA_LVIEC(){
                        Danhmuc_CaLviec = "CD_WHC",// ca đêm
                        DM_NgayLViec = "NLCC",// ngay le cuoi cung
                        TenCa = "Ca đêm, ngày lễ cuối cùng 22h->00h",
                        Time_BatDau = "22:00:00",
                        Time_BatDau2 = "22:00:00",
                        Time_KetThuc = "23:59:59",
                        Time_KetThuc2 = "23:59:59",
                        HeSo_OT = 390
                    },
                     new CA_LVIEC(){
                        Danhmuc_CaLviec = "CD_WHC",// ca đêm
                        DM_NgayLViec = "NLCC",// ngay le cuoi cung
                        TenCa = "Ca đêm, ngày lễ cuối cùng 00h->06h",
                        Time_BatDau = "00:00:00",
                        Time_BatDau2 = "00:00:00",
                        Time_KetThuc = "05:30:00",
                        Time_KetThuc2 = "05:30:00",
                        HeSo_OT = 200
                    },
                     new CA_LVIEC(){
                        Danhmuc_CaLviec = "CD_WHC",// ca đêm
                        DM_NgayLViec = "NLCC",// ngay le cuoi cung
                        TenCa = "Ca đêm, ngày lễ cuối cùng 06h->08h",
                        Time_BatDau = "06:00:00",
                        Time_BatDau2 = "06:00:00",
                        Time_KetThuc = "08:00:00",
                        Time_KetThuc2 = "08:00:00",
                        HeSo_OT = 150
                    },

                     // ngay chu nhat
                      new CA_LVIEC(){
                        Danhmuc_CaLviec = "CN_WHC",// ca ngày
                        DM_NgayLViec = "CN",// ngay chu nhat
                        TenCa = "Ca ngày, ngày chu nhat 08h->20h",
                        Time_BatDau = "08:00:00",
                        Time_BatDau2 = "08:00:00",
                        Time_KetThuc = "20:00:00",
                        Time_KetThuc2 = "20:00:00",
                        HeSo_OT = 200
                    },
                      new CA_LVIEC(){
                        Danhmuc_CaLviec = "CD_WHC",// ca đêm
                        DM_NgayLViec = "CN",// ngay chu nhat
                        TenCa = "Ca đêm, ngày chu nhat 20h->22h",
                        Time_BatDau = "20:00:00",
                        Time_BatDau2 = "20:00:00",
                        Time_KetThuc = "22:00:00",
                        Time_KetThuc2 = "22:00:00",
                        HeSo_OT = 200
                    },
                      new CA_LVIEC(){
                        Danhmuc_CaLviec = "CD_WHC",// ca đêm
                        DM_NgayLViec = "CN",// ngay chu nhat
                        TenCa = "Ca đêm, ngày chu nhat 22h->6h",
                        Time_BatDau = "22:00:00",
                        Time_BatDau2 = "22:00:00",
                        Time_KetThuc = "05:30:00",
                        Time_KetThuc2 = "05:30:00",
                        HeSo_OT = 270
                    },
                       new CA_LVIEC(){
                        Danhmuc_CaLviec = "CD_WHC",// ca đêm
                        DM_NgayLViec = "CN",// ngay chu nhat
                        TenCa = "Ca đêm, ngày chu nhat 06h->8h",
                        Time_BatDau = "06:00:00",
                        Time_BatDau2 = "06:00:00",
                        Time_KetThuc = "08:00:00",
                        Time_KetThuc2 = "08:00:00",
                        HeSo_OT = 200
                    },
                });
            }

            if (_context.SETTING_TIME_DIMUON_VESOM.Count() == 0)
            {
                _context.SETTING_TIME_DIMUON_VESOM.AddRange(new List<SETTING_TIME_DIMUON_VESOM>() {
                    new SETTING_TIME_DIMUON_VESOM()
                    {
                        Danhmuc_CaLviec = "CN_WHC",
                        Time_LateCome = "08:05:00",
                        Time_EarlyLeave = "17:00:00"
                    },
                    new SETTING_TIME_DIMUON_VESOM()
                    {
                        Danhmuc_CaLviec = "CD_WHC",
                        Time_LateCome = "20:05:00",
                        Time_EarlyLeave = "08:00:00"
                    },
                });
            }

            if (_context.NGAY_LE_NAM.Count() == 0)
            {
                _context.NGAY_LE_NAM.AddRange(new List<NGAY_LE_NAM>() {
                    new NGAY_LE_NAM()
                    {
                        Id = "2022-01-01",
                        TenNgayLe="Tết dương lịch",
                        IslastHoliday = "N"
                    },
                     new NGAY_LE_NAM()
                    {
                       Id = "2022-04-30",
                       TenNgayLe="Giải Phóng Miền Nam",
                       IslastHoliday = "N"
                    },
                      new NGAY_LE_NAM()
                    {
                       Id = "2022-05-01",
                       TenNgayLe="Quốc Tế Lao Động",
                       IslastHoliday = "Y"
                    },
                         new NGAY_LE_NAM()
                    {
                       Id = "2022-09-02",
                       TenNgayLe="Lễ Quốc Khánh",
                       IslastHoliday = "N"
                    },
                     new NGAY_LE_NAM()
                    {
                       Id = "2022-04-10",
                       TenNgayLe="Giỗ Tổ Hùng Vương 10/3 âm lịch",
                       IslastHoliday = "N"
                    },
                });
            }

            if (_context.NGAY_DAC_BIET.Count() == 0)
            {
                _context.NGAY_DAC_BIET.AddRange(new List<NGAY_DAC_BIET>()
                {
                    new NGAY_DAC_BIET()
                    {
                        Id="09-01",
                        TenNgayDacBiet = "Ngày kỉ niệm công ty 1-9"
                    }
                });
            }

            if (_context.NGAY_NGHI_BU_LE_NAM.Count() == 0)
            {
                _context.NGAY_NGHI_BU_LE_NAM.AddRange(new List<NGAY_NGHI_BU_LE_NAM>()
                {
                    new NGAY_NGHI_BU_LE_NAM()
                    {
                        NgayNghiBu = "2022-04-11",
                        NoiDungNghi = "Nghỉ bù do Giỗ tổ Hùng Vương là ngày chủ nhật"
                    },
                     new NGAY_NGHI_BU_LE_NAM()
                    {
                        NgayNghiBu = "2022-05-02",
                        NoiDungNghi = "Nghỉ bù do 1-5 là ngày chủ nhật"
                    }
                });
            }

            if (_context.DM_DANGKY_CHAMCONG.Count() == 0)
            {
                _context.DM_DANGKY_CHAMCONG.AddRange(new List<DM_DANGKY_CHAMCONG>() {
                    new DM_DANGKY_CHAMCONG()
                    {
                        TieuDe = "Đăng ký vắng mặt" // id = 1
                    },
                    new DM_DANGKY_CHAMCONG()
                    {
                        TieuDe = "Chấm công làm việc" // id = 4
                    },
                    new DM_DANGKY_CHAMCONG()
                    {
                        TieuDe = "Bổ xung thêm giờ OT" // id = 5
                    }
                });
            }

            if (_context.DANGKY_CHAMCONG_CHITIET.Count() == 0)
            {
                _context.DANGKY_CHAMCONG_CHITIET.AddRange(new List<DANGKY_CHAMCONG_CHITIET>() {
                    new DANGKY_CHAMCONG_CHITIET()
                    {
                       TenChiTiet = "Nghỉ phép",
                       PhanLoaiDM = 1,
                       KyHieuChamCong = "AL"
                    },
                    new DANGKY_CHAMCONG_CHITIET()
                    {
                       TenChiTiet = "Nghỉ bù trực",
                       PhanLoaiDM = 1,
                       KyHieuChamCong = "B"
                    },
                    new DANGKY_CHAMCONG_CHITIET()
                    {
                       TenChiTiet = "Nghỉ bù luân phiên",
                       PhanLoaiDM = 1,
                       KyHieuChamCong = "NB"
                    },
                     new DANGKY_CHAMCONG_CHITIET()
                    {
                       TenChiTiet = "Nghỉ lễ",
                       PhanLoaiDM = 1,
                       KyHieuChamCong = "NH"
                    },
                     new DANGKY_CHAMCONG_CHITIET()
                    {
                       TenChiTiet = " Nghỉ không lương (bao gồm cả thử việc)",
                       PhanLoaiDM = 1,
                       KyHieuChamCong = "UL"
                    },
                     new DANGKY_CHAMCONG_CHITIET()
                    {
                       TenChiTiet = "Làm ca ngày và nghỉ phép 1/2 ngày",
                       PhanLoaiDM = 1,
                       KyHieuChamCong = "AL/DS"
                    },
                     new DANGKY_CHAMCONG_CHITIET()
                    {
                       TenChiTiet = "Làm ca đêm và nghỉ phép 1/2 ngày",
                       PhanLoaiDM = 1,
                       KyHieuChamCong = "AL/NS"
                    },
                    new DANGKY_CHAMCONG_CHITIET()
                    {
                       TenChiTiet = "Làm ca ngày và nghỉ không lương 1/2 ngày",
                       PhanLoaiDM = 1,
                       KyHieuChamCong = "UL/DS"
                    },
                    new DANGKY_CHAMCONG_CHITIET()
                    {
                       TenChiTiet = "Làm ca đêm và nghỉ không lương 1/2 ngày",
                       PhanLoaiDM = 1,
                       KyHieuChamCong = "UL/NS"
                    },
                     new DANGKY_CHAMCONG_CHITIET()
                    {
                       TenChiTiet = "Nghỉ đặc biệt (nghi huong luong)",
                       PhanLoaiDM = 1,
                       KyHieuChamCong = "SL"
                    },
                      new DANGKY_CHAMCONG_CHITIET()
                    {
                       TenChiTiet = "Nghỉ ốm hưởng lương theo quy định cty",
                       PhanLoaiDM = 1,
                       KyHieuChamCong = "HL"
                    },
                    new DANGKY_CHAMCONG_CHITIET()
                    {
                       TenChiTiet = "Nghỉ hưởng 70% lương vùng",
                       PhanLoaiDM = 1,
                       KyHieuChamCong = "L70"
                    },
                     new DANGKY_CHAMCONG_CHITIET()
                    {
                       TenChiTiet = "Nghỉ bảo hiểm",
                       PhanLoaiDM = 1,
                       KyHieuChamCong = "IL"
                    },
                    new DANGKY_CHAMCONG_CHITIET()
                    {
                       TenChiTiet = "Nghỉ không thông báo",
                       PhanLoaiDM = 1,
                       KyHieuChamCong = "NL"
                    },
                    new DANGKY_CHAMCONG_CHITIET()
                    {
                       TenChiTiet = "Nghỉ việc",
                       PhanLoaiDM = 1,
                       KyHieuChamCong = "T"
                    },

                     new DANGKY_CHAMCONG_CHITIET()
                    {
                       TenChiTiet = "Nghỉ 1/2 ca đêm trước ngày lễ và làm đầu ca đêm trước ngày lễ (thử việc)",
                       PhanLoaiDM = 1,
                       KyHieuChamCong = "PH/F"
                    },
                      new DANGKY_CHAMCONG_CHITIET()
                    {
                       TenChiTiet = "Nghỉ 1/2 ca đêm trước ngày lễ và làm cuối ca đêm trước ngày lễ (thử việc)",
                       PhanLoaiDM = 1,
                       KyHieuChamCong = "PH/L"
                    },
                     new DANGKY_CHAMCONG_CHITIET()
                    {
                       TenChiTiet = "Khám Thai",
                       PhanLoaiDM = 1,
                       KyHieuChamCong = "KT"
                    },
                    new DANGKY_CHAMCONG_CHITIET()
                    {
                       TenChiTiet = "Làm ca ngày và nghỉ 1/2 ngày thử việc",
                       PhanLoaiDM = 1,
                       KyHieuChamCong = "P/DS"
                    },
                     new DANGKY_CHAMCONG_CHITIET()
                    {
                       TenChiTiet = "Làm ca đêm và nghỉ 1/2 ca đêm thử việc",
                       PhanLoaiDM = 1,
                       KyHieuChamCong = "P/NS"
                    }, new DANGKY_CHAMCONG_CHITIET()
                    {
                       TenChiTiet = "Đi Công tác",
                       PhanLoaiDM = 1,
                       KyHieuChamCong = "CT"
                    },
                     new DANGKY_CHAMCONG_CHITIET()
                    {
                       TenChiTiet = "Làm đầu ca đêm trước ngày lễ và nghỉ phép 1/2 ngày",
                       PhanLoaiDM = 1,
                       KyHieuChamCong = "AL/BF"
                    },
                     new DANGKY_CHAMCONG_CHITIET()
                    {
                       TenChiTiet = "Làm cuối ca đêm trước ngày lễ và nghỉ phép 1/2 ngày",
                       PhanLoaiDM = 1,
                       KyHieuChamCong = "AL/BL"
                    },
                    new DANGKY_CHAMCONG_CHITIET()
                    {
                       TenChiTiet = "Làm đầu ca đêm trước ngày lễ và nghỉ không lương 1/2 ngày",
                       PhanLoaiDM = 1,
                       KyHieuChamCong = "UL/BF"
                    },
                    new DANGKY_CHAMCONG_CHITIET()
                    {
                       TenChiTiet = "Làm cuối ca đêm trước ngày lễ và nghỉ không lương 1/2 ngày",
                       PhanLoaiDM = 1,
                       KyHieuChamCong = "UL/BL"
                    },
                    new DANGKY_CHAMCONG_CHITIET()
                    {
                       TenChiTiet = "Early Leave",
                       PhanLoaiDM = 6,
                       KyHieuChamCong = "EL"
                    },
                    new DANGKY_CHAMCONG_CHITIET()
                    {
                       TenChiTiet = "Late Come",
                       PhanLoaiDM = 6,
                       KyHieuChamCong = "LC"
                    },
                    new DANGKY_CHAMCONG_CHITIET()
                    {
                       TenChiTiet = "Chấm không đi ăn trong giờ OT",
                       PhanLoaiDM = 3,
                       KyHieuChamCong = "KDA_OT_05"
                    },
                    new DANGKY_CHAMCONG_CHITIET()
                    {
                       TenChiTiet = "Thử việc ca ngày",
                       PhanLoaiDM = 4,
                       KyHieuChamCong = "PD"
                    },
                     new DANGKY_CHAMCONG_CHITIET()
                    {
                       TenChiTiet = "Thử việc ca đêm",
                       PhanLoaiDM = 4,
                       KyHieuChamCong = "PN"
                    },
                    new DANGKY_CHAMCONG_CHITIET()
                    {
                       TenChiTiet = "Thử việc làm thêm ngày chủ nhật/ Probation",
                       PhanLoaiDM = 4,
                       KyHieuChamCong = "TV"
                    },
                    new DANGKY_CHAMCONG_CHITIET()
                    {
                       TenChiTiet = "Thử việc làm thêm ca đêm chủ nhật",
                       PhanLoaiDM = 4,
                       KyHieuChamCong = "TVD"
                    },
                     new DANGKY_CHAMCONG_CHITIET()
                    {
                       TenChiTiet = "Thử việc làm đêm ngày lễ",
                       PhanLoaiDM = 4,
                       KyHieuChamCong = "PDD"
                    },
                    new DANGKY_CHAMCONG_CHITIET()
                    {
                       TenChiTiet = "Làm ca đêm trước ngày lễ( thử việc)",
                       PhanLoaiDM = 4,
                       KyHieuChamCong = "PH"
                    },
                    new DANGKY_CHAMCONG_CHITIET()
                    {
                       TenChiTiet = "Day Shift/ Ca ngày",
                       PhanLoaiDM = 4,
                       KyHieuChamCong = "DS"
                    },
                    new DANGKY_CHAMCONG_CHITIET()
                    {
                       TenChiTiet = " Night Shift/ Ca đêm",
                       PhanLoaiDM = 4,
                       KyHieuChamCong = "NS"
                    },
                    new DANGKY_CHAMCONG_CHITIET()
                    {
                       TenChiTiet = "Làm ca đêm chủ nhật",
                       PhanLoaiDM = 4,
                       KyHieuChamCong = "D"
                    },
                    new DANGKY_CHAMCONG_CHITIET()
                    {
                       TenChiTiet = "Làm ca đêm ngày lễ",
                       PhanLoaiDM = 4,
                       KyHieuChamCong = "NHD"
                    },
                     new DANGKY_CHAMCONG_CHITIET()
                    {
                       TenChiTiet = "Làm ca đêm trước ngày lễ( chính thức)",
                       PhanLoaiDM = 4,
                       KyHieuChamCong = "BH"
                    },
                    new DANGKY_CHAMCONG_CHITIET()
                    {
                       TenChiTiet = "Làm ca ngày chính thức, ngay ki niem",
                       PhanLoaiDM = 4,
                       KyHieuChamCong = "MD"
                    },  new DANGKY_CHAMCONG_CHITIET()
                    {
                       TenChiTiet = "Làm ca ngày thử việc, ngay ki niem",
                       PhanLoaiDM = 4,
                       KyHieuChamCong = "PMD"
                    },
                     new DANGKY_CHAMCONG_CHITIET()
                    {
                       TenChiTiet = "Làm ca đêm ngày kỷ niệm trước lễ thử việc",
                       PhanLoaiDM = 4,
                       KyHieuChamCong = "PM"
                    },
                    new DANGKY_CHAMCONG_CHITIET()
                    {
                       TenChiTiet = "Làm ca đêm ngày kỷ niệm trước lễ chính thức",
                       PhanLoaiDM = 4,
                       KyHieuChamCong = "BM"
                    },
                    new DANGKY_CHAMCONG_CHITIET()
                    {
                       TenChiTiet = "Chấm công đi sớm bật máy",
                       PhanLoaiDM = 5,
                       KyHieuChamCong = "DSBM_OT_150"
                    },
                });
            }

            await _context.SaveChangesAsync();
        }
    }
}
