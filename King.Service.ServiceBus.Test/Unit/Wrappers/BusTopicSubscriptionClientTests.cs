﻿namespace King.Service.ServiceBus.Test.Unit.Wrappers
{
    using King.Service.ServiceBus.Wrappers;
    using Microsoft.ServiceBus.Messaging;
    using NUnit.Framework;
    using System;

    [TestFixture]
    public class BusTopicSubscriptionClientTests
    {
        const string connection = Configuration.ConnectionString;

        [Test]
        public void Constructor()
        {
            var name = Guid.NewGuid().ToString();
            var topicPath = Guid.NewGuid().ToString();
            new BusSubscriptionClient(SubscriptionClient.CreateFromConnectionString(connection, topicPath, name));
        }

        [Test]
        public void ConstructorQueueClientNull()
        {
            Assert.That(() => new BusSubscriptionClient(null), Throws.TypeOf<ArgumentNullException>());
        }
    }
}