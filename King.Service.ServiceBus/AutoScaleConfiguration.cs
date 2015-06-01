namespace King.Service.ServiceBus
{
    using System;
    using King.Azure.Data;

    public class AutoScaleConfiguration
    {
        public virtual IQueueCount QueueCount
        {
            get;
            set;
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
            get;
            set;
        }

        public virtual IRunnable Run
        {
            get
            {
                return new AutoScaler(this);
            }
        }
    }
}