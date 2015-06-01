namespace King.Service.ServiceBus
{
    using System.Collections.Generic;
    using King.Service.Data;

    public class AutoScaler : QueueAutoScaler<AutoScaleConfiguration>
    {
        public AutoScaler(AutoScaleConfiguration config)
            : base(config.QueueCount, config.MessagesPerScaleUnit, config, config.Minimum, config.Maximum, config.CheckScaleInMinutes)
        {
        }

        public override IEnumerable<IScalable> ScaleUnit(AutoScaleConfiguration data)
        {
            yield return data.Task();
        }
    }
}