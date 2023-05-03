using PuppeteerSharp;
using Scout.Domain;
using Scout.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scout.Core.Commands
{
    public class ScoutCommand : IScoutCommand
    {
        private CommandType Command;
        private ScoutCommands Commands;
        private CommandInformation Information;
        private CommonQuery CommonQuery;
        private Func<IPage , Task> BeforeExecute;

        public ScoutCommand(CommandType type,
            ScoutCommands commands,
            CommonQuery commonQuery,
            Func<IPage ,Task> beforeExecute = null)
        {
            Command = type;
            Commands = commands;
            Information = new CommandInformation();
            CommonQuery = commonQuery;
            BeforeExecute = beforeExecute;
        }
        async Task BeforeExecuteAsync()
        {
            if(BeforeExecute != null)
                await BeforeExecute(Commands.Page);
        }

        public async Task<CommandResult> ExecuteAsync()
        {
            await BeforeExecuteAsync();
            return await ExecuteCoreAsync();
        }
        private async Task<CommandResult> ExecuteCoreAsync()
        {
            var result = new CommandResult();

            Information.SetBeforeUrl(Commands.GetCurrentUrlCommand());

            if (Command == CommandType.GetInformation)
            {
                result = new CommandResult(Information);
            }
            else if (Command == CommandType.Click)
            {
                var query = (ClickQuery)CommonQuery;
                result = await Commands.ClickCommandAsync(query);
            }
            else if (Command == CommandType.GoBack)
            {
                var query = (GoBackQuery)CommonQuery;
                result = await Commands.GoBackCommandAsync(query);
            }
            else if (Command == CommandType.ScrolDown)
            {
                var query = (ScrollQuery)CommonQuery;
                result = await Commands.ScrollDownCommandAsync(query);
            }
            else if (Command == CommandType.CheckComponent)
            {
                var query = (CheckQuery)CommonQuery;
                result = await Commands.CheckComponentAsync(query);
            }
            else if (Command == CommandType.GetInnerText)
            {
                var query = (InnerTextQuery)CommonQuery;
                result = await Commands.GetInnerTextCommandAsync(query);
            }
            else if(Command == CommandType.JsQuery)
            {
                var query = (JsQuery)CommonQuery;
                result = await Commands.QueryCommandAsync(query);
            }
            else if(Command == CommandType.GoUrl)
            {
                var query = (GoUrlQuery)CommonQuery;
                result = await Commands.GoUrlCommandAsync(query);
            }
            Information.SetAfterUrl(Commands.GetCurrentUrlCommand());
            return result;
        }
    }
}
