using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;

namespace project.Web.Swagger
{
    /// <summary>
    /// OperationId Swagger Operation Filter
    /// </summary>
    public class OperationIdOperationFilter : IOperationFilter
    {
        /// <summary>
        /// Apply
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="context"></param>
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var registerName = context.MethodInfo.DeclaringType.FullName.Split('.').Where(s => s.Contains("Register")).FirstOrDefault();
            var methodName = context.MethodInfo.Name;

            operation.OperationId = $"{ registerName }Api_{ methodName}";
        }
    }
}
