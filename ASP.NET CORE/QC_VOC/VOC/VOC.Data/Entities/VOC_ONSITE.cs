using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using VOC.Data.Interfaces;
using VOC.Infrastructure.SharedKernel;

namespace VOC.Data.Entities
{
    [Table("VOC_ONSITE")]
    public class VOC_ONSITE : DomainEntity<int>, IDateTracking
    {
        public VOC_ONSITE()
        {

        }

        public VOC_ONSITE(int id,int month,string week,string date,string part,string customer_code, string wisol_model, string customer,int qty, 
            string marking, string setModel, string ok,string ng,string not_measure, string result,string productiondate,string note,string customerDefectName)
        {
            Id = id;
            Month = month;
            Week = week;
            Date = date;
            Part = part;
            Customer_Code = customer_code;
            Wisol_Model = wisol_model;
            Customer = customer;
            Qty = qty;
            Marking = marking;
            SetModel = setModel;
            OK = ok;
            NG = ng;
            Not_Measure = not_measure;
            Result = result;
            ProductionDate = productiondate;
            Note = note;
            CustomerDefectName = customerDefectName;
        }

        public int Month { get; set; }

        [StringLength(50)]
        public string Week { get; set; }

        [StringLength(50)]
        public string Date { get; set; }

        [StringLength(50)]
        public string Part { get; set; }

        [StringLength(50)]
        public string Customer_Code { get; set; }

        [StringLength(50)]
        public string Wisol_Model { get; set; }

        [StringLength(50)]
        public string Customer { get; set; }
        public int Qty { get; set; }

        [StringLength(250)]
        public string Marking { get; set; }

        [StringLength(50)]
        public string SetModel { get; set; }

        [StringLength(10)]
        public string OK { get; set; }

        [StringLength(10)]
        public string NG { get; set; }

        [StringLength(10)]
        public string Not_Measure { get; set; }

        [StringLength(10)]
        public string Result { get; set; }

        [StringLength(50)]
        public string ProductionDate { get; set; }

        [StringLength(500)]
        public string Note { get; set; }

        [StringLength(150)]
        public string CustomerDefectName { get; set; }

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
