using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace SwiftLink.Presentation.Filters;

/// <summary>
/// Adds a custom parameter named “Token” to the header of every operation,
/// which requires the user to enter their subscription key.
/// </summary>
public class SwaggerAuthenticationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        operation.Parameters ??= new List<OpenApiParameter>();

        operation.Parameters.Add(new OpenApiParameter
        {
            Name = "Token",
            In = ParameterLocation.Header,
            Description = "Enter your subscription key"
        });
    }
}
