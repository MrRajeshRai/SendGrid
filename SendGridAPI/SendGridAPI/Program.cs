using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;



namespace SendGridAPI
{
    class Program
    {
        // Commons
        static string fromAddress = ConfigurationManager.AppSettings["FromAddress"];
        static string toAddress = ConfigurationManager.AppSettings["ToAddress"];

        // Needed for SendGrid V2
        static string sendGridUserName = ConfigurationManager.AppSettings["SendGridUserName"];
        static string sendGridPassword = ConfigurationManager.AppSettings["SendGridPassword"];

        // Needed for sendgrid V3
        static string sendGridAPIKey = ConfigurationManager.AppSettings["SendGridAPIKey"];

        static void Main(string[] args)
        {
            #region SendGrid API V2
            // Without SendGrid Library
            SendGridAPIV2().Wait();
            #endregion

            #region SendGrid API V3
            SendGridAPIV3(sendGridAPIKey).Wait();
            #endregion
        }

        /// <summary>
        /// Using .NET only
        /// </summary>
        /// <returns></returns>
        static async Task SendGridAPIV2()
        {
                MailMessage mailMsg = new MailMessage();

                mailMsg.From = new MailAddress(fromAddress, "Name");
                mailMsg.To.Add(new MailAddress(toAddress, "Name"));
                mailMsg.Subject = "Test Email";             
              
                SmtpClient smtpClient = new SmtpClient("smtp.sendgrid.net", 587);
                System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(sendGridUserName, sendGridPassword);
                smtpClient.Credentials = credentials;

                await smtpClient.SendMailAsync(mailMsg);
            
        }

        /// <summary>
        /// SendGrid API V3 using sendgrid library
        /// </summary>
        /// <param name="apiKey"></param>
        /// <returns></returns>
        static async Task SendGridAPIV3(string apiKey)
        { 
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("test@example.com", "Example User");
            var subject = "Sending with SendGrid is Fun";
            var to = new EmailAddress("test@example.com", "Example User");
            var plainTextContent = "and easy to do anywhere, even with C#";
            var htmlContent = "<strong>and easy to do anywhere, even with C#</strong>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            await client.SendEmailAsync(msg);
        }
    }
}
