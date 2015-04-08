using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;
using System.Text;
using System.Threading.Tasks;

namespace LoggingProxy
{


    public class LoggingProxy<T> : RealProxy
    {
        private ILog Log;
        private T _decorated;

        internal LoggingProxy(T decorated,ILog logger = null)
            : base(typeof(T))
        {
            _decorated = decorated;
            Log = logger ?? LoggerFactory.Create();
        }

        public override IMessage Invoke(IMessage msg)
        {
            var methodCall = msg as IMethodCallMessage;
            var methodInfo = methodCall.MethodBase as MethodInfo;
            var args = "";

            methodCall.Args.ToList().ForEach((_) => args = string.Format("{0},{1}", args, _.ToString()));
            
            args = string.IsNullOrEmpty(args) ? args : args.Remove(0, 1);

            Log.Info(string.Format("Executing '{0}' with Args ({1})", methodCall.MethodName,args));
            try
            {
                var result = methodInfo.Invoke(_decorated, methodCall.InArgs);
                return new ReturnMessage(ProxyMe(result), null, 0, methodCall.LogicalCallContext, methodCall);
            }
            catch (Exception e)
            {
                Log.Error(string.Format("Exception {0} occurred executing '{1}'", e.InnerException.Message, methodCall.MethodName), e.InnerException);
                return new ReturnMessage(e.InnerException, methodCall);
            }
        }
        private object ProxyMe(object result)
        {
            return result;
        }
        private class LoggerFactory
        {
            public static ILog Create()
            {
                var caller = Assembly.GetCallingAssembly().GetType();
                var Log = LogManager.GetLogger(caller);
                log4net.Config.XmlConfigurator.Configure();
                return Log;
            }
        }

    }
}
