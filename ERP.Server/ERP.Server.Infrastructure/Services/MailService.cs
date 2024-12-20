using ERP.Server.Application.Services;
using ERP.Server.Domain.Utilities;
using FluentEmail.Core;
using FluentEmail.Core.Models;
using Microsoft.Extensions.DependencyInjection;

namespace ERP.Server.Infrastructure.Services;
internal sealed class MailService : IMailService
{
    public async Task<SendResponse> SendEmailAsync(
        string to,
        string subject,
        string body, CancellationToken cancellationToken = default)
    {
        IFluentEmail fluentEmail = ServiceTool.ServiceProvider.GetRequiredService<IFluentEmail>();
        SendResponse sendResponse =
             await fluentEmail
             .To(to)
             .Subject(subject)
             .Body(body, true)
             .SendAsync(cancellationToken);

        return sendResponse;
    }
}
