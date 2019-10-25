namespace Alpha.Logging.LogEngines
{
    using Alpha.Logging.Configurations;
    using Fanex.Logging;
    using Fanex.Logging.RabbitMQ;

    public class RabbitLogEngine : LogEngineFactory
    {
        public ILogEngine Create(string ravenScriptPath = null)
            => new RabbitMQLogging(AppSettings.RabbitMQConnectionStrings);
    }
}
