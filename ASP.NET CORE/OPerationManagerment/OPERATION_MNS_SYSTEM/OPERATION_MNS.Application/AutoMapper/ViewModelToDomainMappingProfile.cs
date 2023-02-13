using AutoMapper;
using OPERATION_MNS.Application.ViewModels;
using OPERATION_MNS.Application.ViewModels.System;
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
             .ConstructUsing(c => new DATE_OFF_LINE(c.ItemValue, c.OnOff, c.WLP));

            CreateMap<YieldOfModelViewModel, YIELD_OF_MODEL>()
            .ConstructUsing(c => new YIELD_OF_MODEL(c.Model, c.YieldPlan, c.YieldActual, c.YieldGap, c.Month));

            CreateMap<MaterialToSapViewModel, MATERIAL_TO_SAP>()
            .ConstructUsing(c => new MATERIAL_TO_SAP(c.Material, c.SAP_Code));

            CreateMap<CTQSettingViewModel, CTQ_SETTING>()
            .ConstructUsing(c => new CTQ_SETTING(c.OperationID, c.OperationName, c.LWL, c.UWL));

            CreateMap<CTQEmailReceivViewModel, CTQ_EMAIL_RECEIV>()
            .ConstructUsing(c => new CTQ_EMAIL_RECEIV(c.Id, c.Active, c.Department));

            CreateMap<PostOpeationShippingViewModel, POST_OPERATION_SHIPPING>()
            .ConstructUsing(c => new POST_OPERATION_SHIPPING(c.MoveOutTime,c.LotID,c.Model,c.CassetteID,c.Module,c.WaferId,c.DefaultChipQty,
            c.OutputQty,c.ChipMesQty,c.DiffMapMes,c.Rate,c.VanDeDacBiet,c.WaferQty,c.ChipQty,c.NguoiXuat,c.NguoiKiemTraFA,
            c.NguoiNhan,c.NguoiKiemTra,c.GhiChu_XH2,c.GhiChu_XH3,c.ChipMapQty,c.KetQuaFAKiemTra,c.WaferId_Mes));
        }
    }
}
