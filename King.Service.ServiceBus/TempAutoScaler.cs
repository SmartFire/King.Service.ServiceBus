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
        /// 
        /// </summary>
        /// <param name="config">Configuration</param>
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