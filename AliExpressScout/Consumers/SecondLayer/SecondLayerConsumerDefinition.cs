namespace AliExpressScout.Consumers.SecondLayer
{
    using MassTransit;

    public class SecondLayerConsumerDefinition :
        ConsumerDefinition<SecondLayerConsumer>
    {
        protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<SecondLayerConsumer> consumerConfigurator)
        {
            endpointConfigurator.UseMessageRetry(r => r.Intervals(500, 1000));
        }
    }
}