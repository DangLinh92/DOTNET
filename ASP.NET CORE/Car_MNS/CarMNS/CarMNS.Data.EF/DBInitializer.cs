using CarMNS.Data.Entities;
using CarMNS.Data.Enums;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarMNS.Data.EF
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
                    Name = "HR_GA",
                    NormalizedName = "HR_GA",
                    Description = "Điều Xe"
                });

                await _roleManager.CreateAsync(new APP_ROLE()
                {
                    Name = "GrLeader",
                    NormalizedName = "GroupLeader",
                    Description = "Sếp Hàn Duyệt"
                });

                await _roleManager.CreateAsync(new APP_ROLE()
                {
                    Name = "Register",
                    NormalizedName = "Register",
                    Description = "Người Đăng Ký"
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
                    UserName = "GA",
                    FullName = "HR_GA",
                    Email = "HR_GA@gmail.com",
                    ShowPass = "123654$",
                    Avatar = "/img/profiles/avatar-01.jpg"
                }, "123654$");
                var HR_GA = await _userManager.FindByNameAsync("GA");
                await _userManager.AddToRoleAsync(HR_GA, "HR_GA");

                await _userManager.CreateAsync(new APP_USER()
                {
                    UserName = "ApproveBP",
                    FullName = "ApproveBP",
                    Email = "ApproveBP@gmail.com",
                    ShowPass = "123654$",
                    Avatar = "/img/profiles/avatar-01.jpg"
                }, "123654$");
                var ApproveBP = await _userManager.FindByNameAsync("ApproveBP");
                await _userManager.AddToRoleAsync(ApproveBP, "GrLeader");

                await _userManager.CreateAsync(new APP_USER()
                {
                    UserName = "Register",
                    FullName = "Register",
                    Email = "Register@gmail.com",
                    ShowPass = "543120$",
                    Avatar = "/img/profiles/avatar-01.jpg"
                }, "543120$");
                var user1 = await _userManager.FindByNameAsync("HRSub");
                await _userManager.AddToRoleAsync(user1, "Register");
            }

            if (_context.Functions.Count() == 0)
            {
                _context.Functions.AddRange(new List<FUNCTION>()
                {
                    new FUNCTION() {Id = "CAR_MNS", Name = "Car Management",ParentId = null,SortOrder = 1,Status = Status.Active,URL = "",IconCss = "",Area="admin"  },
                    new FUNCTION() {Id = "DANGKY", Name = "Đăng Ký/Duyệt Xe",ParentId = "CAR_MNS",SortOrder = 1,Status = Status.Active,URL = "",IconCss = "la la-share-alt" ,Area="admin"  },
                    new FUNCTION() {Id = "LICHSU_DIEU_XE", Name = "Lịch Sử Điều Xe",ParentId = "CAR_MNS",SortOrder = 2,Status = Status.Active,URL = "",IconCss = "" ,Area="admin"  },
                    new FUNCTION() {Id = "CARS_DRIVE", Name = "Danh Sách Xe/Lái Xe",ParentId = "CAR_MNS",SortOrder = 3,Status = Status.Active,URL = "",IconCss = "" ,Area="admin"  },

                    new FUNCTION() {Id = "OTHER",Name = "Others",ParentId = null,SortOrder = 2,Status = Status.Active,URL = "",IconCss = "",Area="admin"   },
                    new FUNCTION() {Id = "SETTINGS",Name = "Settings",ParentId = "OTHER",SortOrder = 1,Status = Status.Active,URL = "/admin/settings/index",IconCss = "la la-cog",Area="admin"   },
                    new FUNCTION() {Id = "ROLE_PERMISSTION",Name = "Phân Quyền User",ParentId = "SETTINGS",SortOrder = 1,Status = Status.Active,URL = "/admin/roleandpermisstion/index",IconCss = "la la-key",Area="admin"   },
                });
            }

            await _context.SaveChangesAsync();
        }
    }
}
