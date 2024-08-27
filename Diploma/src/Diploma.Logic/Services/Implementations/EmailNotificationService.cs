using System.Net;
using System.Net.Mail;
using Diploma.Domain.Repositories.Users;
using Diploma.Logic.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace Diploma.Logic.Services.Implementations;

public sealed class EmailNotificationService : INotifyService
{
    private readonly IUsersRepository _usersRepository;
    private readonly ILogger<EmailNotificationService> _logger;
    
    private const string Topic = "Уведомление о состоянии";

    public EmailNotificationService(IUsersRepository usersRepository, ILogger<EmailNotificationService> logger)
    {
        _usersRepository = usersRepository;
        _logger = logger;
    }

    public async Task NotifyAsync(IReadOnlyCollection<string> messages, CancellationToken cancellationToken)
    {
        if (!messages.Any()) return;
        _logger.LogInformation("Начинаю отправлять уведомления");
        var usersInfo = await _usersRepository.GetAsync(cancellationToken);

        var messageBody = string.Join(Environment.NewLine, messages);
        var smtpClient = new SmtpClient("smtp.mail.ru", 587);
        var from = new MailAddress("evstratov_as@internet.ru", "Notificator");

        foreach (var email in usersInfo.Select(x => x.Email))
        {
            var to = new MailAddress(email);
            var msg = new MailMessage(from, to);

            msg.Subject = Topic;
            msg.Body = messageBody;
            
            smtpClient.Credentials = new NetworkCredential("evstratov_as@internet.ru", "Z5V98ddxaqCxyVCV7SUt");
            smtpClient.EnableSsl = true;


            try
            {
                await smtpClient.SendMailAsync(msg, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Ошибка отправки информации на почту {email}");
            }
        }
    }
}