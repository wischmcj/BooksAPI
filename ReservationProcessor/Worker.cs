using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ReservationProcessor.Models;
using ReservationProcessor.Services;
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
        private readonly IConfiguration _config;
        private readonly BooksLookupService _booksLookupService;
        private readonly ISendMessages _messageSender;
        public Worker(ILogger<Worker> logger, IConfiguration config, BooksLookupService booksLookupService, ISendMessages messageSender)
        {
            _logger = logger;
            _config = config;
            _booksLookupService = booksLookupService;
            _messageSender = messageSender;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            var config = new ConsumerConfig
            {
                BootstrapServers = _config["Kafka:BoostrapServers"],
                GroupId = _config["Kafka:GroupId"],
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            var consumer = new ConsumerBuilder<Ignore, string>(config).Build();
            consumer.Subscribe(_config["Kafka:Topic"]);
            // reservation-reqeust => reservation-approved | reservation-denied
            while (!stoppingToken.IsCancellationRequested)
            {
                var consumedResult = consumer.Consume(); // no async. This is a blocking call.
                var message = JsonSerializer.Deserialize<Reservation>(consumedResult.Message.Value);
                _logger.LogInformation($"Got a reservation  for {message.For} for the following books {message.Books}");
                var books = new List<Book>();
                var allGood = true;
                foreach(var bookId in message.Books)
                {
                    var response = await _booksLookupService.CheckIfBookExists(bookId);
                    if(response.exists)
                    {
                        books.Add(response.book);
                    } else
                    {
                        allGood = false; // this will tell us we should send a message that says this reservation is bad.
                        books.Add(new Book { Id = bookId, Title = "No Such Book", Author = "No Such Author" });
                    }
                }
                if (allGood)
                {
                   
                    BookReservationResponse response = new()
                    {
                        ReservationId = message.ReservationId,
                        For =message.For,
                        Books = books,
                        Status = "Approved"
                    };

                    await _messageSender.WriteSuccessfulReservation(response);

                } else
                {
                    BookReservationResponse response = new()
                    {
                        ReservationId = message.ReservationId,
                        For = message.For,
                        Books = books,
                        Status = "Failed"
                    };
                    await _messageSender.WriteFailedReservation(response);
                }
            }

            consumer.Close();
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
