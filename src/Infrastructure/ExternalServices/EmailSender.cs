using Domain.Abstractions;
using Domain.Enums;

namespace Infrastructure.ExternalServices
{
    internal class EmailSender : IEmailSender
    {
        public Task SendAsync(NotificationType type, string emailContent, string email)
        {
            throw new NotImplementedException();
        }

        public Task SendAsync(NotificationType type, string emailContent, List<string> emailList)
        {
            throw new NotImplementedException();
        }
    }
}
