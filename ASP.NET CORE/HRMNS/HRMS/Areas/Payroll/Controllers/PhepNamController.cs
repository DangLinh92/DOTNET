using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using HRMNS.Application.Implementation;
using HRMNS.Application.Interfaces;
using HRMNS.Application.ViewModels.HR;
using HRMNS.Data.EF;
using HRMNS.Data.Entities;
using HRMNS.Utilities.Common;
using HRMNS.Utilities.Constants;
using HRMS.Areas.Admin.Models;
using HRMS.Infrastructure.Interfaces;
using HRMS.ScheduledTasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace HRMS.Areas.Payroll.Controllers
{
    public class PhepNamController : AdminBaseController
    {
        IPhepNamService _phepNamService;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public PhepNamController(IPhepNamService phepNamService, IWebHostEnvironment hostingEnvironment)
        {
            _phepNamService = phepNamService;
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index()
        {
            DeleteFileSr(_hostingEnvironment);
            _ = ChotCongFinal();
            return View();
        }

        [HttpGet]
        public object PhepNams(DataSourceLoadOptions loadOptions, string year)
        {
            _ = ChotCongFinal();
            var lstModel = _phepNamService.GetList(year);
            return DataSourceLoader.Load(lstModel, loadOptions);
        }

        [HttpPost]
        public IActionResult Insert(string values)
        {
            var phepNam = new PhepNamViewModel();
            JsonConvert.PopulateObject(values, phepNam);
            phepNam = _phepNamService.Add(phepNam);
            _phepNamService.Save();

            return Ok(phepNam);
        }

        [HttpPut]
        public IActionResult Update(int key, string values)
        {
            var phepNam = _phepNamService.GetById(key);
            JsonConvert.PopulateObject(values, phepNam);

            _phepNamService.Update(phepNam);
            _phepNamService.Save();
            return Ok(phepNam);
        }

        [HttpDelete]
        public void Delete(int key)
        {
            _phepNamService.Delete(key);
            _phepNamService.Save();
        }

        [HttpPost]
        public IActionResult UpdatePhepNam(List<PhepNamViewModel> phepNams)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return new BadRequestObjectResult(allErrors);
            }
            else
            {
                foreach (var item in phepNams)
                {
                    PhepNamViewModel phepNam = _phepNamService.GetById(item.Id);
                    if (phepNam != null)
                    {
                        phepNam.MaNhanVien = item.MaNhanVien;
                        phepNam.SoPhepNam = item.SoPhepNam;
                        phepNam.SoPhepConLai = item.SoPhepConLai;
                        _phepNamService.Update(phepNam);
                    }
                    else
                    {
                        _phepNamService.Add(item);
                    }
                }
                _phepNamService.Save();
                return PartialView("/Areas/Admin/Views/NhanVien/_profileCompassionateLeavePartial.cshtml", phepNams);
            }
        }

        [HttpPost]
        [RequestFormLimits(MultipartBodyLengthLimit = 209715200)]
        [RequestSizeLimit(209715200)]
        public IActionResult ImportExcel(IList<IFormFile> files)
        {
            if (files != null && files.Count > 0)
            {
                var file = files[0];
                var filename = ContentDispositionHeaderValue
                                   .Parse(file.ContentDisposition)
                                   .FileName
                                   .Trim('"');

                string folder = _hostingEnvironment.WebRootPath + $@"\uploaded\excels";
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }

                string filePath = Path.Combine(folder, CorrelationIdGenerator.GetNextId() + filename);
                using (FileStream fs = System.IO.File.Create(filePath))
                {
                    file.CopyTo(fs);
                    fs.Flush();
                }

                _phepNamService.ImportExcel(filePath);
                _phepNamService.Save();

                if (System.IO.File.Exists(filePath))
                {
                    // If file found, delete it    
                    System.IO.File.Delete(filePath);
                }

                return new OkObjectResult(filePath);
            }
            return new NotFoundObjectResult(CommonConstants.NotFoundObjectResult_Msg);
        }

        public async Task ChotCongFinal()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddDbContext<AppDBContext>(options =>
               options.UseSqlServer(
                   @"Persist Security Info=True;Data Source = 10.70.10.97;Initial Catalog = HRMSDB2;User Id = sa;Password = Wisol@123;Connect Timeout=3", o => o.MigrationsAssembly("HRMNS.Data.EF")));

            serviceCollection.AddSingleton(HRMNS.Application.AutoMapper.AutoMapperConfig.RegisterMappings().CreateMapper());
            serviceCollection.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            serviceCollection.AddScoped<UserManager<APP_USER>, UserManager<APP_USER>>();
            serviceCollection.AddScoped<RoleManager<APP_ROLE>, RoleManager<APP_ROLE>>();

            serviceCollection.AddScoped(typeof(IUnitOfWork), typeof(EFUnitOfWork));
            serviceCollection.AddScoped(typeof(IRespository<,>), typeof(EFRepository<,>));
            serviceCollection.AddScoped<UpdatePhepNamDaiLyJob>();
            serviceCollection.AddScoped<INgayChotCongService, NgayChotCongService>();
            serviceCollection.AddScoped<INhanVienService, NhanVienService>();
            serviceCollection.AddScoped<IPhepNamService, PhepNamService>();
            serviceCollection.AddScoped<IDangKyChamCongChiTietService, DangKyChamCongChiTietService>();
            serviceCollection.AddScoped<IDangKyChamCongDacBietService, DangKyChamCongDacBietService>();
            serviceCollection.AddScoped<IDangKyChamCongDacBietService, DangKyChamCongDacBietService>();

            serviceCollection.AddLogging();

            var serviceProvider = serviceCollection.BuildServiceProvider();

            // construct a scheduler factory
            ISchedulerFactory schedFact = new StdSchedulerFactory();

            // get a scheduler
            IScheduler sched = await schedFact.GetScheduler();
            sched.JobFactory = new PhepNamHanldeJobFactory(serviceProvider);

            await sched.Start();

            var job = JobBuilder.Create<UpdatePhepNamDaiLyJob>()
                                .WithIdentity("PhepNamDaiLyJob", "group1")
                                .Build();

            var trigger = TriggerBuilder.Create()
                .WithIdentity("PhepNamDaiLyJob-trigger", "group1")
                .StartNow()
                //.WithSimpleSchedule(x => x
                //.WithIntervalInSeconds(40)
                //.RepeatForever())
                .Build();

            await sched.ScheduleJob(job, trigger);
        }
    }
}
