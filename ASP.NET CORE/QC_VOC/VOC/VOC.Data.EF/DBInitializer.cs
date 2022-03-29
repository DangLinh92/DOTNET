using VOC.Data.Entities;
using VOC.Data.Enums;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VOC.Data.EF
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
                    Name = "Manager",
                    NormalizedName = "Manager",
                    Description = "Manager"
                });
                await _roleManager.CreateAsync(new APP_ROLE()
                {
                    Name = "Staff",
                    NormalizedName = "Staff",
                    Description = "Staff"
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
                    Avatar = "/img/user.jpg"
                }, "123654$");
                var user = await _userManager.FindByNameAsync("admin");
                await _userManager.AddToRoleAsync(user, "Admin");

                await _userManager.CreateAsync(new APP_USER()
                {
                    UserName = "H2105001",
                    FullName = "Le Van Dang",
                    Email = "WH2105001@wisol.co.kr",
                    ShowPass = "543120$",
                    Avatar = "/img/user.jpg"
                }, "543120$");
                var user1 = await _userManager.FindByNameAsync("H2105001");
                await _userManager.AddToRoleAsync(user1, "Staff");

                await _userManager.CreateAsync(new APP_USER()
                {
                    UserName = "H2202060",
                    FullName = "Bui Nhu Huan",
                    Email = "wh2202060@wisol.co.kr",
                    ShowPass = "wisol@123",
                    Avatar = "/img/user.jpg"
                }, "wisol@123");
                var user2 = await _userManager.FindByNameAsync("H2202060");
                await _userManager.AddToRoleAsync(user2, "Manager");

                await _userManager.CreateAsync(new APP_USER()
                {
                    UserName = "QC123",
                    FullName = "QC123",
                    Email = "qc123@wisol.co.kr",
                    ShowPass = "wisol@123",
                    Avatar = "/img/user.jpg"
                }, "wisol@123");
                var user3 = await _userManager.FindByNameAsync("QC123");
                await _userManager.AddToRoleAsync(user3, "Staff");
            }

            if (_context.Functions.Count() == 0)
            {
                _context.Functions.AddRange(new List<FUNCTION>()
                {
                    new FUNCTION() {Id = "HOME", Name = "Home",ParentId = null,SortOrder = 1,Status = Status.Active,URL = "/Admin/Home/Index",IconCss = "la la-dashboard"  },
                });
            }

            await _context.SaveChangesAsync();
        }
    }
}
