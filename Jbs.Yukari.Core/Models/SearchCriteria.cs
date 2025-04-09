using System.Collections.Generic;
using System.ComponentModel;

namespace Jbs.Yukari.Core.Models
{
    public class SearchCriteria
    {
        public string SelectedNode { get; set; }

        [DisplayName("コード")]
        public string Code { get; set; }

        [DisplayName("種別")]
        public string Type { get; set; }

        [DisplayName("名前")]
        public string Name { get; set; }

        [DisplayName("状態")]
        public int? Phase { get; set; }

        public List<KeyValuePair<int?, string>> PhaseList =
        [
            new(null, ""),
            new(0, "反映済み"),
            new(1,"編集中"),
            new(2, "反映待ち")
        ];
        public string RegisterDateFrom { get; set; }
        public string RegisterDateTo { get; set; }

        [DisplayName("削除済みを含める")]
        public bool IncludeDeleted { get; set; }
    }
}
