namespace Midway.Api.Models
{
    public class BroadCastNotifyRequest
    {
        public string EventName { get; set; }
        public string Name { get; set; }
        public string GroupName { get; set; }
        public string UrlCallback { get; set; }
    }
}