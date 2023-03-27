using HRMS.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Syncfusion.EJ2.FileManager.Base;
using System;
using System.Collections.Generic;
//using FileManagerResponse = HRMS.Models.Base.FileManagerResponse;
//using FileManagerDirectoryContent = HRMS.Models.Base.FileManagerDirectoryContent;

using Syncfusion.EJ2.FileManager.PhysicalFileProvider;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using HRMNS.Data.Entities;
using HRMS.Infrastructure.Interfaces;
using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using System.Linq;
using DevExtreme.AspNet.Mvc;
using DevExtreme.AspNet.Data;
using HRMNS.Data.EF.Extensions;

namespace HRMS.Areas.Admin.Controllers
{
    public class FileManagerController : AdminBaseController
    {
        private IRespository<EHS_FILES, int> _filesRepository;
        private IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _memoryCache;
        private string maNgayChiTiet;

        public IActionResult Index()
        {
            _memoryCache.Remove("MaNgayChiTiet");
            return View();
        }

        #region sql provider

        //SQLFileProvider operation;
        //public FileManagerController(IConfiguration configuration)
        //{
        //    operation = new SQLFileProvider(configuration);
        //    operation.SetSQLConnection("FileManagerConnection", "FILE_MANAGER", "0");
        //}

        //[HttpPost]
        //public object SQLFileOperations([FromBody] HRMS.Models.Base.FileManagerDirectoryContent args)
        //{
        //    if ((args.Action == "delete" || args.Action == "rename") && ((args.TargetPath == null) && (args.Path == "")))
        //    {
        //        FileManagerResponse response = new FileManagerResponse();
        //        response.Error = new ErrorDetails { Code = "403", Message = "Restricted to modify the root folder." };
        //        return operation.ToCamelCase(response);
        //    }

        //    switch (args.Action)
        //    {
        //        case "read":
        //            // Reads the file(s) or folder(s) from the given path.
        //            return operation.ToCamelCase(operation.GetFiles(args.Path, false, args.Data));
        //        case "delete":
        //            // Deletes the selected file(s) or folder(s) from the given path.
        //            return operation.ToCamelCase(operation.Delete(args.Path, args.Names, args.Data));
        //        case "details":
        //            // Gets the details of the selected file(s) or folder(s).
        //            return operation.ToCamelCase(operation.Details(args.Path, args.Names, args.Data));
        //        case "create":
        //            // Creates a new folder in a given path.
        //            return operation.ToCamelCase(operation.Create(args.Path, args.Name, args.Data));
        //        case "search":
        //            // Gets the list of file(s) or folder(s) from a given path based on the searched key string.
        //            return operation.ToCamelCase(operation.Search(args.Path, args.SearchString, args.ShowHiddenItems, args.CaseSensitive, args.Data));
        //        case "rename":
        //            // Renames a file or folder.
        //            return operation.ToCamelCase(operation.Rename(args.Path, args.Name, args.NewName, false, args.Data));
        //        case "move":
        //            // Cuts the selected file(s) or folder(s) from a path and then pastes them into a given target path.
        //            return operation.ToCamelCase(operation.Move(args.Path, args.TargetPath, args.Names, args.RenameFiles, args.TargetData, args.Data));
        //        case "copy":
        //            // Copies the selected file(s) or folder(s) from a path and then pastes them into a given target path.
        //            return operation.ToCamelCase(operation.Copy(args.Path, args.TargetPath, args.Names, args.RenameFiles, args.TargetData, args.Data));
        //    }
        //    return null;
        //}

        //[HttpPost]
        //public IActionResult SQLFileOperations_2(FileManagerDirectoryContent args)
        //{
        //    if (args.Action == "Edit")
        //    {
        //        operation.Edit(args.Id, args.StorageLocation, args.DateEx);
        //    }
        //    return new OkObjectResult(args);
        //}

        //// Uploads the file(s) into a specified path
        //public IActionResult SQLUpload(string path, IList<IFormFile> uploadFiles, string action, string data, string storageLocation, string dateEx)
        //{
        //    FileManagerResponse uploadResponse;
        //    FileManagerDirectoryContent[] dataObject = new FileManagerDirectoryContent[1];
        //    dataObject[0] = JsonConvert.DeserializeObject<FileManagerDirectoryContent>(data);
        //    dataObject[0].DateEx = dateEx;
        //    dataObject[0].StorageLocation = storageLocation;
        //    uploadResponse = operation.Upload(path, uploadFiles, action, dataObject);
        //    if (uploadResponse.Error != null)
        //    {
        //        Response.Clear();
        //        Response.ContentType = "application/json; charset=utf-8";
        //        Response.StatusCode = Convert.ToInt32(uploadResponse.Error.Code);
        //        Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = uploadResponse.Error.Message;
        //    }
        //    return Content("");
        //}

