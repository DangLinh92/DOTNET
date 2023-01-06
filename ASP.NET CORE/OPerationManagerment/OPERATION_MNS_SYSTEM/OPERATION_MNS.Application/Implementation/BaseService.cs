using OPERATION_MNS.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Linq;
using OPERATION_MNS.Utilities.Constants;

namespace OPERATION_MNS.Application.Implementation
{
    public class BaseService
    {
        public IHttpContextAccessor _httpContextAccessor;

        public string GetUserId()
        {
            var userId = _httpContextAccessor.HttpContext.User.Identity.Name;
            return userId;
        }

        public string GetDepartment()
        {
            var dept = ((ClaimsIdentity)_httpContextAccessor.HttpContext.User.Identity).Claims.FirstOrDefault(x => x.Type == CommonConstants.UserClaims.Department);
            return dept.Value;
        }
    }
}
