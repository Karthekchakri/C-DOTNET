using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NemsisImport
{
    public static class KafkaProducerConfig
    {
        public static ProducerConfig GetConfig()
        {
            return new ProducerConfig
            {
                BootstrapServers = "localhost:9092", // Replace with your Kafka broker address
                ClientId = "KafkaExampleProducer",
            };
        }
    }
}
