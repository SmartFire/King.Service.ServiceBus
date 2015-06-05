﻿namespace King.Service.ServiceBus.Unit.Tests
{
    using System;
    using System.Linq;
    using King.Azure.Data;
    using King.Service.Data;
    using King.Service.Data.Model;
    using King.Service.Scalability;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class BusQueueAutoScalerTests
    {
        const string ConnectionString = "Endpoint=sb://test.servicebus.windows.net;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=[your secret]";

        [Test]
        public void Constructor()
        {
            var count = Substitute.For<IQueueCount>();
            var connection = Substitute.For<IQueueConnection<object>>();
            new BusQueueAutoScaler<object>(count, connection);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorThroughputNull()
        {
            var count = Substitute.For<IQueueCount>();
            var connection = Substitute.For<IQueueConnection<object>>();
            new BusQueueAutoScaler<object>(count, connection, null);
        }

        [Test]
        public void IsQueueAutoScaler()
        {
            var count = Substitute.For<IQueueCount>();
            var connection = Substitute.For<IQueueConnection<object>>();
            Assert.IsNotNull(new BusQueueAutoScaler<object>(count, connection) as QueueAutoScaler<IQueueConnection<object>>);
        }

        [Test]
        public void Runs()
        {
            var random = new Random();
            var max = (byte)random.Next(byte.MinValue, byte.MaxValue);
            var min = (byte)random.Next(byte.MinValue, max);
            var count = Substitute.For<IQueueCount>();
            var setup = new QueueSetup<object>
            {
                Name = "test",
                Priority = QueuePriority.Low,
                Processor = () => { return Substitute.For<IProcessor<object>>(); },
            };

            var connection = new QueueConnection<object>()
            {
                ConnectionString = ConnectionString,
                Setup = setup,
            };

            var throughput = Substitute.For<IQueueThroughput>();
            throughput.MinimumFrequency(setup.Priority).Returns(min);
            throughput.MaximumFrequency(setup.Priority).Returns(max);

            var s = new BusQueueAutoScaler<object>(count, connection, throughput);
            var runs = s.Runs(connection);

            Assert.IsNotNull(runs);
            Assert.AreEqual(min, runs.MinimumPeriodInSeconds);
            Assert.AreEqual(max, runs.MaximumPeriodInSeconds);

            throughput.Received().MinimumFrequency(setup.Priority);
            throughput.Received().MaximumFrequency(setup.Priority);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RunsSetupNull()
        {
            var count = Substitute.For<IQueueCount>();
            var connection = Substitute.For<IQueueConnection<object>>();

            var s = new BusQueueAutoScaler<object>(count, connection);
            s.Runs(null);
        }

        [Test]
        public void ScaleUnit()
        {
            var count = Substitute.For<IQueueCount>();
            var setup = Substitute.For<IQueueSetup<object>>();
            setup.Name.Returns(Guid.NewGuid().ToString());
            
            var connection = new QueueConnection<object>()
            {
                Setup = setup,
                ConnectionString = ConnectionString,
            };

            var s = new BusQueueAutoScaler<object>(count, connection);
            var unit = s.ScaleUnit(connection);

            Assert.IsNotNull(unit);
            Assert.AreEqual(1, unit.Count());
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ScaleUnitSetupNull()
        {
            var count = Substitute.For<IQueueCount>();
            var connection = Substitute.For<IQueueConnection<object>>();

            var s = new BusQueueAutoScaler<object>(count, connection);
            var unit = s.ScaleUnit(null);

            Assert.IsNotNull(unit);
            Assert.AreEqual(1, unit.Count());
        }
    }
}