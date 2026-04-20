namespace Process.Domain.Entities
{
    public class RoleByUser
    {
        public long RoleByUserId { get; set; }
        public long RoleId { get; set; }
        public int UserId { get; set; }
        public long? CreatorUserId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool Active { get; set; }
        public bool IsDeleted { get; set; }
    }
}
