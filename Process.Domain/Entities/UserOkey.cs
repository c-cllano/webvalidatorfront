namespace Process.Domain.Entities
{
    public class UserOkey
    {
        public long UserId { get; set; }
        public Guid UserGuid { get; set; }
        public string? CellPhone { get; set; }
        public long? CreatorUserId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool Active { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
    }
}
