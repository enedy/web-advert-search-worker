namespace WebAdvert.SearchWorker
{
    public static class MappingHelper
    {
        public static AdvertType Map(AdvertConfirmedMessage message)
        {
            return new AdvertType
            {
                Id = message.Id,
                Title = message.Title,
                CreationDateTime = message.CreationDateTime
            };

        }
    }
}
