using Domain.Abstractions;

using MailKit.Net.Smtp;

using MimeKit;


namespace Infrastructure.ExternalServices
{
    internal class EmailSender : IEmailSender
    {
        private readonly SmtpClient _client;

        public EmailSender()
        {
            _client = new SmtpClient();
            _client.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);            
            _client.Authenticate("drivescheduler@gmail.com", "vxom emjl prqi noxj");            
        }

        public Task SendAsync(string subject, string emailContent, string email)
        {            
            try
            {                
                MimeMessage mail = new MimeMessage();
                mail.From.Add(new MailboxAddress("Drive Scheduler", "drivescheduler@gmail.com"));
                mail.To.Add(new MailboxAddress("", email));
                mail.Subject = subject;
                mail.Body = new TextPart("plain") { Text = emailContent };
                return _client.SendAsync(mail);                                
            }
            catch (Exception) {
                return Task.CompletedTask;
            }
            
        }

        public async Task SendAsync(string subject, string emailContent, List<string> emailList)
        {            
            try
            {
                foreach (string email in emailList)
                {                    
                    MimeMessage mail = new MimeMessage();
                    mail.From.Add(new MailboxAddress("Drive Scheduler", "drivescheduler@gmail.com"));              
                    mail.Subject = subject;
                    mail.Body = new TextPart("plain") { Text = emailContent };
                    mail.To.Add(new MailboxAddress("", email));
                    await _client.SendAsync(mail);
                }                
            }
            catch (Exception) {}            
        }
    }
}
