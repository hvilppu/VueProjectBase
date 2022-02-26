using Microsoft.Extensions.Options;
using project.CommonApiClient;
using System.Net;
using System.Net.Http;

namespace Project.Api
{
    public class ProjectCommonService
    {
        private ProjectCommonOptions _projectCommonOptions;
        private CookieContainer _cookieContainer;

        /// <summary>
        /// DI contructor
        /// </summary>
        /// <param name="projectCommonOptions"></param>
        public ProjectCommonService(IOptions<ProjectCommonOptions> projectCommonOptions)
        {
            _projectCommonOptions = projectCommonOptions.Value;
        }

        /// <summary>
        /// Non DI constructor
        /// </summary>
        /// <param name="baseUrl"></param>
        public ProjectCommonService(string baseUrl, CookieContainer cookieContainer)
        {
            _projectCommonOptions = new ProjectCommonOptions();
            _projectCommonOptions.BaseUrl = baseUrl;
            _cookieContainer = cookieContainer;
        }

        public UserViewModel LogIn(string userName, string passWord)
        {
            return GetSwaggerClientWithContainer().Api_LogInAsync(userName, passWord).GetAwaiter().GetResult();
        }

        private swaggerClient GetSwaggerClientWithContainer()
        {
            var handler = new HttpClientHandler() { CookieContainer = _cookieContainer };
            var client = new HttpClient(handler);
            return new swaggerClient(_projectCommonOptions.BaseUrl, client);
        }

        private swaggerClient GetSwaggerClient()
        {
            return new swaggerClient(_projectCommonOptions.BaseUrl, new HttpClient());
        }
    }
}
