namespace Application.Common.Interfaces.Services;

public interface IEmailService
{
    Task SendUserCreatedEmailAsync(string toEmail);
    Task SendUserConfirmedEmailAsync(string toEmail);
}