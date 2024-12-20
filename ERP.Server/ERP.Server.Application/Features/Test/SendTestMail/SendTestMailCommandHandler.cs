using ERP.Server.Application.Services;
using MediatR;
using TS.Result;

namespace ERP.Server.Application.Features.Mails.SendExampleMail;

internal sealed class SendTestMailCommandHandler(
    IMailService mailService) : IRequestHandler<SendTestMailCommand, Result<string>>
{
    public async Task<Result<string>> Handle(SendTestMailCommand request, CancellationToken cancellationToken)
    {
        string to = "tanersaydam@gmail.com";
        string subject = "Deneme Mail";
        string body = @"<h1>Hello world!</h1>
    <p>Bu bir deneme <b>mailidir</b> dikkate almayın!";
        var response = await mailService.SendEmailAsync(to, subject, body);

        if (!response.Successful)
        {
            return Result<string>.Failure(response.ErrorMessages.ToList());
        }

        return "Test mail send is successful";
    }
}
