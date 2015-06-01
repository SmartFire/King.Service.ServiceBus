﻿namespace King.Service.ServiceBus
{
    using System;
    using King.Azure.Data;
    using King.Service.Data;

    public class AutoScaleConfiguration<T>
    {
        #region Members
        protected readonly string queueName;
        protected readonly string connectionString;
        protected readonly IProcessor<T> processor;
        protected readonly QueuePriority priority = QueuePriority.Low;
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
        public virtual IQueueCount QueueCount
        {
            get
            {
                return new BusQueueReciever(this.queueName, this.connectionString);
            }
        }
        public virtual ushort MessagesPerScaleUnit
        {
            get;
            set;
        }
        public virtual byte Minimum
        {
            get;
            set;
        }
        public virtual byte Maximum
        {
            get;
            set;
        }
        public virtual byte CheckScaleInMinutes
        {
            get;
            set;
        }

        public virtual Func<IScalable> Task
        {
            get
            {
                //This needs to be made dynamic, based on priority
                return () => { return new BackoffRunner(new BusDequeue<T>(new BusQueueReciever(this.queueName, this.connectionString), this.processor)); };
            }
        }

        public virtual IRunnable Runnable
        {
            get
            {
                return new TempAutoScaler<T>(this);
            }
        }
        #endregion
    }
}