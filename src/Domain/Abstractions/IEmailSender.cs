using Domain.Enums;

namespace Domain.Abstractions
{
    public interface IEmailSender
    {
        public Task SendAsync(string subject, string emailContent, string email);
        public Task SendAsync(string subject, string emailContent, List<string> emailList);
    }
}
