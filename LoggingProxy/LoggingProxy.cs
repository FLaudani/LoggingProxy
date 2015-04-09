using System;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;
using log4net;
using log4net.Config;

namespace LoggingProxy
{

    public class LoggingProxy<T> : RealProxy
    {
        private readonly ILog _log;
        private readonly T _decorated;

        internal LoggingProxy(T decorated, ILog logger = null)
            : base(typeof(T))
        {
            _decorated = decorated;
            _log = logger ?? Create();
        }

        public override IMessage Invoke(IMessage msg)
        {
            var methodCall = msg as IMethodCallMessage;
            var methodInfo = methodCall.MethodBase as MethodInfo;
            var args = "";

            methodCall.Args.ToList().ForEach((_) => args = string.Format("{0},{1}", args, _.ToString()));

            args = string.IsNullOrEmpty(args) ? args : args.Remove(0, 1);

            _log.Info(string.Format("Executing '{0}' with Args ({1})", methodCall.MethodName, args));
            try
            {
                var result = methodInfo.Invoke(_decorated, methodCall.InArgs);
                return new ReturnMessage(ProxyMe(result), null, 0, methodCall.LogicalCallContext, methodCall);
            }
            catch (Exception e)
            {
                _log.Error(string.Format("Exception {0} occurred executing '{1}'", e.InnerException.Message, methodCall.MethodName), e.InnerException);
                return new ReturnMessage(e.InnerException, methodCall);
            }
        }
        
        private static object ProxyMe(object result)
        {
            return result;
        }

        public static ILog Create()
        {
            var caller = Assembly.GetCallingAssembly().GetType();
            var log = LogManager.GetLogger(caller);
            XmlConfigurator.Configure();
            return log;
        }
        
    }
}
