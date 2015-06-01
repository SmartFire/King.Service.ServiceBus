namespace King.Service.ServiceBus
{
    using System.Collections.Generic;
    using King.Service;

    public class MyFactory : ITaskFactory<object>
    {
        public IEnumerable<IRunnable> Tasks(object config)
        {
            var x = new AutoScaleConfiguration
            {
                QueueCount = new BusQueueReciever("name", "connection"),
                Task = () => { return new BackoffRunner(new BusDequeue<object>(new BusQueueReciever("name", "connection"), new object())); },
            };

            yield return x.Run;
        }
    }
}