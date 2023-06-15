using Jbs.Yukari.Core.Models;

namespace Jbs.Yukari.Web.Models
{
    public class HomeViewModel
    {
        public string TreeJson { get; set; } = string.Empty;
        public int SelectedNodeId { get; set; } = 1;
        public string TabIndex { get; set; } = string.Empty;
        public SearchCriteria SearchCriteria { get; set; } = new SearchCriteria();
        public SearchResult SearchResult { get; set; } = new SearchResult();
        public int TotalCount { get; set; } = 0;
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 15;
        public bool FirstPage { get; set; }
    }
}
