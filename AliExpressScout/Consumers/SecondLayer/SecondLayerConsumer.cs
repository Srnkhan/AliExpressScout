namespace AliExpressScout.Consumers.SecondLayer
{
    using System.Threading.Tasks;
    using MassTransit;
    using Contracts;
    using Microsoft.Extensions.Logging;
    using Application.Abstraction;
    using PuppeteerSharp;
    using Arch.EntityFrameworkCore.UnitOfWork;
    using Domain.Entities.Productions;
    using System.Collections.Generic;
    using System;
    using System.Linq;
    using Serilog.Core;
    using Domain.Entities.Categories;
    using System.Diagnostics;

    public class SecondLayerConsumer :
        IConsumer<SecondLayer>
    {
        private readonly ILogger<SecondLayerConsumer> Logger;
        private readonly IUnitOfWork UnitOfWork;
        private readonly IRepository<Production> ProductRepository;
        private readonly IRepository<Category> CategoryRepository;
        private readonly IAliExpressScoutFactory ScoutFactory;

        public SecondLayerConsumer(
           ILogger<SecondLayerConsumer> logger,
           IUnitOfWork unitOfWork,
           IAliExpressScoutFactory scoutFactory
           )
        {
            Logger = logger;
            UnitOfWork = unitOfWork;
            ProductRepository = UnitOfWork.GetRepository<Production>();
            CategoryRepository = UnitOfWork.GetRepository<Category>();
            ScoutFactory = scoutFactory;
        }

        public async Task Consume(ConsumeContext<SecondLayer> context)
        {
            Logger.LogInformation($"Second Layer Started At : {DateTime.Now.ToString()} , " +
                $"Category Name : {context.Message.CategoryName} , " +
                $"Category Url : {context.Message.CategoryUrl} ," +
                $"CorrelationId:{context.ConversationId}");
            var categories = (await CategoryRepository.GetAllAsync()).ToList();
            var currentCategory = categories.FirstOrDefault(x => x.Id == context.Message.CategoryId);

            using var currentPage = await ScoutFactory.SecondLayerService.CreatePageAsync(context.Message.CategoryUrl);
            int page = 1;
            var productions = new List<string>();
            try
            {               
                while (page < 70)
                {
                    Logger.LogInformation($"Page : {page},Productions Count: {productions.Count()}");
                    var newProductions = await ScoutFactory.SecondLayerService.TraversePageAsync(currentPage);

                    var nextPage = await ScoutFactory.SecondLayerService.ClickNextPageAsync(currentPage);
                    var lastPaginationCss = await ScoutFactory.SecondLayerService.CheckLastPageAsync(currentPage);

                    if (!newProductions.Any())
                        break;
                    else
                        productions.AddRange(newProductions);
                    if (lastPaginationCss.ResultObject.ToString().Contains("notAllowed"))
                        break;
                    page++;
                }
                productions = productions.Distinct().ToList();
                //await currentPage.Browser.CloseAsync();
                await CreateOrUpdateAsync(context.Message.CategoryId, productions);
            }
            catch (Exception ex)
            {
                Logger.LogError($"Second Layer Error At: {DateTime.Now.ToString()}!, " +
                    $"Message: {ex.Message.ToString()}, " +
                    $"Category Url : {context.Message.CategoryUrl}," +
                    $"Page:{page} , " +
                    $"CorrelationId:{context.ConversationId}");                
                throw new Exception();                
            }
            finally
            {
                Logger.LogInformation($"Second Layer Traverse Done At : {DateTime.Now.ToString()} , " +
                    $"CorrelationId:{context.ConversationId}");
                
                if(categories.Max(x => x.Order)== currentCategory.Order)//Last category 
                {
                    //await currentPage.Browser.CloseAsync();
                    KillChromeProcess(currentPage?.Browser?.Process?.Id);
                    var firstCategory = categories.OrderBy(x => x.Order).FirstOrDefault();
                    await context.Publish<ThirdLayer>(new ThirdLayer { CategoryId = firstCategory.Id , Order = firstCategory.Order , CategoryUrl = firstCategory.Url , CategoryName = firstCategory.Name}, default);
                }
                else if (currentCategory.Order != categories.Max(x => x.Order))//Continue from next category
                {
                    //await currentPage.Browser.CloseAsync();
                    KillChromeProcess(currentPage?.Browser?.Process?.Id);
                    var nextCategory = categories.Where(x => x.Order > currentCategory.Order).OrderBy(x => x.Order).FirstOrDefault();
                    await context.Publish<SecondLayer>(new SecondLayer { CategoryId = nextCategory.Id, CategoryUrl = nextCategory.Url, CategoryName = nextCategory.Name }, default);
                }
                
            }           
        }
        private async Task CreateOrUpdateAsync(Guid categoryId, IEnumerable<string> productUrlList)
        {
            var productions = (await ProductRepository.GetAllAsync(product => product.CategoryId == categoryId, null, null, true, false)).ToList();
            var newProductions = productUrlList.Where(product => !productions.Select(x => x.Url).Contains(product)).ToList();
            var removeProductions = productions.Where(product => !productUrlList.Contains(product.Url)).ToList();
            if (newProductions.Any())
                await ProductRepository.InsertAsync(newProductions.Select(x => new Production(x, "", "", categoryId, null, null, null, null)));
            if (removeProductions.Any())
                ProductRepository.Delete(removeProductions);
            await UnitOfWork.SaveChangesAsync();
        }

        private void KillChromeProcess(int? id)
        {
            var chromeProcess = Process.GetProcesses().FirstOrDefault(x => x.Id == id);
            if (chromeProcess != null)
            {
                chromeProcess.Kill();
            }
        }
    }
}