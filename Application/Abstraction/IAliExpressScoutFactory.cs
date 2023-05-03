using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Abstraction
{
    public interface IAliExpressScoutFactory
    {
        IZeroLayerService ZeroLayerService { get; set; }
        ISecondLayerService SecondLayerService { get; set; }
        IThirdLayerService ThirdLayerService { get; set; }
    }
}
