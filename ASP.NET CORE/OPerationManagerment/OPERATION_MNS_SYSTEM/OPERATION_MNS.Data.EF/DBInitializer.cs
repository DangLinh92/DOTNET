using OPERATION_MNS.Data.Entities;
using OPERATION_MNS.Data.Enums;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPERATION_MNS.Data.EF
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
                    new FUNCTION() {Id = "DMKEHOACH", Name = "Danh Mục Kế Hoạch",ParentId = "KEHOACH",SortOrder = 1,Status = Status.Active,URL = "/OpeationMns/ehsdanhmuckehoach/index",IconCss = ""  },
                    new FUNCTION() {Id = "KEHOACHTHEONAM", Name = "Kế Hoạch Theo Năm",ParentId = "KEHOACH",SortOrder = 2,Status = Status.Active,URL = "/OpeationMns/kehoachtheonam/index",IconCss = ""  },
                    new FUNCTION() {Id = "TONGHOPKEHOACH", Name = "Tổng hợp Kế Hoạch",ParentId = "KEHOACH",SortOrder = 3,Status = Status.Active,URL = "/OpeationMns/tonghopkehoach/index",IconCss = ""  },
                    new FUNCTION() {Id = "DOCUMENT", Name = "Tài Liệu",ParentId = "EHS",SortOrder =2,Status = Status.Active,URL = "/OpeationMns/ehsdocument/index",IconCss = "la la-file-text"  },

                    new FUNCTION() {Id = "HR", Name = "HR",ParentId = null,SortOrder = 2,Status = Status.Active,URL = "",IconCss = ""  },
                    new FUNCTION() {Id = "NHANSU", Name = "Nhân Sự",ParentId = "HR",SortOrder = 1,Status = Status.Active,URL = "",IconCss = "la la-user"  },
                    new FUNCTION() {Id = "NHANVIEN", Name = "Danh Sách Nhân Viên",ParentId = "NHANSU",SortOrder = 1,Status = Status.Active,URL = "/OpeationMns/nhanvien/index",IconCss = ""  },
                    new FUNCTION() {Id = "NHANVIEN_NGHIVIEC", Name = "Danh Sách Nghỉ Việc",ParentId = "NHANSU",SortOrder = 2,Status = Status.Active,URL = "/OpeationMns/nhanvien/nhanviennghiviec",IconCss = ""  },

                    new FUNCTION() {Id = "CHAMCONG", Name = "Chấm Công",ParentId = "HR",SortOrder = 2,Status = Status.Active,URL = "",IconCss = "la la-info"  },
                    new FUNCTION() {Id = "SHIFT_SCHEDULE", Name = "Phân ca",ParentId = "CHAMCONG",SortOrder = 1,Status = Status.Active,URL = "/OpeationMns/nhanvien_calamviec/index",IconCss = ""  },
                    new FUNCTION() {Id = "REGIS_TIMEKEEPING", Name = "Chấm công bổ sung",ParentId = "CHAMCONG",SortOrder = 2,Status = Status.Active,URL = "/OpeationMns/chamcongdacbiet/index",IconCss = ""  },
                    new FUNCTION() {Id = "OVERTIME", Name = "Đăng ký tăng ca",ParentId = "CHAMCONG",SortOrder = 3,Status = Status.Active,URL = "/OpeationMns/dangkyot/index",IconCss = ""  },
                    new FUNCTION() {Id = "THAISAN", Name = "Thai sản",ParentId = "CHAMCONG",SortOrder = 4,Status = Status.Active,URL = "/OpeationMns/NhanVienThaiSan/index",IconCss = ""  },
                    new FUNCTION() {Id = "TIMEKEEPING", Name = "Dữ liệu vào/ra",ParentId = "CHAMCONG",SortOrder = 5,Status = Status.Active,URL = "/OpeationMns/chamcong/index",IconCss = ""  },
                    new FUNCTION() {Id = "ATTENDANCE_RECORD", Name = "Bảng Công",ParentId = "CHAMCONG",SortOrder = 6,Status = Status.Active,URL = "/OpeationMns/bangcong/index",IconCss = ""  },
                    
                    new FUNCTION() {Id = "GA",Name = "GA",ParentId = null,SortOrder = 3,Status = Status.Active,URL = "",IconCss = ""  },
                    new FUNCTION() {Id = "COST",Name = "Cost",ParentId = "GA",SortOrder = 1,Status = Status.Active,URL = "/OpeationMns/cost/index",IconCss = "la la-money"  },

                    new FUNCTION() {Id = "OTHER",Name = "Others",ParentId = null,SortOrder = 5,Status = Status.Active,URL = "",IconCss = ""  },
                    new FUNCTION() {Id = "DOCUMENT_SUB",Name = "Documents",ParentId = "OTHER",SortOrder = 1,Status = Status.Active,URL = "/OpeationMns/documentall/index",IconCss = "la la-file-text"  },
                    new FUNCTION() {Id = "CALENDAR",Name = "Calendar",ParentId = "OTHER",SortOrder = 2,Status = Status.Active,URL = "/OpeationMns/calendar/index",IconCss = "la la-table"  },
                    new FUNCTION() {Id = "SETTINGS",Name = "Settings",ParentId = "OTHER",SortOrder = 3,Status = Status.Active,URL = "/OpeationMns/settings/index",IconCss = "la la-cog"  },
                    new FUNCTION() {Id = "ROLE_PERMISSTION",Name = "Phân Quyền User",ParentId = "SETTINGS",SortOrder = 1,Status = Status.Active,URL = "/OpeationMns/roleandpermisstion/index",IconCss = "la la-key"  },

                     new FUNCTION() {Id = "DAO_TAO_EVENT", Name = "Performance",ParentId = null,SortOrder = 4,Status = Status.Active,URL = "",IconCss = ""  },
                new FUNCTION() {Id = "TRAINING",Name = "Đào Tạo",ParentId = "DAO_TAO_EVENT",SortOrder = 1,Status = Status.Active,URL = "",IconCss = "la la-edit"  },
                new FUNCTION() {Id = "TRAINING_LIST",Name = "Kế Hoạch Đào Tạo",ParentId = "TRAINING",SortOrder = 1,Status = Status.Active,URL = "/OpeationMns/traininglist/index",IconCss = ""  },
                new FUNCTION() {Id = "TRAINING_TYPE",Name = "Danh Mục Đào Tạo",ParentId = "TRAINING",SortOrder = 2,Status = Status.Active,URL = "/OpeationMns/trainingtype/index",IconCss = ""  },
                });
            }

            await _context.SaveChangesAsync();
        }
    }
}
