using OPERATION_MNS.Data.EF.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPERATION_MNS.Areas.OpeationMns.Controllers
{
    [Authorize]
    [Area("OpeationMns")]
    public class AdminBaseController : Controller
    {
        public ILogger _logger;
        public string Department { get => User.Claims.FirstOrDefault(x => x.Type == "Deparment").Value.NullString(); }
        public string UserRole 
        { 
            get => User.Claims.FirstOrDefault(x => x.Type == "Roles").Value.NullString().Split(';')[0]; 
        }

        public string UserName
        {
            get => User.Claims.FirstOrDefault(x => x.Type == "UserName").Value.NullString().Split(';')[0];
        }
    }
}
