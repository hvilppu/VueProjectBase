using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.IO;

namespace project.Web.Swagger
{
    /// <summary>
    /// Swagger generator helper
    /// </summary>
    public static class SwaggerGenHelper
    {
        /// <summary>
        /// Set Swagger generation options
        /// </summary>
        /// <returns></returns>
        public static Action<SwaggerGenOptions> SetSwaggerGenOptions()
        {
            return c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "project API",
                    Version = "v1"
                });

                c.SwaggerDoc("v1-checkpoint", new OpenApiInfo
                {
                    Title = "project API Checkpoint",
                    Version = "v1-checkpoint"
                });

                c.OperationFilter<OperationIdOperationFilter>();
                c.OperationFilter<ResponseTypeOperationFilter>();

                // Set the comments path for the Swagger JSON and UI.
                c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "project.Api.xml"));
                c.UseInlineDefinitionsForEnums();
                c.EnableAnnotations();
                // TODO c.SchemaFilter<ReverseAllOfPropertiesFilter>();
            };
        }
    }

    /// <summary>
    /// ApiExplorer Convention for Swagger
    /// </summary>
    public class ApiExplorerGroupPerVersionConvention : IControllerModelConvention
    {
        /// <summary>
        /// Apply (Autorun)
        /// </summary>
        /// <param name="controller"></param>
        public void Apply(ControllerModel controller)
        {
            var controllerNamespace = controller.ControllerType.Namespace; // e.g. "Controllers.V1"
            var apiVersion = "v1"; //controllerNamespace.Split('.').Last().ToLower();

            if (controllerNamespace.Equals("project.Controllers"))
                apiVersion = "v1";

            else if (controllerNamespace.Equals("project.Controllers.Checkpoint"))
                apiVersion = "v1-checkpoint";

            controller.ApiExplorer.GroupName = apiVersion;
        }
    }
}
