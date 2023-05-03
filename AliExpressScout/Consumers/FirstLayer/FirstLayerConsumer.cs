namespace AliExpressScout.Consumers.FirstLayer
{
    using System.Threading.Tasks;
    using MassTransit;
    using Contracts;
    using Arch.EntityFrameworkCore.UnitOfWork;
    using Domain.Entities.Categories;
    using Microsoft.Extensions.Logging;
    using System;
    using Newtonsoft.Json;
    using System.Linq;

    public class FirstLayerConsumer :
        IConsumer<FirstLayer>
    {
        private readonly ILogger<FirstLayerConsumer> Logger;        
        private readonly IUnitOfWork UnitOfWork;
        private readonly IRepository<Category> CategoryRepository;

        public FirstLayerConsumer(
           ILogger<FirstLayerConsumer> logger,
           IUnitOfWork unitOfWork,
           IRequestClient<FirstLayerConsumer> client)
        {
            Logger = logger;
            UnitOfWork = unitOfWork;
            CategoryRepository = UnitOfWork.GetRepository<Category>();
        }

        public async Task Consume(ConsumeContext<FirstLayer> context)
        {
            Logger.LogInformation($"First Layer Started At : {DateTime.Now.ToString()} , " +
                $"CorrelationId:{context.CorrelationId}");
            var firstCategory = (await CategoryRepository.GetAllAsync()).OrderBy(x => x.Order).FirstOrDefault();
            if(firstCategory != null)
            {
                await context.Publish<SecondLayer>(new SecondLayer { CategoryId = firstCategory.Id, CategoryUrl = firstCategory.Url, CategoryName = firstCategory.Name }, default);
            }
            Logger.LogInformation($"First Layer Done At : {DateTime.Now.ToString()} , " +
                $"CorrelationId:{context.ConversationId}");           
        }
    }
}