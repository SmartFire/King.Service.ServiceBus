namespace King.Service.ServiceBus
{
    using System.Collections.Generic;
    using King.Service.Data;

    public class AutoScaler<T> : QueueAutoScaler<T>
    {
        AutoScaleConfiguration<T> config;

        public AutoScaler(AutoScaleConfiguration<T> config)
            : base(config.QueueCount, config.MessagesPerScaleUnit, config.Configuration, config.Minimum, config.Maximum, config.CheckScaleInMinutes)
        {
        }

        public override IEnumerable<IScalable> ScaleUnit(T data)
        {
            yield return config.Task();
        }
    }

}