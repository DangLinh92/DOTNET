using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Timers;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using HrmsCopyDataService;
using OfficeOpenXml;
using System.IO;
using OfficeOpenXml.Style;
using System.Net;
using System.Security.Principal;
using System.Configuration;
//using static System.Net.Mime.MediaTypeNames;

namespace OMNS_Weebhook_Service
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }
        private Timer timer1 = null;
        private Timer timer2 = null;
        private Timer timer3 = null;
        private Timer timer4 = null;
        protected override void OnStart(string[] args)
        {
            count = 0;
            timer1 = new Timer();
            this.timer1.Interval = 60000; //every 1 p
            this.timer1.Elapsed += new System.Timers.ElapsedEventHandler(this.timer1_Tick);
            timer1.Enabled = true;

            timer2 = new Timer();
            this.timer2.Interval = 3600000; //every 1 h
            this.timer2.Elapsed += new ElapsedEventHandler(this.timer2_Tick);
            timer2.Enabled = true;

            timer3 = new Timer();
            this.timer3.Interval = 90000; //every 90s
            this.timer3.Elapsed += new ElapsedEventHandler(this.timer3_Tick);
            timer3.Enabled = true;

            timer4 = new Timer();
            this.timer4.Interval = 3600000; //every 1 h
            this.timer4.Elapsed += new ElapsedEventHandler(this.timer4_Tick);
            timer4.Enabled = true;

            LogFile.WriteErrorLog("window service START");
        }

        private void timer4_Tick(object sender, ElapsedEventArgs e)
        {
            CreateExcelSuatAn();
        }

        private async void timer3_Tick(object sender, ElapsedEventArgs e)
        {
            await ProcessDB_wlp2();
        }

        int count = 0;
        private async void timer2_Tick(object sender, ElapsedEventArgs e)
        {
            if (DateTime.Now.DayOfWeek == DayOfWeek.Monday && DateTime.Now.Hour > 7 && DateTime.Now.Hour < 9 && count == 0)
            {
                count++;
                await Process_EHS();
            }
            else
            {
                count = 0;
            }
        }

        private async void timer1_Tick(object sender, ElapsedEventArgs e)
        {
            await ProcessDB();
        }

        protected override void OnStop()
        {
            timer1.Enabled = false;
            timer2.Enabled = false;
            timer3.Enabled = false;
            timer4.Enabled = false;
            count = 0;
            LogFile.WriteErrorLog("window service stopped");
        }

        private async Task ProcessDB()
        {
            try
            {
                SQLService sql = new SQLService("Persist Security Info=True;Data Source = 10.70.10.97;Initial Catalog = OPERATION_MNSDB;User Id = sa;Password = Wisol@123;Connect Timeout=3");

                Dictionary<string, string> dic = new Dictionary<string, string>();

                ResultDB resultDB = sql.ExecProceduce2("PKG_BUSINESS@GET_CTQ_ERR", dic);
                System.Data.DataTable table = null;

                string tblHtmlTemp =
                                    @"<table bordercolor='black' border= '1'>
                                     <thead>
                                        <tr style = 'background-color : #004E89; color: White';text-align: center;>
                                           <th style ='border: 1px solid #ddd;padding: 8px;'>Material Id</th>
                                           <th style ='border: 1px solid #ddd;padding: 8px;'>Lot Id</th>
                                           <th style ='border: 1px solid #ddd;padding: 8px;'>Operation</th>
                                           <th style ='border: 1px solid #ddd;padding: 8px;'>Value 1</th>
	                                       <th style ='border: 1px solid #ddd;padding: 8px;'>Value 2</th>
	                                       <th style ='border: 1px solid #ddd;padding: 8px;'>Value 3</th>
	                                       <th style ='border: 1px solid #ddd;padding: 8px;'>LWL</th>
	                                       <th style ='border: 1px solid #ddd;padding: 8px;'>UWL</th>
                                        </tr>
                                     </thead>
                                     <tbody>";

                if (resultDB.ReturnInt == 0 && resultDB.ReturnDataSet.Tables[0].Rows.Count > 0)
                {
                    table = resultDB.ReturnDataSet.Tables[0];

                    try
                    {
                        HttpClient client = new HttpClient();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        MessageCard card = new MessageCard();
                        card.type = "MessageCard";
                        card.context = @"http://schema.org/extensions";
                        card.themeColor = "0076D7";
                        card.summary = "Thông Báo Lỗi Chỉ Số CTQ";

                        List<Section> lstSection = new List<Section>();
                        Section section;
                        List<string> lstID = new List<string>();
                        string text = "";
                        string mailBody = "";

                        section = new Section()
                        {
                            activityTitle = "Thông Báo Lỗi Chỉ Số CTQ",
                            activitySubtitle = "Hệ Thống OMNS Service",
                            activityImage = @"https://teamsnodesample.azurewebsites.net/static/img/image5.png",
                            markdown = true
                        };

                        section.facts = new List<Fact>();
                        section.startGroup = true;

                        foreach (DataRow row in table.Rows)
                        {
                            lstID.Add(row["Id"] + "");

                            text += "<tr>"
                                + " <td style ='border: 1px solid #ddd;padding: 8px;'>" + row["MATERIAL_ID"] + "</td>"
                                + "<td style ='border: 1px solid #ddd;padding: 8px;'>" + row["LOT_ID"] + "</td>"
                                + "<td style ='border: 1px solid #ddd;padding: 8px;'>" + row["OperationName"] + "</td>";

                            if (double.Parse(row["MAIN_VALUE1"] + "") < double.Parse(row["LWL"] + "") || double.Parse(row["MAIN_VALUE1"] + "") > double.Parse(row["UWL"] + ""))
                            {
                                text += "<td style ='border: 1px solid #ddd;padding: 8px;color:red'>" + row["MAIN_VALUE1"] + "</td>";
                            }
                            else
                            {
                                text += "<td style ='border: 1px solid #ddd;padding: 8px;'>" + row["MAIN_VALUE1"] + "</td>";
                            }

                            if (double.Parse(row["MAIN_VALUE2"] + "") < double.Parse(row["LWL"] + "") || double.Parse(row["MAIN_VALUE2"] + "") > double.Parse(row["UWL"] + ""))
                            {
                                text += "<td style ='border: 1px solid #ddd;padding: 8px;color:red'>" + row["MAIN_VALUE2"] + "</td>";
                            }
                            else
                            {
                                text += "<td style ='border: 1px solid #ddd;padding: 8px;'>" + row["MAIN_VALUE2"] + "</td>";
                            }


                            if (double.Parse(row["MAIN_VALUE3"] + "") < double.Parse(row["LWL"] + "") || double.Parse(row["MAIN_VALUE3"] + "") > double.Parse(row["UWL"] + ""))
                            {
                                text += "<td style ='border: 1px solid #ddd;padding: 8px;color:red'>" + row["MAIN_VALUE3"] + "</td>";
                            }
                            else
                            {
                                text += "<td style ='border: 1px solid #ddd;padding: 8px;'>" + row["MAIN_VALUE3"] + "</td>";
                            }

                            text += "<td style ='border: 1px solid #ddd;padding: 8px;'>" + row["LWL"] + "</td>"
                                  + "<td style ='border: 1px solid #ddd;padding: 8px;'>" + row["UWL"] + "</td> </tr> ";
                        }

                        section.text = tblHtmlTemp + text + "</tbody></table>";

                        mailBody = section.text;

                        lstSection.Add(section);
                        card.sections = lstSection;

                        var content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(card), System.Text.Encoding.UTF8, "application/json");

                        var response = await client.PostAsync("https://ddonline.webhook.office.com/webhookb2/55d5cda3-2776-4a08-9ef7-40e0386d4df9@6079baf5-465b-476c-b2ee-8325ae7f3272/IncomingWebhook/0e5325e099f842a2ab5d4994aa8f5455/a41df1e6-6192-480f-94cd-f1021a999840", content);

                        if (response.StatusCode == System.Net.HttpStatusCode.OK && lstID.Count > 0)
                        {
                            Dictionary<string, string> dic1 = new Dictionary<string, string>();
                            dic1.Add("A_DEPT", "WLP1");
                            ResultDB resultDB1 = sql.ExecProceduce2("PKG_BUSINESS@GET_Email_CTQ", dic1);
                            if (resultDB1.ReturnInt == 0 && resultDB1.ReturnDataSet.Tables[0].Rows.Count > 0)
                            {
                                // SEND MAIL
                                EmailSender emailSender = new EmailSender();
                                emailSender.FROM_NAME = "CTQ Notification";
                                emailSender.FROM_ADDRESS = "whcpi1@wisol.co.kr";
                                emailSender.Subject = "THÔNG BÁO LỖI CHỈ SỐ CTQ";

                                foreach (DataRow item in resultDB1.ReturnDataSet.Tables[0].Rows)
                                {
                                    emailSender.AddToEmailAddress(item["mail"] + "");
                                }

                                emailSender.Body = mailBody;
                                bool result = emailSender.Send();

                                if (result)
                                {
                                    LogFile.WriteErrorLog("ProcessDB: Send Mail success.");
                                }
                                else
                                {
                                    LogFile.WriteErrorLog("ProcessDB: Send Mail NG.");
                                }
                            }

                            // Update DB
                            Dictionary<string, string> dic2 = new Dictionary<string, string>();
                            dic2.Add("A_LST_ID", string.Join(",", lstID));
                            ResultDB resultDB2 = sql.ExecProceduce2("PKG_BUSINESS@UPDATE_CTQ_ERR", dic2);

                            if (resultDB2.ReturnInt == 0)
                            {
                                LogFile.WriteErrorLog("ProcessDB: Update success.");
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        LogFile.WriteErrorLog("ProcessDB: " + ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                LogFile.WriteErrorLog("ProcessDB_2: " + ex.Message);
            }
        }

        private async Task ProcessDB_wlp2()
        {
            try
            {
                SQLService sql = new SQLService("Persist Security Info=True;Data Source = 10.70.10.97;Initial Catalog = OPERATION_MNSDB;User Id = sa;Password = Wisol@123;Connect Timeout=3");

                Dictionary<string, string> dic = new Dictionary<string, string>();

                ResultDB resultDB = sql.ExecProceduce2("PKG_BUSINESS@GET_CTQ_ERR_WLP2", dic);
                System.Data.DataTable table = null;

                string tblHtmlTemp =
                                    @"<table bordercolor='black' border= '1'>
                                     <thead>
                                        <tr style = 'background-color : #004E89; color: White';text-align: center;>
                                           <th style ='border: 1px solid #ddd;padding: 8px;'>Material Id</th>
                                           <th style ='border: 1px solid #ddd;padding: 8px;'>Lot Id</th>
                                           <th style ='border: 1px solid #ddd;padding: 8px;'>Operation</th>
                                           <th style ='border: 1px solid #ddd;padding: 8px;'>Value 1</th>
	                                       <th style ='border: 1px solid #ddd;padding: 8px;'>Value 2</th>
	                                       <th style ='border: 1px solid #ddd;padding: 8px;'>Value 3</th>
                                           <th style ='border: 1px solid #ddd;padding: 8px;'>Value 4</th>
                                           <th style ='border: 1px solid #ddd;padding: 8px;'>Value 5</th>
	                                       <th style ='border: 1px solid #ddd;padding: 8px;'>LWL(Min)</th>
	                                       <th style ='border: 1px solid #ddd;padding: 8px;'>UWL(Max)</th>
                                        </tr>
                                     </thead>
                                     <tbody>";

                if (resultDB.ReturnInt == 0 && resultDB.ReturnDataSet.Tables[0].Rows.Count > 0)
                {
                    table = resultDB.ReturnDataSet.Tables[0];

                    try
                    {
                        HttpClient client = new HttpClient();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        MessageCard card = new MessageCard();
                        card.type = "MessageCard";
                        card.context = @"http://schema.org/extensions";
                        card.themeColor = "0076D7";
                        card.summary = "Thông Báo Lỗi Chỉ Số CTQ";

                        List<Section> lstSection = new List<Section>();
                        Section section;
                        List<string> lstID = new List<string>();
                        string text = "";
                        string mailBody = "";

                        section = new Section()
                        {
                            activityTitle = "Thông Báo Lỗi Chỉ Số CTQ",
                            activitySubtitle = "Hệ Thống OMNS Service",
                            activityImage = @"https://teamsnodesample.azurewebsites.net/static/img/image5.png",
                            markdown = true
                        };

                        section.facts = new List<Fact>();
                        section.startGroup = true;

                        foreach (DataRow row in table.Rows)
                        {
                            lstID.Add(row["Id"] + "");

                            text += "<tr>"
                                + " <td style ='border: 1px solid #ddd;padding: 8px;'>" + row["MATERIAL_ID"] + "</td>"
                                + "<td style ='border: 1px solid #ddd;padding: 8px;'>" + row["LOT_ID"] + "</td>"
                                + "<td style ='border: 1px solid #ddd;padding: 8px;'>" + row["OperationName"] + "</td>";

                            if (double.Parse(row["MAIN_VALUE1"] + "") < double.Parse(row["LWL"] + "") || double.Parse(row["MAIN_VALUE1"] + "") > double.Parse(row["UWL"] + ""))
                            {
                                text += "<td style ='border: 1px solid #ddd;padding: 8px;color:red'>" + row["MAIN_VALUE1"] + "</td>";
                            }
                            else
                            {
                                text += "<td style ='border: 1px solid #ddd;padding: 8px;'>" + row["MAIN_VALUE1"] + "</td>";
                            }

                            if (double.Parse(row["MAIN_VALUE2"] + "") < double.Parse(row["LWL"] + "") || double.Parse(row["MAIN_VALUE2"] + "") > double.Parse(row["UWL"] + ""))
                            {
                                text += "<td style ='border: 1px solid #ddd;padding: 8px;color:red'>" + row["MAIN_VALUE2"] + "</td>";
                            }
                            else
                            {
                                text += "<td style ='border: 1px solid #ddd;padding: 8px;'>" + row["MAIN_VALUE2"] + "</td>";
                            }


                            if (double.Parse(row["MAIN_VALUE3"] + "") < double.Parse(row["LWL"] + "") || double.Parse(row["MAIN_VALUE3"] + "") > double.Parse(row["UWL"] + ""))
                            {
                                text += "<td style ='border: 1px solid #ddd;padding: 8px;color:red'>" + row["MAIN_VALUE3"] + "</td>";
                            }
                            else
                            {
                                text += "<td style ='border: 1px solid #ddd;padding: 8px;'>" + row["MAIN_VALUE3"] + "</td>";
                            }

                            if (double.Parse(row["MAIN_VALUE4"] + "") < double.Parse(row["LWL"] + "") || double.Parse(row["MAIN_VALUE4"] + "") > double.Parse(row["UWL"] + ""))
                            {
                                text += "<td style ='border: 1px solid #ddd;padding: 8px;color:red'>" + row["MAIN_VALUE4"] + "</td>";
                            }
                            else
                            {
                                text += "<td style ='border: 1px solid #ddd;padding: 8px;'>" + row["MAIN_VALUE4"] + "</td>";
                            }

                            if (double.Parse(row["MAIN_VALUE5"] + "") < double.Parse(row["LWL"] + "") || double.Parse(row["MAIN_VALUE5"] + "") > double.Parse(row["UWL"] + ""))
                            {
                                text += "<td style ='border: 1px solid #ddd;padding: 8px;color:red'>" + row["MAIN_VALUE5"] + "</td>";
                            }
                            else
                            {
                                text += "<td style ='border: 1px solid #ddd;padding: 8px;'>" + row["MAIN_VALUE5"] + "</td>";
                            }

                            text += "<td style ='border: 1px solid #ddd;padding: 8px;'>" + row["LWL"] + "</td>"
                                  + "<td style ='border: 1px solid #ddd;padding: 8px;'>" + row["UWL"] + "</td> </tr> ";
                        }

                        section.text = tblHtmlTemp + text + "</tbody></table>";

                        mailBody = section.text;

                        lstSection.Add(section);
                        card.sections = lstSection;

                        var content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(card), System.Text.Encoding.UTF8, "application/json");

                        var response = await client.PostAsync("https://ddonline.webhook.office.com/webhookb2/7f4f7f45-b732-4842-b62a-6066ea122717@6079baf5-465b-476c-b2ee-8325ae7f3272/IncomingWebhook/4cad0d34e9e6478d94a11f6b2e3e2839/a41df1e6-6192-480f-94cd-f1021a999840", content);

                        if (response.StatusCode == System.Net.HttpStatusCode.OK && lstID.Count > 0)
                        {
                            Dictionary<string, string> dic1 = new Dictionary<string, string>();
                            dic1.Add("A_DEPT", "WLP2");
                            ResultDB resultDB1 = sql.ExecProceduce2("PKG_BUSINESS@GET_Email_CTQ_WLP2", dic1);
                            if (resultDB1.ReturnInt == 0 && resultDB1.ReturnDataSet.Tables[0].Rows.Count > 0)
                            {
                                // SEND MAIL
                                EmailSender emailSender = new EmailSender();
                                emailSender.FROM_NAME = "CTQ Notification";
                                emailSender.FROM_ADDRESS = "whcpi1@wisol.co.kr";
                                emailSender.Subject = "THÔNG BÁO LỖI CHỈ SỐ CTQ WLP2";

                                foreach (DataRow item in resultDB1.ReturnDataSet.Tables[0].Rows)
                                {
                                    emailSender.AddToEmailAddress(item["mail"] + "");
                                }

                                emailSender.Body = mailBody;
                                bool result = emailSender.Send();

                                if (result)
                                {
                                    LogFile.WriteErrorLog("ProcessDB_wlp2: Send Mail success.");
                                }
                                else
                                {
                                    LogFile.WriteErrorLog("ProcessDB_wlp2: Send Mail NG.");
                                }
                            }

                            // Update DB
                            Dictionary<string, string> dic2 = new Dictionary<string, string>();
                            dic2.Add("A_LST_ID", string.Join(",", lstID));
                            ResultDB resultDB2 = sql.ExecProceduce2("PKG_BUSINESS@UPDATE_CTQ_ERR_WLP2", dic2);

                            if (resultDB2.ReturnInt == 0)
                            {
                                LogFile.WriteErrorLog("ProcessDB_wlp2: Update success.");
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        LogFile.WriteErrorLog("ProcessDB_wlp2: " + ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                LogFile.WriteErrorLog("ProcessDB_wlp2_2: " + ex.Message);
            }
        }

        private async Task Process_EHS()
        {
            try
            {
                SQLService sql = new SQLService("Persist Security Info=True;Data Source = 10.70.10.97;Initial Catalog = HRMSDB2;User Id = sa;Password = Wisol@123;Connect Timeout=3");

                Dictionary<string, string> dic = new Dictionary<string, string>();

                ResultDB resultDB = sql.ExecProceduce2("PKG_BUSINESS@GET_FEATURES_KEHOACH", dic);
                System.Data.DataTable table = null;

                string tblHtmlTemp =
                                    @"<table bordercolor='black' border= '1'>
                                     <thead>
                                        <tr style = 'background-color : #004E89; color: White';text-align: center;>
                                           <th style ='border: 1px solid #ddd;padding: 8px;'>Đề mục-제목</th>
                                           <th style ='border: 1px solid #ddd;padding: 8px;'>Nội dung-내용</th>
                                           <th style ='border: 1px solid #ddd;padding: 8px;'>Thời gian bắt đầu-시작시간</th>
                                           <th style ='border: 1px solid #ddd;padding: 8px;'>Thời gian kết thúc-종료시간</th>
	                                       <th style ='border: 1px solid #ddd;padding: 8px;'>Người phụ trách-담당자</th>
	                                       <th style ='border: 1px solid #ddd;padding: 8px;'>Số ngày còn lại-남은 날수</th>
                                        </tr>
                                     </thead>
                                     <tbody>";

                if (resultDB.ReturnInt == 0 && resultDB.ReturnDataSet.Tables[0].Rows.Count > 0)
                {
                    table = resultDB.ReturnDataSet.Tables[0];

                    try
                    {
                        HttpClient client = new HttpClient();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        MessageCard card = new MessageCard();
                        card.type = "MessageCard";
                        card.context = @"http://schema.org/extensions";
                        card.themeColor = "0076D7";
                        card.summary = "Thông Báo Kế Hoạch EHS Sắp Tới";

                        List<Section> lstSection = new List<Section>();
                        Section section;
                        string text = "";
                        string mailBody = "";

                        section = new Section()
                        {
                            activityTitle = "Thông Báo Kế Hoạch EHS Sắp Tới",
                            activitySubtitle = "Hệ Thống OMNS Service",
                            activityImage = @"https://teamsnodesample.azurewebsites.net/static/img/image5.png",
                            markdown = true
                        };

                        section.facts = new List<Fact>();
                        section.startGroup = true;

                        foreach (DataRow row in table.Rows)
                        {
                            text += "<tr>"
                                + " <td style ='border: 1px solid #ddd;padding: 8px;'>" + row["Demuc"] + "</td>"
                                + "<td style ='border: 1px solid #ddd;padding: 8px;'>" + row["NoiDung"] + "</td>"
                                + "<td style ='border: 1px solid #ddd;padding: 8px;'>" + row["NgayBatDau"] + "</td>"
                                + "<td style ='border: 1px solid #ddd;padding: 8px;'>" + row["NgayKetThuc"] + "</td>"
                                + "<td style ='border: 1px solid #ddd;padding: 8px;'>" + row["NguoiPhuTrach"] + "</td>"
                                + "<td style ='border: 1px solid #ddd;padding: 8px;'>" + row["DIFF"] + "</td></tr>";
                        }

                        section.text = tblHtmlTemp + text + "</tbody></table>";

                        mailBody = section.text;

                        lstSection.Add(section);
                        card.sections = lstSection;

                        var content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(card), System.Text.Encoding.UTF8, "application/json");

                        var response = await client.PostAsync("https://ddonline.webhook.office.com/webhookb2/fc35dd16-a965-48c0-8778-030c4e97cab4@6079baf5-465b-476c-b2ee-8325ae7f3272/IncomingWebhook/407bc66e5ad247a9b7a3f3d2150dd89f/a41df1e6-6192-480f-94cd-f1021a999840", content);

                        if (response.StatusCode == System.Net.HttpStatusCode.OK && table.Rows.Count > 0)
                        {
                            Dictionary<string, string> dic1 = new Dictionary<string, string>();
                            ResultDB resultDB1 = sql.ExecProceduce2("PKG_BUSINESS@GET_EHS_EMAIL", dic1);
                            if (resultDB1.ReturnInt == 0 && resultDB1.ReturnDataSet.Tables[0].Rows.Count > 0)
                            {
                                // SEND MAIL
                                EmailSender emailSender = new EmailSender();
                                emailSender.FROM_NAME = "EHS Notification";
                                emailSender.FROM_ADDRESS = "whcpi1@wisol.co.kr";
                                emailSender.Subject = "Thông Báo Kế Hoạch EHS Sắp Tới";

                                foreach (DataRow item in resultDB1.ReturnDataSet.Tables[0].Rows)
                                {
                                    emailSender.AddToEmailAddress(item["Email"] + "");
                                }

                                emailSender.Body = mailBody;
                                bool result = emailSender.Send();

                                if (result)
                                {
                                    LogFile.WriteErrorLog("Send Mail for EHS success.");
                                }
                                else
                                {
                                    LogFile.WriteErrorLog("Send Mail for EHS NG.");
                                }
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        LogFile.WriteErrorLog("EHS:" + ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                LogFile.WriteErrorLog("EHS 2" + ex);
            }
        }

        private void CreateExcelSuatAn()
        {
            try
            {
                LogFile.WriteErrorLog("CreateExcelSuatAn Begin");
                string monthYear = DateTime.Now.ToString("yyyy-MM");
                string daysofmonth = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month).ToString();
                string fileExtension = DateTime.Now.ToString("yyyyMM") + ".xls";
                if (DateTime.Now.Day == 1 && DateTime.Now.Hour <= 8)
                {
                    monthYear = DateTime.Now.AddDays(-1).ToString("yyyy-MM");
                    daysofmonth = DateTime.DaysInMonth(DateTime.Now.AddDays(-1).Year, DateTime.Now.AddDays(-1).Month).ToString();
                    fileExtension = DateTime.Now.AddDays(-1).ToString("yyyyMM") + ".xls";
                }

                SQLService sql = new SQLService("Persist Security Info=True;Data Source = 10.70.22.240;Initial Catalog = QLSA;User Id = sa;Password = qwe123!@#;Connect Timeout=3");
                Dictionary<string, string> dic = new Dictionary<string, string>
                {
                    { "A_MONTHYEAR", monthYear },
                    { "A_DAYSOFMONTH", daysofmonth }
                };

                ResultDB resultDB = sql.ExecProceduce2("PKG_TKSA003@GET_LIST", dic);
                System.Data.DataTable tableSA = null;
                System.Data.DataTable tableMilk = null;

                if (resultDB.ReturnInt == 0 && resultDB.ReturnDataSet.Tables[0].Rows.Count > 0)
                {
                    tableSA = resultDB.ReturnDataSet.Tables[0];
                    tableMilk = resultDB.ReturnDataSet.Tables[1];
                    ExcelPackage excel = new ExcelPackage();

                    var workSheetSA = excel.Workbook.Worksheets.Add("SA");

                    workSheetSA.Cells[1, 1].Value = "DATE_INPUT";
                    workSheetSA.Cells[1, 2].Value = "SHIFTNAME";
                    workSheetSA.Cells[1, 3].Value = "START_TIME";
                    workSheetSA.Cells[1, 4].Value = "END_TIME";
                    workSheetSA.Cells[1, 5].Value = "STAFFNAME";
                    workSheetSA.Cells[1, 6].Value = "STAFFCODE";
                    workSheetSA.Cells[1, 7].Value = "STAFFDEPT";


                    int i = 2;
                    int endOfRow1 = 0;
                    foreach (DataRow row in tableSA.Rows)
                    {

                        endOfRow1 = i;
                        workSheetSA.Cells[i, 1].Value = row["DATE_INPUT"].ToString();
                        workSheetSA.Cells[i, 2].Value = row["SHIFTNAME"].ToString();
                        workSheetSA.Cells[i, 3].Value = row["START_TIME"].ToString();
                        workSheetSA.Cells[i, 4].Value = row["END_TIME"].ToString();
                        workSheetSA.Cells[i, 5].Value = row["STAFFNAME"].ToString();
                        workSheetSA.Cells[i, 6].Value = row["STAFFCODE"].ToString();
                        workSheetSA.Cells[i, 7].Value = row["STAFFDEPT"].ToString();

                        i += 1;
                    }
                    workSheetSA.Cells["A1:G" + endOfRow1].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    workSheetSA.Cells["A1:G" + endOfRow1].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    workSheetSA.Cells["A1:G" + endOfRow1].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    workSheetSA.Cells["A1:G" + endOfRow1].AutoFitColumns();


                    var workSheetMilk = excel.Workbook.Worksheets.Add("Milk"); ;

                    workSheetMilk.Cells[1, 1].Value = "DATE_INPUT";
                    workSheetMilk.Cells[1, 2].Value = "SHIFTNAME";
                    workSheetMilk.Cells[1, 3].Value = "START_TIME";
                    workSheetMilk.Cells[1, 4].Value = "END_TIME";
                    workSheetMilk.Cells[1, 5].Value = "STAFFNAME";
                    workSheetMilk.Cells[1, 6].Value = "STAFFCODE";
                    workSheetMilk.Cells[1, 7].Value = "STAFFDEPT";

                    int j = 2;
                    int endOfRow = 0;
                    foreach (DataRow row in tableMilk.Rows)
                    {

                        endOfRow = j;
                        workSheetMilk.Cells[j, 1].Value = row["DATE_INPUT"].ToString();
                        workSheetMilk.Cells[j, 2].Value = row["SHIFTNAME"].ToString();
                        workSheetMilk.Cells[j, 3].Value = row["START_TIME"].ToString();
                        workSheetMilk.Cells[j, 4].Value = row["END_TIME"].ToString();
                        workSheetMilk.Cells[j, 5].Value = row["STAFFNAME"].ToString();
                        workSheetMilk.Cells[j, 6].Value = row["STAFFCODE"].ToString();
                        workSheetMilk.Cells[j, 7].Value = row["STAFFDEPT"].ToString();

                        j += 1;
                    }

                    workSheetMilk.Cells["A1:G" + endOfRow1].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    workSheetMilk.Cells["A1:G" + endOfRow1].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    workSheetMilk.Cells["A1:G" + endOfRow1].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    workSheetMilk.Cells["A1:G" + endOfRow1].AutoFitColumns();

                    var location = System.Reflection.Assembly.GetEntryAssembly().Location;
                    var directoryPath = Path.GetDirectoryName(location);

                    string fileName = "SA_MIlk_" + fileExtension;

                    string p_strPath = Path.Combine(directoryPath, fileName);

                    if (File.Exists(p_strPath))
                        File.Delete(p_strPath);

                    // Create excel file on physical disk 
                    FileStream objFileStrm = File.Create(p_strPath);
                    objFileStrm.Close();

                    // Write content to excel file 
                    File.WriteAllBytes(p_strPath, excel.GetAsByteArray());
                    //Close Excel package
                    excel.Dispose();

                    string targetPath = ConfigurationManager.AppSettings["targetPath"];

                    if (!Directory.Exists(targetPath))
                    {
                        System.IO.Directory.CreateDirectory(targetPath);
                    }

                    string address = Path.Combine(targetPath, fileName);
                    File.Copy(p_strPath, address, true);

                    if (File.Exists(p_strPath))
                        File.Delete(p_strPath);

                    LogFile.WriteErrorLog("CreateExcelSuatAn End");
                }
            }
            catch (Exception ex)
            {
                LogFile.WriteErrorLog("CreateExcelSuatAn : " + ex.Message);
            }

        }
    }
}
