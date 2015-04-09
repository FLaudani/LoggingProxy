using System;

namespace LoggingProxy
{
    public class LoggingProxyFactory
    {
        public static T Create<T> (Action<LoggingProxyConfigurator> config) where T : class 
        {
            var configurator = new LoggingProxyConfigurator();
            config(configurator);

            return new LoggingProxy<T>(configurator.Create() as T,configurator.Logger).GetTransparentProxy() as T;
        }
    }
}
