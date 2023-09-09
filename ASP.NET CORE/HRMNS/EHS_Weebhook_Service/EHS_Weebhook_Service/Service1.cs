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

namespace EHS_Weebhook_Service
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }

        private Timer timer2 = null;
        protected override void OnStart(string[] args)
        {
            count = 0;
            timer2 = new Timer();
            this.timer2.Interval = 3600000; //every 1 h
            this.timer2.Elapsed += new ElapsedEventHandler(this.timer2_Tick);
            timer2.Enabled = true;

            LogFile.WriteErrorLog("window service START");
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

        protected override void OnStop()
        {
            timer2.Enabled = false;
            count = 0;
            LogFile.WriteErrorLog("window service stopped");
        }

        private async Task Process_EHS()
        {
            try
            {
                SQLService sql = new SQLService("Persist Security Info=True;Data Source = 10.70.21.208;Initial Catalog = HRMSDB2;User Id = sa;Password = sa@21208;Connect Timeout=3");

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

    }
}
