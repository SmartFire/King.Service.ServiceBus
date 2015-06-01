namespace King.Service.ServiceBus
{
    using System.Collections.Generic;
    using King.Service.Data;

    /// <summary>
    /// Auto Scaler
    /// </summary>
    /// <remarks>
    /// Change Name
    /// </remarks>
    public class AutoScaler : QueueAutoScaler<AutoScaleConfiguration>
    {
        #region Constructors
        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        public AutoScaler(AutoScaleConfiguration config)
            : base(config.QueueCount, config.MessagesPerScaleUnit, config, config.Minimum, config.Maximum, config.CheckScaleInMinutes)
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public override IEnumerable<IScalable> ScaleUnit(AutoScaleConfiguration data)
        {
            yield return data.Task();
        }
        #endregion
    }
}