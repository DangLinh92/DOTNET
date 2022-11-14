﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevExtreme.AspNet.Mvc.FileManagement;
using GroupDocs.Viewer;
using GroupDocs.Viewer.Options;

namespace HRMS.Areas.Admin.Controllers
{
    public class FileManagerImagesApiController : AdminBaseController
    {
        public IActionResult Index()
        {
            return View();
        }

        static readonly string SampleImagesRelativePath = Path.Combine("FileData", "Ehs_Documents");

        public FileManagerImagesApiController(IWebHostEnvironment hostingEnvironment)
        {
            HostingEnvironment = hostingEnvironment;
        }
        public IWebHostEnvironment HostingEnvironment { get; }

        [Route("api/file-manager-file-system-images", Name = "FileManagementImagesApi")]
        public object FileSystem(FileSystemCommand command, string arguments)
        {
            var config = new FileSystemConfiguration
            {
                Request = Request,
                FileSystemProvider = new PhysicalFileSystemProvider(
                    Path.Combine(HostingEnvironment.WebRootPath, SampleImagesRelativePath),
                    (fileSystemItem, clientItem) =>
                    {
                        if (!clientItem.IsDirectory)
                            clientItem.CustomFields["url"] = GetFileItemUrl(fileSystemItem);
                    }
                ),
                //uncomment the code below to enable file/directory management
                AllowCopy = true,
                AllowCreate = true,
                AllowMove = true,
                AllowDelete = true,
                AllowRename = true,
                AllowUpload = true,
                AllowDownload = true
            };
            var processor = new FileSystemCommandProcessor(config);
            var result = processor.Execute(command, arguments);
            return result.GetClientCommandResult();
        }

        string GetFileItemUrl(FileSystemInfo fileSystemItem)
        {
            var relativeUrl = fileSystemItem.FullName
                .Replace(HostingEnvironment.WebRootPath, "")
                .Replace(Path.DirectorySeparatorChar, '/');
            return $"{Request.Scheme}://{Request.Host}{Request.PathBase}{relativeUrl}";
        }
    }
}
