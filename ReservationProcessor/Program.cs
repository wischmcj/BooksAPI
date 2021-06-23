using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ReservationProcessor.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReservationProcessor
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    //services.AddHttpClient(); // any service now can add HttpClient to the constructor
                    //services.AddHttpClient("external");
                    //services.AddHttpClient("internal");
                    services.AddHttpClient<BooksLookupService>();
                    services.AddSingleton<ISendMessages, KafkaProducer>();
                    services.AddHostedService<Worker>();
                });
    }
}
