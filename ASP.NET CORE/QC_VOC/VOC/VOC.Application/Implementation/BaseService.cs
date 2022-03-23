using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace VOC.Application.Implementation
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
