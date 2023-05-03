using Application.Abstraction;
using Domain.Entities.Productions;
using PuppeteerSharp;
using Scout.Core.Builders;
using Scout.Core.Commands;
using Scout.Domain;
using Scout.Queries;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Implementation
{
    public class ThirdLayerServiceImplementation : IThirdLayerService
    {
        public async Task<IPage> CreatePageAsync(string url = "https://www.google.com/")
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

        public async Task<Production> SetNormalPriceInformationsAsync(IPage currentPage ,  Production production)
        {
            double price = 0;
            var bannerPrice = await new ScoutCommand(CommandType.JsQuery,
                                                         new ScoutCommands(currentPage),
                                                         new JsQuery { Query = "document.getElementsByClassName('uniform-banner-box-price')[0].getInnerHTML()", WaitForSelector = ".site-logo" })
                                                         .ExecuteAsync();

            var normalPrice = await new ScoutCommand(CommandType.JsQuery,
                                                 new ScoutCommands(currentPage),
                                                 new JsQuery { Query = "document.getElementsByClassName('product-price-value')[0].getInnerHTML()", WaitForSelector = ".site-logo" })
                                                 .ExecuteAsync();

            if (bannerPrice.IsSuccess)
            {
                var splitPrice = bannerPrice.ResultObject.ToString().Split(" ");
                var currency = splitPrice[0];
                var amount = splitPrice[1];
                production.Currency = currency == "TRY" ? Currency.TL : currency == "TRY" ? Currency.USD : Currency.None;
                production.Price = amount;
            }
            else if (normalPrice.IsSuccess)
            {
                var splitPrice = normalPrice.ResultObject.ToString().Split(" ");
                var currency = splitPrice[0];
                var amount = splitPrice[1];
                production.Currency = currency == "TRY" ? Currency.TL : currency == "TRY" ? Currency.USD : Currency.None;
                production.Price = amount;
            }
            return production;
        }

        public async Task<Production> SetOriginalPriceInformationsAsync(IPage currentPage, Production production)
        {
            var normalPrice = await new ScoutCommand(CommandType.JsQuery,
                                                new ScoutCommands(currentPage),
                                                new JsQuery { Query = "document.getElementsByClassName('product-price-value')[1].getInnerHTML()", WaitForSelector = ".site-logo" })
                                                .ExecuteAsync();
            var bannerPrice = await new ScoutCommand(CommandType.JsQuery,
                                                new ScoutCommands(currentPage),
                                                new JsQuery { Query = "document.querySelector(\"#root > div > div.product-main > div > div.product-info > div.uniform-banner > div.uniform-banner-box > div:nth-child(1) > span.uniform-banner-box-discounts > span:nth-child(1)\").innerHTML", WaitForSelector = ".site-logo" })
                                                .ExecuteAsync();
            if (normalPrice.IsSuccess)
            {
                var splitPrice = bannerPrice.ResultObject.ToString().Split(" ");
                var currency = splitPrice[0];
                var amount = splitPrice[1];
                production.Currency = currency == "TRY" ? Currency.TL : currency == "TRY" ? Currency.USD : Currency.None;
                production.DiscoutLessPrice = amount;
            }
            else if (bannerPrice.IsSuccess)
            {
                var splitPrice = bannerPrice.ResultObject.ToString().Split(" ");
                var currency = splitPrice[0];
                var amount = splitPrice[1];
                production.Currency = currency == "TRY" ? Currency.TL : currency == "TRY" ? Currency.USD : Currency.None;
                production.DiscoutLessPrice = amount;
            }
            return production;
        }
    }
}
