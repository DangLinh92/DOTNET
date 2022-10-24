using HRMNS.Application.ViewModels.HR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HRMNS.Application.ViewModels.System
{
    public class TrainingTypeViewModel
    {
        public TrainingTypeViewModel()
        {

        }
        public TrainingTypeViewModel(string name, string desciption, string status)
        {
            TrainName = name;
            Description = desciption;
            Status = status;
        }

        public int Id { get; set; }

        [StringLength(250)]
        public string TrainName { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        [StringLength(50)]
        public string Status { get; set; }

        [StringLength(50)]
        public string DateCreated { get; set; }

        [StringLength(50)]
        public string DateModified { get; set; }

        [StringLength(50)]
        public string UserCreated { get; set; }

        [StringLength(50)]
        public string UserModified { get; set; }

        public ICollection<Hr_TrainingViewModel> HR_TRAINING { get; set; }
    }

    public class Hr_TrainingViewModel
    {
        public Hr_TrainingViewModel()
        {

        }
        public Hr_TrainingViewModel(Guid id,int trainType, string trainer, string fromdate, string todate, string desciption, float cost)
        {
            Id = id;
            TrainnigType = trainType;
            Trainer = trainer;
            FromDate = fromdate;
            ToDate = todate;
            Description = desciption;
            Cost = cost;
        }
        public Guid Id { get; set; }

        public int TrainnigType { get; set; }

        public Guid MaEventParent { get; set; }

        [StringLength(250)]
        public string Trainer { get; set; }

        [StringLength(50)]
        public string FromDate { get; set; }

        [StringLength(50)]
        public string ToDate { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        public float Cost { get; set; }

        [StringLength(50)]
        public string DateCreated { get; set; }

        [StringLength(50)]
        public string DateModified { get; set; }

        [StringLength(50)]
        public string UserCreated { get; set; }

        [StringLength(50)]
        public string UserModified { get; set; }

        public TrainingTypeViewModel TRAINING_TYPE { get; set; }

        public EventScheduleParentViewModel EVENT_SHEDULE_PARENT { get; set; }

        public ICollection<Training_NhanVienViewModel> TRAINING_NHANVIEN { get; set; }
    }

    public class Training_NhanVienViewModel
    {
        public Training_NhanVienViewModel()
        {

        }
        public Training_NhanVienViewModel(string maNV, Guid trainingId)
        {
            MaNV = maNV;
            TrainnigId = trainingId;
        }

        public int Id { get; set; }

        [StringLength(50)]
        public string MaNV { get; set; }

        public Guid TrainnigId { get; set; }

        [StringLength(50)]
        public string DateCreated { get; set; }

        [StringLength(50)]
        public string DateModified { get; set; }

        [StringLength(50)]
        public string UserCreated { get; set; }

        [StringLength(50)]
        public string UserModified { get; set; }

        public NhanVienViewModel HR_NHANVIEN { get; set; }

        public Hr_TrainingViewModel HR_TRAINING { get; set; }
    }
}
