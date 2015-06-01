namespace King.Service.ServiceBus
{
    using System;
    using King.Azure.Data;

    public class AutoScaleConfiguration<T>
    {
        public IQueueCount QueueCount
        {
            get;
            set;
        }
        public T Configuration
        {
            get;
            set;
        }
        public ushort MessagesPerScaleUnit
        {
            get;
            set;
        }
        public byte Minimum
        {
            get;
            set;
        }
        public byte Maximum
        {
            get;
            set;
        }
        public byte CheckScaleInMinutes
        {
            get;
            set;
        }

        public Func<IScalable> Task
        {
            get;
            set;
        }
    }
}