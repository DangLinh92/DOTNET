using HRMNS.Application.Interfaces;
using HRMNS.Data.EF.Extensions;
using HRMNS.Data.Entities.luong;
using HRMNS.Utilities.Dtos;
using HRMS.Models.AccountViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HRMS.Areas.Luong.Controllers
{
    [Area("Luong")]
    public class LoginController : Controller
    {
        private IViewLuongService _viewLuongService;
        public LoginController(IViewLuongService viewLuongService)
        {
            _viewLuongService = viewLuongService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ChangePassword(string newpassword)
        {
            string userName = HttpContext.Session.GetString("Username");

            if (userName.NullString() != "")
            {
                USER_VIEW_LUONG user = _viewLuongService.GetUserByName(userName);
                bool rs = _viewLuongService.ChangePassword(userName, user.PasswordDefault, newpassword);

                if (rs)
                {
                    _viewLuongService.Save();
                    return new OkObjectResult(new GenericResult(true));
                }
                else
                {
                    return new OkObjectResult(new GenericResult(false, "Thay đổi mật khẩu thất bại"));
                }
            }
            else
            {
                return new OkObjectResult(new GenericResult(false, "Thay đổi mật khẩu thất bại"));
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Authen(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                bool result = _viewLuongService.Login(model.UserName, model.Password);

                if (result)
                {
                    HttpContext.Session.SetString("Username", model.UserName);
                    HttpContext.Session.SetString("IsAuthenticated", "true");

                    USER_VIEW_LUONG user = _viewLuongService.GetUserByName(model.UserName);

                    return new OkObjectResult(new GenericResult(true,user.FirtLogin.ToString()));
                }
                else
                {
                    return new OkObjectResult(new GenericResult(false, "Login failed"));
                }
            }

            // If we got this far, something failed, redisplay form
            return new ObjectResult(new GenericResult(false, model));
        }

        [HttpPost]
        public IActionResult Logout()
        {
            // Xoá thông tin người dùng khỏi session
            HttpContext.Session.Remove("Username");
            HttpContext.Session.Remove("IsAuthenticated");
            return new OkObjectResult(new GenericResult(true));
        }
    }
}
