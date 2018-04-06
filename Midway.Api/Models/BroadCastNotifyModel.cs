namespace Midway.Api.Models
{
    public class BroadCastNotifyModel
    {
        public int Id { get; set; }
        public string EventName { get; set; }
        public string Name { get; set; }
        public string GroupName { get; set; }
        public string UrlCallback { get; set; }
    }
}