namespace Jbs.Yukari.Web.Models
{
    public interface IEditViewModel
    {
        public Guid Yid { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public int Phase { get; set; }
        public string TabIndex { get; set; }
        public string ObjectTabIndex { get; set; }
    }
}
