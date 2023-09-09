using CarMNS.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace CarMNS.Application.Implementation
{
    public class BaseService
    {
        public IHttpContextAccessor _httpContextAccessor;

        public string GetUserId()
        {
            var userId = _httpContextAccessor.HttpContext.User.Identity.Name;
            return userId;
        }

        public string GetUserName()
        {
            var userId = _httpContextAccessor.HttpContext.User.Claims.Where(x=>x.Type == "FullName").First().Value;
            return userId;
        }
    }
}
