using Application.Abstraction;
using Arch.EntityFrameworkCore.UnitOfWork;
using Domain.Entities.Queries;
using Microsoft.Extensions.Configuration;
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
    public class ZeroLayerServiceImplementation : IZeroLayerService
    {
        private readonly IConfiguration Configuration;
        private readonly IUnitOfWork UnitOfWork;
        private readonly IRepository<Query> QueryRepository;

        public ZeroLayerServiceImplementation(IConfiguration configuration, IUnitOfWork unitOfWork)
        {
            Configuration = configuration;
            UnitOfWork = unitOfWork;
            QueryRepository = UnitOfWork.GetRepository<Query>();
        }

        public void Test()
        {
        }

        public async Task<IPage> CreatePageAsync()
        {
            var currentPage = await ScoutBuilderDirector
                               .NewPage
                               .Header(false)
                               .With(1920)
                               .Height(1080)
                               .Url("https://www.aliexpress.us/")
                               .Browser("970485")
                               .PrepareAsync();
            return currentPage;
        }

        public async Task<CommandResult> GetCategoryUrlsAsync(IPage CurrentPage)
        {
            var queries = await QueryRepository.GetAllAsync(x => x.Type == QueryType.CategoryUrlList,
                orderBy: null, 
                include: null,
                disableTracking: true,
                ignoreQueryFilters: true);

            foreach (var query in queries)
            {
                var result = await new ScoutCommand(CommandType.JsQuery,
                                                        new ScoutCommands(CurrentPage),
                                                        new JsQuery { Query = query.QueryText })
                                                        .ExecuteAsync();
                if (result.IsSuccess)
                    return result;

            }

            return CommandResult.Error();
        }
        public async Task<CommandResult> GetCategoryNamesAsync(IPage CurrentPage)
        {
            var queries = await QueryRepository.GetAllAsync(x => x.Type == QueryType.CategoryNameList,
                orderBy: null,
                include: null,
                disableTracking: true,
                ignoreQueryFilters: true);
            foreach (var query in queries)
            {
                var result = await new ScoutCommand(CommandType.JsQuery,
                                                       new ScoutCommands(CurrentPage),
                                                       new JsQuery { Query = query.QueryText })
                                                       .ExecuteAsync();
                if (result.IsSuccess)
                    return result;
            }
            return CommandResult.Error();
        }
    }
}
