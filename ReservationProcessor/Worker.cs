using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;



namespace ReservationProcessor
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;



        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }



        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {



            var config = new ConsumerConfig
            {
                BootstrapServers = "localhost:9092",
                GroupId = "ReservationProcessor",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };



            var consumer = new ConsumerBuilder<Ignore, string>(config).Build();
            consumer.Subscribe("reservation-requests");
            while (!stoppingToken.IsCancellationRequested)
            {
                var consumedResult = consumer.Consume(); // no async. This is a blocking call.
                var message = JsonSerializer.Deserialize<Reservation>(consumedResult.Message.Value);
                _logger.LogInformation($"Got a reservation for {message.For} for the following books {message.Books}");
            }
        }
    }
    public class Reservation
    {
        public string ReservationId { get; set; }
        public string For { get; set; }
        public string[] Books { get; set; }
        public string Status { get; set; }
    }


}
