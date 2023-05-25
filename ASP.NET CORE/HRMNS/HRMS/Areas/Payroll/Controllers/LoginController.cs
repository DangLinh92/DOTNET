using HRMNS.Data.Entities;
using HRMNS.Utilities.Dtos;
using HRMS.Models.AccountViewModels;
using HRMS.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Areas.Payroll.Controllers
{
    [Area("Payroll")]
    public class LoginController : Controller
    {
        private readonly UserManager<APP_USER> _userManager;
        private readonly SignInManager<APP_USER> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly IConfiguration _configuration;
        private readonly IMemoryCache _memoryCache;
        private readonly ILogger _logger;

        public LoginController(UserManager<APP_USER> userManager,
            SignInManager<APP_USER> signInManager,
            IEmailSender emailSender,
            IConfiguration configuration,
            ILogger<AccountController> logger,
            IMemoryCache memoryCache)
        {
            _emailSender = emailSender;
            _configuration = configuration;
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _memoryCache = memoryCache;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Authen(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, lockoutOnFailure: false);

                _logger.LogInformation("User confirm OTP.");

                if (result.RequiresTwoFactor)
                {
                    return RedirectToAction("LoginWithOTP");
                }
            }

            // If we got this far, something failed, redisplay form
            return new ObjectResult(new GenericResult(false, "Login failed"));
        }

        [AllowAnonymous]
        public async Task<IActionResult> LoginWithOTP()
        {
            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                throw new InvalidOperationException("Unable to load two-factor authentication user.");
            }

            var token = await _userManager.GenerateTwoFactorTokenAsync(user, "Email");
            var message = new Message(new string[] { user.Email! }, "OTP Confrimation", "Your security code is: " + token);
            _emailSender.SendEmail(message);

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ResendOTP()
        {
            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                throw new InvalidOperationException("Unable to load two-factor authentication user.");
            }

            var token = await _userManager.GenerateTwoFactorTokenAsync(user, "Email");
            var message = new Message(new string[] { user.Email! }, "OTP Confrimation", "Your security code is: " + token);
            _emailSender.SendEmail(message);

            return new OkObjectResult(new GenericResult(true));
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> OTP(string code)
        {
            if (ModelState.IsValid)
            {
                var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
                if (user == null)
                {
                    throw new InvalidOperationException($"Unable to load two-factor authentication user.");
                }

                var signIn = await _signInManager.TwoFactorSignInAsync("Email", code, false, false);
                if (signIn.Succeeded)
                {
                    return new OkObjectResult(new GenericResult(true));
                }
            }
            return new OkObjectResult(new GenericResult(false, "OTP invalid"));
        }

        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddDays(2),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }
    }
}
