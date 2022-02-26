using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace project.Web.Swagger
{
    /// <summary>
    /// ResponseTyp Swagger Operation Filter
    /// </summary>
    public class ResponseTypeOperationFilter : IOperationFilter
    {
        /// <summary>
        /// Apply
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="context"></param>
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            operation.Responses.Add("401", new OpenApiResponse { Description = "Unauthorized" });
            operation.Responses.Add("404", new OpenApiResponse { Description = "Not found" });
            operation.Responses.Add("500", new OpenApiResponse { Description = "Unexpected server error. Please mention error GUID of the exception when contacting support." });
        }
    }
}
