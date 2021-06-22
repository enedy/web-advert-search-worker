using System;

namespace WebAdvert.SearchWorker
{
    public class AdvertConfirmedMessage
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public DateTime CreationDateTime { get; set; }
    }
}
