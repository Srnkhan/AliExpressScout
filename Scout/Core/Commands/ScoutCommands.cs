using PuppeteerSharp;
using Scout.Domain;
using Scout.Queries;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Scout.Core.Commands
{
    public class ScoutCommands
    {
        internal IPage Page;       
        public ScoutCommands(IPage page)
        {
            Page = page;
        }
        public string GetCurrentUrlCommand() 
        {
            return Page.Url;
        }
        public async Task<CommandResult> CheckComponentAsync(CheckQuery query)
        {
            try
            {
                var pageHeaderHandle = await Page.QuerySelectorAsync(query.Selector);
                return pageHeaderHandle != null ?
                    new CommandResult(null, true, "Success", query.Operation) :
                    new CommandResult(null, false, "Fail", query.Operation);
            }
            catch (Exception ex)
            {
                return new CommandResult(ex, false, ex.Message, query.Operation);
            }
        }
        public async Task<CommandResult> ClickCommandAsync(ClickQuery query)
        {
            
            try
            {
                await Page.WaitForSelectorAsync(query.WaitForSelector);
                var component = await Page.WaitForSelectorAsync(query.Selector);
                await component.ClickAsync();
                return new CommandResult(null, true, "Success" , query.Operation);
            }
            catch (Exception ex)
            {
                return new CommandResult(ex, false, ex.Message, query.Operation);
            }       
        }
        public async Task<CommandResult> GoBackCommandAsync(GoBackQuery query)
        {
            try
            {
                await Page.GoBackAsync();
                return new CommandResult(null, true, "Success", query.Operation);
            }
            catch (Exception ex)
            {
                return new CommandResult(ex, false, ex.Message, query.Operation);
            }
        }
        public async Task<CommandResult> ScrollDownCommandAsync(ScrollQuery query)
        {
            try
            {
                if (query.ScrollHeight <= 0)
                {
                    await Page.EvaluateExpressionAsync("window.scrollBy(0, window.innerHeight)"); 
                }
                else
                {
                    await Page.EvaluateExpressionAsync($"window.scrollBy(0, {query.ScrollHeight})");
                }
                return new CommandResult(null, true, "Success", query.Operation);
            }
            catch (Exception ex)
            {
                return new CommandResult(ex, false, ex.Message, query.Operation);
            }
        }
        public async Task<CommandResult> GetInnerTextCommandAsync(InnerTextQuery query)
        {
            //var selector = await Page.WaitForSelectorAsync(Selector);//Maybe later

            try
            {
                var pageHeaderHandle = await Page.QuerySelectorAsync(query.Selector);
                var innerTextHandle = await pageHeaderHandle.GetPropertyAsync("innerText");
                var innerText = (await innerTextHandle.JsonValueAsync()).ToString();
                var text = innerText ?? string.Empty;
                return new CommandResult(text, true, text, query.Operation);
            }
            catch (Exception ex)
            {
                return new CommandResult(ex, false, ex.Message, query.Operation);
            }
        }
        public async Task<CommandResult> QueryCommandAsync(JsQuery query)
        {
            try
            {
                var queryResult = await Page.EvaluateExpressionAsync(query.Query);
                if (!string.IsNullOrWhiteSpace(query.WaitForSelector))
                {
                    await Page.WaitForSelectorAsync(query.WaitForSelector);
                }
                return queryResult != null ?
                    new CommandResult(queryResult, true, "Success", query.Operation) :
                    new CommandResult(null, false, "Fail", query.Operation);
            }
            catch (Exception ex)
            {

                return new CommandResult(ex, false, "Fail", query.Operation);
            }
        }
        public async Task<CommandResult> GoUrlCommandAsync(GoUrlQuery query)
        {
            var queryResult = await Page.EvaluateExpressionAsync(query.Url);
            if (!string.IsNullOrWhiteSpace(query.WaitForSelector))
            {
                await Page.WaitForSelectorAsync(query.WaitForSelector);
            }
            return queryResult != null ?
                new CommandResult(queryResult, true, "Success", query.Operation) :
                new CommandResult(null, false, "Fail", query.Operation);
        }
    }
}
