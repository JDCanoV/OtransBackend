using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.OpenApi.Models;
using System.Reflection;

public class SwaggerFileUploadFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var apiDescription = context.ApiDescription;
        var formFileParameters = apiDescription.ParameterDescriptions
            .Where(p => p.Type == typeof(IFormFile))
            .ToList();

        if (formFileParameters.Any())
        {
            // Eliminar parámetros anteriores de tipo archivo
            operation.Parameters = operation.Parameters
                .Where(p => !formFileParameters.Any(f => f.Name == p.Name))
                .ToList();

            // Agregar la configuración de multipart/form-data
            operation.RequestBody = new OpenApiRequestBody
            {
                Content = new Dictionary<string, OpenApiMediaType>
                {
                    ["multipart/form-data"] = new OpenApiMediaType
                    {
                        Schema = new OpenApiSchema
                        {
                            Type = "object",
                            Properties = formFileParameters.ToDictionary(
                                file => file.Name,
                                file => new OpenApiSchema
                                {
                                    Type = "string",
                                    Format = "binary"
                                })
                        }
                    }
                }
            };
        }
    }
}