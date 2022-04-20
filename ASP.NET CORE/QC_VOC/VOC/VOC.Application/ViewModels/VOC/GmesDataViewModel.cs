using System;
using System.Collections.Generic;
using System.Text;

namespace VOC.Application.ViewModels.VOC
{
    public class GmesDataViewModel
    {
        public GmesDataViewModel()
        {
            vocPPMYearViewModels = new List<VocPPMYearViewModel>();
            vocPPMViewModels = new List<VocPPMViewModel>();
        }

        public List<VocPPMYearViewModel> vocPPMYearViewModels { get; set; }
        public List<VocPPMViewModel> vocPPMViewModels { get; set; }

        public string Year { get; set; }
    }
}
