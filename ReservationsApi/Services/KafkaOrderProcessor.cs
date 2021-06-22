using Confluent.Kafka;
using ReservationsApi.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace ReservationsApi.Services
{
    public class KafkaOrderProcessor : IProcessOrders
    {
        IProducer<Null, string> _producer;

        public KafkaOrderProcessor()
        {
            var config = new ProducerConfig()
            {
                BootstrapServers = "localhost:9092",
                ClientId = Dns.GetHostName()
            };

            _producer = new ProducerBuilder<Null, string>(config).Build();
        }
        public async Task Send(ReservationResponse request)
        {
            var json = JsonSerializer.Serialize(request);
            await _producer.ProduceAsync("reservation-requests", new Message<Null, string> { Value = json });
        }
    }
}