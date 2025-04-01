namespace Med.MessageBus.Integration.Responses.Users
{
    public record CreateUserResponse
    {
        public bool Success { get; set; }
        public Guid UserId { get; set; }
        public string? Token { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
