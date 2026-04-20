using Process.Domain.Entities;

namespace Process.Application.Menus.Mappers
{
    public static class MenuMapper
    {
        public static List<MenuResponse> BuildMenuTree(IEnumerable<Menu> menus)
        {
            if (menus == null) return [];

            var menuResponses = menus.Select(m => new MenuResponse
            {
                MenuId = m.MenuId,
                ParentId = m.ParentId,
                Order = m.Order,
                Title = m.Title,
                Description = m.Description,
                Icon = m.Icon,
                Link = m.Link,
                Selected = m.Selected,
                Active = m.Active,
                Visible= m.Visible,
                Children = []
            }).ToList();

            var lookup = menuResponses.ToDictionary(m => m.MenuId);
            var roots = new List<MenuResponse>();

            foreach (var menu in menuResponses)
            {
                if (menu.ParentId.HasValue && lookup.TryGetValue(menu.ParentId.Value, out var parent))
                {
                    parent.Children.Add(menu);
                }
                else
                {
                    roots.Add(menu);
                }
            } 

            SortMenusRecursively(roots);

            return roots;
        }

        private static void SortMenusRecursively(List<MenuResponse> menus)
        {
            menus.Sort((a, b) => a.Order.CompareTo(b.Order));
            foreach (var menu in menus)
            {
                if ((menu.Children?.Any()) == true)
                    SortMenusRecursively(menu.Children);
            }
        }
    }
}
