namespace King.Service.ServiceBus
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Azure.Data;
    using King.Service;

    public class MyFactory : ITaskFactory<object>
    {
        public class MyProcessor : IProcessor<object>
        {
            public Task<bool> Process(object data)
            {
                throw new NotImplementedException();
            }
        }

        public IEnumerable<IRunnable> Tasks(object config)
        {
            var x = new AutoScaleConfiguration<object>("name", "connection", new MyProcessor());
            yield return x.Runnable();
        }
    }
}