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

    [Table("SAMSUNG_TRAINING")]
    public class SAMSUNG_TRAINING : DomainEntity<int>, IDateTracking
    {
        public SAMSUNG_TRAINING()
        {
        }

        public SAMSUNG_TRAINING(string _SubNo, string _CoursesName,string _Level, string _Program, string _Category,
                                string _Class, string _Type, string _Year, string _Month, string _Week, string _Date,
                                string _TrainingPlace, string _TrainingRoom, string _GEN, string _Name,
                                string _Company, string _Part, string _Title, string _Score, string _Result, string _Remarks)
        {
            SubNo = _SubNo;
            CoursesName = _CoursesName;
            Program = _Program;
            Level = _Level;
            Category = _Category;
            Class = _Class;
            Type = _Type;
            Year = _Year;
            Month = _Month;
            Week = _Week;
            Date = _Date;
            TrainingPlace = _TrainingPlace;
            TrainingRoom = _TrainingRoom;
            GEN = _GEN;
            Name = _Name;
            Company = _Company;
            Part = _Part;
            Title = _Title;
            Score = _Score;
            Result = _Result;
            Remarks = _Remarks;
        }

        [StringLength(10)]
        public string SubNo { get; set; }

        [StringLength(150)]
        public string CoursesName { get; set; }

        [StringLength(50)]
        public string Level { get; set; }

        [StringLength(50)]
        public string Program { get; set; }

        [StringLength(50)]
        public string Category { get; set; }

        [StringLength(50)]
        public string Class { get; set; }

        [StringLength(50)]
        public string Type { get; set; }

        [StringLength(50)]
        public string Year { get; set; }

        [StringLength(50)]
        public string Month { get; set; }

        [StringLength(50)]
        public string Week { get; set; }

        [StringLength(50)]
        public string Date { get; set; }

        [StringLength(50)]
        public string TrainingPlace { get; set; }

        [StringLength(50)]
        public string TrainingRoom { get; set; }

        [StringLength(50)]
        public string GEN { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(50)]
        public string Company { get; set; }

        [StringLength(50)]
        public string Part { get; set; }

        [StringLength(50)]
        public string Title { get; set; }

        [StringLength(50)]
        public string Score { get; set; }

        [StringLength(50)]
        public string Result { get; set; }

        [StringLength(250)]
        public string Remarks { get; set; }

        [StringLength(50)]
        public string DateCreated { get; set; }

        [StringLength(50)]
        public string DateModified { get; set; }

        [StringLength(50)]
        public string UserCreated { get; set; }

        [StringLength(50)]
        public string UserModified { get; set; }

    }
}
