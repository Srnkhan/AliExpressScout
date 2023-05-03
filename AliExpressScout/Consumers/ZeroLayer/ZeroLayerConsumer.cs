namespace AliExpressScout.Consumers.ZeroLayer
{
    using System.Threading.Tasks;
    using MassTransit;
    using Contracts;
    using Microsoft.Extensions.Logging;
    using Arch.EntityFrameworkCore.UnitOfWork;
    using Domain.Entities.Categories;
    using PuppeteerSharp;
    using Application.Abstraction;
    using System;
    using System.Collections.Generic;
    using Newtonsoft.Json;
    using System.Linq;
    using System.Diagnostics;

    public class ZeroLayerConsumer :
        IConsumer<ZeroLayer>
    {
        private readonly ILogger<ZeroLayerConsumer> Logger;
        private readonly IUnitOfWork UnitOfWork;
        private readonly IRepository<Category> CategoryRepository;
        private readonly IAliExpressScoutFactory AEScoutFactory;

        public ZeroLayerConsumer(ILogger<ZeroLayerConsumer> logger,
            IUnitOfWork unitOfWork,
            IAliExpressScoutFactory aEScoutFactory)
        {
            Logger = logger;
            UnitOfWork = unitOfWork;
            AEScoutFactory = aEScoutFactory;
            CategoryRepository = UnitOfWork.GetRepository<Category>();
        }

        public async Task Consume(ConsumeContext<ZeroLayer> context)
        {
            await Task.Delay(10);
            //KillChromeProcesses();
            using var currentPage = await AEScoutFactory.ZeroLayerService.CreatePageAsync();

            try
            {
                Logger.LogInformation($"Zero Layer Started At : {DateTime.Now.ToString()}, " +
                    $"CorrelationId:{context.ConversationId}");

                var categoryUrlResponse = await AEScoutFactory.ZeroLayerService.GetCategoryUrlsAsync(currentPage);
                var categoryNamesResponse = await AEScoutFactory.ZeroLayerService.GetCategoryNamesAsync(currentPage);
                var urlList = JsonConvert.DeserializeObject<List<string>>(categoryUrlResponse.ResultObject.ToString());
                var nameList = JsonConvert.DeserializeObject<List<string>>(categoryNamesResponse.ResultObject.ToString());
                int index = 0;
                var categories = urlList.Select(url =>
                {
                    string name = nameList[index].ToString();
                    ++index;
                    return new Category(name, url,index);
                }).ToList();
                index = 0;

                await CreateOrUpdateAsync(categories);                               
            }
            catch (Exception ex)
            {
                Logger.LogError($"Zero Layer Error At: {DateTime.Now.ToString()}!, " +
                    $"Message: {ex.Message.ToString()}," +
                    $"CorrelationId:{context.ConversationId}");                
                throw new Exception(ex.Message);
            }
            finally
            {
                Logger.LogInformation($"Zero Layer Done At : {DateTime.Now.ToString()}, " +
                    $"CorrelationId:{context.ConversationId}");
                KillChromeProcess(currentPage?.Browser?.Process?.Id);
                await context.Publish<FirstLayer>(new FirstLayer());

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
        private async Task CreateOrUpdateAsync(IEnumerable<Category> categories)
        {

            var allCategories = (await CategoryRepository.GetAllAsync()).ToList();
            var newCategories = categories.Where(category => !allCategories.Select(x => x.Name).Contains(category.Name)).ToList();
            var oldCategories = allCategories.Where(category => !categories.Select(x => x.Name).Contains(category.Name)).ToList();
            Logger.LogInformation($"Zero Layer At : {DateTime.Now.ToString()} New Categories : {JsonConvert.SerializeObject(newCategories)}");
            Logger.LogInformation($"Zero Layer At : {DateTime.Now.ToString()} Old Categories : {JsonConvert.SerializeObject(oldCategories)}");

            if (newCategories.Any())
                await CategoryRepository.InsertAsync(newCategories);

            if (oldCategories.Any())
            {
                foreach (var category in oldCategories)
                {
                    CategoryRepository.Delete(category);
                }
            }

            if (newCategories.Any() || oldCategories.Any())
                await UnitOfWork.SaveChangesAsync();
        }
    }
}