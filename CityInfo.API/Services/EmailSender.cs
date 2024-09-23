using Microsoft.AspNetCore.Http.HttpResults;
using MimeKit;
using System.Net.Mail;
using MailKit.Net.Smtp;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;
using CityInfo.API.Models;
using Microsoft.Extensions.Options;

namespace CityInfo.API.Services
{
    public class EmailSender: IEmailSender
    {
        private readonly EmailSettings _emailsettings;
        public EmailSender(IOptions<EmailSettings> options)
        {
            this._emailsettings = options.Value;
        }
        public async Task SendEmailAsync(EmailRequest emailRequest)
        {
            try
            {
                var email = new MimeMessage();
                email.Sender = new MailboxAddress("DoctorManagementWebsite",_emailsettings.Email);
                email.To.Add(MailboxAddress.Parse(emailRequest.ToEmail));
                email.Subject = emailRequest.Subject;


                var builder = new BodyBuilder
                {
                    HtmlBody = emailRequest.Body
                };
                email.Body = builder.ToMessageBody();

                using var smtp = new SmtpClient();

                smtp.ServerCertificateValidationCallback = (s, c, h, e) => true;

                await smtp.ConnectAsync(_emailsettings.Host, _emailsettings.Port, MailKit.Security.SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync(_emailsettings.Email, _emailsettings.AppPassword);

                await smtp.SendAsync(email);

                await smtp.DisconnectAsync(true);
            }
            catch (SmtpCommandException ex)
            {
                Console.WriteLine($"SMTP Command Error: {ex.Message} (StatusCode: {ex.StatusCode})");
            }
            catch (SmtpProtocolException ex)
            {
                Console.WriteLine($"SMTP Protocol Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Email sending failed: {ex.Message}");
            }
        

        //var email = new MimeMessage();
        //email.From.Add(new MailboxAddress("May Chu", "emailsender.version01@gmail.com"));
        //email.To.Add(new MailboxAddress("Khach Hang", clientemail));
        //email.Subject = "Create appointment";

        //email.Body = new TextPart(MimeKit.Text.TextFormat.Html)
        //{
        //    Text = "Xin mời " + clientname
        //};

        //try
        //{
        //    using (var smtp = new SmtpClient())
        //    {
        //        string smtpEmail = Environment.GetEnvironmentVariable("SMTP_EMAIL") ?? "emailsender.version01@gmail.com";
        //        string smtpPassword = Environment.GetEnvironmentVariable("SMTP_PASSWORD") ?? "drceyiggjielcilv";

        //        await smtp.ConnectAsync("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
        //        await smtp.AuthenticateAsync(smtpEmail, smtpPassword);
        //        await smtp.SendAsync(email);
        //        await smtp.DisconnectAsync(true);
        //    }
        //}
        //catch (Exception ex)
        //{
        //    Console.WriteLine($"Failed to send email: {ex.Message}");
        //}
    }
    }
}
