﻿using OPERATION_MNS.Data.Interfaces;
using OPERATION_MNS.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace OPERATION_MNS.Data.Entities
{
    [Table("GOC_PLAN_WLP2")]
    public class GOC_PLAN_WLP2 : DomainEntity<int>, IDateTracking
    {
        public GOC_PLAN_WLP2()
        {

        }

        public GOC_PLAN_WLP2(int id,string module,string model,string material,string division,float standarQty,string monthPlan,string datePlan,float qtyPlan,
            float qtyActual,float qtyGap,string dept,string unit,string danhmuc,string type)
        {
            Id = id;
            Module = module;
            Model = model;
            Material = material;
            Division = division;
            StandardQtyForMonth = standarQty;
            QuantityActual = qtyActual;
            MonthPlan = monthPlan;
            DatePlan = datePlan;
            QuantityPlan = qtyPlan;
            QuantityGap = qtyGap;
            Department = dept;
            Unit = unit;
            DanhMuc = danhmuc;
            Type = type;
        }

        [StringLength(50)]
        public string Module { get; set; }

        // SAP CODE
        [StringLength(50)]
        public string Model { get; set; }

        [StringLength(50)]
        public string Material { get; set; }

        [StringLength(50)]
        public string Division { get; set; }

        public float StandardQtyForMonth { get; set; }

        public string MonthPlan { get; set; }

        public string DatePlan { get; set; }

        public float QuantityPlan { get; set; }

        public float QuantityActual { get; set; }

        public float QuantityGap { get; set; }

        [StringLength(50)]
        public string Department { get; set; }

        [StringLength(50)]
        public string Unit { get; set; } // chip , wafe

        [StringLength(50)]
        public string DanhMuc { get; set; } // Nhập kho, sản xuất, xuất SMT

        [StringLength(50)]
        public string Type { get; set; }

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