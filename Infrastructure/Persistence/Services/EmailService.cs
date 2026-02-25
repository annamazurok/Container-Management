using Application.Common.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Persistence.Services;

public class EmailService : IEmailService
{
    private readonly ILogger<EmailService> _logger;
    
    public EmailService(ILogger<EmailService> logger)
    {
        _logger = logger;
    }

    public async Task SendUserCreatedEmailAsync(string toEmail)
    {
        var message = $@"
        Вітаємо!

        Для вас створено обліковий запис в системі Containers Management.

        Щоб активувати акаунт:
        1. Перейдіть на https://yourfrontend.com/login
        2. Натисніть 'Sign in with Google'
        3. Використайте цей email: {toEmail}

        Ваш акаунт очікує підтвердження адміністратора.
        Після підтвердження ви зможете користуватись системою.

        З повагою,
        Команда Containers Management
        ";
        
        _logger.LogInformation("Email would be sent to {Email}: {Message}", toEmail, message);
        
        await Task.CompletedTask;
    }

    public async Task SendUserConfirmedEmailAsync(string toEmail)
    {
        var message = $@"
        Вітаємо!

        Ваш обліковий запис підтверджено адміністратором.

        Тепер ви можете увійти в систему:
        https://yourfrontend.com/login

        З повагою,
        Команда Containers Management
        ";
                
        _logger.LogInformation("Confirmation email would be sent to {Email}", toEmail);
        
        await Task.CompletedTask;
    }
}