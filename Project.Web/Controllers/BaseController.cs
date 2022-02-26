using Project.Common.Services;
using project.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Linq;

namespace Project.Controllers
{
    /// <summary>
    /// Base for all controllers
    /// </summary>
    public class BaseController : Controller
    {
        public const string NO_CACHE = "NoCache";
        protected readonly Project.Common.Services.AuthenticationService _authenticationService;
        protected ProjectMemoryCache _projectMemoryCache;

        public BaseController()
        {
        }

        public BaseController(Project.Common.Services.AuthenticationService authenticationService,
            IMemoryCache memoryCache)
        {
            this._authenticationService = authenticationService;
            this._projectMemoryCache = memoryCache as ProjectMemoryCache;
        }

        protected int currentUserId
        {
            get { return _authenticationService.UserID; }
        }

        protected string currentUserName
        {
            get { return _authenticationService.UserName; }
        }
    }
}