namespace Jbs.Yukari.Core.Models
{
    public class SearchResult
    {
        public BasicInfo ListItemTemplate { get; } = new BasicInfo();
        public PaginatedList<BasicInfo> Items { get; set; }
    }
}
