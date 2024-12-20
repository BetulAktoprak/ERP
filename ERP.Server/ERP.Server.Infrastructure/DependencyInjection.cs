using ERP.Server.Domain.Repositories;
using ERP.Server.Infrastructure.Context;
using ERP.Server.Infrastructure.Options;
using ERP.Server.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Scrutor;

namespace ERP.Server.Infrastructure;
public static class DependencyInjection
{
    public static IServiceCollection AddInfrasturcture(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<EmailOptions>(configuration.GetSection("Email"));

        IOptions<EmailOptions> emailOptions = services.BuildServiceProvider().GetRequiredService<IOptions<EmailOptions>>();

        if (string.IsNullOrEmpty(emailOptions.Value.UserName))
        {
            services
                .AddFluentEmail("info@erp.com")
                .AddSmtpSender(emailOptions.Value.Host, emailOptions.Value.Port);
        }
        else
        {
            services
                .AddFluentEmail("info@erp.com")
                .AddSmtpSender(emailOptions.Value.Host, emailOptions.Value.Port, emailOptions.Value.UserName, emailOptions.Value.Password);
        }

        services.AddHttpContextAccessor();


        services.Configure<JwtOptions>(configuration.GetSection("Jwt"));
        services.ConfigureOptions<JwtSetupOptions>();

        services.AddAuthentication().AddJwtBearer();
        services.AddAuthorization();

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("SqlServer"));
        });

        services.AddScoped<IUnitOfWork>(srv => srv.GetRequiredService<ApplicationDbContext>());

        services.Scan(action => action
        .FromAssemblies(typeof(DependencyInjection).Assembly)
        .AddClasses(publicOnly: false)
        .UsingRegistrationStrategy(registrationStrategy: RegistrationStrategy.Skip)
        .AsImplementedInterfaces()
        .WithScopedLifetime()
        );

        return services;
    }
}
