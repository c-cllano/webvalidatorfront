namespace Process.Domain.Parameters.Sso.User
{
    public class UpdateUserRequest
    {
        public string FirstName { get; set; } = string.Empty;
        public string SecondName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string SecondLastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string DocumentType { get; set; } = string.Empty;
        public string DocumentNumber { get; set; } = string.Empty;
        public int ClientId { get; set; }
        public int RoleId { get; set; }
        public string? CellPhone { get; set; }
        public List<AssignmentRoleAgreements> AssignmentRoleAgreements { get; set; } = [];
        public bool Status { get; set; }
    }
}
