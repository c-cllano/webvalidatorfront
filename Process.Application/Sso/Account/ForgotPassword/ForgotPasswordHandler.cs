using MediatR;
using Nuget.LogService.Services;
using Process.Domain.Entities;
using Process.Domain.Repositories;
using Process.Domain.Services;

namespace Process.Application.Sso.Account.ForgotPassword
{
    public class ForgotPasswordHandler(
        ISsoService ssoService,
        IPasswordRecoveryNotificationRepository passwordRecoveryNotification,
        ILogServices logService) : IRequestHandler<ForgotPasswordCommand, object>
    {
        private readonly ISsoService _ssoService = ssoService;
        private readonly IPasswordRecoveryNotificationRepository _passwordRecoveryNotification = passwordRecoveryNotification;
        private readonly ILogServices _logService = logService;
        public async Task<object> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            Domain.Entities.ForgotPassword forgotPassword = new()
            {
                Email = request.Email,
                Token = request.Token                
            };
            var userId = await _ssoService.ForgotPassword(forgotPassword);
            SavePasswordRecoveryNotificationInBackground(request.Email, userId);            
            return userId!;
        }

        private void SavePasswordRecoveryNotificationInBackground(string email, int userId = 0)
        {
             _ = Task.Run(async () =>
            {
                try
                {
                    var notification = new PasswordRecoveryNotification
                    {
                        UserId = userId,
                        Email = email.Trim().ToLower(),
                        SentAt = DateTime.UtcNow
                    };

                    await _passwordRecoveryNotification.SaveAsync(notification);
                }
                catch (Exception ex)
                {
                    await _logService.CreateErrorAsync(new Nuget.LogService.Models.CreateErrorIn
                    {
                        Component = "Process.API",
                        Date = DateTime.UtcNow.AddHours(-5),
                        SeverityID = 5,
                        Description = ex?.Message.ToString()!,
                        Machine = null,
                        UserID = null,
                        TransactionID = null,
                        Code = null,
                    });
                }
            });
        }

    }
}
