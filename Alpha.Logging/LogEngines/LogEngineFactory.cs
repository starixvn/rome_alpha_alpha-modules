namespace Alpha.Logging.LogEngines
{
    using Fanex.Logging;

    public interface LogEngineFactory
    {
        ILogEngine Create(string ravenScriptPath = null);
    }
}
