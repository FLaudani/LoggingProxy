
using log4net;
using System;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;
using System.Linq;

namespace LoggingProxy
{


    public class LoggingProxy : RealProxy
    {
        private ILog _logger;
        private object _decorated;

        internal LoggingProxy(object decorated,ILog logger = null)
            : base(decorated.GetType())
        {
            _decorated = decorated;
            _logger = logger ?? LoggerFactory.Create();
        }

        public override IMessage Invoke(IMessage msg)
        {
            var methodCall = msg as IMethodCallMessage;
            var methodInfo = methodCall.MethodBase as MethodInfo;
            var args = "";

            methodCall.Args.ToList().ForEach((_) => args = string.Format("{0},{1}", args, _.ToString()));
            
            args = string.IsNullOrEmpty(args) ? args : args.Remove(0, 1);

            _logger.Info(string.Format("Executing '{0}' with Args ({1})", methodCall.MethodName,args));
            try
            {
                var result = methodInfo.Invoke(_decorated, methodCall.InArgs);
                return new ReturnMessage(ProxyMe(result), null, 0, methodCall.LogicalCallContext, methodCall);
            }
            catch (Exception e)
            {
                _logger.Error(string.Format("Exception {0} occurred executing '{1}'", e.InnerException.Message, methodCall.MethodName), e.InnerException);
                return new ReturnMessage(e.InnerException, methodCall);
            }
        }
        private object ProxyMe(object result)
        {
            if (result == null)
                return result;

            if (typeof(MarshalByRefObject).IsAssignableFrom(result.GetType()) || result.GetType().IsInterface)
                return new LoggingProxy(result,_logger).GetTransparentProxy();

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
