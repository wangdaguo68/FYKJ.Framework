using System.Linq;

namespace FYKJ.Module
{
    using System;
    using System.Collections.Generic;
    using System.Web;

    public class HttpModuleService : IHttpModule
    {
        private static Dictionary<string, ContextCollectHandler> handlers;
        private static bool isStarted;
        private static readonly object moduleStart = new object();

        private void context_BeginRequest(object sender, EventArgs e)
        {
            var current = HttpContext.Current;
            var currentExecutionFilePath = current.Request.CurrentExecutionFilePath;
            if (currentExecutionFilePath.EndsWith("collect.axd", StringComparison.OrdinalIgnoreCase))
            {
                currentExecutionFilePath = currentExecutionFilePath.Substring(currentExecutionFilePath.LastIndexOf("/", StringComparison.Ordinal) + 1);
                var key = currentExecutionFilePath.Substring(0, currentExecutionFilePath.LastIndexOf(".", StringComparison.Ordinal)).ToLower();
                if (handlers.ContainsKey(key))
                {
                    handlers[key].ProcessRequest(current);
                }
            }
        }

        public void Dispose()
        {
        }

        public void Init(HttpApplication context)
        {
            if (!isStarted)
            {
                lock (moduleStart)
                {
                    if (!isStarted)
                    {
                        isStarted = true;
                        InitHandlers();
                    }
                }
            }
            context.BeginRequest += new EventHandler(context_BeginRequest);
        }

        private void InitHandlers()
        {
            if (handlers == null)
            {
                handlers = new Dictionary<string, ContextCollectHandler>();
            }
            foreach (var type in from t in GetType().Assembly.GetTypes()
                where t.BaseType == typeof(ContextCollectHandler)
                select t)
            {
                if (!handlers.ContainsKey(type.Name))
                {
                    var handler = Activator.CreateInstance(type) as ContextCollectHandler;
                    if (handler != null)
                    {
                        handlers.Add(type.Name, handler);
                    }
                }
            }
        }
    }
}

