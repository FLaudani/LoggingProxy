using System;
using log4net;

namespace LoggingProxy
{
    public class LoggingProxyConfigurator
    {
        public Func<object> Create { get; private set; }
        public ILog Logger { get; private set; }

        public LoggingProxyConfigurator OnException(Action pippo)
        {
            return this;
        }
        public LoggingProxyConfigurator CreateFunction(Func<object> create)
        {
            Create = create;
            return this;
        }

        public LoggingProxyConfigurator UsingLogger(ILog logger)
        {
            Logger = logger;
            return this;
        }
    }
}
