namespace AliExpressScout.Consumers.ThirdLayer
{
    using MassTransit;

    public class ThirdLayerConsumerDefinition :
        ConsumerDefinition<ThirdLayerConsumer>
    {
        protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<ThirdLayerConsumer> consumerConfigurator)
        {
            endpointConfigurator.UseMessageRetry(r => r.Intervals(500, 1000));
        }
    }
}