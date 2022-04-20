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
                    new FUNCTION() {Id = "Main", Name = "Main",ParentId = null,SortOrder = 1,Status = Status.Active,URL = "",IconCss = ""  },
                    new FUNCTION() {Id = "VOC", Name = "VOC Report",ParentId = "Main",SortOrder = 1,Status = Status.Active,URL = "/admin/Voc/Index",IconCss = "la la-dashboard"  },
                    new FUNCTION() {Id = "K1", Name = "K1 불량율",ParentId = "Main",SortOrder = 2,Status = Status.Active,URL = "/admin/k1/Index",IconCss = "la la-cube"  },
                    new FUNCTION() {Id = "Onsite", Name = "Onsite 불량마킹정보",ParentId = "Main",SortOrder = 3,Status = Status.Active,URL = "/Admin/onsite/Index",IconCss = "la la-edit"  },
                    new FUNCTION() {Id = "Upload", Name = "Upload VOC",ParentId = "Main",SortOrder = 4,Status = Status.Active,URL = "/Admin/ppm/Index",IconCss = "la la-object-group"  },
                    new FUNCTION() {Id = "uVoc", Name = "VOC",ParentId = "Upload",SortOrder = 1,Status = Status.Active,URL = "/admin/Voc/UploadList",IconCss = ""  },
                    new FUNCTION() {Id = "uK1", Name = "K1 불량율",ParentId = "Upload",SortOrder = 2,Status = Status.Active,URL = "/admin/k1/UploadList",IconCss = ""  },
                    new FUNCTION() {Id = "uOnsite", Name = "Onsite",ParentId = "Upload",SortOrder = 3,Status = Status.Active,URL = "/admin/Onsite/UploadList",IconCss = ""  },
                });
            }

            if (_context.VocPPM.Count() == 0)
            {
                _context.VocPPM.AddRange(new List<VOC_PPM>() { 
                    // CSP
                    // SEVT
                    new VOC_PPM()
                    {
                        Year = 2022,
                        Month = 1,
                        Module = "CSP",
                        Customer = "SEVT",
                        Type = "Input",
                        Value = 1933047,
                        TargetValue = 5
                    },
                    new VOC_PPM()
                    {
                        Year = 2022,
                        Month = 2,
                        Module = "CSP",
                        Customer = "SEVT",
                        Type = "Input",
                        Value = 575613,
                        TargetValue = 5
                    },
                    new VOC_PPM()
                    {
                        Year = 2022,
                        Month = 3,
                        Module = "CSP",
                        Customer = "SEVT",
                        Type = "Input",
                        Value = 7741296,
                        TargetValue = 5
                    },

                    new VOC_PPM()
                    {
                        Year = 2022,
                        Month = 1,
                        Module = "CSP",
                        Customer = "SEVT",
                        Type = "Defect",
                        Value = 11,
                        TargetValue = 5
                    },
                    new VOC_PPM()
                    {
                        Year = 2022,
                        Month = 2,
                        Module = "CSP",
                        Customer = "SEVT",
                        Type = "Defect",
                        Value = 2,
                        TargetValue = 5
                    },
                    new VOC_PPM()
                    {
                        Year = 2022,
                        Month = 3,
                        Module = "CSP",
                        Customer = "SEVT",
                        Type = "Defect",
                        Value = 42,
                        TargetValue = 5
                    },

                    // SEV
                     new VOC_PPM()
                    {
                        Year = 2022,
                        Month = 1,
                        Module = "CSP",
                        Customer = "SEV",
                        Type = "Input",
                        Value = 1542788,
                        TargetValue = 5
                    },
                    new VOC_PPM()
                    {
                        Year = 2022,
                        Month = 2,
                        Module = "CSP",
                        Customer = "SEV",
                        Type = "Input",
                        Value = 1149092,
                        TargetValue = 5
                    },
                    new VOC_PPM()
                    {
                        Year = 2022,
                        Month = 3,
                        Module = "CSP",
                        Customer = "SEV",
                        Type = "Input",
                        Value = 443786,
                        TargetValue = 5
                    },

                    new VOC_PPM()
                    {
                        Year = 2022,
                        Month = 1,
                        Module = "CSP",
                        Customer = "SEV",
                        Type = "Defect",
                        Value = 3,
                        TargetValue = 5
                    },
                    new VOC_PPM()
                    {
                        Year = 2022,
                        Month = 2,
                        Module = "CSP",
                        Customer = "SEV",
                        Type = "Defect",
                        Value = 4,
                        TargetValue = 5
                    },
                    new VOC_PPM()
                    {
                        Year = 2022,
                        Month = 3,
                        Module = "CSP",
                        Customer = "SEV",
                        Type = "Defect",
                        Value = 1,
                        TargetValue = 5
                    },

                    // ------------
                    // LFEM
                    // SEVT
                    new VOC_PPM()
                    {
                        Year = 2022,
                        Month = 1,
                        Module = "LFEM",
                        Customer = "SEVT",
                        Type = "Input",
                        Value = 3883385,
                        TargetValue = 5
                    },
                    new VOC_PPM()
                    {
                        Year = 2022,
                        Month = 2,
                        Module = "LFEM",
                        Customer = "SEVT",
                        Type = "Input",
                        Value = 3709188,
                        TargetValue = 5
                    },
                    new VOC_PPM()
                    {
                        Year = 2022,
                        Month = 3,
                        Module = "LFEM",
                        Customer = "SEVT",
                        Type = "Input",
                        Value = 7708822,
                        TargetValue = 5
                    },

                    new VOC_PPM()
                    {
                        Year = 2022,
                        Month = 1,
                        Module = "LFEM",
                        Customer = "SEVT",
                        Type = "Defect",
                        Value = 29,
                        TargetValue = 5
                    },
                    new VOC_PPM()
                    {
                        Year = 2022,
                        Month = 2,
                        Module = "LFEM",
                        Customer = "SEVT",
                        Type = "Defect",
                        Value = 17,
                        TargetValue = 5
                    },
                    new VOC_PPM()
                    {
                        Year = 2022,
                        Month = 3,
                        Module = "LFEM",
                        Customer = "SEVT",
                        Type = "Defect",
                        Value = 82,
                        TargetValue = 5
                    },

                    // SEV
                     new VOC_PPM()
                    {
                        Year = 2022,
                        Month = 1,
                        Module = "LFEM",
                        Customer = "SEV",
                        Type = "Input",
                        Value = 1711854,
                        TargetValue = 5
                    },
                    new VOC_PPM()
                    {
                        Year = 2022,
                        Month = 2,
                        Module = "LFEM",
                        Customer = "SEV",
                        Type = "Input",
                        Value = 1765679,
                        TargetValue = 5
                    },
                    new VOC_PPM()
                    {
                        Year = 2022,
                        Month = 3,
                        Module = "LFEM",
                        Customer = "SEV",
                        Type = "Input",
                        Value = 1636533,
                        TargetValue = 5
                    },

                    new VOC_PPM()
                    {
                        Year = 2022,
                        Month = 1,
                        Module = "LFEM",
                        Customer = "SEV",
                        Type = "Defect",
                        Value = 7,
                        TargetValue = 5
                    },
                    new VOC_PPM()
                    {
                        Year = 2022,
                        Month = 2,
                        Module = "LFEM",
                        Customer = "SEV",
                        Type = "Defect",
                        Value = 4,
                        TargetValue = 5
                    },
                    new VOC_PPM()
                    {
                        Year = 2022,
                        Month = 3,
                        Module = "LFEM",
                        Customer = "SEV",
                        Type = "Defect",
                        Value = 5,
                        TargetValue = 5
                    },
                });
            }

            if (_context.VocPPMYear.Count() == 0)
            {
                _context.VocPPMYear.AddRange(new List<VOC_PPM_YEAR>()
                {
                    new VOC_PPM_YEAR()
                    {
                        Module = "CSP",
                        Year = 2019,
                        ValuePPM = 0,
                        TargetPPM = 0
                    },
                    new VOC_PPM_YEAR()
                    {
                        Module = "CSP",
                        Year = 2020,
                        ValuePPM = 14,
                        TargetPPM = 14
                    },
                    new VOC_PPM_YEAR()
                    {
                        Module = "CSP",
                        Year = 2021,
                        ValuePPM = 8.6,
                        TargetPPM = 12
                    },

                    new VOC_PPM_YEAR()
                    {
                        Module = "LFEM",
                        Year = 2019,
                        ValuePPM = 8.4,
                        TargetPPM = 9.4
                    },
                    new VOC_PPM_YEAR()
                    {
                        Module = "LFEM",
                        Year = 2020,
                        ValuePPM = 6.9,
                        TargetPPM = 8
                    },
                    new VOC_PPM_YEAR()
                    {
                        Module = "LFEM",
                        Year = 2021,
                        ValuePPM = 6.6,
                        TargetPPM = 8
                    }
                });
            }

            if (_context.VocOnsite.Count() == 0)
            {
                _context.VocOnsite.AddRange(new List<VOC_ONSITE>() { 

                    // SEV 
                    new VOC_ONSITE()
                    {
                        Month = 3,
                        Week = "W10",
                        Date = "2022-03-11",
                        Customer = "SEV",
                        Part = "LFEM",
                        Wisol_Model = "L7E0",
                        Customer_Code = "2911-000430",
                        Marking = "L7E0YW2215",
                        ProductionDate = "2022-02-15",
                        SetModel = "A326",
                        OK = "OK",
                        Qty = 1,
                        Result = "OK"
                    },
                    new VOC_ONSITE()
                    {
                        Month = 3,
                        Week = "W10",
                        Date = "2022-03-11",
                        Customer = "SEV",
                        Part = "LFEM",
                        Wisol_Model = "R9Z0",
                        Customer_Code = "2911-000504",
                        Marking = "R9ZFW1C7",
                        ProductionDate = "2021-12-07",
                        SetModel = "A325",
                        OK = "OK",
                        Qty = 1,
                        Result = "OK"
                    },
                    new VOC_ONSITE()
                    {
                        Month = 3,
                        Week = "W10",
                        Date = "2022-03-11",
                        Customer = "SEV",
                        Part = "LFEM",
                        Wisol_Model = "R9Z0",
                        Customer_Code = "2911-000504",
                        Marking = "R9ZRW218",
                        ProductionDate = "2022-01-08",
                        SetModel = "A325",
                        OK = "OK",
                        Qty = 1,
                        Result = "OK"
                    },
                    new VOC_ONSITE()
                    {
                        Month = 3,
                        Week = "W10",
                        Date = "2022-03-11",
                        Customer = "SEV",
                        Part = "LFEM",
                        Wisol_Model = "R9Z0",
                        Customer_Code = "2911-000504",
                        Marking = "R9ZDW21P",
                        ProductionDate = "2022-01-25",
                        SetModel = "A325",
                        OK = "OK",
                        Qty = 1,
                        Result = "OK"
                    },
                    new VOC_ONSITE()
                    {
                        Month = 3,
                        Week = "W11",
                        Date = "2022-03-18",
                        Customer = "SEV",
                        Part = "CSP",
                        Wisol_Model = "SFXG50CY902",
                        Customer_Code = "2910-000385",
                        Marking = "FLQZ o9",
                        ProductionDate = "2022-02-09",
                        SetModel = "A135",
                        OK = "OK",
                        Qty = 1,
                        Result = "OK"
                    },
                    new VOC_ONSITE()
                    {
                        Month = 3,
                        Week = "W11",
                        Date = "2022-03-18",
                        Customer = "SEV",
                        Part = "CSP",
                        Wisol_Model = "SFX836DYJ02",
                        Customer_Code = "2910-000391",
                        Marking = "ZrQZ oE",
                        ProductionDate = "2022-02-14",
                        SetModel = "F1001",
                        OK = "OK",
                        Qty = 1,
                        Result = "OK"
                    },
                    new VOC_ONSITE()
                    {
                        Month = 3,
                        Week = "W11",
                        Date = "2022-03-18",
                        Customer = "SEV",
                        Part = "CSP",
                        Wisol_Model = "SFX836DYJ02",
                        Customer_Code = "2910-000391",
                        Marking = "ZrQZ 05",
                        ProductionDate = "2022-02-05",
                        SetModel = "A135F",
                        OK = "OK",
                        Qty = 1,
                        Result = "OK"
                    },
                    new VOC_ONSITE()
                    {
                        Month = 3,
                        Week = "W11",
                        Date = "2022-03-18",
                        Customer = "SEV",
                        Part = "CSP",
                        Wisol_Model = "SFX836DYJ02",
                        Customer_Code = "2910-000391",
                        Marking = "ZrQZ 0F",
                        ProductionDate = "2022-02-15",
                        SetModel = "A135F",
                        OK = "OK",
                        Qty = 1,
                        Result = "OK"
                    },
                    new VOC_ONSITE()
                    {
                        Month = 3,
                        Week = "W13",
                        Date = "2022-03-30",
                        Customer = "SEV",
                        Part = "LFEM",
                        Wisol_Model = "L7E0",
                        Customer_Code = "2911-000430",
                        Marking = "L7E0AW2305",
                        ProductionDate = "2022-03-05",
                        SetModel = "A325",
                        OK = "OK",
                        Qty = 1,
                        Result = "OK"
                    },
                    new VOC_ONSITE()
                    {
                        Month = 3,
                        Week = "W13",
                        Date = "2022-03-30",
                        Customer = "SEV",
                        Part = "LFEM",
                        Wisol_Model = "L7E0",
                        Customer_Code = "2911-000430",
                        Marking = "L7E09W2306",
                        ProductionDate = "2022-03-06",
                        SetModel = "A325",
                        OK = "OK",
                        Qty = 1,
                        Result = "OK"
                    },
                    new VOC_ONSITE()
                    {
                        Month = 3,
                        Week = "W13",
                        Date = "2022-03-30",
                        Customer = "SEV",
                        Part = "LFEM",
                        Wisol_Model = "R9Z0",
                        Customer_Code = "2911-000504",
                        Marking = "R9ZEW22D",
                        ProductionDate = "2022-02-13",
                        SetModel = "A326",
                        OK = "OK",
                        Qty = 1,
                        Result = "OK"
                    },
                    new VOC_ONSITE()
                    {
                        Month = 3,
                        Week = "W13",
                        Date = "2022-03-30",
                        Customer = "SEV",
                        Part = "LFEM",
                        Wisol_Model = "R9Z0",
                        Customer_Code = "2911-000504",
                        Marking = "R9Z8W21H",
                        ProductionDate = "2022-01-17",
                        SetModel = "A326",
                        OK = "OK",
                        Qty = 1,
                        Result = "OK"
                    },
                    new VOC_ONSITE()
                    {
                        Month = 3,
                        Week = "W13",
                        Date = "2022-03-30",
                        Customer = "SEV",
                        Part = "LFEM",
                        Wisol_Model = "L5E0",
                        Customer_Code = "2911-000487",
                        Marking = "L5ECW217",
                        ProductionDate = "2022-01-07",
                        SetModel = "A536",
                        OK = "OK",
                        Qty = 1,
                        Result = "OK"
                    },
                    new VOC_ONSITE()
                    {
                        Month = 3,
                        Week = "W13",
                        Date = "2022-03-30",
                        Customer = "SEV",
                        Part = "LFEM",
                        Wisol_Model = "L5E0",
                        Customer_Code = "2911-000487",
                        Marking = "L5E2W216",
                        ProductionDate = "2022-01-06",
                        SetModel = "A336",
                        OK = "OK",
                        Qty = 1,
                        Result = "OK"
                    },
                    new VOC_ONSITE()
                    {
                        Month = 4,
                        Week = "W14",
                        Date = "2022-04-06",
                        Customer = "SEV",
                        Part = "LFEM",
                        Wisol_Model = "R9Z0",
                        Customer_Code = "2911-000504",
                        Marking = "R9Z0W22C",
                        ProductionDate = "2022-02-12",
                        SetModel = "A326",
                        OK = "OK",
                        Qty = 1,
                        Result = "OK"
                    },
                    new VOC_ONSITE()
                    {
                        Month = 4,
                        Week = "W14",
                        Date = "2022-04-06",
                        Customer = "SEV",
                        Part = "LFEM",
                        Wisol_Model = "L7E0",
                        Customer_Code = "2911-000430",
                        Marking = "L7E01W2314",
                        ProductionDate = "2022-03-14",
                        SetModel = "A325",
                        OK = "OK",
                        Qty = 1,
                        Result = "OK"
                    },
                    new VOC_ONSITE()
                    {
                        Month = 4,
                        Week = "W14",
                        Date = "2022-04-06",
                        Customer = "SEV",
                        Part = "LFEM",
                        Wisol_Model = "L7E0",
                        Customer_Code = "2911-000430",
                        Marking = "L7E01W2305",
                        ProductionDate = "2022-03-05",
                        SetModel = "A325",
                        OK = "OK",
                        Qty = 1,
                        Result = "OK"
                    },

                    // SEVT
                    new VOC_ONSITE()
                    {
                        Month = 3,
                        Week = "W12",
                        Date = "2022-03-25",
                        Customer = "SEVT",
                        Part = "CSP",
                        Wisol_Model = "X707AH9",
                        Customer_Code = "2910-000429",
                        Marking = "AEOOB",
                        ProductionDate = "2021-12-11",
                        SetModel = "A135",
                        OK = "OK",
                        Qty = 1,
                        Result = "OK"
                    },
                    new VOC_ONSITE()
                    {
                        Month = 3,
                        Week = "W12",
                        Date = "2022-03-25",
                        Customer = "SEVT",
                        Part = "CSP",
                        Wisol_Model = "XG50CY9",
                        Customer_Code = "2910-000385",
                        Marking = "FLNZo9",
                        ProductionDate = "2021-11-09",
                        SetModel = "A127",
                        OK = "OK",
                        Qty = 1,
                        Result = "OK"
                    },
                    new VOC_ONSITE()
                    {
                        Month = 3,
                        Week = "W12",
                        Date = "2022-03-25",
                        Customer = "SEVT",
                        Part = "CSP",
                        Wisol_Model = "XG50CY9",
                        Customer_Code = "2910-000385",
                        Marking = "FLOZoH",
                        ProductionDate = "2021-12-17",
                        SetModel = "A135",
                        OK = "OK",
                        Qty = 1,
                        Result = "OK"
                    },

                    new VOC_ONSITE()
                    {
                        Month = 3,
                        Week = "W12",
                        Date = "2022-03-25",
                        Customer = "SEVT",
                        Part = "CSP",
                        Wisol_Model = "HG89BF3",
                        Customer_Code = "2904-002417",
                        Marking = "APO2",
                        ProductionDate = "2021-12-02",
                        SetModel = "A536",
                        OK = "OK",
                        Qty = 1,
                        Result = "OK"
                    },
                    new VOC_ONSITE()
                    {
                        Month = 3,
                        Week = "W12",
                        Date = "2022-03-25",
                        Customer = "SEVT",
                        Part = "CSP",
                        Wisol_Model = "HG89BF3",
                        Customer_Code = "2904-002417",
                        Marking = "AOOI",
                        ProductionDate = "2021-12-18",
                        SetModel = "A536",
                        OK = "OK",
                        Qty = 1,
                        Result = "OK"
                    },
                    new VOC_ONSITE()
                    {
                        Month = 3,
                        Week = "W12",
                        Date = "2022-03-25",
                        Customer = "SEVT",
                        Part = "CSP",
                        Wisol_Model = "HG89BF3",
                        Customer_Code = "2904-002417",
                        Marking = "AOOM",
                        ProductionDate = "2021-12-22",
                        SetModel = "A536",
                        OK = "OK",
                        Qty = 1,
                        Result = "OK"
                    },
                     new VOC_ONSITE()
                    {
                        Month = 3,
                        Week = "W12",
                        Date = "2022-03-25",
                        Customer = "SEVT",
                        Part = "CSP",
                        Wisol_Model = "HG89BF3",
                        Customer_Code = "2904-002417",
                        Marking = "AOOO",
                        ProductionDate = "2021-12-24",
                        SetModel = "A235",
                        OK = "OK",
                        Qty = 1,
                        Result = "OK"
                    },

                    new VOC_ONSITE()
                    {
                        Month = 3,
                        Week = "W12",
                        Date = "2022-03-25",
                        Customer = "SEVT",
                        Part = "LFEM",
                        Wisol_Model = "L8E0",
                        Customer_Code = "2911-000475",
                        Marking = "L8ETW11U",
                        ProductionDate = "2021-01-30",
                        SetModel = "A235",
                        OK = "OK",
                        Qty = 1,
                        Result = "OK"
                    },
                        new VOC_ONSITE()
                    {
                        Month = 3,
                        Week = "W12",
                        Date = "2022-03-25",
                        Customer = "SEVT",
                        Part = "LFEM",
                        Wisol_Model = "L8E0",
                        Customer_Code = "2911-000475",
                        Marking = "L8E5W128",
                        ProductionDate = "2021-02-08",
                        SetModel = "A235",
                        OK = "OK",
                        Qty = 1,
                        Result = "OK"
                    },
                     new VOC_ONSITE()
                    {
                        Month = 3,
                        Week = "W12",
                        Date = "2022-03-25",
                        Customer = "SEVT",
                        Part = "LFEM",
                        Wisol_Model = "L8E0",
                        Customer_Code = "2911-000475",
                        Marking = "L8EMW0AT",
                        ProductionDate = "2020-10-29",
                        SetModel = "A235",
                        OK = "OK",
                        Qty = 1,
                        Result = "OK"
                    },
                    new VOC_ONSITE()
                    {
                        Month = 3,
                        Week = "W12",
                        Date = "2022-03-25",
                        Customer = "SEVT",
                        Part = "LFEM",
                        Wisol_Model = "L8E0",
                        Customer_Code = "2911-000475",
                        Marking = "L8EGW11P",
                        ProductionDate = "2021-01-25",
                        SetModel = "A235",
                        OK = "OK",
                        Qty = 1,
                        Result = "OK"
                    },
                    new VOC_ONSITE()
                    {
                        Month = 3,
                        Week = "W12",
                        Date = "2022-03-25",
                        Customer = "SEVT",
                        Part = "LFEM",
                        Wisol_Model = "L8E0",
                        Customer_Code = "2911-000475",
                        Marking = "L8EHW0CC",
                        ProductionDate = "2020-12-12",
                        SetModel = "A235",
                        OK = "OK",
                        Qty = 1,
                        Result = "OK"
                    },
                     new VOC_ONSITE()
                    {
                        Month = 3,
                        Week = "W12",
                        Date = "2022-03-25",
                        Customer = "SEVT",
                        Part = "LFEM",
                        Wisol_Model = "L8E0",
                        Customer_Code = "2911-000475",
                        Marking = "L8EPW11I",
                        ProductionDate = "2021-01-18",
                        SetModel = "A235",
                        OK = "OK",
                        Qty = 1,
                        Result = "OK"
                    },
                     new VOC_ONSITE()
                    {
                        Month = 3,
                        Week = "W12",
                        Date = "2022-03-25",
                        Customer = "SEVT",
                        Part = "LFEM",
                        Wisol_Model = "L8E0",
                        Customer_Code = "2911-000475",
                        Marking = "L8EEW0CF",
                        ProductionDate = "2020-12-15",
                        SetModel = "A235",
                        OK = "OK",
                        Qty = 1,
                        Result = "OK"
                    },
                    new VOC_ONSITE()
                    {
                        Month = 3,
                        Week = "W12",
                        Date = "2022-03-25",
                        Customer = "SEVT",
                        Part = "LFEM",
                        Wisol_Model = "L8E0",
                        Customer_Code = "2911-000475",
                        Marking = "L8EAW0CE",
                        ProductionDate = "2020-12-14",
                        SetModel = "A235",
                        OK = "OK",
                        Qty = 1,
                        Result = "OK"
                    },
                     new VOC_ONSITE()
                    {
                        Month = 3,
                        Week = "W12",
                        Date = "2022-03-25",
                        Customer = "SEVT",
                        Part = "LFEM",
                        Wisol_Model = "L8E0",
                        Customer_Code = "2911-000475",
                        Marking = "L8EAW22P",
                        ProductionDate = "2022-02-25",
                        SetModel = "A235",
                        OK = "OK",
                        Qty = 1,
                        Result = "OK"
                    },
                     new VOC_ONSITE()
                    {
                        Month = 3,
                        Week = "W12",
                        Date = "2022-03-25",
                        Customer = "SEVT",
                        Part = "LFEM",
                        Wisol_Model = "L8E0",
                        Customer_Code = "2911-000475",
                        Marking = "L8ESW22N",
                        ProductionDate = "2022-02-23",
                        SetModel = "A235",
                        OK = "OK",
                        Qty = 1,
                        Result = "OK"
                    },
                     new VOC_ONSITE()
                    {
                        Month = 3,
                        Week = "W12",
                        Date = "2022-03-25",
                        Customer = "SEVT",
                        Part = "LFEM",
                        Wisol_Model = "L8E0",
                        Customer_Code = "2911-000475",
                        Marking = "L8ETW11U",
                        ProductionDate = "2021-01-30",
                        SetModel = "A235",
                        OK = "OK",
                        Qty = 1,
                        Result = "OK"
                    },
                      new VOC_ONSITE()
                    {
                        Month = 3,
                        Week = "W12",
                        Date = "2022-03-25",
                        Customer = "SEVT",
                        Part = "LFEM",
                        Wisol_Model = "L8E0",
                        Customer_Code = "2911-000475",
                        Marking = "L8EKW22D",
                        ProductionDate = "2022-02-13",
                        SetModel = "A235",
                        OK = "OK",
                        Qty = 1,
                        Result = "OK"
                    },
                     new VOC_ONSITE()
                    {
                        Month = 3,
                        Week = "W12",
                        Date = "2022-03-25",
                        Customer = "SEVT",
                        Part = "LFEM",
                        Wisol_Model = "L8E0",
                        Customer_Code = "2911-000475",
                        Marking = "L8EUW0B6",
                        ProductionDate = "2020-11-06",
                        SetModel = "A235",
                        OK = "OK",
                        Qty = 1,
                        Result = "OK"
                    },
                      new VOC_ONSITE()
                    {
                        Month = 3,
                        Week = "W12",
                        Date = "2022-03-25",
                        Customer = "SEVT",
                        Part = "LFEM",
                        Wisol_Model = "L8E0",
                        Customer_Code = "2911-000475",
                        Marking = "L8EPW12G",
                        ProductionDate = "2021-02-16",
                        SetModel = "A235",
                        OK = "OK",
                        Qty = 1,
                        Result = "OK"
                    },
                       new VOC_ONSITE()
                    {
                        Month = 3,
                        Week = "W12",
                        Date = "2022-03-25",
                        Customer = "SEVT",
                        Part = "LFEM",
                        Wisol_Model = "L8E0",
                        Customer_Code = "2911-000475",
                        Marking = "L8ERW11B",
                        ProductionDate = "2021-01-11",
                        SetModel = "A235",
                        OK = "OK",
                        Qty = 1,
                        Result = "OK"
                    },
                    new VOC_ONSITE()
                    {
                        Month = 3,
                        Week = "W12",
                        Date = "2022-03-25",
                        Customer = "SEVT",
                        Part = "LFEM",
                        Wisol_Model = "L8E0",
                        Customer_Code = "2911-000475",
                        Marking = "L8EVW0B5",
                        ProductionDate = "2020-11-05",
                        SetModel = "A235",
                        OK = "OK",
                        Qty = 1,
                        Result = "OK"
                    },

                    new VOC_ONSITE()
                    {
                        Month = 3,
                        Week = "W12",
                        Date = "2022-03-25",
                        Customer = "SEVT",
                        Part = "LFEM",
                        Wisol_Model = "L6S0",
                        Customer_Code = "2911-000515",
                        Marking = "L6SBW1COG",
                        ProductionDate = "2021-12-24",
                        SetModel = "A536",
                        Not_Measure = "NM",
                        Note = "Không test được bằng máy cũ",
                        Qty = 1
                    },
                    new VOC_ONSITE()
                    {
                        Month = 3,
                        Week = "W12",
                        Date = "2022-03-25",
                        Customer = "SEVT",
                        Part = "LFEM",
                        Wisol_Model = "L6S0",
                        Customer_Code = "2911-000515",
                        Marking = "L6SVW2131",
                        ProductionDate = "2022-01-03",
                        SetModel = "A536",
                        Not_Measure = "NM",
                        Note = "Không test được bằng máy cũ",
                        Qty = 1
                    },
                    new VOC_ONSITE()
                    {
                        Month = 3,
                        Week = "W12",
                        Date = "2022-03-25",
                        Customer = "SEVT",
                        Part = "LFEM",
                        Wisol_Model = "L6S0",
                        Customer_Code = "2911-000515",
                        Marking = "L6S6W1COF",
                        ProductionDate = "2021-12-24",
                        SetModel = "A536",
                       Not_Measure = "NM",
                       Note = "Không test được bằng máy cũ",
                        Qty = 1
                    },
                     new VOC_ONSITE()
                    {
                        Month = 3,
                        Week = "W12",
                        Date = "2022-03-25",
                        Customer = "SEVT",
                        Part = "LFEM",
                        Wisol_Model = "L6S0",
                        Customer_Code = "2911-000515",
                        Marking = "L6SBW1CMK",
                        ProductionDate = "2021-12-22",
                        SetModel = "A536",
                         Not_Measure = "NM",
                         Note = "Không test được bằng máy cũ",
                        Qty = 1
                    },
                    new VOC_ONSITE()
                    {
                        Month = 3,
                        Week = "W12",
                        Date = "2022-03-25",
                        Customer = "SEVT",
                        Part = "LFEM",
                        Wisol_Model = "L6S0",
                        Customer_Code = "2911-000515",
                        Marking = "L6SEW2216",
                        ProductionDate = "2022-02-01",
                        SetModel = "A536",
                         Not_Measure = "NM",
                         Note = "Không test được bằng máy cũ",
                        Qty = 1
                    },
                     new VOC_ONSITE()
                    {
                        Month = 3,
                        Week = "W12",
                        Date = "2022-03-25",
                        Customer = "SEVT",
                        Part = "LFEM",
                        Wisol_Model = "L6S0",
                        Customer_Code = "2911-000515",
                        Marking = "L6SBW1C35",
                        ProductionDate = "2021-12-03",
                        SetModel = "A536",
                        Not_Measure = "NM",
                        Note = "Không test được bằng máy cũ",
                        Qty = 1
                    },
                      new VOC_ONSITE()
                    {
                        Month = 3,
                        Week = "W12",
                        Date = "2022-03-25",
                        Customer = "SEVT",
                        Part = "LFEM",
                        Wisol_Model = "L6S0",
                        Customer_Code = "2911-000515",
                        Marking = "L6S3W22LH",
                        ProductionDate = "2021-02-21",
                        SetModel = "A536",
                         Not_Measure = "NM",
                         Note = "Không test được bằng máy cũ",
                        Qty = 1
                    },
                       new VOC_ONSITE()
                    {
                        Month = 3,
                        Week = "W12",
                        Date = "2022-03-25",
                        Customer = "SEVT",
                        Part = "LFEM",
                        Wisol_Model = "L6S0",
                        Customer_Code = "2911-000515",
                        Marking = "L6STW2145",
                        ProductionDate = "2022-01-04",
                        SetModel = "A536",
                         Not_Measure = "NM",
                         Note = "Không test được bằng máy cũ",
                        Qty = 1
                    },
                     new VOC_ONSITE()
                    {
                        Month = 3,
                        Week = "W12",
                        Date = "2022-03-25",
                        Customer = "SEVT",
                        Part = "LFEM",
                        Wisol_Model = "L6S0",
                        Customer_Code = "2911-000515",
                        Marking = "L6SZW22B2",
                        ProductionDate = "2022-02-11",
                        SetModel = "A536",
                         Not_Measure = "NM",
                         Note = "Không test được bằng máy cũ",
                        Qty = 1
                    },
                      new VOC_ONSITE()
                    {
                        Month = 3,
                        Week = "W12",
                        Date = "2022-03-25",
                        Customer = "SEVT",
                        Part = "LFEM",
                        Wisol_Model = "L6S0",
                        Customer_Code = "2911-000515",
                        Marking = "L6S3W22A6",
                        ProductionDate = "2022-02-10",
                        SetModel = "A536",
                         Not_Measure = "NM",
                         Note = "Không test được bằng máy cũ",
                        Qty = 1
                    },
                        new VOC_ONSITE()
                    {
                        Month = 3,
                        Week = "W12",
                        Date = "2022-03-25",
                        Customer = "SEVT",
                        Part = "LFEM",
                        Wisol_Model = "L6S0",
                        Customer_Code = "2911-000515",
                        Marking = "L6S6W1COI",
                        ProductionDate = "2021-12-24",
                        SetModel = "A536",
                         Not_Measure = "NM",
                         Note = "Không test được bằng máy cũ",
                        Qty = 1
                    },
                      new VOC_ONSITE()
                    {
                        Month = 3,
                        Week = "W12",
                        Date = "2022-03-25",
                        Customer = "SEVT",
                        Part = "LFEM",
                        Wisol_Model = "L6S0",
                        Customer_Code = "2911-000515",
                        Marking = "L6SYW216F",
                        ProductionDate = "2022-01-06",
                        SetModel = "A536",
                         Not_Measure = "NM",
                         Note = "Không test được bằng máy cũ",
                        Qty = 1
                    },
                        new VOC_ONSITE()
                    {
                        Month = 3,
                        Week = "W12",
                        Date = "2022-03-25",
                        Customer = "SEVT",
                        Part = "LFEM",
                        Wisol_Model = "L6S0",
                        Customer_Code = "2911-000515",
                        Marking = "L6SIW1COG",
                        ProductionDate = "2021-12-24",
                        SetModel = "A536",
                        Not_Measure = "NM",
                        Note = "Không test được bằng máy cũ",
                        Qty = 1
                    },
                    new VOC_ONSITE()
                    {
                        Month = 3,
                        Week = "W12",
                        Date = "2022-03-25",
                        Customer = "SEVT",
                        Part = "LFEM",
                        Wisol_Model = "L6S0",
                        Customer_Code = "2911-000515",
                        Marking = "L6S4W1BJJ",
                        ProductionDate = "2021-11-19",
                        SetModel = "A536",
                         Not_Measure = "NM",
                         Note = "Không test được bằng máy cũ",
                        Qty = 1
                    },
                    new VOC_ONSITE()
                    {
                        Month = 3,
                        Week = "W12",
                        Date = "2022-03-25",
                        Customer = "SEVT",
                        Part = "LFEM",
                        Wisol_Model = "L6S0",
                        Customer_Code = "2911-000515",
                        Marking = "L6S4W1BBJ",
                        ProductionDate = "2021-11-11",
                        SetModel = "A536",
                         Not_Measure = "NM",
                         Note = "Không test được bằng máy cũ",
                        Qty = 1
                    },
                    new VOC_ONSITE()
                    {
                        Month = 3,
                        Week = "W12",
                        Date = "2022-03-25",
                        Customer = "SEVT",
                        Part = "LFEM",
                        Wisol_Model = "L6S0",
                        Customer_Code = "2911-000515",
                        Marking = "L6SSW21OM",
                        ProductionDate = "2022-01-24",
                        SetModel = "A536",
                         Not_Measure = "NM",
                         Note = "Không test được bằng máy cũ",
                        Qty = 1
                    },
                    new VOC_ONSITE()
                    {
                        Month = 3,
                        Week = "W12",
                        Date = "2022-03-25",
                        Customer = "SEVT",
                        Part = "LFEM",
                        Wisol_Model = "L6S0",
                        Customer_Code = "2911-000515",
                        Marking = "L6S3W21P2B",
                        ProductionDate = "2022-01-25",
                        SetModel = "A536",
                         Not_Measure = "NM",
                         Note = "Không test được bằng máy cũ",
                        Qty = 1
                    },
                    new VOC_ONSITE()
                    {
                        Month = 3,
                        Week = "W12",
                        Date = "2022-03-25",
                        Customer = "SEVT",
                        Part = "LFEM",
                        Wisol_Model = "L6S0",
                        Customer_Code = "2911-000515",
                        Marking = "L6SSW1BN6",
                        ProductionDate = "2021-11-23",
                        SetModel = "A536",
                         Not_Measure = "NM",
                         Note = "Không test được bằng máy cũ",
                        Qty = 1
                    },

                   new VOC_ONSITE()
                    {
                        Month = 3,
                        Week = "W12",
                        Date = "2022-03-25",
                        Customer = "SEVT",
                        Part = "LFEM",
                        Wisol_Model = "L5E0",
                        Customer_Code = "2911-000487",
                        Marking = "L5ETW1CK7",
                        ProductionDate = "2021-12-20",
                        SetModel = "A525",
                        OK="OK",
                        Qty = 1,
                        Result = "OK"
                    },
                   new VOC_ONSITE()
                    {
                        Month = 3,
                        Week = "W12",
                        Date = "2022-03-25",
                        Customer = "SEVT",
                        Part = "LFEM",
                        Wisol_Model = "L5E0",
                        Customer_Code = "2911-000487",
                        Marking = "L5E6W1CLN",
                        ProductionDate = "2021-12-21",
                        SetModel = "A536",
                        OK="OK",
                        Qty = 1,
                        Result = "OK"
                    },
                   new VOC_ONSITE()
                    {
                        Month = 3,
                        Week = "W12",
                        Date = "2022-03-25",
                        Customer = "SEVT",
                        Part = "LFEM",
                        Wisol_Model = "L5E0",
                        Customer_Code = "2911-000487",
                        Marking = "L5E6W1CLN",
                        ProductionDate = "2021-12-21",
                        SetModel = "A536",
                        OK="OK",
                        Qty = 1,
                        Result = "OK"
                    },
                   new VOC_ONSITE()
                    {
                        Month = 3,
                        Week = "W12",
                        Date = "2022-03-25",
                        Customer = "SEVT",
                        Part = "LFEM",
                        Wisol_Model = "L5E0",
                        Customer_Code = "2911-000487",
                        Marking = "L5EBW2181",
                        ProductionDate = "2022-01-08",
                        SetModel = "A536",
                        OK="OK",
                        Qty = 1,
                        Result = "OK"
                    },
                   new VOC_ONSITE()
                    {
                        Month = 3,
                        Week = "W12",
                        Date = "2022-03-25",
                        Customer = "SEVT",
                        Part = "LFEM",
                        Wisol_Model = "L5E0",
                        Customer_Code = "2911-000487",
                        Marking = "L5ETW1CRO",
                        ProductionDate = "2021-12-27",
                        SetModel = "G990",
                        OK="OK",
                        Qty = 1,
                        Result = "OK"
                    },
                   new VOC_ONSITE()
                    {
                        Month = 3,
                        Week = "W12",
                        Date = "2022-03-25",
                        Customer = "SEVT",
                        Part = "LFEM",
                        Wisol_Model = "L5E0",
                        Customer_Code = "2911-000487",
                        Marking = "L5ELW22NB",
                        ProductionDate = "2022-02-23",
                        SetModel = "A525",
                        OK="OK",
                        Qty = 1,
                        Result = "OK"
                    },
                   new VOC_ONSITE()
                    {
                        Month = 3,
                        Week = "W12",
                        Date = "2022-03-25",
                        Customer = "SEVT",
                        Part = "LFEM",
                        Wisol_Model = "L5E0",
                        Customer_Code = "2911-000487",
                        Marking = "L5EFW1C3K",
                        ProductionDate = "2021-12-03",
                        SetModel = "A536",
                        OK="OK",
                        Qty = 1,
                        Result = "OK"
                    },
                   new VOC_ONSITE()
                    {
                        Month = 3,
                        Week = "W12",
                        Date = "2022-03-25",
                        Customer = "SEVT",
                        Part = "LFEM",
                        Wisol_Model = "L5E0",
                        Customer_Code = "2911-000487",
                        Marking = "L5E9W22CD",
                        ProductionDate = "2022-02-12",
                        SetModel = "A536",
                        OK="OK",
                        Qty = 1,
                        Result = "OK"
                    },
                    new VOC_ONSITE()
                    {
                        Month = 3,
                        Week = "W12",
                        Date = "2022-03-25",
                        Customer = "SEVT",
                        Part = "LFEM",
                        Wisol_Model = "L5E0",
                        Customer_Code = "2911-000487",
                        Marking = "L5ECW1CTA",
                        ProductionDate = "2021-12-29",
                        SetModel = "A536",
                        OK="OK",
                        Qty = 1,
                        Result = "OK"
                    },

                    new VOC_ONSITE()
                    {
                        Month = 3,
                        Week = "W12",
                        Date = "2022-03-25",
                        Customer = "SEVT",
                        Part = "LFEM",
                        Wisol_Model = "LHB0",
                        Customer_Code = "2911-000507",
                        Marking = "LHBIW1CO",
                        ProductionDate = "2021-12-24",
                        SetModel = "A125",
                        Not_Measure="NM",
                         Note = "Không test được bằng máy cũ",
                        Qty = 1
                    },
                    new VOC_ONSITE()
                    {
                        Month = 3,
                        Week = "W12",
                        Date = "2022-03-25",
                        Customer = "SEVT",
                        Part = "LFEM",
                        Wisol_Model = "LHB0",
                        Customer_Code = "2911-000507",
                        Marking = "LHBPW1CN",
                        ProductionDate = "2021-12-23",
                        SetModel = "A125",
                        Not_Measure="NM",
                         Note = "Không test được bằng máy cũ",
                        Qty = 1
                    },
                    new VOC_ONSITE()
                    {
                        Month = 3,
                        Week = "W12",
                        Date = "2022-03-25",
                        Customer = "SEVT",
                        Part = "LFEM",
                        Wisol_Model = "L1A0",
                        Customer_Code = "2911-000525",
                        Marking = "L1AHW219O",
                        ProductionDate = "2022-01-09",
                        SetModel = "A536",
                        Not_Measure="NM",
                         Note = "Không test được bằng máy cũ",
                        Qty = 1
                    },
                    new VOC_ONSITE()
                    {
                        Month = 3,
                        Week = "W12",
                        Date = "2022-03-25",
                        Customer = "SEVT",
                        Part = "LFEM",
                        Wisol_Model = "L1A0",
                        Customer_Code = "2911-000525",
                        Marking = "L1ASW1CIO",
                        ProductionDate = "2021-12-18",
                        SetModel = "A536",
                        Not_Measure="NM",
                         Note = "Không test được bằng máy cũ",
                        Qty = 1
                    },
                    new VOC_ONSITE()
                    {
                        Month = 3,
                        Week = "W12",
                        Date = "2022-03-25",
                        Customer = "SEVT",
                        Part = "LFEM",
                        Wisol_Model = "L1A0",
                        Customer_Code = "2911-000525",
                        Marking = "L1AAW21EO",
                        ProductionDate = "2022-01-14",
                        SetModel = "A536",
                        Not_Measure="NM",
                         Note = "Không test được bằng máy cũ",
                        Qty = 1
                    },
                    new VOC_ONSITE()
                    {
                        Month = 3,
                        Week = "W12",
                        Date = "2022-03-25",
                        Customer = "SEVT",
                        Part = "LFEM",
                        Wisol_Model = "L1A0",
                        Customer_Code = "2911-000525",
                        Marking = "L1AVW21OO",
                        ProductionDate = "2022-01-24",
                        SetModel = "A536",
                        Not_Measure="NM",
                         Note = "Không test được bằng máy cũ",
                        Qty = 1
                    },
                    new VOC_ONSITE()
                    {
                        Month = 3,
                        Week = "W12",
                        Date = "2022-03-25",
                        Customer = "SEVT",
                        Part = "LFEM",
                        Wisol_Model = "L1A0",
                        Customer_Code = "2911-000525",
                        Marking = "L1AFW21OO",
                        ProductionDate = "2022-01-24",
                        SetModel = "A536",
                        Not_Measure="NM",
                         Note = "Không test được bằng máy cũ",
                        Qty = 1
                    },
                    new VOC_ONSITE()
                    {
                        Month = 3,
                        Week = "W12",
                        Date = "2022-03-25",
                        Customer = "SEVT",
                        Part = "LFEM",
                        Wisol_Model = "D3L0",
                        Customer_Code = "2911-000495",
                        Marking = "D3LHW1CR7",
                        ProductionDate = "2021-12-27",
                        SetModel = "A125",
                        Not_Measure="NM",
                         Note = "Không test được bằng máy cũ",
                        Qty = 1
                    },
                    new VOC_ONSITE()
                    {
                        Month = 3,
                        Week = "W12",
                        Date = "2022-03-25",
                        Customer = "SEVT",
                        Part = "LFEM",
                        Wisol_Model = "LHT0",
                        Customer_Code = "2911-000457",
                        Marking = "LHT08W1B13",
                        ProductionDate = "2021-11-13",
                        SetModel = "G991",
                        Not_Measure="NM",
                         Note = "Không test được bằng máy cũ",
                        Qty = 1
                    },

                     new VOC_ONSITE()
                    {
                        Month = 3,
                        Week = "W13",
                        Date = "2022-03-31",
                        Customer = "SEVT",
                        Part = "LFEM",
                        Wisol_Model = "L8E0",
                        Customer_Code = "2911-000475",
                        Marking = "L8EAW234",
                        ProductionDate = "2022-03-04",
                        SetModel = "A235",
                        OK= "OK",
                        Qty = 1,
                        Result = "OK"
                    },
                      new VOC_ONSITE()
                    {
                       Month = 3,
                        Week = "W13",
                        Date = "2022-03-31",
                        Customer = "SEVT",
                        Part = "LFEM",
                        Wisol_Model = "L8E0",
                        Customer_Code = "2911-000475",
                        Marking = "L8EEW233",
                        ProductionDate = "2022-03-03",
                        SetModel = "A235",
                        OK= "OK",
                        Qty = 1,
                        Result = "OK"
                    },
                    new VOC_ONSITE()
                    {
                       Month = 3,
                        Week = "W13",
                        Date = "2022-03-31",
                        Customer = "SEVT",
                        Part = "LFEM",
                        Wisol_Model = "L8E0",
                        Customer_Code = "2911-000475",
                        Marking = "L8EAW22P",
                        ProductionDate = "2022-02-25",
                        SetModel = "A235",
                        OK= "OK",
                        Qty = 1,
                        Result = "OK"
                    },
                     new VOC_ONSITE()
                    {
                        Month = 3,
                        Week = "W13",
                        Date = "2022-03-31",
                        Customer = "SEVT",
                        Part = "LFEM",
                        Wisol_Model = "L5E0",
                        Customer_Code = "2911-000487",
                        Marking = "L5ELW22N",
                        ProductionDate = "2022-02-23",
                        SetModel = "A536",
                        OK= "OK",
                        Qty = 1,
                        Result = "OK"
                    },
                      new VOC_ONSITE()
                    {
                        Month = 3,
                        Week = "W13",
                        Date = "2022-03-31",
                        Customer = "SEVT",
                        Part = "LFEM",
                        Wisol_Model = "L5E0",
                        Customer_Code = "2911-000487",
                        Marking = "L5E8W11P",
                        ProductionDate = "2021-01-25",
                        SetModel = "A536",
                        OK= "OK",
                        Qty = 1,
                        Result = "OK"
                    },
                        new VOC_ONSITE()
                    {
                        Month = 3,
                        Week = "W13",
                        Date = "2022-03-31",
                        Customer = "SEVT",
                        Part = "LFEM",
                        Wisol_Model = "L5E0",
                        Customer_Code = "2911-000487",
                        Marking = "L5E6W11P",
                        ProductionDate = "2021-01-25",
                        SetModel = "A536",
                        OK= "OK",
                        Qty = 1,
                        Result = "OK"
                    },

                    new VOC_ONSITE()
                    {
                        Month = 3,
                        Week = "W13",
                        Date = "2022-03-31",
                        Customer = "SEVT",
                        Part = "LFEM",
                        Wisol_Model = "L6S0",
                        Customer_Code = "2911-000515",
                        Marking = "L6S3W22A",
                        ProductionDate = "2022-02-10",
                        SetModel = "S901",
                         Not_Measure = "NM",
                         Note = "Không test được bằng máy cũ",
                        Qty = 1
                    },
                     new VOC_ONSITE()
                    {
                        Month = 3,
                        Week = "W13",
                        Date = "2022-03-31",
                        Customer = "SEVT",
                        Part = "LFEM",
                        Wisol_Model = "L6S0",
                        Customer_Code = "2911-000515",
                        Marking = "L6S9W22O",
                        ProductionDate = "2022-02-24",
                        SetModel = "S901",
                       Not_Measure = "NM",
                       Note = "Không test được bằng máy cũ",
                        Qty = 1
                    },
                    new VOC_ONSITE()
                    {
                        Month = 3,
                        Week = "W13",
                        Date = "2022-03-31",
                        Customer = "SEVT",
                        Part = "LFEM",
                        Wisol_Model = "L1A0",
                        Customer_Code = "2911-000525",
                        Marking = "L1AWW21I",
                        ProductionDate = "2022-01-18",
                        SetModel = "A536",
                        Not_Measure = "NM",
                        Note = "Không test được bằng máy cũ",
                        Qty = 1
                    },
                });
            }

            await _context.SaveChangesAsync();
        }
    }
}
