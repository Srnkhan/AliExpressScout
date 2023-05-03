namespace AliExpressScout.Consumers.ThirdLayer
{
    using System.Threading.Tasks;
    using MassTransit;
    using Contracts;
    using Microsoft.Extensions.Logging;
    using Domain.Entities.Productions;
    using Arch.EntityFrameworkCore.UnitOfWork;
    using Application.Abstraction;
    using System;
    using System.Linq;
    using Domain.Entities.Categories;
    using System.Collections.Generic;
    using Microsoft.EntityFrameworkCore.Infrastructure;
    using System.Diagnostics;

    public class ThirdLayerConsumer :
        IConsumer<ThirdLayer>
    {
        private readonly IUnitOfWork UnitOfWork;
        private readonly IRepository<Production> ProductionRepository;
        private readonly ILogger<ThirdLayerConsumer> Logger;
        private readonly IAliExpressScoutFactory ScoutFactory;
        private readonly IRepository<Category> CategoryRepository;

        public ThirdLayerConsumer(IUnitOfWork unitOfWork,
            ILogger<ThirdLayerConsumer> logger,
            IAliExpressScoutFactory scoutFactory)
        {
            UnitOfWork = unitOfWork;
            ProductionRepository = UnitOfWork.GetRepository<Production>();
            CategoryRepository = UnitOfWork.GetRepository<Category>();
            Logger = logger;
            ScoutFactory = scoutFactory;
        }
        public async Task Consume(ConsumeContext<ThirdLayer> context)
        {
            Logger.LogInformation($"ThirdLayer Layer Started At : {DateTime.Now.ToString()} , " +
            $"Category Name : {context.Message.CategoryName} , " +
            $"Category Url : {context.Message.CategoryUrl}  , " +
            $"CorrelationId:{context.ConversationId}");
            using var currentPage = await ScoutFactory.ThirdLayerService.CreatePageAsync();
            var categories = (await CategoryRepository.GetAllAsync());
            var productions = await ProductionRepository.GetAllAsync(product => product.CategoryId == context.Message.CategoryId, null, null, true, false);
            if (productions.Any())
            {
                Logger.LogInformation($"Third Layer Production Found : {productions.Count} ," +
                    $"CorrelationId:{context.ConversationId}");
                var updateList = new List<Production>();
                foreach (var production in productions)
                {
                    try
                    {
                        await currentPage.GoToAsync(production.Url);                        
                        await ScoutFactory.ThirdLayerService.SetNormalPriceInformationsAsync(currentPage, production);
                        await ScoutFactory.ThirdLayerService.SetOriginalPriceInformationsAsync(currentPage, production);
                        updateList.Add(production);
                        Logger.LogInformation($"Third Layer updated production url is {production.Url}" +
                            $"CorrelationId:{context.ConversationId}");                        
                    }
                    catch (Exception ex)
                    {
                        Logger.LogError($"ThirdLayer Layer Error At: {DateTime.Now.ToString()}!, " +
                            $"Message: {ex.Message.ToString()}, " +
                            $"Category Url : {context.Message.CategoryUrl}," +
                            $"Order:{context.Message.Order}," +
                            $"Production:{production.Id}  , " +
                            $"CorrelationId:{context.ConversationId}");

                    }
                    finally
                    {
                        Logger.LogInformation($"ThirdLayer Layer Done At : {DateTime.Now.ToString()}  , " +
                            $"CorrelationId:{context.ConversationId}");
                    }
                }
                ProductionRepository.Update(updateList);
                await UnitOfWork.SaveChangesAsync();

                KillChromeProcess(currentPage?.Browser?.Process?.Id);
                var currentCategory = categories.FirstOrDefault(x => x.Id == context.Message.CategoryId);
                if (currentCategory.Order != categories.Max(x => x.Order))
                {
                    var nextCategory = categories.Where(x => x.Order > currentCategory.Order).OrderBy(x => x.Order).FirstOrDefault();
                    await context.Publish<ThirdLayer>(new ThirdLayer { CategoryId = nextCategory.Id, Order = nextCategory.Order, CategoryUrl = nextCategory.Url, CategoryName = nextCategory.Name }, default);
                }
                else if (currentCategory.Order != categories.Max(x => x.Order))
                {
                    await currentPage.Browser.CloseAsync();
                    var nextCategory = categories.Where(x => x.Order > currentCategory.Order).OrderBy(x => x.Order).FirstOrDefault();
                    await context.Publish<ThirdLayer>(new ThirdLayer { CategoryId = nextCategory.Id, CategoryUrl = nextCategory.Url, CategoryName = nextCategory.Name }, default);
                }

            }
        }
        private void KillChromeProcess(int? id)
        {
            var chromeProcess = Process.GetProcesses().FirstOrDefault(x => x.Id == id);
            if (chromeProcess != null)
            {
                chromeProcess.Kill();
            }
        }
        //public async Task Consume(ConsumeContext<ThirdLayer> context)
        //{
        //    Logger.LogInformation($"ThirdLayer Layer Started At : {DateTime.Now.ToString()} , " +
        //    $"Category Name : {context.Message.CategoryName} , " +
        //    $"Category Url : {context.Message.CategoryUrl}");
        //    var currentPage = await ScoutFactory.ThirdLayerService.CreatePageAsync();
        //    var categories = (await CategoryRepository.GetAllAsync());
        //    try
        //    {
        //        var productions = await ProductionRepository.GetAllAsync(product => product.CategoryId == context.Message.CategoryId, null, null, true, false);

        //        if (productions.Any())
        //        {
        //            var updateList = new List<Production>();
        //            foreach (var production in productions)
        //            {
        //                await currentPage.GoToAsync(production.Url);
        //                //await Task.Delay(2000);
        //                await ScoutFactory.ThirdLayerService.SetNormalPriceInformationsAsync(currentPage, production);
        //                await ScoutFactory.ThirdLayerService.SetOriginalPriceInformationsAsync(currentPage, production);
        //                updateList.Add(production);                        
        //            }
        //            ProductionRepository.Update(updateList);
        //            await UnitOfWork.SaveChangesAsync();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.LogError($"ThirdLayer Layer Error At: {DateTime.Now.ToString()}!, " +
        //            $"Message: {ex.Message.ToString()}, " +
        //            $"Category Url : {context.Message.CategoryUrl}," +
        //            $"Order:{context.Message.Order}");                
        //        throw new Exception(ex.Message);
        //    }
        //    finally
        //    {
        //        Logger.LogInformation($"ThirdLayer Layer Done At : {DateTime.Now.ToString()}");
        //        var currentCategory = categories.FirstOrDefault(x => x.Id == context.Message.CategoryId);
        //        if(currentCategory.Order != categories.Max(x => x.Order))
        //        {
        //            var nextCategory = categories.Where(x => x.Order > currentCategory.Order).OrderBy(x => x.Order).FirstOrDefault();
        //            await context.Publish<ThirdLayer>(new ThirdLayer { CategoryId = nextCategory.Id, Order = nextCategory.Order, CategoryUrl = nextCategory.Url, CategoryName = nextCategory.Name }, default);
        //        }
        //        await currentPage.Browser.CloseAsync();
        //    }
        //}
    }
}