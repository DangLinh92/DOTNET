using HRMNS.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace HRMNS.Application.Implementation
{
    public class BaseService
    {
        public IHttpContextAccessor _httpContextAccessor;

        public string GetUserId()
        {
            var userId = _httpContextAccessor.HttpContext.User.Identity.Name;
            return userId;
        }
    }
}
