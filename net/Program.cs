using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using net.Handlers;using net.Storages;

await new MongoStorage().Init();
await new PostgresStorage().Init();

Host.CreateDefaultBuilder(args)
    .ConfigureServices(services => services
        .AddCors())
    .ConfigureWebHostDefaults(webBuilder => webBuilder
        .UseUrls("http://localhost:8080")
        .Configure(app => app
            .UseCors(builder => builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader())
            .Run(async context => {
                if (context.Request.Path == "/")
                {
                    context.Response.StatusCode = StatusCodes.Status200OK;
                    await context.Response.WriteAsync("Filtex!");
                }
                else if (context.Request.Path == "/metadata")
                {
                    await new MetadataHandler().HandleAsync(context);
                }
                else if (context.Request.Path == "/filter/memory" && context.Request.Method == "POST")
                {
                    await new MemoryFilterHandler().HandleAsync(context);
                }
                else if (context.Request.Path == "/filter/mongo" && context.Request.Method == "POST")
                {
                    await new MongoFilterHandler().HandleAsync(context);
                }
                else if (context.Request.Path == "/filter/postgres" && context.Request.Method == "POST")
                {
                    await new PostgresFilterHandler().HandleAsync(context);
                }
                else {
                    context.Response.StatusCode = StatusCodes.Status404NotFound;
                    await context.Response.WriteAsync("");
                }
            })))
    .Build().Run();