namespace API.Outputs.Users
{
    public sealed class UserAuthenticated
    {
        public Guid Id { get; set; }
        public string Token { get; set; }
    }
}
