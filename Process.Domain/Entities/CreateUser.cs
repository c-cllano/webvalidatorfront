namespace Process.Domain.Entities
{
    public class CreateUser
    {
        public string DocumentType { get; set; } = string.Empty;
        public string DocumentNumber { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string SecondName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string SecondLastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public bool EmailConfirmed { get; set; }
        public int CreatedUserId { get; set; }
        public string ConfirmPassword { get; set; } = string.Empty;
        public int ClientId { get; set; }
        public int RoleId { get; set; }
        public List<long> Agreements { get; set; } = default!;
    }
}
