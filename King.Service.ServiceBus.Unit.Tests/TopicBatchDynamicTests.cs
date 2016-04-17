﻿namespace King.Service.ServiceBus.Unit.Tests
{
    using System;
    using King.Azure.Data;
    using King.Service.Data;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class TopicBatchDynamicTests
    {
        const string ConnectionString = "Endpoint=sb://test.servicebus.windows.net;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=[your secret]";

        [Test]
        public void Constructor()
        {
            var name = Guid.NewGuid().ToString();
            var subscription = Guid.NewGuid().ToString();
            var processor = Substitute.For<IProcessor<object>>();
            new TopicBatchDynamic<object>(name, ConnectionString, subscription, processor);
        }

        [Test]
        public void IsDequeueBatchDynamic()
        {
            var name = Guid.NewGuid().ToString();
            var subscription = Guid.NewGuid().ToString();
            var processor = Substitute.For<IProcessor<object>>();
            Assert.IsNotNull(new TopicBatchDynamic<object>(name, ConnectionString, subscription, processor) as DequeueBatchDynamic<object>);
        }
    }
}