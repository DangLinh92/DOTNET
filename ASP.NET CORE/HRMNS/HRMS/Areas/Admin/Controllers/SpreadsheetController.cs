using DevExpress.AspNetCore.Spreadsheet;
using DevExpress.Spreadsheet;
using DevExpress.XtraSpreadsheet.Export;
using HRMS.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Areas.Admin.Controllers
{
    [IgnoreAntiforgeryToken]
    public class SpreadsheetController : AdminBaseController
    {
        private string DocumentName = "Tonghopnhansu.xlsx";
        const string XlsxContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        private readonly IWebHostEnvironment _hostingEnvironment;

        public SpreadsheetController(IWebHostEnvironment hostEnvironment)
        {
            _hostingEnvironment = hostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [HttpGet]
        public IActionResult DxDocumentRequest()
        {
            return SpreadsheetRequestProcessor.GetResponse(HttpContext);
        }

        public ActionResult Export(string fileName)
        {
            HttpContext.Session.Set("DocumentName", fileName);
            var sWebRootFolder = _hostingEnvironment.WebRootPath;
            string directory = Path.Combine(sWebRootFolder, "export-files");
            ViewData["DocumentPath"] = Path.Combine(directory, fileName);
            return View();
        }

        public IActionResult DownloadXlsx(SpreadsheetClientState spreadsheetState)
        {
            var spreadsheet = SpreadsheetRequestProcessor.GetSpreadsheetFromState(spreadsheetState);

            MemoryStream stream = new MemoryStream();
            spreadsheet.SaveCopy(stream, DocumentFormat.Xlsx);
            stream.Position = 0;
            DocumentName = HttpContext.Session.Get<string>("DocumentName");
            return File(stream, XlsxContentType, DocumentName);
        }

        public IActionResult DownloadHtml(SpreadsheetClientState spreadsheetState)
        {
            var spreadsheet = SpreadsheetRequestProcessor.GetSpreadsheetFromState(spreadsheetState);

            HtmlDocumentExporterOptions options = new HtmlDocumentExporterOptions();
            options.CssPropertiesExportType = DevExpress.XtraSpreadsheet.Export.Html.CssPropertiesExportType.Style;
            options.Encoding = Encoding.UTF8;
            options.EmbedImages = true;
            options.SheetIndex = spreadsheet.Document.Worksheets.ActiveWorksheet.Index;

            MemoryStream stream = new MemoryStream();
            spreadsheet.Document.ExportToHtml(stream, options);
            stream.Position = 0;
            DocumentName = HttpContext.Session.Get<string>("DocumentName");
            string fileHtml = DocumentName.Substring(0, DocumentName.LastIndexOf('.')) + ".html";
            return File(stream, "text/html", fileHtml);
        }
    }
}
