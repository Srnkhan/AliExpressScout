namespace AliExpressScout.Consumers.ZeroLayer
{
    using MassTransit;

    public class ZeroLayerConsumerDefinition :
        ConsumerDefinition<ZeroLayerConsumer>
    {
        protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<ZeroLayerConsumer> consumerConfigurator)
        {
            endpointConfigurator.UseMessageRetry(r => r.Intervals(500, 1000));
        }
    }
}