namespace King.Service.ServiceBus
{
    using System.Collections.Generic;
    using King.Service;

    public class MyFactory : ITaskFactory<object>
    {
        public IEnumerable<IRunnable> Tasks(object config)
        {
            //I am also thinking it might be nice to be fluent
            //yield return new AutoScaleConfiguration()
            //    .Queue("name")
            //    .Connection("creds")
            //    .Priority(High)
            //    .Processor(new MyProcessor());

            var x = new AutoScaleConfiguration
            {
                //Task is OK
                //I would like to give the processing method & Priority
                //Priority = Medium/Low/High
                //Processor = IProcessor.X
				//Then Get rid of these:
                QueueCount = new BusQueueReciever("name", "connection"),
                Task = () => { return new BackoffRunner(new BusDequeue<object>(new BusQueueReciever("name", "connection"), new object())); },
            };

            yield return x.Run;

   //         yield return new AutoScaleConfiguration()
   //             .Queue("name")
   //             .Connection("creds")
   //             .Priority(High)
   //             .Processor(new MyProcessor());

   //         var x = new AutoScaleConfiguration("name", "creds")
   //         {
   //             Priority = High,
   //             Processor = new MyProcessor();
			//};

			//yield return x.Run;
        }
    }
}