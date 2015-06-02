﻿namespace King.Service.ServiceBus
{
    using System.Collections.Generic;
    using Azure.Data;
    using King.Service.Data;

    /// <summary>
    /// Auto Scaler
    /// </summary>
    public class TempAutoScaler : QueueAutoScaler<ITaskCreator>
    {
        #region Constructors
        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="count">Count</param>
        /// <param name="messagesPerScaleUnit">Messages Per-Scale Unit</param>
        /// <param name="task">Task</param>
        /// <param name="minimum">Minimum Scale</param>
        /// <param name="maximum">Maximmum Scale</param>
        /// <param name="checkScaleInMinutes">Check Scale Every</param>
        public TempAutoScaler(IQueueCount count, ushort messagesPerScaleUnit = 10, ITaskCreator task = null, byte minimum = 1, byte maximum = 2, byte checkScaleInMinutes = 2)
            : base(count, messagesPerScaleUnit, task, minimum, maximum, checkScaleInMinutes)
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// Scale Unit
        /// </summary>
        /// <param name="creator">Task Creator</param>
        /// <returns>Task(s)</returns>
        public override IEnumerable<IScalable> ScaleUnit(ITaskCreator creator)
        {
            yield return creator.Task();
        }
        #endregion
    }
}