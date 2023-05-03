using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scout.Core.Builders
{
    public sealed class ScoutBuilderDirector : ScoutPageBuilder<ScoutBuilderDirector>
    {
        public static ScoutBuilderDirector NewPage = new ScoutBuilderDirector();
    }
}
