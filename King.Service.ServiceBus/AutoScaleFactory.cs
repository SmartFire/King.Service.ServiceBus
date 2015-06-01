namespace King.Service.ServiceBus
{
    using King.Azure.Data;
    using King.Service.Data;
    using King.Service.ServiceBus;
    using System.Collections.Generic;

    public class AutoScaleFactory
    {
        public QueueAutoScaler<T> Get<T>(AutoScaleConfiguration<T> config)
        {
            return new QueueAutoScaler<T>(config.QueueCount, config.MessagesPerScaleUnit, config.Configuration, config.Minimum, config.Maximum);
        }
    }
}