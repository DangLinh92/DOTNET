using HRMNS.Data.Interfaces;
using HRMS.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HRMNS.Data.Entities
{
    [Table("HR_TRAINING")]
    public class HR_TRAINING : DomainEntity<Guid>, IDateTracking
    {
        public HR_TRAINING()
        {
        }

        public HR_TRAINING(Guid id,int trainType, string trainer,string fromdate,string todate,string desciption,float cost)
        {
            Id = id;
            TrainnigType = trainType;
            Trainer = trainer;
            FromDate = fromdate;
            ToDate = todate; 
            Description = desciption;
            Cost = cost;
        }

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

        [ForeignKey("TrainnigType")]
        public virtual TRAINING_TYPE TRAINING_TYPE { get; set; }

        [ForeignKey("MaEventParent")]
        public virtual EVENT_SHEDULE_PARENT EVENT_SHEDULE_PARENT { get; set; }

        public virtual ICollection<TRAINING_NHANVIEN> TRAINING_NHANVIEN { get; set; }
    }

    [Table("TRAINING_NHANVIEN")]
    public class TRAINING_NHANVIEN : DomainEntity<int>, IDateTracking
    {
        public TRAINING_NHANVIEN()
        {

        }

        public TRAINING_NHANVIEN(string maNV,Guid trainingId)
        {
            MaNV = maNV;
            TrainnigId = trainingId;
        }

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

        [ForeignKey("MaNV")]
        public virtual HR_NHANVIEN HR_NHANVIEN { get; set; }

        [ForeignKey("TrainnigId")]
        public virtual HR_TRAINING HR_TRAINING { get; set; }
    }

    [Table("TRAINING_TYPE")]
    public class TRAINING_TYPE : DomainEntity<int>, IDateTracking
    {
        public TRAINING_TYPE()
        {
        }

        public TRAINING_TYPE(string name, string desciption, string status)
        {
            TrainName = name;
            Description = desciption;
            Status = status;
        }

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

        public virtual ICollection<HR_TRAINING> HR_TRAINING { get; set; }
    }
}
