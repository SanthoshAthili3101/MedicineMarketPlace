using MailKit.Net.Smtp;
using MedicineMarketPlace.BuildingBlocks.Email.Models;
using MedicineMarketPlace.BuildingBlocks.Extensions;
using MimeKit;

namespace MedicineMarketPlace.BuildingBlocks.Email.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailConfiguration _emailDataConfiguration;

        public EmailService(EmailConfiguration emailDataConfiguration)
        {
            _emailDataConfiguration = emailDataConfiguration;
        }

        public async Task<bool> SendEmail(EmailData emailData)
        {
            try
            {
                MimeMessage emailMessage = new MimeMessage();

                MailboxAddress emailFrom = new MailboxAddress(_emailDataConfiguration.Name, _emailDataConfiguration.From);
                emailMessage.From.Add(emailFrom);

                MailboxAddress emailTo = new MailboxAddress(emailData.ToName, emailData.ToEmailId.ToString());
                emailMessage.To.Add(emailTo);

                if (emailData.CcEmailId.IsNotBlank())
                {
                    MailboxAddress emailCc = new MailboxAddress(emailData.CcName, emailData.CcEmailId);
                    emailMessage.Cc.Add(emailCc);
                }

                emailMessage.Subject = emailData.EmailSubject;

                BodyBuilder emailBodyBuilder = new BodyBuilder();
                emailBodyBuilder.HtmlBody = emailData.EmailBody;

                if (emailData.Attachments != null)
                {
                    byte[] fileBytes;
                    foreach (var file in emailData.Attachments)
                    {
                        if (file.Length > 0)
                        {
                            var ms = new MemoryStream();
                            await file.CopyToAsync(ms);
                            ms.Position = 0;
                            fileBytes = ms.ToArray();
                            emailBodyBuilder.Attachments.Add(file.FileName, fileBytes);
                        }
                    }
                }

                emailMessage.Body = emailBodyBuilder.ToMessageBody();

                SmtpClient emailClient = new SmtpClient();
                await emailClient.ConnectAsync(_emailDataConfiguration.Host, _emailDataConfiguration.Port, _emailDataConfiguration.UseSSL);
                await emailClient.AuthenticateAsync(_emailDataConfiguration.From, _emailDataConfiguration.Password);
                await emailClient.SendAsync(emailMessage);
                await emailClient.DisconnectAsync(true);
                emailClient.Dispose();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
