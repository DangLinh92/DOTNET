using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OPERATION_MNS.Application.Interfaces;
using OPERATION_MNS.Application.ViewModels;
using OPERATION_MNS.Areas.OpeationMns.Models.SignalR;
using OPERATION_MNS.Utilities.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPERATION_MNS.Areas.OpeationMns.Controllers
{
    public class UserMailController : AdminBaseController
    {
        IUserMailService _UserMailService;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public UserMailController(IUserMailService service, IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
            _UserMailService = service;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public object GetMails(DataSourceLoadOptions loadOptions)
        {
            return DataSourceLoader.Load(_UserMailService.GetListMail(), loadOptions);
        }

        [HttpPost]
        public IActionResult PostMail(string values)
        {
            var ctq = new CTQEmailReceivViewModel();
            JsonConvert.PopulateObject(values, ctq);
            _UserMailService.PostMail(ctq);
            return Ok();
        }

        [HttpPut]
        public IActionResult PutMail(string key,string values)
        {
            var mail = _UserMailService.GetMail(key);
            JsonConvert.PopulateObject(values, mail);
            _UserMailService.DeleteMail(key);
            _UserMailService.PostMail(mail);
            return Ok();
        }

        [HttpDelete]
        public void DeleteMail(string key)
        {
            _UserMailService.DeleteMail(key);
        }

    }
}
