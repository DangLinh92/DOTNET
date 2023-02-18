using System;
using System.Collections.Generic;
using System.Text;

namespace HRMNS.Application.ViewModels.EHS
{
    public class KanbanViewModel
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Priority { get; set; }
        public int Progress { get; set; }
        public string Status { get; set; }
        public string IsShowBoard { get; set; }
        public string ActualFinish { get; set; }
        public string BeginTime { get; set; }
        public string NguoiPhuTrach { get; set; }
    }
}
