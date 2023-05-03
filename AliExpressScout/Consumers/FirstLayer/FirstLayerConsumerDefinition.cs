namespace AliExpressScout.Consumers.FirstLayer
{
    using MassTransit;

    public class FirstLayerConsumerDefinition :
        ConsumerDefinition<FirstLayerConsumer>
    {
        protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<FirstLayerConsumer> consumerConfigurator)
        {
            endpointConfigurator.UseMessageRetry(r => r.Intervals(500, 1000));
        }
    }
}