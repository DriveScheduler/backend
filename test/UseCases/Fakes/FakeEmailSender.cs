using Application.Abstractions;

namespace UseCases.Fakes
{
    internal record FakeEmail(string To, string Content);

    internal class FakeEmailSender : IEmailSender
    {
        public List<FakeEmail> EmailsSent { get; set; } = new();


        public Task SendAsync(string subject, string emailContent, string email)
        {
            EmailsSent.Add(new FakeEmail(email, emailContent));
            return Task.CompletedTask;
        }

        public Task SendAsync(string subject, string emailContent, List<string> emailList)
        {
            emailList.ForEach(emailList => EmailsSent.Add(new FakeEmail(emailList, emailContent)));
            return Task.CompletedTask;
        }
    }
}
