namespace Process.Domain.Entities
{
    public class Menu
    {
        public long MenuId { get; set; }
        public long? ParentId { get; set; }
        public int Order { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public string Link { get; set; } = string.Empty;
        public bool Selected { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool Active { get; set; }
        public bool IsDeleted { get; set; } 

        public bool Visible { get; set; }
        public ICollection<Menu> Children { get; set; } = default!;
    }
}
