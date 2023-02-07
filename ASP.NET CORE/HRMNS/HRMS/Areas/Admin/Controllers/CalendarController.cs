using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HRMNS.Application.Interfaces;
using HRMNS.Application.ViewModels.EHS;
using HRMNS.Application.ViewModels.System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.Areas.Admin.Controllers
{
    public class CalendarController : AdminBaseController
    {
        private IEventScheduleParentService _eventScheduleParentService;
        private IDanhMucKeHoachService _danhMucKeHoachService;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public CalendarController(IEventScheduleParentService eventScheduleParentService, IDanhMucKeHoachService danhMucKeHoachService, IWebHostEnvironment hostingEnvironment)
        {
            _eventScheduleParentService = eventScheduleParentService;
            _danhMucKeHoachService = danhMucKeHoachService;
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        public JsonResult LoadData([FromBody] Params param)  // Here we get the Start and End Date and based on that can filter the data and return to Scheduler
        {
            DateTime start = param.StartDate;
            DateTime end = param.EndDate;
            var data = _eventScheduleParentService.GetAllEvent().Where(app => (app.StartTime >= start && app.StartTime <= end) || (app.RecurrenceRule != null && app.RecurrenceRule != "")).ToList();
            return Json(data);
        }

        [HttpPost]
        public JsonResult UpdateData([FromBody] EditParams param)
        {
            if (param.action == "insert" || (param.action == "batch" && param.added != null && param.added.Count > 0)) // this block of code will execute while inserting the appointments
            {
                var value = (param.action == "insert") ? param.value : param.added[0];
                DateTime startTime = Convert.ToDateTime(value.StartTime);
                DateTime endTime = Convert.ToDateTime(value.EndTime);
                EventScheduleParentViewModel appointment = new EventScheduleParentViewModel()
                {
                    Id = Guid.NewGuid(),
                    StartEvent = startTime.ToString("yyyy-MM-dd"),
                    EndEvent = endTime.ToString("yyyy-MM-dd"),
                    StartTime = startTime,
                    EndTime = endTime,
                    Subject = value.Subject,
                    IsAllDay = value.IsAllDay,
                    StartTimezone = value.StartTimezone,
                    EndTimezone = value.EndTimezone,
                    RecurrenceRule = value.RecurrenceRule,
                    RecurrenceID = value.RecurrenceID,
                    RecurrenceException = value.RecurrenceException,
                    UserCreated = UserName,
                    DateCreated = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    Location = value.Location,
                    Description = value.Description
                };
                _eventScheduleParentService.AddEventParent(appointment);
                _eventScheduleParentService.Save();
            }

            if (param.action == "update" || (param.action == "batch" && param.changed != null && param.changed.Count > 0)) // this block of code will execute while updating the appointment
            {
                var value = (param.action == "update") ? param.value : param.changed[0];
                var appointment = _eventScheduleParentService.GetEventById(value.Id);

                if (appointment != null)
                {
                    DateTime startTime = Convert.ToDateTime(value.StartTime);
                    DateTime endTime = Convert.ToDateTime(value.EndTime);
                    appointment.StartEvent = startTime.ToString("yyyy-MM-dd");
                    appointment.EndEvent = endTime.ToString("yyyy-MM-dd");
                    appointment.StartTime = startTime;
                    appointment.EndTime = endTime;
                    appointment.StartTimezone = value.StartTimezone;
                    appointment.EndTimezone = value.EndTimezone;
                    appointment.Subject = value.Subject;
                    appointment.IsAllDay = value.IsAllDay;
                    appointment.RecurrenceRule = value.RecurrenceRule;
                    appointment.RecurrenceID = value.RecurrenceID;
                    appointment.RecurrenceException = value.RecurrenceException;
                    appointment.UserModified = UserName;
                    appointment.DateModified = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    appointment.Location = value.Location;
                    appointment.Description = value.Description;

                    _eventScheduleParentService.EditEvent(appointment);
                    _eventScheduleParentService.Save();

                }
            }

            if (param.action == "remove" || (param.action == "batch" && param.deleted != null && param.deleted.Count > 0)) // this block of code will execute while removing the appointment
            {
                if (param.action == "remove")
                {
                    EventScheduleParentViewModel appointment = _eventScheduleParentService.GetEventById(Guid.Parse(param.key));
                    if (appointment != null) _eventScheduleParentService.DeleteEvent(Guid.Parse(param.key));
                }
                else
                {
                    foreach (var apps in param.deleted)
                    {
                        _eventScheduleParentService.DeleteEvent(apps.Id);
                    }
                }
                _eventScheduleParentService.Save();
            }

            var data = _eventScheduleParentService.GetAllEvent();
            return Json(data);
        }

        public class Params
        {
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
        }

        public class EditParams
        {
            public string key { get; set; }
            public string action { get; set; }
            public List<EventScheduleParentViewModel> added { get; set; }
            public List<EventScheduleParentViewModel> changed { get; set; }
            public List<EventScheduleParentViewModel> deleted { get; set; }
            public EventScheduleParentViewModel value { get; set; }
        }
    }

    #region model in syncfusion
    public class ScheduleEventData
    {
        public Guid Id { get; set; }
        public string Subject { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool IsAllDay { get; set; }
        public string RecurrenceRule { get; set; }
        public string StartTimezone { get; set; }
        public string EndTimezone { get; set; }
        public string RecurrenceID { get; set; }
        public string RecurrenceException { get; set; }
        public string ConferenceId { get; set; }

    }

    //    CREATE TABLE[dbo].[ScheduleEventData]
    //    (
    //          [Id] UNIQUEIDENTIFIER NOT NULL,
    //          [Subject] NVARCHAR(MAX)   NULL,
    //          [StartTime]
    //          DATETIME NULL,
    //          [EndTime]             DATETIME NULL,
    //          [StartTimezone]       NVARCHAR(MAX)   NULL,
    //          [EndTimezone] NVARCHAR(MAX)   NULL,
    //          [IsAllDay]
    //          BIT NULL,
    //          [RecurrenceRule]      NVARCHAR(MAX)   NULL,
    //          [RecurrenceID]
    //          INT NULL,
    //          [RecurrenceException] NVARCHAR(MAX)   NULL,
    //          [ConferenceId]
    //          INT NULL,
    //      PRIMARY KEY CLUSTERED([Id] ASC)
    //      );

    #endregion
}
