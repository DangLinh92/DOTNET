using Aspose.Slides;
using Aspose.Slides.Export;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace VOC.Data.EF.Extensions
{
    public static class ConvertPPTToPdf
    {
        public static string UrlSavePdf = "wwwroot/uploaded/voc_issue/";
        public static void ConvertToPdf(string url)
        {
            FileInfo file = new FileInfo(url);

            if (file.Extension.ToLower().Contains(".pdf"))
            {
                return;
            }

            // Instantiate a Presentation object that represents a PPT file
            Presentation presentation = new Presentation(url);

            // Save the presentation as PDF
            presentation.Save(UrlSavePdf + Path.GetFileNameWithoutExtension(file.Name) + ".pdf", SaveFormat.Pdf);
        }
    }
}
