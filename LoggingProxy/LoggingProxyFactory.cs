using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoggingProxy
{
    public class LoggingProxyFactory
    {
        public static T Create<T> (Action<LoggingProxyCofngiurator> config) where T : class 
        {
            var configurator = new LoggingProxyCofngiurator();
            config(configurator);

            return new LoggingProxy(configurator.Create() as T,configurator.Logger).GetTransparentProxy() as T;
        }
    }
}
