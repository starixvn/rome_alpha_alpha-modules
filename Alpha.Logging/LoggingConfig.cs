namespace Alpha.Logging
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Alpha.Logging.Configurations;
    using Alpha.Logging.LogEngines;
    using Fanex.Logging;
    using Fanex.Logging.Extensions;
    using Fanex.Logging.Extensions.Web;

    public static class LoggingConfig
    {
        public static void Init(IEnumerable<string> sensitiveInfo = null, Func<Exception, IDictionary<string, object>> getDbExceptionInfo = null)
        {
            var libs = AppSettings.LogLibs.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            var logEngines = libs.Select(CreateLogLib).Where(engine => engine != null);

            LogManager.SetDefaultLogCategory(AppSettings.LoggingDefaultCategory).Use(logEngines);

            var regexSensitiveInfo = GetRegexToHideSensitiveInfo(sensitiveInfo ?? Enumerable.Empty<string>());
            var loggingConfiguration = new LoggingConfiguration().GetHttpHeaderInfo().HideSensitiveInfo(regexSensitiveInfo);
            if (getDbExceptionInfo != null)
            {
                loggingConfiguration.UseGetDbExceptionInfo(getDbExceptionInfo);
            }

            ExceptionHandler.UseConfig(loggingConfiguration);
        }

        private static List<string> GetRegexToHideSensitiveInfo(IEnumerable<string> sensitiveInfo)
        {
            var result = new List<string>();

            foreach (var info in sensitiveInfo)
            {
                result.Add($"(?:{info}=)(?<paramValue>[^&|^\\s]+)");
                result.Add($"(\"{info}\":)(?<paramValue>[^,|^\r]+)");
            }

            return result;
        }

        private static ILogEngine CreateLogLib(string libName)
        {
            var type = Type.GetType($"Alpha.Logging.LogEngines.{libName}LogEngine");
            if (type == null)
            {
                return null;
            }

            var engine = Activator.CreateInstance(type) as LogEngineFactory;

            return engine?.Create();
        }
    }
}
