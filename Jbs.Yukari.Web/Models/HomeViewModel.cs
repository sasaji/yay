using Jbs.Yukari.Core.Models;

namespace Jbs.Yukari.Web.Models
{
    public class HomeViewModel
    {
        public string TreeJson { get; set; } = string.Empty;
        public string TabIndex { get; set; } = string.Empty;
        public SearchCriteria SearchCriteria { get; set; } = new SearchCriteria();
        public SearchResult SearchResult { get; set; } = new SearchResult();
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public bool FirstPage { get; set; }
    }
}
