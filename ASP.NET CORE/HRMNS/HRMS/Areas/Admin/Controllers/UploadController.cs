using HRMNS.Utilities.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace HRMS.Areas.Admin.Controllers
{
    public class UploadController : AdminBaseController
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        public UploadController(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Upload image for form
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult UploadImage(string category, string Id)
        {
            var files = Request.Form.Files;
            if (files.Count == 0)
            {
                return new BadRequestObjectResult(files);
            }
            else
            {
                var file = files[0];
                var filename = ContentDispositionHeaderValue
                                    .Parse(file.ContentDisposition)
                                    .FileName
                                    .Trim('"');

                var imageFolder = $@"\uploaded\images\{category}";
                filename = Id + CorrelationIdGenerator.GetNextId() + filename;

                string folder = _hostingEnvironment.WebRootPath + imageFolder;

                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
                string filePath = Path.Combine(folder, filename);
                using (FileStream fs = System.IO.File.Create(filePath))
                {
                    file.CopyTo(fs);
                    fs.Flush();
                }
                return new OkObjectResult(Path.Combine(imageFolder, filename).Replace(@"\", @"/"));
            }
        }
    }
}
