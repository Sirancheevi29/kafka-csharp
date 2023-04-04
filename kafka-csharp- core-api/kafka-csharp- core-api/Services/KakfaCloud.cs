using System;
using System.Diagnostics;
using System.Net;
using Confluent.Kafka;
using kafka_csharp__core_api.Model;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Swagger;

namespace kafka_csharp__core_api.Services
{
    public class KakfaCloud
    {
        public ProducerConfig _producerConfig { get; set; }
        public ConsumerConfig _consumerConfig { get; set; }
        public KakfaCloud()
        {
            _producerConfig = new ProducerConfig
            {
                BootstrapServers = "",
                ClientId = Dns.GetHostName(),
                SaslMechanism = SaslMechanism.Plain,
                SaslUsername = "",
                SaslPassword = "",
                SecurityProtocol = SecurityProtocol.SaslSsl,
                Acks = Acks.Leader
            };

            _consumerConfig = new ConsumerConfig
            {
                BootstrapServers = "",
                ClientId = Dns.GetHostName(),
                SaslMechanism = SaslMechanism.Plain,
                SaslUsername = "",
                SaslPassword = "",
                SecurityProtocol = SecurityProtocol.SaslSsl,
                GroupId = "movies_group",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };
        }

        public bool Producer(string topic, List<Movies> movies)
        {
            try
            {
                using (var producer = new ProducerBuilder<Null, string>(_producerConfig).Build())
                {
                    producer.Produce(topic, new Message<Null, string> { Timestamp = new Timestamp(DateTime.Now), Value = JsonConvert.SerializeObject(movies) });
                }

                return true;
            }
            catch (ProduceException<Null, string> ex)
            {
                return false;
            }
        }

        public List<Movies> Consumer(string topic)
        {
            List<Movies> movies = new List<Movies>();
            try
            {
                using (var consumerBuilder = new ConsumerBuilder
                <Ignore, string>(_consumerConfig).Build())
                {
                    consumerBuilder.Subscribe(topic);
                    var cancelToken = new CancellationTokenSource();

                    try
                    {
                        while (true)
                        {
                            var consumer = consumerBuilder.Consume
                               (cancelToken.Token);
                            movies = JsonConvert.DeserializeObject<List<Movies>>(consumer.Message.Value);
                            
                        }
                    }
                    catch (OperationCanceledException)
                    {
                        consumerBuilder.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            return movies;
        }

    }
}
