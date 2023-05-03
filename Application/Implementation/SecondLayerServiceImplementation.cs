using Application.Abstraction;
using Arch.EntityFrameworkCore.UnitOfWork;
using Domain.Entities.Queries;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using PuppeteerSharp;
using Scout.Core.Builders;
using Scout.Core.Commands;
using Scout.Domain;
using Scout.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Implementation
{
    public class SecondLayerServiceImplementation : ISecondLayerService
    {
        private readonly IConfiguration Configuration;
        private readonly IUnitOfWork UnitOfWork;
        private readonly IRepository<Query> QueryRepository;

        public SecondLayerServiceImplementation(IConfiguration configuration, IUnitOfWork unitOfWork)
        {
            Configuration = configuration;
            UnitOfWork = unitOfWork;
            QueryRepository = UnitOfWork.GetRepository<Query>();
        }

        public async Task<IPage> CreatePageAsync(string url)
        {
            var page = await ScoutBuilderDirector
                                .NewPage
                                .Header(false)
                                .With(1920)
                                .Height(1080)
                                .Url(url)
                                .Browser("970485")
                                .PrepareAsync();
            return page;
        }


        public async Task<List<string>> TraversePageAsync(IPage currentPage)
        {
            int index = 0;
            var productions = new List<string>();
            while (index < 10)
            {
                await new ScoutCommand(CommandType.ScrolDown, new ScoutCommands(currentPage), new ScrollQuery { ScrollHeight = 1200 }).ExecuteAsync();
                var productionsResponse = await new ScoutCommand(CommandType.JsQuery,
                                                                       new ScoutCommands(currentPage),
                                                                    new JsQuery
                                                                    {
                                                                        Query = "var records = Array.from(document.getElementsByTagName('a')); records.filter(item => item.firstChild && item.firstChild.firstChild && item.firstChild.firstChild.tagName === 'IMG').map(item => {return item.href})",
                                                                        WaitForSelector = ".site-logo"
                                                                    })
                                                                       .ExecuteAsync();
                productions = JsonConvert.DeserializeObject<List<string>>(productionsResponse.ResultObject.ToString());
                productions = productions.Distinct().ToList();
                index++;
            }
            productions = productions.Distinct().ToList();
            return productions;
        }

        public async Task<CommandResult> ClickNextPageAsync(IPage CurrentPage)
        {
            var queries = await QueryRepository.GetAllAsync(x => x.Type == QueryType.ProductListNextPage,
                orderBy: null,
                include: null,
                disableTracking: true,
                ignoreQueryFilters: true);
            foreach (var query in queries)
            {
                var result = await new ScoutCommand(CommandType.JsQuery,
                                                           new ScoutCommands(CurrentPage),
                                                           new JsQuery { Query = query.QueryText, WaitForSelector = ".site-logo" })
                                                           .ExecuteAsync();
                if (result.IsSuccess)
                    return result;
            }
            return CommandResult.Error();
        }

        public async Task<CommandResult> CheckLastPageAsync(IPage CurrentPage)
        {
            var queries = await QueryRepository.GetAllAsync(x => x.Type == QueryType.ProductLastPage,
                orderBy: null,
                include: null,
                disableTracking: true,
                ignoreQueryFilters: true);
            foreach (var query in queries)
            {
                var result = await new ScoutCommand(CommandType.JsQuery,
                                                         new ScoutCommands(CurrentPage),
                                                         new JsQuery { Query = query.QueryText, WaitForSelector = ".site-logo" })
                                                         .ExecuteAsync();
                if (result.IsSuccess)
                    return result;
            }
            return CommandResult.Error();
        }
    }
}
