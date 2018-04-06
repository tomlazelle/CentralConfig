namespace Midway.Api.Models
{
    public class NameValueModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public string GroupName { get; set; }
        public string Environment { get; set; }
        public int Version { get; set; } = 1;
    }
}