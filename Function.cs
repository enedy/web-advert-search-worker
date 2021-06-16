using Amazon.Lambda.Core;
using System.Collections.Generic;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace WebAdvert.SearchWorker
{
    public class Functions
    {
        /// <summary>
        /// Default constructor that Lambda will invoke.
        /// </summary>
        public Functions()
        {
        }


        public bool Teste(SNSEvent eventStr, ILambdaContext context)
        {
            context.Logger.LogLine("log event");

            foreach (var record in eventStr.Records)
            {
                var message = record.Sns.Message;
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
