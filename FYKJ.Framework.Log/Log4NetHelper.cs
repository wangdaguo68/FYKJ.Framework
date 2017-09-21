namespace FYKJ.Log
{
    using log4net;
    using log4net.Config;
    using Newtonsoft.Json;
    using System;
    using System.IO;
    using System.Text;
    using Config;

    public static class Log4NetHelper
    {
        static Log4NetHelper()
        {
            var s = CachedConfigContext.Current.ConfigService.GetConfig("log4net").Replace("{connectionString}", CachedConfigContext.Current.DaoConfig.Log);
            var configStream = new MemoryStream(Encoding.Default.GetBytes(s));
            XmlConfigurator.Configure(configStream);
        }

        public static void Debug(LoggerType loggerType, object message, Exception e)
        {
            LogManager.GetLogger(loggerType.ToString()).Debug(SerializeObject(message), e);
        }

        public static void Error(LoggerType loggerType, object message, Exception e)
        {
            LogManager.GetLogger(loggerType.ToString()).Error(SerializeObject(message), e);
        }

        public static void Fatal(LoggerType loggerType, object message, Exception e)
        {
            LogManager.GetLogger(loggerType.ToString()).Fatal(SerializeObject(message), e);
        }

        public static void Info(LoggerType loggerType, object message, Exception e)
        {
            LogManager.GetLogger(loggerType.ToString()).Info(SerializeObject(message), e);
        }

        private static object SerializeObject(object message)
        {
            if (message is string)
            {
                return message;
            }
            if (message == null)
            {
                return null;
            }
            var settings = new JsonSerializerSettings {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
            return JsonConvert.SerializeObject(message, settings);
        }

        public static void Warn(LoggerType loggerType, object message, Exception e)
        {
            LogManager.GetLogger(loggerType.ToString()).Warn(SerializeObject(message), e);
        }
    }
}

