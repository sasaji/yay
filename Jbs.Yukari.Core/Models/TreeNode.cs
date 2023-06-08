using System;
using System.Collections.Generic;

namespace Jbs.Yukari.Core.Models
{
    public class TreeNode
    {
        public Guid Yid { get; set; }
        public Guid ParentYid { get; set; }
        public string Id { get; set; }
        public string Text { get; set; }
        public int Level { get; set; }
        public List<TreeNode> Nodes { get; set; } = new List<TreeNode>();
    }
}
