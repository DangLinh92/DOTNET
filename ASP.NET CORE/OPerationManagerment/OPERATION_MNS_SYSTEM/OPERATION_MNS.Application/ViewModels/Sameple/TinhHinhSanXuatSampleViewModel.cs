using OPERATION_MNS.Data.EF.Extensions;
using OPERATION_MNS.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace OPERATION_MNS.Application.ViewModels.Sameple
{
    public class TinhHinhSanXuatSampleViewModel
    {
        public TinhHinhSanXuatSampleViewModel()
        {
            DELAY_COMMENT_SAMPLES = new List<DELAY_COMMENT_SAMPLE>();
        }
        public int Id { get; set; }
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
        public string Note_Plan_Actual { get; set; }

        [StringLength(250)]
        public string Note_DayOff { get; set; }

        public ObjView ViewLeadTime => new ObjView()
        {
            Plan = LeadTimePlan,
            Actual = LeadTime
        };

        [StringLength(50)]
        public string NguoiChiuTrachNhiem { get; set; }

        [StringLength(50)]
        public string InputDate { get; set; }

        [StringLength(50)]
        public string OutputDate { get; set; }

        [StringLength(50)]
        public string PlanInputDate { get; set; }

        private DateTime planInputDate_1;
        public DateTime? PlanInputDate_1
        {
            get
            {
                var date = PlanInputDate.NullString() != "" ? PlanInputDate.Substring(0, 4) + "-" + PlanInputDate.Substring(4, 2) + "-" + PlanInputDate.Substring(6, 2) : "";
                if (DateTime.TryParse(date, out planInputDate_1))
                    return planInputDate_1;
                return null;
            }
            set
            {
                if (value != null)
                    planInputDate_1 = (DateTime)value;
            }
        }

        [StringLength(50)]
        public string PlanOutputDate { get; set; }


        [StringLength(50)]
        public string Wall_Plan_Date { get; set; }

        private DateTime wall_Plan_Date_1;
        public DateTime? Wall_Plan_Date_1
        {
            get
            {
                var date = Wall_Plan_Date.NullString() != "" ? Wall_Plan_Date.Substring(0, 4) + "-" + Wall_Plan_Date.Substring(4, 2) + "-" + Wall_Plan_Date.Substring(6, 2) : "";
                if (DateTime.TryParse(date, out wall_Plan_Date_1))
                    return wall_Plan_Date_1;
                return null;
            }
            set
            {
                if (value != null)
                    wall_Plan_Date_1 = (DateTime)value;
            }
        }

        [StringLength(50)]
        public string Wall_Actual_Date { get; set; }

        [StringLength(50)]
        public string Roof_Plan_Date { get; set; }

        private DateTime roof_Plan_Date_1;
        public DateTime? Roof_Plan_Date_1
        {
            get
            {
                var date = Roof_Plan_Date.NullString() != "" ? Roof_Plan_Date.Substring(0, 4) + "-" + Roof_Plan_Date.Substring(4, 2) + "-" + Roof_Plan_Date.Substring(6, 2) : "";
                if (DateTime.TryParse(date, out roof_Plan_Date_1))
                    return roof_Plan_Date_1;
                return null;
            }
            set
            {
                if (value != null)
                    roof_Plan_Date_1 = (DateTime)value;
            }
        }

        [StringLength(50)]
        public string Roof_Actual_Date { get; set; }


        [StringLength(50)]
        public string Seed_Plan_Date { get; set; }

        private DateTime seed_Plan_Date_1;
        public DateTime? Seed_Plan_Date_1
        {
            get
            {
                var date = Seed_Plan_Date.NullString() != "" ? Seed_Plan_Date.Substring(0, 4) + "-" + Seed_Plan_Date.Substring(4, 2) + "-" + Seed_Plan_Date.Substring(6, 2) : "";
                if (DateTime.TryParse(date, out seed_Plan_Date_1))
                    return seed_Plan_Date_1;
                return null;
            }
            set
            {
                if (value != null)
                    seed_Plan_Date_1 = (DateTime)value;
            }
        }

        [StringLength(50)]
        public string Seed_Actual_Date { get; set; }


        [StringLength(50)]
        public string PlatePR_Plan_Date { get; set; }

        private DateTime platePR_Plan_Date_1;
        public DateTime? PlatePR_Plan_Date_1
        {
            get
            {
                var date = PlatePR_Plan_Date.NullString() != "" ? PlatePR_Plan_Date.Substring(0, 4) + "-" + PlatePR_Plan_Date.Substring(4, 2) + "-" + PlatePR_Plan_Date.Substring(6, 2) : "";
                if (DateTime.TryParse(date, out platePR_Plan_Date_1))
                    return platePR_Plan_Date_1;
                return null;
            }
            set
            {
                if (value != null)
                    platePR_Plan_Date_1 = (DateTime)value;
            }
        }


        [StringLength(50)]
        public string PlatePR_Actual_Date { get; set; }


        [StringLength(50)]
        public string Plate_Plan_Date { get; set; }

        private DateTime platePR_Actual_Date_1;
        public DateTime? Plate_Plan_Date_1
        {
            get
            {
                var date = Plate_Plan_Date.NullString() != "" ? Plate_Plan_Date.Substring(0, 4) + "-" + Plate_Plan_Date.Substring(4, 2) + "-" + Plate_Plan_Date.Substring(6, 2) : "";
                if (DateTime.TryParse(date, out platePR_Actual_Date_1))
                    return platePR_Actual_Date_1;
                return null;
            }
            set
            {
                if (value != null)
                    platePR_Actual_Date_1 = (DateTime)value;
            }
        }

        [StringLength(50)]
        public string Plate_Actual_Date { get; set; }


        [StringLength(50)]
        public string PreProbe_Plan_Date { get; set; }

        private DateTime preProbe_Plan_Date_1;
        public DateTime? PreProbe_Plan_Date_1
        {
            get
            {
                var date = PreProbe_Plan_Date.NullString() != "" ? PreProbe_Plan_Date.Substring(0, 4) + "-" + PreProbe_Plan_Date.Substring(4, 2) + "-" + PreProbe_Plan_Date.Substring(6, 2) : "";
                if (DateTime.TryParse(date, out preProbe_Plan_Date_1))
                    return preProbe_Plan_Date_1;
                return null;
            }
            set
            {
                if (value != null)
                    preProbe_Plan_Date_1 = (DateTime)value;
            }
        }

        [StringLength(50)]
        public string PreProbe_Actual_Date { get; set; }

        private DateTime preProbe_Actual_Date_1;
        public DateTime? PreProbe_Actual_Date_1
        {
            get
            {
                var date = PreProbe_Actual_Date.NullString() != "" ? PreProbe_Actual_Date.Substring(0, 4) + "-" + PreProbe_Actual_Date.Substring(4, 2) + "-" + PreProbe_Actual_Date.Substring(6, 2) : "";
                if (DateTime.TryParse(date, out preProbe_Actual_Date_1))
                    return preProbe_Actual_Date_1;
                return null;
            }
            set
            {
                if (value != null)
                    preProbe_Actual_Date_1 = (DateTime)value;
            }
        }

        [StringLength(50)]
        public string PreDicing_Plan_Date { get; set; }

        private DateTime preDicing_Plan_Date_1;
        public DateTime? PreDicing_Plan_Date_1
        {
            get
            {
                var date = PreDicing_Plan_Date.NullString() != "" ? PreDicing_Plan_Date.Substring(0, 4) + "-" + PreDicing_Plan_Date.Substring(4, 2) + "-" + PreDicing_Plan_Date.Substring(6, 2) : "";
                if (DateTime.TryParse(date, out preDicing_Plan_Date_1))
                    return preDicing_Plan_Date_1;

                return null;
            }
            set
            {
                if (value != null)
                    preDicing_Plan_Date_1 = (DateTime)value;
            }
        }

        [StringLength(50)]
        public string PreDicing_Actual_Date { get; set; }

        private DateTime preDicing_Actual_Date_1;
        public DateTime? PreDicing_Actual_Date_1
        {
            get
            {
                var date = PreDicing_Actual_Date.NullString() != "" ? PreDicing_Actual_Date.Substring(0, 4) + "-" + PreDicing_Actual_Date.Substring(4, 2) + "-" + PreDicing_Actual_Date.Substring(6, 2) : "";
                if (DateTime.TryParse(date, out preDicing_Actual_Date_1))
                    return preDicing_Actual_Date_1;
                return null;
            }
            set
            {
                if (value != null)
                    preDicing_Actual_Date_1 = (DateTime)value;
            }
        }

        [StringLength(50)]
        public string AllProbe_Plan_Date { get; set; }

        private DateTime allProbe_Plan_Date_1;
        public DateTime? AllProbe_Plan_Date_1
        {
            get
            {
                var date = AllProbe_Plan_Date.NullString() != "" ? AllProbe_Plan_Date.Substring(0, 4) + "-" + AllProbe_Plan_Date.Substring(4, 2) + "-" + AllProbe_Plan_Date.Substring(6, 2) : "";
                if (DateTime.TryParse(date, out allProbe_Plan_Date_1))
                    return allProbe_Plan_Date_1;
                return null;
            }
            set
            {
                if (value != null)
                    allProbe_Plan_Date_1 = (DateTime)value;
            }
        }

        [StringLength(50)]
        public string AllProbe_Actual_Date { get; set; }


        [StringLength(50)]
        public string BG_Plan_Date { get; set; }

        private DateTime bG_Plan_Date_1;
        public DateTime? BG_Plan_Date_1
        {
            get
            {
                var date = BG_Plan_Date.NullString() != "" ? BG_Plan_Date.Substring(0, 4) + "-" + BG_Plan_Date.Substring(4, 2) + "-" + BG_Plan_Date.Substring(6, 2) : "";
                if (DateTime.TryParse(date, out bG_Plan_Date_1))
                    return bG_Plan_Date_1;
                return null;
            }
            set
            {
                if (value != null)
                    bG_Plan_Date_1 = (DateTime)value;
            }
        }

        [StringLength(50)]
        public string BG_Actual_Date { get; set; }


        [StringLength(50)]
        public string Dicing_Plan_Date { get; set; }

        private DateTime dicing_Plan_Date_1;
        public DateTime? Dicing_Plan_Date_1
        {
            get
            {
                var date = Dicing_Plan_Date.NullString() != "" ? Dicing_Plan_Date.Substring(0, 4) + "-" + Dicing_Plan_Date.Substring(4, 2) + "-" + Dicing_Plan_Date.Substring(6, 2) : "";
                if (DateTime.TryParse(date, out dicing_Plan_Date_1))
                    return dicing_Plan_Date_1;
                return null;
            }
            set
            {
                if (value != null)
                    dicing_Plan_Date_1 = (DateTime)value;
            }
        }

        [StringLength(50)]
        public string Dicing_Actual_Date { get; set; }


        [StringLength(50)]
        public string ChipIns_Plan_Date { get; set; }

        private DateTime chipIns_Plan_Date_1;
        public DateTime? ChipIns_Plan_Date_1
        {
            get
            {
                var date = ChipIns_Plan_Date.NullString() != "" ? ChipIns_Plan_Date.Substring(0, 4) + "-" + ChipIns_Plan_Date.Substring(4, 2) + "-" + ChipIns_Plan_Date.Substring(6, 2) : "";
                if (DateTime.TryParse(date, out chipIns_Plan_Date_1))
                    return chipIns_Plan_Date_1;

                return null;
            }
            set
            {
                if (value != null)
                    chipIns_Plan_Date_1 = (DateTime)value;
            }
        }

        [StringLength(50)]
        public string ChipIns_Actual_Date { get; set; }


        [StringLength(50)]
        public string Packing_Plan_Date { get; set; }

        private DateTime packing_Plan_Date_1;
        public DateTime? Packing_Plan_Date_1
        {
            get
            {
                var date = Packing_Plan_Date.NullString() != "" ? Packing_Plan_Date.Substring(0, 4) + "-" + Packing_Plan_Date.Substring(4, 2) + "-" + Packing_Plan_Date.Substring(6, 2) : "";
                if (DateTime.TryParse(date, out packing_Plan_Date_1))
                    return packing_Plan_Date_1;

                return null;
            }
            set
            {
                if (value != null)
                    packing_Plan_Date_1 = (DateTime)value;
            }
        }

        [StringLength(50)]
        public string Packing_Actual_Date { get; set; }


        [StringLength(50)]
        public string OQC_Plan_Date { get; set; }

        private DateTime oQC_Plan_Date_1;
        public DateTime? OQC_Plan_Date_1
        {
            get
            {
                var date = OQC_Plan_Date.NullString() != "" ? OQC_Plan_Date.Substring(0, 4) + "-" + OQC_Plan_Date.Substring(4, 2) + "-" + OQC_Plan_Date.Substring(6, 2) : "";
                if (DateTime.TryParse(date, out oQC_Plan_Date_1))
                    return oQC_Plan_Date_1;
                return null;
            }
            set
            {
                if (value != null)
                    oQC_Plan_Date_1 = (DateTime)value;
            }
        }

        [StringLength(50)]
        public string OQC_Actual_Date { get; set; }


        [StringLength(50)]
        public string Shipping_Plan_Date { get; set; }

        private DateTime shipping_Plan_Date_1;
        public DateTime? Shipping_Plan_Date_1
        {
            get
            {
                var date = Shipping_Plan_Date.NullString() != "" ? Shipping_Plan_Date.Substring(0, 4) + "-" + Shipping_Plan_Date.Substring(4, 2) + "-" + Shipping_Plan_Date.Substring(6, 2) : "";
                if (DateTime.TryParse(date, out shipping_Plan_Date_1))
                    return shipping_Plan_Date_1;

                return null;
            }
            set
            {
                if (value != null)
                    shipping_Plan_Date_1 = (DateTime)value;
            }
        }

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

        public int LeadTimeMax => LeadTimePlan > 0 ? LeadTimePlan : (PlanOutputDate.NullString() != "" ? DateTime.Parse(PlanOutputDate.Substring(0, 4) + "-" + PlanOutputDate.Substring(4, 2) + "-" + PlanOutputDate.Substring(6, 2)).Subtract(DateTime.Parse(PlanInputDate.Substring(0, 4) + "-" + PlanInputDate.Substring(4, 2) + "-" + PlanInputDate.Substring(6, 2))).Days : 0);

        public bool IsHightLight { get; set; }
        public int LevelHightLight { get; set; }

        public DayInOpeation OutPutDay => new DayInOpeation
        {
            Plan = PlanOutputDate.NullString() != "" ? PlanOutputDate.Substring(4, 2) + "-" + PlanOutputDate.Substring(6, 2) : "",
            Actual = OutputDate.NullString() != "" ? OutputDate.Substring(4, 2) + "-" + OutputDate.Substring(6, 2) : "",
            Plan2 = PlanOutputDate.NullString(),
            Actual2 = OutputDate.NullString()
        };

        public DayInOpeation InPutDay => new DayInOpeation
        {
            Actual = InputDate.NullString() != "" ? InputDate.Substring(4, 2) + "-" + InputDate.Substring(6, 2) : "",
            Plan = PlanInputDate.NullString() != "" ? PlanInputDate.Substring(4, 2) + "-" + PlanInputDate.Substring(6, 2) : "",
            Actual2 = InputDate.NullString(),
            Plan2 = PlanInputDate.NullString(),
        };

        public DayInOpeation WallDate => new DayInOpeation
        {
            Actual = Wall_Actual_Date.NullString() != "" ? Wall_Actual_Date.Substring(4, 2) + "-" + Wall_Actual_Date.Substring(6, 2) : "",
            Plan = Wall_Plan_Date.NullString() != "" ? Wall_Plan_Date.Substring(4, 2) + "-" + Wall_Plan_Date.Substring(6, 2) : "",
            Actual2 = Wall_Actual_Date.NullString(),
            Plan2 = Wall_Plan_Date.NullString(),
            Comment = DELAY_COMMENT_SAMPLES.FirstOrDefault()?.Wall
        };

        public DayInOpeation RoofDate => new DayInOpeation
        {
            Actual = Roof_Actual_Date.NullString() != "" ? Roof_Actual_Date.Substring(4, 2) + "-" + Roof_Actual_Date.Substring(6, 2) : "",
            Plan = Roof_Plan_Date.NullString() != "" ? Roof_Plan_Date.Substring(4, 2) + "-" + Roof_Plan_Date.Substring(6, 2) : "",
            Actual2 = Roof_Actual_Date.NullString(),
            Plan2 = Roof_Plan_Date.NullString(),
            Comment = DELAY_COMMENT_SAMPLES.FirstOrDefault()?.Roof
        };

        public DayInOpeation SeedDate => new DayInOpeation
        {
            Actual = Seed_Actual_Date.NullString() != "" ? Seed_Actual_Date.Substring(4, 2) + "-" + Seed_Actual_Date.Substring(6, 2) : "",
            Plan = Seed_Plan_Date.NullString() != "" ? Seed_Plan_Date.Substring(4, 2) + "-" + Seed_Plan_Date.Substring(6, 2) : "",
            Actual2 = Seed_Actual_Date.NullString(),
            Plan2 = Seed_Plan_Date.NullString(),
            Comment = DELAY_COMMENT_SAMPLES.FirstOrDefault()?.Seed
        };

        public DayInOpeation PlatePrDate => new DayInOpeation
        {
            Actual = PlatePR_Actual_Date.NullString() != "" ? PlatePR_Actual_Date.Substring(4, 2) + "-" + PlatePR_Actual_Date.Substring(6, 2) : "",
            Plan = PlatePR_Plan_Date.NullString() != "" ? PlatePR_Plan_Date.Substring(4, 2) + "-" + PlatePR_Plan_Date.Substring(6, 2) : "",
            Actual2 = PlatePR_Actual_Date.NullString(),
            Plan2 = PlatePR_Plan_Date.NullString(),
            Comment = DELAY_COMMENT_SAMPLES.FirstOrDefault()?.PlatePR
        };

        public DayInOpeation PlateDate => new DayInOpeation
        {
            Actual = Plate_Actual_Date.NullString() != "" ? Plate_Actual_Date.Substring(4, 2) + "-" + Plate_Actual_Date.Substring(6, 2) : "",
            Plan = Plate_Plan_Date.NullString() != "" ? Plate_Plan_Date.Substring(4, 2) + "-" + Plate_Plan_Date.Substring(6, 2) : "",
            Actual2 = Plate_Actual_Date.NullString(),
            Plan2 = Plate_Plan_Date.NullString(),
            Comment = DELAY_COMMENT_SAMPLES.FirstOrDefault()?.Plate
        };

        public DayInOpeation PreProbeDate => new DayInOpeation
        {
            Actual = PreProbe_Actual_Date.NullString() != "" ? PreProbe_Actual_Date.Substring(4, 2) + "-" + PreProbe_Actual_Date.Substring(6, 2) : "",
            Plan = PreProbe_Plan_Date.NullString() != "" ? PreProbe_Plan_Date.Substring(4, 2) + "-" + PreProbe_Plan_Date.Substring(6, 2) : "",
            Actual2 = PreProbe_Actual_Date.NullString(),
            Plan2 = PreProbe_Plan_Date.NullString(),
            Comment = DELAY_COMMENT_SAMPLES.FirstOrDefault()?.PreProbe
        };

        public DayInOpeation PreDicingDate => new DayInOpeation
        {
            Actual = PreDicing_Actual_Date.NullString() != "" ? PreDicing_Actual_Date.Substring(4, 2) + "-" + PreDicing_Actual_Date.Substring(6, 2) : "",
            Plan = PreDicing_Plan_Date.NullString() != "" ? PreDicing_Plan_Date.Substring(4, 2) + "-" + PreDicing_Plan_Date.Substring(6, 2) : "",
            Actual2 = PreDicing_Actual_Date.NullString(),
            Plan2 = PreDicing_Plan_Date.NullString(),
            Comment = DELAY_COMMENT_SAMPLES.FirstOrDefault()?.PreDicing
        };

        public DayInOpeation AllProbeDate => new DayInOpeation
        {
            Actual = AllProbe_Actual_Date.NullString() != "" ? AllProbe_Actual_Date.Substring(4, 2) + "-" + AllProbe_Actual_Date.Substring(6, 2) : "",
            Plan = AllProbe_Plan_Date.NullString() != "" ? AllProbe_Plan_Date.Substring(4, 2) + "-" + AllProbe_Plan_Date.Substring(6, 2) : "",
            Actual2 = AllProbe_Actual_Date.NullString(),
            Plan2 = AllProbe_Plan_Date.NullString(),
            Comment = DELAY_COMMENT_SAMPLES.FirstOrDefault()?.AllProbe
        };

        public DayInOpeation BGDate => new DayInOpeation
        {
            Actual = BG_Actual_Date.NullString() != "" ? BG_Actual_Date.Substring(4, 2) + "-" + BG_Actual_Date.Substring(6, 2) : "",
            Plan = BG_Plan_Date.NullString() != "" ? BG_Plan_Date.Substring(4, 2) + "-" + BG_Plan_Date.Substring(6, 2) : "",
            Actual2 = BG_Actual_Date.NullString(),
            Plan2 = BG_Plan_Date.NullString(),
            Comment = DELAY_COMMENT_SAMPLES.FirstOrDefault()?.BG
        };

        public DayInOpeation DicingDate => new DayInOpeation
        {
            Actual = Dicing_Actual_Date.NullString() != "" ? Dicing_Actual_Date.Substring(4, 2) + "-" + Dicing_Actual_Date.Substring(6, 2) : "",
            Plan = Dicing_Plan_Date.NullString() != "" ? Dicing_Plan_Date.Substring(4, 2) + "-" + Dicing_Plan_Date.Substring(6, 2) : "",
            Actual2 = Dicing_Actual_Date.NullString(),
            Plan2 = Dicing_Plan_Date.NullString(),
            Comment = DELAY_COMMENT_SAMPLES.FirstOrDefault()?.Dicing
        };

        public DayInOpeation ChipInsDate => new DayInOpeation
        {
            Actual = ChipIns_Actual_Date.NullString() != "" ? ChipIns_Actual_Date.Substring(4, 2) + "-" + ChipIns_Actual_Date.Substring(6, 2) : "",
            Plan = ChipIns_Plan_Date.NullString() != "" ? ChipIns_Plan_Date.Substring(4, 2) + "-" + ChipIns_Plan_Date.Substring(6, 2) : "",
            Actual2 = ChipIns_Actual_Date.NullString(),
            Plan2 = ChipIns_Plan_Date.NullString(),
            Comment = DELAY_COMMENT_SAMPLES.FirstOrDefault()?.ChipIns
        };

        public DayInOpeation PackingDate => new DayInOpeation
        {
            Actual = Packing_Actual_Date.NullString() != "" ? Packing_Actual_Date.Substring(4, 2) + "-" + Packing_Actual_Date.Substring(6, 2) : "",
            Plan = Packing_Plan_Date.NullString() != "" ? Packing_Plan_Date.Substring(4, 2) + "-" + Packing_Plan_Date.Substring(6, 2) : "",
            Actual2 = Packing_Actual_Date.NullString(),
            Plan2 = Packing_Plan_Date.NullString(),
            Comment = DELAY_COMMENT_SAMPLES.FirstOrDefault()?.Packing
        };

        public DayInOpeation OQCDate => new DayInOpeation
        {
            Actual = OQC_Actual_Date.NullString() != "" ? OQC_Actual_Date.Substring(4, 2) + "-" + OQC_Actual_Date.Substring(6, 2) : "",
            Plan = OQC_Plan_Date.NullString() != "" ? OQC_Plan_Date.Substring(4, 2) + "-" + OQC_Plan_Date.Substring(6, 2) : "",
            Actual2 = OQC_Actual_Date.NullString(),
            Plan2 = OQC_Plan_Date.NullString(),
            Comment = DELAY_COMMENT_SAMPLES.FirstOrDefault()?.OQC
        };

        public DayInOpeation ShippingDate => new DayInOpeation
        {
            Actual = Shipping_Actual_Date.NullString() != "" ? Shipping_Actual_Date.Substring(4, 2) + "-" + Shipping_Actual_Date.Substring(6, 2) : "",
            Plan = Shipping_Plan_Date.NullString() != "" ? Shipping_Plan_Date.Substring(4, 2) + "-" + Shipping_Plan_Date.Substring(6, 2) : "",
            Actual2 = Shipping_Actual_Date.NullString(),
            Plan2 = Shipping_Plan_Date.NullString(),
            Comment = DELAY_COMMENT_SAMPLES.FirstOrDefault()?.Shipping
        };

        public string PlanInputDateTcard { get; set; }

        private DateTime _planInputDateTcardDate;
        public DateTime? PlanInputDateTcardDate
        {
            get
            {
                var date = PlanInputDateTcard.NullString() != "" ? PlanInputDateTcard.Substring(0, 4) + "-" + PlanInputDateTcard.Substring(4, 2) + "-" + PlanInputDateTcard.Substring(6, 2) : "";
                if (DateTime.TryParse(date, out _planInputDateTcardDate))
                    return _planInputDateTcardDate;

                return null;
            }

            set
            {
                if (value != null)
                {
                    _planInputDateTcardDate = (DateTime)value;
                    PlanInputDateTcard = value?.ToString("yyyyMMdd");
                }

                else
                {
                    PlanInputDateTcard = "";
                }
            }
        }

        public int OutPutWafer { get; set; }

        public List<DELAY_COMMENT_SAMPLE> DELAY_COMMENT_SAMPLES { get; set; }
    }

    public class ObjView
    {
        public int Plan { get; set; }
        public int Actual { get; set; }
    }

    public class DayInOpeation
    {
        public string Plan { get; set; }
        public string Actual { get; set; }

        public string Plan2 { get; set; }
        public string Actual2 { get; set; }

        public string Comment { get; set; }

        public int Diff
        {
            get
            {
                if (Plan2.NullString() == "")
                {
                    return 1; // k màu
                }
                if (DateTime.Now.ToString("yyyyMMdd").CompareTo(Plan2) == 0)
                {
                    if (Actual2.NullString() != "")
                    {
                        return 0; // màu vàng , báo cần hoàn thành trong hôm nay
                    }

                    return 1; // k màu
                }
                else
                if (DateTime.Now.AddDays(-1).ToString("yyyyMMdd").CompareTo(Plan2) == 0)
                {
                    if (DateTime.Now.Hour < 8 && (Actual2.NullString() == ""))
                    {
                        return 0;
                    }
                    else
                    if (Plan2.CompareTo(Actual2) >= 0)
                    {
                        return 1;
                    }

                    return -1; // màu đỏ : quá hạn
                }
                else if (DateTime.Now.ToString("yyyyMMdd").CompareTo(Plan2) > 0)
                {
                    if (Plan2.CompareTo(Actual2) >= 0)
                    {
                        return 1;
                    }

                    if (Plan2.CompareTo(Actual2) < 0 || Actual2.NullString() == "")
                    {
                        return -1;
                    }
                }

                return 1;
            }
        }
    }
}
