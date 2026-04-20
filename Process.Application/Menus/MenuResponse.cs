namespace Process.Application.Menus
{
    public class MenuResponse
    {
        public long MenuId { get; set; }
        public long? ParentId { get; set; }
        public int Order { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public string Link { get; set; } = string.Empty;
        public bool Selected { get; set; }
        public bool Active { get; set; }

        public bool Visible { get; set; }

        public List<MenuResponse> Children { get; set; } = new();
    }
}
