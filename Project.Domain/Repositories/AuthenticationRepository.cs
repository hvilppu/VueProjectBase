using Microsoft.AspNetCore.Http;
using Project.Domain.Models;

namespace Project.Domain.Repositories
{
    /// <summary>
    /// https://docs.microsoft.com/en-us/ef/core/querying/related-data
    /// Entity Framework Core will automatically fix-up navigation properties to any other entities that were previously loaded into the context instance.
    /// So even if you don't explicitly include the data for a navigation property, the property may 
    /// still be populated if some or all of the related entities were previously loaded.
    /// </summary>
    public class AuthenticationRepository : BaseRepository, IAuthenticationRepository
    {
        public AuthenticationRepository(ProjectContext projectContext, IHttpContextAccessor httpContextAccessor) : base(projectContext, httpContextAccessor)
        {
        }
    }
}
