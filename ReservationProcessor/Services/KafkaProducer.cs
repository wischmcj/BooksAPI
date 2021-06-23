using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using ReservationProcessor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ReservationProcessor.Services
{
    public class KafkaProducer : ISendMessages
    {

        private readonly IConfiguration _config;
        private readonly IProducer<Null, string> _producer;
        private readonly string _successTopic;
        private readonly string _failureTopic;
        public KafkaProducer(IConfiguration config)
        {
            _config = config;
            var producerConfig = new ProducerConfig
            {
                BootstrapServers = _config["Kafka:BootstrapServers"],
                ClientId = "ReservationProcessor"
            };

            _producer = new ProducerBuilder<Null, string>(producerConfig).Build();

            _successTopic = _config["Kafka:ProducerSuccessTopic"];
            _failureTopic = _config["Kafka:ProducerFailureTopic"];
        }

        public async Task WriteSuccessfulReservation(BookReservationResponse message)
        {
            var json = JsonSerializer.Serialize(message);
            await _producer.ProduceAsync(_successTopic, new Message<Null, string> { Value = json });
        }

        public async Task WriteFailedReservation(BookReservationResponse message)
        {
            var json = JsonSerializer.Serialize(message);
            await _producer.ProduceAsync(_failureTopic, new Message<Null, string> { Value = json });
        }
    }
}
