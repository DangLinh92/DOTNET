using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using VOC.Data.EF.Extensions;

namespace VOC.Application.ViewModels.VOC
{
    public class VOC_MSTViewModel
    {
        public int Id { get; set; }

        // 접수처 (HQ, WTC, WHC)
        [StringLength(50)]
        public string Received_site { get; set; }

        // 부품 생산지(HQ, WTC, WHC)
        [StringLength(50)]
        public string PlaceOfOrigin { get; set; }

        //  접수부서 (영업, 품질)
        [StringLength(50)]
        public string ReceivedDept { get; set; }

        // 접수일자1 (년/월/일)
        [StringLength(50)]
        public string ReceivedDate { get; set; }

        // 불량(SPL) 접수일자
        [StringLength(50)]
        public string SPLReceivedDate { get; set; }

        // 불량(SPL) 접수일자 
        // (주차), ISO8601
        [StringLength(50)]
        public string SPLReceivedDateWeek { get; set; }

        // 고객사
        [StringLength(150)]
        public string Customer { get; set; }

        // SET Model 
        // (고객사)
        [StringLength(100)]
        public string SETModelCustomer { get; set; }

        // 발생공정 
        // (고객사)
        [StringLength(250)]
        public string ProcessCustomer { get; set; }

        // 기종명(Full name)
        [StringLength(50)]
        public string ModelFullname { get; set; }

        // 불량명/현상 (고객사)
        [StringLength(500)]
        public string DefectNameCus { get; set; }

        // PBA FAE 결과 
        // (해당사항 없으면 입력 X)
        [StringLength(50)]
        public string PBA_FAE_Result { get; set; }

        // 불량률 (불량수/투입수)
        [StringLength(50)]
        public string DefectRate { get; set; }

        // 부품 구분1 
        // (SAW, 모듈)
        [StringLength(50)]
        public string PartsClassification { get; set; }

        // 부품 구분2 (샘플, 양산품)
        [StringLength(50)]
        public string PartsClassification2 { get; set; }

        // 부품 생산일자 (마킹)
        [StringLength(500)]
        public string ProdutionDateMarking { get; set; }

        // 불량명 
        // (불량분석 결과)
        [StringLength(500)]
        public string AnalysisResult { get; set; } // defect type

        // VOC Count
        // (진성 유/무)
        [StringLength(50)]
        public string VOCCount { get; set; }

        // 불량원인 (근본원인)
        [StringLength(500)]
        public string DefectCause { get; set; }

        // 원인구분 (5M+1E)
        [StringLength(100)]
        public string DefectClassification { get; set; }

        // 고객통보 불량원인
        [StringLength(500)]
        public string CustomerResponse { get; set; }

        // 대책서 최종 승인자
        [StringLength(50)]
        public string Report_FinalApprover { get; set; }

        // 대책서 회신 담당자
        [StringLength(50)]
        public string Report_Sender { get; set; }

        // 대책서 회신일자
        [StringLength(50)]
        public string Rport_sentDate { get; set; }

        // VOC State 
        // (Close, Open)
        public string VOCState { get; set; }

        // VOC 종결 일자
        [StringLength(50)]
        public string VOCFinishingDate { get; set; }

        public string VOCFinishingWeek
        {
            get
            {
                if (DateTime.TryParse(VOCFinishingDate, out _))
                    return "W" + (DateTime.Parse(VOCFinishingDate).GetWeekOfYear() - 1);

                if (DateTime.TryParse(SPLReceivedDate, out _))
                    return "W" + (DateTime.Parse(SPLReceivedDate).GetWeekOfYear() - 1);

                if (DateTime.TryParse(ReceivedDate, out _))
                    return "W" + (DateTime.Parse(ReceivedDate).GetWeekOfYear() - 1);

                return "W-1";
            }
        }

        public string SPLReceivedDateMonth
        {
            get
            {
                if (DateTime.TryParse(SPLReceivedDateWeek, out _))
                    return (DateTime.Parse(SPLReceivedDateWeek).GeMonthOfYear());

                if (DateTime.TryParse(ReceivedDate, out _))
                    return (DateTime.Parse(ReceivedDate).GeMonthOfYear());

                return "0";
            }
        }

        // VOC 처리시간 (TAT)
        [StringLength(50)]
        public string VOC_TAT { get; set; }

        public string DateCreated { get; set; }
        public string DateModified { get; set; }
        public string UserCreated { get; set; }
        public string UserModified { get; set; }
    }
}
