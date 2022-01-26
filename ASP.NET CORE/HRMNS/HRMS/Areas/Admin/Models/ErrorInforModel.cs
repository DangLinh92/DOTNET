using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS.Areas.Admin.Models
{
    public class ErrorInforModel
    {
        public string Code { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public string BackUrl { get; set; }
    }
}
