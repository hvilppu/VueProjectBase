using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Project.Domain.ViewModels;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AuthenticationService = Project.Common.Services.AuthenticationService;
using IUserService = Project.Domain.Services.IUserService;

namespace Project.Controllers
{
    /// <summary>
    /// Authentication controller
    /// </summary>
    [Route("api/[controller]")]
    public class AuthenticationController : BaseController
    {
        private readonly IUserService _userService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userService"></param>
        /// <param name="authenticationService"></param>
        /// <param name="memoryCache"></param>
        public AuthenticationController(IUserService userService,
            AuthenticationService authenticationService, IMemoryCache memoryCache) : base(authenticationService,
            memoryCache)
        {
            _userService = userService;
        }


        /// <summary>
        /// Log out
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// GET /api/Authentication/HeartBeat
        /// </remarks>
        /// <response code="200"></response> 
        [SwaggerOperation]
        [HttpGet("[action]")]
        public void HeartBeat()
        {
            HttpContext.Response.Cookies.Append("HeartBeat", DateTime.Now.ToString());
        }

        /// <summary>
        /// Get log in information
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="workTimeEntry"></param>
        /// <returns>User information</returns>
        /// <remarks>
        /// GET /api/Authentication/LogIn/{username}{password}
        /// </remarks>
        /// <response code="200">Return logged user information</response>
        [SwaggerOperation]
        [HttpGet("[action]")]
        public async Task<UserViewModel> LogIn(string username, string password, bool workTimeEntry = false)
        {
            try
            {
                var user = _userService.GetUser(username, password);

                if (user != null)
                {
                    var userViewModel = new UserViewModel();
                    userViewModel.name = user.FirstName + " " + user.SecondName;

                    var roles = _userService.GetUserRoles(user.Id);
                    var permissions = _userService.GetUserPermissions(user.Id);                   

                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, username),
                        new Claim(AuthenticationService.CLAIM_USER_ID, user.Id.ToString()),
                        new Claim(AuthenticationService.CLAIM_USER_NAME, user.UserName.ToString())
                    };


                    foreach (var role in roles)
                        claims.Add(new Claim(ClaimTypes.Role, role.Code));

                    userViewModel.permissions = new List<string>();

                    foreach (var permission in permissions)
                    {
                        claims.Add(new Claim("Permissions", permission.Code));
                        userViewModel.permissions.Add(permission.Code);
                    }

                    var claimsIdentity = new ClaimsIdentity(
                        claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    var authProperties = new AuthenticationProperties
                    {
                        //AllowRefresh = <bool>,
                        // Refreshing the authentication session should be allowed.

                        //ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                        // The time at which the authentication ticket expires. A 
                        // value set here overrides the ExpireTimeSpan option of 
                        // CookieAuthenticationOptions set with AddCookie.

                        //IsPersistent = true,
                        // Whether the authentication session is persisted across 
                        // multiple requests. When used with cookies, controls
                        // whether the cookie's lifetime is absolute (matching the
                        // lifetime of the authentication ticket) or session-based.

                        //IssuedUtc = <DateTimeOffset>,
                        // The time at which the authentication ticket was issued.

                        //RedirectUri = <string>
                        // The full path or absolute URI to be used as an http 
                        // redirect response value.
                    };

                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        authProperties);

                    HttpContext.Response.Cookies.Append("login", "True");

                    userViewModel.needSetPassword = !user.PasswordSet;

                    return userViewModel;
                }

                else
                {
                    LogOut();
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Log out
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// GET /api/Authentication/LogOut
        /// </remarks>
        /// <response code="200"></response> 
        [SwaggerOperation]
        [HttpGet("[action]")]
        public async void LogOut(bool workTimeEntry = false)
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}