        //// Downloads the selected file(s) and folder(s)
        //public IActionResult SQLDownload(string downloadInput)
        //{
        //    FileManagerDirectoryContent args = JsonConvert.DeserializeObject<FileManagerDirectoryContent>(downloadInput);
        //    args.Path = (args.Path);
        //    return operation.Download(args.Path, args.Names, args.Data);
        //}

        //public IActionResult SQLGetImage(FileManagerDirectoryContent args)
        //{
        //    return operation.GetImage(args.Path, args.ParentID, args.Id, true, null, args.Data);
        //}

        #endregion

        #region physical file
        public PhysicalFileProvider operation;
        public string basePath;
        string root = "wwwroot\\FileData\\Ehs_Documents";

        [Obsolete]
        public FileManagerController(IHostingEnvironment hostingEnvironment, IRespository<EHS_FILES, int> filesRepository, IUnitOfWork unitOfWork, IMapper mapper, IMemoryCache memoryCache)
        {
            _filesRepository = filesRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _memoryCache = memoryCache;

            this.basePath = hostingEnvironment.ContentRootPath;
            this.operation = new PhysicalFileProvider();
            this.operation.RootFolder("D:\\HRMS_DOCUMENT");//(this.basePath + "\\" + this.root);
        }

        //[Route("FileOperations")] 
        public object FileOperations([FromBody] FileManagerDirectoryContent args)
        {
            if (args.Action == "delete" || args.Action == "rename")
            {
                if ((args.TargetPath == null) && (args.Path == ""))
                {
                    FileManagerResponse response = new FileManagerResponse();
                    response.Error = new ErrorDetails { Code = "401", Message = "Restricted to modify the root folder." };
                    return this.operation.ToCamelCase(response);
                }
            }
            switch (args.Action)
            {
                case "read":
                    // reads the file(s) or folder(s) from the given path.
                    return this.operation.ToCamelCase(this.operation.GetFiles(args.Path, args.ShowHiddenItems));
                case "delete":
                    UpdateFileResult(args.IsFile, args.Path, args.Names[0].ToString(), "", "", "delete");

                    // deletes the selected file(s) or folder(s) from the given path.
                    return this.operation.ToCamelCase(this.operation.Delete(args.Path, args.Names));
                case "copy":
                    UpdateFileResult(args.IsFile, args.Path, args.Names[0].ToString(), args.TargetPath, args.Names[0].ToString(), "copy");

                    // copies the selected file(s) or folder(s) from a path and then pastes them into a given target path.
                    return this.operation.ToCamelCase(this.operation.Copy(args.Path, args.TargetPath, args.Names, args.RenameFiles, args.TargetData));
                case "move":
                    UpdateFileResult(args.IsFile, args.Path, args.Names[0].ToString(), args.TargetPath, args.Names[0].ToString(), "move");

                    // cuts the selected file(s) or folder(s) from a path and then pastes them into a given target path.
                    return this.operation.ToCamelCase(this.operation.Move(args.Path, args.TargetPath, args.Names, args.RenameFiles, args.TargetData));
                case "details":
                    // gets the details of the selected file(s) or folder(s).
                    return this.operation.ToCamelCase(this.operation.Details(args.Path, args.Names, args.Data));
                case "create":
                    // creates a new folder in a given path.
                    return this.operation.ToCamelCase(this.operation.Create(args.Path, args.Name));
                case "search":
                    // gets the list of file(s) or folder(s) from a given path based on the searched key string.
                    return this.operation.ToCamelCase(this.operation.Search(args.Path, args.SearchString, args.ShowHiddenItems, args.CaseSensitive));
                case "rename":

                    UpdateFileResult(args.IsFile, args.Path, args.Name, args.Path, args.NewName, "rename");

                    // renames a file or folder.
                    return this.operation.ToCamelCase(this.operation.Rename(args.Path, args.Name, args.NewName));
            }
            return null;
        }

