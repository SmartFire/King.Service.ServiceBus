namespace King.Service.ServiceBus
{
    using System.Collections.Generic;
    using King.Service;

    public class MyFactory : ITaskFactory<object>
    {
        public IEnumerable<IRunnable> Tasks(object config)
        {
            var x = new AutoScaleConfiguration<object>
            {
                QueueCount = new BusQueueReciever("name", "connection"),
                Configuration = config,
                Task = () => { return new BackoffRunner(new BusDequeue<object>(new BusQueueReciever("name", "connection"), new object())); },
            };
		
            yield return new AutoScaler<object>(x);
        }
    }
}