﻿namespace King.Service.ServiceBus
{
    using King.Azure.Data;
    using King.Service.Data;
    using King.Service.ServiceBus;
    using System.Collections.Generic;
    using System;

    public class AutoScaleFactory
    {
        public QueueAutoScaler<T> Get<T>(AutoScaleConfiguration<T> config)
        {
            var boom = new AutoScaler<T>(config);
            return new boom;
        }
    }

    public class AutoScaler<T> : QueueAutoScaler<T>
    {
        AutoScaleConfiguration<T> config;
        public AutoScaler(AutoScaleConfiguration<T> config)
            :base(config.QueueCount, config.MessagesPerScaleUnit, config.Configuration, config.Minimum, config.Maximum, config.CheckScaleInMinutes)
        { }
        public override IEnumerable<IScalable> ScaleUnit(T data)
        {
            yield return config.Task;//THIS NEEDS TO BE CLONED!
        }
    }

}