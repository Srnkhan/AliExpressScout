using Domain.Entities.Productions;
using PuppeteerSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Abstraction
{
    public interface IThirdLayerService
    {
        Task<IPage> CreatePageAsync(string url = "https://www.google.com/");
        Task<Production> SetNormalPriceInformationsAsync(IPage currentPage, Production production);
        Task<Production> SetOriginalPriceInformationsAsync(IPage currentPage, Production production);
    }
}
