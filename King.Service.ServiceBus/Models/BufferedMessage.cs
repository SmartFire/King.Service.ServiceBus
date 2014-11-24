﻿namespace King.Service.ServiceBus.Unit.Tests.Models
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Buffered Message
    /// </summary>
    public class BufferedMessage : IBufferedMessage
    {
        #region Properties
        /// <summary>
        /// Data
        /// </summary>
        public virtual object Data
        {
            get;
            set;
        }

        /// <summary>
        /// Release At
        /// </summary>
        public virtual DateTime ReleaseAt
        {
            get;
            set;
        }
        #endregion
    }
}