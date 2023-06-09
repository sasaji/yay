namespace Jbs.Yukari.Core.Models
{
    public class SearchResult
    {
        public BasicInfoOutline ListItemTemplate { get; } = new BasicInfoOutline();
        public PaginatedList<BasicInfoOutline> Items { get; set; }
    }
}
