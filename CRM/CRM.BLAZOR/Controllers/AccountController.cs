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
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;

namespace CRM.BLAZOR.Controllers
{
    [ApiController]
    public class AccountController : Controller
    {
        readonly UserManager<User> _userManager;
        readonly ITempService _tempService;
        readonly ILogService _logService;
        SignInManager<User> _signInManager;
        IUserRegistrationService _userRegistrationService;
        IConfiguration _configuration;
        public AccountController(UserManager<User> userManager, ITempService tempService,
            SignInManager<User> signInManager, IUserRegistrationService userRegistrationService,
            ILogService logService, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tempService = tempService;
            _userRegistrationService = userRegistrationService;
            _logService = logService;
            _configuration = configuration;
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
             await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);
                if (result.Succeeded)
                {
                    var claims = new[]
        {
            new Claim(ClaimTypes.Name, model.Email)
        };
                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["KEY"]));
                    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var expiry = DateTime.Now.AddMinutes(Convert.ToInt32(_configuration["LIFETIME"]));
                    var token = new JwtSecurityToken(
            _configuration["ISSUER"],
            _configuration["AUDIENCE"],
            claims,
            expires: expiry,
            signingCredentials: creds
        );
                    
                    //_tempService.CurrentUser = await _userRegistrationService.GetCurrent(model.Email);
                    return Ok(new LoginResult { Successful = true, Token = new JwtSecurityTokenHandler().WriteToken(token) });
                }
                else
                {
                    ModelState.AddModelError("", "Неправильный логин и (или) пароль");
                }
            }
            return Ok(new LoginResult { Successful=false, Error="Авторизация не пройдена"});
        }
        [HttpPost("/Logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            //await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            _tempService.CurrentUser = null;
            return RedirectToAction("index", "home");
        }
    }
}