using MailKit.Security;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
//using MyProject.Controllers;

namespace MyProject.EmailServices;

[Route("api/[controller]")]
[ApiController]
//public class EmailController : ClientBaseController
//{
//    private readonly IConfiguration _configuration;
//    public EmailController(IConfiguration configuration)
//    {
//        _configuration = configuration;
//    }

//    [Route("sendto")]
//    [HttpPost]
//    public dynamic ToEmailConfirm(dynamic obj)
//    {

//        MailContent content = new MailContent
//        {
//            To = obj.To,
//            Subject = obj.Subject,
//            Body = obj.Body
//        };
//        var check = SendMail(content);
//        //  return View();
//        return Ok(true);
//    }

//    public async Task SendMail(MailContent mailContent)
//    {
//        MailModel mailSettings = new MailModel
//        {
//            Mail = _configuration["MailSettings:Mail"],
//            DisplayName = _configuration["MailSettings:DisplayName"],
//            Password = _configuration["MailSettings:Password"],
//            Host = _configuration["MailSettings:Host"],
//            Port = Int16.Parse(_configuration["MailSettings:Port"])
//        };
//        var email = new MimeMessage();
//        email.Sender = new MailboxAddress(mailSettings.DisplayName, mailSettings.Mail);
//        email.From.Add(new MailboxAddress(mailSettings.DisplayName, mailSettings.Mail));
//        email.To.Add(MailboxAddress.Parse(mailContent.To));
//        email.Subject = mailContent.Subject;

//        var builder = new BodyBuilder();
//        builder.HtmlBody = mailContent.Body;
//        email.Body = builder.ToMessageBody();

//        // dùng SmtpClient của MailKit
//        using var smtp = new MailKit.Net.Smtp.SmtpClient();

//        try
//        {
//            smtp.Connect(mailSettings.Host, mailSettings.Port, SecureSocketOptions.StartTls);
//            smtp.Authenticate(mailSettings.Mail, mailSettings.Password);
//            await smtp.SendAsync(email);
//        }
//        catch (Exception ex)
//        {
//            // Gửi mail thất bại, nội dung email sẽ lưu vào thư mục mailssave
//            System.IO.Directory.CreateDirectory("mailssave");
//            var emailsavefile = string.Format(@"mailssave/{0}.eml", Guid.NewGuid());
//            await email.WriteToAsync(emailsavefile);

//        }

//        smtp.Disconnect(true);
//    }
//}


public class MailModel
{
public string Mail { get; set; }
public string DisplayName { get; set; }
public string Password { get; set; }
public string Host { get; set; }
public int Port { get; set; }
}

public class MailContent
{
public string To { get; set; }              // Địa chỉ gửi đến
public string Subject { get; set; }         // Chủ đề (tiêu đề email)
public string Body { get; set; }            // Nội dung (hỗ trợ HTML) của email
}
