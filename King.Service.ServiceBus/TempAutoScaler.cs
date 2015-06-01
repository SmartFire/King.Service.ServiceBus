﻿namespace King.Service.ServiceBus
{
    using System.Collections.Generic;
    using King.Service.Data;

    /// <summary>
    /// Auto Scaler
    /// </summary>
    /// <remarks>
    /// Change Name
    /// </remarks>
    public class TempAutoScaler<T> : QueueAutoScaler<AutoScaleConfiguration<T>>
    {
        #region Constructors
        /// <summary>
        /// 
        /// </summary>
        /// <param name="config">Configuration</param>
        public TempAutoScaler(AutoScaleConfiguration<T> config)
            : base(config.QueueCount, config.MessagesPerScaleUnit, config, config.Minimum, config.Maximum, config.CheckScaleInMinutes)
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// Scale Unit
        /// </summary>
        /// <param name="data">Data/Configuration</param>
        /// <returns>Task(s)</returns>
        public override IEnumerable<IScalable> ScaleUnit(AutoScaleConfiguration<T> data)
        {
            yield return data.Task();
        }
        #endregion
    }
}