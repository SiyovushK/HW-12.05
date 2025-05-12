using System.Net;
using System.Net.Mail;
using Infrastructure.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Services;

public class EmailService(IConfiguration config) : IEmailService
{
    public async Task SendEmailAsync(string to, string subject, string body)
    {
        var smtpClient = new SmtpClient(config["Smtp:Host"])
        {
            Port = int.Parse(config["Smtp:Port"]!),
            Credentials = new NetworkCredential(config["Smtp:User"], config["Smtp:Password"]),
            EnableSsl = bool.Parse(config["Smtp:EnableSsl"]!)
        };

        var mail = new MailMessage
        {
            From = new MailAddress(config["Smtp:User"]!),
            Subject = subject,
            Body = body,
            IsBodyHtml = true
        };

        mail.To.Add(to);

        await smtpClient.SendMailAsync(mail);
    }
}