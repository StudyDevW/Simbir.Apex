using MailKit.Net.Smtp;
using Middleware_Components.DTO;
using MimeKit;
using ServiceUI.Interfaces;
using System.Text;

namespace ServiceUI.Services
{
    public class MailService : IMailService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;
     //   private readonly DataContext _dataContext;

        public MailService(IConfiguration configuration)
        {
            _configuration = configuration;
         //   _dataContext = new DataContext(configuration["DATABASE_CONNECT"]);
            _logger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger("MailSender | mail-logger");
        }

        public async Task SendRegisterData(string email, RegisterMailDTO registerData)
        {
            _logger.LogInformation($"Отправляю сообщение на почту {email}...");

            if (email == null) { throw new Exception("email_null"); }

            var emailMessage = new MimeMessage();

            var subject = "Simbir.Apex - Данные для входа";

            var senderName = _configuration["SENDERNAME"] ?? throw new Exception("SENDERNAME не задан");
            var senderEmail = _configuration["SENDEREMAIL"] ?? throw new Exception("SENDEREMAIL не задан");

            emailMessage.From.Add(new MailboxAddress(senderName, senderEmail));
            emailMessage.To.Add(MailboxAddress.Parse(email));
            emailMessage.Subject = subject;

            StringBuilder message = new StringBuilder();
            message.AppendLine($"*****************  Ваши данные для входа  *****************");
            message.AppendLine($"Добро пожаловать в систему управления событиями информационной безопасности");
            message.AppendLine("----------------------------");
            message.AppendLine($"Ваш юзернейм: {registerData.username}");
            message.AppendLine($"Ваш пароль: {registerData.password}");
            message.AppendLine("----------------------------");

            var htmlMessage = message.ToString()
            .Replace(Environment.NewLine, "<br>")
            .Replace("\n", "<br>")
            .Replace("\r", "");

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = $"<html><head><meta charset='UTF-8'></head><body>{htmlMessage}</body></html>",
                TextBody = message.ToString()
            };

            emailMessage.Body = bodyBuilder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                try
                {
                    if (!int.TryParse(_configuration["MAILPORT"], out int mailPort))
                        throw new Exception("MAILPORT указан некорректно");

                    if (!bool.TryParse(_configuration["USESSL"], out bool useSsl))
                        useSsl = true;

                    await client.ConnectAsync(_configuration["MAILSERVER"], int.Parse(_configuration["MAILPORT"]), bool.Parse(_configuration["USESSL"]));
                    await client.AuthenticateAsync(senderEmail, _configuration["EMAILPASSWORD"]);
                    await client.SendAsync(emailMessage);
                    await client.DisconnectAsync(true);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Ошибка при отправке email");
                    throw;
                }
            }
        }
    }
}
