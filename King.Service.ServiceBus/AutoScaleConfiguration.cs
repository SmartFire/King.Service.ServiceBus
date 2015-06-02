﻿namespace King.Service.ServiceBus
{
    using System;
    using King.Azure.Data;
    using King.Service.Data;
    using Scalability;

    public class AutoScaleConfiguration<T> : ITaskCreator
    {
        #region Members
        protected readonly string queueName;
        protected readonly string connectionString;
        protected readonly IProcessor<T> processor;
        protected readonly QueuePriority priority = QueuePriority.Low;
        protected readonly IQueueThroughput throughput = new QueueThroughput();//Pass into constructor
        #endregion

        #region Constructors
        public AutoScaleConfiguration(string queueName, string connectionString, IProcessor<T> processor, QueuePriority priority = QueuePriority.Low)
        {
            this.queueName = queueName;
            this.connectionString = connectionString;
            this.processor = processor;
            this.priority = priority;
        }
        #endregion

        #region Properties
        public virtual Func<IScalable> Task
        {
            get
            {
                return () => {
                    var reciever = new BusQueueReciever(this.queueName, this.connectionString);
                    var runner = new BusDequeue<T>(reciever, this.processor);
                    return this.throughput.Runner(runner, this.priority);
                };
            }
        }
        #endregion

        #region Methods
        public virtual IRunnable Runnable()
        {
            var queueCount = new BusQueueReciever(this.queueName, this.connectionString);
            var messagesPerScaleUnit = this.throughput.MessagesPerScaleUnit(this.priority);
            var minimumScale = this.throughput.MinimumScale(this.priority);
            var maximumScale = this.throughput.MaximumScale(this.priority);
            var checkScaleEvery = this.throughput.CheckScaleEvery(this.priority);
            return new TempAutoScaler(queueCount, messagesPerScaleUnit, this, minimumScale, maximumScale, checkScaleEvery);
        }
        #endregion
    }

    public interface ITaskCreator
    {
        Func<IScalable> Task
        {
            get;
        }
    }
}