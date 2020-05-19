using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CRM.BLAZOR.Models;
using CRM.BLL.DTO;
using CRM.BLL.Interfaces;
using CRM.BLL.Services;
using CRM.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace CRM.BLAZOR.Controllers
{
    public class AccountController : Controller
    {
        readonly UserManager<User> _userManager;
        readonly ITempService _tempService;
        readonly ILogService _logService;
        SignInManager<User> _signInManager;
        IUserRegistrationService _userRegistrationService;
        public AccountController(UserManager<User> userManager, ITempService tempService,
            SignInManager<User> signInManager, IUserRegistrationService userRegistrationService,
            ILogService logService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tempService = tempService;
            _userRegistrationService = userRegistrationService;
            _logService = logService;
        }
        [HttpGet("/Login")]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost("/Login")]
        public async Task<IActionResult> Login([FromBody]LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var result =
             await _signInManager.PasswordSignInAsync(model.Email, model.Password, true, false);
                if (result.Succeeded)
                {
                    var claims = new List<Claim>
                    {
                      new Claim(ClaimTypes.Name, model.Email)
                    };

                    var claimsIdentity = new ClaimsIdentity(
                      claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var authProperties = new AuthenticationProperties();

                    await HttpContext.SignInAsync(
                      CookieAuthenticationDefaults.AuthenticationScheme,
                      new ClaimsPrincipal(claimsIdentity),
                      authProperties);
                    User user = await _userManager.FindByEmailAsync(model.Email);
                    LogDTO logDTO = new LogDTO
                    {
                        Action = "Успешно авторизовался",
                        UserId = user.Id
                    };
                    await _logService.AddLog(logDTO);
                    //_tempService.CurrentUser = await _userRegistrationService.GetCurrent(model.Email);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Неправильный логин и (или) пароль");
                }
            }
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            //await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            _tempService.CurrentUser = null;
            return RedirectToAction("index", "home");
        }
    }
}