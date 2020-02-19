using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using Quiksoft.FreeSMTP;
using System.Threading;
namespace MVCSite.EmailService
{
    public class Emailer
    {
        private readonly EmailerSettings _settings;

        public Emailer(EmailerSettings settings)
        {
            _settings = settings;
        }

        public void SendHtml(string from, string to, string subject, string body)
        {
            if (_settings.SendToFolder)
            {
                SendHtmlToFolder(from, to, subject, body);
                return;
            }

            using (var mailMsg = new MailMessage())
            {

                mailMsg.To.Add(to);

                var mailAddress = new MailAddress(from, _settings.SMTPDisplayUserName,System.Text.Encoding.UTF8);
                mailMsg.From = mailAddress;

                mailMsg.Subject = subject;
                mailMsg.Body = body;
                mailMsg.IsBodyHtml = true;

                using (SmtpClient smtpClient = new SmtpClient(_settings.SmtpServerName, _settings.SmtpServerPort))
                {
                    smtpClient.UseDefaultCredentials = !_settings.RequiresAuthentication;
                    if (_settings.RequiresAuthentication)
                    {
                        smtpClient.Credentials = new System.Net.NetworkCredential(_settings.SmtpServerUserName,
                                                                                  _settings.SmtpServerUserPassword);
                        smtpClient.EnableSsl = _settings.EnableSSL;
                    }
                    smtpClient.Send(mailMsg);
                }
                mailAddress = null;
            }

        }

        void SendHtmlToFolder(string from, string to, string subject, string body)
        {
            var emailMessage = new EmailMessage();
            emailMessage.From.Email = from;
            emailMessage.Recipients.Add(to, to, RecipientType.To);
            emailMessage.CharsetEncoding = Encoding.UTF8;
            emailMessage.Subject = subject;

            var htmlBodyPart = new BodyPart();
            htmlBodyPart.Format = BodyPartFormat.HTML;
            htmlBodyPart.Body = body;
            emailMessage.BodyParts.Add(htmlBodyPart);
            var smtp = new SMTP();
            smtp.SubmitToExpress(emailMessage, _settings.OutgoingFolder);
        }
    }

    public class EmailerSettings
    {
        public EmailerSettings()
        {
            SmtpServerPort = 25;
            RequiresAuthentication = false;
            EnableSSL = false;
        }

        public string SmtpServerName { get; set; }
        public int SmtpServerPort { get; set; }
        public bool RequiresAuthentication { get; set; }
        public bool EnableSSL { get; set; }
        public string SmtpServerUserName { get; set; }
        public string SmtpServerUserPassword { get; set; }

        public string SMTPDisplayUserName { get; set; }

        public bool SendToFolder { get; set; }
        public string OutgoingFolder { get; set; }
        public int SendEmailInterval { get; set; }
        public int MaxDegreeOfParallelism { get; set; }
        public int WaitSecondsAfterScan { get; set; }        
    }
}
