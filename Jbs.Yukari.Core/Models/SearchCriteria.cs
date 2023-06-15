using System.Collections.Generic;

namespace Jbs.Yukari.Core.Models
{
    public class SearchCriteria
    {
        public string SelectedNode { get; set; }
        public string Id { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public int? Phase { get; set; }
        public List<KeyValuePair<int?, string>> PhaseList = new List<KeyValuePair<int?, string>>
        {
            new KeyValuePair<int?, string>(null, ""),
            new KeyValuePair<int?, string>(0, "反映済み"),
            new KeyValuePair<int?, string>(1,"編集中"),
            new KeyValuePair<int?, string>(2, "反映待ち")
        };
    }
}
