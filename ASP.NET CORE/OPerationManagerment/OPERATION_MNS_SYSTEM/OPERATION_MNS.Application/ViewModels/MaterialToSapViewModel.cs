using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OPERATION_MNS.Application.ViewModels
{
    public class MaterialToSapViewModel
    {
        public int Id { get; set; }
        public string Material { get; set; }
        public string SAP_Code { get; set; }
    }
}
