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
                    ShowPass = "123654$"
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

            await _context.SaveChangesAsync();
        }
    }
}
