﻿using AutoMapper;
using OPERATION_MNS.Application.ViewModels;
using OPERATION_MNS.Application.ViewModels.Lfem;
using OPERATION_MNS.Application.ViewModels.Sameple;
using OPERATION_MNS.Application.ViewModels.System;
using OPERATION_MNS.Application.ViewModels.Wlp2;
using OPERATION_MNS.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace OPERATION_MNS.Application.AutoMapper
{
    public class ViewModelToDomainMappingProfile : Profile
    {
        public ViewModelToDomainMappingProfile()
        {
            CreateMap<RoleViewModel, APP_ROLE>()
          .ConstructUsing(c => new APP_ROLE(c.Name, c.Description));

            CreateMap<PermisstionViewModel, PERMISSION>()
         .ConstructUsing(c => new PERMISSION(c.Id, c.RoleId, c.FunctionId, c.CanCreate, c.CanRead, c.CanUpdate, c.CanDelete, c.ApproveL1, c.ApproveL2, c.ApproveL3));

            CreateMap<GocPlanViewModel, GOC_PLAN>()
                  .ConstructUsing(c => new GOC_PLAN(c.Id, c.Module, c.Model, c.Material, c.Division, c.StandardQtyForMonth, c.MonthPlan, c.DatePlan, c.QuantityPlan, c.QuantityActual, c.QuantityGap, c.Department, c.Unit));

            CreateMap<GocPlanViewModel, GOC_STANDAR_QTY>()
               .ConstructUsing(c => new GOC_STANDAR_QTY(c.Id, c.Module, c.Model, c.Material, c.Material, c.StandardQtyForMonth, c.Department, c.Unit));

            CreateMap<SettingItemsViewModel, SETTING_ITEMS>()
             .ConstructUsing(c => new SETTING_ITEMS(c.ItemValue, c.Id));

            CreateMap<DateOffLineViewModel, DATE_OFF_LINE>()
             .ConstructUsing(c => new DATE_OFF_LINE(c.ItemValue, c.OnOff, c.WLP, c.DanhMuc, c.OWNER));

            CreateMap<YieldOfModelViewModel, YIELD_OF_MODEL>()
            .ConstructUsing(c => new YIELD_OF_MODEL(c.Model, c.YieldPlan, c.YieldActual, c.YieldGap, c.Month));

            CreateMap<MaterialToSapViewModel, MATERIAL_TO_SAP>()
            .ConstructUsing(c => new MATERIAL_TO_SAP(c.Material, c.SAP_Code, c.Department));

            CreateMap<CTQSettingViewModel, CTQ_SETTING>()
            .ConstructUsing(c => new CTQ_SETTING(c.OperationID, c.OperationName, c.LWL, c.UWL));

            CreateMap<CTQSettingWLP2ViewModel, CTQ_SETTING_WLP2>()
           .ConstructUsing(c => new CTQ_SETTING_WLP2(c.SpeacialModel, c.OperationID, c.OperationName, c.ThickNet, c.MinV, c.MaxV));


            CreateMap<CTQEmailReceivViewModel, CTQ_EMAIL_RECEIV>()
            .ConstructUsing(c => new CTQ_EMAIL_RECEIV(c.Id, c.Active, c.Department));

            CreateMap<CTQEmailReceivViewModel, CTQ_EMAIL_RECEIV_WLP2>()
            .ConstructUsing(c => new CTQ_EMAIL_RECEIV_WLP2(c.Id, c.Active, c.Department));

            CreateMap<PostOpeationShippingViewModel, POST_OPERATION_SHIPPING>()
            .ConstructUsing(c => new POST_OPERATION_SHIPPING(c.MoveOutTime, c.LotID, c.Model, c.CassetteID, c.Module, c.WaferId, c.DefaultChipQty,
            c.OutputQty, c.ChipMesQty, c.DiffMapMes, c.Rate, c.VanDeDacBiet, c.WaferQty, c.ChipQty, c.NguoiXuat, c.NguoiKiemTraFA,
            c.NguoiNhan, c.NguoiKiemTra, c.GhiChu_XH2, c.GhiChu_XH3, c.ChipMapQty, c.KetQuaFAKiemTra, c.WaferId_Mes));

            CreateMap<GocPlanViewModel, GOC_PLAN_WLP2>()
                .ConstructUsing(c => new GOC_PLAN_WLP2(c.Id, c.Module, c.Model, c.Material, c.Division, c.StandardQtyForMonth,
                c.MonthPlan, c.DatePlan, c.QuantityPlan, c.QuantityActual, c.QuantityGap, c.Department, c.Unit, c.DanhMuc, c.Type));

            CreateMap<SmtReturnWlp2ViewModel, SMT_RETURN_WLP2>()
          .ConstructUsing(c => new SMT_RETURN_WLP2(c.Id, c.SapCode, c.SmtReturn));

            CreateMap<BoPhanDeNghiXuatNVLViewModel, BOPHAN_DE_NGHI_XUAT_NLIEU>()
        .ConstructUsing(c => new BOPHAN_DE_NGHI_XUAT_NLIEU(c.Id, c.BoPhanDeNghi, c.NgayDeNghi, c.SanPham, c.Module, c.SapCode, c.DinhMuc, c.DonVi, c.SoLuongYeuCau, c.LuongThucTe, c.Note));


            CreateMap<Stay_lot_list_priory_wlp2ViewModel, STAY_LOT_LIST_PRIORY_WLP2>()
       .ConstructUsing(c => new STAY_LOT_LIST_PRIORY_WLP2(c.Id, c.SapCode, c.Priory, c.Number_Priory, c.Material, c.CassetteID, c.LotID, c.OperationName, c.OperationId, c.ERPProductionOrder, c.ChipQty, c.StayDay));

            CreateMap<ThickNetModelWlp2ViewModel, THICKNET_MODEL_WLP2>()
       .ConstructUsing(c => new THICKNET_MODEL_WLP2(c.Id, c.Material, c.ThickNet));

            // sample
            CreateMap<TinhHinhSanXuatSampleViewModel, TINH_HINH_SAN_XUAT_SAMPLE>()
            .ConstructUsing(c => new TINH_HINH_SAN_XUAT_SAMPLE(c.Id, c.Year, c.Month, c.MucDoKhanCap, c.Model, c.Code, c.PhanLoai, c.ModelDonLinhKien, c.LotNo, c.QtyInput, c.QtyNG, c.OperationNow, c.MucDichNhap,
            c.GhiChu, c.NguoiChiuTrachNhiem, c.InputDate, c.OutputDate, c.PlanInputDate, c.PlanOutputDate, c.Wall_Plan_Date, c.Wall_Actual_Date, c.Roof_Plan_Date, c.Roof_Actual_Date, c.Seed_Plan_Date, c.Seed_Actual_Date,
            c.PlatePR_Plan_Date, c.PlatePR_Actual_Date, c.Plate_Plan_Date, c.Plate_Actual_Date, c.PreProbe_Plan_Date, c.PreProbe_Actual_Date, c.PreDicing_Plan_Date, c.PreDicing_Actual_Date, c.AllProbe_Plan_Date, c.AllProbe_Actual_Date,
            c.BG_Plan_Date, c.BG_Actual_Date, c.Dicing_Plan_Date, c.Dicing_Actual_Date, c.ChipIns_Plan_Date, c.ChipIns_Actual_Date, c.Packing_Plan_Date, c.Packing_Actual_Date, c.OQC_Plan_Date, c.OQC_Actual_Date, c.Shipping_Plan_Date,
            c.Shipping_Actual_Date, c.LeadTime, c.DeleteFlg, c.PlanInputDateTcard, c.OutPutWafer, c.LeadTimePlan, c.Note_DayOff));

            // lfem
            CreateMap<Stay_lot_list_priory_lfem_ViewModel, STAY_LOT_LIST_PRIORY_LFEM>()
     .ConstructUsing(c => new STAY_LOT_LIST_PRIORY_LFEM(c.Id, c.MesItem, c.Priory, c.StayDay, c.Size, c.LotID, c.ProductOrder, c.OperationName, c.OperationId, c.ChipQty,
                                                        c.FAID, c.AssyLotID, c.Date, c.DateDiff, c.Unit, c.StartFlag, c.EquipmentName, c.Worker, c.Number_Priory));
        }
    }
}
