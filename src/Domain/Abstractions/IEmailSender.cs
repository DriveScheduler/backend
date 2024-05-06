using Domain.Enums;

namespace Domain.Abstractions
{
    public interface IEmailSender
    {
        public Task SendAsync(NotificationType type, string emailContent, string email);
        public Task SendAsync(NotificationType type, string emailContent, List<string> emailList);
    }
}
