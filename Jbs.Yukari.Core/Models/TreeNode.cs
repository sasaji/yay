using System;
using System.Collections.Generic;

namespace Jbs.Yukari.Core.Models
{
    public class TreeNode
    {
        public Guid Id { get; set; }
        public Guid ParentId { get; set; }
        public string Code { get; set; }
        public string Text { get; set; }
        public int Level { get; set; }
        public List<TreeNode> Nodes { get; set; } = new List<TreeNode>();
    }
}
