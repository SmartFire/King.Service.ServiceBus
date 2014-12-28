﻿namespace King.Service.ServiceBus
{
    using King.Service.ServiceBus.Models;
    using King.Service.ServiceBus.Wrappers;
    using Microsoft.ServiceBus;
    using Microsoft.ServiceBus.Messaging;
    using Newtonsoft.Json;
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Threading.Tasks;

    /// <summary>
    /// Bus Queue Sender
    /// </summary>
    public class BusQueueSender : BusQueue, IBusQueueSender
    {
        #region Members
        /// <summary>
        /// Buffered Offset (Seconds)
        /// </summary>
        public const sbyte BufferedOffset = -6;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">Queue Name</param>
        /// <param name="connectionString">Connection String</param>
        public BusQueueSender(string name, string connectionString)
            : base(name, connectionString)
        {
        }

        /// <summary>
        /// Mockable Constructor
        /// </summary>
        /// <param name="name">Queue Name</param>
        /// <param name="manager">Manager</param>
        /// <param name="client"Client></param>
        public BusQueueSender(string name, NamespaceManager manager, IBusQueueClient client)
            : base(name, manager, client)
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// Send Message to Queue
        /// </summary>
        /// <param name="message">Message</param>
        /// <returns>Task</returns>
        public virtual async Task Send(BrokeredMessage message)
        {
            if (null == message)
            {
                throw new ArgumentNullException("message");
            }

            while (true)
            {
                try
                {
                    await this.client.Send(message);

                    break;
                }
                catch (MessagingException ex)
                {
                    if (ex.IsTransient)
                    {
                        this.HandleTransientError(ex);
                    }
                    else
                    {
                        Trace.TraceError("Error: '{0}'", ex.ToString());

                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// Send Object to queue, as json
        /// </summary>
        /// <param name="obj">object</param>
        /// <returns>Task</returns>
        public virtual async Task Send(object obj)
        {
            if (null == obj)
            {
                throw new ArgumentNullException("obj");
            }

            if (obj is BrokeredMessage)
            {
                await this.Send(obj as BrokeredMessage);
            }
            else
            {
                var msg = new BrokeredMessage(obj)
                {
                    ContentType = obj.GetType().ToString(),
                };

                await this.Send(msg);
            }
        }

        /// <summary>
        /// Send Message to Queue
        /// </summary>
        /// <param name="messages">Messages</param>
        /// <returns>Task</returns>
        public virtual async Task Send(IEnumerable<BrokeredMessage> messages)
        {
            if (null == messages)
            {
                throw new ArgumentNullException("message");
            }

            while (true)
            {
                try
                {
                    await this.client.Send(messages);

                    break;
                }
                catch (MessagingException ex)
                {
                    if (ex.IsTransient)
                    {
                        this.HandleTransientError(ex);
                    }
                    else
                    {
                        Trace.TraceError("Error: '{0}'", ex.ToString());

                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// Send Object to queue, as json
        /// </summary>
        /// <param name="messages">Messages</param>
        /// <returns>Task</returns>
        public virtual async Task Send(IEnumerable<object> messages)
        {
            if (null == messages)
            {
                throw new ArgumentNullException("obj");
            }

            if (messages is IEnumerable<BrokeredMessage>)
            {
                await this.Send(messages as IEnumerable<BrokeredMessage>);
            }
            else
            {
                await this.Send(messages.Select(obj => new BrokeredMessage(obj)
                {
                    ContentType = obj.GetType().ToString(),
                }));
            }
        }

        /// <summary>
        /// Send Message
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="enqueueAt">Schedule for Enqueue</param>
        /// <returns>Task</returns>
        public virtual async Task Send(object message, DateTime enqueueAt)
        {
            if (null == message)
            {
                throw new ArgumentNullException("message");
            }

            var msg = new BrokeredMessage(message)
            {
                ScheduledEnqueueTimeUtc = enqueueAt,
                ContentType = message.GetType().ToString(),
            };

            await this.Send(msg);
        }

        /// <summary>
        /// Send Message for Buffer
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="enqueueAt">Schedule for Enqueue</param>
        /// <param name="offset">Offset</param>
        /// <returns>Task</returns>
        public virtual async Task SendBuffered(object data, DateTime releaseAt, sbyte offset = BufferedOffset)
        {
            var message = new BufferedMessage
            {
                Data = null == data ? null : JsonConvert.SerializeObject(data),
                ReleaseAt = releaseAt,
            };

            await this.Send(message, releaseAt.AddSeconds(offset));
        }
        #endregion
    }
}