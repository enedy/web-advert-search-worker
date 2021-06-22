using Amazon.Lambda.Core;
using System.Collections.Generic;
using Nest;
using Newtonsoft.Json;
using System.Threading.Tasks;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace WebAdvert.SearchWorker
{
    public class Functions
    {
        public Functions():this(ElasticSearchHelper.GetInstance(ConfigurationHelper.Instance))
        {

        }

        private readonly IElasticClient _client;
        public Functions(IElasticClient client)
        {
            _client = client;
        }


        public async Task<bool> Teste(SNSEvent eventStr, ILambdaContext context)
        {
            context.Logger.LogLine("log event");

            foreach (var record in eventStr.Records)
            {
                var message = JsonConvert.DeserializeObject<AdvertConfirmedMessage>(record.Sns.Message);
                var advertDocument = MappingHelper.Map(message);
                await _client.IndexDocumentAsync(advertDocument);
            }

            return true;
        }

        public class SNSEvent
        {
            public IList<SNSRecord> Records { get; set; }

            public class SNSRecord
            {
                public SNSMessage Sns { get; set; }
            }

            public class SNSMessage
            {
                public string Message { get; set; }
               
            }
        }
    }
}
