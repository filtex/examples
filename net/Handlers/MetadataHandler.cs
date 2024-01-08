using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace net.Handlers
{
    public class MetadataHandler
    {
        public async Task HandleAsync(HttpContext context)
        {
            try
            {
                var projectFilter = Filters.Filters.ProjectFilter();
                var response = projectFilter.Metadata();

                var jsonResponse = JsonSerializer.Serialize(response);

                context.Response.StatusCode = (int)HttpStatusCode.OK;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(jsonResponse);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                await context.Response.WriteAsync("Internal Server Error");
            }
        }
    }
}