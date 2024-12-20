using ERP.Server.Application;
using ERP.Server.Domain.Utilities;
using ERP.Server.Infrastructure;
using ERP.Server.WebAPI.BackgroundServices;
using ERP.Server.WebAPI.Hubs;
using ERP.Server.WebAPI.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using Serilog;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors();

builder.Services.AddExceptionHandler<ExceptionHandler>().AddProblemDetails();

builder.Services.AddApplication();
builder.Services.AddInfrasturcture(builder.Configuration);

builder.Services.AddSignalR();

builder.Services.AddControllers();

builder.Services.AddSwaggerGen(setup =>
{
    OpenApiSecurityScheme jwtSecuritySheme = new OpenApiSecurityScheme
    {
        BearerFormat = "JWT",
        Name = "JWT Authentication",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        Description = "Put **_ONLY_** yourt JWT Bearer token on textbox below!",

        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };

    setup.AddSecurityDefinition(jwtSecuritySheme.Reference.Id, jwtSecuritySheme);

    setup.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    { jwtSecuritySheme, Array.Empty<string>() }
                });
});
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddHostedService<OutboxBackgroundService>();

builder.Services.AddServiceTool();

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console(restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information)
    //.WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri("http://localhost:9200"))
    //{
    //    AutoRegisterTemplate = true,
    //    IndexFormat = "mylogs",
    //    MinimumLogEventLevel = Serilog.Events.LogEventLevel.Information
    //})
    .CreateLogger();

builder.Host.UseSerilog();

WebApplication app = builder.Build();

app.UseSwagger();

app.UseSwaggerUI();

app.UseExceptionHandler();

app.UseCors(x => x.AllowAnyHeader().SetIsOriginAllowed(x => true).AllowAnyMethod().AllowCredentials());

app.UseAuthentication();

app.UseAuthorization();

app.AddStockMovementEndpoints();

app.MapControllers().RequireAuthorization();

app.MapHub<DbHub>("/db-hub");

app.Run();