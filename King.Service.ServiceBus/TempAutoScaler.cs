namespace King.Service.ServiceBus
{
    using System.Collections.Generic;
    using Azure.Data;
    using King.Service.Data;

    /// <summary>
    /// Auto Scaler
    /// </summary>
    /// <remarks>
    /// Change Name
    /// </remarks>
    public class TempAutoScaler : QueueAutoScaler<ITask>
    {
        #region Constructors
        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="count">Count</param>
        /// <param name="messagesPerScaleUnit">Messages Per-Scale Unit</param>
        /// <param name="configuration">Configuration</param>
        /// <param name="minimum">Minimum Scale</param>
        /// <param name="maximum">Maximmum Scale</param>
        /// <param name="checkScaleInMinutes">Check Scale Every</param>
        public TempAutoScaler(IQueueCount count, ushort messagesPerScaleUnit = 10, ITask task = null, byte minimum = 1, byte maximum = 2, byte checkScaleInMinutes = 2)
            : base(count, messagesPerScaleUnit, task, minimum, maximum, checkScaleInMinutes)
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// Scale Unit
        /// </summary>
        /// <param name="data">Data/Configuration</param>
        /// <returns>Task(s)</returns>
        public override IEnumerable<IScalable> ScaleUnit(ITask data)
        {
            yield return data.Task();
        }
        #endregion
    }
}