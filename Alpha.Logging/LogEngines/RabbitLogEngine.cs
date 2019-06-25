namespace Alpha.Logging.LogEngines
{
    using Alpha.Logging.Configurations;
    using Fanex.Logging;
    using Fanex.Logging.RabbitMQ;

    public class RabbitLogEngine : LogEngineFactory
    {
        public ILogEngine Create()
            => new RabbitMQLogging(AppSettings.RabbitMQConnectionStrings);
    }
}
