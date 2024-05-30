namespace API.Outputs.Users
{
    public sealed class UserAuthenticated
    {
        public Guid UserId { get; set; }
        public string Token { get; set; }
    }
}
