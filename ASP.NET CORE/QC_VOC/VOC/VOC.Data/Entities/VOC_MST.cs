using VOC.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using VOC.Data.Interfaces;

namespace VOC.Data.Entities
{
    [Table("VOC_MST")]
    public class VOC_MST : DomainEntity<int>, IDateTracking
    {
        public VOC_MST()
        {
        }

        public VOC_MST(string received_site, string placeoforigin, string receiveddept,
            string receiveddate, string splreceiveddate, string splreceiveddateweek, string customer,
            string setmodelcustomer, string processcustomer, string modelFullname,
            string defectNameCus, string defectrate, string partsClassification, string partsclassification2,
            string produtiondatemarking, string analysisresult, string voccount, string defectcause,
            string defectclassification, string customerresponse,
            string report_finalapprover, string report_sender, string rport_sentdate, string vocstate,
            string vocfinishingdate, string voc_tat, string pba_fae_result, string link,string customerGroup,string produtionDate,string produtionDate_2,string receivedDate_2,string sPLReceivedDate_2)
        {
            Received_site = received_site;
            PlaceOfOrigin = placeoforigin;
            ReceivedDept = receiveddept;
            ReceivedDate = receiveddate;
            SPLReceivedDate = splreceiveddate;
            SPLReceivedDateWeek = splreceiveddateweek;
            Customer = customer;
            SETModelCustomer = setmodelcustomer;
            ProcessCustomer = processcustomer;
            ModelFullname = modelFullname;
            DefectNameCus = defectNameCus;
            DefectRate = defectrate;
            PartsClassification = partsClassification;
            PartsClassification2 = partsclassification2;
            ProdutionDateMarking = produtiondatemarking;
            AnalysisResult = analysisresult;
            VOCCount = voccount;
            DefectCause = defectcause;
            DefectClassification = defectclassification;
            CustomerResponse = customerresponse;
            Report_FinalApprover = report_finalapprover;
            Report_Sender = report_sender;
            Rport_sentDate = rport_sentdate;
            VOCState = vocstate;
            VOCFinishingDate = vocfinishingdate;
            VOC_TAT = voc_tat;
            PBA_FAE_Result = pba_fae_result;
            LinkReport = link;

            CustomerGroup = customerGroup;
            ProdutionDate = produtionDate;
            ProdutionDate_2 = produtionDate_2;
            ReceivedDate_2 = receivedDate_2;
            SPLReceivedDate_2 = sPLReceivedDate_2;
        }

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
        [StringLength(250)]
        public string ModelFullname { get; set; }

        // 불량명/현상 (고객사)
        [StringLength(500)]
        public string DefectNameCus { get; set; }

        // 불량률 (불량수/투입수)
        [StringLength(50)]
        public string DefectRate { get; set; }

        // PBA FAE 결과 
        // (해당사항 없으면 입력 X)
        [StringLength(50)]
        public string PBA_FAE_Result { get; set; }

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

        // VOC 처리시간 (TAT)
        [StringLength(50)]
        public string VOC_TAT { get; set; }

        // Received date (YY-MM)
        [StringLength(50)]
        public string ReceivedDate_2 { get; set; }

        // SPL 접수 일자 2
        [StringLength(50)]
        public string SPLReceivedDate_2 { get; set; }

        // 고객사 구분
        [StringLength(50)]
        public string CustomerGroup { get; set; }

        // 부품 생산일자
        [StringLength(50)]
        public string ProdutionDate { get; set; }

        // 부품 생산일자 2
        [StringLength(50)]
        public string ProdutionDate_2 { get; set; }

        [StringLength(1000)]
        public string LinkReport { get; set; }

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
