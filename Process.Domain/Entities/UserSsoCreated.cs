namespace Process.Domain.Entities;

public class UserSsoCreated
{
    public int UserId { get; set; }
    public int UserStatusId { get; set; }
    public Guid UserGuid { get; set; }
    public string UserStatus  { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string SecondName { get; set; } = string.Empty;
    public string FirstLastName { get; set; } = string.Empty;
    public string SecondLastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string DocumentType { get; set; } = string.Empty;
    public string DocumentNumber { get; set; } = string.Empty;  
    public DateTime? LastLoginDate { get; set; }  
    public DateTime CreationDate { get; set; }
    public int CreatorUserId { get; set; }
    public bool IsDeleted { get; set; }
}