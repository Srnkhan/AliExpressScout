using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using MassTransit;
using AliExpressScout.Consumers.ZeroLayer;

using Microsoft.Extensions.DependencyInjection;
using LogExtension;
using UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Arch.EntityFrameworkCore.UnitOfWork;
using Application.Abstraction;
using Application.Implementation;
using AliExpressScout.Consumers.FirstLayer;
using AliExpressScout.Consumers.SecondLayer;
using AliExpressScout.Consumers.ThirdLayer;

namespace AliExpressScout
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            await CreateHostBuilder(args).Build().RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddMassTransit(x =>
                    {
                        // elided...
                        x.UsingRabbitMq((context, cfg) =>
                        {
                            cfg.Host("208.64.33.68", "/", h =>
                            {
                                h.Username("guest");
                                h.Password("guest");
                            });

                            cfg.ConfigureEndpoints(context);
                        });
                        x.AddConsumer<ZeroLayerConsumer>(typeof(ZeroLayerConsumerDefinition));
                        x.AddConsumer<FirstLayerConsumer>(typeof(FirstLayerConsumerDefinition));
                        x.AddConsumer<SecondLayerConsumer>(typeof(SecondLayerConsumerDefinition));
                        x.AddConsumer<ThirdLayerConsumer>(typeof(ThirdLayerConsumerDefinition));
                    });
                    services.AddHostedService<Worker>();

                    services
                      .AddDbContext<AliExpressScoutDbContext>(opt =>
                      opt.UseSqlServer("Data Source=208.64.33.68, 52359;Database=AliExpressScout;User Id=srnk;Password=Mrkb6895!;TrustServerCertificate=True"))
                      .AddUnitOfWork<AliExpressScoutDbContext>();

                    services.AddTransient<IAliExpressScoutFactory, AliExpressScoutFactoryImplementation>();
                    services.AddTransient<IZeroLayerService, ZeroLayerServiceImplementation>();
                    services.AddTransient<ISecondLayerService, SecondLayerServiceImplementation>();
                    services.AddTransient<IThirdLayerService, ThirdLayerServiceImplementation>();
                }).MongoUseSerilog();
    }
}
