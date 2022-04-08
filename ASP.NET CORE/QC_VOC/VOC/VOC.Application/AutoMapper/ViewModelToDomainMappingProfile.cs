﻿using AutoMapper;
using VOC.Data.Entities;
using VOC.Application.ViewModels.VOC;

namespace VOC.Application.AutoMapper
{
    public class ViewModelToDomainMappingProfile : Profile
    {
        public ViewModelToDomainMappingProfile()
        {
            CreateMap<VOC_MSTViewModel, VOC_MST>()
           .ConstructUsing(c => new VOC_MST(
            c.Received_site,
            c.PlaceOfOrigin,
            c.ReceivedDept,
            c.ReceivedDate,
            c.SPLReceivedDate,
            c.SPLReceivedDateWeek,
            c.Customer,
            c.SETModelCustomer,
            c.ProcessCustomer,
            c.ModelFullname,
            c.DefectNameCus,
            c.DefectRate,
            c.PartsClassification,
            c.PartsClassification2,
            c.ProdutionDateMarking,
            c.AnalysisResult,
            c.VOCCount,
            c.DefectCause,
            c.DefectClassification,
            c.CustomerResponse,
            c.Report_FinalApprover,
            c.Report_Sender,
            c.Rport_sentDate,
            c.VOCState,
            c.VOCFinishingDate,
            c.VOC_TAT, c.PBA_FAE_Result));

            CreateMap<VOC_DefectTypeViewModel, VOC_DEFECT_TYPE>()
               .ConstructUsing(c => new VOC_DEFECT_TYPE(c.EngsNotation, c.KoreanNotation));

            CreateMap<VocPPMYearViewModel, VOC_PPM_YEAR>()
              .ConstructUsing(c => new VOC_PPM_YEAR(c.Id, c.Year, c.Module, c.ValuePPM, c.TargetPPM));

            CreateMap<VocPPMViewModel, VOC_PPM>()
              .ConstructUsing(c => new VOC_PPM(c.Id, c.Module, c.Customer, c.Type, c.Year, c.Month, c.Value, c.TargetValue));
        }
    }
}
