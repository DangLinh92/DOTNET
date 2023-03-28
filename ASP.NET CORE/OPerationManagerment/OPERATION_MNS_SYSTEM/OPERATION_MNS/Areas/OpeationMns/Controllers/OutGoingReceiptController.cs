﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using OPERATION_MNS.Application.Interfaces;
using OPERATION_MNS.Application.ViewModels.Wlp2;
using OPERATION_MNS.Data.Entities;
using OPERATION_MNS.Utilities.Common;
using OPERATION_MNS.Utilities.Constants;
using OPERATION_MNS.Utilities.Dtos;

namespace OPERATION_MNS.Areas.OpeationMns.Controllers
{
    public class OutGoingReceiptController : AdminBaseController
    {
        private IOutGoingReceiptService _IOutGoingReceiptService;

        private readonly IWebHostEnvironment _hostingEnvironment;

        public OutGoingReceiptController(IOutGoingReceiptService OutGoingReceiptService, IWebHostEnvironment hostingEnvironment, ILogger<OutGoingReceiptController> logger)
        {
            _IOutGoingReceiptService = OutGoingReceiptService;
            _hostingEnvironment = hostingEnvironment;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public object Get(DataSourceLoadOptions loadOptions)
        {
            return DataSourceLoader.Load(_IOutGoingReceiptService.GetAllToDay(), loadOptions);
        }

        [HttpPut]
        public IActionResult Put(string key, string values)
        {
            OUTGOING_RECEIPT_WLP2 model = _IOutGoingReceiptService.GetAllToDay().Find(x=>x.Key == key);

            JsonConvert.PopulateObject(values, model);

            _IOutGoingReceiptService.Update(model);
            return Ok();
        }

    }
}