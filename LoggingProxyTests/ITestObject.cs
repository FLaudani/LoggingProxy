using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LoggingProxyTests
{
    interface ITestObject
    {
        void TestMethod();
        void ExceptionMethod();
        void TestMethod(int testItem);
    }
}
