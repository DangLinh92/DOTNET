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
        private Timer timer3 = null;
        protected override void OnStart(string[] args)
        {
            timer1 = new Timer();
            this.timer1.Interval = 60000; //every 1 p
            this.timer1.Elapsed += new System.Timers.ElapsedEventHandler(this.timer1_Tick);
            timer1.Enabled = true;

           

            timer3 = new Timer();
            this.timer3.Interval = 90000; //every 90s
            this.timer3.Elapsed += new ElapsedEventHandler(this.timer3_Tick);
            timer3.Enabled = true;


            LogFile.WriteErrorLog("window service START");
        }
        private async void timer3_Tick(object sender, ElapsedEventArgs e)
        {
            await ProcessDB_wlp2();
        }

        private async void timer1_Tick(object sender, ElapsedEventArgs e)
        {
            await ProcessDB();
        }

        protected override void OnStop()
        {
            timer1.Enabled = false;
            timer3.Enabled = false;
            LogFile.WriteErrorLog("window service stopped");
        }

        private async Task ProcessDB()
        {
            try
            {
                SQLService sql = new SQLService("Persist Security Info=True;Data Source = 10.70.21.216;Initial Catalog = OPERATION_MNSDB;User Id = sa;Password = Wisol@123;Connect Timeout=3");

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

                        var response = await client.PostAsync("https://ddonline.webhook.office.com/webhookb2/1f4879e6-bcf5-4f25-a304-a29e365c4098@6079baf5-465b-476c-b2ee-8325ae7f3272/IncomingWebhook/ee592a69b6c640d58d24c7aaca927b42/a41df1e6-6192-480f-94cd-f1021a999840", content);

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
                SQLService sql = new SQLService("Persist Security Info=True;Data Source = 10.70.21.216;Initial Catalog = OPERATION_MNSDB;User Id = sa;Password = Wisol@123;Connect Timeout=3");

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
    }
}
