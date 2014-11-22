﻿namespace King.Service.ServiceBus.Unit.Tests
{
    using King.Azure.Data;
    using King.Service.Data;
    using King.Service.ServiceBus;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class BusDequeueBatchTests
    {
        [Test]
        public void Constructor()
        {
            var processor = Substitute.For<IProcessor<object>>();
            var queue = Substitute.For<IBusQueueReciever>();
            new BusDequeueBatch<object>(queue, processor);
        }

        [Test]
        public void IsDequeueBatch()
        {
            var processor = Substitute.For<IProcessor<object>>();
            var queue = Substitute.For<IBusQueueReciever>();
            Assert.IsNotNull(new BusDequeueBatch<object>(queue, processor) as DequeueBatch<object>);
        }
    }
}