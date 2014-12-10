﻿namespace King.Service.ServiceBus.Unit.Tests.Wrappers
{
    using King.Service.ServiceBus.Models;
    using King.Service.ServiceBus.Timing;
    using King.Service.ServiceBus.Wrappers;
    using Newtonsoft.Json;
    using NSubstitute;
    using NUnit.Framework;
    using System;
    using System.Threading.Tasks;

    [TestFixture]
    public class BufferedMessageEventHandlerTests
    {
        [Test]
        public void Constructor()
        {
            var handler = Substitute.For<IBusEventHandler<object>>();
            var sleep = Substitute.For<ISleep>();
            new BufferedMessageEventHandler<object>(handler, sleep);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorHandlerNull()
        {
            var sleep = Substitute.For<ISleep>();
            new BufferedMessageEventHandler<object>(null, sleep);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorSleepNull()
        {
            var handler = Substitute.For<IBusEventHandler<object>>();
            new BufferedMessageEventHandler<object>(handler, null);
        }

        [Test]
        public void IsIBusEventHandlerBufferedMessage()
        {
            var handler = Substitute.For<IBusEventHandler<object>>();
            var sleep = Substitute.For<ISleep>();
            Assert.IsNotNull(new BufferedMessageEventHandler<object>(handler, sleep) as IBusEventHandler<BufferedMessage>);
        }

        [Test]
        public void OnError()
        {
            var action = Guid.NewGuid().ToString();
            var ex = new Exception();

            var handler = Substitute.For<IBusEventHandler<object>>();
            handler.OnError(action, ex);
            var sleep = Substitute.For<ISleep>();

            var h = new BufferedMessageEventHandler<object>(handler, sleep);
            h.OnError(action, ex);

            handler.Received().OnError(action, ex);
        }

        [Test]
        public async Task Process()
        {
            var data = Guid.NewGuid();
            var msg = new BufferedMessage()
            {
                ReleaseAt = DateTime.UtcNow,
                Data = JsonConvert.SerializeObject(data),
            };

            var handler = Substitute.For<IBusEventHandler<Guid>>();
            handler.Process(data).Returns(Task.FromResult(true));
            var sleep = Substitute.For<ISleep>();
            sleep.Until(msg.ReleaseAt);

            var h = new BufferedMessageEventHandler<Guid>(handler, sleep);
            var s = await h.Process(msg);

            Assert.IsTrue(s);

            sleep.Received().Until(msg.ReleaseAt);
            handler.Received().Process(data);
        }

        [Test]
        public async Task ProcessDataNull()
        {
            var data = Guid.Empty;
            var msg = new BufferedMessage()
            {
                ReleaseAt = DateTime.UtcNow,
                Data = null,
            };

            var handler = Substitute.For<IBusEventHandler<Guid>>();
            handler.Process(data).Returns(Task.FromResult(true));
            var sleep = Substitute.For<ISleep>();
            sleep.Until(msg.ReleaseAt);

            var h = new BufferedMessageEventHandler<Guid>(handler, sleep);
            var s = await h.Process(msg);

            Assert.IsTrue(s);

            sleep.Received().Until(msg.ReleaseAt);
            handler.Received().Process(data);
        }
    }
}