namespace Jbs.Yukari.Core.Models
{
    public class SearchResult
    {
        public ListItem ListItemTemplate { get; } = new ListItem();
        public PaginatedList<ListItem> Items { get; set; }
    }
}
