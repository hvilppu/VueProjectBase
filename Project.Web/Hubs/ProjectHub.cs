using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using project.Web;
using Project.Api;
using Projects.Hubs;
using System;
using System.Threading.Tasks;

namespace project.Hubs
{
    /// <summary>
    /// Fish portal hub
    /// </summary>
    public class ProjectHub : Hub, IProjectHub
    {
        protected IHubContext<ProjectHub> _context;
        private readonly ProjectMemoryCache _projectMemoryCache;
        private readonly ProjectCommonOptions _projectCommonOptions;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="memoryCache"></param>
        /// <param name="projectCommonOptions"></param>
        public ProjectHub(IHubContext<ProjectHub> context, IMemoryCache memoryCache, IOptions<ProjectCommonOptions> projectCommonOptions)
        {
            this._context = context;
            this._projectMemoryCache = memoryCache as ProjectMemoryCache;
            this._projectCommonOptions = projectCommonOptions.Value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public override System.Threading.Tasks.Task OnDisconnectedAsync(Exception ex)
        {
            return base.OnDisconnectedAsync(ex);
        }

    }
}