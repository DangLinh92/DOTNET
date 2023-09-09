using OPERATION_MNS.Data.Interfaces;
using OPERATION_MNS.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Text;

namespace OPERATION_MNS.Data.Entities
{
    [Table("TINH_HINH_SAN_XUAT_SAMPLE")]
    public class TINH_HINH_SAN_XUAT_SAMPLE : DomainEntity<int>, IDateTracking
    {
        public TINH_HINH_SAN_XUAT_SAMPLE()
        {
            DELAY_COMMENT_SAMPLES = new HashSet<DELAY_COMMENT_SAMPLE>();
        }

        public TINH_HINH_SAN_XUAT_SAMPLE(
            int id, int year, int month, int mucdokhancap, string model, string code, string phanloai, string modelDonLinhKien,
            string lotno, int qtyInput, int qtyNG, string operationNow, string mucDichNhap, string ghiChu, string nguoichiutrachnhiem, string inputDate, string outPutDate,
            string planInputDate, string planOutputDate, string wall_Plan_Date, string wall_Actual_Date, string roof_Plan_Date, string roof_Actual_Date, string seed_Plan_Date,
            string seed_Actual_Date, string platePR_Plan_Date, string platePR_Actual_Date, string plate_Plan_Date, string plate_Actual_Date, string preProbe_Plan_Date,
            string preProbe_Actual_Date, string preDicing_Plan_Date, string preDicing_Actual_Date, string allProbe_Plan_Date, string allProbe_Actual_Date, string bg_Plan_Date, string bg_Actual_Date,
            string dicing_Plan_Date, string dicing_Actual_Date, string chipIns_Plan_Date, string chipIns_Actual_Date, string packing_Plan_Date, string packing_Actual_Date, string oqc_Plan_Date,
            string oqc_Actual_Date, string shipping_Plan_Date, string shipping_Actual_Date, int leadTime, string deleteFlg, string planInputDateTcard,int outPutWafer, int leadTimePlan,string note_DayOff)
        {
            Id = id;
            Year = year;
            Month = month;
            MucDoKhanCap = mucdokhancap;
            Model = model;
            Code = code;
            PhanLoai = phanloai;
            ModelDonLinhKien = modelDonLinhKien;
            LotNo = lotno;
            QtyInput = qtyInput;
            QtyNG = qtyNG;
            OperationNow = operationNow;
            MucDichNhap = mucDichNhap;
            GhiChu = ghiChu;
            NguoiChiuTrachNhiem = nguoichiutrachnhiem;
            InputDate = inputDate;
            OutputDate = outPutDate;
            PlanInputDate = planInputDate;
            PlanOutputDate = planOutputDate;
            Wall_Plan_Date = wall_Plan_Date;
            Wall_Actual_Date = wall_Actual_Date;
            Roof_Plan_Date = roof_Plan_Date;
            Roof_Actual_Date = roof_Actual_Date;
            Seed_Plan_Date = seed_Plan_Date;
            Seed_Actual_Date = seed_Actual_Date;
            PlatePR_Plan_Date = platePR_Plan_Date;
            PlatePR_Actual_Date = platePR_Actual_Date;
            Plate_Plan_Date = plate_Plan_Date;
            Plate_Actual_Date = plate_Actual_Date;
            PreProbe_Plan_Date = preProbe_Plan_Date;
            PreProbe_Actual_Date = preProbe_Actual_Date;
            PreDicing_Plan_Date = preDicing_Plan_Date;
            PreDicing_Actual_Date = preDicing_Actual_Date;
            AllProbe_Plan_Date = allProbe_Plan_Date;
            AllProbe_Actual_Date = allProbe_Actual_Date;
            BG_Plan_Date = bg_Plan_Date;
            BG_Actual_Date = bg_Actual_Date;
            Dicing_Plan_Date = dicing_Plan_Date;
            Dicing_Actual_Date = dicing_Actual_Date;
            ChipIns_Plan_Date = chipIns_Plan_Date;
            ChipIns_Actual_Date = chipIns_Actual_Date;
            Packing_Plan_Date = packing_Plan_Date;
            Packing_Actual_Date = packing_Actual_Date;
            OQC_Plan_Date = oqc_Plan_Date;
            OQC_Actual_Date = oqc_Actual_Date;
            Shipping_Plan_Date = shipping_Plan_Date;
            Shipping_Actual_Date = shipping_Actual_Date;
            LeadTime = leadTime;
            DeleteFlg = deleteFlg;
            PlanInputDateTcard = planInputDateTcard;
            OutPutWafer = outPutWafer;
            LeadTimePlan = leadTimePlan;
            Note_DayOff = note_DayOff;
        }

