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

    public class Task
    {
        public int STT { get; set; }
        public string NoiDung { get; set; }
        public string NgayBatDau { get; set; }
        public string Status { get; set; }
        public string KetQua { get; set; }
        public string NguoiPhuTrach { get; set; }
    }

    // Thống kê dữ liệu của task
    public class TaskStatistics
    {
        public TaskStatistics()
        {
            Tasks = new List<Task>();
        }
        public List<Task> Tasks { get; set; }
        public string MaKeHoach { get; set; }
        public int TotalTask { get; set; }
        public int OverdueTask { get; set; }
        public int CompleteTask { get; set; }
        public int InprogressTask { get; set; }
        public int PendingTask { get; set; }
        public int HoldTask { get; set; }

        public int NGTask { get; set; }

        public decimal PercenCompleteTask
        {
            get
            {
                if (TotalTask > 0)
                    return Math.Round(100*(decimal)CompleteTask / TotalTask);

                return 0;
            }
        }
        public decimal PercenInprogressTask
        {
            get
            {
                if (TotalTask > 0)
                    return Math.Round(100 * (decimal)InprogressTask / TotalTask);

                return 0;
            }
        }
        public decimal PercenPendingTask
        {
            get
            {
                if (TotalTask > 0)
                    return Math.Round(100 * (decimal)PendingTask / TotalTask);

                return 0;
            }
        }

        public decimal PercenHoldTask { get => 100 - PercenCompleteTask - PercenInprogressTask - PercenPendingTask; }
    }
}
