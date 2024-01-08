using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using FiltexNet.Builders.Memory;
using FiltexNet.Builders.Memory.Types;
using FiltexNet.Expressions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using net.Storages;

namespace net.Handlers
{
    public class MemoryFilterHandler
    {
        public async Task HandleAsync(HttpContext context)
        {
            try
            {
                var projectFilter = Filters.Filters.ProjectFilter();
                var requestBody = await GetRequestBodyAsync(context.Request.Body);

                if (!requestBody.TryGetValue("query", out var queryValue))
                {
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    await context.Response.WriteAsync("Invalid request format");
                    return;
                }

                IExpression expression = null;

                if (context.Request.Query.TryGetValue("type", out StringValues typeQuery))
                {
                    switch (typeQuery[0])
                    {
                        case "text":
                            if (queryValue == null || queryValue.ToString() == "")
                            {
                                break;
                            }
                            
                            projectFilter.ValidateFromText(queryValue.ToString());
                            expression = projectFilter.ExpressionFromText(queryValue.ToString());
                            break;
                        case "json":
                            if (queryValue == null)
                            {
                                break;
                            }
                            
                            projectFilter.ValidateFromJson(JsonSerializer.Serialize(queryValue));
                            expression = projectFilter.ExpressionFromJson(JsonSerializer.Serialize(queryValue));
                            break;
                        default:
                            throw new Exception("invalid type");
                    }
                }
                else
                {
                    throw new Exception("invalid type");
                }

                MemoryExpression memoryFilter;

                if (expression != null)
                {
                    memoryFilter = new MemoryFilterBuilder().Build(expression);
                }
                else
                {
                    memoryFilter = new MemoryExpression
                    {
                        Fn = data => true
                    };
                }

                var results = new MemoryStorage().Query(memoryFilter);

                var jsonResponse = JsonSerializer.Serialize(results);

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

        private async Task<Dictionary<string, object>> GetRequestBodyAsync(Stream body)
        {
            using var reader = new StreamReader(body);
            var requestBody = await reader.ReadToEndAsync();
            return JsonSerializer.Deserialize<Dictionary<string, object>>(requestBody);
        }
    }
}