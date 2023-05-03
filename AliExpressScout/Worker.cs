using Arch.EntityFrameworkCore.UnitOfWork;
using Contracts;
using Domain.Entities.Categories;
using MassTransit;
using MassTransit.Transports;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace AliExpressScout
{
    public class Worker : BackgroundService
    {
        public readonly IPublishEndpoint PublishEndpoint;

        public Worker(IServiceProvider serviceProvider)
        {
            PublishEndpoint = serviceProvider.CreateScope().ServiceProvider.GetRequiredService<IPublishEndpoint>();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await PublishEndpoint.Publish<ThirdLayer>(new ThirdLayer { Order = 12 , CategoryId = Guid.Parse("702f81db-a50e-499b-80d4-08db47e653ea") , CategoryName = "Home Improvement" , CategoryUrl = "https://www.aliexpress.com/category/13/home-improvement.html" });

            //await PublishEndpoint.Publish<ZeroLayer>(new ZeroLayer() );
        }
    }
}