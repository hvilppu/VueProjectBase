using Microsoft.AspNetCore.Http;
using System.IO;
using System.Linq;
using System.Security.Claims;

namespace Project.Common.Services
{
    public class AuthenticationService 
    {
        #region Constants

        public const string CLAIM_ROLE = nameof(CLAIM_ROLE);
        public const string CLAIM_USER_ID = nameof(CLAIM_USER_ID);
        public const string CLAIM_USER_NAME = nameof(CLAIM_USER_NAME);      
        public const string CLAIM_SESSIONLOG_ID = nameof(CLAIM_SESSIONLOG_ID);
        public const string PROJECT_API_AUTHORIZATION_HEADER = "X-APIAuthorization";

        #endregion Constants

        #region Fields

        private readonly HttpContext _httpContext;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="contextAccessor"></param>
        public AuthenticationService(IHttpContextAccessor contextAccessor)
        {
            _httpContext = contextAccessor.HttpContext;
        }

        #endregion Constructors

        #region Properties

        public int UserID
        {
            get { return IsAuthenticated() ?  int.Parse(GetUser().FindFirst(CLAIM_USER_ID).Value) : 0; }
        }

        public string UserName
        {
            get { return IsAuthenticated() ? GetUser().FindFirst(CLAIM_USER_NAME).Value : ""; }
        }

        public string ProtectedUID
        {
            get { return IsAuthenticated() ? GetUser().FindFirst(CLAIM_USER_ID).Value : null; }
        }

        public int SessionLogID
        {
            get { return IsAuthenticated() ? int.Parse(GetUser().FindFirst(CLAIM_SESSIONLOG_ID).Value) : 0; }
        }

        #endregion Properties

        #region Public methods

        /// <summary>
        /// Get user
        /// </summary>
        /// <returns></returns>
        public ClaimsPrincipal GetUser()
        {
            return _httpContext.User;
        }

        /// <summary>
        /// Is User Authenticated
        /// </summary>
        /// <returns></returns>
        public bool IsAuthenticated()
        {
            return _httpContext.User.Identity.IsAuthenticated && (_httpContext.User.FindAll(CLAIM_USER_ID).Count() > 0);
        }

        /// <summary>
        /// Get AuthenticationService
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public static AuthenticationService GetInstance(HttpContext httpContext)
        {
            return (AuthenticationService)httpContext.RequestServices.GetService(typeof(AuthenticationService));
        }

        /// <summary>
        /// Does user have atleast one of the parameter roles
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public bool HasRole(params string[] roles)
        {
            foreach (string role in roles)
            {
                if (_httpContext.User.HasClaim(CLAIM_ROLE, role))
                    return true;
            }

            return false;
        }

        #endregion Public methods
    }
}