        // uploads the file(s) into a specified path
        //[Route("Upload")]
        public IActionResult Upload(string path, IList<IFormFile> uploadFiles, string action)
        {
            _memoryCache.TryGetValue("MaNgayChiTiet", out maNgayChiTiet);

            FileManagerResponse uploadResponse;
            List<EHS_FILES> files = new List<EHS_FILES>();
            EHS_FILES ehsFile;
            foreach (var file in uploadFiles)
            {
                var folders = (file.FileName).Split('/');
                // checking the folder upload
                if (folders.Length > 1)
                {
                    for (var i = 0; i < folders.Length - 1; i++)
                    {
                        string newDirectoryPath = Path.Combine(this.basePath + path, folders[i]);
                        if (!Directory.Exists(newDirectoryPath))
                        {
                            this.operation.ToCamelCase(this.operation.Create(path, folders[i]));
                        }
                        path += folders[i] + "/";
                    }
                }

                ehsFile = new EHS_FILES()
                {
                    FileName = file.FileName,
                    UrlFile = path,
                    MaNgayChiTiet = maNgayChiTiet
                };
                files.Add(ehsFile);
            }

            uploadResponse = operation.Upload(path, uploadFiles, action, null);
            if (uploadResponse.Error != null)
            {
                Response.Clear();
                Response.ContentType = "application/json; charset=utf-8";
                Response.StatusCode = Convert.ToInt32(uploadResponse.Error.Code);
                Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = uploadResponse.Error.Message;
            }
            else
            {
                _filesRepository.AddRange(files);
                _unitOfWork.Commit();
            }

            return Content("");
        }

        private void UpdateFileResult(bool isFile, string oldPath, string oldName, string newPath, string newName, string action)
        {
            _memoryCache.TryGetValue("MaNgayChiTiet", out maNgayChiTiet);

            if (action == "delete")
            {
                if (isFile)
                {
                    var file = _filesRepository.FindSingle(x => x.MaNgayChiTiet == maNgayChiTiet && x.FileName == oldName && x.UrlFile == oldPath);

                    if (file != null)
                        _filesRepository.Remove(file);
                }
                else
                {
                    var files = _filesRepository.FindAll(x => x.MaNgayChiTiet == maNgayChiTiet && x.UrlFile.Contains(oldPath)).ToList();
                    if (files != null && files.Count() > 0)
                        _filesRepository.RemoveMultiple(files);
                }

                _unitOfWork.Commit();
            }
            else
            {
                var file = _filesRepository.FindSingle(x => x.MaNgayChiTiet == maNgayChiTiet && x.FileName == oldName && x.UrlFile == oldPath);
                if (file != null)
                {
                    file.FileName = newName;
                    file.UrlFile = newPath;
                }

                _filesRepository.Update(file);
                _unitOfWork.Commit();
            }
        }

        // downloads the selected file(s) and folder(s)
        //[Route("Download")]
        public IActionResult Download(string downloadInput)
        {
            FileManagerDirectoryContent args = JsonConvert.DeserializeObject<FileManagerDirectoryContent>(downloadInput);
            return operation.Download(args.Path, args.Names, args.Data);
        }

        // gets the image(s) from the given path
        //[Route("GetImage")]
        public IActionResult GetImage(FileManagerDirectoryContent args)
        {
            return this.operation.GetImage(args.Path, args.Id, false, null, null);
        }
        #endregion

        [HttpPost]
        public IActionResult SaveMaNgayChiTiet(string key)
        {
            _memoryCache.Remove("MaNgayChiTiet");
            _memoryCache.Set("MaNgayChiTiet", key.NullString());
            var files = _filesRepository.FindAll(x => x.MaNgayChiTiet == key).ToList();
            if (files != null && files.Count > 0)
            {
                return new OkObjectResult(files[0].UrlFile);
            }

            return new OkObjectResult("/");
        }

        [HttpGet]
        public IActionResult ResetMaNgayChiTiet()
        {
            //_memoryCache.Remove("MaNgayChiTiet");
            //_memoryCache.Set("MaNgayChiTiet", "");

            return new OkObjectResult("/");
        }

        [HttpGet]
        public object GetfileUrl(DataSourceLoadOptions loadOptions, string key)
        {
            var files = _filesRepository.FindAll(x => x.MaNgayChiTiet == key).ToList();
            return DataSourceLoader.Load(files, loadOptions);
        }

        [HttpPost]
        public IActionResult GetUrlFileDetail(int id)
        {
            var file = _filesRepository.FindById(id);
            return new OkObjectResult(file.UrlFile);
        }
    }
}
