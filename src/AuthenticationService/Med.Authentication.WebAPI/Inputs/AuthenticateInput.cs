namespace Med.Authentication.WebAPI.Inputs
{
    public record AuthenticateInput
    {
        public required string UsernameOrEmail { get; set; }
        public required string Password { get; set; }
    }
}
