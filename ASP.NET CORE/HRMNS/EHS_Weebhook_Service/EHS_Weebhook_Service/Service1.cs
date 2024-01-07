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
        private Timer timer4 = null;
        private bool isStart = false;
        protected override void OnStart(string[] args)
        {
            isStart = true;
            count = 0;
            timer2 = new Timer();
            this.timer2.Interval = 3600000; //every 1 h
            this.timer2.Elapsed += new ElapsedEventHandler(this.timer2_Tick);
            timer2.Enabled = true;

            timer4 = new Timer();
            this.timer4.Interval = 3600000; //every 1 h
            this.timer4.Elapsed += new ElapsedEventHandler(this.timer4_Tick);
            timer4.Enabled = true;
          
            LogFile.WriteErrorLog("window service START");
        }

        int count = 0;
        private async void timer2_Tick(object sender, ElapsedEventArgs e)
        {
            if (isStart)
            {
                LogFile.WriteErrorLog("EHS: isStart : " + isStart);
                await Process_EHS();
                isStart = false;
            }
            else
            {
                LogFile.WriteErrorLog("EHS: isStart : " + isStart);

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
        }

        private void timer4_Tick(object sender, ElapsedEventArgs e)
        {
            CreateExcelSuatAn();
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

        protected override void OnStop()
        {
            timer2.Enabled = false;
            timer4.Enabled = false;
            count = 0;
            isStart = false;
            LogFile.WriteErrorLog("window service stopped");
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

                LogFile.WriteErrorLog("EHS: start " + resultDB.ReturnInt);

                if (resultDB.ReturnInt == 0 && resultDB.ReturnDataSet.Tables[0].Rows.Count > 0)
                {
                    table = resultDB.ReturnDataSet.Tables[0];

                    LogFile.WriteErrorLog("EHS: row count " + table.Rows.Count);

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

                        var response = await client.PostAsync("https://ddonline.webhook.office.com/webhookb2/57cad998-b448-4c1c-8c20-238c416ebb43@6079baf5-465b-476c-b2ee-8325ae7f3272/IncomingWebhook/001e8f0077c14d7cbc1e033180fb21b7/a41df1e6-6192-480f-94cd-f1021a999840", content);

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
