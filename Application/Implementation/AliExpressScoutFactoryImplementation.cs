using Application.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Implementation
{
    public class AliExpressScoutFactoryImplementation : IAliExpressScoutFactory
    {
        public IZeroLayerService ZeroLayerService { get ; set ; }
        public ISecondLayerService SecondLayerService { get ; set ; }
        public IThirdLayerService ThirdLayerService { get; set; }

        public AliExpressScoutFactoryImplementation(IZeroLayerService zeroLayerService, 
            ISecondLayerService secondLayerService, 
            IThirdLayerService thirdLayerService)
        {
            ZeroLayerService = zeroLayerService;
            SecondLayerService = secondLayerService;
            ThirdLayerService = thirdLayerService;
        }
    }
}
