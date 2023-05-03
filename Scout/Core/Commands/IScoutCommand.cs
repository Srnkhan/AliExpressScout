using Scout.Domain;
using Scout.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scout.Core.Commands
{
    public interface IScoutCommand
    {
        Task<CommandResult> ExecuteAsync();
    }
}
