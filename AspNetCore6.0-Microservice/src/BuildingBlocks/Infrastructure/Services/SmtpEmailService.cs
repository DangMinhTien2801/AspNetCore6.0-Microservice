using Contracts.Services;
using Shared.Services.Email;
using Serilog;
using Contracts.Configurations;
using MailKit.Net.Smtp;
using MimeKit;
using Infrastructure.Configurations;
using System.Threading;

namespace Infrastructure.Services
{
    public class SmtpEmailService : ISmtpEmailService
    {
        private readonly ILogger _logger;
        private readonly SmtpEmailSetting _settings;
        private readonly SmtpClient _smtpClient;

        public SmtpEmailService(ILogger logger,
            SmtpEmailSetting settings)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _smtpClient = new SmtpClient();
        }
        public async Task SendEmailAsync(MailRequest request, 
            CancellationToken cancellationToken = default)
        {
            var emailMessage = GetMimeMessage(request);

            try
            {
                await _smtpClient.ConnectAsync(_settings.SMTPServer, _settings.Port,
                    _settings.UseSsl, cancellationToken);
                await _smtpClient.AuthenticateAsync(_settings.UserName, _settings.Password, cancellationToken);
                await _smtpClient.SendAsync(emailMessage, cancellationToken);
                await _smtpClient.DisconnectAsync(true, cancellationToken);
            }
            catch(Exception ex)
            {
                _logger.Error(ex.Message, ex);
            }
            finally
            {
                await _smtpClient.DisconnectAsync(true, cancellationToken);
                _smtpClient.Dispose();
            }
        }

        public void SendEmail(MailRequest request)
        {
            var emailMessage = GetMimeMessage(request);

            try
            {
                _smtpClient.Connect(_settings.SMTPServer, _settings.Port, _settings.UseSsl);
                _smtpClient.Authenticate(_settings.UserName, _settings.Password);
                _smtpClient.Send(emailMessage);
                _smtpClient.Disconnect(true);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
            }
            finally
            {
                _smtpClient.Disconnect(true);
                _smtpClient.Dispose();
            }
        }

        private MimeMessage GetMimeMessage(MailRequest request)
        {
            var emailMessage = new MimeMessage
            {
                Sender = new MailboxAddress(_settings.DisplayName, request.From ?? _settings.From),
                Subject = request.Subject,
                Body = new BodyBuilder
                {
                    HtmlBody = request.Body
                }.ToMessageBody()
            };
            if (request.ToAddresses.Any())
            {
                foreach (var toAddress in request.ToAddresses)
                {
                    emailMessage.To.Add(MailboxAddress.Parse(toAddress));
                }
            }
            else
            {
                var toAddress = request.ToAddress;
                emailMessage.To.Add(MailboxAddress.Parse(toAddress));
            }

            return emailMessage;
        }
    }
}