        public int Year { get; set; }

        public int Month { get; set; }

        public int MucDoKhanCap { get; set; }

        [StringLength(50)]
        public string Model { get; set; }

        [StringLength(50)]
        public string Code { get; set; }

        [StringLength(50)]
        public string PhanLoai { get; set; }

        [StringLength(50)]
        public string ModelDonLinhKien { get; set; }

        [StringLength(50)]
        public string LotNo { get; set; }

        public int QtyInput { get; set; }

        public int QtyNG { get; set; }

        [StringLength(50)]
        public string OperationNow { get; set; }

        [StringLength(50)]
        public string MucDichNhap { get; set; }

        [StringLength(250)]
        public string GhiChu { get; set; }

        [StringLength(50)]
        public string NguoiChiuTrachNhiem { get; set; }

        [StringLength(50)]
        public string InputDate { get; set; }

        [StringLength(50)]
        public string OutputDate { get; set; }

        [StringLength(50)]
        public string PlanInputDate { get; set; }

        [StringLength(50)]
        public string PlanOutputDate { get; set; }


        [StringLength(50)]
        public string Wall_Plan_Date { get; set; }

        [StringLength(50)]
        public string Wall_Actual_Date { get; set; }


        [StringLength(50)]
        public string Roof_Plan_Date { get; set; }

        [StringLength(50)]
        public string Roof_Actual_Date { get; set; }


        [StringLength(50)]
        public string Seed_Plan_Date { get; set; }

        [StringLength(50)]
        public string Seed_Actual_Date { get; set; }


        [StringLength(50)]
        public string PlatePR_Plan_Date { get; set; }

        [StringLength(50)]
        public string PlatePR_Actual_Date { get; set; }


        [StringLength(50)]
        public string Plate_Plan_Date { get; set; }

        [StringLength(50)]
        public string Plate_Actual_Date { get; set; }


        [StringLength(50)]
        public string PreProbe_Plan_Date { get; set; }

        [StringLength(50)]
        public string PreProbe_Actual_Date { get; set; }


        [StringLength(50)]
        public string PreDicing_Plan_Date { get; set; }

        [StringLength(50)]
        public string PreDicing_Actual_Date { get; set; }


        [StringLength(50)]
        public string AllProbe_Plan_Date { get; set; }

        [StringLength(50)]
        public string AllProbe_Actual_Date { get; set; }


        [StringLength(50)]
        public string BG_Plan_Date { get; set; }

        [StringLength(50)]
        public string BG_Actual_Date { get; set; }


        [StringLength(50)]
        public string Dicing_Plan_Date { get; set; }

        [StringLength(50)]
        public string Dicing_Actual_Date { get; set; }


        [StringLength(50)]
        public string ChipIns_Plan_Date { get; set; }

        [StringLength(50)]
        public string ChipIns_Actual_Date { get; set; }


        [StringLength(50)]
        public string Packing_Plan_Date { get; set; }

        [StringLength(50)]
        public string Packing_Actual_Date { get; set; }


        [StringLength(50)]
        public string OQC_Plan_Date { get; set; }

        [StringLength(50)]
        public string OQC_Actual_Date { get; set; }


        [StringLength(50)]
        public string Shipping_Plan_Date { get; set; }

        [StringLength(50)]
        public string Shipping_Actual_Date { get; set; }

        public int LeadTime { get; set; }

        public int LeadTimePlan { get; set; }

        [StringLength(5)]
        public string DeleteFlg { get; set; }

        [StringLength(50)]
        public string DateCreated { get; set; }

        [StringLength(50)]
        public string DateModified { get; set; }

        [StringLength(50)]
        public string UserCreated { get; set; }

        [StringLength(50)]
        public string UserModified { get; set; }

        [StringLength(50)]
        public string PlanInputDateTcard { get; set; }

        [StringLength(250)]
        public string Note_DayOff { get; set; }

        // actual output
        public int OutPutWafer { get; set; }

        public ICollection<DELAY_COMMENT_SAMPLE> DELAY_COMMENT_SAMPLES { get; set; }

    }
}
