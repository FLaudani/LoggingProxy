using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LoggingProxy
{
    public class LoggingProxyCofngiurator
    {
        public Func<object> Create { get; private set; }
        private ILog _logger;

        public ILog Logger { get { return _logger; } }

        public LoggingProxyCofngiurator OnException(Action pippo)
        {
            return this;
        }
        public LoggingProxyCofngiurator CreateFunction(Func<object> create)
        {
            Create = create;
            return this;
        }

        public LoggingProxyCofngiurator UsingLogger(ILog logger)
        {
            _logger = logger;
            return this;
        }
    }
}
